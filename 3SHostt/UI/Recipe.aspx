<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Recipe.aspx.cs"
    Inherits="UI_Recipe" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Recipe</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style2
        {
            height: 119px;
        }
        .style3
        {
            width: 248px;
        }
        .style4
        {
            width: 244px;
        }
        .style5
        {
            width: 59px;
        }
        .auto-style4 {
            width: 92px;
            text-align: right;
        }
        .auto-style14 {
            width: 90px;
            text-align: right;
        }
        .auto-style16 {
            width: 251px;
        }
        .auto-style20 {
            width: 249px;
        }
        .auto-style22 {
            width: 59px;
            text-align: right;
        }
        .auto-style23 {
            width: 59px;
            text-align: right;
            height: 43px;
        }
        .auto-style24 {
            width: 249px;
            height: 43px;
        }
        .auto-style25 {
            width: 92px;
            text-align: right;
            height: 43px;
        }
        .auto-style26 {
            width: 251px;
            height: 43px;
        }
        .auto-style27 {
            width: 90px;
            text-align: right;
            height: 43px;
        }
        .auto-style28 {
            width: 248px;
            height: 43px;
        }
        .auto-style29 {
            height: 43px;
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
            <td class="style2">
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0" 
                        style="height: 100px; margin-bottom: 5px">
                        <tr>
                            <td class="auto-style22">
                            </td>
                            <td class="auto-style20">
                                &nbsp;</td>
                            <td class="auto-style4">
                            </td>
                            <td class="auto-style16">
                            </td>
                            <td class="auto-style14">
                            </td>
                            <td class="style3">
                            </td>
                            <td>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td class="auto-style23">
                                Group :</td>
                            <td class="auto-style24">
                                <rad:RadComboBox Skin="WebBlue" ID="ddlGroupItem" runat="server" Height="170px" Width="200px"
                                    DataValueField="GroupCode" MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false"
                                    OnItemDataBound="ddlGroupItem_ItemDataBound" HighlightTemplatedItems="true" DropDownWidth="200px"
                                    OnSelectedIndexChanged="ddlGroupItem_SelectedIndexChanged" AutoPostBack="true"
                                    ItemRequestTimeout="500" TabIndex="5">
                                    <HeaderTemplate>
                                        <table width="50" cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td height="30">
                                                    <b>Group Item</b>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table width="50" cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <%# DataBinder.Eval(Container.DataItem, "GroupCode")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </rad:RadComboBox>
                            </td>
                            <td class="auto-style25">
                                Menu Item:</td>
                            <td class="auto-style26">
                                <rad:RadComboBox ID="ddlMenuItem" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlMenuItem_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="200px">
                                    <HeaderTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td height="30">
                                                    <b>Item</b>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </rad:RadComboBox>
                            </td>
                            <td class="auto-style27">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey" OnClick="btnSearch_Click"
                                    Width="173px" Height="69px"></asp:Button>
                            </td>
                            <td class="auto-style28">
                            </td>
                            <td class="auto-style29">
                            </td>
                            <td class="auto-style29">
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
        <tr>
            <td style="padding-left: 5px;">
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1827px" Height="1062px" 
                            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        </rsweb:ReportViewer>
    
            </td>
        </tr>
    </table>
 <%--   <%# DataBinder.Eval(Container.DataItem, "Name") %>--%>
    <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
