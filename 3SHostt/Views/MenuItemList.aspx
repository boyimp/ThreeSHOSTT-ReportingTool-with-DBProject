<%@ Page Language="C#"   Title="Menu Item List" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="MenuItemList.aspx.cs" Inherits="UI_MenuItemList" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

	<telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
		<table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>
						<div class="bordered" style="width:100%">
							<table width="100%" cellpadding="5" cellspacing="0" border="0">
								<tr>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td height="30">Department</td>
									<td>
										<rad:RadComboBox ID="ddlTicketType" Runat="server" AllowCustomText="true"
											DataValueField="id" DropDownWidth="200px" EnableLoadOnDemand="false"
											Height="170px" HighlightTemplatedItems="true" ItemRequestTimeout="500"
											MarkFirstMatch="true" OnItemDataBound="ddlTicketType_ItemDataBound"
											Skin="WebBlue" TabIndex="5" Width="200px">
											<HeaderTemplate>
												<table border="0" cellpadding="3" cellspacing="0" width="200">
													<tr>
														<td height="30">
															<b>Department</b></td>
													</tr>
												</table>
											</HeaderTemplate>
											<ItemTemplate>
												<table border="0" cellpadding="3" cellspacing="0" width="200">
													<tr>
														<td>
															<%# DataBinder.Eval(Container.DataItem, "Name") %>
														</td>
													</tr>
												</table>
											</ItemTemplate>
										</rad:RadComboBox>
									</td>
									<td>
										&nbsp;</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td class="style2"></td>
									<td class="style2"></td>
									<td class="style2"></td>
									<td class="style2"></td>
									<td class="style2"></td>
									<td class="style2">
										<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg"
											OnClick="btnSearch_Click" Width="173px" Height="78px"></asp:Button>
									</td>
									<td class="style2">
									</td>
									<td class="style2"></td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr><td>
						&nbsp;</td></tr>
				<tr>
					<td style="padding-left: 5px;">
						&nbsp;</td>
				</tr>
				<tr>
					<td style="padding-left: 5px;">
						&nbsp;</td>
				</tr>
				<tr>
					<td style="padding-left: 5px;">

		 <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1490px" Height="1782px"
							Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
							WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
		</rsweb:ReportViewer>

					</td>
				</tr>
				<tr>
					<td style="padding-left: 5px;">

						<asp:Label ID="lblPath" runat="server" Text="Label"></asp:Label>

					</td>
				</tr>
			</table>
</asp:Content>


