﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportviewerWithoutMaster.aspx.cs" Inherits="ReportviewerWithoutMaster" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    

    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager1" runat="server" />
         <rsweb:ReportViewer ID="rptViewer" runat="server" Width="839px" Height="662px">
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
