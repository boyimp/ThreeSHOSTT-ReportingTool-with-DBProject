<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Views_login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="author" content="GeeksLabs">
    <link rel="shortcut icon" href="img/favicon.png">
    <title>3SHostt Reporting Tool</title>
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/bootstrap-theme.css" rel="stylesheet">
    <link href="css/elegant-icons-style.css" rel="stylesheet" />
    <link href="css/font-awesome.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet">
    <link href="css/style-responsive.css" rel="stylesheet" />
</head>

  <body class="login-img3-body">

    <div class="container">

      <form class="login-form" runat="server">
          <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="login-wrap card">
            <p class="login-img"><img src="../images/3sHost.png" border="0" /></p>
            <div class="input-group">
                 <span class="input-group-addon"><i class="icon_profile"></i></span>
                <asp:TextBox ID="txtUsername" runat="server"  type="text" CssClass="form-control" placeholder="Username"></asp:TextBox>
            </div>
            <div class="input-group">
                <span class="input-group-addon"><i class="icon_key_alt"></i></span>
                <asp:TextBox ID="txtPassword" runat="server" type="password" class="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>

            </div>
            <label class="checkbox">
                <input type="checkbox" value="remember-me"> Remember me
            </label>
            <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="btn btn-primary btn-lg btn-block" type="submit" OnClick="btnLogin_Click" />
        </div>

        <div>
            <p style="text-align:center;"><asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label></p>
        </div>
      </form>
    <div class="text-right">
            <div class="credits">

                <a href="http://3s-bd.com/">Reporting Tool</a> by <a href="http://3s-bd.com/">3S-BD</a>
            </div>
        </div>
    </div>
  </body>
</html>

