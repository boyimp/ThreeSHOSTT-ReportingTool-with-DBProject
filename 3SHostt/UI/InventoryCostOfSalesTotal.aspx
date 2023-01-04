<%@ Page Language="C#" AutoEventWireup="true"
CodeFile="InventoryCostOfSalesTotal.aspx.cs"
Inherits="UI_InventoryCostOfSalesTotal" %> <%@ Register TagPrefix="uc"
TagName="header" Src="../Includes/Header.ascx" %> <%@ Register TagPrefix="uc"
TagName="footer" Src="../Includes/Footer.ascx" %> <%@ Register
Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI"
tagprefix="telerik" %> <%@ Register Assembly="RadComboBox.Net2"
Namespace="Telerik.WebControls" TagPrefix="rad" %> <%@ Register
assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral,
PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms"
tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head1" runat="server">
    <title>Inventory Cost Of Sales Total</title>
    <!-- CSS only -->
    <link
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css"
      rel="stylesheet"
      integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT"
      crossorigin="anonymous"
    />
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
      .auto-style1 {
        width: 155px;
      }
      .auto-style2 {
        width: 155px;
        height: 60px;
        text-align: right;
      }
      .auto-style3 {
        height: 60px;
      }
      .auto-style4 {
        height: 60px;
        width: 242px;
      }
      .auto-style5 {
        width: 242px;
      }
      .auto-style6 {
        width: 104px;
        height: 60px;
      }
      .auto-style7 {
        width: 104px;
      }
      .auto-style8 {
        width: 155px;
        height: 46px;
      }
      .auto-style9 {
        width: 242px;
        height: 46px;
      }
      .auto-style10 {
        width: 104px;
        height: 46px;
      }
      .auto-style11 {
        height: 46px;
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
                    cellpadding="3"
                    cellspacing="0"
                    border="0"
                  >
                    <tr>
                      <td class="auto-style2">From Date:</td>
                      <td class="auto-style4">
                        <telerik:RadDatePicker
                          ID="dtpFromDate"
                          runat="server"
                        ></telerik:RadDatePicker>
                      </td>
                      <td align="right" class="auto-style6">To Date:</td>
                      <td class="auto-style3">
                        <telerik:RadDatePicker
                          ID="dtpToDate"
                          runat="server"
                        ></telerik:RadDatePicker>
                      </td>
                      <td class="auto-style3"></td>
                    </tr>
                    <tr>
                      <td align="right" class="auto-style8">
                        Inventory Group Item:
                      </td>
                      <td class="auto-style9">
                        <rad:RadComboBox
                          Skin="WebBlue"
                          id="ddlGroupItem"
                          Runat="server"
                          Height="170px"
                          Width="200px"
                          DataValueField="GroupCode"
                          MarkFirstMatch="true"
                          AllowCustomText="true"
                          EnableLoadOnDemand="false"
                          OnItemDataBound="ddlGroupItem_ItemDataBound"
                          HighlightTemplatedItems="true"
                          DropDownWidth="200px"
                          onselectedindexchanged="ddlGroupItem_SelectedIndexChanged"
                          AutoPostBack="true"
                          ItemRequestTimeout="500"
                          TabIndex="5"
                        >
                          <HeaderTemplate>
                            <table
                              width="50"
                              cellpadding="3"
                              cellspacing="0"
                              border="0"
                            >
                              <tr>
                                <td height="30"><b>Group Item</b></td>
                              </tr>
                            </table>
                          </HeaderTemplate>
                          <ItemTemplate>
                            <table
                              width="50"
                              cellpadding="3"
                              cellspacing="0"
                              border="0"
                            >
                              <tr>
                                <td>
                                  <%# DataBinder.Eval(Container.DataItem,
                                  "GroupCode")%>
                                </td>
                              </tr>
                            </table>
                          </ItemTemplate>
                        </rad:RadComboBox>
                      </td>
                      <td align="right" class="auto-style10">
                        Inventory Item:
                      </td>
                      <td class="auto-style11">
                        <rad:RadComboBox
                          ID="ddlInventoryItem"
                          Runat="server"
                          AllowCustomText="true"
                          DataValueField="id"
                          DropDownWidth="200px"
                          EnableLoadOnDemand="false"
                          Height="170px"
                          HighlightTemplatedItems="true"
                          ItemRequestTimeout="500"
                          MarkFirstMatch="true"
                          OnItemDataBound="ddlInventoryItem_ItemDataBound"
                          Skin="WebBlue"
                          TabIndex="5"
                          Width="200px"
                        >
                          <HeaderTemplate>
                            <table border="0" cellpadding="3" cellspacing="0">
                              <tr>
                                <td height="30">
                                  <b>Item</b>
                                </td>
                              </tr>
                            </table>
                          </HeaderTemplate>
                          <ItemTemplate>
                            <table border="0" cellpadding="3" cellspacing="0">
                              <tr>
                                <td>
                                  <%# DataBinder.Eval(Container.DataItem,
                                  "Name")%>
                                </td>
                              </tr>
                            </table>
                          </ItemTemplate>
                        </rad:RadComboBox>
                      </td>
                      <td class="auto-style11"></td>
                    </tr>
                    <tr>
                      <td align="right" class="auto-style1">Warehouse:</td>
                      <td class="auto-style5">
                        <rad:RadComboBox
                          Skin="WebBlue"
                          id="ddlWarehouse"
                          Runat="server"
                          Height="170px"
                          Width="200px"
                          DataValueField="id"
                          MarkFirstMatch="true"
                          AllowCustomText="true"
                          EnableLoadOnDemand="false"
                          HighlightTemplatedItems="true"
                          DropDownWidth="200px"
                          ItemRequestTimeout="500"
                          TabIndex="5"
                          OnItemDataBound="ddlWarehouse_ItemDataBound"
                        >
                          <HeaderTemplate>
                            <table
                              width="200"
                              cellpadding="3"
                              cellspacing="0"
                              border="0"
                            >
                              <tr>
                                <td height="30"><b>Group Item</b></td>
                              </tr>
                            </table>
                          </HeaderTemplate>
                          <ItemTemplate>
                            <table
                              width="200"
                              cellpadding="3"
                              cellspacing="0"
                              border="0"
                            >
                              <tr>
                                <td>
                                  <%# DataBinder.Eval(Container.DataItem,
                                  "Name")%>
                                </td>
                              </tr>
                            </table>
                          </ItemTemplate>
                        </rad:RadComboBox>
                      </td>
                      <td class="auto-style7">&nbsp;</td>
                      <td>
                        <asp:Button
                          ID="btnSearch"
                          runat="server"
                          Text="Search"
                          CssClass="buttonGrey btn searchBtn"
                          OnClick="btnSearch_Click"
                          Height="80px"
                          Width="229px"
                        ></asp:Button>
                      </td>
                      <td></td>
                    </tr>
                  </table>
                </div>
              </td>
            </tr>
            <tr>
              <td>
                <asp:Label
                  ID="lblWorkPeriod"
                  runat="server"
                  style="font-size: large; font-weight: 700"
                  Visible="False"
                ></asp:Label>
              </td>
            </tr>
            <tr>
              <td style="padding-left: 1px; width: 100%">
                <rsweb:ReportViewer
                  ID="rptViewer1"
                  runat="server"
                  Width="2097px"
                  Height="1110px"
                >
                </rsweb:ReportViewer>
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
