using System;
using log4net;
using server.model.enums;

namespace server.model
{
    public class RestScore
    {
        //GameID
        public long gameID { get; set; }
        //Name
        public string name{get; set; }
        //Score
        public long score{ get; set;}
        //StartDatum
        public DateTime StartDatum { get; set; }
        public DateTime EndDatum { get; set; }
       
                
        //Constructor, if the Method has the same Name like the class
        public RestScore(int id, string name, int score, DateTime StartDatum, DateTime EndDatum)
        {
            this.gameID = id;
            this.name = name;
            this.score = score;
            this.StartDatum = StartDatum;
            this.EndDatum = EndDatum;
        }

        internal void close()
        {
            this.EndDatum=DateTime.Now;
        }
    }
}