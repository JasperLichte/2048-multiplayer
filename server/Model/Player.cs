using System.Collections.Generic;

namespace server.model
{
    public class Player
    {
       public int id {get;set;}
        public List<Board> Boards {get;set;}
        private long score;
    }
}