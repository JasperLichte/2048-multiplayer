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
        public DateTime StartDatum { get; set; }
        public DateTime EndDatum { get; set; }

        public Game(long gameID){
            Random random = new Random();
            this.id = gameID;
            this.players = new List<Player>();
            this.status= Status.CREATED;
            this.StartDatum = DateTime.Now;
        }

        public Game(int id, DateTime StartDatum, DateTime EndDatum)
        {
            this.id = id;
            this.StartDatum = StartDatum;
            this.EndDatum = EndDatum;
        }

        public bool allowedToRegister()
        {
           if(status==Status.CREATED)
           {
               return true;
           }
            return false;
        }

            public Boolean registerPlayer(Player player){
                if (players.Find( x => x.id == player.id)!=null)
                {
                    Console.WriteLine($"Player with id {player.id} already registered");
                    return false;
                }
            players.Add(player);
            if (players.Count==1)
            {
              player.isAdmin=true;  
            }
            Console.WriteLine($"Player with id {player.id} registered!");
            return true;
        }

        internal void close()
        {
            this.status=Status.FINISHED;
            this.EndDatum=DateTime.Now;
        }
    }
}