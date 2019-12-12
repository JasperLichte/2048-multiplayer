using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using server.model.interfaces;

namespace server.model.responses
{
    class NewPlayerResponse:IResponse
    {
        [JsonProperty]
        public ResponseTypes type;

        [JsonProperty]
        public List<Player> players;

        public NewPlayerResponse(List<Player> players){
            this.players=players;
            this.type = ResponseTypes.NEW_PLAYER_REGISTERED;
        }

    }
}