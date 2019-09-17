using Newtonsoft.Json;
using server.model.enums;
using server.model.interfaces;
using System.Collections.Generic;

namespace server.model.responses
{
    class UpdateResponse:IResponse
    {
        [JsonProperty]
        public ResponseTypes type;
        [JsonProperty]
        public long remainingTime;
        [JsonProperty]
        public List<Player> players;
        [JsonProperty]
        public Status gameStatus;


        public UpdateResponse(List<Player> players,long remainingTime , Status gameStatus){
            this.type=ResponseTypes.UPDATE;
            this.players=players;
            this.remainingTime=remainingTime;
            this.gameStatus = gameStatus;
        }
    }
}