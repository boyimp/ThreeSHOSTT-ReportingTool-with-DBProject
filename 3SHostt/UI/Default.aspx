<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="UI_Default" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>3SHostt Reporting Tool</title>
	<link rel="manifest" href="../manifest.json">
    <link rel="stylesheet" type="text/css" href="../styles.css" />
	<link href="<%=ConfigurationManager.AppSettings["ReactBundlePath"] %>/static/css/main.css" rel="stylesheet" />
    <style type="text/css">
        .style1
        {
            font-size: medium;
        }
    </style>
    <!-- Chrome, Firefox OS and Opera -->
    <meta name="theme-color" content="#1a202c">
    <!-- Windows Phone -->
    <meta name="msapplication-navbutton-color" content="#1a202c">
    <!-- iOS Safari -->
    <meta name="apple-mobile-web-app-status-bar-style" content="#1a202c">
	<meta name="viewport" content="width=device-width">
	<script type="text/javascript">
		if ("serviceWorker" in navigator) {
			window.addEventListener("load", function() {
				navigator.serviceWorker
				.register("./serviceWorker.js")
				.then(res => console.log("service worker registered"))
				.catch(err => console.log("service worker not registered", err));
			});
		}
	</script>
</head>
<%--<script type="text/javascript">
	function managePopup(tabName) { 
		if (tabName != null && tabName != undefined) {
			switch (tabName) {                
				case "ForgotPassword":
					dock = $find("<%= RadDocForgotPassword.ClientID %>");
					dock.set_closed(!dock.get_closed());
					break;
			}
		}
	}
</script>--%>
<body>
	<div id="root"></div>
    <script src="<%=ConfigurationManager.AppSettings["ReactBundlePath"] %>/static/js/main.js" type="text/javascript"></script>
<%--<form id="form1" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	<telerik:RadDockLayout runat="server" ID="RadDockLayout1"></telerik:RadDockLayout>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>  
			<table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>
						<img src="../images/3sHost.png" border="0" />
					</td>
				</tr>
				<tr>
					<td>
						<br><br><br><br><br><br><br><br><br><br><br>
						
						<table width="700" border="0" cellspacing="1" cellpadding="5" align="center" 
                            bgcolor="#d6d6d6" style="width: 454px">
						    <tr>
						       <td>
						         <b>User Log In</b>
						       </td>						       
						    </tr>
							<tr>
								<td bgcolor="#ffffff">
									<table border="0" cellspacing="0" cellpadding="5">									   
										<tr>
											<td><span class="style1">Username</span><br /></td>
											<td>
												<asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
												<asp:RequiredFieldValidator ID="reqUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="required" ForeColor="Red"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td class="style1">Password:</td>
											<td>
												<asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
												<asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="required" ForeColor="Red"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td><h1><asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="buttonGrey" 
                                                    onclick="btnLogin_Click" Height="74px" Width="198px" /></h1></td>
										</tr>
									</table>
									<asp:LinkButton ID="lnkForgotPassword" runat="server" Text="Forgot Password?" CausesValidation="false"
										onclick="lnkForgotPassword_Click"></asp:LinkButton>
										
				                </td>
							</tr>
						</table>
						<p align="center"><asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label></p>
						
						<br /><br /><br /><br /><br /><br />
					</td>
				</tr>
			</table>
        </ContentTemplate>
    </asp:UpdatePanel>
	<uc:footer id="footer1" runat="server"></uc:footer>
</form>--%>
</body>
</html>
