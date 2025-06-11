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
    public partial class AddPartWindow : Window
    {
        public AddPartWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PartNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(SkuTextBox.Text) ||
                !int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                MessageBox.Show("Заполните все обязательные поля и введите корректное количество.");
                return;
            }

            using (var context = new RepairServiceDBEntities())
            {
                var newPart = new SparePart
                {
                    PartName = PartNameTextBox.Text,
                    SKU = SkuTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    QuantityInStock = quantity
                };
                context.SpareParts.Add(newPart);
                context.SaveChanges();
            }
            MessageBox.Show("Деталь успешно добавлена!");
            this.Close();
        }
    }
}