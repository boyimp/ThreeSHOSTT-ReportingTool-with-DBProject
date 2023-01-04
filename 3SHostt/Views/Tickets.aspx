<%@ Page Language="C#" Title="" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="Tickets.aspx.cs" Inherits="UI_Tickets" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        #Content1 {
            background: blanchedalmond;
        }
    </style>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <div class="bordered" style="width: 100%">
                        <table width="100%" cellpadding="5" cellspacing="0" border="0">
                            <tr>
                                <td class="auto-style12"></td>
                                <td class="auto-style13"></td>
                                <td align="right" class="auto-style14"></td>
                                <td class="auto-style15"></td>
                                <td class="auto-style16"></td>
                            </tr>
                            <tr>
                                <td class="auto-style26">From Date:
                                </td>
                                <td class="auto-style27">
                                    <telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Height="21px" Width="172px">
                                    </telerik:RadDateTimePicker>
                                </td>
                                <td align="right" class="auto-style28">To Date:
                                </td>
                                <td class="auto-style29">
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
                                <td class="auto-style30"></td>
                            </tr>
                            <tr>
                                <td class="auto-style22">Ticket Type:
                                </td>
                                <td class="auto-style23">
                                    <rad:RadComboBox ID="ddlTicketType" runat="server" AllowCustomText="true" DataValueField="id"
                                        DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                        ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlTicketType_ItemDataBound"
                                        Skin="WebBlue" TabIndex="5" Width="200px">
                                        <HeaderTemplate>
                                            <table border="0" cellpadding="3" cellspacing="0" width="200">
                                                <tr>
                                                    <td height="30">
                                                        <b>Ticket Type</b>
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
                                <td class="auto-style11">
                                    <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="True" Checked="true"
                                        OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
                                </td>
                                <td class="auto-style24">
                                    <asp:CheckBox ID="cbExactTime" runat="server"
                                        OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                                </td>
                                <td class="auto-style25">&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style17">Outlets:
                                </td>
                                <td class="auto-style18">
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
                                <td class="auto-style19">
                                    <asp:CheckBox ID="chkOpen" runat="server" Checked="false" Text="Ony open tickets" />
                                </td>
                                <td class="auto-style20">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg" OnClick="btnSearch_Click"
                                        Width="142px" Height="66px"></asp:Button>
                                </td>
                                <td class="auto-style21"></td>
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
                <td style="padding-left: 1px; width: 100%;">
                    <telerik:RadGrid ID="RadGrid1" AllowSorting="True" RenderMode="Auto" FilterItemStyle-Width="150px"
                        AllowFilteringByColumn="True" ShowFooter="True"
                        AutoGenerateColumns="False" runat="server"
                        OnNeedDataSource="RadGrid1_NeedDataSource1" OnItemCommand="RadGrid1_ItemCommand"
                        OnColumnCreated="RadGrid1_ColumnCreated"
                        OnPdfExporting="RadGrid1_PdfExporting" OnGridExporting="RadGrid1_GridExporting"
                        OnHTMLExporting="RadGrid1_HTMLExporting" Skin="WebBlue">
                        <MasterTableView AutoGenerateColumns="true" DataKeyNames="id" ClientDataKeyNames="id">
                            <Columns>
                                <telerik:GridButtonColumn HeaderText="Detail" ButtonType="ImageButton" CommandName="Preview"
                                    ImageUrl="~/Images/view.png" ItemStyle-HorizontalAlign="Center" UniqueName="More">
                                    <HeaderStyle Width="50px" Wrap="true" />
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn HeaderText="Invoice" ButtonType="ImageButton" CommandName="InvoicePreview"
                                    ImageUrl="~/Images/view.png" ItemStyle-HorizontalAlign="Center" UniqueName="More2">
                                    <HeaderStyle Width="50px" Wrap="true" />
                                </telerik:GridButtonColumn>
                            </Columns>
                        </MasterTableView>
                        <ExportSettings>
                            <Pdf BorderType="AllBorders"></Pdf>
                            <Pdf BorderStyle="Medium"></Pdf>
                        </ExportSettings>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
