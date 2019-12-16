using System;
using System.Collections.Generic;
using server.model;
using server.model.interfaces;

namespace server
{
    class DBQueryDummy : IDBQueryHelper
    {
        public List<RestGame> loadGameData(int numberOfPlayer)
        {
            throw new NotImplementedException();
        }

        public List<RestScore> highestScore()
        {
             throw new NotImplementedException();
        }
        public List<RestPlayerCountGame> playerCountGame()
        {
            throw new NotImplementedException();
        }
        public Game loadLastGameData()
        {
            return new Game(0, DateTime.Now, DateTime.Now);
        }

        public Player loadLastPlayerData()
        {
            return new Player(0,"John Doe");
        }

        public void storeData(Game gameData)
        {
        }
    }
}