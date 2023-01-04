<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="UI_NewDashboard" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.WebControls" Assembly="RadComboBox.Net2, Version=2.8.8.0, Culture=neutral, PublicKeyToken=175e9829b585f1f6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<telerik:RadScriptManager ID="RadScriptManager1" runat="server">
		<Scripts>
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
		</Scripts>
	</telerik:RadScriptManager>
	<script type="text/javascript">
		//Put your JavaScript code here.
	</script>
	<style>

		 .RadHtmlChart .k-chart {
			 border-radius: 3% !important;
		 }
	</style>
	<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
	</telerik:RadAjaxManager>
    <div>
        <div class="panel-body">
            <div class="btn-group btn-group-justified">
                <a class="btn btn-primary" id="daywisenetbtn" onclick="loadDayWiseNetSale()">Day Wise Net Sale</a>
                <a class="btn btn-primary" onclick="loadDayWiseGrossSale()">Day Wise Gross Sale</a>
                <a class="btn btn-primary" onclick="loadMonthlyNetSale()">Monthly Net Sale</a>
                <a class="btn btn-primary" onclick="loadMonthlyGrossSale()">Monthly Gross Sale</a>
            </div>
        </div>
        <strong>Outlet: </strong>
        <select id="dropdown" onchange="DropDownChanged(this.value);"></select>
        <br />
        <div id="spinnerChart" class="spinner" style="display: none;">
            <img src="loading.gif" alt="Loading" height="50" width="50" />
        </div>
        <br />
        <div id="Title" style="display: block; color: black; text-align: center; width: 100%; font-size: 2em;"></div>
        <div>
            <telerik:RadHtmlChart runat="server" ID="CommonChart" Transitions="false">

                <PlotArea>
                    <Series>
                        <telerik:ColumnSeries ColorField="Color" DataFieldY="Sales">
                        </telerik:ColumnSeries>

                    </Series>
                    <XAxis DataLabelsField="StartDate">
                        <MajorGridLines Visible="false"></MajorGridLines>
                        <MinorGridLines Visible="false"></MinorGridLines>
                        <LabelsAppearance RotationAngle="90"></LabelsAppearance>
                    </XAxis>
                    <YAxis>
                        <MajorGridLines Visible="false"></MajorGridLines>
                        <MinorGridLines Visible="false"></MinorGridLines>

                    </YAxis>
                </PlotArea>
            </telerik:RadHtmlChart>
        </div>
    </div>
	<script>
		var selectedReport = "1";
		function DropDownChanged(value) {
			if (window.selectedValue === "1") {
				loadDayWiseNetSale();
			}
			else if (window.selectedValue === "2") {
				loadDayWiseGrossSale();
			}
			else if (window.selectedValue === "3") {
				loadMonthlyNetSale();
			}
			else if (window.selectedValue === "4") {
				loadMonthlyGrossSale();
			}
		}
		var buildDropdown = function (result, dropdown, emptyMessage) {
			// Remove current options
			dropdown.html('');
			// Add the empty option with the empty message
			dropdown.append('<option value="0">' + emptyMessage + '</option>');
			// Check result isnt empty
			if (result != '') {
				// Loop through each of the results and append the option to the dropdown
				$.each(result, function (k, v) {
					dropdown.append('<option value="' + v.Id + '">' + v.Name + '</option>');
				});
			}
		}

		$.getJSON("../api/dashboard/GetOutlets", function (data) {
			var obj = jQuery.parseJSON(data);
			buildDropdown(
				obj,
				$('#dropdown'),
				'All'
			);
		});
		var loadDayWiseNetSale = function () {
			$('#spinnerChart').show();
			console.log("Dhukse");
			var value = WebForm_GetElementById('dropdown').value;
			$.getJSON("../api/dashboard/GetdailySales/" + value, function (data) {
				var obj = jQuery.parseJSON(data);
				var menuWiseSalesChart = $find('<%=CommonChart.ClientID %>');
				//Set the new datasource
				menuWiseSalesChart.set_dataSource(obj);

				//Turning animation on before repainting the chart
				menuWiseSalesChart.set_transitions(true);
				WebForm_GetElementById('Title').innerText = "Daily Net Sales";
				window.selectedReport = "1";
				//Redrawing the chart
				menuWiseSalesChart.repaint();
				$('#spinnerChart').hide();
			});
		}
		var loadDayWiseGrossSale = function () {
			$('#spinnerChart').show();
			console.log("Dhukse");
			var value = WebForm_GetElementById('dropdown').value;
			$.getJSON("../api/dashboard/GetDailyNetSales/" + value, function (data) {
				var obj = jQuery.parseJSON(data);
				var menuWiseSalesChart = $find('<%=CommonChart.ClientID %>');
				//Set the new datasource
				menuWiseSalesChart.set_dataSource(obj);

				//Turning animation on before repainting the chart
				menuWiseSalesChart.set_transitions(true);
				WebForm_GetElementById('Title').innerText = "Daily Gross Sales";
				//Redrawing the chart
				menuWiseSalesChart.repaint();
				window.selectedReport = "2";
				$('#spinnerChart').hide();
			});
		}
		var loadMonthlyGrossSale = function () {
			$('#spinnerChart').show();
			var value = WebForm_GetElementById('dropdown').value;

			$.getJSON("../api/dashboard/GetMonthlyGrossSales/" + value, function (data) {
				var obj = jQuery.parseJSON(data);
				var menuWiseSalesChart = $find('<%=CommonChart.ClientID %>');

				//Set the new datasource
				menuWiseSalesChart.set_dataSource(obj);

				//Turning animation on before repainting the chart
				menuWiseSalesChart.set_transitions(true);
				WebForm_GetElementById('Title').innerText = "Monthly Gross Sales";
				//Redrawing the chart
				menuWiseSalesChart.repaint();
				window.selectedReport = "4";
				$('#spinnerChart').hide();
			});
		}
		var loadMonthlyNetSale = function () {
			$('#spinnerChart').show();
			var value = WebForm_GetElementById('dropdown').value;
			$.getJSON("../api/dashboard/GetMonthlyNetSales/" + value, function (data) {
				var obj = jQuery.parseJSON(data);
				var menuWiseSalesChart = $find('<%=CommonChart.ClientID %>');
				//Set the new datasource
				menuWiseSalesChart.set_dataSource(obj);

				//Turning animation on before repainting the chart
				menuWiseSalesChart.set_transitions(true);
				WebForm_GetElementById('Title').innerText = "Monthly Net Sales";
				//Redrawing the chart
				menuWiseSalesChart.repaint();
				window.selectedReport = "3";
				$('#spinnerChart').hide();
			});
		}

		window.onresize = function () {
			$find("<%=CommonChart.ClientID%>").get_kendoWidget().resize();
		}
	</script>

</asp:Content>