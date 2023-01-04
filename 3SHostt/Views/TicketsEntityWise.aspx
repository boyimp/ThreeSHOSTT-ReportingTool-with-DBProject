<%@ Page Language="C#"  Title="Tickets Entity Wise" MasterPageFile="~/Views/ReportingTool.master"  AutoEventWireup="true" CodeFile="TicketsEntityWise.aspx.cs" Inherits="UI_TicketsEntityWise" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >

	<telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
		<table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>
						<div class="bordered" style="width:100%">
							<table width="100%" cellpadding="5" cellspacing="0" border="0">
								<tr>
									<td class="style3"><strong>From Date:</strong></td>
									<td class="style2"><telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker></td>
									<td class="style2">To Date:<telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker></td>
									<td class="style2"></td>
									<td class="style2"></td>
									<td class="style2">
									</td>
									<td class="style2">
									</td>
									<td class="style2"></td>
								</tr>
								<tr>
									<td>&nbsp;</td>
									<td>
										<asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="ChckedChanged" Text="Current Work Period"/>
									</td>
									<td><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg"
											OnClick="btnSearch_Click" Width="173px" Height="55px"></asp:Button></td>
									<td>&nbsp;</td>
									<td height="30">&nbsp;</td>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
									<td>&nbsp;</td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td style="padding-left: 5px;">
						&nbsp;</td>
				</tr>
				<tr>
					<td style="padding-left: 5px;">

		 <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1827px" Height="897px"
							Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
							WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
		</rsweb:ReportViewer>

					</td>
				</tr>
			</table>

 </asp:Content>

