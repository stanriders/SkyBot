// Skybot 2013-2016

using System;
using System.Text.RegularExpressions;

namespace SkyBot.Modules.Tools
{
    class Module_Roll : IModule
    {
        private Random random;

        public Module_Roll()
        {
            ID = ModuleList.Roll;

            random = new Random();
        }
        public override string ProcessMessage(string msg)
        {
            string result = string.Empty;

            if (Regex.IsMatch(msg, @"^\dd\d+$", RegexOptions.IgnoreCase))
            {
                Match myMatch = Regex.Match(msg, @"(^\d)d(\d+$)");
                if (int.Parse(myMatch.Groups[1].Value) <= 10 && int.Parse(myMatch.Groups[2].Value) <= 32767) // ограничения: максимум роллов - 10, максимальное число рандома - 32767
                {
                    for (int i = 1; i <= int.Parse(myMatch.Groups[1].Value);)
                    {
                        string diceresult = random.Next(1, int.Parse(myMatch.Groups[2].Value) + 1).ToString();
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
            return result; // should never happen
        }
    }
}
