<%@ Page Language="C#" AutoEventWireup="true"
CodeFile="InventoryTransactionDateWise.aspx.cs"
Inherits="UI_InventoryTransactionDateWise" %> <%@ Register TagPrefix="uc"
TagName="header" Src="../Includes/Header.ascx" %> <%@ Register TagPrefix="uc"
TagName="footer" Src="../Includes/Footer.ascx" %> <%@ Register
Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI"
tagprefix="telerik" %> <%@ Register Assembly="RadComboBox.Net2"
Namespace="Telerik.WebControls" TagPrefix="rad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Inventory Transaction DateWise.aspx</title>
    <!-- CSS only -->
    <link
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css"
      rel="stylesheet"
      integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT"
      crossorigin="anonymous"
    />
    <link rel="stylesheet" type="text/css" href="../styles.css" />

    <style type="text/css">
      .style3 {
        width: 436px;
        height: 77px;
      }
      .style6 {
        width: 99px;
        font-size: 13px;
        font-weight: bold;
        height: 77px;
      }
      .style7 {
        width: 369px;
        text-align: right;
        height: 77px;
      }
      .style8 {
        width: 64px;
        font-size: 13px;
        font-weight: bold;
        height: 77px;
      }
      .style9 {
        width: 313px;
        height: 77px;
      }
      .style10 {
        width: 436px;
        height: 114px;
      }
      .style11 {
        width: 99px;
        height: 114px;
        font-size: 13px;
        font-weight: bold;
      }
      .style12 {
        width: 369px;
        height: 114px;
      }
      .style13 {
        width: 64px;
        height: 114px;
        font-size: 13px;
        font-weight: bold;
      }
      .style14 {
        width: 313px;
        height: 114px;
      }
      .style15 {
        height: 114px;
      }
      .style16 {
        height: 199px;
      }
      .style17 {
        height: 77px;
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
      <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
      </telerik:RadDockLayout>
      <%--<asp:UpdatePanel
        ID="UpdatePanel1"
        runat="server"
        UpdateMode="Conditional"
      >
        <ContentTemplate
          >--%>
          <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
              <td class="style16">
                <div class="bordered" style="width: 100%">
                  <table
                    width="100%"
                    cellpadding="5"
                    cellspacing="0"
                    border="0"
                    style="height: 190px; margin-bottom: 4px"
                    class="filterContainer"
                  >
                    <tr>
                      <td class="style10"></td>
                      <td class="style11">From Date:</td>
                      <td class="style12">
                        <telerik:RadDatePicker
                          ID="dtpFromDate"
                          runat="server"
                        ></telerik:RadDatePicker>
                      </td>
                      <td align="right" class="style13">To Date:</td>
                      <td class="style14">
                        <telerik:RadDatePicker
                          ID="dtpToDate"
                          runat="server"
                        ></telerik:RadDatePicker>
                      </td>
                      <td class="style15"></td>
                    </tr>
                    <tr>
                      <td class="style3"></td>
                      <td class="style6"></td>
                      <td class="style7">
                        <asp:Button
                          ID="btnReport"
                          runat="server"
                          Text="Search"
                          CssClass="buttonGrey btn searchBtn"
                          OnClick="btnReport_Click"
                          Height="66px"
                          Width="182px"
                          BackColor="#49C4E9"
                        ></asp:Button>
                      </td>
                      <td align="right" class="style8"></td>
                      <td class="style9"></td>
                      <td class="style17">&nbsp;&nbsp;&nbsp;</td>
                    </tr>
                  </table>
                </div>
              </td>
            </tr>
            <asp:Label
              ID="llWorkPeriod"
              runat="server"
              Text=""
              style="font-size: large; font-weight: 700"
            ></asp:Label>
            <tr>
              <td>
                <telerik:RadGrid
                  ID="RadGrid1"
                  runat="server"
                  AllowFilteringByColumn="true"
                  CellSpacing="0"
                  GridLines="None"
                  OnNeedDataSource="RadGrid1_NeedDataSource1"
                  OnItemCommand="RadGrid1_ItemCommand"
                  oncolumncreated="RadGrid1_ColumnCreated"
                  ShowFooter="True"
                >
                  <MasterTableView
                    AutoGenerateColumns="true"
                    DataKeyNames="DocId,DocName,Date"
                    ClientDataKeyNames="DocId,DocName,Date"
                    Height="100%"
                  >
                    <Columns>
                      <telerik:GridButtonColumn
                        ButtonType="ImageButton"
                        CommandName="Preview"
                        ImageUrl="~/Images/view.png"
                        ItemStyle-HorizontalAlign="Center"
                      >
                        <HeaderStyle Width="30px" />
                      </telerik:GridButtonColumn>
                    </Columns>
                  </MasterTableView>
                </telerik:RadGrid>
              </td>
            </tr>
          </table>

          <%--</ContentTemplate
        > </asp:UpdatePanel
      >--%>

      <uc:footer id="footer1" runat="server"></uc:footer>
    </form>
  </body>
</html>
