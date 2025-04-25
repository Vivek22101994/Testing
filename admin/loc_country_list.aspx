<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="loc_country_list.aspx.cs" Inherits="RentalInRome.admin.loc_country_list" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div id="fascia1">
		<div class="pannello_fascia1">
			<div style="clear: both">
				<h1>Elenco Country</h1>
				<div class="bottom_agg">
                    <asp:LinkButton ID="lnk_nuovo" runat="server" OnClick="lnk_nuovo_Click" ><span>Salva</span></asp:LinkButton>
                </div>
			</div>
            <div style="clear: both">
                <b>Attenzione!</b>
                <br/>
                Salvare ogni pagina prima di passare alla prossima.
            </div>
			<div style="clear: both">
				<asp:ListView ID="LV" runat="server" OnPagePropertiesChanged="LV_PagePropertiesChanged">
					<ItemTemplate>
						<tr class="">
							<td>
								<span>
									<%# Eval("title")%></span>
							</td>
							<td>
								<span>
									<%# Eval("inner_notes")%></span>
							</td>
                            <td>
                                <telerik:RadNumericTextBox ID="ntxt_codeCoGe" runat="server" Width="50">
                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </td>
                             <td>
                                <telerik:RadNumericTextBox ID="ntxt_country_code" runat="server" Width="50">
                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </td>
							<td>
								<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:Label ID="lbl_codeCoGe" Visible="false" runat="server" Text='<%# Eval("codeCoGe") %>' />
                                <asp:Label ID="lbl_country_code" Visible="false" runat="server" Text='<%# Eval("country_code") %>' />
							</td>
						</tr>
					</ItemTemplate>
					<AlternatingItemTemplate>
						<tr class="alternate">
                            <td>
                                <span>
                                    <%# Eval("title")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("inner_notes")%></span>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="ntxt_codeCoGe" runat="server" Width="50">
                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </td>
                             <td>
                                <telerik:RadNumericTextBox ID="ntxt_country_code" runat="server" Width="50">
                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:Label ID="lbl_codeCoGe" Visible="false" runat="server" Text='<%# Eval("codeCoGe") %>' />
                                <asp:Label ID="lbl_country_code" Visible="false" runat="server" Text='<%# Eval("country_code") %>' />
                            </td>
						</tr>
					</AlternatingItemTemplate>
					<EmptyDataTemplate>
						<table id="Table1" runat="server" style="">
							<tr>
								<td>
									No data was returned.
								</td>
							</tr>
						</table>
					</EmptyDataTemplate>
					<LayoutTemplate>
                        <div class="page">
                            <asp:DataPager ID="DataPager2" runat="server" PageSize="30" style="border-right: medium none;" QueryStringField="pg">
                                <Fields>
                                    <asp:NumericPagerField ButtonCount="20" />
                                </Fields>
                            </asp:DataPager>
                            <div class="nulla">
                            </div>
                        </div>
						<div class="table_fascia">
							<table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
								<tr id="Tr1" runat="server" style="text-align: left">
									<th id="Th2" runat="server" style="width: 200px">
										Country
									</th>
									<th id="Th1" runat="server" style="width: 50px">
										Code
									</th>
                                    <th id="Th4" runat="server" style="width: 50px">
                                        Codice CoGe
                                    </th>
                                     <th id="Th5" runat="server" style="width: 50px">
                                        Prefisso Internazionale
                                    </th>
									<th id="Th3" runat="server">
									</th>
								</tr>
								<tr id="itemPlaceholder" runat="server">
								</tr>
							</table>
						</div>
					</LayoutTemplate>
				</asp:ListView>
			</div>
		</div>
		<div style="clear: both">
		</div>
	</div>
	<div class="nulla">
	</div>
</asp:Content>
