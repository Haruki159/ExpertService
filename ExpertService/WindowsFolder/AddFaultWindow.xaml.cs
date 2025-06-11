using ExpertService.DataBase;
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
using System.Windows.Shapes;

namespace ExpertService.WindowsFolder
{
    public partial class AddFaultWindow : Window
    {
        public AddFaultWindow() { InitializeComponent(); }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FaultNameTextBox.Text))
            {
                MessageBox.Show("Введите название неисправности.");
                return;
            }

            using (var context = new RepairServiceDBEntities())
            {
                var newFault = new TypicalFault
                {
                    FaultName = FaultNameTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    RecommendedSolution = SolutionTextBox.Text
                };
                context.TypicalFaults.Add(newFault);
                context.SaveChanges();
            }
            MessageBox.Show("Подсказка успешно добавлена!");
            this.Close();
        }
    }
}
