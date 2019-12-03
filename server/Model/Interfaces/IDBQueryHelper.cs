namespace server.model.interfaces
{
    public interface IDBQueryHelper
    {
         void storeData(Game gameData);
         Game loadLastGameData();
         Player loadLastPlayerData();
    }
}