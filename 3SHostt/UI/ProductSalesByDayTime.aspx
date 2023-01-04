<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductSalesByDayTime.aspx.cs" Inherits="UI_ProductSalesByDayTime" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Sales by Day & Time</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style2
        {
            height: 47px;
        }
        .style5
        {
            width: 218px;
        }
        .style6
        {
            width: 103px;
        }
        .style7
        {
            width: 607px;
        }
        .style8
        {
            width: 336px;
        }
        .style9
        {
            width: 336px;
            height: 50px;
        }
        .style10
        {
            width: 218px;
            height: 50px;
        }
        .style11
        {
            width: 103px;
            height: 50px;
        }
        .style12
        {
            width: 607px;
            height: 50px;
        }
        .style13
        {
            height: 50px;
        }
        .auto-style7 {
            width: 250px;
            height: 50px;
        }
        .auto-style10 {
            width: 138px;
            text-align: right;
        }
        .auto-style12 {
            width: 250px;
        }
        .auto-style13 {
            width: 149px;
            height: 50px;
        }
        .auto-style14 {
            width: 149px;
            text-align: left;
        }
        .auto-style15 {
            width: 138px;
            text-align: right;
            height: 57px;
        }
        .auto-style16 {
            width: 250px;
            height: 57px;
        }
        .auto-style17 {
            width: 149px;
            height: 57px;
            text-align: right;
        }
        .auto-style18 {
            width: 239px;
            height: 57px;
        }
        .auto-style19 {
            height: 57px;
        }
    .RadPicker{vertical-align:middle}.RadPicker .rcTable{table-layout:auto}.RadPicker .RadInput{vertical-align:baseline}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}
        .auto-style25 {
            width: 239px;
            height: 50px;
        }
        .auto-style26 {
            width: 239px;
        }
        .auto-style27 {
            width: 138px;
            height: 50px;
        }
        .auto-style28 {
            height: 50px;
            width: 90px;
            text-align: right;
        }
        .auto-style29 {
            height: 57px;
            width: 90px;
        }
        .auto-style30 {
            width: 90px;
        }
