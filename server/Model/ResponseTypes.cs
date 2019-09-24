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
        SCORE_UPDATE=5,
        ERROR=6,
        PLAYER_BOARD=7,
        GAME_CLOSED=8
    }
}