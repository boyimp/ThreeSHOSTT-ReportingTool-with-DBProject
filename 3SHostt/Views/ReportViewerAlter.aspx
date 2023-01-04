<%@ Page Language="C#"  Title="Report Viewer" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="ReportViewerAlter.aspx.cs" Inherits="UI_ReportViewerAlter" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <rsweb:ReportViewer ID="rptViewer" runat="server" Width="2310px"
            Height="1923px">
        </rsweb:ReportViewer>
    </div>
 </asp:Content>
