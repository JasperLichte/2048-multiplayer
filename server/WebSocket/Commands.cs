using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace server.websocket
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Commands
    {
        GAME_REGISTER=1,
        GAME_START=2,
        UPDATE=3,
        USER_REGISTER=4
    }
}