using System;
using System.Collections.Generic;

namespace server.model
{
    public class Player
    {
       public long id {get;set;}
        public Board Board {get;set;}
        public long score {get;set;}
        public String name{get;set;}
        public Boolean isAdmin{get;set;}=false;

        public Player(){
            Random random = new System.Random();
            this.id = DateTime.Now.Ticks / (long)TimeSpan.TicksPerMillisecond;
        }
    }
}