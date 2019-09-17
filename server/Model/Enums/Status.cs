using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace server.model.enums
{
  [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
      created = 0,
      running = 1,
      finished=2,  
    } 
}