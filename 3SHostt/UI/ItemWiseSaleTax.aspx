<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemWiseSaleTax.aspx.cs"
Inherits="UI_ItemWiseSaleTax" %> <%@ Register TagPrefix="uc" TagName="header"
Src="../Includes/Header.ascx" %> <%@ Register TagPrefix="uc" TagName="footer"
Src="../Includes/Footer.ascx" %> <%@ Register Assembly="AjaxControlToolkit"
Namespace="AjaxControlToolkit" TagPrefix="act" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head2" runat="server">
    <title>Item Wise Sale Tax</title>
    <script type="text/javascript" src="../jquery.js"></script>
    <!-- CSS only -->
    <link
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css"
      rel="stylesheet"
      integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT"
      crossorigin="anonymous"
    />
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <script type="text/javascript">
      $(document).ready(function () {
        $("#nav li a").click(function () {
          try {
            var sdss = document.getElementById("dumdiv");
            if (sdss == null) {
              var s = $(this).attr("id");
              var imgid = $("#" + s + " img").attr("id");
              var imgsrc = $("#" + imgid + "").attr("src");
              if (imgsrc == "../images/insert.jpg") {
                $("#" + imgid + "").attr("src", "../images/remove.jpg");
                $(this).next().slideDown(800);
              } else {
                $("#" + imgid + "").attr("src", "../images/insert.jpg");
                $(this).next().slideUp(800);
              }
            }
          } catch (e) {}
        });
      });
    </script>
    <link
      rel="stylesheet"
      href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"
    />
    <link rel="stylesheet" href="/resources/demos/style.css" />
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
      //$(document).ready(function () {
      //    setInterval(
      //        $(function () {
      //            $("#progressbar").progressbar({
      //                value: $("#progressbarValue").val()
      //            });
      //        }) , 100);
      //});
    </script>
    <meta name="theme-color" content="#FFA817" />
    <meta name="msapplication-navbutton-color" content="#FFA817" />
    <meta name="apple-mobile-web-app-status-bar-style" content="#FFA817" />
    <style type="text/css">
      .auto-style12 {
        width: 108px;
        text-align: right;
        height: 36px;
      }
      .auto-style13 {
        width: 188px;
        height: 36px;
      }
      .auto-style14 {
        width: 177px;
        height: 36px;
      }
      .auto-style15 {
        width: 212px;
        height: 36px;
      }
      .auto-style16 {
        height: 36px;
      }
      .auto-style20 {
        width: 212px;
        height: 51px;
      }
      .auto-style26 {
        width: 108px;
        text-align: right;
        height: 55px;
      }
      .auto-style27 {
        width: 188px;
        height: 55px;
      }
      .auto-style28 {
        width: 177px;
        height: 55px;
      }
      .auto-style29 {
        width: 212px;
        height: 55px;
      }
      .auto-style100 {
        width: 100%;
        text-align: center;
        margin: auto;
      }
      .ui-widget-header {
        border: 1px solid #dddddd;
        background: #00cc00 !important;
        color: #333333;
        font-weight: bold;
      }

      .filterContainer {
        margin: 0px 20px;
      }
      .searchBtn {
        height: 45px !important;
        width: 120px !important;
        background-color: #fa7242;
        color: white;
        font-weight: bold;
      }
      .searchBtn:hover {
        background-color: #f86530 !important;
        color: white !important;
      }

      #ImageButton1,
      #ImageButton2,
      #ImageButton3 {
        width: 35px !important;
      }
    </style>
  </head>
  <body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      <uc:header id="header1" runat="server"></uc:header>
      <div align="center">
        <div class="left">
          <table
            width="100%"
            cellpadding="0"
            cellspacing="0"
            border="0"
            class="filterContainer"
          >
            <tr>
              <td>
                <div class="bordered" style="width: 100%">
                  <table
                    width="100%"
                    cellpadding="5"
                    cellspacing="0"
                    border="0"
                  >
                    <tr>
                      <td class="auto-style12"></td>
                      <td class="auto-style13"></td>
                      <td align="right" class="auto-style14"></td>
                      <td class="auto-style15"></td>
                      <td class="auto-style16"></td>
                    </tr>
                    <tr>
                      <td class="auto-style26">From Date:</td>
                      <td class="auto-style27">
                        <telerik:RadDateTimePicker
                          ID="dtpFromDate"
                          runat="server"
                          AutoPostBackControl="Both"
                          Height="21px"
                          Width="172px"
                        ></telerik:RadDateTimePicker>
                      </td>
                      <td align="right" class="auto-style28">To Date:</td>
                      <td class="auto-style29">
                        <telerik:RadDateTimePicker
                          ID="dtpToDate"
                          runat="server"
                          Height="21px"
                          Width="184px"
                        ></telerik:RadDateTimePicker>
                      </td>
                      <td class="auto-style20">
                        <asp:Button
                          ID="btnSearch"
                          runat="server"
                          Text="Search"
                          CssClass="buttonGrey btn searchBtn"
                          OnClick="btnSearch_Click"
                          Width="142px"
                          Height="66px"
                        ></asp:Button>
                      </td>
                    </tr>
                    <tr>
                      <td class="auto-style26" style="color: red">&nbsp;</td>
                      <td class="auto-style27">
                        <asp:Label
                          ID="TotalPendingProcess"
                          runat="server"
                          Font-Bold="True"
                          Font-Italic="True"
                          Font-Names="Tahoma"
                          ForeColor="Red"
                          Font-Size="Medium"
                        ></asp:Label>
                      </td>
                      <td align="right" class="auto-style28"></td>
                      <td class="auto-style29">
                        <%--
                        <asp:Label
                          ID="RunningPendingProcess"
                          runat="server"
                          Font-Bold="True"
                          Font-Italic="True"
                          Font-Names="Tahoma"
                          ForeColor="#00CC00"
                          Font-Size="Medium"
                        ></asp:Label>
                        --%>

                        <asp:UpdatePanel
                          ID="UpdatePanel1"
                          runat="server"
                          UpdateMode="Conditional"
                        >
                          <ContentTemplate>
                            <asp:Label
                              ID="RunningPendingProcess"
                              runat="server"
                              Font-Bold="True"
                              Font-Italic="True"
                              Font-Names="Tahoma"
                              ForeColor="#00CC00"
                              Font-Size="Medium"
                            ></asp:Label>
                            <asp:Timer
                              ID="Timer1"
                              runat="server"
                              OnTick="Timer1_Tick"
                              Enabled="true"
                              Interval="100"
                            ></asp:Timer>
                          </ContentTemplate>
                        </asp:UpdatePanel>
                      </td>
                      <td class="auto-style20"></td>
                    </tr>
                    <tr>
                      <td class="auto-style26" style="color: red">&nbsp;</td>
                      <td class="auto-style27"></td>
                      <td align="right" class="auto-style28">
                        <%--<asp:Label
                          ID="progressbarValue"
                          runat="server"
                        ></asp:Label
                        >--%>
                        <asp:Button
                          ID="btnProcess"
                          runat="server"
                          Text="Process"
                          BackColor="DarkGreen"
                          CssClass="buttonGrey"
                          OnClick="btnProcess_Click"
                          Width="142px"
                          Height="66px"
                        ></asp:Button>
                      </td>
                      <td class="auto-style29"></td>
                      <td class="auto-style20"></td>
                    </tr>
                  </table>
                </div>
              </td>
            </tr>
            <tr>
              <td class="auto-style100">
                <asp:UpdatePanel
                  ID="UpdatePanel2"
                  runat="server"
                  UpdateMode="Conditional"
                >
                  <ContentTemplate>
                    <asp:Label
                      ID="DateErrorMessage"
                      runat="server"
                      Font-Bold="True"
                      Font-Italic="True"
                      Font-Names="Tahoma"
                      ForeColor="Red"
                    ></asp:Label>
                  </ContentTemplate>
                </asp:UpdatePanel>
              </td>
            </tr>
          </table>
        </div>
      </div>
      <uc:footer id="footer1" runat="server"></uc:footer>
    </form>
  </body>
</html>
