using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using server.model.interfaces;
using server.model.responses;

namespace server
{
    //Singleton/only 1 object exists at runtime
    //object can be accessed via the getInstance() method
    class BroadcastHandler
    {
        private List<WebSocket> websockets;
        private static BroadcastHandler broadcastHandler = new BroadcastHandler();
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken ct;
        GameHandler gameHandler;
        private static readonly ILog log = LogManager.GetLogger(typeof(BroadcastHandler));
        private BroadcastHandler()
        {
            this.websockets = new List<WebSocket>();
            this.gameHandler = GameHandler.getHandler();
            ct = source.Token;
            gameHandler.TimerElapsed += timerElapsed;
        }

        private void timerElapsed(object sender, EventArgs e)
        {
            log.Debug("Broadcasthandler called timeelapsed event");
            sendResponseToAll(new GameEndedResponse(gameHandler.game.players));
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
        //schedules 1 thread for each websocket to send a message to
        public void sendResponseToAll(IResponse response)
        {

            if (websockets.Count > 0)
            {

                log.Debug($"Sending {response.GetType().ToString()} to {websockets.Count} websockets");
                //lock the websocket list so no other thread can modify the list while a message is being send
                lock (websockets)
                {
                    lock (gameHandler.game.players)
                    {
                        lock (response)
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
        }
        public void removeAllWebsockets()
        {
            websockets.ForEach(websocket =>
            {
                websocket.Abort();
            });
            this.websockets = new List<WebSocket>();
        }
    }
}