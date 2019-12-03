using System;

namespace server.model
{
    public class Player
    {
       public long id {get;set;}
        public Board Board {get;set;}
        public long score {get;set;}
        public String name{get;set;}
        public Boolean isAdmin{get;set;}=false;

        public Player(long id){

            this.id = id;
            this.Board = new Board();
        }
        public Player(long playerID, string name)
        {
            this.id= playerID;
            this.name = name;
        }
        
    }
}