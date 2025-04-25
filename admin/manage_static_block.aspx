<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="manage_static_block.aspx.cs" Inherits="RentalInRome.admin.manage_static_block" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


	<script type="text/javascript">
		var _editors = ['<%=txt_description.ClientID %>'];
		function removeTinyEditor() {
			removeTinyEditors(_editors);
		}
		function setTinyEditor() {
			setTinyEditors(_editors, false);
		}
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
	<ContentTemplate>
	<asp:HiddenField ID="HF_id" Value="0" runat="server" />
	<asp:HiddenField ID="HF_lang" Value="1" runat="server" />
	<asp:LinqDataSource ID="LDS_page_blocks" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_VIEW_TBL_BLOCKs" OrderBy="block_name" 
			Where="pid_lang == @pid_lang">
		<WhereParameters>
			<asp:Parameter DefaultValue="1" Name="pid_lang" Type="Int32" />
		</WhereParameters>
	</asp:LinqDataSource>
	<div id="fascia1">
		<div class="pannello_fascia1">
			<div style="clear: both">
				<h1>Lista blocchi </h1>
				<div class="bottom_agg">
					<asp:LinkButton ID="lnk_nuovo" runat="server" OnClick="lnk_nuovo_Click" OnClientClick="removeTinyEditor();"><span>+ Nuovo</span></asp:LinkButton>
				</div>
			</div>
			<div style="clear: both">
				<asp:ListView ID="LV_blocks" runat="server" DataSourceID="LDS_page_blocks" OnSelectedIndexChanging="LV_blocks_SelectedIndexChanging">
					<ItemTemplate>
						<tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
							<td>
								<asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
							</td>
							<td>
								<asp:Label ID="Label2" runat="server" Text='<%# Eval("block_name") %>' />
							</td>
							<td>
								<asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
							</td>
							<td>
								<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
								<asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="removeTinyEditor();" />
								<asp:ImageButton ID="ibtn_delete" AlternateText="sch." runat="server" Height="9px" CommandArgument='<%# Eval("id") %>' ImageUrl="~/images/ico/ico_del.gif" ToolTip="Scheda" Style="text-decoration: none;
									border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="return confirm('Sta per eliminare Record?');removeTinyEditor();" OnClick="DeleteRecord" />
							</td>
						</tr>
					</ItemTemplate>
					<AlternatingItemTemplate>
						<tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
							<td>
								<asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
							</td>
							<td>
								<asp:Label ID="Label2" runat="server" Text='<%# Eval("block_name") %>' />
							</td>
							<td>
								<asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
							</td>
							<td>
								<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
								<asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="removeTinyEditor();" />
								<asp:ImageButton ID="ibtn_delete" AlternateText="sch." runat="server" Height="9px" CommandArgument='<%# Eval("id") %>' ImageUrl="~/images/ico/ico_del.gif" ToolTip="Scheda" Style="text-decoration: none;
									border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="return confirm('Sta per eliminare Record?');removeTinyEditor();" OnClick="DeleteRecord" />
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
									<th id="Th2" runat="server" style="width: 40px">
										ID
									</th>
									<th id="Th1" runat="server" style="width: 200px">
										Nome Blocco
									</th>
									<th id="Th5" runat="server" style="width: 300px">
										Titolo
									</th>
									<th id="Th3" runat="server">
									</th>
								</tr>
								<tr id="itemPlaceholder" runat="server">
								</tr>
							</table>
						</div>
						<div class="page">
							<asp:DataPager ID="DataPager1" runat="server" style="border-right: medium none;">
								<Fields>
									<asp:NumericPagerField />
								</Fields>
							</asp:DataPager>
						</div>
					</LayoutTemplate>
					<EditItemTemplate>
					</EditItemTemplate>
					<SelectedItemTemplate>
						<tr class="current">
							<td>
								<asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
							</td>
							<td>
								<asp:Label ID="Label2" runat="server" Text='<%# Eval("block_name") %>' />
							</td>
							<td>
								<asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
							</td>
							<td>
							</td>
						</tr>
					</SelectedItemTemplate>
				</asp:ListView>
			</div>
		</div>
		<div style="clear: both">
		</div>
	</div>
	<div class="nulla">
	</div>
	<asp:Panel ID="pnlContent" runat="server" Width="100%" Visible="false">
		<h1 class="titolo_main">Scheda </h1>
		<!-- INIZIO MAIN LINE -->
		<div class="mainline">
			<!-- BOX 1 -->
			<div class="mainbox">
				<div class="top">
					<div style="float: left;">
						<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
					<div style="float: right;">
						<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
				</div>
				<div class="center">
					<div class="boxmodulo">
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
								<td colspan="2">
									<asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
										<WhereParameters>
											<asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
										</WhereParameters>
									</asp:LinqDataSource>
									<asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound" OnClientClick="removeTinyEditor();">
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
							<tr>
								<td colspan="2">
									Descrizione:
									<br />
									<asp:TextBox runat="server" ID="txt_description" Width="500px" TextMode="MultiLine" Height="250px" />
								</td>
							</tr>
						</table>
					</div>
				</div>
				<div class="bottom">
					<div style="float: left;">
						<img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
					<div style="float: right;">
						<img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
				</div>
			</div>
		</div>
		<div class="nulla">
		</div>
		<div class="salvataggio">
			<div class="bottom_salva">
				<asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
			</div>
			<div class="bottom_salva">
				<asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
			</div>
			<div class="nulla">
			</div>
		</div>
	</asp:Panel>
	<asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
		<tr>
			<td class="td_title">
				Sottotitolo:
			</td>
			<td>
				<asp:TextBox runat="server" ID="txt_sub_title" Width="230px" />
			</td>
		</tr>
		<tr>
			<td class="td_title">
				Sommario:
			</td>
			<td>
				<asp:TextBox runat="server" ID="txt_summary" Width="300px" TextMode="MultiLine" Height="120px" />
			</td>
		</tr>
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
	</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
