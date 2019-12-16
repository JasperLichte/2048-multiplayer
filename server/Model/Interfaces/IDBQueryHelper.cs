using System.Collections.Generic;

namespace server.model.interfaces
{
    public interface IDBQueryHelper
    {
         void storeData(Game gameData);
         Game loadLastGameData();
         Player loadLastPlayerData();
         List<RestGame> loadGameData(int numberOfPlayer);
         List<RestScore> highestScore();
         List<RestPlayerCountGame> playerCountGame();
    }
}