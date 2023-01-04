<%@ Page Language="C#"  Title="Production Cost Analysis" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="ProductionCost.aspx.cs" Inherits="UI_ProductionCost" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	<uc:header id="header1" runat="server"></uc:header>

	<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>--%>

		  <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td class="auto-style2">
						<div class="bordered" style="width:100%">
							<table width="100%" cellpadding="3" cellspacing="0" border="0">
								<tr>
									<td>From Date:</td>
									<td class="auto-style1"><telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker></td>
									<td align="right">To Date:</td>
									<td><telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker></td>
									<td> &nbsp;</td>
								</tr>
								<tr>
									<td align="right">&nbsp;</td>
									<td class="auto-style1">&nbsp;</td>
									<td align="right">&nbsp;</td>
									<td>
										&nbsp;</td>
									<td></td>
								</tr>
								<tr>
									<td align="right">&nbsp;</td>
									<td class="auto-style1" align="right">
										<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg"
											OnClick="btnSearch_Click" Height="52px" Width="257px" BackColor="#6ECFED"></asp:Button>
									</td>
									<td >&nbsp;</td>
									<td >&nbsp;</td>
									<td></td>

								</tr>
							</table>
						</div>
					</td>
				</tr>
				</table>
						</div>
					</td>
				</tr>
				<tr>
					<td style="padding-left: 1px;" class="auto-style3">
						&nbsp;</td>
				</tr>
			</table>

	   <%-- <%# DataBinder.Eval(Container.DataItem, "Name")%>--%>

						<telerik:RadGrid ID="RadGrid1" AllowSorting="true" FilterItemStyle-Width = "150px" AllowFilteringByColumn="False" ShowFooter="True"
						 AutoGenerateColumns="false" runat="server" OnItemDatabound ="RadGrid1_ItemDataBound" OnNeedDataSource="RadGrid1_NeedDataSource1" OnItemCommand="RadGrid1_ItemCommand" OnColumnCreated ="RadGrid1_ColumnCreated" >
							<MasterTableView AutoGenerateColumns="true" DataKeyNames="ID,Description,Value" ClientDataKeyNames="ID,Description,Value" Font-Size="12" Font-Bold="true">
								<Columns>
									<telerik:GridButtonColumn HeaderText="" ButtonType="ImageButton" CommandName="Preview" ImageUrl="~/Images/view.png"
										ItemStyle-HorizontalAlign="Center" UniqueName="Detail">
										<HeaderStyle Width="30px" Wrap="true"/>
									</telerik:GridButtonColumn>
									<%--<telerik:GridBoundColumn DataField="InventoryItemName" HeaderText="Inventory Item Name"  />
									<telerik:GridBoundColumn DataField="Unit" HeaderText="Unit"  />
									<telerik:GridNumericColumn DataField="InStock" HeaderText="InStock" DataType="System.Decimal" />
									<telerik:GridNumericColumn DataField="Purchase" HeaderText="Purchase" DataType="System.Decimal" />
									<telerik:GridNumericColumn DataField="Cost" HeaderText="Cost" DataType="System.Decimal" />
									<telerik:GridNumericColumn DataField="InventoryConsumption" HeaderText="Inventory Consumption" DataType="System.Decimal" />
									<telerik:GridNumericColumn DataField="InventoryPrediction" HeaderText="Inventory Prediction" DataType="System.Decimal" />
									<telerik:GridNumericColumn DataField="CurrentInventory" HeaderText="Current Inventory" DataType="System.Decimal" Aggregate="Sum" FooterText="Total:" />--%>
									<%--<telerik:GridBoundColumn DataField="Date" DataFormatString="{0:d}" HeaderText="Date"
									DataType="System.DateTime" />
									<telerik:GridCheckBoxColumn DataField="Discontinued" HeaderText="Discontinued" DataType="System.Boolean" />--%>
								</Columns>
							</MasterTableView>
						 </telerik:RadGrid>

</asp:Content>

