using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace server.model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResponseTypes
    {
        UPDATE = 0,
        REGISTER = 1,
        UNREGISTER = 2,
        GAME_START = 3,
        GAME_END=4,
        ROUND_START=5,
        ROUND_END=6,
        SCORE_UPDATE=7
    }
}