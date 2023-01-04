<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VoidOrders.aspx.cs" Inherits="UI_VoidOrders" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Void Order Entries</title>
        <!-- CSS only -->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT" crossorigin="anonymous">
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style2
        {
            width: 196px;
        }
        .style3
        {
            width: 139px;
        }
        .style4
        {
            width: 161px;
        }
        .style5
        {
            width: 404px;
        }
        .style6
        {
            height: 95px;
        }
        .style7
        {
            width: 139px;
            height: 53px;
        }
        .style8
        {
            width: 196px;
            height: 53px;
        }
        .style9
        {
            width: 161px;
            height: 53px;
        }
        .style10
        {
            width: 404px;
            height: 53px;
        }
        .style11
        {
            height: 53px;
        }
        .auto-style8 {
            width: 150px;
            text-align: right;
            height: 67px;
        }
        .auto-style9 {
            width: 295px;
            text-align: left;
            font-size: larger;
            height: 67px;
        }
        .auto-style10 {
            width: 168px;
            height: 67px;
            text-align: left;
        }
        .auto-style11 {
            width: 200px;
            height: 67px;
        }
        .auto-style12 {
            height: 67px;
        }
        .auto-style13 {
            width: 139px;
            text-align: right;
        }
        .auto-style14 {
            width: 161px;
            text-align: left;
        }
        
.filterContainer {
  margin: 20px;
}
.searchBtn {
  height: 45px !important;
  width: 120px !important;
  background-color: #fa7242;
  color: white;
  font-weight: bold;
}
.searchBtn:hover {
  background-color: #f86530 !important;
  color: white !important;
}

#ImageButton1,
#ImageButton2,
#ImageButton3 {
  width: 35px !important;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc:header ID="header1" runat="server"></uc:header>
    <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
    </telerik:RadDockLayout>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="style6">
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0" 
                        style="height: 134px" class="filterContainer">
                        <tr>
                            <td class="style7">
                                From Date:
                            </td>
                            <td class="style8">
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td align="right" class="style9">
                                To Date:
                            </td>
                            <td class="style10">
                                <telerik:RadDatePicker ID="dtpToDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td class="style11">
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
                                <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true"
                                    OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
                                </td>
                           <td class="auto-style11">
                                        <asp:RadioButton ID="rbItemWise" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Item Wise" Checked="True" />
                                        <asp:RadioButton ID="rbGroupWise" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Group Wise" />
                                </td>
                            <td class="auto-style12">
                        </tr>
                        <tr>
                            <td class="auto-style13">
                                Departments :</td>
                            <td class="style2">
                                &nbsp;&nbsp;&nbsp;
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlTicketType" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Width="250" Skin="MetroTouch" >
                                
                                 </telerik:RadComboBox>
                            </td>
                            <td class="auto-style14">
                                <asp:CheckBox ID="cbExactTime" runat="server"
                                    OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                            </td>
                            <td class="style5">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey btn searchBtn" OnClick="btnSearch_Click"
                                    Width="142px" Height="60px" ></asp:Button>
                            </td>
                            <td>
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
             <td style="padding-left: 5px; width: 100%;">
                <telerik:RadGrid ID="RadGrid1" AllowSorting="true" FilterItemStyle-Width="150px" 
                    runat="server" AllowFilteringByColumn="true" CellSpacing="0"
                    GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource1" OnColumnCreated="RadGrid1_ColumnCreated" OnItemCommand="RadGrid1_ItemCommand"
                    ShowFooter="True" OnPdfExporting="RadGrid1_PdfExporting">
                    <MasterTableView AutoGenerateColumns="true" DataKeyNames="TicketId" ClientDataKeyNames="TicketId"
                        Height="100%">
                        <Columns>
                         <telerik:GridButtonColumn HeaderText="Detail" ButtonType="ImageButton" CommandName="Preview"
                                ImageUrl="~/Images/view.png" ItemStyle-HorizontalAlign="Center" UniqueName="More">
                                <HeaderStyle Width="50px" Wrap="true" />
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
        <tr>
            <td>
            </td>
        </tr>
    </table>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
