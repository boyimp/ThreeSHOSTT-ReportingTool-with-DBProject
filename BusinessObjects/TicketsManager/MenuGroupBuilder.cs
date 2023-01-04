using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ThreeS.Domain.Models.Tickets;
using ThreeS.Domain.Models.Accounts;
using System.ComponentModel.DataAnnotations;
using ThreeS.Domain.Models.Menus;
using BusinessObjects.AccountsManager;

namespace BusinessObjects.TicketsManager
{
    //Badhon check
    internal class UserInfo
    {
        public string UserName { get; set; }
        public decimal Amount { get; set; }

    }

    internal class TicketTagInfo
    {
        public decimal Amount { get; set; }
        public int TicketCount { get; set; }
        private string _tagName;
        public string TagName
        {
            get { return !string.IsNullOrEmpty(_tagName) ? _tagName.Trim() : "[Ticket]"; }
            set { _tagName = value; }
        }
    }

    internal class MenuItemSellInfo
    {
        //public string GroupName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
    }

    internal class MenuItemSpentTimeSellInfo
    {
        //public string GroupName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Portion { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountWithOrderTag { get; set; }
        public decimal TimeSpent { get; set; }
    }

    internal class DepartmentWiseSellInfo
    {
        //public string GroupName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    public class MenuItemPrice : ValueClass
    {
        public int MenuItemPortionId { get; set; }
        [StringLength(10)]
        public string PriceTag { get; set; }
        public decimal Price { get; set; }
    }

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
            var result = Prices.SingleOrDefault(x => string.IsNullOrEmpty(x.PriceTag));
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
                var price = Prices.SingleOrDefault(x => x.PriceTag == mitemPrice.PriceTag);
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

    public class MenuItem : EntityClass
    {
        public MenuItem()
            : this(string.Empty)
        {
        }

        public MenuItem(string name)
        {
            Name = name;
            _portions = new List<MenuItemPortion>();
        }

        public string GroupCode { get; set; }
        public string Barcode { get; set; }
        public string Tag { get; set; }

        private IList<MenuItemPortion> _portions;
        public virtual IList<MenuItemPortion> Portions
        {
            get { return _portions; }
            set { _portions = value; }
        }

        private static MenuItem _all;
        public static MenuItem All { get { return _all ?? (_all = new MenuItem { Name = "*" }); } }

        public MenuItemPortion AddPortion(string portionName, decimal price, string currencyCode)
        {
            var mip = new MenuItemPortion
            {
                Name = portionName,
                Price = price,
                MenuItemId = Id
            };
            Portions.Add(mip);
            return mip;
        }

        internal MenuItemPortion GetPortion(string portionName)
        {
            foreach (var portion in Portions)
            {
                if (portion.Name == portionName)
                    return portion;
            }
            if (string.IsNullOrEmpty(portionName) && Portions.Count > 0) return Portions[0];
            throw new Exception("Portion not found.");
        }

        public string UserString
        {
            get { return string.Format("{0} [{1}]", Name, GroupCode); }
        }

        public static MenuItemPortion AddDefaultMenuPortion(MenuItem item)
        {
            return item.AddPortion("Normal", 0, "");
        }

        public static OrderTag AddDefaultMenuItemProperty(OrderTagGroup item)
        {
            return item.AddOrderTag("", 0);
        }

        public static MenuItem Create()
        {
            var result = new MenuItem();
            AddDefaultMenuPortion(result);
            return result;
        }
    }

