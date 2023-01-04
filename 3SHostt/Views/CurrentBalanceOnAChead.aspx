<%@ Page Language="C#" Title="Current Balance of Accounts Head" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="CurrentBalanceOnAChead.aspx.cs"
    Inherits="UI_CurrentBalanceOnAChead" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >

    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0">
                        <tr>
                            <td class="auto-style11">
                            </td>
                            <td class="auto-style12">
                            </td>
                            <td class="auto-style13">
                            </td>
                            <td class="auto-style16">
                            </td>
                            <td class="auto-style19">
                            </td>
                            <td class="auto-style15">
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style10">
                                From Date:
                            </td>
                            <td class="auto-style4">
                    <telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Height="21px" Width="172px">
                    </telerik:RadDateTimePicker>
                            </td>
                            <td class="auto-style5">
                                To Date:
                            </td>
                            <td class="auto-style17">
                    <telerik:RadDateTimePicker ID="dtpToDate" runat="server" Height="21px" Width="184px">
                        <TimeView CellSpacing="-1">
                        </TimeView>
                        <TimePopupButton HoverImageUrl="" ImageUrl="" />
                        <Calendar EnableWeekends="True" FastNavigationNextText="&amp;lt;&amp;lt;" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                        </Calendar>
                        <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" Height="21px" LabelWidth="40%">
                            <EmptyMessageStyle Resize="None" />
                            <ReadOnlyStyle Resize="None" />
                            <FocusedStyle Resize="None" />
                            <DisabledStyle Resize="None" />
                            <InvalidStyle Resize="None" />
                            <HoveredStyle Resize="None" />
                            <EnabledStyle Resize="None" />
                        </DateInput>
                        <DatePopupButton HoverImageUrl="" ImageUrl="" />
                    </telerik:RadDateTimePicker>
                            </td>
                            <td class="auto-style20">
                    <asp:CheckBox ID="cbExactTime" runat="server" OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                            </td>
                            <td class="auto-style3">
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style9">
                                Outlets:</td>
                            <td class="auto-style2">
                                <rad:RadComboBox ID="ddlOutlets" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlOutlets_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="145px">
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
                            <td class="auto-style8">
                                <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true"
                                    OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
                            </td>
                            <td class="auto-style18">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg" OnClick="btnSearch_Click"
                                    Width="193px" Height="66px"></asp:Button>
                            </td>
                            <td class="auto-style21">
                                &nbsp;</td>
                            <td class="auto-style7">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Excel.png" OnClick="ImgbtnExcel_Click"
                    AlternateText="Excel" ToolTip="Excel" />
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Word.png" OnClick="ImgbtnWord_Click"
                    AlternateText="Word" ToolTip="Word" />
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/PDF.png" OnClick="ImgbtnPDF_Click"
                    AlternateText="PDF" ToolTip="PDF" />
                <asp:Label ID="lblWorkPeriod" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 5px;">
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="true" CellSpacing="0"
                    GridLines="Both" OnNeedDataSource="RadGrid1_NeedDataSource1" ShowFooter="True"
                    OnColumnCreated="RadGrid1_ColumnCreated" OnPdfExporting="RadGrid1_PdfExporting" OnItemCommand="RadGrid1_ItemCommand">
                    <MasterTableView AutoGenerateColumns="true" DataKeyNames="AccountId, AccountName" ClientDataKeyNames="AccountId, AccountName">
                    <Columns>
                    <telerik:GridButtonColumn HeaderText="" ButtonType="ImageButton" CommandName="Preview" ImageUrl="~/Images/view.png"
                                        ItemStyle-HorizontalAlign="Center" UniqueName="Detail">
                                        <HeaderStyle Width="30px" Wrap="true"/>
                                    </telerik:GridButtonColumn>
                     </Columns>
                    </MasterTableView>

                    <ExportSettings>
                        <Pdf BorderType="AllBorders">
                        </Pdf>
                        <Pdf BorderStyle="Medium">
                        </Pdf>
                    </ExportSettings>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
  </asp:Content>