//@ts-check
import React from 'react';
import { Route, Switch, HashRouter, Redirect } from 'react-router-dom';
import 'react-datepicker/dist/react-datepicker.css';
import './index.css';
import { ChakraProvider } from '@chakra-ui/react';
import theme from './theme';
import { DashboardFilterContextProvider } from './context/DashboardFilterContext';
import { SidebarContextProvider } from './context/SidebarContext';
import Sidebar from './pages/shared/Sidebar';
import PageLoading from './pages/shared/PageLoading';
import PageNotFound from './pages/shared/PageNotFound';
import Login from './pages/Login/Login';
import NewDashboard from './pages/NewDashboard/NewDashboard';
import DailySalesChart from './pages/chart/DailySalesChart/DailySalesChart';
import MonthlySalesChart from './pages/chart/MonthlySalesChart/MonthlySalesChart';
import OutletComparisonChart from './pages/chart/OutletComparisonChart/OutletComparisonChart';
import DayAndTimeWiseSalesChart from './pages/chart/DayAndTimeWiseSalesChart/DayAndTimeWiseSalesChart';
import YearlyComparisonChart from './pages/chart/YearlyComparisonChart/YearlyComparisonChart';
import TimeWiseSalesChart from './pages/chart/TimeWiseSalesChart/TimeWiseSalesChart';
import TicketsReport from './pages/commet/TicketsReport';
import DayWiseOutletsPaymentsTotal from './pages/commet/DayWiseOutletsPaymentsTotal';
import MenuItemAccountDetails from './pages/commet/MenuItemAccountDetails';
import VoidGiftAndOthersReport from './pages/commet/VoidGiftAndOthersReport';
import EntityWiseSales from './pages/commet/EntityWiseSales';
import OutletWiseModifierSales from './pages/commet/OutletWiseModifierSales';
import OutletWiseItemSales_Modified from './pages/commet/OutletWiseItemSales_Modified';
import MonthWiseItemSales from './pages/commet/MonthWiseItemSales';
import OutletWiseItemSales from './pages/commet/OutletWiseItemSales';
import TicketWisePaymentsReport2 from './pages/commet/TicketWisePaymentsReport2';
import WorkPeriodDetails from './pages/sales_report/WorkPeriodDetails/WorkPeriodDetails';
import InvoiceWiseSales from './pages/sales_report/InvoiceWiseSales/InvoiceWiseSales';
import ItemSalesReport from './pages/item_wise_sales/ItemSalesReport/ItemSalesReport';
import { QueryClient, QueryClientProvider } from 'react-query';
import GiftDetails from './pages/markdown_reports/GiftDetails/GiftDetails';
import ItemSalesProfitLossRecipe from './pages/item_wise_sales/ItemSalesProfitLossRecipe/ItemSalesProfitLossRecipe';

const App = () => {
    const queryClient = new QueryClient();

    return (
        <>
            <QueryClientProvider client={queryClient}>
                <SidebarContextProvider>
                    <ChakraProvider theme={theme}>
                        <HashRouter>
                            <Sidebar />

                            <Switch>
                                <Route exact path="/login" component={Login} />
                                <Route exact path="/dashboard">
                                    <DashboardFilterContextProvider>
                                        <NewDashboard />
                                    </DashboardFilterContextProvider>
                                </Route>
                                <Route exact path="/loading" component={PageLoading} />
                                <Route exact path="/">
                                    <Redirect to="/login" />
                                </Route>

                                {/* chart reports*/}
                                <Route
                                    exact
                                    path="/day_wise_sales_chart"
                                    component={DailySalesChart}
                                />
                                <Route
                                    exact
                                    path="/month_wise_sales_chart"
                                    component={MonthlySalesChart}
                                />
                                <Route
                                    exact
                                    path="/outlet_comparison_chart"
                                    component={OutletComparisonChart}
                                />
                                <Route
                                    exact
                                    path="/time_wise_sales_chart"
                                    component={TimeWiseSalesChart}
                                />
                                <Route
                                    exact
                                    path="/day_and_time_wise_sales_chart"
                                    component={DayAndTimeWiseSalesChart}
                                />
                                <Route
                                    exact
                                    path="/yearly_comparison_chart"
                                    component={YearlyComparisonChart}
                                />

                                {/* sales reports */}
                                <Route
                                    exact
                                    path="/work_period_details"
                                    component={WorkPeriodDetails}
                                />
                                <Route
                                    exact
                                    path="/invoice_wise_sales"
                                    component={InvoiceWiseSales}
                                />

                                {/* item wise sales  */}
                                <Route
                                    exact
                                    path="/item_sales_report"
                                    component={ItemSalesReport}
                                />
                                <Route
                                    exact
                                    path="/item_sales_profit_loss_recipe"
                                    component={ItemSalesProfitLossRecipe}
                                />

                                {/* markdown reports  */}
                                <Route exact path="/gift_details_report" component={GiftDetails} />

                                {/* comet reports*/}
                                <Route exact path="/tickets_report" component={TicketsReport} />
                                <Route
                                    exact
                                    path="/day_wise_outlets_payments_total"
                                    component={DayWiseOutletsPaymentsTotal}
                                />
                                <Route
                                    exact
                                    path="/menu_item_account_details"
                                    component={MenuItemAccountDetails}
                                />
                                <Route
                                    exact
                                    path="/void_gift_and_others_report"
                                    component={VoidGiftAndOthersReport}
                                />
                                <Route
                                    exact
                                    path="/entity_wise_sales"
                                    component={EntityWiseSales}
                                />
                                <Route
                                    exact
                                    path="/outlet_wise_modifier_sales"
                                    component={OutletWiseModifierSales}
                                />
                                <Route
                                    exact
                                    path="/outlet_wise_item_sales_modified"
                                    component={OutletWiseItemSales_Modified}
                                />
                                <Route
                                    exact
                                    path="/month_wise_item_sales"
                                    component={MonthWiseItemSales}
                                />
                                <Route
                                    exact
                                    path="/outlet_wise_item_sales"
                                    component={OutletWiseItemSales}
                                />
                                <Route
                                    exact
                                    path="/ticket_wise_payments_report2"
                                    component={TicketWisePaymentsReport2}
                                />

                                <Route exact path="*" component={PageNotFound} />
                            </Switch>
                        </HashRouter>
                    </ChakraProvider>
                </SidebarContextProvider>
            </QueryClientProvider>
        </>
    );
};

export default App;
