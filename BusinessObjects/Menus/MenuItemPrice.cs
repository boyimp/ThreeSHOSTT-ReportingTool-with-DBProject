using System.ComponentModel.DataAnnotations;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Menus
{
    public class MenuItemPrice : ValueClass
    {
        public int MenuItemPortionId { get; set; }
        [StringLength(10)]
        public string PriceTag { get; set; }
        public decimal Price { get; set; }
    }
}
