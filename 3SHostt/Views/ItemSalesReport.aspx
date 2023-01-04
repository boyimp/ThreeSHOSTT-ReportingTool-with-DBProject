<%@ Page Language="C#"  Title="Items Sales Report" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="ItemSalesReport.aspx.cs"
    Inherits="UI_ItemSalesReport" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="style2">
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0"
                        style="height: 100px; margin-bottom: 5px">
                        <tr>
                            <td class="auto-style22"></td>
                            <td class="auto-style20">&nbsp;</td>
                            <td class="auto-style4"></td>
                            <td class="auto-style16"></td>
                            <td class="auto-style14"></td>
                            <td class="style3"></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="auto-style5">From Date:
                            </td>
                            <td class="auto-style21">
                                <telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Height="21px" Width="172px">
                                </telerik:RadDateTimePicker>
                            </td>
                            <td class="auto-style19">To Date:
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
                            <td class="auto-style15">Department
                                :
                            </td>
                            <td class="auto-style9">
                                <rad:RadComboBox ID="ddlTicketType" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlTicketType_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="200px">
                                    <HeaderTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0" width="200">
                                            <tr>
                                                <td height="30">
                                                    <b>Department</b>
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
                            <td class="auto-style10">&nbsp;</td>
                            <td class="auto-style10"></td>
                        </tr>
                        <tr>
                            <td class="auto-style22">Group :</td>
                            <td class="auto-style20">
                                <rad:RadComboBox Skin="WebBlue" ID="ddlGroupItem" runat="server" Height="170px" Width="200px"
                                    DataValueField="GroupCode" MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false"
                                    OnItemDataBound="ddlGroupItem_ItemDataBound" HighlightTemplatedItems="true" DropDownWidth="200px"
                                    OnSelectedIndexChanged="ddlGroupItem_SelectedIndexChanged" AutoPostBack="true"
                                    ItemRequestTimeout="500" TabIndex="5">
                                    <HeaderTemplate>
                                        <table width="50" cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td height="30">
                                                    <b>Group Item</b>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table width="50" cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <%# DataBinder.Eval(Container.DataItem, "GroupCode")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </rad:RadComboBox>
                            </td>
                            <td class="auto-style4">Menu Item:</td>
                            <td class="auto-style16">
                                <rad:RadComboBox ID="ddlMenuItem" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlMenuItem_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="200px">
                                    <HeaderTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td height="30">
                                                    <b>Item</b>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </rad:RadComboBox>
                            </td>
                            <td height="30" class="auto-style14">Outlet :</td>
                            <td class="style3">
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
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style23"></td>
                            <td class="auto-style24"></td>
                            <td class="auto-style25"></td>
                            <td class="auto-style26">
                                <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true"
                                    OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
                            </td>
                            <td class="auto-style27">
                                <asp:CheckBox ID="cbExactTime" runat="server" OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                            </td>
                            <td class="auto-style28">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg" OnClick="btnSearch_Click"
                                    Width="229px" Height="66px"></asp:Button>
                            </td>
                            <td class="auto-style29"></td>
                            <td class="auto-style29"></td>
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
                <asp:Label ID="lblWorkPeriod" runat="server" Text="Label" Style="font-size: medium; font-weight: 700"></asp:Label>
                <asp:Button ID="btnSearch0" runat="server" Text="Re-Process" CssClass="btn btn-primary btn-lg"
                    OnClick="btnReprocess_Click" Width="173px"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 5px;">
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="true" CellSpacing="0"
                    GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource1" ShowFooter="True"
                    OnColumnCreated="RadGrid1_ColumnCreated" OnItemCommand="RadGrid1_ItemCommand"
                    OnPdfExporting="RadGrid1_PdfExporting">
                    <MasterTableView AutoGenerateColumns="true" DataKeyNames="ItemId, PortionName" ClientDataKeyNames="ItemId, PortionName"
                        Height="100%">
                        <Columns>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" ImageUrl="~/Images/view.png"
                                ItemStyle-HorizontalAlign="Center" UniqueName="More">
                                <HeaderStyle Width="40px" />
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