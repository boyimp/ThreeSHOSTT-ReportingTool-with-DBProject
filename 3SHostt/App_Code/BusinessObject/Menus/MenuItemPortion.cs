using System;
using System.Collections.Generic;
using System.Linq;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Menus
{
    public class MenuItemPortion : EntityClass
    {
        public int MenuItemId { get; set; }
        public int Multiplier { get; set; }

        private IList<MenuItemPrice> _prices;
        public virtual IList<MenuItemPrice> Prices
        {
            get { return _prices; }
            set { _prices = value; }
        }

        public MenuItemPortion()
        {
            Multiplier = 1;
            _prices = new List<MenuItemPrice>();
        }

        public decimal Price
        {
            get { return GetDefaultPrice().Price; }
            set { GetDefaultPrice().Price = value; }
        }

        private MenuItemPrice GetDefaultPrice()
        {
            var result = Prices.FirstOrDefault(x => string.IsNullOrEmpty(x.PriceTag));
            if (result == null)
            {
                result = new MenuItemPrice();
                Prices.Add(result);
            }
            return result;
        }

        public void UpdatePrices(IEnumerable<MenuItemPrice> prices)
        {
            foreach (var menuItemPrice in prices)
            {
                var mitemPrice = menuItemPrice;
                var price = Prices.FirstOrDefault(x => x.PriceTag == mitemPrice.PriceTag);
                if (price == null)
                {
                    price = new MenuItemPrice();
                    Prices.Add(price);
                }
                price.Price = mitemPrice.Price;
                price.PriceTag = mitemPrice.PriceTag;
            }
        }
    }
}
