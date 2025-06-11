using ExpertService.DataBase;
using ExpertService.ViewModels;
using ExpertService.WindowsFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpertService.PagesFolder
{
    /// <summary>
    /// Логика взаимодействия для PartsPage.xaml
    /// </summary>
    public partial class PartsPage : Page
    {
        private readonly RepairServiceDBEntities _context;

        public PartsPage()
        {
            InitializeComponent();
            _context = RepairServiceDBEntities.GetContext();
            LoadParts();
        }

        /// <summary>
        /// Загружает запчасти из БД с учетом поискового запроса.
        /// </summary>
        /// <param name="searchText">Текст для поиска. Если пустой, загружаются все запчасти.</param>
        private void LoadParts(string searchText = "")
        {
            try
            {
                var query = _context.SpareParts.AsQueryable();

                // Применяем фильтр, если поисковая строка не пустая
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string lowerSearchText = searchText.ToLower();
                    query = query.Where(p =>
                        p.PartName.ToLower().Contains(lowerSearchText) ||
                        p.SKU.ToLower().Contains(lowerSearchText) ||
                        p.Description.ToLower().Contains(lowerSearchText));
                }

                // Проецируем результат в ViewModel для удобного отображения
                var partsList = query
                                .ToList() // Сначала получаем данные из БД
                                .Select(p => new PartViewModel(p)) // Затем трансформируем в ViewModel
                                .ToList();

                PartsDataGrid.ItemsSource = partsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки запчастей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Найти".
        /// </summary>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadParts(SearchTextBox.Text);
        }

        /// <summary>
        /// Обработчик изменения текста в поле поиска для "живого" поиска.
        /// </summary>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadParts(SearchTextBox.Text);
        }

        private void AddPartButton_Click(object sender, RoutedEventArgs e)
        {
            AddPartWindow addPartWindow = new AddPartWindow();
            addPartWindow.ShowDialog(); // Открываем окно модально
            LoadParts();
        }

        private void DeletePartButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем, выбрана ли запчасть в таблице
            if (PartsDataGrid.SelectedItem is PartViewModel selectedPartVm)
            {
                // 2. Запрашиваем подтверждение у пользователя
                var result = MessageBox.Show($"Вы уверены, что хотите удалить запчасть '{selectedPartVm.PartName}'?",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = RepairServiceDBEntities.GetContext();

                        // 3. ПРОВЕРКА: Используется ли запчасть в заказах
                        // Находим ID запчасти по артикулу (SKU), т.к. он уникален
                        var partInDb = context.SpareParts.FirstOrDefault(p => p.SKU == selectedPartVm.SKU);
                        if (partInDb == null)
                        {
                            MessageBox.Show("Не удалось найти запчасть для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        bool isUsed = context.UsedParts.Any(up => up.PartID == partInDb.PartID);
                        if (isUsed)
                        {
                            MessageBox.Show("Невозможно удалить эту запчасть, так как она уже используется в одном или нескольких заказах.", "Операция отменена", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        // 4. Удаление, если проверка пройдена
                        context.SpareParts.Remove(partInDb);
                        context.SaveChanges();

                        MessageBox.Show("Запчасть успешно удалена со склада.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadParts(SearchTextBox.Text); // Обновляем список
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запчасть для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    
    }
}
