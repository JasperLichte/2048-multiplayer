using Newtonsoft.Json;
using server.model.interfaces;

namespace server.model.responses
{
    class GameClosedResponse : IResponse
    {
        [JsonProperty]
        public ResponseTypes type = ResponseTypes.GAME_CLOSED;

    }
}