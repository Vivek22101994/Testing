<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_cont_block_edit.ascx.cs" Inherits="RentalInRome.admin.uc.UC_cont_block_edit" %>

<asp:HiddenField ID="HF_id" Value="0" runat="server" />
<asp:HiddenField ID="HF_id_collection" Value="0" runat="server" />
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_lang" Value="1" runat="server" />
<asp:HiddenField ID="HF_show_delay" Value="0" runat="server" />
<asp:HiddenField ID="HF_show_sub_title" Value="0" runat="server" />
<asp:HiddenField ID="HF_show_img" Value="0" runat="server" />
<asp:HiddenField ID="HF_show_summary" Value="0" runat="server" />
<asp:HiddenField ID="HF_show_desc" Value="1" runat="server" />
<asp:HiddenField ID="HF_editor_width" Value="500" runat="server" />
<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
	<tr>
		<td class="td_title">
			Nome:
		</td>
		<td>
			<asp:TextBox runat="server" ID="txt_name" Width="230px" />
		</td>
	</tr>
	<tr>
		<td class="td_title">
			Attivo:
		</td>
		<td>
			<asp:CheckBox ID="chk_is_active" runat="server" />
		</td>
	</tr>
	<asp:PlaceHolder ID="PH_show_delay" runat="server">
		<tr>
			<td class="td_title">
				Ritardo:
			</td>
			<td>
				<asp:DropDownList ID="drp_show_delay" runat="server">
					<asp:ListItem Text="-senza-" Value="0"></asp:ListItem>
					<asp:ListItem Text="1 secondo" Value="1"></asp:ListItem>
					<asp:ListItem Text="2 secondi" Value="2"></asp:ListItem>
					<asp:ListItem Text="3 secondi" Value="3"></asp:ListItem>
					<asp:ListItem Text="4 secondi" Value="4"></asp:ListItem>
					<asp:ListItem Text="5 secondi" Value="5"></asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="PH_show_img" runat="server" Visible="false">
		<tr>
			<td class="td_title">
				Immagine:
			</td>
			<td>
				<asp:TextBox runat="server" ID="txt_img" Width="230px" />
			</td>
		</tr>
	</asp:PlaceHolder>
	<tr>
		<td colspan="2">
			<asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
				<WhereParameters>
					<asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
				</WhereParameters>
			</asp:LinqDataSource>
			<asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound">
				<ItemTemplate>
					<asp:LinkButton ID="lnk_lang" CommandName="change_lang" OnClientClick="removeTinyEditor()" CssClass="tab_item" runat="server">
						<span>
							<%# Eval("title") %></span>
						<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
					</asp:LinkButton>
				</ItemTemplate>
				<EmptyDataTemplate>
					<span>No data was returned.</span>
				</EmptyDataTemplate>
				<LayoutTemplate>
					<div class="menu2">
						<a id="itemPlaceholder" runat="server" />
					</div>
				</LayoutTemplate>
			</asp:ListView>
		</td>
	</tr>
	<tr>
		<td class="td_title">
			Titolo:
		</td>
		<td>
			<asp:TextBox runat="server" ID="txt_title" Width="230px" />
		</td>
	</tr>
	<asp:PlaceHolder ID="PH_show_sub_title" runat="server" Visible="false">
		<tr>
			<td class="td_title">
				Sottotitolo:
			</td>
			<td>
				<asp:TextBox runat="server" ID="txt_sub_title" Width="230px" />
			</td>
		</tr>
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="PH_show_summary" runat="server" Visible="false">
		<tr>
			<td class="td_title">
				Sommario:
			</td>
			<td>
				<asp:TextBox runat="server" ID="txt_summary" Width="300px" TextMode="MultiLine" Height="120px" />
			</td>
		</tr>
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="PH_show_desc" runat="server">
		<tr>
			<td colspan="2">
				Descrizione:
				<br />
				<asp:TextBox runat="server" ID="txt_description" TextMode="MultiLine" Height="250px" />
			</td>
		</tr>
	</asp:PlaceHolder>
	<tr>
		<td colspan="2">
			<div class="salvataggio">
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_modify" runat="server" OnClick="lnk_modify_Click"><span>Modifica</span></asp:LinkButton>
				</div>
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click"><span>Salva Modifiche</span></asp:LinkButton>
				</div>
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
				</div>
				<div class="nulla">
				</div>
			</div>
		</td>
	</tr>
</table>
<asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
	<tr>
		<td class="td_title">
			Imagine thumb:
		</td>
		<td>
			<asp:FileUpload runat="server" ID="FU_thumb" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FU_thumb" ErrorMessage="*" ValidationGroup="foto2"></asp:RequiredFieldValidator>
			&nbsp;&nbsp;<asp:LinkButton ValidationGroup="foto2" runat="server" Text="carica" ID="lnk_carica_thumb" OnClick="lnk_carica_thumb_Click"></asp:LinkButton><br />
			<asp:LinkButton ID="lnk_delete_thumb" runat="server" CssClass="bott_elimina_img" OnClick="lnk_delete_thumb_Click">elimina</asp:LinkButton>
			<br />
			<asp:Image runat="server" ID="img_thumb" />
			<asp:HiddenField ID="HF_img_thumb" runat="server" />
		</td>
	</tr>
</asp:PlaceHolder>
<div class="nulla">
</div>
