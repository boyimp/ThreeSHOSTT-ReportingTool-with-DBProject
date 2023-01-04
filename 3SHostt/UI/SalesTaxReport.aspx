<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesTaxReport.aspx.cs" Inherits="UI_SalesTaxReport" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tickets Account Wise</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT" crossorigin="anonymous">
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
            width: 309px;
        }
        .style4
        {
            height: 68px;
            font-weight: 700;
            width: 496px;
        }
        .style5
        {
            width: 496px;
        }
        .style6
        {
            width: 309px;
        }
        .auto-style1 {
            height: 68px;
            text-align: right;
            width: 147px;
        }
        .auto-style2 {
            width: 147px;
            text-align: left;
        }
        .auto-style3 {
            height: 68px;
            font-weight: 700;
            width: 372px;
        }
        .auto-style5 {
            height: 20px;
            text-align: right;
            width: 147px;
        }
        .auto-style6 {
            height: 20px;
            font-weight: 700;
            width: 372px;
        }
        .auto-style7 {
            height: 20px;
            font-weight: 700;
        }
        .auto-style8 {
            width: 147px;
            height: 64px;
            text-align: left;
        }
        .auto-style9 {
            width: 372px;
            height: 64px;
        }
        .auto-style10 {
            height: 64px;
        }
    .RadPicker{vertical-align:middle}.RadPicker .rcTable{table-layout:auto}.RadPicker .RadInput{vertical-align:baseline}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}
        .auto-style11 {
            width: 372px;
        }
        .filterContainer {
        margin: 20px;
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
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
	<uc:header id="header1" runat="server"></uc:header>

   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>

        <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>
						<div class="bordered" style="width:100%">
							<table width="100%" cellpadding="5" cellspacing="0" border="0" class="filterContainer">
								<tr>
									<td class="auto-style5"></td>
									<td class="auto-style6"></td>
									<td class="auto-style7"></td>
									<td class="auto-style7"></td>
                                    <td class="auto-style7"></td>
                                    <td class="auto-style7">
									</td>
                                    <td class="auto-style7">
                                    </td>
                                    <td class="auto-style7"></td>
								</tr>
								<tr>
									<td class="auto-style1"><strong>From Date:</strong></td>
									<td class="auto-style3">
                                <telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Height="21px" Width="172px">
                                </telerik:RadDateTimePicker>
                                    </td>
									<td class="style2">To Date:&nbsp; 
                                <telerik:RadDateTimePicker ID="dtpToDate" runat="server" Height="21px" Width="184px">
                                </telerik:RadDateTimePicker>
                                    </td>
									<td class="style2"></td>
                                    <td class="style2"></td>
                                    <td class="style2">
									</td>
                                    <td class="style2">
                                    </td>
                                    <td class="style2"></td>
								</tr>
								<tr>
									<td class="auto-style8">
                                        <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="ChckedChanged" Text="Current Work Period"/>
                                    </td>
									<td class="auto-style9">
                                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;Outlets :
                                <rad:RadComboBox ID="ddlOutlets" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="250px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlOutlets_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="250px">
                                    <HeaderTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0" width="250">
                                            <tr>
                                                <td height="30">
                                                    <b>Outlets</b>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0" width="250">
                                            <tr>
                                                <td>
                                                    <%# DataBinder.Eval(Container.DataItem, "Name") %>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </rad:RadComboBox>
                                    </td>
									<td class="auto-style10">Select : <asp:RadioButton ID="rbTicketsView" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Tickets View" />
                                        <asp:RadioButton ID="rbDaysView" runat="server" GroupName="View" style="font-weight: 700" 
                                            Text="Days View" />
                                    </td>
									<td class="auto-style10"></td>
                                    <td class="auto-style10"></td>
                                    <td class="auto-style10">
										</td>
                                    <td class="auto-style10">
                                        </td>
                                    <td class="auto-style10"></td>
								</tr>
								<tr>
									<td class="auto-style2">
                                <asp:CheckBox ID="cbExactTime" runat="server"
                                    OnCheckedChanged="ChckedChanged" Text="Exact Time" />
                                    </td>
									<td class="auto-style11">
                                        Departments:&nbsp; <telerik:RadComboBox RenderMode="Lightweight" ID="ddlTicketType" runat="server" DataTextField="Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Width="250" Skin="MetroTouch" >
                                
                                 </telerik:RadComboBox>
                                    </td>
									<td><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey btn searchBtn" 
                                            OnClick="btnSearch_Click" Width="272px" Height="62px"></asp:Button></td>
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

       <%-- <%# DataBinder.Eval(Container.DataItem, "Name") %>--%>
    
    <uc:footer id="footer1" runat="server"></uc:footer>
</form>

</body>
</html>


