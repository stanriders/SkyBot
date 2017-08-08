// Skybot 2013-2017

using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Timers;
using System.Collections.Generic;

namespace SkyBot.Modules.AI.Background
{
    class MYouTube : IModule
    {
        // probability of triggering bot that defined as one from the specified number
        // this value is checked once a minute
        private int _interval = 240;
        private string _dbFolderPath = "\\plugins\\MYouTube.db";

        private Dictionary<string, List<string>> _textList = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> _urlList = new Dictionary<string, List<string>>();
        // blacklist for those urls that has been posted in chat
        private Dictionary<string, List<string>> _urlBlacklist = new Dictionary<string, List<string>>();
        private Timer _youtubeTimer;
        private SkyBot _skyBot;

        public MYouTube(SkyBot skyBot)
        {
            _skyBot = skyBot;

            ID = ModuleList.YouTube;
            UsableBy = APIList.All;

            if (File.Exists(Directory.GetCurrentDirectory() + _dbFolderPath))
                LoadBase();

            if (_textList.Count == 0) return;
            if (_urlList.Count == 0) return;

            _youtubeTimer = new Timer(60 * 1000);
            _youtubeTimer.Elapsed += new ElapsedEventHandler(OnSendTimerEvent);
            _youtubeTimer.Enabled = true;
            _youtubeTimer.AutoReset = true;
        }

        private void LoadBase()
        {
            SQLHandler sqlHand = new SQLHandler(_dbFolderPath);

            var tableList = sqlHand.GetTables();

            foreach (var tableName in tableList)
            {
                if (tableName.Contains("TEXT"))
                {
                    var textList = sqlHand.GetList(
                        tableName: tableName,
                        returnField: "TEXT");

                    _textList.Add(tableName.Replace("TEXT_", string.Empty), textList);
                }

                if (tableName.Contains("URL"))
                {
                    var urlList = sqlHand.GetList(
                        tableName: tableName,
                        returnField: "URL");

                    _urlList.Add(tableName.Replace("URL_", string.Empty), urlList);
                }
            }
        }

        private void OnSendTimerEvent(object source, ElapsedEventArgs e)
        {
            foreach (var dict in _textList)
            {
                int chanceMsg = RNG.Next(0, dict.Value.Count);
                string answer = dict.Value[chanceMsg];

                int chanceAns = RNG.Next(1, _interval);
                if (chanceAns == 1)
                {
                    foreach (var API in _skyBot.APIs)
                    {
                        if (API.Status == APIStatus.Connected)
                            API.SendMessage(GetVideo(dict.Key), dict.Key);
                    }
                }
            }
        }

        public string GetVideo(string chatId)
        {
            try
            {
                WebClient web = new WebClient();
                web.Encoding = Encoding.GetEncoding(1251);

                string source = web.DownloadString(_urlList[chatId][RNG.Next(0, _urlList[chatId].Count)]);

                var arr = Regex.Matches(source, @"href=""(/.+)""\s*class=""(?:.+)""\s*data-sessionlink=""(?:.+)""\s*dir=""(?:.+)"">(?:.+)</a>", RegexOptions.IgnoreCase)
                    .Cast<Match>()
                    .Select(m => "http://www.youtube.com" + m.Groups[1].Value)
                    .ToArray();

                for (int i = 0; ; i++)
                {
                    string url = arr[RNG.Next(1, arr.Length)];

                    if (!CheckBlacklist(url, chatId) && url.Length < 50)   // && защита от случая, когда парсер вместе с ссылкой хватает кусок кода
                    {
                        AddBlacklist(url, chatId);

                        return GetText(url, chatId);
                    }
                    else if (i == arr.Length)   // если все ролики из выдачи поиска на первой внезапно окажутся в блэклисте, дропаем цикл
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception e)
            {
                InformationCollector.Error(this, e.Message);
                return string.Empty;
            }
        }

        private string GetText(string url, string chatId)
        {
            string answer = _textList[chatId][RNG.Next(0, _textList[chatId].Count)];

            return answer.Replace("%url%", url);
        }

        private bool CheckBlacklist(string url, string chatId)
        {
            if (_urlBlacklist.Keys.Any(x => x == chatId))
                return _urlBlacklist[chatId].Any(x => x == url);
            else
                return false;
        }

        private void AddBlacklist(string url, string chatId)
        {
            if (_urlBlacklist.Keys.Any(x => x == chatId))
                _urlBlacklist[chatId].Add(url);
            else
                _urlBlacklist.Add(chatId, new List<string>() { url });
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            string trigger = GetTrigger(msg.API);

            // only triggers
            if (msg.Text.IndexOf(trigger, 0, 1) >= 0)
            {
                string cmd = msg.Text.Remove(0, 1);

                if (cmd.Contains("set youtube interval"))
                {
                    try
                    {
                        _interval = Convert.ToInt32(cmd
                            .Replace("set youtube interval ", ""));
                        return "New youtube interval equil " + _interval;
                    }
                    catch (Exception)
                    {
                        return "Аккуратней играйся с такими командами блеать!";
                    }
                }
            }
            return string.Empty;
        }
    }
}
