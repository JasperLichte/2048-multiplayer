using Newtonsoft.Json;
using server.model.interfaces;

namespace server.model.responses
{
    class GameEndedResponse : IResponse
    {
        [JsonProperty]
        public ResponseTypes type = ResponseTypes.GAME_ENDED;

    }
}