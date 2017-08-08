// Skybot 2013-2017

using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace SkyBot.Modules.AI.Background
{
    class MRandom : IModule
    {
        // probability of triggering bot that defined as one from the specified number
        // this value is checked once a minute
        private int _interval = 240;
        private string _dbFolderPath = "\\plugins\\MRandom.db";

        private Dictionary<string, List<string>> _textList = new Dictionary<string, List<string>>();
        private Timer _randomTimer;
        private SkyBot _skyBot;

        public MRandom(SkyBot skyBot)
        {
            _skyBot = skyBot;

            ID = ModuleList.Random;
            UsableBy = APIList.All;

            if (File.Exists(Directory.GetCurrentDirectory() + _dbFolderPath))
                LoadBase();

            if (_textList.Count == 0) return;

            _randomTimer = new Timer(60 * 1000);
            _randomTimer.Elapsed += new ElapsedEventHandler(OnSendTimerEvent);
            _randomTimer.Enabled = true;
            _randomTimer.AutoReset = true;
        }

        private void LoadBase()
        {
            SQLHandler sqlHand = new SQLHandler(_dbFolderPath);

            var tableList = sqlHand.GetTables();

            foreach (var tableName in tableList)
            {
                var textList = sqlHand.GetList(
                    tableName: tableName,
                    returnField: "TEXT" );

                _textList.Add(tableName, textList);
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
                            API.SendMessage(dict.Value[chanceMsg], dict.Key);
                    }
                }
            }
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            string trigger = GetTrigger(msg.API);

            // only triggers
            if (msg.Text.IndexOf(trigger, 0, 1) >= 0)
            {
                string cmd = msg.Text.Remove(0, 1);

                if (cmd.Contains("set random interval"))
                {
                    try
                    {
                        _interval = Convert.ToInt32(cmd
                            .Replace("set random interval ", ""));
                        return "New random interval equil " + _interval;
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

