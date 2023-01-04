<%@ Page Language="C#"  Title="Inventory Transaction DateWise" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="InventoryTransactionDateWise.aspx.cs" Inherits="UI_InventoryTransactionDateWise" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
    </telerik:RadDockLayout>

    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="style16">
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0"
                        style="height: 190px; margin-bottom: 4px">

                        <tr>
                            <td class="style10"></td>
                            <td class="style11">From Date:</td>
                            <td class="style12">
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server"></telerik:RadDatePicker>
                            </td>
                            <td align="right" class="style13">To Date:</td>
                            <td class="style14">
                                <telerik:RadDatePicker ID="dtpToDate" runat="server"></telerik:RadDatePicker>
                            </td>
                            <td class="style15"></td>
                        </tr>
                        <tr>
                            <td class="style3"></td>
                            <td class="style6"></td>
                            <td class="style7">
                                <asp:Button ID="btnReport" runat="server" Text="Search" CssClass="btn btn-primary btn-lg"
                                    OnClick="btnReport_Click" Height="66px" Width="182px" BackColor="#49C4E9"></asp:Button>
                            </td>
                            <td align="right" class="style8"></td>
                            <td class="style9"></td>
                            <td class="style17">&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <asp:Label ID="llWorkPeriod" runat="server" Text=""
            Style="font-size: large; font-weight: 700"></asp:Label>
        <tr>
            <td>
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="true" CellSpacing="0"
                    GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource1" OnItemCommand="RadGrid1_ItemCommand" OnColumnCreated="RadGrid1_ColumnCreated" ShowFooter="True">
                    <MasterTableView AutoGenerateColumns="true" DataKeyNames="DocId,DocName,Date" ClientDataKeyNames="DocId,DocName,Date" Height="100%">
                        <Columns>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" ImageUrl="~/Images/view.png"
                                ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="30px" />
                            </telerik:GridButtonColumn>

                        </Columns>
                    </MasterTableView>

                </telerik:RadGrid>
            </td>
        </tr>
    </table>

</asp:Content>
