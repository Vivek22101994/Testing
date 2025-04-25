<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="manage_static_collection.aspx.cs" Inherits="RentalInRome.admin.manage_static_collection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		function removeTinyEditor() {
		}
		function setTinyEditor() {
		}
		setTinyEditor();
		function UpdateExternalPage(url) {
			var xmlhttp = GetXmlHttpObject();
			if (xmlhttp == null) {
				alert("Your browser does not support XMLHTTP!");
				return "ok";
			}
			xmlhttp.open("GET", url + "admin/utility.aspx?fill=true", false);
			xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
			xmlhttp.send(null);
		}
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:HiddenField ID="HF_id" Value="0" runat="server" />
			<asp:HiddenField ID="HF_lang" Value="1" runat="server" />
			<asp:LinqDataSource ID="LDS_collections" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_TBL_COLLECTIONs" OrderBy="title">
			</asp:LinqDataSource>
			<div id="fascia1">
				<div class="pannello_fascia1">
					<div style="clear: both">
						<h1>Lista collezione statiche </h1>
						<div class="bottom_agg">
						</div>
					</div>
					<div style="clear: both">
						<asp:ListView ID="LV_collection" runat="server" DataSourceID="LDS_collections" OnSelectedIndexChanging="LV_collection_SelectedIndexChanging">
							<ItemTemplate>
								<tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
									<td>
										<asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
									</td>
									<td>
										<asp:Label ID="Label1" runat="server" Text='<%# CommonUtilities.CutString("" + Eval("title"), 200)  %>' />
									</td>
									<td>
										<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
										<asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" OnClientClick="removeTinyEditor()" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda"
											Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" />
									</td>
								</tr>
							</ItemTemplate>
							<AlternatingItemTemplate>
								<tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
									<td>
										<asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
									</td>
									<td>
										<asp:Label ID="Label1" runat="server" Text='<%# CommonUtilities.CutString("" + Eval("title"), 200)  %>' />
									</td>
									<td>
										<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
										<asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" OnClientClick="removeTinyEditor()" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda"
											Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" />
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
											<th id="Th1" runat="server" style="width: 300px">
												Titolo
											</th>
											<th id="Th6" runat="server" style="width: 400px">
												Descrizione
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
										<asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
									</td>
									<td>
										<asp:Label ID="Label1" runat="server" Text='<%# CommonUtilities.CutString("" + Eval("title"), 200)  %>' />
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
							<span class="titoloboxmodulo">1. Dati collezione</span>
							<div class="boxmodulo">
								<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
									<tr>
										<td class="td_title">
											Titolo:
										</td>
										<td>
											<asp:TextBox runat="server" ID="txt_title" Width="230px" />
										</td>
									</tr>
								</table>
							</div>
							<span class="titoloboxmodulo">2. Blocchi</span>
							<div class="boxmodulo">
								<table>
									<tr>
										<td class="td_title">
											<asp:ListView ID="DL_blocks" runat="server" OnItemCommand="DL_blocks_ItemCommand" OnItemDeleting="DL_blocks_ItemDeleting">
												<ItemTemplate>
													<tr>
														<td>
															<asp:Label ID="pid_appartmentLabel" runat="server" Text='<%# CommonUtilities.GetStaticBlockName("" + Eval("pid_block")) %>'></asp:Label>
														</td>
														<td>
															<asp:ImageButton ID="ibtn_up" runat="server" ToolTip="Su" AlternateText="Su" OnClick="UpBlock" CommandArgument='<%# Eval("pid_block") %>' ImageUrl="~/images/ico/Go_up.png" />
														</td>
														<td>
															<asp:ImageButton ID="ibtn_down" runat="server" ToolTip="Giù" AlternateText="Giù" OnClick="DownBlock" CommandArgument='<%# Eval("pid_block") %>' ImageUrl="~/images/ico/Go_down.png" />
														</td>
														<td>
															<asp:ImageButton ID="ibtn_annulla" runat="server" CommandArgument='<%# Eval("pid_block") %>' CommandName="delete" OnClientClick="return confirm('Vuoi eliminare Blocco dalla Collezione?')"
																Height="11px" ImageUrl="~/images/ico/ico_del.gif" ToolTip="Elimina" />
														</td>
													</tr>
												</ItemTemplate>
												<AlternatingItemTemplate>
													<tr>
														<td>
															<asp:Label ID="pid_appartmentLabel" runat="server" Text='<%# CommonUtilities.GetStaticBlockName("" + Eval("pid_block")) %>'></asp:Label>
														</td>
														<td>
															<asp:ImageButton ID="ibtn_up" runat="server" ToolTip="Su" AlternateText="Su" OnClick="UpBlock" CommandArgument='<%# Eval("pid_block") %>' ImageUrl="~/images/ico/Go_up.png" />
														</td>
														<td>
															<asp:ImageButton ID="ibtn_down" runat="server" ToolTip="Giù" AlternateText="Giù" OnClick="DownBlock" CommandArgument='<%# Eval("pid_block") %>' ImageUrl="~/images/ico/Go_down.png" />
														</td>
														<td>
															<asp:ImageButton ID="ibtn_annulla" runat="server" CommandArgument='<%# Eval("pid_block") %>' CommandName="delete" OnClientClick="return confirm('Vuoi eliminare Blocco dalla Collezione?')"
																Height="11px" ImageUrl="~/images/ico/ico_del.gif" ToolTip="Elimina" />
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
																<th id="Th1" runat="server">
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
											<div class="agg" style="width: 200px;">
												<div class="sx">
													<span class="tit_agg">+ Aggiungi Blocco:</span>
													<span style="clear: both; float: left;">
														<asp:DropDownList ID="drp_blocks" runat="server" Width="150px">
														</asp:DropDownList>
													</span>
												</div>
												<div class="dx">
													<asp:ImageButton ID="ibtn_aggiungi_blocco" runat="server" Height="19px" Width="21px" ImageUrl="~/images/ico/agg.gif" ToolTip="Aggiungi Blocco" OnClick="ibtn_aggiungi_blocco_Click" />
												</div>
												<div class="nulla">
												</div>
											</div>
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

			<script type="text/javascript">
				function ReloadBlocks(id) {
					Shadowbox.close();
					__doPostBack('<%=lnk_annulla.ClientID%>', id);
				}
			</script>

		</ContentTemplate>
	</asp:UpdatePanel>
	<div style="display: none">
		<tr>
			<td colspan="2">
				<br />
				Descrizione:<br />
				<asp:TextBox runat="server" ID="txt_description" Width="400px" TextMode="MultiLine" Height="250px" />
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
		<tr>
			<td class="td_title">
				Imagine anteprima:
			</td>
			<td>
				<asp:FileUpload runat="server" ID="FU_img_pre" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FU_img_pre" ErrorMessage="*" ValidationGroup="foto3"></asp:RequiredFieldValidator>
				&nbsp;&nbsp;<asp:LinkButton ValidationGroup="foto3" runat="server" Text="carica" ID="lnk_carica_pre" OnClick="lnk_carica_pre_Click"></asp:LinkButton><br />
				<asp:LinkButton ID="lnk_delete_preview" runat="server" CssClass="bott_elimina_img" OnClick="lnk_delete_preview_Click">elimina</asp:LinkButton>
				<br />
				<asp:Image runat="server" ID="img_pre" />
				<asp:HiddenField ID="HF_img_pre" runat="server" />
			</td>
		</tr>
	</div>
</asp:Content>