    internal class MenuItemGroupInfo
    {
        public string GroupName { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountWithOrderTag { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal QuantityRate { get; set; }
        public decimal GrossAmount { get; set; }
    }
    internal class MenuItemGroupAccountsInfo
    {
        public string GroupName { get; set; }
        public int SortOrder { get; set; }
        public bool IsTax { get; set; }
        public bool IsSD { get; set; }
        public bool IsDiscount { get; set; }
        public bool IsServiceCharge { get; set; }
        public string AccountHead { get; set; }
        public decimal Amount { get; set; }
        public decimal SDAmount { get; set; }
        public decimal ServiceChargeAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal OrderTotal { get; set; }
    }
    internal class MenuItemOrderAccountsInfo
    {
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public string GroupName { get; set; }
        public string MenuItemName { get; set; }
        public string PortionName { get; set; }
        public int SortOrder { get; set; }
        public string AccountHead { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal AmountExcludingDiscount { get; set; }
    }
    internal class MenuGroupItemInfo
    {
        public string GroupName { get; set; }
        public string ItemName { get; set; }
        public string Portion { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountWithOrderTag { get; set; }
        public decimal Price { get; set; }
        public decimal TimeMins { get; set; }
    }

    internal class DepartmentItemInfo
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
    public class CustomerAdvanceDueInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    internal static class MenuGroupBuilder
    {
        public static IEnumerable<MenuItemGroupInfo> CalculateMenuGroups(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by menuItem.GroupCode into grp
                select new MenuItemGroupInfo
                {
                    GroupName = grp.Key,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    AmountWithOrderTag = grp.Sum(y => CalculateOrderWithOrderTagTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };

            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.AmountWithOrderTag);

            var sum = menuItemInfoGroups.Sum(x => x.AmountWithOrderTag);
            foreach (var menuItemInfoGroup in result)
            {
                if (sum > 0)
                    menuItemInfoGroup.Rate = (menuItemInfoGroup.AmountWithOrderTag * 100) / sum;
                if (string.IsNullOrEmpty(menuItemInfoGroup.GroupName))
                    menuItemInfoGroup.GroupName = "[Undefined]";
            }

            var qsum = menuItemInfoGroups.Sum(x => x.Quantity);
            foreach (var menuItemInfoGroup in result)
            {
                if (qsum > 0)
                    menuItemInfoGroup.QuantityRate = (menuItemInfoGroup.Quantity * 100) / qsum;
                if (string.IsNullOrEmpty(menuItemInfoGroup.GroupName))
                    menuItemInfoGroup.GroupName = "[Undefined]";
            }
            return result;
        }        
        public static IEnumerable<MenuItemGroupAccountsInfo> CalculateGroupCodePaymentInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItemOrderAccountsInfo> Sales, IEnumerable<MenuItemOrderAccountsInfo> Calculations, 
            IEnumerable<MenuItemOrderAccountsInfo> Taxes, List<string> payments, IEnumerable<TaxTemplate> taxTemplates, IEnumerable<CalculationType> calculationTypes)
        {            
            var OrderSalesAmount = from sale in Sales group sale by new { OrderId = sale.OrderId } into salegrp
                                   select new MenuItemOrderAccountsInfo { OrderId = salegrp.Key.OrderId, Amount = salegrp.Sum(y => y.Amount) };
            var OrderTaxAmount = from sale in Taxes
                                   group sale by new { OrderId = sale.OrderId } into salegrp
                                   select new MenuItemOrderAccountsInfo { OrderId = salegrp.Key.OrderId, Amount = salegrp.Sum(y => y.Amount) };
            var OrderCalculationAmount = from sale in Calculations
                                         group sale by new { OrderId = sale.OrderId } into salegrp
                                   select new MenuItemOrderAccountsInfo { OrderId = salegrp.Key.OrderId, Amount = salegrp.Sum(y => y.Amount) };
            var menuItemInfoGroups =
                from sale in OrderSalesAmount
                join calculation in OrderCalculationAmount on sale.OrderId equals calculation.OrderId
                join taxTemp in OrderTaxAmount on sale.OrderId equals taxTemp.OrderId into tempJoin
                from tax in tempJoin.DefaultIfEmpty()
                group new { Sales = sale.Amount, Taxes = (tax == null ? 0 : tax.Amount), Calculations = calculation.Amount } 
                by new { OrderId = sale.OrderId, MenuItemName = sale.MenuItemName, GroupCode = sale.GroupName }
                into grp                
                select new MenuItemOrderAccountsInfo
                {
                    OrderId = grp.Key.OrderId,
                    MenuItemName = "",
                    GroupName = "",
                    AccountHead = "Net",
                    SortOrder = -2,
                    Amount = grp.Sum(y => y.Sales + y.Calculations + y.Taxes),                    
                };
            var menuItemPaymentInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join Info in menuItemInfoGroups on c.Order.Id equals Info.OrderId
                from payment in payments
                group new { c.Ticket, c.Order, OrderAmount = Info.Amount} 
                by new { OrderId = c.Order.Id, c.Order.MenuItemName, GroupCode = c.Order.MenuGroupName, PaymentName = payment } into grp
                select new MenuItemOrderAccountsInfo
                {
                    OrderId = grp.Key.OrderId,
                    MenuItemName = grp.Key.MenuItemName,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.PaymentName,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculatePaymentTotal(y.Ticket, y.Order, grp.Key.PaymentName, y.OrderAmount)),
                };                                    

            var menuItemGroupPaymentInfo =
                from info in menuItemPaymentInfoGroups
                group info by new { GroupCode = info.GroupName, PaymentName = info.AccountHead } into grp
                select new MenuItemGroupAccountsInfo
                {
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.PaymentName,
                    SortOrder = 2000,
                    Quantity = 0,
                    Amount = grp.Sum(y => y.Amount),
                    OrderTotal = 0,
                    GrossAmount = 0
                };
            var result = menuItemGroupPaymentInfo.ToList().OrderBy(x => x.GroupName).OrderBy(x => x.AccountHead);
            return result;
        }
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateItemPaymentInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItemOrderAccountsInfo> Sales, IEnumerable<MenuItemOrderAccountsInfo> Calculations,
           IEnumerable<MenuItemOrderAccountsInfo> Taxes, List<string> payments, IEnumerable<TaxTemplate> taxTemplates, IEnumerable<CalculationType> calculationTypes)
        {
            var OrderSalesAmount = from sale in Sales
                                   group sale by new { OrderId = sale.OrderId } into salegrp
                                   select new MenuItemOrderAccountsInfo { OrderId = salegrp.Key.OrderId, Amount = salegrp.Sum(y => y.Amount) };
            var OrderTaxAmount = from sale in Taxes
                                 group sale by new { OrderId = sale.OrderId } into salegrp
                                 select new MenuItemOrderAccountsInfo { OrderId = salegrp.Key.OrderId, Amount = salegrp.Sum(y => y.Amount) };
            var OrderCalculationAmount = from sale in Calculations
                                         group sale by new { OrderId = sale.OrderId } into salegrp
                                         select new MenuItemOrderAccountsInfo { OrderId = salegrp.Key.OrderId, Amount = salegrp.Sum(y => y.Amount) };
            var menuItemInfoGroups =
                from sale in OrderSalesAmount
                join calculation in OrderCalculationAmount on sale.OrderId equals calculation.OrderId
                join taxTemp in OrderTaxAmount on sale.OrderId equals taxTemp.OrderId into tempJoin
                from tax in tempJoin.DefaultIfEmpty()
                group new { Sales = sale.Amount, Taxes = (tax == null ? 0 : tax.Amount), Calculations = calculation.Amount }
                by new { OrderId = sale.OrderId, MenuItemName = sale.MenuItemName, GroupCode = sale.GroupName }
                into grp
                select new MenuItemOrderAccountsInfo
                {
                    OrderId = grp.Key.OrderId,
                    MenuItemName = "",
                    GroupName = "",
                    AccountHead = "Net",
                    SortOrder = -2,
                    Amount = grp.Sum(y => y.Sales + y.Calculations + y.Taxes),
                };
            var menuItemPaymentInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join Info in menuItemInfoGroups on c.Order.Id equals Info.OrderId
                from payment in payments
                group new { c.Ticket, c.Order, OrderAmount = Info.Amount }
                by new { MenuItemId = c.Order.MenuItemId, c.Order.MenuItemName, c.Order.PortionName, c.Order.Price, GroupCode = c.Order.MenuGroupName, PaymentName = payment } into grp
                select new MenuItemOrderAccountsInfo
                {
                    MenuItemId = grp.Key.MenuItemId,
                    MenuItemName = grp.Key.MenuItemName,
                    PortionName = grp.Key.PortionName,
                    Price = grp.Key.Price,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.PaymentName,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculatePaymentTotal(y.Ticket, y.Order, grp.Key.PaymentName, y.OrderAmount)),
                };
            //var menuItemPaymentInfoGroups2 =
            //    from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
            //    join Info in menuItemInfoGroups on c.Order.Id equals Info.OrderId
            //    from payment in payments
            //    group new { c.Ticket, c.Order, OrderAmount = Info.Amount }
            //    by new { OrderId = c.Order.Id, c.Order.MenuItemName, GroupCode = c.Order.MenuGroupName, PaymentName = payment } into grp
            //    select new MenuItemOrderAccountsInfo
            //    {
            //        OrderId = grp.Key.OrderId,
            //        MenuItemName = grp.Key.MenuItemName,
            //        GroupName = grp.Key.GroupCode,
            //        AccountHead = grp.Key.PaymentName,
            //        Quantity = grp.Sum(y => y.Order.Quantity),
            //        Amount = grp.Sum(y => CalculatePaymentTotal(y.Ticket, y.Order, grp.Key.PaymentName, y.OrderAmount)),
            //    };
            //var menuItemGroupPaymentInfo =
            //    from info in menuItemPaymentInfoGroups
            //    group info by new { GroupCode = info.GroupName, PaymentName = info.AccountHead } into grp
            //    select new MenuItemGroupAccountsInfo
            //    {
            //        GroupName = grp.Key.GroupCode,
            //        AccountHead = grp.Key.PaymentName,
            //        SortOrder = 2000,
            //        Quantity = 0,
            //        Amount = grp.Sum(y => y.Amount),
            //        OrderTotal = 0,
            //        GrossAmount = 0
            //    };
            var result = menuItemPaymentInfoGroups.ToList().OrderBy(x => x.MenuItemName).OrderBy(x => x.AccountHead);
            return result;
        }
        //OrderSales
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateOrderSalesInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                group c by new { OrderId = c.Order.Id, c.Order.MenuItemName, GroupCode = c.Order.MenuGroupName } into grp
                select new MenuItemOrderAccountsInfo
                {
                    OrderId = grp.Key.OrderId,
                    MenuItemName = grp.Key.MenuItemName,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = "Sales",
                    SortOrder = -2,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => y.Order.Total),
                };
            var result = menuItemInfoGroups.ToList().OrderBy(x => x.OrderId).OrderBy(x => x.AccountHead);
            DataSet dsMenGroupsAccountsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menuItemInfoGroups.ToList());
            return result;
        }
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateItemOrderSalesInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                group c by new { MenuItemId = c.Order.MenuItemId, c.Order.MenuItemName,c.Order.PortionName, c.Order.Price, GroupCode = c.Order.MenuGroupName } into grp
                select new MenuItemOrderAccountsInfo
                {
                    MenuItemId = grp.Key.MenuItemId,
                    MenuItemName = grp.Key.MenuItemName,
                    PortionName = grp.Key.PortionName,
                    Price = grp.Key.Price,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = "Sales",
                    SortOrder = -2,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => y.Order.Total),
                };
            var result = menuItemInfoGroups.ToList().OrderBy(x => x.OrderId).OrderBy(x => x.AccountHead);
            DataSet dsMenGroupsAccountsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menuItemInfoGroups.ToList());
            return result;
        }
        //OrderPayment
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateOrderPaymentInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<string> payments)
        {
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                from payment in payments
                group c by new { OrderId = c.Order.Id, c.Order.MenuItemName, GroupCode = c.Order.MenuGroupName, PaymentName = payment } into grp
                select new MenuItemOrderAccountsInfo
                {
                    OrderId = grp.Key.OrderId,
                    MenuItemName = grp.Key.MenuItemName,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.PaymentName,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculatePaymentTotal(y.Ticket, y.Order, grp.Key.PaymentName, 0)),
                };
            var result = menuItemInfoGroups.ToList().OrderBy(x => x.OrderId).OrderBy(x => x.AccountHead);
            DataSet dsMenGroupsAccountsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menuItemInfoGroups.ToList());
            return result;
        }
        //OrderPayment
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateMenuItemOrderWiseAccountsInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<TaxTemplate> taxTemplates)
        {
            DataTable dtAccountType = AccountManager.GetAccountType();
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                from taxTemplate in taxTemplates
                group c by new { OrderId = c.Order.Id, c.Order.MenuItemName, GroupCode = c.Order.MenuGroupName, taxTemplate.AccountTransactionType.SourceAccountTypeId, AccountTransactionTypeId = taxTemplate.AccountTransactionType.Id, TaxTemplateName = taxTemplate.Name } into grp
                select new MenuItemOrderAccountsInfo
                {
                    OrderId = grp.Key.OrderId,
                    MenuItemName = grp.Key.MenuItemName,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.TaxTemplateName,
                    SortOrder = Convert.ToInt32(dtAccountType.Select(string.Format("Id='{0}'", grp.Key.SourceAccountTypeId.ToString()))[0]["SortOrder"]),
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    //Amount = grp.Sum(y => y.Order.GetTotalTaxAmount(y.Ticket.TaxIncluded, y.Ticket.PlainSum, y.Ticket.PreTaxServicesTotal, grp.Key.AccountTransactionTypeId)),
                    Amount = grp.Sum(y => CalculateTaxTotal(y.Ticket, y.Order, grp.Key.AccountTransactionTypeId)),
                };

            var result = menuItemInfoGroups.ToList().OrderBy(x => x.AccountHead).OrderBy(x => x.OrderId);
            DataSet dsMenGroupsAccountsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menuItemInfoGroups.ToList());
            return result;
        }
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateItemOrderWiseAccountsInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<TaxTemplate> taxTemplates)
        {
            DataTable dtAccountType = AccountManager.GetAccountType();
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                from taxTemplate in taxTemplates
                group c by new { MenuItemId = c.Order.MenuItemId, c.Order.MenuItemName,c.Order.PortionName,c.Order.Price, GroupCode = c.Order.MenuGroupName, taxTemplate.AccountTransactionType.SourceAccountTypeId, AccountTransactionTypeId = taxTemplate.AccountTransactionType.Id, TaxTemplateName = taxTemplate.Name } into grp
                select new MenuItemOrderAccountsInfo
                {
                    MenuItemId = grp.Key.MenuItemId,
                    MenuItemName = grp.Key.MenuItemName,
                    PortionName = grp.Key.PortionName,
                    Price = grp.Key.Price,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.TaxTemplateName,
                    SortOrder = Convert.ToInt32(dtAccountType.Select(string.Format("Id='{0}'", grp.Key.SourceAccountTypeId.ToString()))[0]["SortOrder"]),
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    //Amount = grp.Sum(y => y.Order.GetTotalTaxAmount(y.Ticket.TaxIncluded, y.Ticket.PlainSum, y.Ticket.PreTaxServicesTotal, grp.Key.AccountTransactionTypeId)),
                    Amount = grp.Sum(y => CalculateTaxTotal(y.Ticket, y.Order, grp.Key.AccountTransactionTypeId)),
                };

            var result = menuItemInfoGroups.ToList().OrderBy(x => x.AccountHead).OrderBy(x => x.OrderId);
            DataSet dsMenGroupsAccountsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menuItemInfoGroups.ToList());
            return result;
        }
        public static decimal CalculateTaxTotal(Ticket ticket, Order order, int AccountTransactionTypeId)
        {
            if (order.Id == 14498)
            {
                
            }
            decimal OrderPriceIncludingPreTaxService = ticket.CalculatePreTaxPrice(ticket.Calculations.Where(x => !x.IncludeTax), order);
            decimal nTotal = 0;
            nTotal = order.GetTotalTaxAmountUpdated(ticket.TaxIncluded, ticket.PlainSum, OrderPriceIncludingPreTaxService, AccountTransactionTypeId);
            return nTotal;
        }
        //OrderCalculations
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateMenuItemOrderGroupCalculationInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<CalculationType> calculationTypes)
        {
            DataTable dtAccountType = AccountManager.GetAccountType();
            var menuItemInfoGroups =
                 from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                 from calculationType in calculationTypes
                 group c by new
                 {
                     OrderId = c.Order.Id,
                     c.Order.MenuItemName,
                     GroupCode = c.Order.MenuGroupName,
                     calculationType.DecreaseAmount,
                     calculationType.AccountTransactionType.TargetAccountTypeId,
                     calculationType.AccountTransactionType.SourceAccountTypeId,
                     CalculationTypeId = calculationType.Id,
                     CalculationTypeName = calculationType.Name,
                     AccountTransactionTypeId = calculationType.AccountTransactionType.Id
                 } into grp
                 select new MenuItemOrderAccountsInfo
                 {
                     OrderId = grp.Key.OrderId,
                     MenuItemName = grp.Key.MenuItemName,
                     GroupName = grp.Key.GroupCode,
                     AccountHead = grp.Key.CalculationTypeName,
                     SortOrder = 0,
                     Quantity = grp.Sum(y => y.Order.Quantity),
                     Amount = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId, false, false, false)),
                 };
            var result = menuItemInfoGroups.OrderBy(x => x.OrderId).OrderBy(x => x.AccountHead);
            DataSet dsMenGroupsAccountsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menuItemInfoGroups.ToList());
            return result;
        }
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateMenuItemOrderCalculationInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<CalculationType> calculationTypes)
        {
            DataTable dtAccountType = AccountManager.GetAccountType();
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                from calculationType in calculationTypes
                group c by new { MenuItemId = c.Order.MenuItemId, c.Order.MenuItemName,c.Order.PortionName,c.Order.Price, GroupCode = c.Order.MenuGroupName, calculationType.DecreaseAmount, calculationType.AccountTransactionType.TargetAccountTypeId, calculationType.AccountTransactionType.SourceAccountTypeId, CalculationTypeId = calculationType.Id, CalculationTypeName = calculationType.Name, AccountTransactionTypeId = calculationType.AccountTransactionType.Id } into grp
                select new MenuItemOrderAccountsInfo
                {
                    MenuItemId = grp.Key.MenuItemId,
                    MenuItemName = grp.Key.MenuItemName,
                    PortionName = grp.Key.PortionName,
                    Price = grp.Key.Price,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.CalculationTypeName,
                    SortOrder = Convert.ToInt32(dtAccountType.Select(string.Format("Id='{0}'", grp.Key.DecreaseAmount ? grp.Key.TargetAccountTypeId.ToString() : grp.Key.SourceAccountTypeId.ToString()))[0]["SortOrder"]),
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId)),
                };
            var result = menuItemInfoGroups.ToList().OrderBy(x => x.OrderId).OrderBy(x => x.AccountHead);
            DataSet dsMenGroupsAccountsInfo = DatasetConverter.ToDataSet<MenuItemOrderAccountsInfo>(menuItemInfoGroups.ToList());
            return result;
        }
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateMenuItemAccountsInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<TaxTemplate> taxTemplates)
        {
            DataTable dtAccountType = AccountManager.GetAccountType();
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))                
                from taxTemplate in taxTemplates
                group c by new { MenuItemId = c.Order.MenuItemId, c.Order.MenuItemName, c.Order.PortionName, c.Order.Price, GroupCode = c.Order.MenuGroupName, taxTemplate.AccountTransactionType.SourceAccountTypeId, AccountTransactionTypeId = taxTemplate.AccountTransactionType.Id, TaxTemplateName = taxTemplate.Name } into grp
                select new MenuItemOrderAccountsInfo
                {
                    MenuItemId = grp.Key.MenuItemId,
                    MenuItemName = grp.Key.MenuItemName,
                    PortionName = grp.Key.PortionName,
                    Price = grp.Key.Price,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.TaxTemplateName,
                    SortOrder = Convert.ToInt32(dtAccountType.Select(string.Format("Id='{0}'", grp.Key.SourceAccountTypeId.ToString()))[0]["SortOrder"]),
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculateTaxTotal(y.Ticket, y.Order, grp.Key.AccountTransactionTypeId)),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                    //AmountExcludingDiscount = grp.Sum(y => y.Sum(z => z.GetSum()) - y.Sum(z => z.CalculateTax(z.GetPlainSum(), y.GetPreTaxServicesTotal())) + y.Sum(z => z.GetPostTaxDiscountServicesTotal()) + y.Sum(z => z.GetPreTaxDiscountServicesTotal()) - x.Sum(y => y.GetPostTaxServicesTotal()) - x.Sum(y => y.GetPreTaxServicesTotal()))
                };

            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            foreach (Ticket tick in tickets)
            {
                decimal TaxValues = 0;
                foreach (Order Order in tick.Orders)
                {
                    foreach (TaxTemplate taxTemplate in taxTemplates)
                    {
                        TaxValues += Order.GetTotalTaxAmount(tick.TaxIncluded, tick.PlainSum, tick.PreTaxServicesTotal, taxTemplate.AccountTransactionType_Id);
                    }
                }
            }
            return result;
        }
        public static IEnumerable<MenuItemGroupAccountsInfo> CalculateMenuItemGroupAccountsInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<TaxTemplate> taxTemplates)
        {
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                from taxTemplate in taxTemplates
                group c by new { GroupCode = c.Order.MenuGroupName, taxTemplate.AccountTransactionType.SourceAccountTypeId, AccountTransactionTypeId = taxTemplate.AccountTransactionType.Id, TaxTemplateName = taxTemplate.Name } into grp
                select new MenuItemGroupAccountsInfo
                {
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.TaxTemplateName,
                    SortOrder = 0,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculateTaxTotal(y.Ticket, y.Order, grp.Key.AccountTransactionTypeId)),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };

            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            foreach (Ticket tick in tickets)
            {
                decimal TaxValues = 0;
                foreach (Order Order in tick.Orders)
                {
                    foreach (TaxTemplate taxTemplate in taxTemplates)
                    {
                        TaxValues += Order.GetTotalTaxAmount(tick.TaxIncluded, tick.PlainSum, tick.PreTaxServicesTotal, taxTemplate.AccountTransactionType.Id);
                    }
                }
            }
            return result;
        }
        public static IEnumerable<MenuItemGroupAccountsInfo> CalculateMenuItemGroupCalculationInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<CalculationType> calculationTypes)
        {
            DataTable dtAccountType = AccountManager.GetAccountType();
            CalculateMenuItemOrderGroupCalculationInfo(tickets, menuItems, calculationTypes);
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))                
                from calculationType in calculationTypes
                group c by new { GroupCode = c.Order.MenuGroupName, calculationType.DecreaseAmount , calculationType.IsServiceCharge, calculationType.IsDiscount, calculationType.IsSD, calculationType.IsTax, calculationType.AccountTransactionType.TargetAccountTypeId, calculationType.AccountTransactionType.SourceAccountTypeId , CalculationTypeId = calculationType.Id, CalculationTypeName = calculationType.Name, AccountTransactionTypeId = calculationType.AccountTransactionType.Id } into grp
                select new MenuItemGroupAccountsInfo
                {
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.CalculationTypeName,
                    IsServiceCharge = grp.Key.IsServiceCharge,
                    IsDiscount = grp.Key.IsDiscount,
                    IsTax = grp.Key.IsTax,
                    IsSD = grp.Key.IsSD,
                    SortOrder = Convert.ToInt32(dtAccountType.Select(string.Format("Id='{0}'", grp.Key.DecreaseAmount ? grp.Key.TargetAccountTypeId.ToString() : grp.Key.SourceAccountTypeId.ToString()))[0]["SortOrder"]),
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId, false, false,false)),
                    SDAmount = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId, true, false,false)),
                    ServiceChargeAmount = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId, false, false, true)),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };
            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            #region CheckSum
            //int ticketcount = 0;
            //foreach (Ticket tick in tickets)
            //{
            //    foreach (Calculation calculation in tick.Calculations)
            //    {
            //        decimal CalculationValues = 0;
            //        foreach (Order Order in tick.Orders)
            //        {
            //            CalculationValues += CalculateCalculationTotal(tick, Order, calculation.CalculationTypeId, calculation.AccountTransactionTypeId);
            //        }
            //        decimal temp = Math.Round(CalculationValues, 2);
            //        if (calculation.CalculationAmount != temp)
            //        {
            //            int x = 0;
            //        }
            //    }
            //    ticketcount++;
            //} 
            #endregion
            return result;
        }
        public static IEnumerable<MenuItemGroupAccountsInfo> CalculateTaxCalculationInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<CalculationType> calculationTypes)
        {
            CalculateMenuItemOrderGroupCalculationInfo(tickets, menuItems, calculationTypes);
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                from calculationType in calculationTypes.OrderBy(x => x.SortOrder)
                group c by new
                {
                    calculationType.DecreaseAmount,
                    calculationType.IsDiscount,
                    calculationType.IsTax,
                    calculationType.IsSD,
                    calculationType.IsServiceCharge,
                    calculationType.AccountTransactionType.TargetAccountTypeId,
                    calculationType.AccountTransactionType.SourceAccountTypeId,
                    CalculationTypeId = calculationType.Id,
                    CalculationTypeName = calculationType.Name,
                    AccountTransactionTypeId = calculationType.AccountTransactionType.Id
                } into grp
                select new MenuItemGroupAccountsInfo
                {
                    AccountHead = grp.Key.CalculationTypeName,
                    IsDiscount = grp.Key.IsDiscount,
                    IsSD = grp.Key.IsSD,
                    IsTax = grp.Key.IsTax,
                    IsServiceCharge = grp.Key.IsServiceCharge,
                    SortOrder = 0,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId, false, false, false)),
                    SDAmount = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId, true, false, false)),
                    ServiceChargeAmount = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId, false, false, true)),
                    OrderTotal = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId, false, true, false)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };
            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            return result;
        }
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateMenuItemCalculationInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<CalculationType> calculationTypes)
        {
            DataTable dtAccountType = AccountManager.GetAccountType();
            CalculateMenuItemOrderGroupCalculationInfo(tickets, menuItems, calculationTypes);
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                from calculationType in calculationTypes
                group c by new { MenuItemId = c.Order.MenuItemId, c.Order.MenuItemName,c.Order.PortionName, c.Order.Price, GroupCode = c.Order.MenuGroupName, calculationType.DecreaseAmount, calculationType.AccountTransactionType.TargetAccountTypeId, calculationType.AccountTransactionType.SourceAccountTypeId, CalculationTypeId = calculationType.Id, CalculationTypeName = calculationType.Name, AccountTransactionTypeId = calculationType.AccountTransactionType.Id } into grp
                select new MenuItemOrderAccountsInfo
                {
                    MenuItemId = grp.Key.MenuItemId,
                    MenuItemName = grp.Key.MenuItemName,
                    PortionName = grp.Key.PortionName,
                    Price = grp.Key.Price,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.CalculationTypeName,
                    SortOrder = Convert.ToInt32(dtAccountType.Select(string.Format("Id='{0}'", grp.Key.DecreaseAmount ? grp.Key.TargetAccountTypeId.ToString() : grp.Key.SourceAccountTypeId.ToString()))[0]["SortOrder"]),
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculateCalculationTotal(y.Ticket, y.Order, grp.Key.CalculationTypeId, grp.Key.AccountTransactionTypeId, false, false,false)),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };
            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            #region CheckSum
            //int ticketcount = 0;
            //foreach (Ticket tick in tickets)
            //{
            //    foreach (Calculation calculation in tick.Calculations)
            //    {
            //        decimal CalculationValues = 0;
            //        foreach (Order Order in tick.Orders)
            //        {
            //            CalculationValues += CalculateCalculationTotal(tick, Order, calculation.CalculationTypeId, calculation.AccountTransactionTypeId);
            //        }
            //        decimal temp = Math.Round(CalculationValues, 2);
            //        if (calculation.CalculationAmount != temp)
            //        {
            //            int x = 0;
            //        }
            //    }
            //    ticketcount++;
            //} 
            #endregion
            return result;
        }
        public static IEnumerable<MenuItemGroupAccountsInfo> CalculatePaymentInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<string> payments)
        {            
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))                
                from payment in payments
                group c by new { GroupCode = c.Order.MenuGroupName, PaymentName = payment} into grp
                select new MenuItemGroupAccountsInfo
                {
                    GroupName = grp.Key.GroupCode,
                    AccountHead = grp.Key.PaymentName,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculatePaymentTotal(y.Ticket, y.Order, grp.Key.PaymentName, 0)),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };
            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            int ticketcount = 0;
            foreach (Ticket tick in tickets)
            {
                foreach (string payment in payments)
                {
                    decimal CalculationValues = 0;
                    foreach (Order Order in tick.Orders)
                    {
                        CalculationValues += CalculatePaymentTotal(tick, Order, payment, 0);
                    }
                    decimal temp = Math.Round(CalculationValues, 2);
                    decimal TicketPaymentTotal = tick.Payments.Where(y => y.Name == payment).Sum(z => z.Amount);
                }
                ticketcount++;
            }
            return result;
        }
        public static IEnumerable<MenuItemGroupAccountsInfo> CalculatePaymentTotalInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, IEnumerable<string> payments)
        {
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))                
                group c by new { GroupCode = c.Order.MenuGroupName } into grp
                select new MenuItemGroupAccountsInfo
                {
                    GroupName = grp.Key.GroupCode,
                    AccountHead = "Total",
                    SortOrder = 2000,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => CalculateAllPaymentTotal(y.Ticket, y.Order)),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };
            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            return result;
        }
        public static IEnumerable<MenuItemGroupAccountsInfo> CalculateSalesInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {           
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))                
                group c by new {GroupCode = c.Order.MenuGroupName } into grp
                select new MenuItemGroupAccountsInfo
                {
                    GroupName = grp.Key.GroupCode,
                    AccountHead = "Sales",
                    SortOrder = -2,
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => y.Order.Total),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };
            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            return result;
        }
       
        public static IEnumerable<MenuItemGroupAccountsInfo> CalculateGiftInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory && !y.CalculatePrice).Select(y => new { Ticket = x, Order = y }))                
                group c by new { GroupCode = c.Order.MenuGroupName } into grp
                select new MenuItemGroupAccountsInfo
                {
                    GroupName = grp.Key.GroupCode,
                    AccountHead = "Gift",
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => y.Order.GetPriceValue),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };
            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            return result;
        }
        public static IEnumerable<MenuItemOrderAccountsInfo> CalculateItemGiftInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory && !y.CalculatePrice).Select(y => new { Ticket = x, Order = y }))
                group c by new { MenuItemId = c.Order.MenuItemId, c.Order.MenuItemName, c.Order.PortionName, c.Order.Price, GroupCode = c.Order.MenuGroupName } into grp
                select new MenuItemOrderAccountsInfo
                {
                    MenuItemId = grp.Key.MenuItemId,
                    MenuItemName = grp.Key.MenuItemName,
                    PortionName = grp.Key.PortionName,
                    Price = grp.Key.Price,
                    GroupName = grp.Key.GroupCode,
                    AccountHead = "Gift",
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => y.Order.GetPriceValue),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };
            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            return result;
        }
        public static IEnumerable<MenuItemGroupAccountsInfo> CalculateIncludingTaxInfo(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemInfoGroups =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))                
                group c by new { GroupCode = c.Order.MenuGroupName } into grp
                select new MenuItemGroupAccountsInfo
                {
                    GroupName = grp.Key.GroupCode,
                    AccountHead = "Inlcluded VAT",
                    Quantity = grp.Sum(y => y.Order.Quantity),
                    Amount = grp.Sum(y => y.Order.CalculateIncludingTax()),
                    OrderTotal = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)),
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };
            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);
            return result;
        }
        public static IEnumerable<MenuItemSpentTimeSellInfo> CalculateMenuItemsWithTimer(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by new { menuItem.Id, c.Order.PortionName, c.Order.Price } into grp
                select new MenuItemSpentTimeSellInfo { ID = grp.Key.Id, Quantity = grp.Sum(y => y.Order.Quantity), Portion = grp.Key.PortionName, Price = grp.Key.Price, TimeSpent = grp.Sum(y => CalculateOrderProductTimer(y.Ticket, y.Order)), Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)), AmountWithOrderTag = grp.Sum(y => CalculateOrderWithOrderTagTotal(y.Ticket, y.Order)) };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.Name);

            return result;
        }
        public static IEnumerable<DepartmentWiseSellInfo> CalculateDepartmentWiseSales(IEnumerable<Ticket> tickets, IEnumerable<Department> departments)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join department in departments on c.Order.DepartmentId equals department.Id
                group c by new { department.Id, department.Name } into grp
                select new DepartmentWiseSellInfo { ID = grp.Key.Id, Name = grp.Key.Name, Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)) };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.Name);

            return result;
        }
        public static IEnumerable<MenuItemSellInfo> CalculateMenuItems(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by new { menuItem.Id, c.Order.Price } into grp
                select new MenuItemSellInfo { ID = grp.Key.Id, Quantity = grp.Sum(y => y.Order.Quantity), Price = grp.Key.Price, Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)) };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.Name);

            return result;
        }
        public static IEnumerable<CustomerAdvanceDueInfo> CalculateCustomerAdvanceDues(IEnumerable<AccountTransactionType> trantypes, IEnumerable<AccountTransactionDocument> trandocs)
        {
            var customeradvancedueinfo =
                from c in trandocs.SelectMany(x => x.AccountTransactions.Where(z => z.TargetAccountTypeId == 3 && (z.AccountTransactionTypeId == 1 || z.AccountTransactionTypeId == 2)).Select(y => new { Doc = x, Tran = y }))
                    //join tranvalue in tranvalues.Where(x => x.AccountId == 5 || x.AccountId == 6) on c.Doc.Id equals tranvalue.AccountTransactionDocumentId
                join trantype in trantypes on c.Tran.AccountTransactionTypeId equals trantype.Id
                group c by new { trantype.Id, trantype.Name } into grp
                select new CustomerAdvanceDueInfo { ID = grp.Key.Id, Name = grp.Key.Name, Amount = grp.Sum(y => y.Tran.Amount) };

            var result = customeradvancedueinfo.ToList().OrderByDescending(x => x.Name);

            return result;
        }
        public static IEnumerable<MenuItemSellInfo> CalculateReturnedItems(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.IncreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by menuItem.Id into grp
                select new MenuItemSellInfo { ID = grp.Key, Quantity = grp.Sum(y => y.Order.Quantity), Price = grp.Max(y => GetPrice(y.Ticket, y.Order)), Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)) };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.Name);

            return result;
        }
        public static decimal CalculateOrderPlainTotal(Ticket ticket, Order order)
        {
            decimal nTotal = 0;
            nTotal = order.GetPlainTotal();
            return nTotal;
        }
        public static decimal CalculateOrderTotal(Ticket ticket, Order order)
        {
            var discount = ticket.PreTaxServicesTotal;
            if (discount != 0)
            {
                var tsum = ticket.GetPlainSum();
                var rate = tsum > 0 ? (discount * 100) / tsum : 100;
                var tiTotal = order.GetTotal();
                var itemDiscount = (tiTotal * rate) / 100;
                return tiTotal + itemDiscount;
            }
            decimal nTotal = 0;
            nTotal = order.GetTotal();
            return nTotal;
        }
        public static decimal CalculateOrderWithOrderTagTotal(Ticket ticket, Order order)
        {           
            var tsum = ticket.GetPlainSum();
            var tiTotal = order.GetTotal();
            return tiTotal;           
        }
        //Old before Selective Included Calculations
        public static decimal CalculateCalculationTotal(Ticket ticket, Order order, int CalculationTypeId, int AccountTransactionTypeId)
        {
            decimal calculationAmount = 0;
            var calculation = ticket.Calculations.SingleOrDefault(x => x.CalculationTypeId == CalculationTypeId);
            if (calculation != null && calculation.Amount > 0)
            {
                decimal PlainSum = ticket.Orders.Where(y => y.GetCalculaionSum(calculation, ticket.ZoneId)).Sum(z => z.Total);
                if (order.GetCalculaionSum(calculation, ticket.ZoneId))
                    calculationAmount = (order.Total / PlainSum) * calculation.CalculationAmount;
            }
            return calculationAmount;
        }
        public static decimal CalculateCalculationTotal(Ticket ticket, Order order, int CalculationTypeId, int AccountTransactionTypeId, bool CalculateSD, bool CalculateSum, bool CalculatedServiceCharge)
        {
            decimal calculationAmount = 0;
            var calculation = ticket.Calculations.SingleOrDefault(x => x.CalculationTypeId == CalculationTypeId);
            if (calculation != null && calculation.Amount > 0)
            {
                calculationAmount = ticket.CalculateService(ticket.Calculations, calculation, order, CalculateSD, CalculatedServiceCharge, CalculateSum);
                decimal PlainSum = ticket.Orders.Where(y => y.GetCalculaionSum(calculation, ticket.ZoneId)).Sum(z => z.Total);
                if (order.GetCalculaionSum(calculation, ticket.ZoneId) && calculation.CalculationType == 4)
                    calculationAmount = (order.Total / PlainSum) * calculation.CalculationAmount;
            }
            return calculationAmount;
        }
        public static decimal CalculatePaymentTotal(Ticket ticket, Order order, string paymentName, decimal OrderTotal)
        {
            if (ticket.PlainSum == 0) return 0;
            var payments = ticket.Payments.Where(x => x.Name == paymentName);
            decimal calculationAmount = 0;
            foreach (Payment payment in payments)
            {
                //if (payment != null && payment.Amount > 0) // Edited by badhon : for Menuitem Accounts Group wise report not deducting refunded amount.
                if (payment != null)
                {
                    calculationAmount += (OrderTotal / ticket.TotalAmount) * payment.Amount;
                }
            }
            return calculationAmount;
        }
        public static decimal CalculateAllPaymentTotal(Ticket ticket, Order order)
        {
            if (ticket.PlainSum == 0) return 0;
            var payments = ticket.Payments.Where(x => x.Name != "");
            decimal calculationAmount = 0;
            foreach (Payment payment in payments)
            {
                if (payment != null && payment.Amount > 0)
                {
                    calculationAmount += (order.Total / ticket.PlainSum) * payment.Amount;
                }
            }
            return calculationAmount;
        }
        public static decimal CalculateOrderProductTimer(Ticket ti, Order order)
        {
            decimal price = ti.GetActiveTimerClosedAmount();
            if (price != 0)
            {

            }
            return order.GetActiveTimerAmount();
        }
        public static decimal GetPrice(Ticket ticket, Order order)
        {
            decimal nTotal = 0;
            nTotal = order.GetPrice();
            return nTotal;
        }
    }
}