using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.App.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal price { get; set; }
        public int CategoryId { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}
