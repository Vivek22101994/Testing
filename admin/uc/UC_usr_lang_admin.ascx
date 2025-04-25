<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_usr_lang_admin.ascx.cs" Inherits="RentalInRome.admin.uc.UC_usr_lang_admin" %>
<asp:HiddenField runat="server" ID="HF_id" />
<span class="titoloboxmodulo"><asp:Literal ID="ltr_title" runat="server"></asp:Literal></span>
<div id="pnl_view" runat="server" class="boxmodulo">
</div>
<div id="pnl_modify" runat="server" class="boxmodulo">
	<asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand">
		<ItemTemplate>
			<tr>
				<td>
					<span><%# AdminUtilities.usr_adminName((int)Eval("pid_admin")) %></span>
				</td>
				<td>
					<span>
						<%# AdminUtilities.usr_adminEmail((int)Eval("pid_admin")) %></span>
				</td>
				<td>
					<asp:ImageButton ID="ibtn_up" runat="server" ToolTip="Su" AlternateText="Su" CommandName="move_up" ImageUrl="~/images/ico/Go_up.png" />
				</td>
				<td>
					<asp:ImageButton ID="ibtn_down" runat="server" ToolTip="Giù" AlternateText="Giù" CommandName="move_down" ImageUrl="~/images/ico/Go_down.png" />
				</td>
				<td>
					<asp:Label ID="lbl_id" runat="server" Text='<%# Eval("pid_admin") %>' Visible="false"></asp:Label>
					&nbsp;&nbsp;&nbsp;
					<asp:LinkButton ID="lnk_edit" CommandName="edit_block" ToolTip="Modifica" runat="server">modifica</asp:LinkButton>
					&nbsp;&nbsp;&nbsp;
					<asp:ImageButton ID="ibtn_annulla" runat="server" CommandArgument='<%# Eval("pid_admin") %>' CommandName="delete" OnClientClick="return confirm('Vuoi eliminare Blocco dalla Collezione?')"
						Height="11px" ImageUrl="~/images/ico/ico_del.gif" ToolTip="Elimina" />
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="alternate">
				<td>
					<asp:Label ID="pid_appartmentLabel" runat="server" Text='<%# CommonUtilities.GetStaticBlockName("" + Eval("pid_block")) %>'></asp:Label>
				</td>
				<td>
					<asp:ImageButton ID="ibtn_up" runat="server" ToolTip="Su" AlternateText="Su" CommandName="move_up" ImageUrl="~/images/ico/Go_up.png" />
				</td>
				<td>
					<asp:ImageButton ID="ibtn_down" runat="server" ToolTip="Giù" AlternateText="Giù" CommandName="move_down" ImageUrl="~/images/ico/Go_down.png" />
				</td>
				<td>
					<asp:Label ID="lbl_id" runat="server" Text='<%# Eval("pid_block") %>' Visible="false"></asp:Label>
					&nbsp;&nbsp;&nbsp;
					<asp:LinkButton ID="lnk_edit" CommandName="edit_block" ToolTip="Modifica" runat="server">modifica</asp:LinkButton>
					&nbsp;&nbsp;&nbsp;
					<asp:ImageButton ID="ibtn_annulla" runat="server" CommandArgument='<%# Eval("pid_block") %>' CommandName="delete" OnClientClick="return confirm('Vuoi eliminare Blocco dalla Collezione?')"
						Height="11px" ImageUrl="~/images/ico/ico_del.gif" ToolTip="Elimina" />
				</td>
			</tr>
		</AlternatingItemTemplate>
		<EmptyDataTemplate>
			<table id="Table1" runat="server" style="">
				<tr>
					<td>
						Nessun Blocco
						<br />
						<asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click">nuovo</asp:LinkButton>
					</td>
				</tr>
			</table>
		</EmptyDataTemplate>
		<LayoutTemplate>
			<div class="table_fascia">
				<asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click">nuovo</asp:LinkButton>
				<br />
				<table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
					<tr id="Tr1" runat="server" style="text-align: left">
						<th id="Th1" runat="server" style="width: 150px;">
							Blocco
						</th>
						<th id="Th2" runat="server">
						</th>
						<th id="Th4" runat="server">
						</th>
						<th id="Th5" runat="server">
						</th>
					</tr>
					<tr id="itemPlaceholder" runat="server">
					</tr>
				</table>
			</div>
		</LayoutTemplate>
	</asp:ListView>
</div>
<div class="salvataggio">
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_modify" runat="server" OnClick="lnk_modify_Click"><span>Modifica</span></asp:LinkButton>
	</div>
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" ValidationGroup="invoice"><span>Salva Modifiche</span></asp:LinkButton>
	</div>
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
	</div>
	<div class="nulla">
	</div>
</div>
<asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="sequence" TableName="CONT_RL_COLLECTION_BLOCKs" Where="pid_collection == @pid_collection">
	<WhereParameters>
		<asp:ControlParameter ControlID="HF_id" Name="pid_collection" PropertyName="Value" Type="Int32" />
	</WhereParameters>
</asp:LinqDataSource>
