// Skybot 2013-2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace SkyBot.Modules.Tools
{
    class MNotepad : IModule
    {
        private string _dbFolderPath = "\\plugins\\MNotepad.db";

        private Dictionary<object, List<string>> _notepadList = new Dictionary<object, List<string>>();

        public MNotepad()
        {
            ID = ModuleList.Notepad;
            UsableBy = APIList.All;

            if (File.Exists(Directory.GetCurrentDirectory() + _dbFolderPath))
                LoadBase();
        }

        private void LoadBase()
        {
            SQLHandler sqlHand = new SQLHandler(_dbFolderPath);

            var tableList = sqlHand.GetTables();

            foreach (var tableName in tableList)
            {
                var answerList = sqlHand.GetList(
                    tableName: tableName,
                    returnField: "TEXT");

                _notepadList.Add(tableName, answerList);
            }
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            var trigger = GetTrigger(msg.API);
            var message = msg.Text.Remove(0, 1);

            try
            {
                //if (Regex.IsMatch(message, @"n(?:ote)?p(?:ad)?"))
                if (msg.Text.Contains(trigger + "np") || msg.Text.Contains(trigger + "notepad"))
                {

                    if (Regex.IsMatch(message, @"n(?:ote)?p(?:ad)?\s+(\d+)$"))
                    {
                        return ProcessNotepadRead(
                            int.Parse(Regex.Match(message, @"n(?:ote)?p(?:ad)?\s+(\d+)$", RegexOptions.IgnoreCase).Groups[1].ToString()),
                            msg.Sender);
                    }
                    else if (Regex.IsMatch(message, @"n(?:ote)?p(?:ad)?\s+list$"))
                    {
                        return ProcessNotepadList(msg.Sender);
                    }
                    else if (Regex.IsMatch(message, @"^n(ote)?p(ad)?\s+"))
                    {
                        message = new Regex(@"^n(ote)?p(ad)?\s+").Replace(message, string.Empty);
                        return ProcessNotepadWrite(message, msg.Username, msg.Sender);
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        public string ProcessNotepadList(object chat)
        {
            if (_notepadList[chat].Count == 0)
                return "Соник еще не успел заспамить, потому записей нет.";
            else
                return "Всего найдено " + (_notepadList[chat].Count) + " записей. Читай любую.";
        }

        private string ProcessNotepadRead(int arrIndex, object chat)
        {
            if (arrIndex == 0)
                return "Не пытайся меня обкурячить, пиши нормальное число!";

            if (!_notepadList.Keys.Any(x => x.ToString() == chat.ToString()))
                return "Ваще голяк в базе по этому чату.";

            if (_notepadList[chat].Count > (arrIndex - 1))
                return _notepadList[chat][arrIndex - 1];
            else
                return "Ты охуел, нет столько записей!";
        }

        private string ProcessNotepadWrite(string str, string user, object chat)
        {
            string text = "Добавлено " + DateTime.Now.ToShortDateString() + " в " + DateTime.Now.ToLongTimeString() + " пользователем " + user + "\n" + str;

            SQLHandler sqlHand = new SQLHandler(_dbFolderPath);

            sqlHand.SetString(chat.ToString(), "TEXT", text);

            if (_notepadList.Keys.Any(x => x.ToString() == chat.ToString()))
                _notepadList[chat].Add(text);
            else
                _notepadList.Add(chat, new List<string>() { text });

            return "Записал под номером " + (_notepadList[chat].Count) + ", такие дела";
        }
    }
}
