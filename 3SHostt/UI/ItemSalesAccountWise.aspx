<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemSalesAccountWise.aspx.cs" Inherits="UI_ItemSalesAccountWise" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ItemSalesAccountWise</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style2
        {
            height: 68px;
            font-weight: 700;
        }
        .style3
        {
            height: 68px;
            text-align: right;
        }
        .auto-style1 {
            height: 68px;
            text-align: right;
            width: 158px;
        }
        .auto-style2 {
            width: 158px;
        }
        .auto-style3 {
            height: 21px;
            text-align: right;
            width: 158px;
        }
        .auto-style4 {
            height: 21px;
            font-weight: 700;
        }
        .auto-style5 {
            height: 21px;
            font-weight: 700;
            width: 234px;
        }
        .auto-style6 {
            height: 68px;
            font-weight: 700;
            width: 234px;
        }
        .auto-style7 {
            width: 234px;
        }
        .auto-style11 {
            width: 158px;
            text-align: right;
            height: 48px;
        }
        .auto-style12 {
            width: 234px;
            height: 48px;
        }
        .auto-style14 {
            height: 48px;
        }
        .auto-style15 {
            height: 21px;
            font-weight: 700;
            width: 350px;
        }
        .auto-style16 {
            height: 68px;
            font-weight: 700;
            width: 350px;
        }
        .auto-style17 {
            width: 350px;
            height: 48px;
        }
        .auto-style18 {
            width: 350px;
            text-align: right;
        }
        .auto-style19 {
            width: 234px;
            height: 48px;
            text-align: center;
        }
        .auto-style20 {
            font-weight: normal;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
	<uc:header id="header1" runat="server"></uc:header>

   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>

        <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>
						<div class="bordered" style="width:100%">
							<table width="100%" cellpadding="5" cellspacing="0" border="0">
								<tr>
									<td class="auto-style3"></td>
									<td class="auto-style5"></td>
									<td class="auto-style15"></td>
									<td class="auto-style4"></td>
                                    <td class="auto-style4"></td>
                                    <td class="auto-style4">
									</td>
                                    <td class="auto-style4">
                                    </td>
                                    <td class="auto-style4"></td>
								</tr>
								<tr>
									<td class="auto-style1">From Date:</td>
									<td class="auto-style6">
                                        <telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Height="23px" Width="172px">
                                        </telerik:RadDateTimePicker>
									</td>
									<td class="auto-style16"><span class="auto-style20">To Date:</span>
                                    <telerik:RadDateTimePicker ID="dtpToDate" runat="server" Width="182px"></telerik:RadDateTimePicker></td>
									<td class="style2"></td>
                                    <td class="style2"></td>
                                    <td class="style2">
									</td>
                                    <td class="style2">
                                    </td>
                                    <td class="style2"></td>
								</tr>
								<tr>
									<td class="auto-style11">Select :</td>
									<td class="auto-style12">
                                        &nbsp;
                                        <asp:RadioButton ID="rbGroupView" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Group View" Checked="True" />
                                        <asp:RadioButton ID="rbItemView" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Item View" />
                                    </td>
									<td class="auto-style17">Outlet:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
									<td class="auto-style14"></td>
                                    <td class="auto-style14"></td>
                                    <td class="auto-style14">
										</td>
                                    <td class="auto-style14">
                                        </td>
                                    <td class="auto-style14"></td>
								</tr>
								<tr>
									<td class="auto-style11">
                                        <strong>
                                        <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="ChckedChanged" Text="Current Work Period"/>
                                        </strong>
                                    </td>
									<td class="auto-style19">
                                        <strong>
                                <asp:CheckBox ID="cbExactTime" runat="server"
                                    OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                                        </strong>
                                    </td>
									<td class="auto-style17">Department :<telerik:RadComboBox RenderMode="Lightweight" ID="ddlTicketType" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Width="250" Skin="MetroTouch" >
                                
                                 </telerik:RadComboBox>
                                    </td>
									<td class="auto-style14">&nbsp;</td>
                                    <td class="auto-style14">&nbsp;</td>
                                    <td class="auto-style14">
										&nbsp;</td>
                                    <td class="auto-style14">
                                        &nbsp;</td>
                                    <td class="auto-style14">&nbsp;</td>
								</tr>
								<tr>
									<td class="auto-style2">
                                        &nbsp;</td>
									<td class="auto-style7">
                                        &nbsp;</td>
									<td class="auto-style18"><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey" 
                                            OnClick="btnSearch_Click" Width="173px" Height="66px"></asp:Button></td>
									<td>&nbsp;</td>
                                    <td height="30">&nbsp;</td>
                                    <td>
										&nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                    <td>&nbsp;</td>
								</tr>
							</table>
						</div>
					</td>
				</tr> 
                <tr>
					<td style="padding-left: 5px;">
                        &nbsp;</td>
				</tr>
                <tr>
					<td style="padding-left: 5px;">
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="3000px" Height="3000px" 
                            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
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


