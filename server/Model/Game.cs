using System.Collections.Generic;

namespace server.model
{
    class Game
    {
        public long id { get; set; }
        public string status { get; set; }
        public List<Player> players { get; set; }

    }
}