<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CurrentStock.aspx.cs"
Inherits="UI_CurrentStock" %> <%@ Register TagPrefix="uc" TagName="header"
Src="../Includes/Header.ascx" %> <%@ Register TagPrefix="uc" TagName="footer"
Src="../Includes/Footer.ascx" %> <%@ Register Assembly="AjaxControlToolkit"
Namespace="AjaxControlToolkit" TagPrefix="act" %> <%@ Register
assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %> <%@
Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls"
TagPrefix="rad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head1" runat="server">
    <title>Inventory Current Stock</title>
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
        width: 149px;
      }
      .auto-style2 {
        width: 263px;
      }
      .auto-style3 {
        width: 97px;
      }
      .auto-style6 {
        width: 97px;
        height: 65px;
      }
      .auto-style9 {
        width: 373px;
      }
      .auto-style10 {
        width: 149px;
        height: 60px;
      }
      .auto-style11 {
        width: 263px;
        height: 60px;
      }
      .auto-style12 {
        width: 97px;
        height: 60px;
        text-align: right;
      }
      .auto-style13 {
        height: 60px;
        width: 373px;
      }
      .auto-style14 {
        height: 60px;
      }
      .auto-style19 {
        width: 149px;
        height: 33px;
      }
      .auto-style20 {
        width: 263px;
        height: 33px;
      }
      .auto-style21 {
        width: 97px;
        height: 33px;
      }
      .auto-style22 {
        height: 33px;
        width: 373px;
      }
      .auto-style23 {
        height: 33px;
      }
      .auto-style24 {
        width: 149px;
        height: 65px;
      }
      .auto-style25 {
        width: 263px;
        height: 65px;
      }
      .auto-style26 {
        height: 65px;
        width: 373px;
      }
      .auto-style27 {
        height: 65px;
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

          <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
              <td>
                <div class="bordered" style="width: 100%">
                  <table
                    width="100%"
                    cellpadding="3"
                    cellspacing="0"
                    border="0"
                    class="filterContainer"
                  >
                    <tr>
                      <td align="right" class="auto-style19"></td>
                      <td class="auto-style20"></td>
                      <td align="right" class="auto-style21"></td>
                      <td class="auto-style22"></td>
                      <td class="auto-style23"></td>
                    </tr>
                    <tr>
                      <td align="right" class="auto-style24">
                        Inventory Group Item:
                      </td>
                      <td class="auto-style25">
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
                      <td align="right" class="auto-style6">Inventory Item:</td>
                      <td class="auto-style26">
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
                      <td class="auto-style27"></td>
                    </tr>
                    <tr>
                      <td align="right" class="auto-style10">Vendor:</td>
                      <td class="auto-style11">
                        <rad:RadComboBox
                          Skin="WebBlue"
                          id="ddlVendor"
                          Runat="server"
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
                          OnItemDataBound="ddlVendor_ItemDataBound"
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
                      </td>
                      <td class="auto-style12">Brand:</td>
                      <td class="auto-style13">
                        <rad:RadComboBox
                          ID="ddlBrand"
                          Runat="server"
                          AllowCustomText="true"
                          DataValueField="Brand"
                          DropDownWidth="200px"
                          EnableLoadOnDemand="false"
                          Height="170px"
                          HighlightTemplatedItems="true"
                          ItemRequestTimeout="500"
                          MarkFirstMatch="true"
                          OnItemDataBound="ddlBrand_ItemDataBound"
                          Skin="WebBlue"
                          TabIndex="5"
                          Width="200px"
                        >
                          <HeaderTemplate>
                            <table border="0" cellpadding="3" cellspacing="0">
                              <tr>
                                <td height="30">
                                  <b>Brand</b>
                                </td>
                              </tr>
                            </table>
                          </HeaderTemplate>
                          <ItemTemplate>
                            <table border="0" cellpadding="3" cellspacing="0">
                              <tr>
                                <td>
                                  <%# DataBinder.Eval(Container.DataItem,
                                  "Brand")%>
                                </td>
                              </tr>
                            </table>
                          </ItemTemplate>
                        </rad:RadComboBox>
                      </td>
                      <td class="auto-style14"></td>
                    </tr>
                    <tr>
                      <td align="right" class="auto-style1">Warehouse:</td>
                      <td class="auto-style2">
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
                      <td class="auto-style3">&nbsp;</td>
                      <td class="auto-style9">
                        <asp:Button
                          ID="btnSearch"
                          runat="server"
                          Text="Search"
                          CssClass="buttonGrey btn searchBtn"
                          OnClick="btnSearch_Click"
                          Height="71px"
                          Width="230px"
                        ></asp:Button>
                      </td>
                      <td>&nbsp;</td>
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
            <tr>
              <td style="padding-left: 1px; width: 100%">
                <telerik:RadGrid
                  ID="RadGrid1"
                  AllowSorting="True"
                  AllowFilteringByColumn="True"
                  ShowFooter="True"
                  AutoGenerateColumns="False"
                  runat="server"
                  OnPdfExporting="RadGrid1_PdfExporting"
                >
                  <GroupingSettings></GroupingSettings>

                  <MasterTableView>
                    <Columns>
                      <telerik:GridBoundColumn
                        DataField="Warehouse"
                        HeaderText="Warehouse"
                      />
                      <telerik:GridBoundColumn
                        DataField="Barcode"
                        FilterControlAltText="Filter Barcode column"
                        HeaderText="Barcode"
                        UniqueName="Barcode"
                      >
                      </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn
                        DataField="InventoryItemName"
                        HeaderText="Name"
                      />
                      <telerik:GridBoundColumn
                        DataField="Unit"
                        HeaderText="Unit"
                      />
                      <telerik:GridNumericColumn
                        DataField="InStock"
                        HeaderText="Opening"
                        DataType="System.Decimal"
                      />
                      <telerik:GridNumericColumn
                        DataField="Purchase"
                        HeaderText="Purchase"
                        DataType="System.Decimal"
                      />
                      <telerik:GridNumericColumn
                        DataField="LatestCost"
                        HeaderText="Latest Cost"
                        DataType="System.Decimal"
                      />
                      <telerik:GridNumericColumn
                        DataField="AverageCost"
                        HeaderText="Average Cost"
                        DataType="System.Decimal"
                      />
                      <telerik:GridNumericColumn
                        DataField="InventoryConsumption"
                        HeaderText="Inventory Consumption"
                        DataType="System.Decimal"
                      />
                      <telerik:GridNumericColumn
                        DataField="InventoryPrediction"
                        HeaderText="Inventory Prediction"
                        DataType="System.Decimal"
                      />
                      <telerik:GridNumericColumn
                        DataField="CurrentInventory"
                        HeaderText="Current Inventory"
                        DataType="System.Decimal"
                        Aggregate="Sum"
                        FooterText="Total:"
                      />
                      <%--<telerik:GridBoundColumn
                        DataField="Date"
                        DataFormatString="{0:d}"
                        HeaderText="Date"
                        DataType="System.DateTime"
                      />
                      <telerik:GridCheckBoxColumn
                        DataField="Discontinued"
                        HeaderText="Discontinued"
                        DataType="System.Boolean"
                      />--%>
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
