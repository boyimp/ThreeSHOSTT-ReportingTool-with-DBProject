<%@ Page Language="C#" AutoEventWireup="true"
CodeFile="InvntoryRegister.aspx.cs" Inherits="UI_WorkPeriodWiseInvntoryRegister"
%> <%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"
TagPrefix="act" %> <%@ Register Assembly="Telerik.Web.UI"
Namespace="Telerik.Web.UI" TagPrefix="telerik" %> <%@ Register
Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head1" runat="server">
    <title>Inventory Register</title>
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
        height: 77px;
      }
      .auto-style2 {
        height: 73px;
      }
      .auto-style3 {
        text-align: right;
        width: 136px;
        height: 47px;
      }
      .auto-style4 {
        height: 73px;
        width: 136px;
      }
      .auto-style5 {
        height: 77px;
        width: 136px;
      }
      .auto-style6 {
        width: 244px;
        height: 47px;
      }
      .auto-style7 {
        height: 73px;
        width: 244px;
      }
      .auto-style8 {
        height: 77px;
        width: 244px;
      }
      .auto-style9 {
        height: 47px;
      }
      .auto-style10 {
        height: 47px;
        width: 99px;
      }
      .auto-style11 {
        height: 73px;
        width: 99px;
      }
      .auto-style12 {
        height: 77px;
        width: 99px;
      }
      .auto-style13 {
        height: 47px;
        width: 312px;
      }
      .auto-style14 {
        height: 73px;
        width: 312px;
      }
      .auto-style15 {
        height: 77px;
        width: 312px;
      }
      .auto-style16 {
        width: 100%;
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
      <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
      <uc:header ID="header1" runat="server"></uc:header>
      <%--<asp:UpdatePanel
        ID="UpdatePanel1"
        runat="server"
        UpdateMode="Conditional"
      >
        <ContentTemplate
          >--%>
          <table
            cellpadding="0"
            cellspacing="0"
            border="0"
            class="auto-style16"
          >
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
                      <td class="auto-style3">From Date:</td>
                      <td class="auto-style6">
                        <telerik:RadDatePicker ID="dtpFromDate" runat="server">
                        </telerik:RadDatePicker>
                      </td>
                      <td align="right" class="auto-style10">To Date:</td>
                      <td class="auto-style13">
                        <telerik:RadDatePicker ID="dtpToDate" runat="server">
                        </telerik:RadDatePicker>
                      </td>
                      <td class="auto-style9">&nbsp;</td>
                    </tr>
                    <tr>
                      <td align="right" class="auto-style4">
                        Inventory Group Item:
                      </td>
                      <td class="auto-style7">
                        <rad:RadComboBox
                          Skin="WebBlue"
                          ID="ddlGroupItem"
                          runat="server"
                          Height="170px"
                          Width="200px"
                          DataValueField="GroupCode"
                          MarkFirstMatch="true"
                          AllowCustomText="true"
                          EnableLoadOnDemand="false"
                          OnItemDataBound="ddlGroupItem_ItemDataBound"
                          HighlightTemplatedItems="true"
                          DropDownWidth="200px"
                          OnSelectedIndexChanged="ddlGroupItem_SelectedIndexChanged"
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
                                <td height="30">
                                  <b>Group Item</b>
                                </td>
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
                      <td align="right" class="auto-style11">
                        Inventory Item:
                      </td>
                      <td class="auto-style14">
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
                      <td class="auto-style2"></td>
                    </tr>
                    <tr>
                      <td align="right" class="auto-style5">Warehouse:</td>
                      <td class="auto-style8">
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
                                <td height="30">
                                  <b>Group Item</b>
                                </td>
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
                      <td class="auto-style12">&nbsp;</td>
                      <td class="auto-style15">
                        <asp:Button
                          ID="btnSearch"
                          runat="server"
                          Text="Search"
                          CssClass="buttonGrey btn searchBtn"
                          OnClick="btnSearch_Click"
                          Height="65px"
                          Width="232px"
                        ></asp:Button>
                      </td>
                      <td class="auto-style1"></td>
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
                <asp:Label
                  ID="lblWorkPeriod"
                  runat="server"
                  Style="font-size: large; font-weight: 700"
                ></asp:Label>
              </td>
            </tr>
            <tr>
              <td style="padding-left: 1px; width: 100%">
                <telerik:RadGrid
                  ID="RadGrid1"
                  AllowSorting="true"
                  FilterItemStyle-Width="150px"
                  AllowFilteringByColumn="true"
                  ShowFooter="True"
                  AutoGenerateColumns="false"
                  runat="server"
                  OnNeedDataSource="RadGrid1_NeedDataSource1"
                  OnItemCommand="RadGrid1_ItemCommand"
                  OnColumnCreated="RadGrid1_ColumnCreated"
                  OnPdfExporting="RadGrid1_PdfExporting"
                >
                  <MasterTableView
                    AutoGenerateColumns="true"
                    DataKeyNames="InventoryID,WarehouseID,StartDate,EndDate,GroupCode,Inventory,Warehouse"
                    ClientDataKeyNames="InventoryID,WarehouseID,StartDate,EndDate,GroupCode,Inventory,Warehouse"
                  >
                    <Columns>
                      <telerik:GridButtonColumn
                        HeaderText="More"
                        ButtonType="ImageButton"
                        CommandName="Preview"
                        ImageUrl="~/Images/view.png"
                        ItemStyle-HorizontalAlign="Center"
                        UniqueName="More"
                      >
                        <HeaderStyle Width="35px" Wrap="true" />
                      </telerik:GridButtonColumn>
                    </Columns>
                  </MasterTableView>
                  <ExportSettings>
                    <Pdf BorderType="AllBorders"></Pdf>
                    <Pdf BorderStyle="Medium"></Pdf>
                  </ExportSettings>
                </telerik:RadGrid>
              </td>
            </tr>
          </table>
          <%--</ContentTemplate
        > </asp:UpdatePanel
      >--%>
      <uc:footer ID="footer1" runat="server"></uc:footer>
    </form>
  </body>
</html>
