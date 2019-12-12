using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace server.model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResponseTypes
    {
        UPDATE = 0,
        REGISTERED = 1,
        GAME_STARTED = 2,
        GAME_ENDED=3,
        ERROR=4,
        NEW_PLAYER_REGISTERED=5,
        GAME_CLOSED=6
    }
}