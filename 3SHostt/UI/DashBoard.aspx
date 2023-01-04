<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="UI_DashBoard" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Dashboard</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .auto-style1
        {
            width: 104%;
        }
        .auto-style12 {
            width: 99%;
        }
        .auto-style13 {
            height: 28px;
        }
        .auto-style14 {
            text-align: right;
            font-size: small;
            height: 73px;
            width: 59px;
        }
        .auto-style15 {
            height: 73px;
        }
        .auto-style16 {
            width: 853px;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	<uc:header id="header1" runat="server"></uc:header>

    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
          <table cellpadding="0" cellspacing="0" border="0" class="auto-style12">
              <tr>
                  <table width="100%" cellpadding="5" cellspacing="0" border="0" class="auto-style13">
                      <tr>
                          <td class="auto-style14">
                              <strong>Outlet: </strong>
                          </td>
                          <td class="auto-style16">
                              <rad:RadComboBox ID="ddlOutlets" runat="server" AllowCustomText="true" DataValueField="id"
                                  DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                  ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlOutlets_ItemDataBound" ShowChooser="true"
                                  Skin="WebBlue" TabIndex="5" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlOutlets_SelectedIndexChanged">
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
                          <td class="auto-style15"></td>
                          <td class="auto-style15">&nbsp;&nbsp;&nbsp;
                          </td>
                      </tr>
                  </table>

                  <td style="padding-left: 1px;" class="auto-style1">&nbsp;</td>
              </tr>			
			</table>
          <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td style="padding-left: 1px;" class="auto-style1">
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="100%" Height="4068px" ProcessingMode="Local" SizeToReportContent="true"
                            style="margin-right: 0px">
        </rsweb:ReportViewer>
                      
					</td>
				</tr>			
			</table>
    
    <uc:footer id="footer1" runat="server" />
</form>

</body>
</html>

