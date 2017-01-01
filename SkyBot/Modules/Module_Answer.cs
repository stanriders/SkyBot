// Skybot 2013-2017

using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SkyBot.Modules
{
    // Main answering module
    class Module_Answer : IModule
    {
        private SQLiteConnection connection;
        private Configurable db_filename;

        public Module_Answer()
        {
            ID = ModuleList.Answer;
            UsableBy = APIList.All;

            db_filename = new Configurable()
            {
                Name = "dbpath",
                Parent = this
            };
            Configurables.Add(db_filename);

            connection = new SQLiteConnection("Data Source=" + db_filename.Value + ";Version=3;");
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            // ignoring triggers
            if (msg.Text.IndexOf("!", 0, 1) >= 0)
                return string.Empty;

            string result = string.Empty;

            string message = msg.Text.ToLower();

            try
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
                connection.Open();

                using (SQLiteCommand sql_cmd = new SQLiteCommand("SELECT * FROM 'words' WHERE (message='" + message + "')", connection))
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
                            result = answers[RNG.Next(0, answers.Count)];
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception e)
            {
                InformationCollector.Error(this, e.Message);
            }

            return result;
        }
    }
}
