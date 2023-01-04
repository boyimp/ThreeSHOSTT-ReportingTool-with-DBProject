<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryVsRecipes.aspx.cs" Inherits="UI_InventoryVsRecipes" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inventory Count Variance</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
</head>

<body>
<form id="form1" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	<uc:header id="header1" runat="server"></uc:header>

    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>

          <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>
						<div class="bordered" style="width:100%">
							<table width="100%" cellpadding="3" cellspacing="0" border="0">	
                            	<tr>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td align="right">&nbsp;</td>
									<td>&nbsp;</td>
									<td> &nbsp;</td>
								</tr>
								<tr>
                                    <td align="right">Inventory Items:</td>
								<td>
									<telerik:RadComboBox RenderMode="Lightweight" ID="ddlInventoryItem" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
										Width="250" Skin="MetroTouch" OnItemDataBound="ddlInventoryItem_ItemDataBound1">
									</telerik:RadComboBox>
								</td>
									
								</tr>
								<tr>
									<td align="right">&nbsp;</td>
									<td>
										<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey" 
                                            OnClick="btnSearch_Click" Height="60px" Width="257px"></asp:Button>
									</td>
									<td >&nbsp;</td>
									<td >&nbsp;</td>
									<td></td>
									
								</tr>
							</table>
						</div>
					</td>
				</tr> 
				<tr>
                    <td>
                        <asp:Label ID="lblWorkPeriod" runat="server" 
                            style="font-size: large; font-weight: 700" Visible="False"></asp:Label>
                     </td>
                </tr>
				<tr>
					<td style="padding-left: 1px;width:100%;">
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1795px" Height="662px">
        </rsweb:ReportViewer>
					</td>
				</tr>			
			</table>

        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    
    <uc:footer id="footer1" runat="server"></uc:footer>
</form>

</body>
</html>

