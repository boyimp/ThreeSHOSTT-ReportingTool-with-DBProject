<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountWise.aspx.cs" Inherits="UI_AccountWise" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Account Wise Report</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .auto-style1
        {
            width: 399px;
            height: 39px;
        }
        .auto-style2
        {
            width: 320px;
            height: 39px;
        }
        .auto-style3
        {
            width: 90px;
            height: 39px;
        }
        .auto-style4
        {
            width: 53px;
            height: 39px;
        }
        .auto-style10
        {
            width: 90px;
            height: 44px;
        }
        .auto-style11
        {
            width: 399px;
            height: 44px;
        }
        .auto-style12
        {
            width: 53px;
            height: 44px;
        }
        .auto-style13
        {
            width: 320px;
            height: 44px;
        }
        .auto-style14
        {
            height: 44px;
        }
        .auto-style15
        {
            height: 39px;
        }
        .auto-style16
        {
            width: 90px;
            height: 32px;
        }
        .auto-style17
        {
            width: 399px;
            height: 32px;
        }
        .auto-style18
        {
            width: 53px;
            height: 32px;
        }
        .auto-style19
        {
            width: 320px;
            height: 32px;
        }
        .auto-style20
        {
            height: 32px;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	<uc:header id="header1" runat="server"></uc:header>

    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>

          <table cellpadding="0" cellspacing="0" border="0" style="width: 98%">
				<tr>
					<td>
						<div class="bordered" style="width:100%">
							<table width="100%" cellpadding="3" cellspacing="0" border="0" style="height: 108px">	
                            	<tr>
									<td align="right" class="auto-style10">From Date:</td>
									<td class="auto-style11"><telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker></td>
									<td align="right" class="auto-style12">To Date:</td>
									<td class="auto-style13"><telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker></td>
									<td class="auto-style14"> </td>
								</tr>
								<tr>
									<td align="right" class="auto-style16">Account Types:</td>
									<td class="auto-style17"><rad:RadComboBox Skin="WebBlue" id="ddlAccountType" Runat="server" Height="170px" Width="200px" DataValueField="Name"
											MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false" OnItemDataBound="ddlAccountType_ItemDataBound" 
											HighlightTemplatedItems="true" DropDownWidth="200px" onselectedindexchanged="ddlAccountType_SelectedIndexChanged" AutoPostBack="true" ItemRequestTimeout="500" TabIndex="5">
											<HeaderTemplate>
												<table width="50" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td height="30"><b>Group Item</b></td>
													</tr>
												</table>                                
											</HeaderTemplate>
											<ItemTemplate>
												<table width="50" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td><%# DataBinder.Eval(Container.DataItem, "Name")%></td>
													</tr>
												</table>
											</ItemTemplate>
										</rad:RadComboBox></td>
									<td align="right" class="auto-style18">Accounts:</td>
									<td class="auto-style19">
                                        <rad:RadComboBox ID="ddlAccounts" Runat="server" AllowCustomText="true" 
                                            DataValueField="id" DropDownWidth="200px" EnableLoadOnDemand="false" 
                                            Height="170px" HighlightTemplatedItems="true" ItemRequestTimeout="500" 
                                            MarkFirstMatch="true" OnItemDataBound="ddlAccounts_ItemDataBound"                                             
                                            Skin="WebBlue" TabIndex="5" Width="200px">
                                            <HeaderTemplate>
                                                <table border="0" cellpadding="3" cellspacing="0" >
                                                    <tr>
                                                        <td height="30">
                                                            <b>Item</b></td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table border="0" cellpadding="3" cellspacing="0" >
                                                    <tr>
                                                        <td>
                                                            <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </rad:RadComboBox>
                                    </td>
									<td class="auto-style20"></td>
								</tr>
								<tr>
									<td align="right" class="auto-style3"></td>
									<td align="left" class="auto-style1">
										<asp:CheckBox ID="chkOpeningBalance" runat="server" Checked="false" 
                                            Text="Opening Balance"/>
                                    </td>
									<td class="auto-style4" ></td>
									<td class="auto-style2" ><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey" 
                                            OnClick="btnSearch_Click" Height="54px" Width="218px" BackColor="#49C4E9"></asp:Button>
                                    </td>
									<td class="auto-style15"></td>
									
								</tr>
							</table>
						</div>
					</td>
				</tr> 
				<tr>
					<td style="padding-left: 1px;width:100%;">
                       
         <rsweb:ReportViewer ID="rptViewer2" runat="server" Width="1504px" Height="895px">
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="AccountTypeWiseBalanceDataSetTableAdapters.AccountTypeWiseBalanceTableAdapter" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
        
                       
					</td>
				</tr>			
			</table>

        <%--<%# DataBinder.Eval(Container.DataItem, "Name")%>--%>
    
    <uc:footer id="footer1" runat="server"></uc:footer>
</form>

</body>
</html>

