// Skybot 2013-2016

using System;
using System.Text.RegularExpressions;

namespace SkyBot.Modules.Tools
{
    class Module_Roll : IModule
    {
        public Module_Roll()
        {
            ID = ModuleList.Roll;
            UsableBy = APIList.All;
        }
        public override string ProcessMessage(string msg)
        {
            string result = string.Empty;

            if (Regex.IsMatch(msg, @"^\dd\d+$", RegexOptions.IgnoreCase))
            {
                Match myMatch = Regex.Match(msg, @"(^\d)d(\d+$)");
                try
                {
                    if (short.Parse(myMatch.Groups[1].Value) <= 10 && int.Parse(myMatch.Groups[2].Value) <= int.MaxValue) // ограничения: максимум роллов - 10, максимальное число рандома - int.MaxValue
                    {
                        for (int i = 1; i <= short.Parse(myMatch.Groups[1].Value);)
                        {
                            string diceresult = RNG.Next(1, int.Parse(myMatch.Groups[2].Value) + 1).ToString();
                            result += i + " ролл, " + "результат: " + diceresult + "\n";
                            i++;
                        }
                        return result;
                    }
                    else
                    {
                        return "Не знаю таких чисел.";
                    }
                }
                catch (Exception e)
                {
                    InformationCollector.Error(this, e.Message);
                    return "Нихуя ты загнул.";
                }
            }
            return result; // should never happen
        }
    }
}
