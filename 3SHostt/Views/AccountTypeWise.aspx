<%@ Page Language="C#" Title="Account Type Wise Report" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="AccountTypeWise.aspx.cs" Inherits="UI_AccountTypeWise" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <table>
        <tr>
            <td class="auto-style3" align="justify">
                <div class="bordered" style="width: auto">
                    <table cellpadding="3" cellspacing="0" border="0" style="width: 66%">
                        <tr>
                            <td align="right" class="auto-style2">From Date:</td>
                            <td class="auto-style5">
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker>
                            </td>
                            <td align="left" class="auto-style4">To Date:<telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker>
                            </td>
                            <td class="auto-style7">&nbsp;</td>
                            <td class="auto-style8"></td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">&nbsp;</td>
                            <td class="auto-style5">
                                <asp:CheckBox ID="chkOpeningBalance" runat="server" Checked="false"
                                    Text="Opening Balance" />
                            </td>
                            <td align="right" class="auto-style4">&nbsp;</td>
                            <td class="auto-style7">&nbsp;</td>
                            <td class="auto-style8"></td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">Account Type</td>
                            <td class="auto-style5">
                                <rad:RadComboBox Skin="WebBlue" ID="ddlAccountType" runat="server" Height="170px" Width="200px" DataValueField="Name"
                                    MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false" OnItemDataBound="ddlAccountType_ItemDataBound"
                                    HighlightTemplatedItems="true" DropDownWidth="200px" OnSelectedIndexChanged="ddlAccountType_SelectedIndexChanged" AutoPostBack="false" ItemRequestTimeout="500" TabIndex="5" NoWrap="True">
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
                                </rad:RadComboBox>
                            </td>
                            <td class="auto-style4">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg"
                                    OnClick="btnSearch_Click" Height="52px" Width="257px" BackColor="#6ECFED"></asp:Button>
                            </td>
                            <td class="auto-style7">&nbsp;</td>
                            <td class="auto-style8"></td>

                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 1px;" class="auto-style1">

                <rsweb:ReportViewer ID="rptViewer1" runat="server" Width="1490px" Height="897px">
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="AccountTypeWiseBalanceDataSetTableAdapters.AccountTypeWiseBalanceTableAdapter" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>


            </td>
        </tr>
    </table>
</asp:Content>
