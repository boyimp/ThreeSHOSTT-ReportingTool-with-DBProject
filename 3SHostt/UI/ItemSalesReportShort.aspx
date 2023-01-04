<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemSalesReportShort.aspx.cs" Inherits="UI_ItemSalesReportShort" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Item Sales Report Short</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style2
        {
            width: 181px;
        }
        .style3
        {
            width: 265px;
        }
        .style4
        {
            width: 96px;
        }
        .auto-style4 {
            width: 295px;
            text-align: left;
            font-size: larger;
            height: 79px;
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
            width: 281px;
            height: 67px;
        }
        .auto-style12 {
            height: 67px;
        }
        .auto-style13 {
            width: 150px;
            text-align: right;
            height: 79px;
        }
        .auto-style14 {
            width: 168px;
            height: 79px;
            text-align: left;
        }
        .auto-style15 {
            width: 281px;
            height: 79px;
        }
        .auto-style16 {
            height: 79px;
        }
        .auto-style18 {
            width: 150px;
            text-align: right;
            height: 46px;
        }
        .auto-style19 {
            width: 295px;
            height: 46px;
        }
        .auto-style20 {
            width: 168px;
            height: 46px;
        }
        .auto-style21 {
            width: 281px;
            height: 46px;
        }
        .auto-style22 {
            height: 46px;
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
            <td>
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0" 
                        style="height: 119px">
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
                                &nbsp;Select :&nbsp;
                                        <asp:RadioButton ID="rbDepartmentWise" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Department Wise" />
                                        <asp:RadioButton ID="rbMenuWise" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Menu Wise" />
                                </td>
                            <td class="auto-style12">
                                &nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style13">
                                Departments :</td>
                            <td class="auto-style4">
										 <telerik:RadComboBox RenderMode="Lightweight" ID="ddlTicketType" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Width="250" Skin="MetroTouch" >
                                
                                 </telerik:RadComboBox>
                            </td>
                            <td class="auto-style14">
                                <asp:CheckBox ID="cbExactTime" runat="server"
                                    OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                                </td>
                            <td class="auto-style15">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey" OnClick="btnSearch_Click"
                                    Width="200px" Height="60px"></asp:Button>
                            </td>
                            <td class="auto-style16">
                            </td>
                        </tr>
                    </table>
                </div>
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
        <%--<%# DataBinder.Eval(Container.DataItem, "Name") %>--%>
    <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
