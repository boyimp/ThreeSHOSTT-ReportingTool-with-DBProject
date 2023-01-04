<%@ Page Language="C#" Title="Entity Report" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="EntityReport.aspx.cs" Inherits="UI_EntityReport" %>
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
							<table cellpadding="5" cellspacing="0" border="0" style="width: 71%">
								<tr>
									<td class="style3"><strong>From Date:</strong></td>
									<td class="style6"><telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker></td>
									<td class="style4">To Date:&nbsp; <telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker></td>
									<td class="style2">
										<asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="ChckedChanged" Text="Current Work Period"/>
									</td>
								</tr>
								<tr>
									<td class="style8">
										<strong>Entity Type :</strong></td>
									<td class="style7">
										&nbsp;<rad:RadComboBox ID="ddlEntityType" runat="server" AllowCustomText="true" DataValueField="id"
									DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
									ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlEntityType_ItemDataBound"
									Skin="WebBlue" TabIndex="5" Width="200px"
											onselectedindexchanged="ddlEntityType_SelectedIndexChanged"
											AutoPostBack="True">
									<HeaderTemplate>
										<table border="0" cellpadding="3" cellspacing="0" width="200">
											<tr>
												<td height="30">
													<b>Entity Type</b>
												</td>
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
									<td class="style5">
										<strong>Entity :</strong>&nbsp;&nbsp;&nbsp;&nbsp;
								<rad:RadComboBox ID="ddlEntity" runat="server" AllowCustomText="true" DataValueField="id"
									DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
									ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlEntity_ItemDataBound"
									Skin="WebBlue" TabIndex="5" Width="200px"
											onselectedindexchanged="ddlEntity_SelectedIndexChanged">
									<HeaderTemplate>
										<table border="0" cellpadding="3" cellspacing="0" width="200">
											<tr>
												<td height="30">
													<b>Entity</b>
												</td>
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
									<td><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg"
											OnClick="btnSearch_Click" Width="173px" Height="55px"></asp:Button></td>
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

		 <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1827px" Height="1450px"
							Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
							WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
		</rsweb:ReportViewer>

					</td>
				</tr>
			</table>
</asp:Content>