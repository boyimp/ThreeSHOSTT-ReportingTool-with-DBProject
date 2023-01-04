using System.Collections.Generic;
using System.Linq;
using ThreeS.Domain.Models.Accounts;
using ThreeS.Domain.Models.Menus;
using ThreeS.Domain.Models.Tickets;

namespace ThreeS.Modules.BasicReports.Reports
{
    public static class MenuGroupBuilder
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
                    GrossAmount = grp.Sum(y => CalculateOrderPlainTotal(y.Ticket, y.Order))
                };

            var result = menuItemInfoGroups.ToList().OrderByDescending(x => x.Amount);

            var sum = menuItemInfoGroups.Sum(x => x.Amount);
            foreach (var menuItemInfoGroup in result)
            {
                if (sum > 0)
                    menuItemInfoGroup.Rate = (menuItemInfoGroup.Amount * 100) / sum;
                if (string.IsNullOrEmpty(menuItemInfoGroup.GroupName))
                    menuItemInfoGroup.GroupName ="" ;//Localization.Properties.Resources.UndefinedWithBrackets;
            }

            var qsum = menuItemInfoGroups.Sum(x => x.Quantity);
            foreach (var menuItemInfoGroup in result)
            {
                if (qsum > 0)
                    menuItemInfoGroup.QuantityRate = (menuItemInfoGroup.Quantity * 100) / qsum;
                if (string.IsNullOrEmpty(menuItemInfoGroup.GroupName))
                    menuItemInfoGroup.GroupName = "";// Localization.Properties.Resources.UndefinedWithBrackets;
            }

            return result;
        }

        public static IEnumerable<MenuItemSpentTimeSellInfo> CalculateMenuItemsWithTimer(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by new { menuItem.Id, c.Order.Price, c.Order.DepartmentId, c.Order.PortionName,} into grp
                select new MenuItemSpentTimeSellInfo { ID = grp.Key.Id, Quantity = grp.Sum(y => y.Order.Quantity), Price = grp.Key.Price, DepartmentID = grp.Key.DepartmentId, Portion = grp.Key.PortionName, TimeSpent = grp.Sum(y => CalculateOrderProductTimer(y.Ticket, y.Order)), Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order))
                ,AmountWithOrderTag = grp.Sum(y => CalculateOrderWithOrderTagTotal(y.Ticket, y.Order))
                };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.ID);

            return result;
        }
        public static IEnumerable<MenuItemSpentTimeSellInfo> CalculateMenuItemsWithTimerAllDepartment(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by new { menuItem.Id, c.Order.Price, c.Order.PortionName, } into grp
                select new MenuItemSpentTimeSellInfo { ID = grp.Key.Id, Quantity = grp.Sum(y => y.Order.Quantity), Price = grp.Key.Price, DepartmentID = 0, Portion = grp.Key.PortionName, TimeSpent = grp.Sum(y => CalculateOrderProductTimer(y.Ticket, y.Order)), Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order))
                ,AmountWithOrderTag = grp.Sum(y => CalculateOrderWithOrderTagTotal(y.Ticket, y.Order))
                };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.ID);

            return result;
        }
        //
        public static IEnumerable<MenuItemSpentTimeSellInfo> CalculateMenuItemsWithTimerWithDepartment(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, int departmentID)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory && y.DepartmentId == departmentID).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by new { menuItem.Id, c.Order.Price, c.Order.DepartmentId, c.Order.PortionName, } into grp
                select new MenuItemSpentTimeSellInfo { ID = grp.Key.Id, Quantity = grp.Sum(y => y.Order.Quantity), Price = grp.Key.Price, DepartmentID = grp.Key.DepartmentId, Portion = grp.Key.PortionName, TimeSpent = grp.Sum(y => CalculateOrderProductTimer(y.Ticket, y.Order)), Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order))
                ,AmountWithOrderTag = grp.Sum(y => CalculateOrderWithOrderTagTotal(y.Ticket, y.Order))
                };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.ID);

            return result;
        }
        //
        // Clue for department wise item wise sales
        public static IEnumerable<WaiterMenuItemSpentTimeSellInfo> CalculateMenuItemsWithTimerWaiterWise(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by new { c.Ticket.Waiter, menuItem.Id, c.Order.Price, c.Order.DepartmentId, c.Order.PortionName, } into grp
                select new WaiterMenuItemSpentTimeSellInfo { ID = grp.Key.Id, Waiter = grp.Key.Waiter, Quantity = grp.Sum(y => y.Order.Quantity), Price = grp.Key.Price, DepartmentID = grp.Key.DepartmentId, Portion = grp.Key.PortionName, TimeSpent = grp.Sum(y => CalculateOrderProductTimer(y.Ticket, y.Order)), Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)) };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.ID);

            return result;
        }
        //
        public static IEnumerable<WaiterMenuItemSpentTimeSellInfo> CalculateMenuItemsWithTimerWithDepartmentWaiterWise(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems, int departmentID)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory && y.DepartmentId == departmentID).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by new { c.Ticket.Waiter, menuItem.Id, c.Order.Price, c.Order.DepartmentId, c.Order.PortionName, } into grp
                select new WaiterMenuItemSpentTimeSellInfo { ID = grp.Key.Id, Waiter = grp.Key.Waiter, Quantity = grp.Sum(y => y.Order.Quantity), Price = grp.Key.Price, Portion = grp.Key.PortionName, DepartmentID = grp.Key.DepartmentId, TimeSpent = grp.Sum(y => CalculateOrderProductTimer(y.Ticket, y.Order)), Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)) };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.ID);

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
       
        //
        public static IEnumerable<MenuItemSellInfo> CalculateMenuItems(IEnumerable<Ticket> tickets, IEnumerable<MenuItem> menuItems)
        {
            var menuItemSellInfos =
                from c in tickets.SelectMany(x => x.Orders.Where(y => y.DecreaseInventory).Select(y => new { Ticket = x, Order = y }))
                join menuItem in menuItems on c.Order.MenuItemId equals menuItem.Id
                group c by new{menuItem.Id, c.Order.Price} into grp
                select new MenuItemSellInfo { ID = grp.Key.Id, Quantity = grp.Sum(y => y.Order.Quantity), Price = grp.Key.Price, Amount = grp.Sum(y => CalculateOrderTotal(y.Ticket, y.Order)) };

            var result = menuItemSellInfos.ToList().OrderByDescending(x => x.Name);

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
            var discount = ticket.GetPreTaxServicesTotal();
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