<%@ Page Language="C#" Title="Inventory Current Stock" MasterPageFile="~/Views/ReportingTool.master" AutoEventWireup="true" CodeFile="CurrentStock.aspx.cs" Inherits="UI_CurrentStock" %>
<%@ Register TagPrefix="uc" TagName="header" Src="../Includes/Header.ascx" %>
<%@ Register TagPrefix="uc" TagName="footer" Src="../Includes/Footer.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="RadComboBox.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >

	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

          <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>
						<div class="bordered" style="width:100%">
							<table width="100%" cellpadding="3" cellspacing="0" border="0">
								<tr>
									<td align="right" class="auto-style4">Inventory Group Item:</td>
									<td class="auto-style5"><rad:RadComboBox Skin="WebBlue" id="ddlGroupItem" Runat="server" Height="170px" Width="200px" DataValueField="GroupCode"
											MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false" OnItemDataBound="ddlGroupItem_ItemDataBound"
											HighlightTemplatedItems="true" DropDownWidth="200px" onselectedindexchanged="ddlGroupItem_SelectedIndexChanged" AutoPostBack="true" ItemRequestTimeout="500" TabIndex="5">
											<HeaderTemplate>
												<table width="50" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td height="30"><b>Group Item</b></td>
													</tr>
												</table>
											</HeaderTemplate>
											<ItemTemplate>
												<table width="50" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td><%# DataBinder.Eval(Container.DataItem, "GroupCode")%></td>
													</tr>
												</table>
											</ItemTemplate>
										</rad:RadComboBox></td>
									<td align="right" class="auto-style6">Inventory Item:</td>
									<td class="auto-style8">
                                        <rad:RadComboBox ID="ddlInventoryItem" Runat="server" AllowCustomText="true"
                                            DataValueField="id" DropDownWidth="200px" EnableLoadOnDemand="false"
                                            Height="170px" HighlightTemplatedItems="true" ItemRequestTimeout="500"
                                            MarkFirstMatch="true" OnItemDataBound="ddlInventoryItem_ItemDataBound"
                                            Skin="WebBlue" TabIndex="5" Width="200px">
                                            <HeaderTemplate>
                                                <table border="0" cellpadding="3" cellspacing="0" >
                                                    <tr>
                                                        <td height="30">
                                                            <b>Item</b></td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table border="0" cellpadding="3" cellspacing="0" >
                                                    <tr>
                                                        <td>
                                                            <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </rad:RadComboBox>
                                    </td>
									<td class="auto-style7"></td>
								</tr>
								<tr>
									<td align="right" class="auto-style1">Warehouse:</td>
									<td class="auto-style2">
										<rad:RadComboBox Skin="WebBlue" id="ddlWarehouse" Runat="server"
                                            Height="170px" Width="200px" DataValueField="id"
											MarkFirstMatch="true" AllowCustomText="true" EnableLoadOnDemand="false"
											HighlightTemplatedItems="true" DropDownWidth="200px" ItemRequestTimeout="500" TabIndex="5"
                                            OnItemDataBound="ddlWarehouse_ItemDataBound">
											<HeaderTemplate>
												<table width="200" cellpadding="3" cellspacing="0" border="0">
													<tr>
														<td height="30"><b>Group Item</b></td>
													</tr>
												</table>
											</HeaderTemplate>
											<ItemTemplate>
												<table width="200" cellpadding="3" cellspacing="0" border="0">
													<tr>
                                                        <td>
                                                            <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                                        </td>
                                                    </tr>
												</table>
											</ItemTemplate>
										</rad:RadComboBox>
									</td>
									<td class="auto-style3" >&nbsp;</td>
									<td class="auto-style9" ><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg"
                                            OnClick="btnSearch_Click" Height="71px" Width="230px"></asp:Button>
                                    </td>
									<td></td>

								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr>
                    <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Excel.png"
                        OnClick="ImgbtnExcel_Click" AlternateText="Excel" ToolTip="Excel"/>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Word.png"
                        OnClick="ImgbtnWord_Click" AlternateText="Word" ToolTip="Word" />
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/PDF.png"
                        OnClick="ImgbtnPDF_Click" AlternateText="PDF" ToolTip="PDF" />
                     </td>
                </tr>
				<tr>
					<td style="padding-left: 1px;width:100%;">
                        <telerik:RadGrid ID="RadGrid1" AllowSorting="true" AllowFilteringByColumn="true" ShowFooter="True"
                         AutoGenerateColumns="false" runat="server" OnPdfExporting="RadGrid1_PdfExporting" >
                            <MasterTableView>
                             <Columns>
                                    <telerik:GridBoundColumn DataField="Warehouse" HeaderText="Warehouse"  />
                                    <telerik:GridBoundColumn DataField="InventoryItemName" HeaderText="Inventory Item Name"  />
                                    <telerik:GridBoundColumn DataField="Unit" HeaderText="Unit"  />
                                    <telerik:GridNumericColumn DataField="InStock" HeaderText="InStock" DataType="System.Decimal" />
                                    <telerik:GridNumericColumn DataField="Purchase" HeaderText="Purchase" DataType="System.Decimal" />
                                    <telerik:GridNumericColumn DataField="LatestCost" HeaderText="Latest Cost" DataType="System.Decimal" />
                                     <telerik:GridNumericColumn DataField="AverageCost" HeaderText="Average Cost" DataType="System.Decimal" />
                                    <telerik:GridNumericColumn DataField="InventoryConsumption" HeaderText="Inventory Consumption" DataType="System.Decimal" />
                                    <telerik:GridNumericColumn DataField="InventoryPrediction" HeaderText="Inventory Prediction" DataType="System.Decimal" />
                                    <telerik:GridNumericColumn DataField="CurrentInventory" HeaderText="Current Inventory" DataType="System.Decimal" Aggregate="Sum" FooterText="Total:" />
                                    <%--<telerik:GridBoundColumn DataField="Date" DataFormatString="{0:d}" HeaderText="Date"
                                    DataType="System.DateTime" />
                                    <telerik:GridCheckBoxColumn DataField="Discontinued" HeaderText="Discontinued" DataType="System.Boolean" />--%>
                                </Columns>
                            </MasterTableView>
                         </telerik:RadGrid>
					</td>
				</tr>
			</table>

</asp:Content>