// Skybot 2013-2017

using System.Data.SQLite;
using System.IO;
using System.Text;

namespace SkyBot
{
    class SQLBase
    {
        protected StringBuilder _sqlBuilder;
        protected SQLiteConnection m_dbConnection;
        protected string _dbPath;

        private int index;
        protected string WhereOrAnd()
        {
            index++;
            if (index == 0) return "WHERE";
            else return "AND";
        }

        protected void СonnectToDatabase()
        {
            index = -1;

            _sqlBuilder?.Clear();

            if (m_dbConnection != null) return;

            m_dbConnection = new SQLiteConnection("Data Source=" + Directory.GetCurrentDirectory() + _dbPath + ";Version=3;");
            m_dbConnection.Open();
        }

        protected void DisconnectFromDatabase()
        {
            m_dbConnection?.Close();
            m_dbConnection = null;
        }
    }
}
