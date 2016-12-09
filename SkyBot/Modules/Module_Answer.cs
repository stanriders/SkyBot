// Skybot 2013-2016

using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SkyBot.Modules
{
    // Main answering module
    class Module_Answer : IModule
    {
        private SQLiteConnection connection;
        private string db_filename;

        public Module_Answer()
        {
            ID = ModuleList.Answer;
            UsableBy = APIList.All;

            Configurables.Add("dbpath");
            db_filename = Config.Read(ID.ToString(), "dbpath");

            connection = new SQLiteConnection("Data Source=" + db_filename + ";Version=3;");
        }

        public override string ProcessMessage(string msg)
        {
            // ignoring triggers
            if (msg.IndexOf("!", 0, 1) >= 0)
                return string.Empty;

            string result = string.Empty;

            msg.ToLower();

            // updating config
            if (Config.Read(ID.ToString(), "dbpath") != db_filename)
            {
                db_filename = Config.Read(ID.ToString(), "dbpath");
                connection = new SQLiteConnection("Data Source=" + db_filename + ";Version=3;");
            }

            if (connection.State != System.Data.ConnectionState.Closed)
                connection.Close();
            connection.Open();

            using (SQLiteCommand sql_cmd = new SQLiteCommand("SELECT * FROM 'words' WHERE (message='" + msg + "')", connection))
            using (SQLiteDataReader reader = sql_cmd.ExecuteReader())
            {
                List<string> answers = new List<string>();

                if (reader.Read())
                {
                    while (reader.GetString(1) != "")
                    {
                        answers.Add(reader.GetString(1));
                        if (!reader.Read())
                            break;
                    }

                    if (answers.Count <= 1)
                        result = answers[0];
                    else
                    {
                        Random rnd = new Random();
                        result = answers[rnd.Next(0, answers.Count)];
                    }
                }
            }
            connection.Close();

            return result;
        }
    }
}
