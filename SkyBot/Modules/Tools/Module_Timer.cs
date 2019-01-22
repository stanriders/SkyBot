// Skybot 2013-2017

using System;
using System.Text.RegularExpressions;
using System.Timers;

namespace SkyBot.Modules.Tools
{
    class Module_Timer : IModule
    {
        private SkyBot bot;

        public Module_Timer(SkyBot b)
        {
            bot = b;

            ID = ModuleList.Timer;
            UsableBy = APIList.All;
        }
        public override string ProcessMessage(ReceivedMessage msg)
        {
            if (msg.Text.IndexOf(msg.API.GetTrigger(), 0, 1) >= 0)
            {
                string cmd = msg.Text.Remove(0, 1);

                int sTimer, mTimer, hTimer;
                Regex regTimer = new Regex(@"t(?:i)?m(?:er)?\s+(\d\d?):?(\d\d?)?:?(\d\d?)?", RegexOptions.IgnoreCase);
                MatchCollection mcTimer = regTimer.Matches(cmd);
                Match mvTimer = regTimer.Match(cmd);

                if (mcTimer.Count > 0)
                {
                    try
                    {
                        sTimer = int.Parse(mvTimer.Groups[3].Value);
                        mTimer = int.Parse(mvTimer.Groups[2].Value);
                        hTimer = int.Parse(mvTimer.Groups[1].Value);
                        if (sTimer > 59 && mTimer > 59 && hTimer > 23)
                            return "Ты с какой временной линии вообще, наркоман?";

                        ProcessSetTimer setTimer = new ProcessSetTimer(bot, msg, hTimer * 3600, mTimer * 60, sTimer);
                        return "Таймер установлен на " + hTimer + " часов " + mTimer + " минут " + sTimer + " секунд";
                    }
                    catch (Exception)
                    {
                        try
                        {
                            sTimer = int.Parse(mvTimer.Groups[2].Value);
                            mTimer = int.Parse(mvTimer.Groups[1].Value);
                            if (sTimer > 59 && mTimer > 59)
                                return "Ты с какой временной линии вообще, наркоман?";

                            ProcessSetTimer setTimer = new ProcessSetTimer(bot, msg, 61, mTimer * 60, sTimer);
                            return "Таймер установлен на " + mTimer + " минут " + sTimer + " секунд";
                        }
                        catch (Exception)
                        {
                            try
                            {
                                sTimer = int.Parse(mvTimer.Groups[1].Value);
                                if (sTimer > 59)
                                    return "Ты с какой временной линии вообще, наркоман?";

                                ProcessSetTimer setTimer = new ProcessSetTimer(bot, msg, 61, 61, sTimer);
                                return "Таймер установлен на " + sTimer + " секунд";
                            }
                            catch (Exception)
                            {
                                return "Таймер не установлен, ибо ты криворукий мудак.";
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }
    }

    class ProcessSetTimer
    {
        private SkyBot bot;
        private ReceivedMessage msg;

        private Timer timer;

        public ProcessSetTimer(SkyBot skybot, ReceivedMessage message, int h, int m, int s)
        {
            bot = skybot;
            msg = message;

            timer = new Timer(10000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            if (h != 61 && h != 0) timer.Interval = ((h + m) + s) * 1000;
            else if (m != 61 && h != 0) timer.Interval = (m + s) * 1000;
            else timer.Interval = s * 1000;

            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            msg.API.SendMessage("Timer event!", msg.Sender);
            timer.Stop();
        }
    }
}
