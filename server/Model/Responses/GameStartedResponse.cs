using Newtonsoft.Json;
namespace server.model.responses
{
    class GameStartedResponse
    {
        [JsonProperty]
        public ResponseTypes type;
    }
}