using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace server.model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResponseTypes
    {
        UPDATE = 0,
        REGISTERED = 1,
        UNREGISTER = 2,
        GAME_STARTED = 3,
        GAME_ENDED=4,
        ROUND_STARTED=5,
        ROUND_ENDED=6,
        SCORE_UPDATE=7,
        ERROR=8
    }
}