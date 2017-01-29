// Skybot 2013-2017

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace SkyBot.Modules
{
    // Main answering module
    class Module_Answer : IModule
    {
        private SQLiteConnection connection;
        public Configurable db_filename;
        private Module_Answer_AddServer server;

        public Module_Answer() : base()
        {
            ID = ModuleList.Answer;
            UsableBy = APIList.All;

            connection = new SQLiteConnection("Data Source=" + db_filename.Value + ";Version=3;");

            server = new Module_Answer_AddServer(this);
        }

        public override void Cleanup()
        {
            server.Cleanup();
        }

        public override void SetupConfig()
        {
            db_filename = new Configurable()
            {
                Name = "dbpath",
                ReadableName = "Path to Database",
                Parent = this
            };
            Configurables.Add(db_filename);
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            // ignoring triggers
            if (msg.Text.IndexOf(GetTrigger(msg.API), 0, 1) >= 0)
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
                InformationCollector.Error(e.Source, e.Message);
            }

            return result;
        }
    }

    // 
    // Web-server for adding new entries to the answering database
    // Adds new entries via POST with params 'word' and 'answer'
    //
    class Module_Answer_AddServer
    {
        private HttpListener http;

        private Configurable mainPageLocation;
        private Configurable port;
        private static string mainPage;
        private static string dbPath;

        private Thread thread;

        public Module_Answer_AddServer(Module_Answer m)
        {
            mainPageLocation = new Configurable()
            {
                Name = "WebPagePath",
                ReadableName = "Path to webpage for Adding new entries to DB",
                Parent = m
            };
            port = new Configurable()
            {
                Name = "webPort",
                ReadableName = "Web-server Port",
                DefaultValue = "80",
                Parent = m
            };
            m.Configurables.Add(mainPageLocation);
            m.Configurables.Add(port);

            mainPage = Accessories.ReadFileToString(mainPageLocation.Value);

            try
            {
                http = new HttpListener();
                http.Prefixes.Add("http://*:"+port.Value+"/");
                http.Start();
            }
            catch (Exception e) { InformationCollector.Error(http, e.Message); }

            thread = new Thread(Listen);
            thread.Start();

            dbPath = m.db_filename.Value;
        }
        public void Cleanup()
        {
            http.Stop();
            thread.Abort();
        }
        private void Listen()
        {
            while (http.IsListening)
            {
                IAsyncResult result = http.BeginGetContext(new AsyncCallback(ListenerCallback), http);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private static void ListenerCallback(IAsyncResult result)
        {
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;

                HttpListenerContext context = listener.EndGetContext(result);
                HttpListenerRequest request = context.Request;

                if (request.HttpMethod == "POST")
                {
                    ReceivedPOST(request);
                }

                HttpListenerResponse response = context.Response;
                string responseString = mainPage;

                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                output.Close();
            }
            catch (Exception e)
            {
                InformationCollector.Error(e.Source, e.Message);
            }
        }

        private static void ReceivedPOST(HttpListenerRequest request)
        {
            if (!request.HasEntityBody) return;

            using (Stream body = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(body))
                {
                    string[] text = WebUtility.UrlDecode(reader.ReadToEnd()).Split('&'); // taking word=123&answer=321 and splitting in two

                    string message = text[0].Remove(0, 5).ToLower(); // removing 'word='
                    string answer = text[1].Remove(0, 7); // removing 'answer='

                    if (message == "" || answer == "")
                        return;

                    /*	$sql = "
			                INSERT INTO
		                `words` (`message`, `answer`)
			                VALUES
		                ('$word', '$answer')";
                     */

                    foreach (string s in text)
                        InformationCollector.Info(request, s);

                    using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
                    {
                        try
                        {
                            if (connection.State != System.Data.ConnectionState.Closed)
                                connection.Close();
                            connection.Open();

                            using (SQLiteCommand sql_cmd = new SQLiteCommand("INSERT INTO 'words' (`message`, `answer`) VALUES ('" + message + "', '" + answer + "')", connection))
                                sql_cmd.ExecuteNonQuery();

                            connection.Close();
                        }
                        catch (Exception e)
                        {
                            InformationCollector.Error(connection, e.Message);
                        }
                    }
                }
            }
        }
    }
}
