<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Entities.aspx.cs" Inherits="UI_Tickets" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Entities And Accounts</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .auto-style1 {
            width: 233px;
        }
        .auto-style2 {
            width: 154px;
            text-align: right;
        }
        .auto-style3 {
            width: 154px;
            height: 36px;
        }
        .auto-style4 {
            width: 233px;
            height: 36px;
        }
        .auto-style5 {
            height: 36px;
        }
        .auto-style6 {
            width: 154px;
            height: 63px;
            text-align: right;
        }
        .auto-style7 {
            width: 233px;
            height: 63px;
        }
        .auto-style8 {
            height: 63px;
        }
        .auto-style9 {
            width: 154px;
            height: 36px;
            text-align: right;
        }
        .auto-style10 {
            height: 36px;
            width: 144px;
        }
        .auto-style11 {
            height: 63px;
            width: 144px;
        }
        .auto-style12 {
            width: 144px;
        }
        .auto-style13 {
            height: 36px;
            width: 283px;
        }
        .auto-style14 {
            height: 63px;
            width: 283px;
        }
        .auto-style15 {
            width: 283px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc:header ID="header1" runat="server"></uc:header>
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <div class="bordered" style="width: 100%">
                    <table width="100%" cellpadding="5" cellspacing="0" border="0">
                        <tr>
                            <td class="auto-style3">
                                &nbsp;</td>
                            <td class="auto-style4">
                                &nbsp;</td>
                            <td align="right" class="auto-style10">
                                &nbsp;</td>
                            <td class="auto-style13">
                                &nbsp;</td>
                            <td class="auto-style5">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style9">
                                Transaction From Date:
                            </td>
                            <td class="auto-style4">
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td align="right" class="auto-style10">
                                To Date:
                            </td>
                            <td class="auto-style13">
                                <telerik:RadDatePicker ID="dtpToDate" runat="server">
                                </telerik:RadDatePicker>
                            </td>
                            <td class="auto-style5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style6">
                    <asp:CheckBox ID="cboCreationDate" runat="server" AutoPostBack="True" OnCheckedChanged="ChckedChanged" Text="Creation From Date" TextAlign="Left" />
                            </td>
                            <td class="auto-style7">
                                <telerik:RadDatePicker ID="dtpCreationFromDate" runat="server" Enabled="False">
                                </telerik:RadDatePicker>
                            </td>
                            <td align="right" class="auto-style11">
                                To Date:</td>
                            <td class="auto-style14">
                                <telerik:RadDatePicker ID="dtpCreationToDate" runat="server" Enabled="False">
                                </telerik:RadDatePicker>
                            </td>
                            <td class="auto-style8">
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style2">
                                Entity Type:
                            </td>
                            <td class="auto-style1">
                                <rad:RadComboBox ID="ddlTicketType" runat="server" AllowCustomText="true" DataValueField="id"
                                    DropDownWidth="200px" EnableLoadOnDemand="false" Height="170px" HighlightTemplatedItems="true"
                                    ItemRequestTimeout="500" MarkFirstMatch="true" OnItemDataBound="ddlTicketType_ItemDataBound"
                                    Skin="WebBlue" TabIndex="5" Width="152px">
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
                            <td align="right" class="auto-style12">
                                        <asp:CheckBox ID="CheckCurrentWorkPeriod" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="ChckedChangedWorkPeriod" Text="Current Work Period"/>
                            </td>
                            <td class="auto-style15">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonGrey" OnClick="btnSearch_Click"
                                    Width="157px" Height="61px"></asp:Button>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;
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
            <td style="padding-left: 5px;">
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="true" CellSpacing="0"
                    GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource1"
                    OnColumnCreated="RadGrid1_ColumnCreated" ShowFooter="True" OnPdfExporting="RadGrid1_PdfExporting">
                    <MasterTableView AutoGenerateColumns="true" DataKeyNames="id" ClientDataKeyNames="id">
                        <Columns>
                           <%-- <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Preview" ImageUrl="~/Images/view.png"
                                ItemStyle-HorizontalAlign="Center" UniqueName="More">
                                <HeaderStyle Width="20px" />
                            </telerik:GridButtonColumn>--%>
                        </Columns>
                    </MasterTableView>
                    <ExportSettings>
                        <Pdf BorderType="AllBorders"></Pdf>
                        <Pdf BorderStyle="Medium"></Pdf>
                    </ExportSettings>
                </telerik:RadGrid>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
        <%--<%# DataBinder.Eval(Container.DataItem, "Name") %>--%>
    <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
