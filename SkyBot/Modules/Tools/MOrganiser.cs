// Skybot 2013-2017

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;

namespace SkyBot.Modules.Tools
{
    class TData
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public object ChatID { get; set; }
    }

    class MOrganiser : IModule
    {
        private List<TData> _orgDataDict = new List<TData>();
        private Timer _skyTimerOrg;
        private SkyBot _skyBot;

        public MOrganiser(SkyBot skyBot)
        {
            _skyBot = skyBot;

            ID = ModuleList.Organiser;
            UsableBy = APIList.All;

            _skyTimerOrg = new Timer(60 * 1000);
            _skyTimerOrg.Elapsed += new ElapsedEventHandler(OnTimedOrgEvent);
            _skyTimerOrg.Enabled = true;
            _skyTimerOrg.AutoReset = true;
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            if (msg.Text.IndexOf(GetTrigger(msg.API), 0, 1) >= 0)
            {
                string cmd = msg.Text.Remove(0, 1);
                Regex regOrg = new Regex(@"org(?:aniser)?\s+(?:(\d\d?):(\d\d?)\s?(\d\d?)?\.?(\d\d?)?\s+(.+)|(\d\d?):(\d\d?)\s+(.+))", RegexOptions.IgnoreCase);
                MatchCollection mcOrg = regOrg.Matches(cmd);
                Match mvOrg = regOrg.Match(cmd);

                if (mcOrg.Count > 0)
                {
                    try
                    {
                        DateTime date = new DateTime(
                            DateTime.Now.Year,
                            int.Parse(mvOrg.Groups[4].Value),
                            int.Parse(mvOrg.Groups[3].Value),
                            int.Parse(mvOrg.Groups[1].Value),
                            int.Parse(mvOrg.Groups[2].Value), 0);

                        _orgDataDict.Add(new TData()
                        {
                            Text = mvOrg.Groups[5].Value,
                            Date = date,
                            ChatID = msg.Sender,
                        });

                        return "Добавлено напоминание в " + date.Hour + ":" + date.Minute + " " + date.Day + "." + date.Month;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            DateTime date = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                int.Parse(mvOrg.Groups[1].Value),
                                int.Parse(mvOrg.Groups[2].Value), 0);

                            _orgDataDict.Add(new TData()
                            {
                                Text = mvOrg.Groups[5].Value,
                                Date = date,
                                ChatID = msg.Sender,
                            });

                            return "Добавлено напоминание в " + date.Hour + ":" + date.Minute;
                        }
                        catch (Exception)
                        {
                            return "Напоминание не добавлено, ибо ты криворукий мудак.";
                        }
                    }
                }
            }

            return string.Empty;
        }

        private void OnTimedOrgEvent(object source, ElapsedEventArgs e)
        {
            foreach (var orgData in _orgDataDict)
            {
                if (orgData.Date.ToShortDateString() != DateTime.Now.ToShortDateString()) continue;
                if (orgData.Date.ToShortTimeString() != DateTime.Now.ToShortTimeString()) continue;
                //if (orgData.Date.Month != DateTime.Now.Month) continue;
                //if (orgData.Date.Date.Day != DateTime.Now.Day) continue;
                //if (orgData.Date.Date.Hour != DateTime.Now.Hour) continue;
                //if (orgData.Date.Date.Minute != DateTime.Now.Minute) continue;

                foreach (var API in _skyBot.APIs)
                {
                    if (API.Status == APIStatus.Connected)
                        API.SendMessage(orgData.Text, orgData.ChatID);
                }
            }
        }
    }
}
