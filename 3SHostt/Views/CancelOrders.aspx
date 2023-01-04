<%@ Page Language="C#" Title="Order Cancel Report" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="CancelOrders.aspx.cs" Inherits="UI_CancelOrders" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
    </telerik:RadDockLayout>

    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="style6">
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0"
                        style="height: 134px">
                        <tr>
                            <td class="style7">From Date:
                            </td>
                            <td class="style8">
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td align="right" class="style9">To Date:
                            </td>
                            <td class="style10">
                                <telerik:RadDatePicker ID="dtpToDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td class="style11"></td>
                        </tr>
                        <tr>
                            <td class="style3"></td>
                            <td class="style2">&nbsp;&nbsp;&nbsp;
                            </td>
                            <td align="right" class="style4">
                                <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true"
                                    OnCheckedChanged="ChckedChanged" Text="Current Work Period" />
                            </td>
                            <td class="style5">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg" OnClick="btnSearch_Click"
                                    Width="142px" Height="60px"></asp:Button>
                            </td>
                            <td></td>
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
            <td style="padding-left: 5px;">
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="true" CellSpacing="0"
                    GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource1" OnColumnCreated="RadGrid1_ColumnCreated"
                    ShowFooter="True" OnPdfExporting="RadGrid1_PdfExporting">
                    <MasterTableView AutoGenerateColumns="true" DataKeyNames="id" ClientDataKeyNames="id"
                        Height="100%">
                        <Columns>
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
        <tr>
            <td></td>
        </tr>
    </table>
</asp:Content>
