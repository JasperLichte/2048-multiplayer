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
        internal async Task socketHandle(HttpContext context, WebSocket websocket)
        {
            this.webSocket = websocket;
            source = new CancellationTokenSource();
            ct = source.Token;
            Config config = Config.loadConfig();
            while (!source.IsCancellationRequested)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                var command = await ReceiveJSOnAsync(this.webSocket, ct);
                log.Debug($"Got Command {JsonConvert.SerializeObject(command)}");
                if (this.webSocket.State.ToString() == "Open")
                {
                    switch (command.type)
                    {
                        default:
                            break;
                        case Commands.REGISTER:
                            await SendResponseJson(this.webSocket, gameHandler.registerNewPlayer(), ct);
                            log.Debug("Send Game register response");
                            broadcastHandler.addWebSocket(this.webSocket);
                            await SendResponseJson(this.webSocket, gameHandler.getUpdate(), ct);
                            log.Debug("Send Update response");
                            broadcastHandler.sendResponseToAll(gameHandler.getAllPlayers());
                            log.Debug("Send Player registered Response to everyone");
                            break;
                        case Commands.REGISTER_PLAYER:
                            gameHandler.registerPlayerName(command.playerID,command.name);
                            log.Debug("Registered Playername");
                            broadcastHandler.sendResponseToAll(gameHandler.getAllPlayers());
                            log.Debug("Send Player registered Response to everyone");
                            break;
                        case Commands.GAME_START:
                            if (gameHandler.startGame(command.playerID))
                            {
                                broadcastHandler.sendResponseToAll(new GameStartedResponse(ResponseTypes.GAME_STARTED, gameHandler.game));
                                broadcastHandler.sendResponseToAll(gameHandler.getUpdate());
                            }
                            log.Debug("Game started");
                            break;
                        case Commands.GET_UPDATE:
                            await SendResponseJson(this.webSocket, gameHandler.getUpdate(), ct);
                            log.Debug("Send Get_Update response");
                            break;
                        case Commands.DO_PLAYER_UPDATE:
                            gameHandler.updatePlayer(command.playerID, command.newScore, command.board);
                            log.Debug("Player Update done");
                            break;
                        case Commands.UNREGISTER:
                            gameHandler.unregisterPlayer(command.playerID);
                            log.Debug("Removed Player");
                            break;
                    }
                }
                else
                {
                    source.Cancel();
                }
            }
        }
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

        private Task SendResponseJson(WebSocket webSocket, IResponse response, CancellationToken ct)
        {
            string data = JsonConvert.SerializeObject(response);
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return webSocket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

    }
}