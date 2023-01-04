<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PushNotification.aspx.cs" Inherits="UI_PushNotification" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Notification Preference</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style2 {
            width: 181px;
        }

        .style3 {
            width: 265px;
        }

        .style4 {
            width: 96px;
        }

        .auto-style18 {
            width: 150px;
            text-align: right;
            height: 46px;
        }

        .auto-style19 {
            width: 318px;
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
            height: 83px;
        }
        .auto-style24 {
            width: 318px;
            height: 83px;
        }
        .auto-style25 {
            width: 168px;
            height: 83px;
        }
        .auto-style26 {
            width: 200px;
            height: 83px;
        }
        .auto-style27 {
            height: 83px;
        }
        .auto-style28 {
            width: 150px;
            text-align: right;
            height: 41px;
        }
        .auto-style29 {
            width: 318px;
            height: 41px;
        }
        .auto-style30 {
            width: 168px;
            height: 41px;
        }
        .auto-style31 {
            width: 200px;
            height: 41px;
        }
        .auto-style32 {
            height: 41px;
        }
        .auto-style33 {
            height: 119px;
        }
    </style>
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons">
    <link rel="stylesheet" href="https://code.getmdl.io/1.2.1/material.indigo-pink.min.css">
    <script defer src="https://code.getmdl.io/1.2.1/material.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc:header ID="header1" runat="server"></uc:header>
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <div class="bordered" style="width: 100%">
                        <table width="100%" cellpadding="5" cellspacing="0" border="0" class="auto-style33">
                            <tr>
                                <td class="auto-style28"></td>
                                <td class="auto-style29">

                                </td>
                                <td align="right" class="auto-style30"></td>
                                <td class="auto-style31"></td>
                                <td class="auto-style32"></td>
                            </tr>
                            <tr>
                                <td class="auto-style23"></td>
                                <td class="auto-style24">
                                    <button disabled class="js-push-btn mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect">
                                        Enable Push Messaging
                                    </button>

                                </td>
                                <td align="right" class="auto-style25"></td>
                                <td class="auto-style26"></td>
                                <td class="auto-style27"></td>
                            </tr>
                            <tr>
                                <td class="auto-style18">&nbsp;</td>
                                <td class="auto-style19">
                                    <p class="mdl-typography--text-center">

                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonGrey" OnClick="btnSave_Click"
                                            Width="128px" Height="56px"></asp:Button>
                                    </p>
                                    <p>
                                        <section class="subscription-details js-subscription-details is-invisible">
                                          <asp:Label class="js-subscription-json" ID="lblEndpoint" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label>
                                              <asp:HiddenField ID="lblHidden" Value="0" runat="server" />
                                              <asp:HiddenField ID="lblIsSubscribed" Value="0" runat="server" />
                                            </section>
                                    </p>
                                    <p>
                                        <asp:Label ID="lblMessage" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label>
                                    </p>

                                </td>
                                <td align="right" class="auto-style20">&nbsp;</td>
                                <td class="auto-style21">&nbsp;</td>
                                <td class="auto-style22"></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <asp:Label ID="lblWorkPeriod" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label>
            <tr>
                <td style="padding-left: 1px; width: 100%;">&nbsp;</td>
            </tr>
        </table>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
          <script src="js/main.js"></script>
           <script src="https://code.getmdl.io/1.2.1/material.min.js"></script>
        <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
