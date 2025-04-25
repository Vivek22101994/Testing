<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_static_page_multimedia.ascx.cs" Inherits="RentalInRome.admin.uc.UC_static_page_multimedia" %>
<asp:HiddenField ID="HF_id_multimedia" Value="0" runat="server" />
<asp:HiddenField ID="HF_id_statpage" runat="server" />
<asp:HiddenField ID="HF_type" Value="0" runat="server" />
<div class="mainbox">
	<div class="top">
		<div style="float: left;">
			<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
		<div style="float: right;">
			<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
	</div>
	<div class="center">
		<span class="titoloboxmodulo">
			<asp:Literal ID="ltrl_titolo" runat="server"></asp:Literal></span>
		<div class="boxmodulo">
			<div class="salvataggio" style="margin-bottom: 0px;">
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_new_2" runat="server" OnClick="lnk_new_2_Click"><span>Aggiungi Photo </span></asp:LinkButton>
				</div>
			</div>
		</div>
		<br />
		<br />
		<asp:Panel ID="pnl_edit" runat="server" Visible="false" CssClass="boxmodulo">
			<table>
				<tr>
					<td style="width: 100px; text-align: left; vertical-align: top;">
						Titolo:
					</td>
					<td style="width: 220px;">
						<asp:TextBox ID="txt_titolo" Width="210px" runat="server"></asp:TextBox>
						<br />
						<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txt_titolo" ValidationGroup="image" runat="server" ErrorMessage="// inserire titolo"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<table style="border-bottom: thin solid;">
							<tr>
								<td>
									<span class="titoloboxmodulo">Immagine Anteprima</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:FileUpload runat="server" ID="FU_img_preview" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FU_img_preview" ErrorMessage="// seleziona file"
										ValidationGroup="img_preview"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Image runat="server" ID="img_preview" />
								</td>
							</tr>
							<tr>
								<td>
									<asp:LinkButton ValidationGroup="img_preview" runat="server" CssClass="bott_carica" ID="lnk_load_preview" OnClick="lnk_load_preview_Click">carica</asp:LinkButton>
									<asp:LinkButton ID="lnk_delete_preview" runat="server" CssClass="bott_elimina_img" OnClick="lnk_delete_preview_Click">elimina</asp:LinkButton>
									<asp:HiddenField ID="HF_img_preview" runat="server" />
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<table style="border-bottom: thin solid;">
							<tr>
								<td>
									<span class="titoloboxmodulo">Immagine Grande</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:FileUpload runat="server" ID="FU_img_banner" /><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="FU_img_banner" ErrorMessage="// seleziona file"
										ValidationGroup="img_banner"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Image runat="server" ID="img_banner" />
								</td>
							</tr>
							<tr>
								<td>
									<asp:LinkButton ValidationGroup="img_banner" runat="server" CssClass="bott_carica" ID="LinkButton1" OnClick="lnk_load_banner_Click">carica</asp:LinkButton>
									<asp:LinkButton ID="LinkButton2" runat="server" CssClass="bott_elimina_img" OnClick="lnk_delete_banner_Click">elimina</asp:LinkButton>
									<asp:HiddenField ID="HF_img_banner" runat="server" />
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<div class="salvataggio" style="margin: 0px;">
							<div class="bottom_salva">
								<asp:LinkButton ID="lnk_upload" runat="server" CausesValidation="true" ValidationGroup="image" OnClick="lnk_upload_Click"><span>Salva</span></asp:LinkButton>
							</div>
							<div class="bottom_salva">
								<asp:LinkButton ID="lnk_cancel" runat="server" OnClick="lnk_cancel_Click"><span>Annulla</span></asp:LinkButton>
							</div>
							<div class="nulla">
							</div>
						</div>
					</td>
				</tr>
			</table>
		</asp:Panel>
	</div>
	<div class="center">
		<span class="titoloboxmodulo" style="margin-top: 20px;">Elenco Photo</span>
		<div class="boxmodulo">
			<asp:ListView ID="LV_video" runat="server" DataSourceID="LDS_video" OnItemCommand="LV_video_ItemCommand">
				<ItemTemplate>
					<div style="height: 90px; width: 60px; border: solid 1px; margin: 2px; float: left;">
						<a href="<%=CurrentAppSettings.ROOT_PATH %><%# Eval("path") %>" rel="shadowbox;width=500;height=350;" style="cursor: pointer; float: left;">
							<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
							<img alt="" width="54px" height="54px" src="<%=CurrentAppSettings.ROOT_PATH %><%# Eval("img_thumb") %>" />
						</a>
						<asp:Label ID="Label1" Visible="false" runat="server" Text='<%# Eval("id") %>' /><br />
						<asp:LinkButton ID="LinkButton2" CommandName="elimina" runat="server" OnClientClick="return confirm('vuoi eliminare il Photo?')">elimina</asp:LinkButton><br />
						<asp:LinkButton ID="LinkButton1" CommandName="seleziona" runat="server">modifica</asp:LinkButton>
					</div>
				</ItemTemplate>
				<LayoutTemplate>
					<div id="itemPlaceholderContainer" runat="server" style="float: left;">
						<a id="itemPlaceholder" runat="server" />
					</div>
					<div class="nulla">
					</div>
				</LayoutTemplate>
			</asp:ListView>
			<asp:LinqDataSource ID="LDS_video" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_RL_PAGE_MULTIMEDIAs" Where="pid_page == @pid_page &amp;&amp; type == @type">
				<WhereParameters>
					<asp:ControlParameter ControlID="HF_id_statpage" Name="pid_page" PropertyName="Value" Type="Int32" />
					<asp:Parameter DefaultValue="2" Name="type" Type="Int32" />
				</WhereParameters>
			</asp:LinqDataSource>
		</div>
	</div>
	<div class="bottom">
		<div style="float: left;">
			<img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
		<div style="float: right;">
			<img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
	</div>
</div>
