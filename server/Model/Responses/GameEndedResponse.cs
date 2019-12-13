using System.Collections.Generic;
using Newtonsoft.Json;
using server.model.enums;
using server.model.interfaces;

namespace server.model.responses
{
    class GameEndedResponse : IResponse
    {
        [JsonProperty]
        public ResponseTypes type = ResponseTypes.GAME_ENDED;
        [JsonProperty]
        public long remainingTime =0;
        [JsonProperty]
        public List<Player> players;
        [JsonProperty]
        public Status gameStatus = Status.FINISHED;

        public GameEndedResponse(List<Player> players){
            this.players = players;
        }

    }
}