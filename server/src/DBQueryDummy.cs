using System;
using server.model;
using server.model.interfaces;

namespace server
{
    class DBQueryDummy : IDBQueryHelper
    {
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