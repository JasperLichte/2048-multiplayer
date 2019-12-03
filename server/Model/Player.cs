using System;

namespace server.model
{
    public class Player
    {
       public long id {get;set;}
        public Board board {get;set;}
        public long score {get;set;}
        public String name{get;set;}
        public Boolean isAdmin{get;set;}=false;

        internal Player(Config config, long id){

            this.id = id;
            this.board = new Board().fill(config);
        }
        public Player(long playerID, string name)
        {
            this.id= playerID;
            this.name = name;
        }
        
    }
}