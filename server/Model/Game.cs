using System;
using System.Collections.Generic;
using server.model.enums;

namespace server.model
{
    public class Game
    {
        public long id { get; set; }
        public Status status { get; set; }
        public List<Player> players { get; set; }

        public Game(){
            Random random = new Random();
            this.id = random.Next();
        }

        public bool enterInGame()
        {
           if(status==Status.created)
           {
               return true;
           }
            return false;
        }
    }
}