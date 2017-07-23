// Skybot 2013-2017

// Болтун: искусственный интеллект
// Модуль портирован из чат-бот серверной программы MaxXBot v1.6.1

using System;
using System.Timers;
using static SkyBot.SQLHandler;

namespace SkyBot.Modules.AI
{
    class MBoltun : IModule
    {
        // TODO: добавить управление ньюфажеством через команды
        // задержка перед ответом бота для ньюфагов чата
        public bool _newfag = false;
        private string _dbFolderPath = "\\plugins\\MBoltun.db";

        private string _newfagAnswer;
        private Timer _newfagTimer;
        private SQLHandler _sqlHand;

        public MBoltun()
        {
            ID = ModuleList.Boltun;
            UsableBy = APIList.All;

            _sqlHand = new SQLHandler(_dbFolderPath);
        }

        private string Formatting(string str, bool ignored = false)
        {
            str = str
                .Replace(".", "")
                .Replace(",", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace("-", "")
                .Replace(":", "")
                .Replace(";", "")
                .Replace(")", "")
                .Replace("(", "");

            if (ignored)
            {
                string[] split = str.Split(new Char[] { ' ' });
                string newstr = string.Empty;
                foreach (string word in split)
                {
                    if (_sqlHand.GetBoltunData(Mode.NOLIKE, str, "IGNORED") == string.Empty)
                        str += word + " ";
                }
                return newstr;
            }
            return str;
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            // включена задержка для ньюфагов чата
            if (_newfag)
            {
                // чтобы бот не отвечал на другой пост, пока будет работать таймер задержки
                if (_newfagAnswer.Length > 0)
                    return string.Empty; 

                _newfagAnswer = Stage_1(msg.Text);

                // задержка 1 сек на каждые 10 символов
                var perWord = _newfagAnswer.Length / 10;
                var minInt = perWord * 1;
                var maxInt = perWord * 5;
                var interval = RNG.Next(minInt, maxInt);

                _newfagTimer = new Timer(interval * 1000); 
                _newfagTimer.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs e) =>
                {
                    msg.API.SendMessage(_newfagAnswer, msg.Sender);

                    _newfagAnswer = string.Empty;
                });
                _newfagTimer.Enabled = true;

                return string.Empty;
            }
            else
            {
                return Stage_1(msg.Text);
            }
        }

        // заглавные буквы игнорируются всегда, а база QUESTION вообще не используется и хуй знает для чего нужна
        // сравнение с базами USER и replies НЕ РАБОТАЕТ, потому что реплики в базу занесены с большого регистра, нужно получить lower-запись      
        // 2k17: хуй знает зачем, но модуль был успешно портирован на новую версию бота с проведением рефакторинга

        /*
         * 1. Сначала идет поиск полного соответствия фразам из USER
         * совпадение: полностью, знаки препинания: да, заглавные буквы: да
         * О чем ты хочешь поговорить?
         * Я хочу поговорить о музыке.
         */
        private string Stage_1(string str)
        {
            string str_ = _sqlHand.GetBoltunData(Mode.NOLIKE, str, "USER");

            string results_ = str_.ToLower();

            if (str_ != string.Empty && results_ == str)
            {
                InformationCollector.Log("stage: 1\ninput: " + str + "\noutput: " + str_);
                return str_;
            }
            else
            {
                return Stage_2(str);
            }
        }

        /*
         * 2. Затем идет поиск полного соответствия фразам из replies
         * совпадение: полностью, знаки препинания: да, заглавные буквы: да
         * Привет!
         * Ну, дарова!
         * Привет!
         * Здравствуй!
         */
        private string Stage_2(string str)
        {
            string str_ = _sqlHand.GetBoltunData(Mode.NOLIKE, str, "replies");

            string results_ = str_.ToLower();

            if (str_ != string.Empty && results_ == str)
            {
                InformationCollector.Log("stage: 2\ninput: " + str + "\noutput: " + str_);
                return str_;
            }
            else
            {
                return Stage_3(str);
            }
        }

