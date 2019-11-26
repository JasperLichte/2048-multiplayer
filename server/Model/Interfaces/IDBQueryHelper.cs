using System.Data.SqlClient;

namespace server.model.interfaces
{
    public interface IDBQueryHelper
    {
         void storageData();
         void loadData();
    }
}