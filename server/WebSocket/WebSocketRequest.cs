using server.model;

namespace server.websocket
{
    class WebSocketRequest
    {
        public Commands type { get; set; }
        public Player player{get;set;}
        
    }
}