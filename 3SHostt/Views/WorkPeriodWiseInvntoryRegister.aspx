<%@ Page Language="C#" Title="Work Period Stock" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="WorkPeriodWiseInvntoryRegister.aspx.cs"
    Inherits="UI_WorkPeriodWiseInvntoryRegister" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="3" cellspacing="0" border="0">
                        <tr>
                            <td class="auto-style2">
                                From Date:
                            </td>
                            <td class="auto-style4">
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td align="right" class="auto-style6">
                                To Date:
                            </td>
                            <td class="auto-style8">
                                <telerik:RadDatePicker ID="dtpToDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td class="auto-style1">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style3">
                                Inventory Group Item:
                            </td>
                            <td class="auto-style5">
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
                            <td align="right" class="auto-style7">
                                Inventory Item:
                            </td>
                            <td class="auto-style9">
                                <rad:RadComboBox ID="ddlInventoryItem" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlInventoryItem_ItemDataBound"
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
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style3">
                                Warehouse:
                            </td>
                            <td class="auto-style5">
                                <rad:RadComboBox Skin="WebBlue" ID="ddlWarehouse" runat="server" Height="170px" Width="200px"
                                    DataValueField="id" MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false"
                                    HighlightTemplatedItems="true" DropDownWidth="200px" ItemRequestTimeout="500"
                                    TabIndex="5" OnItemDataBound="ddlWarehouse_ItemDataBound">
                                    <HeaderTemplate>
                                        <table width="200" cellpadding="3" cellspacing="0" border="0">
                                            <tr>
                                                <td height="30">
                                                    <b>Group Item</b>
                                                </td>
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
                            <td class="auto-style7">
                                &nbsp;
                            </td>
                            <td class="auto-style9">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg" OnClick="btnSearch_Click"
                                    Height="68px" Width="231px"></asp:Button>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Excel.png" OnClick="ImgbtnExcel_Click"
                    AlternateText="Excel" ToolTip="Excel" />
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Word.png" OnClick="ImgbtnWord_Click"
                    AlternateText="Word" ToolTip="Word" />
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/PDF.png" OnClick="ImgbtnPDF_Click"
                    AlternateText="PDF" ToolTip="PDF" />
                <asp:Label ID="lblWorkPeriod" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 1px; width: 100%;">
                <telerik:RadGrid ID="RadGrid1" AllowSorting="true" AllowFilteringByColumn="true"
                    ShowFooter="True" AutoGenerateColumns="false" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource1"
                    OnItemCommand="RadGrid1_ItemCommand" OnColumnCreated="RadGrid1_ColumnCreated"
                    OnPdfExporting="RadGrid1_PdfExporting">
                    <MasterTableView AutoGenerateColumns="true" DataKeyNames="InventoryID,WarehouseID,StartDate,EndDate,GroupCode,Inventory,Warehouse"
                        ClientDataKeyNames="InventoryID,WarehouseID,StartDate,EndDate,GroupCode,Inventory,Warehouse">
                        <Columns>
                            <telerik:GridButtonColumn HeaderText="More" ButtonType="ImageButton" CommandName="Preview"
                                ImageUrl="~/Images/view.png" ItemStyle-HorizontalAlign="Center" UniqueName="More">
                                <HeaderStyle Width="35px" />
                            </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ExportSettings>
                        <Pdf BorderType="AllBorders">
                        </Pdf>
                        <Pdf BorderStyle="Medium">
                        </Pdf>
                    </ExportSettings>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
</asp:Content>
