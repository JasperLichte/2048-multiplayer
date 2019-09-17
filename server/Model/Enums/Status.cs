using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace server.model.enums
{
  [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
      CREATED = 0,
      RUNNING = 1,
      FINISHED=2,  
    } 
}