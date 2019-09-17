using Newtonsoft.Json;
using server.model.interfaces;
using server.websocket;

namespace server.model.responses
{
    public class GameResponse:IResponse
    {
        //gameid,
        [JsonProperty]
        public ResponseTypes type;
        [JsonProperty]
        public Game game {get;set;}
        public GameResponse(ResponseTypes type, Game game )
        {
            this.type = type;
            this.game = game;

        }
    }
}