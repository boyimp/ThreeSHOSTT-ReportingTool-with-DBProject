<%@ Page Language="C#" AutoEventWireup="true"
CodeFile="ItemSalesProfitAnalysis.aspx.cs" Inherits="UI_ItemSalesProfitAnalysis"
%> <%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"
TagPrefix="act" %> <%@ Register Assembly="Telerik.Web.UI"
Namespace="Telerik.Web.UI" TagPrefix="telerik" %> <%@ Register
Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head1" runat="server">
    <title>Items Sales Profit Analysis</title>
    <!-- CSS only -->
    <link
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css"
      rel="stylesheet"
      integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT"
      crossorigin="anonymous"
    />
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
      .style4 {
        width: 244px;
      }
      .style3 {
        width: 248px;
      }
      .style6 {
        width: 174px;
      }
      .style7 {
        width: 73px;
      }
      .auto-style1 {
        width: 68px;
      }
      .auto-style2 {
        width: 147px;
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
    <form id="form1" runat="server" class="filterContainer">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
      <uc:header ID="header1" runat="server"></uc:header>
      <%--
      <asp:UpdatePanel
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
                    cellpadding="5"
                    cellspacing="0"
                    border="0"
                  >
                    <tr>
                      <td class="style7">From Date:</td>
                      <td>
                        <telerik:RadDatePicker ID="dtpFromDate" runat="server">
                        </telerik:RadDatePicker>
                      </td>
                      <td class="auto-style1">To Date:</td>
                      <td class="auto-style2">
                        <telerik:RadDatePicker ID="dtpToDate" runat="server">
                        </telerik:RadDatePicker>
                      </td>
                      <td height="30">Department</td>
                      <td>
                        <rad:RadComboBox
                          ID="ddlTicketType"
                          runat="server"
                          AllowCustomText="true"
                          DataValueField="id"
                          DropDownWidth="200px"
                          EnableLoadOnDemand="false"
                          Height="170px"
                          HighlightTemplatedItems="true"
                          ItemRequestTimeout="500"
                          MarkFirstMatch="true"
                          OnItemDataBound="ddlTicketType_ItemDataBound"
                          Skin="WebBlue"
                          TabIndex="5"
                          Width="200px"
                        >
                          <HeaderTemplate>
                            <table
                              border="0"
                              cellpadding="3"
                              cellspacing="0"
                              width="200"
                            >
                              <tr>
                                <td height="30">
                                  <b>Department</b>
                                </td>
                              </tr>
                            </table>
                          </HeaderTemplate>
                          <ItemTemplate>
                            <table
                              border="0"
                              cellpadding="3"
                              cellspacing="0"
                              width="200"
                            >
                              <tr>
                                <td>
                                  <%# DataBinder.Eval(Container.DataItem,
                                  "Name") %>
                                </td>
                              </tr>
                            </table>
                          </ItemTemplate>
                        </rad:RadComboBox>
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
                      <td>&nbsp;</td>
                    </tr>
                    <tr>
                      <td class="style7">Group :</td>
                      <td class="style4">
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
                      <td class="auto-style1">&nbsp;</td>
                      <td class="auto-style2">&nbsp;</td>
                      <td height="30">Menu Item:</td>
                      <td class="style3">
                        <rad:RadComboBox
                          ID="ddlMenuItem"
                          runat="server"
                          AllowCustomText="true"
                          DataValueField="id"
                          DropDownWidth="200px"
                          EnableLoadOnDemand="false"
                          Height="170px"
                          HighlightTemplatedItems="true"
                          ItemRequestTimeout="500"
                          MarkFirstMatch="true"
                          OnItemDataBound="ddlMenuItem_ItemDataBound"
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
                      <td>
                        <asp:Button
                          ID="btnSearch"
                          runat="server"
                          Text="Search"
                          CssClass="buttonGrey btn searchBtn"
                          OnClick="btnSearch_Click"
                          Width="173px"
                          Height="72px"
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
                <asp:Label
                  ID="lblWorkPeriod"
                  runat="server"
                  Text="Label"
                  Style="font-size: medium;
                    font-weight: 700"
                ></asp:Label>
                <asp:Button
                  ID="btnSearch0"
                  runat="server"
                  Text="Re-Process"
                  CssClass="buttonGrey"
                  OnClick="btnReprocess_Click"
                  Width="173px"
                ></asp:Button>
              </td>
            </tr>
            <tr>
              <td style="padding-left: 5px">
                <telerik:RadGrid
                  ID="RadGrid1"
                  runat="server"
                  AllowFilteringByColumn="true"
                  CellSpacing="0"
                  GridLines="None"
                  OnNeedDataSource="RadGrid1_NeedDataSource1"
                  ShowFooter="True"
                  OnColumnCreated="RadGrid1_ColumnCreated"
                  OnItemCommand="RadGrid1_ItemCommand"
                  OnPdfExporting="RadGrid1_PdfExporting"
                >
                  <MasterTableView
                    AutoGenerateColumns="true"
                    DataKeyNames="ItemId, PortionName"
                    ClientDataKeyNames="ItemId, PortionName"
                    Height="100%"
                  >
                    <Columns>
                      <telerik:GridButtonColumn
                        ButtonType="ImageButton"
                        CommandName="Preview"
                        ImageUrl="~/Images/view.png"
                        ItemStyle-HorizontalAlign="Center"
                        UniqueName="More"
                      >
                        <HeaderStyle Width="40px" />
                      </telerik:GridButtonColumn>
                    </Columns>
                  </MasterTableView>
                  <ExportSettings>
                    <Pdf BorderType="AllBorders"> </Pdf>
                    <Pdf BorderStyle="Medium"> </Pdf>
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
