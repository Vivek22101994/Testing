<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="inv_digital_invoice_error.aspx.cs" Inherits="RentalInRome.admin.inv_digital_invoice_error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div id="fascia1">
		<div class="pannello_fascia1">			
			<div style="clear: both">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
				<asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand" OnItemDataBound="LV_ItemDataBound">
					<ItemTemplate>
						<tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
							<td>
								<span>
									<%# Eval("pid_invoice") %>
                                    <asp:Label ID="lbl_invoice_id" runat="server" Text='<%# Eval("pid_invoice") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
								</span>
							</td>
                            <td>
								<span>
									<asp:Label ID="lbl_invoice_code" runat="server"></asp:Label></span>
							</td>
                            <td>
								<span>
									<asp:Label ID="lbl_res_code" runat="server"></asp:Label></span>
							</td>
							<td>
								<span>
									<%# Eval("error")%></span>
							</td>
							<td>
								<span>
									<%# Eval("datetime")%></span>
							</td>	
                            <td>
								<span>
									<a id="lbl_res_area" runat="server" target="_blank">scheda</a></span>
							</td>	
                            <td>
								<asp:LinkButton ID="lnk_send_invoice" runat="server" CommandName="sendInvoice">Send Invoice</asp:LinkButton>
							</td>							
						</tr>
					</ItemTemplate>
					<AlternatingItemTemplate>
						<tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
							<td>
								<span>
									<%# Eval("pid_invoice") %>
                                    <asp:Label ID="lbl_invoice_id" runat="server" Text='<%# Eval("pid_invoice") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
								</span>
							</td>
                            <td>
								<span>
									<asp:Label ID="lbl_invoice_code" runat="server"></asp:Label></span>
							</td>
                            <td>
								<span>
									<asp:Label ID="lbl_res_code" runat="server"></asp:Label></span>
							</td>
							<td>
								<span>
									<%# Eval("error")%></span>
							</td>
							<td>
								<span>
									<%# Eval("datetime")%></span>
							</td>
                            <td><span>
								<a id="lbl_res_area" runat="server" target="_blank">scheda</a>
									</span>
							</td>	
                             <td>
								<asp:LinkButton ID="lnk_send_invoice" runat="server" CommandName="sendInvoice">Send Invoice</asp:LinkButton>
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
						<div class="table_fascia">
							<table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
								<tr id="Tr1" runat="server" style="text-align: left">
									<th id="Th2" runat="server" style="width: 30px">
										Invoice ID
									</th>
                                    <th id="Th5" runat="server" style="width: 30px">
										Invoice Code
									</th>
                                    <th id="Th7" runat="server" style="width: 30px">
										Res Code
									</th>
									<th id="Th1" runat="server" style="width: 500px">
										Error
									</th>
									<th id="Th4" runat="server" style="width: 150px">
										Date Time
									</th>
                                    <th id="Th6" runat="server" style="width: 150px">
										Res. Area
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
                        </ContentTemplate>
                </asp:UpdatePanel>
			</div>
		</div>
		<div style="clear: both">
		</div>
	</div>
	<div class="nulla">
	</div>
</asp:Content>
