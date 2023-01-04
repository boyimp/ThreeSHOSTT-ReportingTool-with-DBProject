<%@ Page Language="C#" Title="Work Period Report" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="WorkPeriodReport.aspx.cs" Inherits="UI_WorkPeriodReport" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >

	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>

	<telerik:RadDockLayout runat="server" ID="RadDockLayout1">
	</telerik:RadDockLayout>

	<table>
		<tr>
			<td>

					<table style="width: 70%;">
						<tr>
							<td class="auto-style18">
							</td>
							<td class="auto-style19">
							</td>
							<td align="right" class="auto-style20">
							</td>
							<td class="auto-style21">
							</td>
							<td class="auto-style22">
								</td>
						</tr>
						<tr>
							<td class="auto-style18">
								From Date:
							</td>
							<td class="auto-style19">
								<telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Height="21px" Width="172px">
								</telerik:RadDateTimePicker>
							</td>
							<td align="right" class="auto-style20">
								To Date:
							</td>
							<td class="auto-style21">
								<telerik:RadDateTimePicker ID="dtpToDate" runat="server" Height="21px" Width="184px">
<TimeView CellSpacing="-1"></TimeView>

<TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>

<Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" EnableWeekends="True" FastNavigationNextText="&amp;lt;&amp;lt;"></Calendar>

<DateInput DisplayDateFormat="M/d/yyyy" DateFormat="M/d/yyyy" LabelWidth="40%" Height="21px">
<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
</DateInput>

<DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
								</telerik:RadDateTimePicker>
							</td>
							<td class="auto-style22">
								</td>
						</tr>
						<tr>
							<td class="auto-style8">
								Outlets:</td>
							<td class="auto-style9">
								<rad:RadComboBox ID="ddlOutlets" runat="server" AllowCustomText="true" DataValueField="id"
									DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
									ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlOutlets_ItemDataBound"
									Skin="WebBlue" TabIndex="5" Width="200px">
									<HeaderTemplate>
										<table border="0" cellpadding="3" cellspacing="0" width="200">
											<tr>
												<td height="30">
													<b>Outlets</b>
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
							<td class="auto-style10">
								<asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="True" Checked="true"
									OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
								</td>
							<td class="auto-style11">
								</td>
							<td class="auto-style12">
								&nbsp;&nbsp;&nbsp;
							</td>
						</tr>
						<tr>
							<td class="auto-style13">
								Departments :</td>
							<td class="auto-style4">
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
							<td class="auto-style14">
								<asp:CheckBox ID="cbExactTime" runat="server"
									OnCheckedChanged="ChckedChanged" Text="Exact Time" />
								</td>
							<td class="auto-style15">
								<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg" OnClick="btnSearch_Click"
									Width="142px" Height="60px"></asp:Button>
							</td>
							<td class="auto-style16">
							</td>
						</tr>
					</table>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
		</tr>
		<asp:Label ID="lblWorkPeriod" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label>
		<tr>
			<td style="padding-left: 1px; width: 100%;">

		 <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1827px" Height="1346px"
							Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
							WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
		</rsweb:ReportViewer>

			</td>
		</tr>
	</table>
</asp:Content>
