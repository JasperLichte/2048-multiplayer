using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using server.model;

namespace server.websocket
{
    //Author: 
    //Author: Ricardo Kittmann
    class SocketHandler
    {
        WebSocket webSocket;
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
                    switch (command.Command)
                    {
                        default:
                            break;
                        case Commands.GAME_REGISTER:
                                await SendResponseJson(this.webSocket, new Response(ResponseTypes.REGISTER, new Game() ), ct);
                                Console.WriteLine("Send Game register response");
                            break;
                        case Commands.GAME_START:

                            await SendResponseJson(this.webSocket, new Response(ResponseTypes.GAME_START, new Game() ), ct);
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
            await SendResponseJson(this.webSocket, new Response(responseType, game ), ct);
        }
        public void cancelCTSource()
        {
            this.source.Cancel();
        }

        private Task SendResponseJson(WebSocket webSocket, Response response, CancellationToken ct)
        {
            string data = JsonConvert.SerializeObject(response);
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return webSocket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }
    }
}