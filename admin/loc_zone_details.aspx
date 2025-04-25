<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="loc_zone_details.aspx.cs" Inherits="RentalInRome.admin.loc_zone_details" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="modContent/UCgetImg.ascx" TagName="UCgetImg" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

	<script type="text/javascript">
		function removeTinyEditor() {
		}
		function setTinyEditor(IsReadOnly) {
		}
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:HiddenField ID="HF_id" Value="0" runat="server" />
			<asp:HiddenField ID="HF_lang" Value="1" runat="server" />
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
                            <table cellpadding="3" cellspacing="0">
                                <tr>
                                    <td>
                                        Attivo:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_is_active" runat="server" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td>
                                        Nome intero:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_img_thumb" Width="150px" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td>
                                        Citta:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_city" Width="120px" />
                                    </td>
                                </tr>
                            </table>
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
							<table cellpadding="0" cellspacing="10">
								<tr>
									<td style="border-right: medium ridge;">
										<table cellpadding="3" cellspacing="0">
											<tr>
                                                <td>
													<span class="titoloboxmodulo">Visualizzazione</span>
												</td>
											</tr>
                                            <tr>
                                                <td>
                                                    Percorso Pagina:<br />
                                                    <asp:TextBox runat="server" ID="txt_folder_path" Width="300px" MaxLength="100" />
                                                    <asp:TextBox runat="server" ID="txt_file_extension" Width="40px" MaxLength="100" Text=".htm" ReadOnly="true" />
                                                </td>
                                            </tr>
											<tr>
                                                <td>
													Titolo:
                                                    <br />
													<asp:TextBox runat="server" ID="txt_title" Width="300px" />
												</td>
											</tr>
											<tr>
												<td>
													Sommario:<br />
													<asp:TextBox runat="server" ID="txt_summary" Width="400px" TextMode="MultiLine" Height="115px" />
												</td>
											</tr>
										</table>
									</td>
									<td style="border-left: medium ridge;">
										<table cellpadding="3" cellspacing="0">
											<tr>
                                                <td>
													<span class="titoloboxmodulo">Motori Ricerca</span>
												</td>
											</tr>
                                            <tr>
												<td>
                                                    Meta Title:<br />
                                                    <span class="error_text" id='count_txt_meta_title' style="display: none">
                                                        <span class="char"></span>
                                                        &nbsp;caratteri e&nbsp;
                                                        <span class="word"></span>
                                                        &nbsp;parole </span>
                                                    <br />
                                                    <asp:TextBox runat="server" ID="txt_meta_title" Width="350px" MaxLength="100" onkeyup="CountWords(this,'count_txt_meta_title')" />
												</td>
											</tr>
											<tr>
                                                <td>
                                                    Meta Description:<br />
                                                    <span class="error_text" id='count_txt_meta_description' style="display: none">
                                                        <span class="char"></span>
                                                        &nbsp;caratteri e&nbsp;
                                                        <span class="word"></span>
                                                        &nbsp;parole </span>
                                                    <br />
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator12" ControlToValidate="txt_meta_description" Display="Dynamic" Text="// 500 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,500}$" ValidationGroup="dati" runat="server" />
                                                    <asp:TextBox runat="server" ID="txt_meta_description" TextMode="MultiLine" MaxLength="500" Height="150px" Width="350px" onkeyup="CountWords(this,'count_txt_meta_description')" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
                                <tr>
                                    <td colspan="2">
                                       Descrizione:<br />
                                        <telerik:RadEditor runat="server" ID="re_description" SkinID="DefaultSetOfTools" Height="400" Width="600" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
                                            <CssFiles>
                                                <telerik:EditorCssFile Value="" />
                                            </CssFiles>
                                        </telerik:RadEditor>
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
				<div class="mainbox">
					<div class="top">
						<div style="float: left;">
							<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
					</div>
					<div class="center">
						<span class="titoloboxmodulo">Immagine Banner nella scheda</span>
						<div class="boxmodulo">
                            <uc1:UCgetImg ID="imgBanner" runat="server" ImgMaxWidth="405" />
                        </div>
					</div>
					<div class="bottom">
						<div style="float: left;">
							<img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
					</div>
				</div>
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Immagine Anteprima HomePage</span>
                        <div class="boxmodulo">
                            <uc1:UCgetImg ID="imgPreview" runat="server" ImgMaxWidth="405" />
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
					<asp:LinkButton ID="lnk_modify" runat="server" OnClick="lnk_modify_Click" OnClientClick="removeTinyEditor();"><span>Modifica</span></asp:LinkButton>
				</div>
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
				</div>
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
				</div>
				<div class="bottom_salva">
					<a href="<%=listPage %>">
						<span>Torna nel elenco</span></a>
				</div>
				<div class="nulla">
				</div>
			</div>
			<asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false"></asp:PlaceHolder>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
