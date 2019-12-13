using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using server.model;
using server.model.interfaces;
using server.model.responses;

namespace server.websocket
{
    //Author: Ricardo Kittmann

    class SocketHandler
    {
        WebSocket webSocket;
        GameHandler gameHandler = GameHandler.getHandler();
        BroadcastHandler broadcastHandler = BroadcastHandler.getBroadcastHandler();
        CancellationTokenSource source;
        CancellationToken ct;
        private static readonly ILog log = LogManager.GetLogger(typeof(SocketHandler));

        //loops until the CTSource is canceled or the websocket is closed
        internal async Task socketHandle(HttpContext context, WebSocket websocket)
        {
            this.webSocket = websocket;
            source = new CancellationTokenSource();
            ct = source.Token;
            Config config = Config.loadConfig();
            IResponse check;
            while (!source.IsCancellationRequested)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                //wait until another message is send via the websocket
                //expected json = WebSocketRequest.cs object
                var request = await ReceiveJSOnAsync(this.webSocket, ct);
                log.Debug($"Got Command {JsonConvert.SerializeObject(request)}");
                if (this.webSocket.State.ToString() == "Open")
                {
                    //perform different actions, depending on the 'type' parameter inside the send WebSocketRequest object
                    switch (request.type)
                    {
                        default:
                            break;
                        case Commands.REGISTER:
                            //create a new player/game in the gamehandler
                            await SendResponseJson(this.webSocket, gameHandler.registerNewPlayer(), ct);
                            log.Debug("Send Game register response");
                            //register this websocket to the broadcasthandler
                            broadcastHandler.addWebSocket(this.webSocket);
                            await SendResponseJson(this.webSocket, gameHandler.getUpdate(), ct);
                            log.Debug("Send Update response");
                            //inform all players that a new player registered
                            broadcastHandler.sendResponseToAll(gameHandler.getAllPlayers());
                            log.Debug("Send Player registered Response to everyone");
                            break;
                        case Commands.REGISTER_PLAYER:
                            //update the players name, only players with a name are allowed to play
                            gameHandler.registerPlayerName(request.playerID, request.name);
                            log.Debug("Registered Playername");
                            //inform all players
                            broadcastHandler.sendResponseToAll(gameHandler.getAllPlayers());
                            log.Debug("Send Player registered Response to everyone");
                            break;
                        case Commands.GAME_START:
                            if (gameHandler.startGame(request.playerID))
                            {
                                //inform all players that the game started
                                broadcastHandler.sendResponseToAll(new GameStartedResponse(ResponseTypes.GAME_STARTED, gameHandler.game));
                                broadcastHandler.sendResponseToAll(gameHandler.getUpdate());
                            }
                            log.Debug("Game started");
                            break;
                        case Commands.GET_UPDATE:
                            //called by the clients to get an update about other players
                            //only registered players get an update, if the players id isn't registered
                            //this websocket connection gets closed after an ErrorResponse is send to the client
                            check = gameHandler.checkAuthorization(request.playerID);
                            if (check == null)
                            {
                                await SendResponseJson(this.webSocket, gameHandler.getUpdate(), ct);
                                log.Debug("Send Get_Update response");
                            }
                            else
                            {
                                await SendResponseJson(this.webSocket, check, ct);
                                broadcastHandler.removeWebSocket(this.webSocket);
                            }
                            break;
                        case Commands.DO_PLAYER_UPDATE:
                            //called by the clients to update his board
                            //only registered players can update their board, if the players id isn't registered
                            //this websocket connection gets closed after an ErrorResponse is send to the client
                            check = gameHandler.checkAuthorization(request.playerID);
                            if (check == null)
                            {
                                gameHandler.updatePlayer(request.playerID, request.newScore, request.board);
                                log.Debug("Player Update done");
                            }
                            else
                            {
                                await SendResponseJson(this.webSocket, check, ct);
                                broadcastHandler.removeWebSocket(this.webSocket);
                            }
                            break;
                        case Commands.UNREGISTER:
                            gameHandler.unregisterPlayer(request.playerID);
                            broadcastHandler.removeWebSocket(this.webSocket);
                            log.Debug("Removed Player");
                            broadcastHandler.sendResponseToAll(gameHandler.getAllPlayers());
                            log.Debug("Send registered Response to everyone after unregistering a person");
                            break;
                    }
                }
                //if websocket state != open -> cancel the source and close the sockethandler
                else
                {
                    source.Cancel();
                }
            }
        }
        //some voodoo; listen to the websocket and convert the incoming byte stream to a WebSocketRequest object
        private static async Task<WebSocketRequest> ReceiveJSOnAsync(WebSocket webSocket, CancellationToken ct)
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();
                    //waiting for a message from the UI
                    result = await webSocket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);
                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }
                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    String temp = await reader.ReadToEndAsync();
                    log.Debug($"Received json string: {temp}");
                    //Deserialize JSON string to a 'WebsocketRequest' object
                    return JsonConvert.DeserializeObject<WebSocketRequest>(temp);
                }
            }
        }

        //send an object-json implementing the IResponse interface over the websocket connection
        private Task SendResponseJson(WebSocket webSocket, IResponse response, CancellationToken ct)
        {
            string data = JsonConvert.SerializeObject(response);
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return webSocket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

    }
}