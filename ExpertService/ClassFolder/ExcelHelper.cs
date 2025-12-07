using ExpertService.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExpertService.ClassFolder
{
    public class ExcelHelper
    {
        public void GenerateReport(List<Order> orders)
        {
            Excel.Application excelApp = null;
            try
            {
                // 1. Запускаем Excel
                excelApp = new Excel.Application();
                // excelApp.Visible = false; // Пока рисуем - скрываем (чтобы не мелькало)
                excelApp.Workbooks.Add();
                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

                // --- ШАГ 1: РИСУЕМ ШАПКУ ФИРМЫ ---

                // Название компании (Объединяем ячейки A1-E1)
                Excel.Range companyNameRange = workSheet.Range["A1", "E1"];
                companyNameRange.Merge();
                companyNameRange.Value = "Сервисный центр \"Эксперт\"";
                companyNameRange.Font.Size = 16;
                companyNameRange.Font.Bold = true;
                companyNameRange.Font.Name = "Arial";
                companyNameRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Адрес и телефон (A2-E2)
                Excel.Range addressRange = workSheet.Range["A2", "E2"];
                addressRange.Merge();
                addressRange.Value = "г. Кунгур, ул. Карла Маркса, д. 30 | Тел: +7 (342) 712-22-89";
                addressRange.Font.Size = 10;
                addressRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Заголовок отчета (A4-E4) - отступаем строчку
                Excel.Range titleRange = workSheet.Range["A4", "E4"];
                titleRange.Merge();
                titleRange.Value = $"ОТЧЕТ ПО ЗАКАЗАМ ОТ {DateTime.Now:dd.MM.yyyy}";
                titleRange.Font.Size = 12;
                titleRange.Font.Bold = true;
                titleRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue); // Голубой фон
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                titleRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // Рамка вокруг заголовка

                // --- ШАГ 2: ЗАГОЛОВКИ ТАБЛИЦЫ (Строка 6) ---
                int startRow = 6;

                workSheet.Cells[startRow, "A"] = "№";
                workSheet.Cells[startRow, "B"] = "Дата приема";
                workSheet.Cells[startRow, "C"] = "Клиент";
                workSheet.Cells[startRow, "D"] = "Оборудование";
                workSheet.Cells[startRow, "E"] = "Неисправность";

                // Красивое оформление заголовков
                Excel.Range headerRange = workSheet.Range[$"A{startRow}", $"E{startRow}"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                // --- ШАГ 3: ЗАПОЛНЯЕМ ДАННЫЕ ---
                int currentRow = startRow + 1;

                foreach (var order in orders)
                {
                    // Номер
                    workSheet.Cells[currentRow, "A"] = order.OrderID;
                    workSheet.Range[$"A{currentRow}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // По центру

                    // Дата (с проверкой, если вдруг Nullable)
                    workSheet.Cells[currentRow, "B"] = order.DateCreated.ToString();
                    // Или order.DateCreated.HasValue ? ... если дата может быть null

                    // Клиент
                    workSheet.Cells[currentRow, "C"] = order.Client != null ? order.Client.FullName : "Удален";

                    // Устройство
                    string device = "";
                    if (order.Device != null) device = $"{order.Device.Manufacturer} {order.Device.Model}";
                    workSheet.Cells[currentRow, "D"] = device;

                    // Описание
                    workSheet.Cells[currentRow, "E"] = order.ProblemDescription;

                    currentRow++;
                }

                // --- ШАГ 4: ФИНАЛЬНЫЕ ШТРИХИ ---

                // 1. Рисуем сетку (границы) вокруг ВСЕЙ таблицы данных
                int lastRow = currentRow - 1;
                Excel.Range dataRange = workSheet.Range[$"A{startRow}", $"E{lastRow}"];
                dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // Тонкие линии

                // 2. Автоширина колонок (чтобы текст влез)
                workSheet.Columns.AutoFit();

                // 3. Подвал (Итого)
                int footerRow = lastRow + 2;
                workSheet.Cells[footerRow, "D"] = "ИТОГО ЗАКАЗОВ:";
                workSheet.Range[$"D{footerRow}"].Font.Bold = true;
                workSheet.Range[$"D{footerRow}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                workSheet.Cells[footerRow, "E"] = orders.Count; // Кол-во строк
                workSheet.Range[$"E{footerRow}"].Font.Bold = true;

                // Показываем Excel
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка Excel: " + ex.Message);
                if (excelApp != null) excelApp.Quit(); // Если ошибка - закрываем процесс
            }
        }
    }
}
