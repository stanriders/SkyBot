// Skybot 2013-2017

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.IO;
using System.Text;

namespace SkyBot.Modules.Tools
{
    class MFicBook : IModule
    {
        private string _cfgPath =  "\\plugins\\plugins\\ficbook.cfg";
        private string _archivePath = "\\plugins\\archive";
        private int _readLimit = 4096;
        private int _previewLimit = 128;

        private int _currentPage;
        private string _currentAuthor;
        private string _currentTitle;

        public MFicBook()
        {
            ID = ModuleList.FicBook;
            UsableBy = APIList.All;
        }

        public string GetAuthorList()
        {
            string result = "Список авторов";

            DirectoryInfo dir = new DirectoryInfo(_archivePath);
            foreach (var item in dir.GetDirectories())
            {
                result += "\n" + item.Name;
            }

            return result;
        }

        public string GetFromAuthor(string author)
        {
            string result = "Список произведений пользователя " + author;

            DirectoryInfo dir = new DirectoryInfo(_archivePath + "\\" + author);
            foreach (var item in dir.GetFiles())
            {
                result += "\n" + item.Name;
            }

            result = result.Replace(".pdf", "");
            return result;
        }

        public string GetFromTitle(string title)
        {
            DirectoryInfo dir = new DirectoryInfo(_archivePath);
            foreach (var dirInfo in dir.GetDirectories())
            {
                DirectoryInfo _dir = new DirectoryInfo(_archivePath + "\\" + dirInfo.Name);
                foreach (var fileInfo in _dir.GetFiles())
                {
                    if (fileInfo.Name == title)
                    {
                        _currentAuthor = dirInfo.Name;
                        _currentTitle = fileInfo.Name;
                        _currentPage = 1;
                        return ReadPage(_currentAuthor, _currentTitle, _currentPage);
                    }
                }
            }

            return "Произведение с таким названием не найдено";
        }

        public string ReadPage(string author, string title, int page, bool preview = false)
        {
            string text = ExtractTextFromPdf(_archivePath + "\\" + author + "\\" + title);

            if (text.Length <= _readLimit)
            {
                return text;
            }
            else
            {
                try
                {
                    if (preview)
                    {
                        return _currentPage == ((text.Length / _readLimit) + 1)
                            ? text.Substring(((_currentPage - 1) * _readLimit))
                            : text.Substring(((_currentPage - 1) * _readLimit), _previewLimit);
                    }
                    else
                    {
                        return _currentPage == ((text.Length / _readLimit) + 1)
                            ? "Страница " + _currentPage + " из " + ((text.Length / _readLimit) + 1) + "\n" + text.Substring(((_currentPage - 1) * _readLimit))
                            : "Страница " + _currentPage + " из " + ((text.Length / _readLimit) + 1) + "\n" + text.Substring(((_currentPage - 1) * _readLimit), _readLimit);
                    }
                }
                catch (Exception)
                {
                    return "Страница не найдена";
                }
            }
        }

        public string SavePage(string user)
        {
            string section;
            for (int i = 1; ; i++)
            {
                section = Config.Read(i.ToString(), "user", _cfgPath);
                if (section.Length == 0)
                {
                    Config.Write(i.ToString(), "user",
                        user + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(),
                        _cfgPath);
                    Config.Write(i.ToString(), "author", _currentAuthor, _cfgPath);
                    Config.Write(i.ToString(), "title", _currentTitle, _cfgPath);
                    Config.Write(i.ToString(), "page", _currentPage.ToString(), _cfgPath);
                    return "Сохранено под номером " + i;
                }
            }
        }

        public string LoadPage(string section)
        {
            string user = Config.Read(section, "user", _cfgPath);
            if (user.Length == 0) return "Запись под номером " + section + " не найдена";

            _currentAuthor = Config.Read(section, "author", _cfgPath);
            _currentTitle = Config.Read(section, "title", _cfgPath);
            _currentPage = int.Parse(Config.Read(section, "page", _cfgPath));

            return ReadPage(_currentAuthor, _currentTitle, _currentPage);
        }