.RadComboBox_MetroTouch{color:#000;font-size:16px;font-family:"Segoe UI",Arial,Helvetica,sans-serif}.RadComboBox{text-align:left;display:inline-block;vertical-align:middle;white-space:nowrap;*display:inline;*zoom:1}.RadComboBox_MetroTouch .rcbReadOnly .rcbInputCell{border-color:#cdcdcd;background-color:#f9f9f9;color:#000}.RadComboBox .rcbReadOnly .rcbInputCellLeft{background-position:0 -88px}.RadComboBox_MetroTouch .rcbInputCell{border-color:#cdcdcd;color:#000;background-color:#fff}.RadComboBox_MetroTouch .rcbInputCell{height:34px;border-width:1px;border-style:solid}.RadComboBox .rcbInputCellLeft{background-position:0 0}.RadComboBox .rcbInputCell{padding-right:4px;padding-left:5px;width:100%;height:20px;line-height:20px;text-align:left;vertical-align:middle}.RadComboBox .rcbInputCell{padding:0;border-width:0;border-style:solid;background-color:transparent;background-repeat:no-repeat}.RadComboBox_MetroTouch .rcbReadOnly .rcbArrowCellRight{border-left-width:0}.RadComboBox .rcbReadOnly .rcbArrowCellRight{background-position:-162px -176px}.RadComboBox_MetroTouch .rcbArrowCell{border-color:#cdcdcd;color:#000;background-color:#f9f9f9;width:28px}.RadComboBox_MetroTouch .rcbArrowCell{height:34px;border-width:1px;border-style:solid}.RadComboBox .rcbArrowCellRight{background-position:-18px -176px}.RadComboBox .rcbArrowCell{width:18px}.RadComboBox .rcbArrowCell{padding:0;border-width:0;border-style:solid;background-color:transparent;background-repeat:no-repeat}
    </style>
    <link rel="stylesheet" href="styles/kendo.common.min.css" />
    <link rel="stylesheet" href="styles/kendo.default.min.css" />
    <link rel="stylesheet" href="styles/kendo.default.mobile.min.css" />

    <script src="js/jquery.min.js"></script>
    <script src="js/kendo.all.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc:header ID="header1" runat="server"></uc:header>
    <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
        <table width="100%" cellpadding="5" cellspacing="0" 
    border="0">
            <tr>
                <td class="auto-style27" style="text-align: right">
                    From Date:
                </td>
                <td class="auto-style7">
                    <telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Height="21px" Width="172px">
                    </telerik:RadDateTimePicker>
                </td>
                <td align="right" class="auto-style13">
                    To Date:
                </td>
                <td class="auto-style25">
                    <telerik:RadDateTimePicker ID="dtpToDate" runat="server" Height="21px" Width="184px">
                     
                    </telerik:RadDateTimePicker>
                </td>
                <td class="auto-style28">
                    Outlet :</td>
                <td class="style13">
                    <rad:RadComboBox ID="ddlOutlets" runat="server" AllowCustomText="true" DataValueField="id" DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true" ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlOutlets_ItemDataBound" Skin="WebBlue" TabIndex="5" Width="200px">
                        <HeaderTemplate>
                            <table border="0" cellpadding="3" cellspacing="0" width="200">
                                <tr>
                                    <td height="30"><b>Outlets</b> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table border="0" cellpadding="3" cellspacing="0" width="200">
                                <tr>
                                    <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </rad:RadComboBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style15">
                    Group:</td>
                <td class="auto-style16">
                    <telerik:RadComboBox ID="ddlGroupItem" runat="server" CheckBoxes="true" DataTextField="Name" EnableCheckAllItemsCheckBox="true" RenderMode="Lightweight" Skin="MetroTouch" Width="250">
                    </telerik:RadComboBox>
                </td>
                <td class="auto-style17">
                    Product:</td>
                <td class="auto-style18">
                    <rad:RadComboBox ID="ddlMenuItem" runat="server" AllowCustomText="true" DataValueField="id" DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true" ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlMenuItem_ItemDataBound" Skin="WebBlue" TabIndex="5" Width="200px">
                        <HeaderTemplate>
                            <table border="0" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td height="30"><b>Item</b> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table border="0" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td><%# DataBinder.Eval(Container.DataItem, "Name")%></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </rad:RadComboBox>
                </td>
                <td class="auto-style29">
                    &nbsp;&nbsp;&nbsp; Department:</td>
                <td class="auto-style19">
                    <rad:RadComboBox ID="ddlDepartment" Runat="server" AllowCustomText="true" DataValueField="id" DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true" ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlDepartment_ItemDataBound" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" Skin="WebBlue" TabIndex="5" Width="200px">
                        <HeaderTemplate>
                            <table border="0" cellpadding="3" cellspacing="0" width="200">
                                <tr>
                                    <td height="30"><b>Department</b></td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table border="0" cellpadding="3" cellspacing="0" width="200">
                                <tr>
                                    <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </rad:RadComboBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style10">&nbsp;</td>
                <td class="auto-style12">
                    <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="True" Checked="true" OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
                </td>
                <td class="auto-style14">
                    <asp:CheckBox ID="cbExactTime" runat="server" OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                </td>
                <td class="auto-style26">
                    <asp:Button ID="btnSearch" runat="server" CssClass="buttonGrey" Height="69px" OnClick="btnSearch_Click" Text="Search" Width="212px" />
                </td>
                <td class="auto-style30">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </telerik:RadDockLayout>
       <%-- <%# DataBinder.Eval(Container.DataItem, "Name") %>--%>
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="style2">
                       
 
            </td>
        </tr>
        <asp:Label ID="lblWorkPeriod" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label>
        <tr>
            <td style="padding-left: 1px; width: 100%;">
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="2000px" Height="3000px" 
                            style="margin-right: 0px">
        </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
      <%--  <%# DataBinder.Eval(Container.DataItem, "Name")%>--%>
    <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
