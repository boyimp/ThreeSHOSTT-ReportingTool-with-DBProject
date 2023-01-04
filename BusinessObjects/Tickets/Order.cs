using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using ThreeS.Domain.Models.Menus;
using ThreeS.Infrastructure.Data;
using ThreeS.Infrastructure.Helpers;
using System.Runtime.Serialization;
using BusinessObjects;

namespace ThreeS.Domain.Models.Tickets
{
    [DataContract]
    public class OrderOnAir //: IEquatable<OrderOnAir>
    {
        public OrderOnAir()
        {
            CreatedDateTime = Convert.ToString(DateTime.Now);
            MenuItemId = 0;
            Quantity = 0;
        }
        [DataMember(Name = "MenuItemId")]
        public int MenuItemId { get; set; }
        [DataMember(Name = "PortionName")]
        public string PortionName { get; set; }
        [DataMember(Name = "Quantity")]
        public decimal Quantity { get; set; }
        [DataMember(Name = "PortionCount")]
        public int PortionCount { get; set; }
        [DataMember(Name = "CreatingUserName")]
        public string CreatingUserName { get; set; }
        [DataMember(Name = "CreatedDateTime")]
        public string CreatedDateTime { get; set; }

        private static OrderOnAir _default;
        public static OrderOnAir Default
        {
            get { return _default ?? (_default = new OrderOnAir()); }
        }

        public bool Equals(OrderOnAir other)
        {
            if (other == null) return false;
            return other.PortionName == PortionName && other.CreatingUserName == CreatingUserName;
        }

        public override int GetHashCode()
        {
            return (PortionName + "_" + CreatingUserName).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var item = obj as OrderOnAir;
            return item != null && Equals(item);
        }
    }

    public class Order : BusinessObjects.ValueClass
    {
        public Order()
        {
            _selectedQuantity = 0;
            CreatedDateTime = DateTime.Now;
            CalculatePrice = true;
            DecreaseInventory = true;
            _ticketServiceAmount = 0;
            _ticketSum = 0;
            _ticketServiceAmount = 0;
            _ticketSum = 0;
        }


