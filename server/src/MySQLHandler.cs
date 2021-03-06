using System;
using System.Collections.Generic;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(MySQLHandler));
        public MySQLHandler()
        {
            this.config = Config.loadConfig();
            this.ConnectionString = config.connectionString;
        }
        public Game loadLastGameData()
        {
            List<Game> list = new List<Game>();
            using (var conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from game order by GameID DESC LIMIT 1", conn);
                // Try catch for the case there are no entries in the DB 
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            log.Debug(reader.GetInt32(0));
                            //add an entry to the list with the parameters of a Game() 
                            list.Add(new Game(reader.GetInt32(0), reader.GetDateTime(1), reader.GetDateTime(2)));
                        }
                    }
                }
                catch (System.Exception)
                {

                    list.Add(new Game(0, DateTime.Now, DateTime.Now));
                }

            }
            log.Debug(JsonConvert.SerializeObject(list));
            if (list.Count == 0)
            {
                return null;
            }
            return list[0];
        }
        public List<RestGame> loadGameData(int numberOfPlayer)
        {
            List<RestGame> list = new List<RestGame>();
            using (var conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from game order by GameID DESC LIMIT {numberOfPlayer}", conn);
                // Try catch for the case there are no entries in the DB 
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            log.Debug(reader.GetInt32(0));
                            //add an entry to the list with the parameters of a Game() 
                            list.Add(new RestGame(reader.GetInt32(0), reader.GetDateTime(1), reader.GetDateTime(2)));
                        }
                    }
                }
                catch (System.Exception)
                {
                    list.Add(new RestGame(0, DateTime.Now, DateTime.Now));
                }

            }
            log.Debug(JsonConvert.SerializeObject(list));
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }

        public List<RestScore> highestScore()
        {
            List<RestScore> list = new List<RestScore>();
            using (var conn = GetConnection())
            {
                conn.Open();
                MySqlCommand highestScore = new MySqlCommand("select game.GameID, spieler.Name, scores.Score , game.StartDatum, game.EndDatum "
                + "from game "
               + "join scores on scores.GameID = game.GameID "
               + " join spieler on spieler.PlayerID = scores.PlayerID "
               + " where scores.score=(select max(scores.Score) from scores)", conn);
                // Try catch for the case there are no entries in the DB 
                try
                {
                    using (var reader = highestScore.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            log.Debug(reader.GetInt32(0));
                            //add an entry to the list with the parameters of a Game() 
                            list.Add(new RestScore(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDateTime(3), reader.GetDateTime(4)));
                        }
                    }
                }
                catch (System.Exception)
                {
                    list.Add(new RestScore(0, "", 0, DateTime.Now, DateTime.Now));
                }

            }
            log.Debug(JsonConvert.SerializeObject(list));
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }


        public List<RestPlayerCountGame> playerCountGame()
        {
            List<RestPlayerCountGame> list = new List<RestPlayerCountGame>();
            using (var conn = GetConnection())
            {
                conn.Open();
                MySqlCommand playerCountGame = new MySqlCommand("select spieler.Name, Count(spieler.Name) as AnzahlSpiele "
                            + "from spieler "
                            + "group by spieler.Name "
                            + "order by AnzahlSpiele DESC LIMIT 1", conn);
                // Try catch for the case there are no entries in the DB 
                try
                {
                    using (var reader = playerCountGame.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //add an entry to the list with the parameters of a Game() 
                            list.Add(new RestPlayerCountGame(reader.GetString(0), reader.GetInt32(1)));
                        }
                    }
                }
                catch (System.Exception e)
                {
                    log.Debug(e);
                    list.Add(new RestPlayerCountGame("", 0));
                }

            }
            log.Debug(JsonConvert.SerializeObject(list));
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }
        public Player loadLastPlayerData()
        {
            List<Player> list = new List<Player>();
            using (var conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select *  from spieler order by PlayerID DESC LIMIT 1", conn);
                // Try catch for add an test entry for the database to get the next id 
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            log.Debug(reader.GetInt32(0));
                            list.Add(new Player(reader.GetInt32(0), reader.GetString(1)));
                        }
                    }
                }
                catch (MySqlException e)
                {
                    log.Debug(e);
                    list.Add(new Player(0, "Hallo"));

                }

            }
            log.Debug(JsonConvert.SerializeObject(list));
            if (list.Count == 0)
            {
                return null;
            }
            return list[0];
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public void storeData(Game game)
        {

            //Insert Select  für: UpdateGame, UpdatePlayer, UpdateScore
            using (var conn = GetConnection())
            {
                conn.Open();
                insertGameData(game.id, game.StartDatum, game.EndDatum, conn);
                game.players.ForEach(player =>
                {
                    insertPlayerData(player.id, player.name, conn);
                    scoreData(game.id, player.id, player.score, conn);
                });
            }
        }
        private void insertGameData(long gameID, DateTime startDatum, DateTime endDatum, MySqlConnection connection)
        {
            MySqlCommand insertGameData = connection.CreateCommand();
            //insert the current game with details as entry for  the database
            insertGameData.CommandText = "Insert into game (GameID, StartDatum, EndDatum)"
                                    + "Values(" + gameID + ",'" + startDatum.ToString("yyyy-MM-dd HH:mm:ss") + "','" + endDatum.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            insertGameData.ExecuteNonQuery();
        }
        private void insertPlayerData(long playerID, string name, MySqlConnection connection)
        {
            MySqlCommand insertPlayerData = new MySqlCommand("Insert into spieler (PlayerID, Name)"
                                    + "Values(" + playerID + ",'" + name + "')", connection);
            insertPlayerData.ExecuteNonQuery();
        }
        private void scoreData(long gameID, long playerID, long score, MySqlConnection connection)
        {
            MySqlCommand insertScoreData = new MySqlCommand("Insert into scores (GameID, PlayerID, Score)"
                                    + "Values(" + gameID + "," + playerID + ",'" + score + "')", connection);
            insertScoreData.ExecuteNonQuery();
        }

    }

    internal class ThisGame
    {
        public long GameID { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EndDatum { get; set; }

        public ThisGame(long gameID, DateTime StartDatum, DateTime EndDatum)
        {
            this.GameID = gameID;
            this.StartDatum = StartDatum;
            this.EndDatum = EndDatum;
        }
    }
}