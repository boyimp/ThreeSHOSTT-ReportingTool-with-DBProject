<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Includes_Header" %>
<%@ Register Assembly="RadMenu.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<style type="text/css">
    .style1 {
        font-size: x-large;
        color: white;
    }
    .full-wrapper{
        background: rgb(0, 56, 101);
    }
    .brandArea{
        margin:20px;
        cursor:pointer;
    }
    .brandArea .brand {
        display: flex;
        align-items: center;
    }
    .brandArea p {
        font-size: 16px;
        color:white;
    }
    .brandArea .brand .logoDevider {
        height: 20px;
        width: 1px;
        background: gray;
        margin: 0 10px;
    }
</style>
<script src="../UI/js/main.js"></script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="full-wrapper">
    <tr class="abc">
        <td width="840px">
            <!-- <img src="../images/3sHost.png" border="0" /> -->
            <div class="brandArea" onclick="goToDashboard()">
                <div class="brand">
                    <img width="80" src="../images/3SLogo1.png" border="0" />
                    <span class="logoDevider"></span>
                    <img width="90" src="../images/3S_Hostt.png" border="0" />
                </div>

                <p>Taking Technology Forward</p>
            </div>
        </td>

        <td valign="middle">
            <div>
                <span class="style1">Reporting Tool-{{VERSION_NAME}}</span>
            </div>
    
            <table cellpadding="0" cellspacing="0" border="0" align="left">
                <%--<tr>
					<td width="100"><font color="white"><b>PF Company: </b></font></td>
					<td width="170">&nbsp;</td>
				</tr>--%>
                <tr >
                    <td>
                        <font color="white">User:   <asp:Label ID="lblLoggedInPFCompanyUser" runat="server" ForeColor="white"></asp:Label> </font>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <rad:RadMenu ID="radMenu" runat="server" CausesValidation="False" Skin="Fusion">
                <Items>
                    <rad:RadMenuItem Text="HOME" NavigateUrl="../#/dashboard" Width="50%" style="cursor:pointer;"></rad:RadMenuItem>
                </Items>
            </rad:RadMenu>
        </td>
    </tr>
</table>
