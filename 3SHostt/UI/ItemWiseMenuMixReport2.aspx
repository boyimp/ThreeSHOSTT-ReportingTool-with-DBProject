<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemWiseMenuMixReport2.aspx.cs" Inherits="UI_ItemWiseMenuMixReport2" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Item Wise Menu Mix Report</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style3
        {
            width: 78px;
            font-weight: 700;
            text-align: right;
        }
        .style4
        {
            width: 232px;
        }
        .style5
        {
            width: 233px;
        }
        .style6
        {
            width: 230px;
            font-weight: 700;
        }
        .style8
        {
            width: 230px;
            font-weight: 700;
        }
        .style10
        {
            width: 135px;
            font-weight: 700;
        }
        .style11
        {
            width: 78px;
            font-weight: bold;
            text-align: right;
        }
        .style12
        {
            width: 233px;
            font-weight: 700;
        }
        .style13
        {
            width: 135px;
            font-weight: 700;
            height: 29px;
        }
        .style14
        {
            width: 232px;
            height: 29px;
        }
        .style15
        {
            width: 78px;
            font-weight: 700;
            height: 29px;
            text-align: right;
        }
        .style16
        {
            width: 233px;
            height: 29px;
        }
        .style17
        {
            width: 230px;
            font-weight: 700;
            height: 29px;
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
							<table cellpadding="5" cellspacing="0" border="0" 
                                style="height: 89px; width: 87%;">
								<tr>
									<td class="style13" style="font-weight: 700">From Date:</td>
									<td class="style14"><telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker></td>
									<td class="style15">To Date:</td>
									<td class="style16"><telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker></td>
                                    <td class="style17" style="font-weight: 700; text-align: left">
                                        <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" 
                                            Checked="true" OnCheckedChanged="ChckedChanged" Text="Current Work Period" 
                                            style="font-weight: 700; text-align: left;"/>
                                    </td>
								</tr>
								<tr>
                            <td class="style10">
                                Group :</td>
                            <td class="style4">
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
                            <td class="style11">
                                Menu Item:</td>
                            <td class="style12">
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
                            <td height="30" class="style6" style="font-weight: 700">
                                &nbsp;</td>
                                </tr>
								<tr>
									<td class="style10" style="font-weight: 700">
                                        Department:</td>
									<td class="style4">
                                <rad:RadComboBox ID="ddlTicketType" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlTicketType_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="200px">
                                    <HeaderTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0" width="200">
                                            <tr>
                                                <td height="30">
                                                    <b>Department</b>
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
									<td class="style3">View:</td>
									<td class="style5">
                                        <asp:RadioButton ID="rbRecipe" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Recipe" />
                                        <asp:RadioButton ID="rbFixedCost" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Fixed Cost" />
                                    </td>
                                    <td height="30" class="style8" style="font-weight: 700">
                                        <asp:Button ID="btnSearch" 
                                            runat="server" Text="Search" CssClass="buttonGrey" 
                                            OnClick="btnSearch_Click" Width="173px" Height="62px"></asp:Button></td>
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
                        &nbsp;</td>
				</tr>
                <tr>
					<td style="padding-left: 5px;">
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1490px" Height="1336px" 
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


