using Newtonsoft.Json;
using server.websocket;

namespace server.model
{
    public class Response
    {
        //gameid,
        [JsonProperty]
        public ResponseTypes _type;
        [JsonProperty]
        public Game game {get;set;}
        public Response(ResponseTypes type, Game game )
        {
            this._type = type;
            this.game = game;

        }
    }
}