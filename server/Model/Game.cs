using System;
using System.Collections.Generic;

namespace server.model
{
    public class Game
    {
        public long id { get; set; }
        public string status { get; set; }
        public List<Player> players { get; set; }

        public Game(){
            Random random = new Random();
            this.id = random.Next();
        }

    }
}