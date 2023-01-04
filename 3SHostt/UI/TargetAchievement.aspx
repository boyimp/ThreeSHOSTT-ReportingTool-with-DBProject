<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TargetAchievement.aspx.cs" Inherits="UI_TargetAchievement" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Target Vs Achievement</title>
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
        .auto-style3 {
            width: 150px;
            text-align: right;
        }
        .auto-style4 {
            width: 316px;
            text-align: left;
            font-size: larger;
            height: 79px;
        }
        .auto-style5 {
            width: 316px;
        }
        .auto-style6 {
            width: 82px;
        }
        .auto-style7 {
            width: 200px;
        }
        .auto-style8 {
            width: 150px;
            text-align: right;
            height: 68px;
        }
        .auto-style9 {
            width: 316px;
            text-align: left;
            font-size: larger;
            height: 68px;
        }
        .auto-style10 {
            width: 82px;
            height: 68px;
        }
        .auto-style11 {
            width: 200px;
            height: 68px;
        }
        .auto-style12 {
            height: 68px;
        }
        .auto-style13 {
            width: 150px;
            text-align: right;
            height: 79px;
        }
        .auto-style14 {
            width: 82px;
            height: 79px;
        }
        .auto-style15 {
            width: 200px;
            height: 79px;
        }
        .auto-style16 {
            height: 79px;
        }
        .auto-style17 {
            width: 111%;
        }
        .auto-style18 {
            width: 111%;
            height: 164px;
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
            <td class="auto-style18">
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0" 
                        style="height: 119px">
                        <tr>
                            <td class="auto-style3">
                                &nbsp;</td>
                            <td class="auto-style5">
                                &nbsp;</td>
                            <td align="right" class="auto-style6">
                                &nbsp;</td>
                            <td class="auto-style7">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style3">
                                &nbsp;</td>
                            <td class="auto-style5">
                                &nbsp;</td>
                            <td align="right" class="auto-style6">
                                &nbsp;</td>
                            <td class="auto-style7">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style3">
                                From Date:
                            </td>
                            <td class="auto-style5">
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td align="right" class="auto-style6">
                                To Date:
                            </td>
                            <td class="auto-style7">
                                <telerik:RadDatePicker ID="dtpToDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style13">
                                &nbsp;</td>
                            <td class="auto-style4">
                                &nbsp;</td>
                            <td align="right" class="auto-style14">
                                </td>
                            <td class="auto-style15">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey" OnClick="btnSearch_Click"
                                    Width="142px" Height="72px"></asp:Button>
                            </td>
                            <td class="auto-style16">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="auto-style17">
                &nbsp;</td>
        </tr>
        <asp:Label ID="lblWorkPeriod" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label>
        <tr>
            <td style="padding-left: 1px; " class="auto-style17">
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="5000px" Height="1346px" 
                            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        </rsweb:ReportViewer>
    
            </td>
        </tr>
    </table>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
