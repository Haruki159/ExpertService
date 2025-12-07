using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using System.IO;

namespace ExpertService.ClassFolder
{
    internal class WordHelper
    {
        private FileInfo _fileInfo;

        public WordHelper(string fileName)
        {
            // Проверяем, существует ли файл шаблона
            if (File.Exists(fileName))
            {
                _fileInfo = new FileInfo(fileName);
            }
            else
            {
                throw new ArgumentException("Файл шаблона не найден!");
            }
        }

        // Метод, который принимает словарь данных (ключ-значение)
        public void CreateDocument(Dictionary<string, string> items, string savePath)
        {
            Word.Application app = null;
            try
            {
                app = new Word.Application();
                Object file = _fileInfo.FullName;
                Object missing = Type.Missing;

                // Открываем шаблон
                Word.Document doc = app.Documents.Open(file);

                // Заполняем закладки (твой код)
                foreach (var item in items)
                {
                    if (doc.Bookmarks.Exists(item.Key))
                    {
                        doc.Bookmarks[item.Key].Range.Text = item.Value;
                    }
                }

                // === НОВОЕ: Сохраняем файл ===
                doc.SaveAs2(FileName: savePath);

                // Делаем Word видимым
                app.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
