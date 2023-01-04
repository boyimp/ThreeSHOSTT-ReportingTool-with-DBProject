<%@ Page Language="C#" AutoEventWireup="true"
CodeFile="InventoryPotentialRevenue.aspx.cs"
Inherits="UI_InventoryPotentialRevenue" %> <%@ Register TagPrefix="uc"
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
    <title>Inventory Potential Revenue</title>
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
      <asp:ScriptManager runat="server"></asp:ScriptManager>
      <uc:header ID="header1" runat="server"></uc:header>
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
              <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                  <td></td>
                  <td></td>
                  <td align="right"></td>
                  <td></td>
                  <td></td>
                  <td></td>
                  <td></td>
                </tr>
                <tr>
                  <td>From Date:</td>
                  <td>
                    <telerik:RadDatePicker
                      ID="dtpFromDate"
                      runat="server"
                    ></telerik:RadDatePicker>
                  </td>
                  <td align="right">To Date:</td>
                  <td>
                    <telerik:RadDatePicker
                      ID="dtpToDate"
                      runat="server"
                    ></telerik:RadDatePicker>
                  </td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td></td>
                </tr>
                <tr>
                  <td align="right">Inventory Group Item:</td>
                  <td>
                    <telerik:RadComboBox
                      RenderMode="Lightweight"
                      ID="ddlGroupItem"
                      runat="server"
                      DataTextField="Name"
                      CheckBoxes="true"
                      EnableCheckAllItemsCheckBox="true"
                      Width="250"
                      Skin="MetroTouch"
                    >
                    </telerik:RadComboBox>
                  </td>
                  <td align="right">Brand:</td>
                  <td>
                    <telerik:RadComboBox
                      RenderMode="Lightweight"
                      ID="ddlBrand"
                      runat="server"
                      DataTextField="Name"
                      CheckBoxes="true"
                      EnableCheckAllItemsCheckBox="true"
                      Width="250"
                      Skin="MetroTouch"
                    >
                    </telerik:RadComboBox>
                  </td>
                  <td>
                    <asp:CheckBox
                      ID="isForCompiled"
                      runat="server"
                      AutoPostBack="True"
                      OnCheckedChanged="ChckedChanged"
                      Text="Compiled Report"
                    />
                    <br />
                    <br />
                    <asp:CheckBox
                      ID="CheckFifoPrice"
                      runat="server"
                      AutoPostBack="True"
                      OnCheckedChanged="ChckedChanged"
                      Text="Avg FIFO Price"
                    />
                  </td>
                  <td></td>
                </tr>
                <tr>
                  <td align="right" class="auto-style31">
                    Warehouse:<br />
                    <br />
                    Vendor:<br />
                    <br />
                    Inventory Item:
                  </td>
                  <td class="auto-style32">
                    <rad:RadComboBox
                      Skin="WebBlue"
                      ID="ddlWarehouse"
                      runat="server"
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
                      OnItemDataBound="DdlWarehouse_ItemDataBound"
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
                              <%# DataBinder.Eval(Container.DataItem, "Name")%>
                            </td>
                          </tr>
                        </table>
                      </ItemTemplate>
                    </rad:RadComboBox>
                    <br />
                    <br />
                    <rad:RadComboBox
                      Skin="WebBlue"
                      ID="ddlVendor"
                      runat="server"
                      Height="170px"
                      Width="200px"
                      DataValueField="Vendor"
                      MarkFirstMatch="true"
                      AllowCustomText="true"
                      EnableLoadOnDemand="false"
                      HighlightTemplatedItems="true"
                      DropDownWidth="200px"
                      ItemRequestTimeout="500"
                      TabIndex="5"
                      OnItemDataBound="DdlVendor_ItemDataBound"
                    >
                      <HeaderTemplate>
                        <table
                          width="200"
                          cellpadding="3"
                          cellspacing="0"
                          border="0"
                        >
                          <tr>
                            <td height="30"><b>Vendor</b></td>
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
                              "Vendor")%>
                            </td>
                          </tr>
                        </table>
                      </ItemTemplate>
                    </rad:RadComboBox>
                    <br />
                    <br />
                    <rad:RadComboBox
                      ID="ddlInventoryItem"
                      runat="server"
                      AllowCustomText="true"
                      DataValueField="id"
                      DropDownWidth="200px"
                      EnableLoadOnDemand="false"
                      Height="170px"
                      HighlightTemplatedItems="true"
                      ItemRequestTimeout="500"
                      MarkFirstMatch="true"
                      OnItemDataBound="DdlInventoryItem_ItemDataBound"
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
                              <%# DataBinder.Eval(Container.DataItem, "Name")%>
                            </td>
                          </tr>
                        </table>
                      </ItemTemplate>
                    </rad:RadComboBox>
                  </td>
                  <td style="text-align: right">Take Type :</td>
                  <td>
                    <telerik:RadComboBox
                      RenderMode="Lightweight"
                      ID="ddlTakeType"
                      runat="server"
                      DataTextField="Name"
                      CheckBoxes="true"
                      EnableCheckAllItemsCheckBox="true"
                      Width="250"
                      Skin="MetroTouch"
                    >
                    </telerik:RadComboBox>
                  </td>
                  <td>
                    <asp:Button
                      ID="btnSearch"
                      runat="server"
                      Text="Search"
                      CssClass="buttonGrey btn searchBtn"
                      OnClick="BtnSearch_Click"
                      Height="68px"
                      Width="231px"
                    ></asp:Button>
                  </td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
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
              Style="font-size: large; font-weight: 700"
              Visible="False"
            ></asp:Label>
          </td>
        </tr>
        <tr>
          <td style="text-align: center; padding-left: 1px; width: 100%">
            <div style="text-align: center">
              <rsweb:ReportViewer
                ShowToolBar="True"
                ID="rptViewer1"
                DocumentMapWidth="100%"
                runat="server"
                Width="100%"
                Height="1110px"
              >
              </rsweb:ReportViewer>
            </div>
          </td>
        </tr>
      </table>
      <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
  </body>
</html>
