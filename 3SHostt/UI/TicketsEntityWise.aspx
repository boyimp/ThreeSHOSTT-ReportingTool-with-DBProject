<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketsEntityWise.aspx.cs" Inherits="UI_TicketsEntityWise" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tickets Entity Wise</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style2 {
            height: 68px;
            font-weight: 700;
        }

        .style3 {
            height: 68px;
            text-align: right;
        }

        .auto-style1 {
            height: 92px;
            text-align: right;
            width: 101px;
        }

        .auto-style2 {
            width: 101px;
        }

        .auto-style3 {
            height: 92px;
            font-weight: 700;
            width: 219px;
        }

        .auto-style4 {
            width: 219px;
        }

        .auto-style5 {
            height: 92px;
            font-weight: 700;
            width: 341px;
        }

        .auto-style6 {
            width: 341px;
        }

        .auto-style7 {
            height: 92px;
            font-weight: 700;
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
                                <td class="auto-style1"><strong>From Date:</strong></td>
                                <td class="auto-style3">
                                    <telerik:RadDateTimePicker ID="dtpFromDate" runat="server"></telerik:RadDateTimePicker>
                                </td>
                                <td class="auto-style5">To Date :<telerik:RadDateTimePicker ID="dtpToDate" runat="server"></telerik:RadDateTimePicker>
                                </td>
                                <td class="auto-style4">
                                    <asp:CheckBox ID="cbExactTime" runat="server"
                                        OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                                </td>
                                <
                                <td class="auto-style4">
                                    <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
                                </td>
                                <td class="auto-style7"></td>
                                <td class="auto-style7"></td>
                                <td class="auto-style7"></td>
                            </tr>
                            <tr>
                                <td class="auto-style1"><strong>Outlet(s):</strong></td>
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
                                <td style="display:none;" align="right"><strong>Department(s):</strong></td>
                                <td style="display:none;" class="auto-style4">                                    
                                    <telerik:RadComboBox RenderMode="Lightweight" ID="ddlTicketType" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                        Width="250" Skin="MetroTouch">
                                    </telerik:RadComboBox>
                                </td>                                
                                <td class="auto-style6">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey"
                                        OnClick="btnSearch_Click" Width="173px" Height="55px"></asp:Button></td>
                                <td height="30">&nbsp;</td>
                                <td class="auto-style2">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 5px;">&nbsp;</td>
            </tr>
            <tr>
                <td style="padding-left: 5px;">

                    <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="2500px" Height="1827px"
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


