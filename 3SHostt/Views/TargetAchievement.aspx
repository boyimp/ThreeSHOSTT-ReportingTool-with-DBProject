<%@ Page Language="C#"  Title="Target Vs Achievement" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="TargetAchievement.aspx.cs" Inherits="UI_TargetAchievement" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
    </telerik:RadDockLayout>

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
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg" OnClick="btnSearch_Click"
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
</asp:Content>
