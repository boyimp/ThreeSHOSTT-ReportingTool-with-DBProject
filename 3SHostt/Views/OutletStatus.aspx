<%@ Page Language="C#"  Title="Outlet Status" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="OutletStatus.aspx.cs" Inherits="UI_OutletStatus" %>

<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
    </telerik:RadDockLayout>

    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="auto-style1">

            </td>
        </tr>
        <tr>
            <td>

                <asp:Label ID="lblWorkPeriod" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label>

            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>

        <tr>
            <td style="padding-left: 1px; width: 100%;">

                <telerik:RadGrid ID="RadGrid1" AllowSorting="true" AllowFilteringByColumn="true"
                    ShowFooter="True" AutoGenerateColumns="false" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource" OnColumnCreated="RadGrid1_ColumnCreated"
                   >
                    <MasterTableView AutoGenerateColumns="true" >
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
    </table>
</asp:Content>