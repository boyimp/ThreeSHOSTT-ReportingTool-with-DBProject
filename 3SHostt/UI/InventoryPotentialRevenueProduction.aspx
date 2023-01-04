<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryPotentialRevenueProduction.aspx.cs" Inherits="UI_InventoryPotentialRevenueProduction" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Prep Item Report</title>
        <!-- CSS only -->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT" crossorigin="anonymous">
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .style2
        {
            width: 177px;
            text-align: right;
        }
        .style3
        {
            width: 177px;
            text-align: right;
            height: 42px;
        }
        .style4
        {
            height: 42px;
        }
        .style5
        {
            height: 42px;
            width: 237px;
        }
        .style6
        {
            width: 237px;
        }
        .style7
        {
            height: 42px;
            width: 116px;
        }
        .style8
        {
            width: 116px;
        }
        .style9
        {
            height: 42px;
            width: 275px;
        }
        .style10
        {
            width: 275px;
        }
        .auto-style1 {
            width: 177px;
            text-align: right;
            height: 55px;
        }
        .auto-style2 {
            width: 237px;
            height: 55px;
        }
        .auto-style4 {
            width: 275px;
            height: 55px;
        }
        .auto-style5 {
            height: 55px;
        }
        .auto-style13 {
            height: 55px;
            width: 116px;
        }
        .auto-style16 {
            width: 177px;
            text-align: right;
            height: 44px;
        }
        .auto-style17 {
            height: 44px;
            width: 237px;
        }
        .auto-style18 {
            height: 44px;
            width: 116px;
        }
        .auto-style19 {
            height: 44px;
            width: 275px;
        }
        .auto-style20 {
            height: 44px;
        }
        .auto-style21 {
            width: 116px;
            height: 55px;
            text-align: right;
        }
        .auto-style22 {
            width: 177px;
            text-align: right;
            height: 32px;
        }
        .auto-style23 {
            height: 32px;
            width: 237px;
        }
        .auto-style24 {
            height: 32px;
            width: 116px;
        }
        .auto-style25 {
            height: 32px;
            width: 275px;
        }
        .auto-style26 {
            height: 32px;
        }
        .filterContainer {
            margin: 0px 20px;
}
.searchBtn {
  height: 45px !important;
  width: 120px !important;
  background-color: #fa7242;
  color: white;
  font-weight: bold;
}
.searchBtn:hover {
  background-color: #f86530 !important;
  color: white !important;
}

#ImageButton1,
#ImageButton2,
#ImageButton3 {
  width: 35px !important;
}
    </style>
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
							<table width="100%" cellpadding="3" cellspacing="0" border="0" class="filterContainer">	
                            	<tr>
									<td class="auto-style22"></td>
									<td class="auto-style23"></td>
									<td align="right" class="auto-style24"></td>
									<td class="auto-style25"></td>
                                    <td class="auto-style26">
                                        </td>
									<td class="auto-style26"> </td>
								</tr>
                            	<tr>
									<td class="auto-style1">From Date:</td>
									<td class="auto-style2"><telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker></td>
									<td align="right" class="auto-style13">To Date:</td>
									<td class="auto-style4"><telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker></td>
                                    <td class="auto-style5">
                                        <rad:RadComboBox ID="ddlBrand" Runat="server" AllowCustomText="true" 
                                            DataValueField="Brand" DropDownWidth="200px" EnableLoadOnDemand="false" 
                                            Height="170px" HighlightTemplatedItems="true" ItemRequestTimeout="500" 
                                            MarkFirstMatch="true" OnItemDataBound="ddlBrand_ItemDataBound"                                             
                                            Skin="WebBlue" TabIndex="5" Width="200px" Visible="False">
                                            <HeaderTemplate>
                                                <table border="0" cellpadding="3" cellspacing="0" >
                                                    <tr>
                                                        <td height="30">
                                                            <b>Brand</b></td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table border="0" cellpadding="3" cellspacing="0" >
                                                    <tr>
                                                        <td>
                                                            <%# DataBinder.Eval(Container.DataItem, "Brand")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </rad:RadComboBox>
                                    </td>
									<td class="auto-style5"> </td>
								</tr>
								<tr>
									<td align="right" class="auto-style16">Inventory Group Item:</td>
									<td class="auto-style17">
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ddlGroupItem" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Width="250" Skin="MetroTouch" >
                                
                                 </telerik:RadComboBox>
                                    </td>
									<td align="right" class="auto-style18">Inventory Item:</td>
									<td class="auto-style19">
                                        <rad:RadComboBox ID="ddlInventoryItem" Runat="server" AllowCustomText="true" 
                                            DataValueField="id" DropDownWidth="200px" EnableLoadOnDemand="false" 
                                            Height="170px" HighlightTemplatedItems="true" ItemRequestTimeout="500" 
                                            MarkFirstMatch="true" OnItemDataBound="ddlInventoryItem_ItemDataBound"                                             
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
									<td class="auto-style20">
										<rad:RadComboBox Skin="WebBlue" id="ddlVendor" Runat="server" 
                                            Height="170px" Width="200px" DataValueField="Vendor"
											MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false" 
											HighlightTemplatedItems="true" DropDownWidth="200px" ItemRequestTimeout="500" TabIndex="5" 
                                            OnItemDataBound="ddlVendor_ItemDataBound" Visible="False">
											<HeaderTemplate>
												<table width="200" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td height="30"><b>Vendor</b></td>
													</tr>
												</table>                                
											</HeaderTemplate>
											<ItemTemplate>
												<table width="200" cellpadding="3" cellspacing="0" border="0">
													<tr>
                                                        <td>
                                                            <%# DataBinder.Eval(Container.DataItem, "Vendor")%>
                                                        </td>
                                                    </tr>
												</table>
											</ItemTemplate>
										</rad:RadComboBox>
									</td>
								</tr>
								<tr>
									<td align="right" class="auto-style1">Warehouse:</td>
									<td class="auto-style2">
										<rad:RadComboBox Skin="WebBlue" id="ddlWarehouse" Runat="server" 
                                            Height="170px" Width="200px" DataValueField="id"
											MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false" 
											HighlightTemplatedItems="true" DropDownWidth="200px" ItemRequestTimeout="500" TabIndex="5" 
                                            OnItemDataBound="ddlWarehouse_ItemDataBound">
											<HeaderTemplate>
												<table width="200" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td height="30"><b>Group Item</b></td>
													</tr>
												</table>                                
											</HeaderTemplate>
											<ItemTemplate>
												<table width="200" cellpadding="3" cellspacing="0" border="0">
													<tr>
                                                        <td>
                                                            <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                                        </td>
                                                    </tr>
												</table>
											</ItemTemplate>
										</rad:RadComboBox>
									</td>
									<td class="auto-style21" ></td>
									<td class="auto-style4" >
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey btn searchBtn" 
                                            OnClick="btnSearch_Click" Height="68px" Width="231px"></asp:Button>
                                    </td>
									<td class="auto-style5">
                                        <asp:CheckBox ID="CheckFifoPrice" runat="server" AutoPostBack="True"
                                            OnCheckedChanged="ChckedChanged" Text="Avg FIFO Price" Visible="False" />
                                    </td>
									
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
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="2097px" Height="1110px">
        </rsweb:ReportViewer>
					</td>
				</tr>			
			</table>

        <%--<%# DataBinder.Eval(Container.DataItem, "GroupCode")%>--%>
    
    <uc:footer id="footer1" runat="server"></uc:footer>
</form>

</body>
</html>

