<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_operator_list.aspx.cs" Inherits="RentalInRome.admin.usr_operator_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:HiddenField ID="HF_id" Value="0" runat="server" />
			<asp:HiddenField ID="HF_lang" Value="1" runat="server" />
			<asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_ADMIN" OrderBy="name" Where="(id > 2) && is_deleted!=1">
			</asp:LinqDataSource>
			<div id="fascia1">
				<div class="pannello_fascia1">
					<div style="clear: both">
						<h1>Elenco Account</h1>
						<div class="bottom_agg">
						</div>
					</div>
					<div style="clear: both">
						<asp:ListView ID="LV" runat="server" DataSourceID="LDS">
							<ItemTemplate>
								<tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
									<td>
										<span><%# Eval("name") %> <%# Eval("surname") %></span>
									</td>
									<td>
										<span>
											<%# Eval("email") %></span>
									</td>
									<td>
										<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
										<a href="usr_operator_details.aspx?id=<%# Eval("id") %>">modifica</a>
									</td>
								</tr>
							</ItemTemplate>
							<AlternatingItemTemplate>
								<tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
									<td>
										<span>
											<%# Eval("name") %> <%# Eval("surname") %></span>
									</td>
									<td>
										<span>
											<%# Eval("email") %></span>
									</td>
									<td>
										<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
										<a href="usr_operator_details.aspx?id=<%# Eval("id") %>">modifica</a>
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
									<table border="0" cellpadding="0" cellspacing="0" style="">
										<tr style="text-align: left">
											<th style="width: 150px">
												Nome Cognome
											</th>
											<th style="width: 200px">
												Email
											</th>
											<th id="Th3" runat="server">
											</th>
										</tr>
										<tr id="itemPlaceholder" runat="server" />
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
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
