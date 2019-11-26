using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using server.model;
using server.model.interfaces;

namespace server
{
    class MySQLHandler : IDBQueryHelper
    {
        public String ConnectionString;
        private Config config;
        public MySQLHandler()
        {
            this.config = Config.loadConfig();
            this.ConnectionString = config.connectionString;
        }
        public void loadData()
        {
            List<ThisGame> list = new List<ThisGame>();
            using (var conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from game order by GameID DESC", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetInt32(0));
                        list.Add(new ThisGame(reader.GetInt32(0), reader.GetDateTime(1),reader.GetDateTime(2))                        );
                    }
                }
            }
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public void storageData()
        {



        }







    }

    internal class ThisGame
    {
        public long GameID { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EndDatum { get; set; }

        public ThisGame(long gameid, DateTime StartDatum, DateTime EndDatum)
        {
            this.GameID=gameid;
            this.StartDatum=StartDatum;
            this.EndDatum=EndDatum;
        }
    }
}