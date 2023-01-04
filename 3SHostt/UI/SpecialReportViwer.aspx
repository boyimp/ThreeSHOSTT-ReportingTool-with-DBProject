<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpecialReportViwer.aspx.cs" Inherits="UI_SpecialReportViwer" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .resp-container {
    position: relative;
    overflow: hidden;
    padding-top: 56.25%;
}
        .resp-iframe {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    border: 0;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc:header ID="header1" runat="server"></uc:header>
  <div class="resp-container">
    <iframe class="resp-iframe" src="http://localhost:2008/report/" gesture="media"  allow="encrypted-media" allowfullscreen></iframe>
</div>
        <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
</body>
</html>
