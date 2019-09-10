using server.model;

namespace server.websocket
{
    class WebSocketRequest
    {
        public Commands Command { get; set; }
        public Player player{get;set;}
        
    }
}