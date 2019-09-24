using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using server.model.interfaces;
using server.model.responses;

namespace server
{
    class BroadcastHandler
    {
        public List<WebSocket> websockets;
        public static BroadcastHandler broadcastHandler = new BroadcastHandler();
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken ct;
        GameHandler gameHandler;
        private BroadcastHandler()
        {
            this.websockets = new List<WebSocket>();
            this.gameHandler = GameHandler.getHandler();
            ct = source.Token;
            gameHandler.TimerElapsed += timerElapsed;
            Console.WriteLine("Registered to timeelapsed event");
        }

        private void timerElapsed(object sender, EventArgs e)
        {
            Console.WriteLine("Called timeelapsed event");
            sendResponseToAll(new GameEndedResponse(),ct);
        }

        public static BroadcastHandler getBroadcastHandler()
        {
            return broadcastHandler;
        }

        public void addWebSocket(WebSocket webSocket)
        {
            websockets.Add(webSocket);
        }
        public void removeWebSocket(WebSocket webSocket)
        {
            websockets.Remove(webSocket);
        }
        public void sendResponseToAll(IResponse response, CancellationToken ct)
        {
            if (websockets.Count > 0)
            {
                websockets.ForEach(websocket =>
                {
                    Task.Run(() =>
                    {
                        string data = JsonConvert.SerializeObject(response);
                        var buffer = Encoding.UTF8.GetBytes(data);
                        var segment = new ArraySegment<byte>(buffer);
                        websocket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
                    });
                });
            }
        }
    }
}