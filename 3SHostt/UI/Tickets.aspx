<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tickets.aspx.cs" Inherits="UI_Tickets" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tickets</title>
    <!-- CSS only -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT" crossorigin="anonymous">
   
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    
    <style type="text/css">
        .auto-style11 {
            height: 74px;
            width: 177px;
            text-align: left;
        }
        .auto-style12 {
            width: 108px;
            text-align: right;
            height: 36px;
        }
        .auto-style13 {
            width: 304px;
            height: 36px;
        }
        .auto-style14 {
            width: 177px;
            height: 36px;
        }
        .auto-style15 {
            width: 212px;
            height: 36px;
        }
        .auto-style16 {
            height: 36px;
        }
        .auto-style17 {
            width: 108px;
            text-align: right;
            height: 51px;
        }
        .auto-style18 {
            width: 304px;
            height: 51px;
        }
        .auto-style19 {
            width: 177px;
            height: 51px;
            text-align: left;
        }
        .auto-style20 {
            width: 212px;
            height: 51px;
        }
        .auto-style21 {
            height: 51px;
        }
        .auto-style22 {
            width: 108px;
            text-align: right;
            height: 74px;
        }
        .auto-style23 {
            width: 304px;
            height: 74px;
        }
        .auto-style24 {
            width: 212px;
            height: 74px;
        }
        .auto-style25 {
            height: 74px;
        }
        .auto-style26 {
            width: 108px;
            text-align: right;
            height: 55px;
        }
        .auto-style27 {
            width: 304px;
            height: 55px;
        }
        .auto-style28 {
            width: 177px;
            height: 55px;
        }
        .auto-style29 {
            width: 212px;
            height: 55px;
        }
        .auto-style30 {
            height: 55px;
        }
        .searchBtn{
            height:45px!important;
            width:120px!important;
            background-color:#FA7242;
            color:white;
            font-weight: bold;
        }
        .searchBtn:hover{
            background-color:#f86530!important;
            color:white!important;
        }
        .filterContainer{
            margin:20px;
        }
        #ImageButton1,
        #ImageButton2,
        #ImageButton3{
            width:35px!important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager1" runat="server" EnableTheming="True">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
        </telerik:RadStyleSheetManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
    <uc:header ID="header1" runat="server"></uc:header>
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>

    <table width="100%" cellpadding="0" cellspacing="0" border="0" >
        <tr>
            <td>
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0" class="filterContainer">
                        <tr>
                            <td class="auto-style26">
                                From Date:
                            </td>
                            <td class="auto-style27">
                                <telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Height="21px" Width="172px">
                                </telerik:RadDateTimePicker>
                            </td>
                            <td align="right" class="auto-style28">
                                To Date:
                            </td>
                            <td class="auto-style29">
                                <telerik:RadDateTimePicker ID="dtpToDate" runat="server" Height="21px" Width="184px">

                                </telerik:RadDateTimePicker>
                            </td>
                            <td class="auto-style30">
                                </td>
                        </tr>
                        <tr>
                            <td class="auto-style22">
                                Ticket Type:
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
                            <td class="auto-style25">
                                &nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style17">
                                Outlets:
                            </td>
                            <td class="auto-style18">
                                <rad:RadComboBox ID="ddlOutlets" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlOutlets_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="200px" style="text-decoration: underline">
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
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey btn searchBtn" OnClick="btnSearch_Click" Width="142px" Height="66px"></asp:Button>
                            </td>
                            <td class="auto-style21"> </td>
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
                <telerik:RadGrid ID="RadGrid1" AllowSorting="True" FilterItemStyle-Width="150px"
                    AllowFilteringByColumn="True" ShowFooter="True" runat="server"
                    OnNeedDataSource="RadGrid1_NeedDataSource1" OnItemCommand="RadGrid1_ItemCommand"
                    OnColumnCreated="RadGrid1_ColumnCreated" 
                    OnPdfExporting="RadGrid1_PdfExporting" ongridexporting="RadGrid1_GridExporting" 
                    onhtmlexporting="RadGrid1_HTMLExporting" Skin="Bootstrap">
                    <ClientSettings AllowColumnsReorder="True" AllowDragToGroup="True" ReorderColumnsOnClient="True">
                    </ClientSettings>
                    <MasterTableView DataKeyNames="id" ClientDataKeyNames="id" AllowPaging="True">
                        <Columns>
                            <telerik:GridButtonColumn HeaderText="Detail" ButtonType="ImageButton" CommandName="Preview"
                                ImageUrl="~/Images/view.png" ItemStyle-HorizontalAlign="Center" UniqueName="More">
                                <HeaderStyle Width="50px" Wrap="true" />

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </telerik:GridButtonColumn>
                            <telerik:GridButtonColumn HeaderText="Invoice" ButtonType="ImageButton" CommandName="InvoicePreview"
                                ImageUrl="~/Images/view.png" ItemStyle-HorizontalAlign="Center" UniqueName="More2">
                                <HeaderStyle Width="50px" Wrap="true" />

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
<GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>

                    <ExportSettings>
                        <Pdf BorderType="AllBorders"></Pdf>
                        <Pdf BorderStyle="Medium"></Pdf>
                    </ExportSettings>

<FilterItemStyle Width="150px"></FilterItemStyle>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
        <%--<%# DataBinder.Eval(Container.DataItem, "Name") %>--%>
    <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
