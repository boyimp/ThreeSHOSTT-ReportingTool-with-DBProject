<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="UI_Home" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>3SHostt Reporting Tool</title>
    <script type="text/javascript" src="../jquery.js"></script>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <script type="text/javascript">
        $(document).ready(function () {
            /*click function starts here*/
            $('#nav li a').click(function () {
                try {
                    //                var sds = document.getElementById("dum");
                    //                if (sds == null) {
                    //                    alert("You are using a free package.\n You are not allowed to remove the tag.\n");
                    //                }
                    var sdss = document.getElementById("dumdiv");
                    //                if (sdss == null) {
                    //                    alert("You are using a free package.\n You are not allowed to remove the tag.\n");
                    //                }
                    if (sdss == null) {
                        var s = $(this).attr('id');
                        var imgid = $("#" + s + " img").attr('id');
                        var imgsrc = $("#" + imgid + "").attr('src');
                        if (imgsrc == "../images/insert.jpg") {
                            $("#" + imgid + "").attr('src', '../images/remove.jpg');
                            $(this).next().slideDown(800);
                        }
                        else {
                            $("#" + imgid + "").attr('src', '../images/insert.jpg');
                            $(this).next().slideUp(800);
                        }
                    }
                }
                catch (e)
                { }
            });
            /*click function ends here*/

            /*mouseover function starts here*/
            /*$("#nav li a").mouseover(function() {
            var sds = document.getElementById("dum");
            if(sds == null){
            alert("You are using a free package.\n You are not allowed to remove the tag.\n");
            }
            var sdss = document.getElementById("dumdiv");
            if(sdss == null){
            alert("You are using a free package.\n You are not allowed to remove the tag.\n");
            }
            if(sdss != null){
            var s = $(this).attr('id');
            var imgid=$("#"+s+" img").attr('id');
            var imgsrc=$("#"+imgid+"").attr('src');
            if(imgsrc=="insert.jpg")
            {
            $("#"+imgid+"").attr('src','remove.jpg');
            $(this).parent().find(".count").slideDown('medium').show(); 
            }
            $(this).parent().hover(function() 
            {
            }, 
            function()
            {	$("#"+imgid+"").attr('src','insert.jpg');
            $(this).parent().find(".count").slideUp('slow'); 
            });
            }
            });*/
            /*mouseover function ends here*/
        });
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <uc:header id="header1" runat="server"></uc:header>
    <div align='center' >
    <div class="left"> 
    <ul id="nav">
        <li><a href="../UI/DashBoard.aspx"><h1 align='center'> Dashboard</h1></a></li>
         <li><a id='A2'><img src="../images/insert.jpg" id="Img3" align='left'><h1 align='center'>Charts</h1></a>
            <ul class='count'>
                <li><a href="../UI/TimeWiseSalesChart.aspx"><h1 align='center'>Time wise Sales Chart</h1></a></li>
            </ul>
        </li>
        <li><a id='im1'><img src="../images/insert.jpg" id="1" align='left'><h1 align='center'>Ticket wise Sales Reports</h1></a>
            <ul class='count'>
                <li><a href="../UI/WorkPeriodReport.aspx"><h1 align='center'>Work Period Report</h1></a></li>
                <li><a href="../UI/Tickets.aspx"><h1 align='center'> Ticket wise Sales</h1></a></li>
                <li><a href="../UI/TicketsAccountWise.aspx?ReportType=Normal"><h1 align='center'>Tickets Account Wise</h1></a></li>
                <li><a href="../UI/TicketsAccountWise.aspx?ReportType=VATIncluded"><h1 align='center'>Tickets Account Wise (VAT Included)</h1></a></li>
                <li><a href="../UI/TicketsEntityWise.aspx"><h1 align='center'>Tickets Entity Wise</h1></a></li>
                <li><a href="../UI/WaiterWiseItemSalesReport.aspx"><h1 align='center'>Waiter wise Sales</h1></a></li>
                <li><a href="../UI/GiftOrders.aspx"><h1 align='center'>Gift Given</h1></a></li>
                <li><a href="../UI/VoidOrders.aspx"><h1 align='center'>Void Entries</h1></a></li>
                <li><a href="../UI/CancelOrders.aspx"><h1 align='center'>Cancel Entries</h1></a></li>
                <li><a href="../UI/EntityReport.aspx"><h1 align='center'>Entity wise Report</h1></a></li>
                <li><a href="../UI/CalculationsReport.aspx"><h1 align='center'>Calculations Report</h1></a></li>
                <li><a href="../UI/CalculationsEntityWiseReport.aspx"><h1 align='center'>Calculations Entity Wise</h1></a></li>
            </ul>
        </li>
         <li><a id='A5'><img src="../images/insert.jpg" id="Img5" align='left'><h1 align='center'>Menu Item wise Sales Reports</h1></a>
            <ul class='count'>
                <li><a href="../UI/ItemSalesReport.aspx"><h1 align='center'>Item Sales Report</h1></a></li>
                <li><a href="../UI/ItemSalesOutletWise.aspx"><h1 align='center'>Item Sales Outlet Wise</h1></a></li>
                <li><a href="../UI/ItemSalesProfitAnalysis.aspx"><h1 align='center'>Item Sales Profit Analysis</h1></a></li>
                <li><a href="../UI/ItemSalesAccountWise.aspx"><h1 align='center'>Item Sales Account Details</h1></a></li>
                <li><a href="../UI/ItemSalesProfitLossFixed.aspx"><h1 align='center'>Item Sales Profit/Loss Fixed Cost Wise</h1></a></li>
                <li><a href="../UI/ItemSalesProfitLossRecipe.aspx"><h1 align='center'>Item Sales Profit/Loss Recipe Wise</h1></a></li>
                <li><a href="../UI/ItemWiseMenuMixReport.aspx"><h1 align='center'>Item Wise Menu Mix Report</h1></a></li>
                <li><a href="../UI/ItemWiseMenuMixReport2.aspx"><h1 align='center'>Item Wise Menu Mix Report 2</h1></a></li>
                <li><a href="../UI/TargetAchievement.aspx"><h1 align='center'>Target Vs Achievement Report</h1></a></li>
            </ul>
        </li>
       <%-- <li><a id='A2'><img src="../images/insert.jpg" id="Img3" align='left'><h1 align='center'>Purchase Report</h1></a>
            <ul class='count'>
                <li><a><h1 align='center'>Purchase Report-challan wise</h1></a></li>
                <li><a><h1 align='center'>Inventory Item wise purchase report</h1></a></li>
            </ul>
        </li>--%>
        <li><a id='A3'><img src="../images/insert.jpg" id="Img4" align='left'><h1 align='center'>Accounts Reports</h1></a>
            <ul class='count'>
                <li><a href="../UI/CurrentBalanceOnAChead.aspx"><h1 align='center'>Current Balance of Accounts Head</h1></a></li>
                <li><a href="../UI/AccountTypeWise.aspx"><h1 align='center'>Accounnt Type Wise Report</h1></a></li>
                <li><a href="../UI/AccountWise.aspx"><h1 align='center'>Account Wise Report</h1></a></li>
                <li><a href="../UI/Entities.aspx"><h1 align='center'>Entity And Accounts</h1></a></li>                
            </ul>
        </li>
        <li><a id='A4'><img src="../images/insert.jpg" id="Img1" align='left'><h1 align='center'>Stock Reports</h1></a>
            <ul class='count'>
                <li><a href="../UI/InventoryPotentialRevenue.aspx?ReportType=All" ><h1 align='center'>Inventory Potential Revenue</h1></a></li>
                <li><a href="../UI/InventoryPotentialRevenue.aspx?ReportType=TheoUsage" ><h1 align='center'>Consumption/Theoritical Usage Report</h1></a></li>
                <li><a href="../UI/InventoryPotentialRevenue.aspx?ReportType=Wastage" ><h1 align='center'>Wastage Report</h1></a></li>
                <li><a href="../UI/InventoryPotentialRevenue.aspx?ReportType=CountVariance" ><h1 align='center'>Count Variance Report</h1></a></li>
                <li><a href="../UI/InventoryPotentialRevenue.aspx?ReportType=StockTake" ><h1 align='center'>Stock Take Report</h1></a></li>
                <li><a href="../UI/InventoryCostOfSalesTotal.aspx" ><h1 align='center'>Cost of Sales Summary</h1></a></li>
                <li><a href="../UI/CurrentStock.aspx"><h1 align='center'>Stock Report</h1></a></li>
                <li><a href="../UI/InvntoryRegister.aspx"><h1 align='center'>Inventory Register</h1></a></li>
                <li><a href="../UI/WorkPeriodWiseInvntoryRegister.aspx"><h1 align='center'>Work Period wise Inventory Register</h1></a></li>
                <li><a href="../UI/WorkPeriodEndReport.aspx"><h1 align='center'>Work Period End Report</h1></a></li>
                <li><a href="../UI/SpecialInventoryRegister.aspx"><h1 align='center'>Special Inventory Register</h1></a></li>
                <li><a href="../UI/InventoryTransactionDateWise.aspx"><h1 align='center'>Inventory Transactions</h1></a></li>
            </ul>
        </li>
        <li><a id='A1'><img src="../images/insert.jpg" id="Img2" align='left'><h1 align='center'>Special Reports</h1></a>
            <ul class='count'>
                <li ><asp:LinkButton ID="InventoryList" runat="server" 
                        OnClick="InventoryList_onClick" align='center' Text="Inventory Item List" 
                        Font-Bold="True" Font-Size="X-Large"></asp:LinkButton></li>
                 <li><a href="../UI/MenuItemList.aspx"><h1 align='center'>Menu Item List</h1></a></li>
                <li><a href="../UI/Recipe.aspx"><h1 align='center'>Recipe</h1></a></li>
                <li><a href="../UI/ProductionCost.aspx"><h1 align='center'>Production Cost Analysis</h1></a></li>
                <li><a href="../UI/InventoryCostOfSalesDetail.aspx"><h1 align='center'>Inventory Cost Of Sales Detail</h1></a></li>
                <li><a href="../UI/OutletStatus.aspx"><h1 align='center'>Outlet Status</h1></a></li>
<%--                <li><a href="../UI/InventoryCountVariance.aspx"><h1 align='center'>Inventory Count Variance</h1></a></li>
                <li><a href="../UI/InventoryPotentialRevenueShort.aspx"><h1 align='center'>Inventory Potential Revenue 2</h1></a></li>--%>
<%--                <li><a><h1 align='center'>Stock Transfer Report</h1></a></li>
                <li><a><h1 align='center'>Expense Report</h1></a></li>
                <li><a><h1 align='center'>Shadow Sales Report</h1></a></li>
                <li><a><h1 align='center'>Delivery Report</h1></a></li>
                <li><a><h1 align='center'>Sales Report</h1></a></li>
                <li><a><h1 align='center'>Challan</h1></a></li>--%>
            </ul>
        </li>
  </ul>
    </div>
    </div>
    <uc:footer id="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
