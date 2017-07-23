// Skybot 2013-2017

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SkyBot
{
    class SQLHandler : SQLBase
    {
        public enum Mode
        {
            NA,
            NOLIKE,
            RANDOM,
            LIKE,
        }

        public SQLHandler(string dbPath)
        {
            _sqlBuilder = new StringBuilder();
            _dbPath = dbPath;
        }

        public string GetString(string tableName, string returnField, List<string> filerList = null)
        {
            try
            {
                СonnectToDatabase();

                _sqlBuilder.Append($"SELECT * FROM '{ tableName }'");

                if (filerList != null)
                    foreach (var filter in filerList)
                    {
                        _sqlBuilder.Append($" { WhereOrAnd() } { filter }");
                    }

                SQLiteCommand command = new SQLiteCommand(_sqlBuilder.ToString(), m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                reader.Read();

                return reader[returnField].ToString();
            }
            catch (Exception ex)
            {
                InformationCollector.Error(tableName, ex.Message);
                return string.Empty;
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        public List<string> GetList(string tableName, string returnField, List<string> filerList = null)
        {
            try
            {
                var list = new List<string>();

                СonnectToDatabase();

                _sqlBuilder.Append($"SELECT * FROM '{ tableName }'");

                if (filerList != null)
                    foreach (var filter in filerList)
                    {
                        _sqlBuilder.Append($" { WhereOrAnd() } { filter }");
                    }

                SQLiteCommand command = new SQLiteCommand(_sqlBuilder.ToString(), m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader[returnField] != null)
                        list.Add(reader[returnField].ToString());
                }

                return list;
            }
            catch (Exception ex)
            {
                InformationCollector.Error(tableName, ex.Message);
                return new List<string>();
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        public List<T> GetList<T>(string tableName) where T : new()
        {
            try
            {
                var list = new List<T>();
                var dict = new Dictionary<string, object>();
                var fields = Converters.GetProperties<T>();

                СonnectToDatabase();

                _sqlBuilder.Append($"SELECT * FROM '{ tableName }'");

                SQLiteCommand command = new SQLiteCommand(_sqlBuilder.ToString(), m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dict.Clear();
                    foreach (var field in fields)
                    {
                        var _data = reader[field];

                        if (_data.GetType() == typeof(DBNull))
                            continue;

                        if (_data.GetType() == typeof(long))
                            _data = Convert.ToInt32(_data);

                        dict.Add(field, _data);
                    }
                    T data = new T();
                    list.Add(data);
                    Converters.SetProperties(data, dict);
                }

                return list;
            }
            catch (Exception ex)
            {
                InformationCollector.Error(tableName, ex.Message);
                return new List<T>();
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        public string GetBoltunData(Mode mode, string str, string table, string like = null)
        {
            try
            {
                СonnectToDatabase();

                switch (mode)
                {
                    case Mode.NOLIKE:
                        _sqlBuilder.Append("SELECT * FROM '" + table + "' WHERE q='" + str + "'");
                        break;

                    case Mode.RANDOM:
                        _sqlBuilder.Append("SELECT * FROM '" + table + "' ORDER BY RANDOM() LIMIT 1");
                        break;

                    case Mode.LIKE:
                        _sqlBuilder.Append("SELECT * FROM '" + table + "' WHERE 'q' LIKE '%" + like + "%'");
                        break;

                    default:
                        break;
                }

                InformationCollector.Log("ConnectionToSQL\nstr: " + str + "\ntable: " + table + "\nlike: " + like);

                SQLiteCommand command = new SQLiteCommand(_sqlBuilder.ToString(), m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                List<string> answers = new List<string>();
                int i = 0;

                reader.Read();

                InformationCollector.Log("ConnectionToSQL\nreader.GetString(1): " + reader.GetString(1));

                while (reader.GetString(1) != string.Empty)
                {
                    answers.Add(reader.GetString(1));
                    i++;
                    reader.Read();
                }

                if (i == 1) return answers[0];
                else if (i > 1) return answers[RNG.Next(0, i)];
                else return string.Empty;
            }
            catch (Exception ex)
            {
                InformationCollector.Error(table, ex.Message);
                return string.Empty;
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        public void SetString(string tableName, string key, string value)
        {
            try
            {
                СonnectToDatabase();

                _sqlBuilder.Append($"INSERT INTO '{ tableName }' ({ key }) VALUES ('{ value }')");

                SQLiteCommand command = new SQLiteCommand(_sqlBuilder.ToString(), m_dbConnection);
                command.ExecuteNonQuery();

                DisconnectFromDatabase();
            }
            catch (Exception ex)
            {
                InformationCollector.Error(tableName, ex.Message);
            }
        }

        public void CreateTable(string tableName, List<string> fields)
        {
            try
            {
                СonnectToDatabase();

                var tableNames = string.Empty;
                foreach (var field in fields)
                {
                    tableNames += field;
                    if (field != fields.Last())
                        tableNames += ", ";
                }

                _sqlBuilder.Append($"CREATE TABLE '{ tableName }' ({ tableNames })");

                SQLiteCommand command = new SQLiteCommand(_sqlBuilder.ToString(), m_dbConnection);
                command.ExecuteNonQuery();

                DisconnectFromDatabase();
            }
            catch (Exception ex)
            {
                InformationCollector.Error(tableName, ex.Message);
            }
        }

        public List<string> GetTables()
        {
            try
            {
                СonnectToDatabase();

                DataTable schema = m_dbConnection.GetSchema("Tables");

                DisconnectFromDatabase();

                List<string> tableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    tableNames.Add(row[2].ToString());
                }

                return tableNames;
            }
            catch (Exception ex)
            {
                InformationCollector.Error(string.Empty, ex.Message);
                return new List<string>();
            }
        }
    }
}
