using server.model;

namespace server.websocket
{
    class WebSocketRequest
    {
        public Commands type { get; set; }
        public long playerID{get;set;}

        public long newScore{get;set;}
        public Board board{get;set;}
        public string name{get;set;}
    }
}