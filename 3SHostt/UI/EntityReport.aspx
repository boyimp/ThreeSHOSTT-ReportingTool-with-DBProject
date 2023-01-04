<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EntityReport.aspx.cs" Inherits="UI_EntityReport" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Entity Report</title>
        <!-- CSS only -->
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
            width: 144px;
        }
        .style4
        {
            height: 68px;
            font-weight: 700;
            width: 319px;
        }
        .style5
        {
            width: 319px;
        }
        .style6
        {
            height: 68px;
            font-weight: 700;
            width: 257px;
        }
        .style7
        {
            width: 257px;
        }
        .style8
        {
            width: 144px;
            text-align: right;
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
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
	<uc:header id="header1" runat="server"></uc:header>

   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>

        <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>
						<div class="bordered" style="width:100%">
							<table cellpadding="5" cellspacing="0" border="0" style="width: 71%" class="filterContainer">
								<tr>
									<td class="style3"><strong>From Date:</strong></td>
									<td class="style6"><telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker></td>
									<td class="style4">To Date:&nbsp; <telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker></td>
									<td class="style2">
                                        <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="ChckedChanged" Text="Current Work Period"/>
                                    </td>
                                    <td class="style8">
                                        <strong>Screen Menu :</strong></td>
									<td class="style7">
                                        <rad:RadComboBox ID="ddlScreenMenu" runat="server" AllowCustomText="True" DataValueField="id"
                                        DropDownWidth="200px" Height="170px" HighlightTemplatedItems="True"
                                        ItemRequestTimeout="500" MarkFirstMatch="True" OnItemDataBound="ddlScreenMenu_ItemDataBound"
                                        Skin="WebBlue" TabIndex="5" Width="200px"
                                        OnSelectedIndexChanged="ddlScreenMenu_SelectedIndexChanged"
                                        AutoPostBack="True" SkinsPath="~/RadControls/ComboBox/Skins">
                                        <HeaderTemplate>
                                            <table border="0" cellpadding="3" cellspacing="0" width="200">
                                                <tr>
                                                    <td height="30">
                                                        <b>Screen Menu</b>
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
								</tr>
								<tr>
									<td class="style8">
                                        <strong>Entity Type :</strong></td>
									<td class="style7">
                                        &nbsp;<rad:RadComboBox ID="ddlEntityType" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlEntityType_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="200px" 
                                            onselectedindexchanged="ddlEntityType_SelectedIndexChanged" 
                                            AutoPostBack="True">
                                    <HeaderTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0" width="200">
                                            <tr>
                                                <td height="30">
                                                    <b>Entity Type</b>
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
									<td class="style5">
                                        <strong>Entity :</strong>&nbsp;&nbsp;&nbsp;&nbsp;
                                <rad:RadComboBox ID="ddlEntity" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlEntity_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="200px" 
                                            onselectedindexchanged="ddlEntity_SelectedIndexChanged">
                                    <HeaderTemplate>
                                        <table border="0" cellpadding="3" cellspacing="0" width="200">
                                            <tr>
                                                <td height="30">
                                                    <b>Entity</b>
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
                                        <br />
                                        <br />
                                        <asp:CheckBox ID="considerAll" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" Text="Consider All Entity" />
                                    </td>
									<td><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey btn searchBtn" 
                                            OnClick="btnSearch_Click" Width="173px" Height="55px"></asp:Button></td>
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
                       
         <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1827px" Height="1450px" 
                            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        </rsweb:ReportViewer>
    
					</td>
				</tr>
			</table>  

        <%--<%# DataBinder.Eval(Container.DataItem, "Name") %>--%>
    
    <uc:footer id="footer1" runat="server"></uc:footer>
</form>

</body>
</html>


