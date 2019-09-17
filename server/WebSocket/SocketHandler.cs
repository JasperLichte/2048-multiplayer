using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using server.model;
using server.model.interfaces;
using server.model.responses;

namespace server.websocket
{
    //Author: 
    //Author: Ricardo Kittmann

    //notes:
    //timer beendet round end message
    class SocketHandler
    {
        WebSocket webSocket;
        List<WebSocket> webSocketList = new List<WebSocket>();
        GameHandler gameHandler =  GameHandler.getHandler();
        CancellationTokenSource source;
        CancellationToken ct;
        internal async Task socketHandle(HttpContext context, WebSocket websocket)
        {
            this.webSocket = websocket;
            /* var callerIp = context.Connection.RemoteIpAddress; */
            source = new CancellationTokenSource();
            ct = source.Token;
            while (!source.IsCancellationRequested)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                var command = await ReceiveJSOnAsync(this.webSocket, ct);
                Console.WriteLine(JsonConvert.SerializeObject(command));
                if (this.webSocket.State.ToString() == "Open")
                {
                    switch (command.type)
                    {
                        default:
                            break;
                        case Commands.REGISTER:
                        //direkt danach Update schicken mit allen spielern
                        
                                await SendResponseJson(this.webSocket, gameHandler.registerNewPlayer(), ct);
                                Console.WriteLine("Send Game register response");
                                await SendResponseJson(this.webSocket, gameHandler.getUpdate(), ct);
                                Console.WriteLine("Send Update response");
                            break;
                        case Commands.ROUND_START:
                        //ersteller des games kann nur starten.
                            await SendResponseJson(this.webSocket, new GameResponse(ResponseTypes.GAME_STARTED, new Game() ), ct);
                            break;
                        case Commands.GET_UPDATE:
                        //übriger timer
                        //nur scores vom spieler keine Felder
                        await SendResponseJson(this.webSocket, gameHandler.getUpdate(), ct);
                        break;
                        case Commands.DO_PLAYER_UPDATE:
                            //keine Antwort
                            //update den spieler
                        break;
                        case Commands.GET_PLAYER_BOARD:
                        //send board of playerid
                        break;
                        case Commands.UNREGISTER:
                        //unregistered oder game ended wenn admin leaved
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
                    //Deserialize JSON string to a 'Commands' object
                    return JsonConvert.DeserializeObject<WebSocketRequest>(temp);
                }
            }
        }
        public async void sendMessage(ResponseTypes responseType, Game game)
        {
            CancellationToken ct = source.Token;
            await SendResponseJson(this.webSocket, new GameResponse(responseType, game ), ct);
        }
        public void cancelCTSource()
        {
            this.source.Cancel();
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