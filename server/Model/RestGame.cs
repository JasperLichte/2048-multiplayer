using System;

namespace server.model
{
    public class RestGame
    {
        public long id { get; set; }       
        public DateTime StartDatum { get; set; }
        public DateTime EndDatum { get; set; }
        //private static readonly ILog log = LogManager.GetLogger(typeof(Game));
        
       //Constructor, if the Method has the same Name like the class
        public RestGame(int id, DateTime StartDatum, DateTime EndDatum)
        {
            this.id = id;
            this.StartDatum = StartDatum;
            this.EndDatum = EndDatum;
        }

        internal void close()
        {
            this.EndDatum=DateTime.Now;
        }
    }
}