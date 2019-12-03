using Newtonsoft.Json;
using server.model.interfaces;

namespace server.model.responses
{
    public class GameStartedResponse:IResponse
    {
        //gameid,
        [JsonProperty]
        public ResponseTypes type;
        [JsonProperty]
        public Game game {get;set;}
        public GameStartedResponse(ResponseTypes type, Game game )
        {
            this.type = type;
            this.game = game;

        }
    }
}