        /*
         * 3. Затем идет поиск соответствия фразам из USER
         * совпадение: 2 любых слова из паттерна, знаки препинания: нет, заглавные буквы: нет, IGNORED: да
         * О чем ты хочешь поговорить?
         * Я хочу поговорить о музыке.
         */
        private string Stage_3(string str)
        {
            string results = Formatting(str, true);
            string[] split = results.Split(new Char[] { ' ' });

            int i = 0;
            foreach (string word in split)
            {
                string str_ = _sqlHand.GetBoltunData(Mode.LIKE, results, "USER", word);

                string results_ = Formatting(str_).ToLower();

                if (results_ != string.Empty)
                {
                    i++;
                }
                if (i == 2)
                {
                    InformationCollector.Log("stage: 3\ninput: " + str + "\noutput: " + str_);
                    return str_;
                }
            }
            return Stage_4(str);
        }

        /*
         * 4. Затем идет поиск соответствия фразам из replies
         * совпадение: 2 любых слова из паттерна, знаки препинания: нет, заглавные буквы: нет, IGNORED: да
         * О чем ты хочешь поговорить?
         * Я хочу поговорить о музыке.
         */
        private string Stage_4(string str)
        {
            string results = Formatting(str, true);
            string[] split = results.Split(new Char[] { ' ' });

            int i = 0;
            foreach (string word in split)
            {
                string str_ = _sqlHand.GetBoltunData(Mode.LIKE, results, "replies", word);

                string results_ = Formatting(str_).ToLower();

                if (results_ != string.Empty)
                {
                    i++;
                }
                if (i == 2)
                {
                    InformationCollector.Log("stage: 4\ninput: " + str + "\noutput: " + str_);
                    return str_;
                }
            }
            return Stage_5(str);
        }

        /*
         * 5. Затем идет поиск вхождения в строку из keywords
         * совпадение: весь паттерн, знаки препинания: на конце и внутри шаблона, если есть, заглавные буквы: нет
         * знаешь?
         * Конечно, знаю!
         * не знаю.
         * А я уверен, что ты знаешь...
         */
        private string Stage_5(string str)
        {
            string results = Formatting(str);
            string[] split = results.Split(new Char[] { ' ' });

            foreach (string word in split)
            {
                string str_ = _sqlHand.GetBoltunData(Mode.LIKE, results, "keywords", word);

                string results_ = Formatting(str_).ToLower();

                if (results_ != string.Empty && results.Contains(results_))
                {
                    InformationCollector.Log("stage: 5\ninput: " + str + "\noutput: " + str_);
                    return str_;
                }
            }
            return Stage_6(str);
        }

        /*
         * 6. Затем идет поиск вхождения в строку из specwords
         * совпадение: 1 любое слово из паттерна, знаки препинания: на конце, заглавные буквы: нет
         * я меня мне мной.
         * Ты все время говоришь о себе.
         * уже.
         * Уже?
         */
        private string Stage_6(string str)
        {
            string results = Formatting(str, true);
            string[] split = results.Split(new Char[] { ' ' });

            foreach (string word in split)
            {
                string str_ = _sqlHand.GetBoltunData(Mode.LIKE, results, "specwords", word);

                string results_ = Formatting(str_).ToLower();

                if (results_ != string.Empty)
                {
                    InformationCollector.Log("stage: 6\ninput: " + str + "\noutput: " + str_);
                    return str_;
                }
            }
            return Stage_7(str);
        }

        /*
         * 7. Затем рандомно выбирается фраза из ESCAPE_
         * Что ты сказал?
         */
        private string Stage_7(string str)
        {
            string results = Formatting(str, true);
            string[] split = results.Split(new Char[] { ' ' });

            string str_ = _sqlHand.GetBoltunData(Mode.LIKE, results, "ESCAPE_", "RANDOM");
            InformationCollector.Log("stage: 7\ninput: " + str + "\noutput: " + str_);
            return str_;
        }

        /*
         * 8. Затем рандомно выбирается фраза из INITIAL
         * Привет, дружище!
         */
        private string Stage_8(string str)
        {
            string results = Formatting(str, true);
            string[] split = results.Split(new Char[] { ' ' });

            string str_ = _sqlHand.GetBoltunData(Mode.LIKE, results, "INITIAL", "RANDOM");
            InformationCollector.Log("stage: 8\ninput: " + str + "\noutput: " + str_);
            return str_;
        }

        /* 
         * 9. Затем выводится реплика "Не понял"
         */
        private string Stage_9(string str)
        {
            string str_ = "Не понял";
            InformationCollector.Log("stage: 9\ninput: " + str + "\noutput: " + str_);
            return str_;
        }
    }
}
