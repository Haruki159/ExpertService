using ExpertService.DataBase;
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
    /// Логика взаимодействия для HelpPage.xaml
    /// </summary>
    public partial class HelpPage : Page
    {
        private readonly RepairServiceDBEntities _context;

        public HelpPage()
        {
            InitializeComponent();
            _context = RepairServiceDBEntities.GetContext();
            LoadFaultsList();
        }

        /// <summary>
        /// Загружает список типовых неисправностей в ListView.
        /// </summary>
        private void LoadFaultsList()
        {
            try
            {
                FaultsListView.ItemsSource = _context.TypicalFaults.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки справочника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик выбора элемента в списке неисправностей.
        /// </summary>
        private void FaultsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, что выбранный элемент - это сущность TypicalFault
            if (FaultsListView.SelectedItem is TypicalFault selectedFault)
            {
                // Показываем панель с деталями и скрываем текст-заглушку
                FaultDetailsPanel.Visibility = Visibility.Visible;
                HelpPlaceholderTextBlock.Visibility = Visibility.Collapsed;

                // Заполняем текстовые блоки данными из выбранного объекта
                FaultNameTextBlock.Text = selectedFault.FaultName;
                FaultDescriptionTextBlock.Text = selectedFault.Description ?? "Описание отсутствует.";
                FaultSolutionTextBlock.Text = selectedFault.RecommendedSolution ?? "Рекомендации отсутствуют.";
            }
            else
            {
                // Если выбор снят, возвращаем исходное состояние
                FaultDetailsPanel.Visibility = Visibility.Collapsed;
                HelpPlaceholderTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void AddFaultButton_Click(object sender, RoutedEventArgs e)
        {
            AddFaultWindow addFaultWindow = new AddFaultWindow();
            addFaultWindow.ShowDialog();
            LoadFaultsList();
        }
    }
}
