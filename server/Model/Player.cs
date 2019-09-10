using System.Collections.Generic;

namespace server.model
{
    class Player
    {
       public int id {get;set;}
        public List<Board> Boards {get;set;}
        private long score;
    }
}