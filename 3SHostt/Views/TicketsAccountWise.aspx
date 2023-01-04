<%@ Page Language="C#" Title="Tickets Account Wise" MasterPageFile="~/Views/ReportingTool.master"  AutoEventWireup="true" CodeFile="TicketsAccountWise.aspx.cs" Inherits="UI_TicketsAccountWise" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >

	<telerik:RadScriptManager runat="server" ID="RadScriptManager1" />


	<table>
		<tr>
			<td>
				<div class="bordered" style="width: 100%">
					<table width="100%" cellpadding="5" cellspacing="0" border="0">
						<tr>
							<td class="auto-style5"></td>
							<td class="auto-style6"></td>
							<td class="auto-style7"></td>
							<td class="auto-style7"></td>
							<td class="auto-style7"></td>
							<td class="auto-style7"></td>
							<td class="auto-style7"></td>
							<td class="auto-style7"></td>
						</tr>
						<tr>
							<td class="auto-style1"><strong>From Date:</strong></td>
							<td class="auto-style3">
								<telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Height="21px" Width="172px">
								</telerik:RadDateTimePicker>
							</td>
							<td class="style2">To Date:&nbsp;
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
							<td class="style2"></td>
							<td class="style2"></td>
							<td class="style2"></td>
							<td class="style2"></td>
							<td class="style2"></td>
						</tr>
						<tr>
							<td class="auto-style8">
								<asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
							</td>
							<td class="auto-style9">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Select
										:&nbsp;
										<asp:RadioButton ID="rbTicketsView" runat="server" GroupName="View" Style="font-weight: 700"
											Text="Tickets View" />
								<asp:RadioButton ID="rbDaysView" runat="server" GroupName="View" Style="font-weight: 700"
									Text="Days View" />
							</td>
							<td class="auto-style10">Outlets :
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
							<td class="auto-style10"></td>
							<td class="auto-style10"></td>
							<td class="auto-style10"></td>
							<td class="auto-style10"></td>
							<td class="auto-style10"></td>
						</tr>
						<tr>
							<td class="auto-style2">
								<asp:CheckBox ID="cbExactTime" runat="server"
									OnCheckedChanged="ChckedChanged" Text="Exact Time" />
							</td>
                            <td class="auto-style11">
                                        Departments:&nbsp; <telerik:RadComboBox RenderMode="Lightweight" ID="ddlTicketType" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Width="250" Skin="MetroTouch" >
                                
                                 </telerik:RadComboBox>
                                    </td>
							<td class="auto-style4">&nbsp;</td>
							<td>
								<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg"
									OnClick="btnSearch_Click" Width="173px" Height="62px"></asp:Button></td>
							<td>&nbsp;</td>
							<td height="30">&nbsp;</td>
							<td>&nbsp;</td>
							<td>&nbsp;</td>
							<td>&nbsp;</td>
						</tr>
					</table>
				</div>
			</td>
		</tr>
		<tr>
			<td style="padding-left: 5px;">&nbsp;</td>
		</tr>
		<tr>
			<td style="padding-left: 5px;">

				<rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1827px" Height="1062px"
					Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
					WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
				</rsweb:ReportViewer>

			</td>
		</tr>
	</table>

</asp:Content>
