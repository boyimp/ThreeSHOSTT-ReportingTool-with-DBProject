<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs"
Inherits="Views_Index" %>

<!DOCTYPE html>
<html lang="en">
  <head>
    <script src="js/jquery.min.js"></script>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="shortcut icon" href="img/favicon.png" />
    <title>Home</title>
    <!-- Bootstrap CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <!-- Bootstrap CSS  -->
    <link
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css"
      rel="stylesheet"
      integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT"
      crossorigin="anonymous"
    />
    <!-- bootstrap theme -->
    <link href="css/bootstrap-theme.css" rel="stylesheet" />
    <!--external css-->
    <!-- font icon -->
    <link href="css/elegant-icons-style.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <!-- owl carousel -->
    <link rel="stylesheet" href="css/owl.carousel.css" type="text/css" />
    <link href="css/jquery-jvectormap-1.2.2.css" rel="stylesheet" />
    <!-- Custom styles -->
    <link rel="stylesheet" href="css/fullcalendar.css" />
    <link href="css/widgets.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style-responsive.css" rel="stylesheet" />

    <link href="css/jquery-ui-1.10.4.min.css" rel="stylesheet" />
    <%--
    <script src="js/popper.js"></script>
    --%>
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAYruD6-wv8sz3F8tKG3sJ8PUuz1imUsbE"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- javascripts -->
    <script src="js/jquery.min.js"></script>
    <script src="js/jquery-ui-1.10.4.min.js"></script>

    <script
      type="text/javascript"
      src="js/jquery-ui-1.9.2.custom.min.js"
    ></script>
    <!-- bootstrap -->
    <script src="js/bootstrap.min.js"></script>
    <!-- nice scroll -->
    <script src="js/jquery.scrollTo.min.js"></script>
    <script src="js/jquery.nicescroll.js" type="text/javascript"></script>
    <!-- charts scripts -->

    <script src="js/owl.carousel.js"></script>

    <script src="js/jquery.rateit.min.js"></script>
    <!-- custom select -->
    <script src="js/jquery.customSelect.min.js"></script>

    <script src="js/scripts.js"></script>

    <script src="js/jquery-jvectormap-1.2.2.min.js"></script>
    <script src="js/jquery-jvectormap-world-mill-en.js"></script>

    <script src="js/jquery.autosize.min.js"></script>
    <script src="js/jquery.placeholder.min.js"></script>
    <script src="js/gdp-data.js"></script>
    <script src="js/morris.min.js"></script>
    <script src="js/jquery.slimscroll.min.js"></script>

    <script>
      $.noConflict();
    </script>
  </head>
  <body>
    <form runat="server">
      <telerik:RadScriptManager runat="server">
        <Scripts>
          <asp:ScriptReference
            Assembly="Telerik.Web.UI"
            Name="Telerik.Web.UI.Common.Core.js"
          ></asp:ScriptReference>
          <asp:ScriptReference
            Assembly="Telerik.Web.UI"
            Name="Telerik.Web.UI.Common.jQuery.js"
          ></asp:ScriptReference>
          <%--<asp:ScriptReference
            Assembly="Telerik.Web.UI"
            Name="Telerik.Web.UI.Common.jQueryInclude.js"
          ></asp:ScriptReference
          >--%>
        </Scripts>
      </telerik:RadScriptManager>
      <telerik:RadStyleSheetManager
        runat="server"
      ></telerik:RadStyleSheetManager>
      <telerik:RadAjaxManager runat="server"></telerik:RadAjaxManager>
      <section id="container" class="">
        <header class="header dark-bg">
          <div class="toggle-nav">
            <div
              class="icon-reorder tooltips"
              data-original-title="Toggle Navigation"
              data-placement="bottom"
            >
              <i class="icon_menu"></i>
            </div>
          </div>

          <!--logo start-->
          <a href="index.aspx" class="logo"
            >3S <span class="lite">HOSTT</span></a
          >
          <!--logo end-->

          <div class="nav search-row" id="top_menu">
            <!--  search form start -->
            <ul class="nav top-menu">
              <li>
                <div class="navbar-form">
                  <input
                    class="form-control"
                    placeholder="Search"
                    type="text"
                  />
                </div>
              </li>
            </ul>
            <!--  search form end -->
          </div>

          <div class="top-nav notification-row">
            <ul class="nav pull-right top-menu">
              <li class="dropdown">
                <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                  <span class="profile-ava">
                    <img alt="" src="" />
                  </span>
                  <span
                    runat="server"
                    id="lblLoggedInPFCompanyUser"
                    class="username"
                  ></span>
                  <b class="caret"></b>
                </a>
                <ul class="dropdown-menu extended logout">
                  <li>
                    <a href="Default.aspx.html"
                      ><i class="icon_key_alt"></i>Log Out</a
                    >
                  </li>
                </ul>
              </li>
            </ul>
          </div>
        </header>
        <aside>
          <div id="sidebar" class="nav-collapse">
            <!-- sidebar menu start-->
            <ul class="sidebar-menu">
              <li class="active">
                <a class="" href="../Views/Dashboard.aspx">
                  <i class="icon_house_alt"></i>
                  <span>Dashboard</span>
                </a>
              </li>
              <li class="sub-menu">
                <a href="javascript:;" class="">
                  <span
                    >Charts <span class="menu-arrow arrow_carrot-right"></span
                  ></span>
                </a>
                <ul class="sub">
                  <li>
                    <a class="" href="../Views/TimeWiseSalesChart.aspx"
                      >Time wise Sales Chart</a
                    >
                  </li>
                </ul>
              </li>
              <li class="sub-menu">
                <a href="javascript:;" class="">
                  <span
                    >Ticket wise Sales Reports

                    <span class="menu-arrow arrow_carrot-right"></span> </span
                ></a>
                <ul class="sub">
                  <li>
                    <a href="../Views/WorkPeriodReport.aspx">
                      <h5 style="text-align: center">Work Period Report</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/Tickets.aspx">
                      <h5 style="text-align: center">Ticket wise Sales</h5>
                    </a>
                  </li>
                  <li>
                    <a
                      href="../Views/TicketsAccountWise.aspx?ReportType=Normal"
                    >
                      <h5 style="text-align: center">Tickets Account Wise</h5>
                    </a>
                  </li>
                  <li>
                    <a
                      href="../Views/TicketsAccountWise.aspx?ReportType=VATIncluded"
                    >
                      <h5 style="text-align: center">
                        Tickets Account Wise (VAT Included)
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/TicketsEntityWise.aspx">
                      <h5 style="text-align: center">Tickets Entity Wise</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/WaiterWiseItemSalesReport.aspx">
                      <h5 style="text-align: center">Waiter wise Sales</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/GiftOrders.aspx">
                      <h5 style="text-align: center">Gift Given</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/VoidOrders.aspx">
                      <h5 style="text-align: center">Void Entries</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/CancelOrders.aspx">
                      <h5 style="text-align: center">Cancel Entries</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/EntityReport.aspx">
                      <h5 style="text-align: center">Entity wise Report</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/CalculationsReport.aspx">
                      <h5 style="text-align: center">Calculations Report</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/CalculationsEntityWiseReport.aspx">
                      <h5 style="text-align: center">
                        Calculations Entity Wise
                      </h5>
                    </a>
                  </li>
                </ul>
              </li>
              <li class="sub-menu">
                <a href="javascript:;" class="">
                  <span
                    >Menu Item wise Sales Reports
                    <span class="menu-arrow arrow_carrot-right"></span
                  ></span>
                </a>
                <ul class="sub">
                  <li>
                    <a href="../Views/ItemSalesReport.aspx">
                      <h5 style="text-align: center">Item Sales Report</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/ItemSalesOutletWise.aspx">
                      <h5 style="text-align: center">Item Sales Outlet Wise</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/ItemSalesProfitAnalysis.aspx">
                      <h5 style="text-align: center">
                        Item Sales Profit Analysis
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/ItemSalesAccountWise.aspx">
                      <h5 style="text-align: center">
                        Item Sales Account Details
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/ItemSalesProfitLossFixed.aspx">
                      <h5 style="text-align: center">
                        Item Sales Profit/Loss Fixed Cost Wise
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/ItemSalesProfitLossRecipe.aspx">
                      <h5 style="text-align: center">
                        Item Sales Profit/Loss Recipe Wise
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/ItemWiseMenuMixReport.aspx">
                      <h5 style="text-align: center">
                        Item Wise Menu Mix Report
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/ItemWiseMenuMixReport2.aspx">
                      <h5 style="text-align: center">
                        Item Wise Menu Mix Report 2
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/TargetAchievement.aspx">
                      <h5 style="text-align: center">
                        Target Vs Achievement Report
                      </h5>
                    </a>
                  </li>
                </ul>
              </li>
              <li class="sub-menu">
                <a href="javascript:;" class="">
                  <span
                    >Accounts Reports
                    <span class="menu-arrow arrow_carrot-right"></span
                  ></span>
                </a>
                <ul class="sub">
                  <li>
                    <a href="../Views/CurrentBalanceOnAChead.aspx">
                      <h5 style="text-align: center">
                        Current Balance of Accounts Head
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/AccountTypeWise.aspx">
                      <h5 style="text-align: center">
                        Accounnt Type Wise Report
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/AccountWise.aspx">
                      <h5 style="text-align: center">Account Wise Report</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/Entities.aspx">
                      <h5 style="text-align: center">Entity And Accounts</h5>
                    </a>
                  </li>
                </ul>
              </li>

              <li class="sub-menu">
                <a href="javascript:;" class="">
                  <span
                    >Stock Reports<span
                      class="menu-arrow arrow_carrot-right"
                    ></span
                  ></span>
                </a>
                <ul class="sub">
                  <li>
                    <a
                      href="../Views/InventoryPotentialRevenue.aspx?ReportType=All"
                    >
                      <h5 style="text-align: center">
                        Inventory Potential Revenue
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a
                      href="../Views/InventoryPotentialRevenue.aspx?ReportType=TheoUsage"
                    >
                      <h5 style="text-align: center">
                        Consumption/Theoritical Usage Report
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a
                      href="../Views/InventoryPotentialRevenue.aspx?ReportType=Wastage"
                    >
                      <h5 style="text-align: center">Wastage Report</h5>
                    </a>
                  </li>
                  <li>
                    <a
                      href="../Views/InventoryPotentialRevenue.aspx?ReportType=CountVariance"
                    >
                      <h5 style="text-align: center">Count Variance Report</h5>
                    </a>
                  </li>
                  <li>
                    <a
                      href="../Views/InventoryPotentialRevenue.aspx?ReportType=StockTake"
                    >
                      <h5 style="text-align: center">Stock Take Report</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/InventoryCostOfSalesTotal.aspx">
                      <h5 style="text-align: center">Cost of Sales Summary</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/CurrentStock.aspx">
                      <h5 style="text-align: center">Stock Report</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/InvntoryRegister.aspx">
                      <h5 style="text-align: center">Inventory Register</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/WorkPeriodWiseInvntoryRegister.aspx">
                      <h5 style="text-align: center">
                        Work Period wise Inventory Register
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/WorkPeriodEndReport.aspx">
                      <h5 style="text-align: center">Work Period End Report</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/SpecialInventoryRegister.aspx">
                      <h5 style="text-align: center">
                        Special Inventory Register
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/InventoryTransactionDateWise.aspx">
                      <h5 style="text-align: center">Inventory Transactions</h5>
                    </a>
                  </li>
                </ul>
              </li>
              <li class="sub-menu">
                <a href="javascript:;" class="">
                  <span
                    >Special Reports
                    <span class="menu-arrow arrow_carrot-right"></span
                  ></span>
                </a>
                <ul class="sub">
                  <li>
                    <a>
                      <asp:Button
                        runat="server"
                        Style="background-color: transparent; color: white; background: transparent; width: 100%; border: 0px;"
                        OnClick="InventoryList_onClick"
                        Text="Inventory Item List"
                    /></a>
                  </li>
                  <li>
                    <a href="../Views/MenuItemList.aspx">
                      <h5 style="text-align: center">Menu Item List</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/Recipe.aspx">
                      <h5 style="text-align: center">Recipe</h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/ProductionCost.aspx">
                      <h5 style="text-align: center">
                        Production Cost Analysis
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/InventoryCostOfSalesDetail.aspx">
                      <h5 style="text-align: center">
                        Inventory Cost Of Sales Detail
                      </h5>
                    </a>
                  </li>
                  <li>
                    <a href="../Views/OutletStatus.aspx">
                      <h5 style="text-align: center">Outlet Status</h5>
                    </a>
                  </li>
                </ul>
              </li>
            </ul>
            <!-- sidebar menu end-->
          </div>
        </aside>
        <!--sidebar end-->
        <!--main content start-->
        <section id="main-content">
          <section class="wrapper">
            <!--overview start-->
            <div class="row">
              <div class="col-lg-12">
                <h3 class="page-header">
                  <i class="fa fa-laptop"></i>Dashboard
                </h3>
                <ol class="breadcrumb">
                  <li>
                    <i class="fa fa-home"></i><a href="Dashboard.aspx">Home</a>
                  </li>
                  <li><i class="fa fa-laptop"></i>Dashboard</li>
                </ol>
              </div>
            </div>

            <div class="row">
              <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <a href="../Views/Tickets.aspx">
                  <div class="info-box blue-bg">
                    <div
                      id="spinnerNetTotal"
                      class="spinner"
                      style="display: none"
                    >
                      <img
                        id="img-spinner"
                        src="loading.gif"
                        alt="Loading"
                        height="50"
                        width="50"
                      />
                    </div>
                    <div id="divNetTotal" class="count"></div>
                    <div class="title">Net Total</div>
                  </div>
                </a>
                <!--/.info-box-->
              </div>
              <!--/.col-->
              <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <a href="../Views/Tickets.aspx">
                  <div class="info-box brown-bg">
                    <div
                      id="spinnerGrossTotal"
                      class="spinner"
                      style="display: none"
                    >
                      <img
                        src="loading.gif"
                        alt="Loading"
                        height="50"
                        width="50"
                      />
                    </div>
                    <div id="divGrossTotal" class="count"></div>
                    <div class="title">Gross Total</div>
                  </div>
                </a>
                <!--/.info-box-->
              </div>
              <!--/.col-->

              <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <a class="" href="../Views/TimeWiseSalesChart.aspx">
                  <div class="info-box dark-bg">
                    <div
                      id="spinnerTicketNumber"
                      class="spinner"
                      style="display: none"
                    >
                      <img
                        src="loading.gif"
                        alt="Loading"
                        height="50"
                        width="50"
                      />
                    </div>
                    <div
                      runat="server"
                      class="count"
                      id="divTotalNumberTickets"
                    ></div>
                    <div class="title">No of Tickets</div>
                    <div
                      id="spinnerTicketAmount"
                      class="spinner"
                      style="display: none"
                    >
                      <img
                        src="loading.gif"
                        alt="Loading"
                        height="50"
                        width="50"
                      />
                    </div>
                    <div
                      runat="server"
                      class="count"
                      id="divTicketTotalAmount"
                    ></div>
                    <div class="title">Amount</div>
                  </div>
                </a>
                <!--/.info-box-->
              </div>
              <!--/.col-->
              <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <a href="../Views/VoidOrders.aspx">
                  <div class="info-box green-bg">
                    <div id="spinnerNOV" class="spinner" style="display: none">
                      <img
                        src="loading.gif"
                        alt="Loading"
                        height="50"
                        width="50"
                      />
                    </div>
                    <div class="count" runat="server" id="divNOV"></div>
                    <div class="title">Number of Voids</div>
                    <div id="spinnerAOV" class="spinner" style="display: none">
                      <img
                        src="loading.gif"
                        alt="Loading"
                        height="50"
                        width="50"
                      />
                    </div>
                    <div class="count" runat="server" id="divAOV"></div>
                    <div class="title">Amount</div>
                  </div>
                </a>
                <!--/.info-box-->
              </div>
              <!--/.col-->
            </div>
            <div class="row">
              <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <div class="info-box lime-bg">
                  <div id="spinnerDSC" class="spinner" style="display: none">
                    <img
                      src="loading.gif"
                      alt="Loading"
                      height="50"
                      width="50"
                    />
                  </div>
                  <div runat="server" id="divDSC" class="count"></div>
                  <div class="title">Discount</div>
                  <div id="spinnerSPDSC" class="spinner" style="display: none">
                    <img
                      src="loading.gif"
                      alt="Loading"
                      height="50"
                      width="50"
                    />
                  </div>
                  <div runat="server" id="divSPDSC" class="count"></div>
                  <div class="title">Special Discount</div>
                </div>
                <!--/.info-box-->
              </div>
              <!--/.col-->

              <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <div class="info-box linkedin-bg">
                  <div
                    id="spinnerSrockPurchase"
                    class="spinner"
                    style="display: none"
                  >
                    <img
                      src="loading.gif"
                      alt="Loading"
                      height="50"
                      width="50"
                    />
                  </div>
                  <div runat="server" id="divStockPurchase" class="count"></div>
                  <div class="title">Stock Purchase</div>
                </div>
                <!--/.info-box-->
              </div>
              <!--/.col-->

              <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <div class="info-box red-bg">
                  <div
                    id="spinnerCurrentStock"
                    class="spinner"
                    style="display: none"
                  >
                    <img
                      src="loading.gif"
                      alt="Loading"
                      height="50"
                      width="50"
                    />
                  </div>
                  <div runat="server" class="count" id="divCurrentStock"></div>
                  <div class="title">Warehouse Value</div>
                </div>
                <!--/.info-box-->
              </div>
              <!--/.col-->

              <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <a class="" href="../Views/TimeWiseSalesChart.aspx">
                  <div class="info-box twitter-bg">
                    <div id="spinnerNOG" class="spinner" style="display: none">
                      <img
                        src="loading.gif"
                        alt="Loading"
                        height="50"
                        width="50"
                      />
                    </div>
                    <div class="count" runat="server" id="divNOG"></div>
                    <div class="title">Number of Guests</div>
                    <div id="spinnerANOG" class="spinner" style="display: none">
                      <img
                        src="loading.gif"
                        alt="Loading"
                        height="50"
                        width="50"
                      />
                    </div>
                    <div class="count" runat="server" id="divANOG"></div>
                    <div class="title">AVG guest per ticket</div>
                  </div>
                </a>
                <!--/.info-box-->
              </div>
              <!--/.col-->
            </div>
            <div class="row">
              <div class="col-lg-9 col-md-9">
                <div class="panel panel-default">
                  <div class="panel-heading">
                    <h2>
                      <i class="fa fa-flag-o red"></i
                      ><strong>Last 7 Days Sales</strong>
                    </h2>
                    <div class="panel-actions">
                      <a href="index.aspx" class="btn-setting"
                        ><i class="fa fa-rotate-right"></i
                      ></a>
                      <a href="index.aspx" class="btn-minimize"
                        ><i class="fa fa-chevron-up"></i
                      ></a>
                      <a href="index.aspx" class="btn-close"
                        ><i class="fa fa-times"></i
                      ></a>
                    </div>
                  </div>
                  <div class="panel-body">
                    <table class="table bootstrap-datatable countries">
                      <thead>
                        <tr>
                          <th>Date</th>
                          <th>Total Net Sales</th>
                          <th>Total Gross Sale</th>
                        </tr>
                      </thead>

                      <tbody runat="server" id="salesTableBody">
                        <tr
                          id="spinner7DaysSales"
                          class="spinner"
                          style="display: none"
                        >
                          <td></td>
                          <td>
                            <div>
                              <img
                                src="loading.gif"
                                alt="Loading"
                                height="50"
                                width="50"
                              />
                            </div>
                          </td>
                          <td></td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
              <div class="col-lg-3 col-md-3">
                <div class="panel panel-default">
                  <div class="panel-heading">
                    <h2>
                      <i class="fa fa-flag-o red"></i
                      ><strong>Account Wise Sales</strong>
                    </h2>
                    <div class="panel-actions">
                      <a href="index.aspx" class="btn-setting"
                        ><i class="fa fa-rotate-right"></i
                      ></a>
                      <a href="index.aspx" class="btn-minimize"
                        ><i class="fa fa-chevron-up"></i
                      ></a>
                      <a href="index.aspx" class="btn-close"
                        ><i class="fa fa-times"></i
                      ></a>
                    </div>
                  </div>
                  <div class="panel-body">
                    <table class="table bootstrap-datatable countries">
                      <thead>
                        <tr>
                          <th>Account</th>
                          <th>Net Amount</th>
                        </tr>
                      </thead>
                      <tbody runat="server" id="currentIncomeSummaryBody">
                        <tr
                          id="spinnerAccountWise"
                          class="spinner"
                          style="display: none"
                        >
                          <td></td>
                          <td>
                            <div>
                              <img
                                src="loading.gif"
                                alt="Loading"
                                height="50"
                                width="50"
                              />
                            </div>
                          </td>
                          <td></td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
                >
              </div>
              <!--/col-->
            </div>
            <div class="row">
              <div class="col-lg-6 col-md-6">
                <div class="panel panel-default">
                  <div class="panel-heading">
                    <h2>
                      <i class="fa fa-flag-o red"></i
                      ><strong>Top 10 Selling Items</strong>
                    </h2>
                    <div class="panel-actions">
                      <a href="index.aspx" class="btn-setting"
                        ><i class="fa fa-rotate-right"></i
                      ></a>
                      <a href="index.aspx" class="btn-minimize"
                        ><i class="fa fa-chevron-up"></i
                      ></a>
                      <a href="index.aspx" class="btn-close"
                        ><i class="fa fa-times"></i
                      ></a>
                    </div>
                  </div>
                  <div class="panel-body">
                    <table class="table bootstrap-datatable countries">
                      <thead>
                        <tr>
                          <th>Item Name</th>
                          <th>Portion</th>
                          <th>Quantity</th>
                        </tr>
                      </thead>
                      <tbody runat="server" id="ItemWiseDataBody">
                        <tr
                          id="spinnerItemWise"
                          class="spinner"
                          style="display: none"
                        >
                          <td>
                            <div>
                              <img
                                src="loading.gif"
                                alt="Loading"
                                height="50"
                                width="50"
                              />
                            </div>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
              <div class="col-lg-6 col-md-6">
                <div class="panel panel-default">
                  <div class="panel-heading">
                    <h2>
                      <i class="fa fa-flag-o red"></i
                      ><strong>Menu Wise Sales Chart</strong>
                    </h2>
                    <div class="panel-actions">
                      <a href="index.aspx" class="btn-setting"
                        ><i class="fa fa-rotate-right"></i
                      ></a>
                      <a href="index.aspx" class="btn-minimize"
                        ><i class="fa fa-chevron-up"></i
                      ></a>
                      <a href="index.aspx" class="btn-close"
                        ><i class="fa fa-times"></i
                      ></a>
                    </div>
                  </div>

                  <div id="spinnerChart" class="spinner" style="display: none">
                    <img
                      src="loading.gif"
                      alt="Loading"
                      height="50"
                      width="50"
                    />
                  </div>

                  <div id="chart" style="text-align: center" class="panel-body">
                    <telerik:RadHtmlChart
                      runat="server"
                      ID="MenuWiseSalesChart"
                      Transitions="false"
                    >
                      <PlotArea>
                        <Series>
                          <telerik:PieSeries
                            DataFieldY="Sum"
                            ColorField="Color"
                            NameField="Group"
                            ExplodeField="IsExploded"
                          >
                          </telerik:PieSeries>
                        </Series>
                      </PlotArea>
                    </telerik:RadHtmlChart>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
              <div class="panel panel-default">
                <div class="panel-heading">
                  <h2>
                    <i class="fa fa-flag-o red"></i><strong>Outlets</strong>
                  </h2>
                  <div class="panel-actions">
                    <a href="index.aspx" class="btn-setting"
                      ><i class="fa fa-rotate-right"></i
                    ></a>
                    <a href="index.aspx" class="btn-minimize"
                      ><i class="fa fa-chevron-up"></i
                    ></a>
                    <a href="index.aspx" class="btn-close"
                      ><i class="fa fa-times"></i
                    ></a>
                  </div>
                  <div
                    id="map"
                    class="col-lg-6 col-md-6"
                    style="height: 400px; width: 100%"
                  ></div>
                </div>
              </div>
            </div>
            <!--/col-->
          </section>
          <div class="text-right">
            <div class="credits">
              <a href="https://3S-bd.com/">Hostt Reporting Tool</a> by
              <a href="https://3S-bd.com/">3S-bd</a>
            </div>
          </div>
        </section>
        <!--main content end-->
      </section>
      <!-- container section start -->
      <script src="https://cdnjs.cloudflare.com/ajax/libs/jQuery-Knob/1.2.13/jquery.knob.min.js"></script>

      <script>
        $("#spinnerNetTotal").show();
        $("#spinnerGrossTotal").show();
        $("#spinner7DaysSales").show();
        $("#spinnerAccountWise").show();
        $("#spinnerItemWise").show();
        $("#spinnerChart").show();
        $("#spinnerTicketNumber").show();
        $("#spinnerTicketAmount").show();
        $("#spinnerNOG").show();
        $("#spinnerNOV").show();
        $("#spinnerAOV").show();
        $("#spinnerANOG").show();
        $("#spinnerSPDSC").show();
        $("#spinnerDSC").show();
        $("#spinnerStockPurchase").show();
        $("#spinnerCurrentStock").show();
      </script>

      <script>
        $(document).ready(function () {
          $.getJSON("../api/home/GetCurrentNetSale", function (data) {
            WebForm_GetElementById("divNetTotal").innerText = data;
            console.log(data);
            $("#spinnerNetTotal").hide();
          });
          $.getJSON("../api/home/GetCurrentGrossSale", function (data) {
            WebForm_GetElementById("divGrossTotal").innerText = data;
            console.log(data);
            $("#spinnerGrossTotal").hide();
          });
          $.getJSON("../api/home/GetTotalNumberofTickets", function (data) {
            WebForm_GetElementById("divTotalNumberTickets").innerText = data;
            console.log(data);
            $("#spinnerTicketNumber").hide();
          });

          $.getJSON("../api/home/GetTicketsAmount", function (data) {
            WebForm_GetElementById("divTicketTotalAmount").innerText = data;
            console.log(data);
            $("#spinnerTicketAmount").hide();
          });
          $.getJSON("../api/home/GetNumberOfVoids", function (data) {
            WebForm_GetElementById("divNOV").innerText = data;
            console.log(data);
            $("#spinnerNOV").hide();
          });
          $.getJSON("../api/home/GetAmountOfVoids", function (data) {
            WebForm_GetElementById("divAOV").innerText = data;
            console.log(data);
            $("#spinnerAOV").hide();
          });
          $.getJSON("../api/home/GetStockPurchase", function (data) {
            WebForm_GetElementById("divStockPurchase").innerText = data;
            console.log(data);
            $("#spinnerStockPurchase").hide();
          });
          $.getJSON("../api/home/GetCurrentStockValue", function (data) {
            WebForm_GetElementById("divCurrentStock").innerText = data;
            console.log(data);
            $("#spinnerCurrentStock").hide();
          });
          $.getJSON("../api/home/GetNumberOfGuests", function (data) {
            WebForm_GetElementById("divNOG").innerText = data;
            console.log(data);
            WebForm_GetElementById("divANOG").innerText =
              parseInt(WebForm_GetElementById("divNOG").innerText) /
              parseInt(
                WebForm_GetElementById("divTotalNumberTickets").innerText
              );
            $("#spinnerNOG").hide();
            $("#spinnerANOG").hide();
          });

          $.getJSON("../api/home/dailyNetSales", function (data) {
            if (data) {
              var obj = jQuery.parseJSON(data);
              $("#salesTableBody").empty();
              $.each(obj, function (key, val) {
                var row =
                  "<tr><td>" +
                  val.StartDate +
                  "</td><td>" +
                  val.Sales +
                  "</td><td>" +
                  val.TicketTotalAmount +
                  "</td></tr>";
                $(row).appendTo($("#salesTableBody"));
              });
            }
            $("#spinner7DaysSales").hide();
          });
          $.getJSON(
            "../api/home/GetCurrentAccoountWiseReport",
            function (data) {
              var obj = jQuery.parseJSON(data);
              var spfound = 0;
              var dscfound = 0;
              $("#currentIncomeSummaryBody").empty();
              $.each(obj, function (key, val) {
                if (val.UltimateAccount.toString() !== "Sales") {
                  if (val.UltimateAccount.toString() === "Discount") {
                    WebForm_GetElementById("divDSC").innerText = val.Amount;
                    dscfound = 1;
                    $("#spinnerDSC").hide();
                  }
                  if (val.UltimateAccount.toString() === "Special Discount") {
                    WebForm_GetElementById("divDSC").innerText = val.Amount;
                    spfound = 1;
                    $("#spinnerSPDSC").hide();
                  }
                  var row =
                    "<tr><td>" +
                    val.UltimateAccount +
                    "</td><td>" +
                    val.Amount +
                    "</td></tr>";
                  $(row).appendTo($("#currentIncomeSummaryBody"));
                }
              });
              if (dscfound === 0) {
                WebForm_GetElementById("divDSC").innerText = 0;
                $("#spinnerDSC").hide();
              }
              if (spfound === 0) {
                WebForm_GetElementById("divSPDSC").innerText = 0;
                $("#spinnerSPDSC").hide();
              }
              $("#spinnerAccountWise").hide();
            }
          );

          $.getJSON("../api/home/Top10SellingItems", function (data) {
            var obj = jQuery.parseJSON(data);

            $("#ItemWiseDataBody").empty();
            $.each(obj, function (key, val) {
              var row =
                "<tr><td>" +
                val.ItemName +
                "</td><td>" +
                val.PortionName +
                "</td><td>" +
                val.Quantity +
                "</td></tr>";
              $(row).appendTo($("#ItemWiseDataBody"));
            });
            $("#spinnerItemWise").hide();
          });

          $.getJSON("../api/home/GetMenuWiseCurrentSales", function (data) {
            var obj = jQuery.parseJSON(data);
            var menuWiseSalesChart = $find("<%=MenuWiseSalesChart.ClientID %>");
            //Set the new datasource
            menuWiseSalesChart.set_dataSource(obj);

            //Turning animation on before repainting the chart
            menuWiseSalesChart.set_transitions(true);

            //Redrawing the chart
            menuWiseSalesChart.repaint();
            $("#spinnerChart").hide();
          });
        });
        window.onresize = function () {
          $find("<%=MenuWiseSalesChart.ClientID%>").get_kendoWidget().resize();
        };
        var markers = [
          [
            "Order 1 Source",
            23.7642087,
            90.3621477,
            "Order 1 Destination",
            23.7142087,
            90.3521477,
          ],
          [
            "Order 2 Source",
            23.6642087,
            90.3021477,
            "Order 2 Destination",
            23.7542087,
            90.3633377,
          ],
          [
            "Order 3 Source",
            23.6942087,
            90.3321477,
            "Order 3 Destination",
            23.732087,
            90.4033377,
          ],
        ];

        function initialize() {
          var center = { lat: 51.5159729, lng: -0.1015987 };
          var map = new google.maps.Map(document.getElementById("map"), {
            disableDefaultUI: true,
            center: center,
            zoom: 11,
          });

          var mark = [];
          var iconNormal = "http://i.stack.imgur.com/AAsD3.png",
            iconSelected =
              "https://webdesign.danols.com/static/template/images/icons/light/pin_map_icon&48.png",
            bounds = new google.maps.LatLngBounds();
          function setMarkers(map) {
            for (var i = 0; i < markers.length; i++) {
              var marker = markers[i],
                myLatLng = new google.maps.LatLng(marker[1], marker[2]),
                eachMarker = new google.maps.Marker({
                  record_id: i,
                  position: myLatLng,
                  map: map,
                  animation: google.maps.Animation.DROP,
                  icon: iconNormal,
                  Label: marker[0],
                  title: marker[0],
                });
              //var selectedMarker;
              bounds.extend(myLatLng);
              mark.push(eachMarker);
            }
          }
          map.fitBounds(bounds);
          setMarkers(map);
        }
        google.maps.event.addDomListener(window, "load", initialize);
      </script>
    </form>
  </body>
</html>
