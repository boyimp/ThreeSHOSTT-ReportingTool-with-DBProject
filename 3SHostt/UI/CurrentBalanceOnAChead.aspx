<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CurrentBalanceOnAChead.aspx.cs"
    Inherits="UI_CurrentBalanceOnAChead" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Current Balance of Accounts Head</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style2
        {
            width: 28px;
        }
        .style3
        {
            width: 510px;
        }
        .style4
        {
            width: 498px;
        }
        .auto-style2 {
            width: 210px;
            height: 31px;
        }
        .auto-style3 {
            height: 77px;
        }
        .auto-style4 {
            width: 210px;
            height: 77px;
        }
        .auto-style5 {
            width: 139px;
            height: 77px;
        }
        .auto-style7 {
            height: 31px;
        }
        .auto-style8 {
            width: 139px;
            height: 31px;
        }
        .auto-style9 {
            height: 31px;
            text-align: right;
            width: 81px;
        }
        .auto-style10 {
            height: 77px;
            text-align: right;
            width: 81px;
        }
        .auto-style11 {
            height: 23px;
            text-align: right;
            width: 81px;
        }
        .auto-style12 {
            width: 210px;
            height: 23px;
        }
        .auto-style13 {
            width: 139px;
            height: 23px;
        }
        .auto-style15 {
            height: 23px;
        }
    .RadPicker{vertical-align:middle}
    .RadPicker{vertical-align:middle}.RadPicker{vertical-align:middle}.RadPicker .rcTable{table-layout:auto}.RadPicker .rcTable{table-layout:auto}.RadPicker .rcTable{table-layout:auto}.RadPicker .RadInput{vertical-align:baseline}.RadPicker .RadInput{vertical-align:baseline}.RadPicker .RadInput{vertical-align:baseline}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}
        .RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}
        .auto-style16 {
            width: 198px;
            height: 23px;
        }
        .auto-style17 {
            width: 198px;
            height: 77px;
        }
        .auto-style18 {
            width: 198px;
            height: 31px;
        }
        .auto-style19 {
            height: 23px;
            width: 140px;
        }
        .auto-style20 {
            height: 77px;
            width: 140px;
        }
        .auto-style21 {
            height: 31px;
            width: 140px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <uc:header ID="header1" runat="server"></uc:header>
    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
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
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey" OnClick="btnSearch_Click"
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
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
