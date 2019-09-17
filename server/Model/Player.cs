using System;
using System.Collections.Generic;

namespace server.model
{
    public class Player
    {
       public long id {get;set;}
        public List<Board> Boards {get;set;}
        private long score;
        public Boolean isAdmin{get;set;}=false;

        public Player(){
            Random random = new System.Random();
            this.id = DateTime.Now.Ticks / (long)TimeSpan.TicksPerMillisecond;
        }
    }
}