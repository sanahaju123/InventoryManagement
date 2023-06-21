using InventoryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BusinessLayer.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal price { get; set; }
        public int CategoryId { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}
