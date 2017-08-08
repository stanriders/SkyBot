// Skybot 2013-2017

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SkyBot.Modules.AI
{
    class TData
    {
        /// <summary>
        /// Список ID пользователя(-ей) через пробел
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Список слов через пробел
        /// </summary>
        public string Words { get; set; }

        /// <summary>
        /// Количество слов, необходимых для срабатывания триггера
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Время, в течение которого должны быть найдены слова из Words Count раз
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Ответы бота через ;
        /// </summary>
        public string Answers { get; set; }

        /// <summary>
        /// Уникальный ID триггера, позволяет добавлять и идентифицировать несколько триггеров в коллекцию TUserData с одинаковым ChatID
        /// </summary>
        public int DataID { get; set; }
    }

    class TUserData
    {
        /// <summary>
        /// Совпавший по UserID триггер
        /// </summary>
        public TData TData { get; set; }

        /// <summary>
        /// Количество слов, найденных сейчас до истечения таймаута
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Время последнего обновления счетчика
        /// </summary>
        public DateTime LastTime { get; set; }

        /// <summary>
        /// ID чата для ответа
        /// </summary>
        public object ChatID { get; set; }
    }

    class MTrigger : IModule
    {
        private string _dbFolderPath = "\\plugins\\MTrigger.db";

        // коллекция триггеров из БД
        private List<TData> _tDataList = new List<TData>();
        // коллекция подобранных, но еще не сработавших триггеров
        private List<TUserData> _tUserDataList = new List<TUserData>();

        public MTrigger()
        {
            ID = ModuleList.Trigger;
            UsableBy = APIList.All;
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            // ignoring triggers
            if (msg.Text.IndexOf(GetTrigger(msg.API), 0, 1) >= 0)
                return string.Empty;

            if (File.Exists(Directory.GetCurrentDirectory() + _dbFolderPath))
                LoadBase(msg.Sender);

            if (_tDataList.Count == 0)
                return string.Empty;

            string message = msg.Text.ToLower();

            return AnalyseMessage(message, msg.Sender, msg.Username);
        }

        private void LoadBase(object chatId)
        {
            SQLHandler sqlHand = new SQLHandler(_dbFolderPath);

            _tDataList = sqlHand.GetList<TData>(
                tableName: chatId.ToString());

            foreach (var tData in _tDataList)
                tData.DataID = tData.UserID.Length * tData.Words.Length;
        }

        public string AnalyseMessage(string message, object chatId, string userId)
        {
            foreach (var tData in _tDataList)
            {
                // проверяем совпадение ника в триггере
                if (tData.UserID.Split(new Char[] { ' ' }).Any(x => x == userId || x == "<all>"))
                {
                    var wordList = message
                        .Replace(',', ' ')
                        .Replace('.', ' ')
                        .Replace('?', ' ')
                        .Replace('!', ' ')
                        .Replace('-', ' ')
                        .Split(new Char[] { ' ' })
                        .ToList();

                    // првоеряем совпадение любого слова из сообщения со словом из триггера
                    if (tData.Words
                        .Split(new Char[] { ' ' })
                        .ToList()
                        .Intersect(wordList)
                        .ToList()
                        .Count > 0)
                    {
                        // проверяем наличие этого триггера в коллекции найденных триггеров по ID триггера и чата
                        if (_tUserDataList.Any(x => x.TData.DataID == tData.DataID && x.ChatID.ToString() == chatId.ToString()))
                        {
                            // триггер найден, проверяем кол-во найденных слов с необходимым для срабатывания
                            var tDataFound = _tUserDataList.Find(x => x.TData.DataID == tData.DataID && x.ChatID.ToString() == chatId.ToString());
                            tDataFound.Count++;

                            if (tDataFound.Count == tDataFound.TData.Count)
                            {
                                string[] answerList = tDataFound.TData.Answers.Split(new Char[] { ';' });

                                _tUserDataList.Remove(tDataFound);

                                // проверяем истечение таймаута
                                if ((int)(DateTime.Now - tDataFound.LastTime).TotalSeconds <= tDataFound.TData.Timeout)
                                {
                                    return answerList[RNG.Next(0, answerList.Length/* - 1*/)];
                                }
                            }
                        }
                        else
                        {
                            // триггер не найден, добавляем в коллекцию
                            _tUserDataList.Add(new TUserData()
                            {
                                TData = tData, Count = 1, LastTime = DateTime.Now, ChatID = chatId,
                            });
                        }

                        break;
                    }
                }
            }

            return string.Empty;
        }
    }
}
