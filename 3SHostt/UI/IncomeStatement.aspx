<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IncomeStatement.aspx.cs" Inherits="UI_IncomeStatement" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Income Statement</title>
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
            height: 35px;
        }
        .auto-style9 {
            width: 295px;
            text-align: left;
            font-size: larger;
            height: 35px;
        }
        .auto-style10 {
            width: 168px;
            height: 35px;
            text-align: left;
        }
        .auto-style11 {
            width: 200px;
            height: 35px;
        }
        .auto-style12 {
            height: 35px;
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
            width: 200px;
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
            width: 200px;
            height: 46px;
        }
        .auto-style22 {
            height: 46px;
        }
        .auto-style23 {
            width: 150px;
            text-align: right;
            height: 34px;
        }
        .auto-style24 {
            width: 295px;
            height: 34px;
        }
        .auto-style25 {
            width: 168px;
            height: 34px;
        }
        .auto-style26 {
            width: 200px;
            height: 34px;
        }
        .auto-style27 {
            height: 34px;
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
                            <td class="auto-style23">
                            </td>
                            <td class="auto-style24">
                            </td>
                            <td align="right" class="auto-style25">
                            </td>
                            <td class="auto-style26">
                            </td>
                            <td class="auto-style27">
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
                                Outlets:<br />
                                <br />
                                <br />
                                <br />
                                Department Tag:</td>
                            <td class="auto-style9">
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlOutlets" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Width="250" Skin="MetroTouch" OnSelectedIndexChanged="ddlOutlets_SelectedIndexChanged" >
                                
                                 </telerik:RadComboBox>
                                <br />
                                <br />
                                <br />
                                <br />
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlTicketType" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
								Width="250" Skin="MetroTouch" >								
								 </telerik:RadComboBox>
                            </td>
                            <td class="auto-style10">
                                <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="True" Checked="true"
                                    OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
                                </td>
                            <td class="auto-style11">
                                Production Overhead %:
                                <asp:TextBox ID="txtProductionOverhead" runat="server" TextMode="Number" Width="127px" ValidateRequestMode="Disabled">7.0</asp:TextBox>
                                <br />
                                Marketing Cost %:
                                <asp:TextBox ID="txtMarketingCost" runat="server" TextMode="Number" Width="127px" ValidateRequestMode="Disabled">6.0</asp:TextBox>
                                <br />
                                Depreciation %:
                                <asp:TextBox ID="txtDepreciation" runat="server" TextMode="Number" Width="127px" ValidateRequestMode="Disabled">1.5</asp:TextBox>
                                <br />
                                Royalty %:<br />
                                <asp:TextBox ID="txtRoyalty" runat="server" TextMode="Number" Width="127px" ValidateRequestMode="Disabled">5.0</asp:TextBox>
                                </td>
                            <td class="auto-style12">
                                <br />
                                Inventory Group:
&nbsp;<telerik:RadComboBox RenderMode="Lightweight" ID="ddlGroupItem" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Width="250" Skin="MetroTouch" >
                                
                                 </telerik:RadComboBox>
                                    <br />
                                <br />
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Brand :&nbsp;<telerik:RadComboBox RenderMode="Lightweight" ID="ddlBrand" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Width="250" Skin="MetroTouch" >
                                
                                 </telerik:RadComboBox>
                                    </td>
                        </tr>
                        <tr>
                            <td class="auto-style13">
                                Select :</td>
                            <td class="auto-style4">
                                &nbsp;<asp:RadioButton ID="rbSalesView" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Sales Only" Checked="True" Font-Size="Small" />
                                        <asp:RadioButton ID="rbStockView" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Stock View" />
                            </td>
                            <td class="auto-style14">
                                <asp:CheckBox ID="cbExactTime" runat="server"
                                    OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                                </td>
                            <td class="auto-style15">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey" OnClick="btnSearch_Click"
                                    Width="142px" Height="60px"></asp:Button>
                            </td>
                            <td class="auto-style16">
                                &nbsp;</td>
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
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="2500px" Height="2000px" 
                            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        </rsweb:ReportViewer>
    
            </td>
        </tr>
    </table>
    <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
