using Newtonsoft.Json;
using System.Collections.Generic;

namespace server.model.responses
{
    class UpdateResponse:IResponse
    {
        [JsonProperty]
        public ResponseTypes type;
        [JsonProperty]
        public long remainingTime;
        public List<Player> players;

        public UpdateResponse(List<Player> players,long remainingTime){
            this.type=ResponseTypes.UPDATE;
            this.players=players;
            this.remainingTime=remainingTime;
        }
    }
}