        public bool IsSelected { get; set; } // Not Stored
        public string MenuGroupName { get; set; } // Not Stored
        public int TicketId { get; set; }
        public int WarehouseId { get; set; }
        public int DepartmentId { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string PortionName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public int PortionCount { get; set; }
        public bool Locked { get; set; }
        public bool CalculatePrice { get; set; }
        public bool DecreaseInventory { get; set; }
        public bool IncreaseInventory { get; set; }
        public int OrderNumber { get; set; }
        public string CreatingUserName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int AccountTransactionTypeId { get; set; }
        public int? ProductTimerValueId { get; set; }
        public virtual ProductTimerValue ProductTimerValue { get; set; }
        /**/
        public decimal ActiveTimerAmount { get; set; }
        public decimal PlainTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Value { get; set; }
        public decimal GetPriceValue { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal GrossUnitPrice { get; set; }
        public decimal OrderTagPrice { get; set; }
        public decimal UnitProductionCost { get; set; }
        public decimal OrderPriceIncludingPreTaxService { get; set; }

        private string _calculationTypes;
        public string CalculationTypes
        {
            get { return _calculationTypes; }
            set
            {
                _calculationTypes = value;
                _calculationTypeValues = null;
            }
        }
        private IList<OrderCalculationTypeValue> _orderCalculationTypeValues;
        internal IList<OrderCalculationTypeValue> OrderCalculationTypeValues
        {
            get { return _orderCalculationTypeValues ?? (_orderCalculationTypeValues = JsonHelper.Deserialize<List<OrderCalculationTypeValue>>(OrderCalculationTypes)); }
        }

        private string _orderCalculationTypes;
        public string OrderCalculationTypes
        {
            get { return _orderCalculationTypes; }
            set
            {
                _orderCalculationTypes = value;
                _orderCalculationTypeValues = null;
            }
        }
        /**/
        private IList<CalculationTypeValue> _calculationTypeValues;
        internal IList<CalculationTypeValue> CalculationTypeValues
        {
            get { return _calculationTypeValues ?? (_calculationTypeValues = JsonHelper.Deserialize<List<CalculationTypeValue>>(CalculationTypes)); }
        }

        //private IList<OrderTagValue> _orderTagValues;       

        public string PriceTag { get; set; }
        public string Tag { get; set; }

        private string _taxes;
        public string Taxes
        {
            get { return _taxes; }
            set
            {
                _taxes = value;
                _taxValues = null;
            }
        }

        private string _orderTags;
        public string OrderTags
        {
            get { return _orderTags; }
            set
            {
                _orderTags = value;
                _orderTagValues = null;
            }
        }

        private string _orderStates;
        public string OrderStates
        {
            get { return _orderStates; }
            set
            {
                _orderStates = value;
                _orderStateValues = null;
            }
        }

        private decimal _selectedQuantity;
        public decimal SelectedQuantity
        {
            get { return _selectedQuantity; }
        }

        public string Description
        {
            get
            {
                var desc = MenuItemName + GetPortionDesc();
                if (SelectedQuantity > 0 && SelectedQuantity != Quantity)
                    desc = string.Format("({0:#.##}) {1}", SelectedQuantity, desc);
                return desc;
            }
        }

        private decimal _ticketServiceAmount;
        public decimal TicketServiceAmount
        {
            get
            { return _ticketServiceAmount; }

            //set
            //{ _ticketServiceAmount = value; }
        }

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get
            { return _totalAmount; }

            //set
            //{ _ticketSum = value; }
        }
        private decimal _ticketSum;
        public decimal TicketSum
        {
            get
            { return _ticketSum; }

            //set
            //{ _ticketSum = value; }
        }

        private IList<OrderTagValue> _orderTagValues;
        public IList<OrderTagValue> OrderTagValues
        {
            get { return _orderTagValues ?? (_orderTagValues = JsonHelper.Deserialize<List<OrderTagValue>>(OrderTags)); }
        }


        private IList<OrderStateValue> _orderStateValues;
        public IList<OrderStateValue> OrderStateValues
        {
            get { return _orderStateValues ?? (_orderStateValues = JsonHelper.Deserialize<List<OrderStateValue>>(OrderStates)); }
        }


        private IList<TaxValue> _taxValues;
        public IList<TaxValue> TaxValues
        {
            get { return _taxValues ?? (_taxValues = JsonHelper.Deserialize<List<TaxValue>>(Taxes)); }
        }

        public bool OrderTagExists(Func<OrderTagValue, bool> prediction)
        {
            return OrderTagValues.Any(prediction);
        }

        public IEnumerable<OrderTagValue> GetOrderTagValues(Func<OrderTagValue, bool> prediction)
        {
            return OrderTagValues.Where(prediction);
        }

        public IEnumerable<OrderTagValue> GetOrderTagValues()
        {
            return OrderTagValues;
        }

        private static Order _null;
        public static Order Null { get { return _null ?? (_null = new Order { ProductTimerValue = new ProductTimerValue() }); } }

        public void UpdateMenuItem(string userName, MenuItem menuItem, IEnumerable<TaxTemplate> taxTemplates, MenuItemPortion portion, string priceTag, int quantity)
        {
            MenuItemId = menuItem.Id;
            MenuItemName = menuItem.Name;
            Debug.Assert(portion != null);
            UpdatePortion(portion, priceTag, taxTemplates);
            Quantity = quantity;
            _selectedQuantity = quantity;
            PortionCount = menuItem.Portions.Count;
            CreatingUserName = userName;
            CreatedDateTime = DateTime.Now;
        }

        public void RecalculateOrderProperties()
        {
            ActiveTimerAmount = GetActiveTimerAmount();
            PlainTotal = GetPlainTotal();
            Total = GetTotal();
            Value = GetValue();
            GetPriceValue = GetPrice();
            GrossPrice = GetGrossPrice();
            GrossUnitPrice = GetGrossUnitPrice();
            OrderTagPrice = GetOrderTagPrice();
        }
        public decimal CalculateIncludingTax()
        {
            decimal dIncludingTax = 0;
            if ((TaxValues.Where(z => z.TaxTemplateName.ToLower().Contains("vat")) == null || TaxValues.Where(z => z.TaxTemplateName.ToLower().Contains("vat")).Count() == 0) && (decimal)DBUtility.GetVATIncludingDenominator() > 0)
            {
                dIncludingTax = GetValue() / (decimal)DBUtility.GetVATIncludingDenominator();
            }
            return dIncludingTax;
        }

        public void SetOrderCalculationTypeValue(int calculationTypeId, string calculationName, bool isSD, bool isDiscount, bool isTax, bool isSC, decimal price, decimal calculationAmount, string calculationKey)
        {
            var cv = OrderCalculationTypeValues.SingleOrDefault(x => x.CalculationTypeID == calculationTypeId);
            if (cv == null && calculationAmount != 0)
            {
                cv = new OrderCalculationTypeValue
                {
                    CalculationTypeID = calculationTypeId,
                    CalculationName = calculationName,
                    Price = price,
                    IsSC = isSC,
                    IsTax = isTax,
                    IsDiscount = isDiscount,
                    IsSD = isSD,
                    CalculationAmount = calculationAmount,
                    CalculationKey = calculationKey
                };
                OrderCalculationTypeValues.Add(cv);
            }
            else
                OrderCalculationTypeValues.Remove(cv);

            OrderCalculationTypes = JsonHelper.Serialize(OrderCalculationTypeValues);
            _orderCalculationTypeValues = null;
        }
        public bool GetCalculaionSum(Calculation calculation, int ZoneId)
        {
            if (!(this.DecreaseInventory || this.IncreaseInventory)) return false;
            var menuItemGroupCode = this.MenuGroupName;

            var maps = calculation.CalculationTypeMaps.ToList();

            maps = maps.Count(x => x.MenuItemGroupCode == menuItemGroupCode) > 0
                       ? maps.Where(x => x.MenuItemGroupCode == menuItemGroupCode).ToList()
                       : maps.Where(x => x.MenuItemGroupCode == null).ToList();

            maps = maps.Count(x => x.MenuItemId == this.MenuItemId) > 0
                       ? maps.Where(x => x.MenuItemId == this.MenuItemId).ToList()
                       : maps.Where(x => x.MenuItemId == 0).ToList();
            maps = maps.Count(x => x.ZoneId == ZoneId) > 0
                       ? maps.Where(x => x.ZoneId == ZoneId || x.ZoneId == 0).ToList()
                       : maps.Where(x => x.ZoneId == 0).ToList();
            if (maps.FirstOrDefault() == null || !this.CalculatePrice) return false;
            else
            {
                if (calculation.Dynamic)
                {
                    CalculationTypeValue oCValue = CalculationTypeValues.SingleOrDefault(x => x.ID == calculation.CalculationTypeId);
                    if (oCValue != null)
                        return true;
                    else return false;
                }
            }
            return true;
        }
        public void UpdatePortion(MenuItemPortion portion, string priceTag, IEnumerable<TaxTemplate> taxTemplates)
        {
            PortionName = portion.Name;

            UpdateTaxTemplates(taxTemplates);

            if (!string.IsNullOrEmpty(priceTag))
            {
                string tag = priceTag;
                var price = portion.Prices.SingleOrDefault(x => x.PriceTag == tag);
                if (price != null && price.Price > 0)
                {
                    UpdatePrice(price.Price, price.PriceTag);
                }
                else priceTag = "";
            }

            if (string.IsNullOrEmpty(priceTag))
            {
                UpdatePrice(portion.Price, "");
            }

            if (OrderTagValues.Any(x => x.MenuItemId > 0 && x.PortionName != portion.Name))
            {
                foreach (var orderTagValue in OrderTagValues.Where(x => x.MenuItemId > 0))
                {
                    orderTagValue.PortionName = portion.Name;
                }
                OrderTags = JsonHelper.Serialize(OrderTagValues);
                _orderTagValues = null;
            }
        }

        public void TagIfNotTagged(OrderTagGroup orderTagGroup, OrderTag orderTag, int userId, string tagNote)
        {
            if (OrderTagValues.FirstOrDefault(x => x.OrderTagGroupId == orderTagGroup.Id && x.TagValue == orderTag.Name) == null)
            {
                ToggleOrderTag(orderTagGroup, orderTag, userId, tagNote);
            }
        }

        public bool UntagIfTagged(OrderTagGroup orderTagGroup, OrderTag orderTag)
        {
            var value = OrderTagValues.FirstOrDefault(x => x.OrderTagGroupId == orderTagGroup.Id && x.TagValue == orderTag.Name);
            if (value != null)
            {
                UntagOrder(value);
                return true;
            }
            return false;
        }

        private void TagOrder(OrderTagGroup orderTagGroup, OrderTag orderTag, int userId, int tagIndex, string tagNote)
        {
            var otag = new OrderTagValue
            {
                TagValue = orderTag.Name,
                OrderTagGroupId = orderTagGroup.Id,
                TagName = orderTagGroup.Name,
                TagNote = !string.IsNullOrEmpty(tagNote) ? tagNote : null,
                MenuItemId = orderTag.MenuItemId,
                AddTagPriceToOrderPrice = orderTagGroup.AddTagPriceToOrderPrice,
                PortionName = orderTag.MenuItemId > 0 ? PortionName : null,
                UserId = userId,
                Quantity = 1,
                OrderKey = orderTagGroup.SortOrder.ToString("000") + orderTag.SortOrder.ToString("000")
            };

            otag.UpdatePrice(orderTag.Price);

            if (tagIndex > -1)
                OrderTagValues.Insert(tagIndex, otag);
            else
                OrderTagValues.Add(otag);
            OrderTags = JsonHelper.Serialize(OrderTagValues);
            _orderTagValues = null;
        }

        public void UntagOrder(OrderTagValue orderTagValue)
        {
            OrderTagValues.Remove(orderTagValue);
            OrderTags = JsonHelper.Serialize(OrderTagValues);
            _orderTagValues = null;
        }

        public bool ToggleOrderTag(OrderTagGroup orderTagGroup, OrderTag orderTag, int userId, string tagNote)
        {
            var result = true;
            var otag = OrderTagValues.FirstOrDefault(x => x.TagValue == orderTag.Name);
            if (otag == null)
            {
                if (orderTagGroup.MaxSelectedItems > 1 && OrderTagValues.Count(x => x.OrderTagGroupId == orderTagGroup.Id) >= orderTagGroup.MaxSelectedItems) return false;
                var tagIndex = -1;
                if (orderTagGroup.MaxSelectedItems == 1)
                {
                    var sTag = OrderTagValues.SingleOrDefault(x => x.OrderTagGroupId == orderTag.OrderTagGroupId);
                    if (sTag != null) tagIndex = OrderTagValues.IndexOf(sTag);
                    OrderTagValues.Where(x => x.OrderTagGroupId == orderTagGroup.Id).ToList().ForEach(x => OrderTagValues.Remove(x));
                }
                TagOrder(orderTagGroup, orderTag, userId, tagIndex, tagNote);
            }
            else
            {
                otag.Quantity++;
                if (orderTagGroup.MaxSelectedItems == 1 || (orderTag.MaxQuantity > 0 && otag.Quantity > orderTag.MaxQuantity))
                {
                    UntagOrder(otag);
                    result = false;
                }
            }
            return result;
        }

        public bool IsTaggedWith(OrderTag orderTag)
        {
            return OrderTagValues.Any(x => x.TagValue == orderTag.Name);
        }

        public bool IsTaggedWith(OrderTagGroup orderTagGroup)
        {
            return OrderTagValues.FirstOrDefault(x => x.OrderTagGroupId == orderTagGroup.Id) != null;
        }

        public OrderStateValue GetStateValue(string groupName)
        {
            return OrderStateValues.SingleOrDefault(x => x.StateName == groupName) ?? OrderStateValue.Default;
        }

        public void SetStateValue(string groupName, int groupOrder, string state, int stateOrder, string stateValue)
        {
            var sv = OrderStateValues.SingleOrDefault(x => x.StateName == groupName);
            if (sv == null)
            {
                sv = new OrderStateValue { StateName = groupName, State = state, StateValue = stateValue };
                OrderStateValues.Add(sv);
            }
            else
            {
                sv.State = state;
                sv.StateValue = stateValue;
            }

            sv.OrderKey = groupOrder.ToString("000") + stateOrder.ToString("000");

            if (string.IsNullOrEmpty(sv.State))
                OrderStateValues.Remove(sv);

            OrderStates = JsonHelper.Serialize(OrderStateValues);
            _orderStateValues = null;
        }

        public string GetStateDesc()
        {
            return string.Join(",",
                           OrderStateValues.OrderBy(x => x.OrderKey).Where(x => !string.IsNullOrEmpty(x.State)).Select(
                               x =>
                               string.Format("{0} {1}", x.State.Trim(),
                                             !string.IsNullOrEmpty(x.StateValue)
                                                 ? string.Format(":{0}", x.StateValue.Trim())
                                                 : "")));
        }

        public string GetStateData()
        {
            return string.Join("\r", OrderStateValues.Where(x => !string.IsNullOrEmpty(x.State)).OrderBy(x => x.OrderKey).Select(x => string.Format("{0} {1}", x.State, !string.IsNullOrEmpty(x.StateValue) ? string.Format("[{0}]", x.StateValue) : "")));
        }

        public decimal GetOrderTagPrice()
        {
            return GetOrderTagSum(OrderTagValues.Where(x => !x.AddTagPriceToOrderPrice));
        }

        private static decimal GetOrderTagSum(IEnumerable<OrderTagValue> orderTags)
        {
            return orderTags.Sum(orderTag => orderTag.Price * orderTag.Quantity);
        }

        public void IncSelectedQuantity()
        {
            _selectedQuantity++;
            if (SelectedQuantity > Quantity) _selectedQuantity = 1;
        }

        public void DecSelectedQuantity()
        {
            _selectedQuantity--;
            if (SelectedQuantity < 1) _selectedQuantity = 1;
        }

        public void ResetSelectedQuantity()
        {
            _selectedQuantity = Quantity;
        }

        public string GetPortionDesc()
        {
            if (PortionCount > 1
                && !string.IsNullOrEmpty(PortionName)
                && !string.IsNullOrEmpty(PortionName.Trim('\b', ' ', '\t'))
                && PortionName.ToLower() != "normal")
                return "." + PortionName;
            return "";
        }

        public void UpdatePrice(decimal value, string priceTag)
        {
            Price = value;
            PriceTag = priceTag;
        }

        public void UpdateTaxTemplates(IEnumerable<TaxTemplate> taxTemplates)
        {
            if (taxTemplates == null) return;
            TaxValues.Clear();
            foreach (var template in taxTemplates)
            {
                TaxValues.Add(new TaxValue(template));
            }
            Taxes = JsonHelper.Serialize(TaxValues);
            _taxValues = null;
        }

        public decimal GetTotalTaxAmount(bool taxIncluded, decimal plainSum, decimal preTaxServices)
        {
            var result = CalculatePrice ? TaxValues.Sum(x => x.GetTaxAmount(taxIncluded, GetPrice(), TaxValues.Sum(y => y.TaxRate), plainSum, preTaxServices)) * Quantity : 0;
            return decimal.Round(result, 4, MidpointRounding.AwayFromZero);
        }
        public decimal GetTotalTaxAmountUpdated(bool taxIncluded, decimal plainSum, decimal preTaxServicePrice)
        {
            var result = CalculatePrice ? TaxValues.Sum(x => x.GetTaxAmountUpdated(taxIncluded, preTaxServicePrice, TaxValues.Sum(y => y.TaxRate))) * Quantity : 0;
            return decimal.Round(result, 4, MidpointRounding.AwayFromZero);
        }
        public decimal GetTotalVATAmount(bool taxIncluded, decimal plainSum, decimal preTaxServices)
        {
            var result = CalculatePrice ? TaxValues.Where(x => x.TaxTemplateName.ToUpper().Contains("VAT")).Sum(x => x.GetTaxAmount(taxIncluded, GetPrice(), TaxValues.Sum(y => y.TaxRate), plainSum, preTaxServices)) * Quantity : 0;
            return decimal.Round(result, 4, MidpointRounding.AwayFromZero);
        }
        public decimal GetTotalServiceChargeAmount(bool taxIncluded, decimal plainSum, decimal preTaxServices)
        {
            var result = CalculatePrice ? TaxValues.Where(x => x.TaxTemplateName.Contains("Service Charge")).Sum(x => x.GetTaxAmount(taxIncluded, GetPrice(), TaxValues.Sum(y => y.TaxRate), plainSum, preTaxServices)) * Quantity : 0;
            return decimal.Round(result, 4, MidpointRounding.AwayFromZero);
        }
        public decimal GetTotalTaxAmount(bool taxIncluded, decimal plainSum, decimal preTaxServices, int taxTemplateId)
        {
            var result = CalculatePrice ? TaxValues.Where(x => x.TaxTempleteAccountTransactionTypeId == taxTemplateId)
                .Sum(x => x.GetTaxAmount(taxIncluded, GetPrice(), TaxValues.Sum(y => y.TaxRate), plainSum, preTaxServices)) * Quantity : 0;
            return result;
        }
        public decimal GetTotalTaxAmountUpdated(bool taxIncluded, decimal plainSum, decimal preTaxServicePrice, int taxTemplateId)
        {
            var result = CalculatePrice ? TaxValues.Where(x => x.TaxTempleteAccountTransactionTypeId == taxTemplateId)
                .Sum(x => x.GetTaxAmountUpdated(taxIncluded, preTaxServicePrice, TaxValues.Sum(y => y.TaxRate))) * Quantity : 0;
            return result;
        }
        public OrderTagValue GetOrderTagValue(string s)
        {
            if (OrderTagValues.Any(x => x.TagName == s))
                return OrderTagValues.First(x => x.TagName == s);
            return OrderTagValue.Empty;
        }

        public string OrderKey { get { return string.Join("", OrderTagValues.OrderBy(x => x.OrderKey).Select(x => x.OrderKey)); } }

        public void UpdateProductTimer(ProductTimer timer)
        {
            if (timer != null)
            {
                ProductTimerValue = new ProductTimerValue
                {
                    ProductTimerId = timer.Id,
                    MinTime = timer.MinTime,
                    PriceType = timer.PriceType,
                    PriceDuration = timer.PriceDuration,
                    TimeRounding = timer.TimeRounding,
                };
            }
        }

        public void SetServiceAmountAndSumOfTicket(decimal totalamount, decimal sum)
        {
            _totalAmount = totalamount;
            _ticketSum = sum;
        }

        public decimal GetGrossPrice()
        {
            if (_ticketSum == 0 || _totalAmount == 0) return 0;
            var result = (GetValue() / _ticketSum) * _totalAmount;
            return Math.Round(result);
        }

        public decimal GetGrossUnitPrice()
        {
            if (_ticketSum == 0 || _totalAmount == 0) return 0;
            var result = ((GetValue() / _ticketSum) * _totalAmount) / Quantity;
            return Math.Round(result);
        }

        public void StopProductTimer()
        {
            if (ProductTimerValue != null)
                ProductTimerValue.Stop();
        }

        public bool IsInState(string stateName, string state)
        {
            if (stateName == "*") return OrderStateValues.Any(x => x.State == state);
            if (string.IsNullOrEmpty(state)) return OrderStateValues.All(x => x.StateName != stateName);
            return OrderStateValues.Any(x => x.StateName == stateName && x.State == state);
        }
        public bool IsInState(string stateName, string state, string stateValue)
        {
            if (stateName == "*") return OrderStateValues.Any(x => x.State == state);
            if (string.IsNullOrEmpty(state)) return OrderStateValues.All(x => x.StateName != stateName);
            return OrderStateValues.Any(x => x.StateName == stateName && x.State == state && x.StateValue == stateValue);
        }

        public bool IsInState(string stateValue)
        {
            return OrderStateValues.Any(x => x.StateValue == stateValue);
        }

        public IEnumerable<OrderStateValue> GetOrderStateValues()
        {
            return OrderStateValues;
        }

        //Vergi etkilememiş fiyat
        public decimal GetPrice()
        {
            var result = Price + OrderTagValues.Sum(x => x.Price * x.Quantity);
            if (ProductTimerValue != null)
                result = ProductTimerValue.GetPrice(result);
            return result;
        }

        public decimal GetValue()
        {
            return GetPrice() * Quantity;
        }

        //Görünen fiyat
        public decimal GetVisiblePrice()
        {
            var result = Price + OrderTagValues.Where(x => x.AddTagPriceToOrderPrice).Sum(x => x.Price * x.Quantity);
            if (ProductTimerValue != null)
                result = ProductTimerValue.GetPrice(result + GetOrderTagPrice());
            return result;
        }

        public decimal GetVisibleValue()
        {
            return GetVisiblePrice() * Quantity;
        }

        public decimal GetSelectedValue()
        {
            return SelectedQuantity > 0 ? SelectedQuantity * GetPrice() : GetValue();
        }

        public decimal GetTotal()
        {
            if (CalculatePrice)
            {
                return GetValue();
            }
            return 0;
        }

        public decimal GetPlainTotal()
        {
            return Quantity * Price;
        }

        public IEnumerable<TaxValue> GetTaxValues()
        {
            return TaxValues;
        }

        public TaxValue GetTaxValue(string taxTemplateName)
        {
            return TaxValues.Any(x => x.TaxTemplateName == taxTemplateName)
                ? GetTaxValues().SingleOrDefault(x => x.TaxTemplateName == taxTemplateName)
                : TaxValue.Empty;
        }

        public decimal GetActiveTimerAmount()
        {
            if (this.ProductTimerValue != null)
                return this.ProductTimerValue.GetTimePeriod();
            else
                return 0;
        }
    }
    public class OrderCalculationTypeValue : IEquatable<OrderCalculationTypeValue>
    {
        [DataMember(Name = "CD")]
        public int CalculationTypeID { get; set; }
        [DataMember(Name = "CN")]
        public string CalculationName { get; set; }
        [DataMember(Name = "TX")]
        public bool IsTax { get; set; }
        [DataMember(Name = "DS")]
        public bool IsDiscount { get; set; }
        [DataMember(Name = "SD")]
        public bool IsSD { get; set; }
        [DataMember(Name = "SC")]
        public bool IsSC { get; set; }
        [DataMember(Name = "CA")]
        public decimal CalculationAmount { get; set; }
        [DataMember(Name = "PR")]
        public decimal Price { get; set; }
        [DataMember(Name = "CK")]
        public string CalculationKey { get; set; }

        private static OrderCalculationTypeValue _default;
        public static OrderCalculationTypeValue Default
        {
            get { return _default ?? (_default = new OrderCalculationTypeValue()); }
        }
        public bool Equals(OrderCalculationTypeValue other)
        {
            if (other == null) return false;
            return other.CalculationTypeID == CalculationTypeID;
        }
        public override int GetHashCode()
        {
            return (CalculationTypeID + "_" + Price).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var item = obj as OrderCalculationTypeValue;
            return item != null && Equals(item);
        }
    }
}
