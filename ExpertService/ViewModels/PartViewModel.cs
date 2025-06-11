using ExpertService.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertService.ViewModels
{
    public class PartViewModel
    {
        private const int LowStockThreshold = 5; // Порог, при котором товар считается заканчивающимся

        private readonly SparePart _part;

        public PartViewModel(SparePart part)
        {
            _part = part;
        }

        public string SKU => _part.SKU;
        public string PartName => _part.PartName;
        public string Description => _part.Description;
        public int QuantityInStock => _part.QuantityInStock;

        // Свойство для триггера в XAML, который подсвечивает строки
        public bool IsLowStock => _part.QuantityInStock > 0 && _part.QuantityInStock < LowStockThreshold;
    }
}

