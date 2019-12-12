using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace server.websocket
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Commands
    {
        REGISTER=1,
        GAME_START=2,
        GET_UPDATE=3,
        DO_PLAYER_UPDATE=4,
        UNREGISTER=5,
        REGISTER_PLAYER=6
    }
}