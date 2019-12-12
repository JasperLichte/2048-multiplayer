using System;
using System.Collections.Generic;
using log4net;
using server.model.enums;

namespace server.model
{
    public class Game
    {
        public long id { get; set; }
        public long lastPlayerID {get;set;}=0;
        public Status status { get; set; }
        public List<Player> players { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EndDatum { get; set; }
        private static readonly ILog log = LogManager.GetLogger(typeof(Game));
        
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
                    log.Info($"Player with id {player.id} already registered");
                    return false;
                }
            players.Add(player);
            if (players.Count==1)
            {
              player.isAdmin=true;  
            }
            log.Info($"Player registered with id {player.id}!");
            lastPlayerID=player.id;
            return true;
        }

        internal void close()
        {
            this.status=Status.FINISHED;
            this.EndDatum=DateTime.Now;
        }
    }
}