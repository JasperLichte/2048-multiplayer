using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace server.websocket
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Commands
    {
        REGISTER=1,
        ROUND_START=2,
        GET_UPDATE=3,
        DO_PLAYER_UPDATE=4,
        GET_PLAYER_BOARD=5,
        UNREGISTER=6
    }
}