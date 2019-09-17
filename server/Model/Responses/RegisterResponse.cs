using System;
using Newtonsoft.Json;
using server.model.interfaces;

namespace server.model.responses
{
    class RegisterResponse:IResponse
    {
        [JsonProperty]
        public ResponseTypes type;
        [JsonProperty]
        public Boolean isAdmin;
        [JsonProperty]
        public long gameID;
        [JsonProperty]
        public long localPlayerID;

        public RegisterResponse(Player player, long gameID){
            this.type=ResponseTypes.REGISTERED;
            this.isAdmin=player.isAdmin;
            this.gameID=gameID;
            this.localPlayerID=player.id;
        }

    }
}