        public string ViewSave()
        {
            string result = "Доступные сохранения:";

            for (int i = 1; ; i++)
            {
                string user = Config.Read(i.ToString(), "user", _cfgPath);
                if (user.Length != 0)
                {
                    string author = Config.Read(i.ToString(), "author", _cfgPath);
                    string title = Config.Read(i.ToString(), "title", _cfgPath);
                    string page = Config.Read(i.ToString(), "page", _cfgPath);
                    result += "\n* * *\n#" + i + " " + user +
                        "\n" + author + " - " + title.Replace(".pdf", "") +
                        "\n" + ReadPage(author, title, Convert.ToInt32(page), preview: true);
                }
                else
                {
                    return result;
                }
            }
        }

        public string ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
                return text.ToString();
            }
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            // ignoring triggers
            if (msg.Text.IndexOf(GetTrigger(msg.API), 0, 1) >= 0)
                return string.Empty;

            var tr = GetTrigger(msg.API);
            var com = msg.Text.Remove(0, 1);

            switch (com)
            {
                case "fic ?":
                case "fic help":
                    return $"{ tr }fic auth - получить список авторов, { tr }fic auth <nickname> - получить список произведений по автору, { tr }fic <title> - получить произведение по названию, { tr }fic nav - получить справку по навигации во время чтения";

                case "fic nav":
                case "fic navigation":
                    return $"Навигация во время чтения: { tr }fic next ({ tr }fn) - следующая страница, { tr }fic back ({ tr }fb) - предыдущая страница, { tr }fic read ({ tr }fr) - текущая страница, { tr }fic read <number> ({ tr }fr <number>) - показать страницу под номером, { tr }fic save ({ tr }fs <number>) - сохранить прогресс, { tr }fic load <number> ({ tr }fl <number>) - загрузить сохранение под номером, { tr }fic load ({ tr }fl) - список сохранений";

                case "fic auth":
                    return GetAuthorList();

                case "fn":
                case "fic next":
                    return ReadPage(_currentAuthor, _currentTitle, ++_currentPage);

                case "fb":
                case "fic back":
                    return ReadPage(_currentAuthor, _currentTitle, --_currentPage);

                case "fr":
                case "fic read":
                    return ReadPage(_currentAuthor, _currentTitle, _currentPage);

                case "fs":
                case "fic save":
                    return SavePage(msg.Username);

                case "fl":
                case "fic load":
                    return ViewSave();

                default:
                    {
                        if (com.Contains("fic auth"))
                        {
                            return GetFromAuthor(com
                                .Replace("fic auth ", ""));
                        }
                        else if (com.Contains("fr") || com.Contains("fic read"))
                        {
                            try
                            {
                                _currentPage = Convert.ToInt32(com
                                        .Replace("fr ", "")
                                        .Replace("fic read ", ""));

                                return ReadPage(_currentAuthor, _currentTitle, _currentPage);
                            }
                            catch (Exception)
                            {
                                return "Ровнее цифры пешы блеать!";
                            }
                        }
                        else if (com.Contains("fl") || com.Contains("fic load"))
                        {
                            return LoadPage(com
                                .Replace("fl ", "")
                                .Replace("fic load ", ""));
                        }
                        else if (com.Contains("set read limit"))
                        {
                            try
                            {
                                _readLimit = Convert.ToInt32(com
                                    .Replace("set read limit ", ""));
                                return "New read limit equil " + _readLimit;
                            }
                            catch (Exception)
                            {
                                return "Аккуратней играйся с такими командами блеать!";
                            }
                        }
                        else if (com.Contains("set preview limit"))
                        {
                            try
                            {
                                _previewLimit = Convert.ToInt32(com
                                    .Replace("set preview limit ", ""));
                                return "New preview limit equil " + _previewLimit;
                            }
                            catch (Exception)
                            {
                                return "Аккуратней играйся с такими командами блеать!";
                            }
                        }
                        else
                        {
                            return GetFromTitle(com);
                        }
                    }
            }
        }
    }
}
