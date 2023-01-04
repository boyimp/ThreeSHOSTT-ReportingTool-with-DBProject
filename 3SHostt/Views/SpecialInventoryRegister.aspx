<%@ Page Language="C#"  Title="Special Inventory Register" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="SpecialInventoryRegister.aspx.cs" Inherits="UI_SpecialInventoryRegister" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	 <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
		   <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>
						<div class="bordered" style="width:100%">
							<table width="100%" cellpadding="3" cellspacing="0" border="0">
								<tr>
									<td>From Date:</td>
									<td><telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker></td>
									<td align="right">To Date:</td>
									<td><telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker></td>
									<td> &nbsp;</td>
								</tr>
								<tr>
									<td align="right">Inventory Group Item:</td>
									<td><rad:RadComboBox Skin="WebBlue" id="ddlGroupItem" Runat="server" Height="170px" Width="200px" DataValueField="GroupCode"
											MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false" OnItemDataBound="ddlGroupItem_ItemDataBound"
											HighlightTemplatedItems="true" DropDownWidth="200px" onselectedindexchanged="ddlGroupItem_SelectedIndexChanged" AutoPostBack="true" ItemRequestTimeout="500" TabIndex="5">
											<HeaderTemplate>
												<table width="50" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td height="30"><b>Group Item</b></td>
													</tr>
												</table>
											</HeaderTemplate>
											<ItemTemplate>
												<table width="50" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td><%# DataBinder.Eval(Container.DataItem, "GroupCode")%></td>
													</tr>
												</table>
											</ItemTemplate>
										</rad:RadComboBox></td>
									<td align="right">Inventory Item:</td>
									<td>
										<rad:RadComboBox ID="ddlInventoryItem" Runat="server" AllowCustomText="true"
											DataValueField="id" DropDownWidth="200px" EnableLoadOnDemand="false"
											Height="170px" HighlightTemplatedItems="true" ItemRequestTimeout="500"
											MarkFirstMatch="true" OnItemDataBound="ddlInventoryItem_ItemDataBound"
											Skin="WebBlue" TabIndex="5" Width="200px">
											<HeaderTemplate>
												<table border="0" cellpadding="3" cellspacing="0" >
													<tr>
														<td height="30">
															<b>Item</b></td>
													</tr>
												</table>
											</HeaderTemplate>
											<ItemTemplate>
												<table border="0" cellpadding="3" cellspacing="0" >
													<tr>
														<td>
															<%# DataBinder.Eval(Container.DataItem, "Name")%>
														</td>
													</tr>
												</table>
											</ItemTemplate>
										</rad:RadComboBox>
									</td>
									<td></td>
								</tr>
								<tr>
									<td align="right">Warehouse:</td>
									<td>
										<rad:RadComboBox Skin="WebBlue" id="ddlWarehouse" Runat="server"
											Height="170px" Width="200px" DataValueField="id"
											MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false"
											HighlightTemplatedItems="true" DropDownWidth="200px" ItemRequestTimeout="500" TabIndex="5"
											OnItemDataBound="ddlWarehouse_ItemDataBound">
											<HeaderTemplate>
												<table width="200" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td height="30"><b>Group Item</b></td>
													</tr>
												</table>
											</HeaderTemplate>
											<ItemTemplate>
												<table width="200" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td>
															<%# DataBinder.Eval(Container.DataItem, "Name")%>
														</td>
													</tr>
												</table>
											</ItemTemplate>
										</rad:RadComboBox>
									</td>
									<td >&nbsp;</td>
									<td ><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg"
											OnClick="btnSearch_Click" Height="44px" Width="257px"></asp:Button>
									</td>
									<td></td>

								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="lblWorkPeriod" runat="server"
							style="font-size: large; font-weight: 700" Visible="False"></asp:Label>
					 </td>
				</tr>
				<tr>
					<td style="padding-left: 1px;width:100%;">

		 <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1862px" Height="972px">
		</rsweb:ReportViewer>
					</td>
				</tr>
			</table>

   </asp:Content>