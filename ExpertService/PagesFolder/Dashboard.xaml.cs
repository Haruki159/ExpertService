using ExpertService.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpertService.PagesFolder
{
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                // Получаем все заказы
                var allOrders = RepairServiceDBEntities.GetContext().Orders.ToList();
                int total = allOrders.Count;
                TxtTotalOrders.Text = total.ToString();

                // 1. ЗАПОЛНЯЕМ КАРТОЧКИ (ЦИФРЫ)
                DateTime today = DateTime.Today;
                int todayCount = allOrders.Count(o => o.DateCreated >= today);
                TxtOrdersToday.Text = todayCount.ToString();

                // Используем StatusID == 2 для "В работе" (как ты использовал ранее)
                int inProgressCount = allOrders.Count(o => o.StatusID == 2);
                TxtOrdersInProgress.Text = inProgressCount.ToString();


                // 2. СОЗДАЕМ ПРОГРЕСС-БАРЫ ДЛЯ СТАТУСОВ
                if (total == 0) return;

                // Группируем по StatusID, так как навигационного свойства Status нет
                var statusStats = allOrders
                    .GroupBy(o => o.StatusID)
                    .Select(g => new {
                        StatusID = g.Key, // ID статуса (число)
                        Count = g.Count(),
                        Percent = (double)g.Count() / total * 100 // Вычисляем процент
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                // Набор цветов для красоты
                var colors = new List<Brush>
                {
                    Brushes.Green, Brushes.Orange, Brushes.Red, Brushes.Blue, Brushes.Purple, Brushes.Brown, Brushes.Magenta
                };
                int colorIndex = 0;


                foreach (var stat in statusStats)
                {
                    // ВАЖНО: Находим название статуса по его ID (stat.StatusID)
                    // ПРОВЕРЬ: Название "Statuses" и поле "Title" могут быть другими в твоей БД.
                    // Если это не сработает, нужно будет изменить "Statuses" или "Title".
                    var statusTitle = RepairServiceDBEntities.GetContext().OrderStatuses // <--- ПРОВЕРЬ НАЗВАНИЕ КОЛЛЕКЦИИ
                      .FirstOrDefault(s => s.StatusID == stat.StatusID)?.StatusName // <--- ПРОВЕРЬ НАЗВАНИЕ ПОЛЯ (Title или Name?)
                      ?? $"ID {stat.StatusID} (Не найдено)"; // Защита от Null


                    // Контейнер (Grid) для текста и бара
                    Grid statusGrid = new Grid { Margin = new Thickness(0, 5, 0, 5) };
                    statusGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
                    statusGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    // Текст (Название статуса и процент)
                    TextBlock statusText = new TextBlock
                    {
                        Text = $"{statusTitle}: {stat.Count} шт. ({stat.Percent:F1}%)",
                        FontWeight = FontWeights.Bold,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(statusText, 0);

                    // Прогресс-бар
                    ProgressBar progressBar = new ProgressBar
                    {
                        Minimum = 0,
                        Maximum = 100,
                        Value = stat.Percent,
                        Height = 20,
                        Foreground = colors[colorIndex % colors.Count]
                    };
                    Grid.SetColumn(progressBar, 1);

                    // Добавляем элементы
                    statusGrid.Children.Add(statusText);
                    statusGrid.Children.Add(progressBar);

                    StatusStackPanel.Children.Add(statusGrid);

                    colorIndex++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки статистики: " + ex.Message);
            }
        }
    }
}