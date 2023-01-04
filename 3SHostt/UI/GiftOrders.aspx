<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GiftOrders.aspx.cs"
Inherits="UI_Tickets" %> <%@ Register TagPrefix="uc" TagName="header"
Src="../Includes/Header.ascx" %> <%@ Register TagPrefix="uc" TagName="footer"
Src="../Includes/Footer.ascx" %> <%@ Register Assembly="AjaxControlToolkit"
Namespace="AjaxControlToolkit" TagPrefix="act" %> <%@ Register
Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %> <%@
Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls"
TagPrefix="rad" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Gift Order Entries</title>
    <!-- CSS only -->
    <link
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css"
      rel="stylesheet"
      integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT"
      crossorigin="anonymous"
    />
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style>
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
      <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
      <uc:header ID="header1" runat="server"></uc:header>
      <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
      </telerik:RadDockLayout>
      <%--<asp:UpdatePanel
        ID="UpdatePanel1"
        runat="server"
        UpdateMode="Conditional"
      >
        <ContentTemplate
          >--%>
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
                      <td>From Date:</td>
                      <td>
                        <telerik:RadDatePicker ID="dtpFromDate" runat="server">
                        </telerik:RadDatePicker>
                      </td>
                      <td align="right">To Date:</td>
                      <td>
                        <telerik:RadDatePicker ID="dtpToDate" runat="server">
                        </telerik:RadDatePicker>
                      </td>
                      <td>
                        <asp:CheckBox
                          ID="CheckCurrentWorkPeriod"
                          runat="server"
                          AutoPostBack="true"
                          Checked="true"
                          OnCheckedChanged="ChckedChanged"
                          Text="Current Work Period"
                        />
                      </td>
                    </tr>
                    <tr>
                      <td></td>
                      <td>
                        <asp:Button
                          ID="btnSearch"
                          runat="server"
                          Text="Search"
                          CssClass="buttonGrey btn searchBtn"
                          OnClick="btnSearch_Click"
                          Width="142px"
                        ></asp:Button
                        >&nbsp;&nbsp;&nbsp;
                      </td>
                      <td align="right"></td>
                      <td></td>
                      <td></td>
                    </tr>
                  </table>
                </div>
              </td>
            </tr>
            <tr>
              <td>
                <asp:ImageButton
                  ID="ImageButton1"
                  runat="server"
                  ImageUrl="~/Images/Excel.png"
                  OnClick="ImgbtnExcel_Click"
                  AlternateText="Excel"
                  ToolTip="Excel"
                />
                <asp:ImageButton
                  ID="ImageButton2"
                  runat="server"
                  ImageUrl="~/Images/Word.png"
                  OnClick="ImgbtnWord_Click"
                  AlternateText="Word"
                  ToolTip="Word"
                />
                <asp:ImageButton
                  ID="ImageButton3"
                  runat="server"
                  ImageUrl="~/Images/PDF.png"
                  OnClick="ImgbtnPDF_Click"
                  AlternateText="PDF"
                  ToolTip="PDF"
                />
              </td>
            </tr>
            <asp:Label
              ID="lblWorkPeriod"
              runat="server"
              Text=""
              Style="font-size: large; font-weight: 700"
            ></asp:Label>
            <tr>
              <td style="padding-left: 5px">
                <telerik:RadGrid
                  ID="RadGrid1"
                  runat="server"
                  AllowFilteringByColumn="true"
                  CellSpacing="0"
                  GridLines="None"
                  OnNeedDataSource="RadGrid1_NeedDataSource1"
                  OnColumnCreated="RadGrid1_ColumnCreated"
                  ShowFooter="True"
                  OnPdfExporting="RadGrid1_PdfExporting"
                >
                  <MasterTableView
                    AutoGenerateColumns="true"
                    DataKeyNames="id"
                    ClientDataKeyNames="id"
                  >
                    <Columns> </Columns>
                  </MasterTableView>
                  <ExportSettings>
                    <Pdf BorderType="AllBorders"> </Pdf>
                    <Pdf BorderStyle="Medium"> </Pdf>
                  </ExportSettings>
                </telerik:RadGrid>
              </td>
            </tr>
            <tr>
              <td></td>
            </tr>
          </table>
          <%--</ContentTemplate
        > </asp:UpdatePanel
      >--%>
      <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
  </body>
</html>
