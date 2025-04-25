<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_operator_details.aspx.cs" Inherits="RentalInRome.admin.usr_operator_details" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_usr_rl_country_lang.ascx" TagName="UC_usr_rl_country_lang" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_usr_tbl_admin_availability_view.ascx" TagName="UC_usr_tbl_admin_availability_view" TagPrefix="uc1" %>
<%@ Register src="uc/UC_usr_admin_password.ascx" tagname="UC_usr_admin_password" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:HiddenField ID="HF_id" Value="0" runat="server" />
			<asp:HiddenField ID="HF_lang" Value="1" runat="server" />
			<h1 class="titolo_main">Scheda Account</h1>
			<!-- INIZIO MAIN LINE -->
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
				<div class="bottom_salva">
					<a href="<%= listPage %>">
						<span>Torna nel elenco</span></a>
				</div>
                <div class="bottom_salva">
                    <a href="<%= "usr_operator_config.aspx?id="+HF_id.Value %>"><span>Configurazione mailing</span></a>
                </div>
                <div class="nulla">
				</div>
			</div>
			<div class="nulla">
			</div>
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
						<span class="titoloboxmodulo">Dati Identificativi</span>
						<div class="boxmodulo">
							<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
								<tr>
									<td class="td_title">
										Nome:
									</td>
									<td>
										<asp:TextBox runat="server" ID="txt_name" Width="300px" />
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Cognome:
									</td>
									<td>
										<asp:TextBox runat="server" ID="txt_surname" Width="300px" />
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Email:
									</td>
									<td>
										<asp:TextBox runat="server" ID="txt_email" Width="300px" />
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Telefono:
									</td>
									<td>
										<asp:TextBox runat="server" ID="txt_phone" Width="300px" />
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Cellulare:
									</td>
									<td>
										<asp:TextBox runat="server" ID="txt_cell" Width="300px" />
									</td>
								</tr>
                                <asp:PlaceHolder ID="PH_superadmin" runat="server">
								<tr>
									<td colspan="2">
										<strong>Limite Mail</strong>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Massimo:
									</td>
									<td>
										<asp:DropDownList ID="drp_mailing_max" runat="server">
											<asp:ListItem Text="non ha limite" Value="0"></asp:ListItem>
											<asp:ListItem Text="25 mail" Value="25"></asp:ListItem>
											<asp:ListItem Text="50 mail" Value="50"></asp:ListItem>
											<asp:ListItem Text="100 mail" Value="100"></asp:ListItem>
											<asp:ListItem Text="150 mail" Value="150"></asp:ListItem>
											<asp:ListItem Text="200 mail" Value="200"></asp:ListItem>
											<asp:ListItem Text="250 mail" Value="250"></asp:ListItem>
											<asp:ListItem Text="300 mail" Value="300"></asp:ListItem>
											<asp:ListItem Text="350 mail" Value="350"></asp:ListItem>
											<asp:ListItem Text="400 mail" Value="400"></asp:ListItem>
											<asp:ListItem Text="450 mail" Value="450"></asp:ListItem>
											<asp:ListItem Text="500 mail" Value="500"></asp:ListItem>
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										in periodo:
									</td>
									<td>
										<asp:DropDownList ID="drp_mailing_days" runat="server">
										</asp:DropDownList>
									</td>
								</tr>
                                </asp:PlaceHolder>
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
						<span class="titoloboxmodulo" style="margin-bottom: 0;">Orari Lavorativi</span>
						<div class="boxmodulo">
							<table cellpadding="3" cellspacing="3" style="margin-bottom: 1px">
								<tr>
									<td colspan="3">
										<span class="error_text">! Attenzione !<br />
											In questa sezione si inseriscono gli orari che vengono calcolati come lavorativo (compreso l'ora di fine),
											<br />
											tutto ciò che non e compreso viene calcolato come festivo o orario non lavorativo (non vengono inviate mail)!
											<br />
										</span>
										<asp:LinkButton ID="lnk_set_time" runat="server" OnClick="lnk_set_time_Click">clicca per impostare dal Tempo Pieno</asp:LinkButton>
									</td>
								</tr>
								<tr class="alternate">
									<td class="td_title">
										Lunedì
									</td>
									<td>
										dalle:
										<asp:DropDownList runat="server" ID="drp_time_start1" Style="width: 100px;" AutoPostBack="true" OnSelectedIndexChanged="drp_time_start1_SelectedIndexChanged">
										</asp:DropDownList>
									</td>
									<td>
										alle:
										<asp:DropDownList runat="server" ID="drp_time_end1" Style="width: 100px;">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Martedì
									</td>
									<td>
										dalle:
										<asp:DropDownList runat="server" ID="drp_time_start2" Style="width: 100px;" AutoPostBack="true" OnSelectedIndexChanged="drp_time_start2_SelectedIndexChanged">
										</asp:DropDownList>
									</td>
									<td>
										alle:
										<asp:DropDownList runat="server" ID="drp_time_end2" Style="width: 100px;">
										</asp:DropDownList>
									</td>
								</tr>
								<tr class="alternate">
									<td class="td_title">
										Mercoledì
									</td>
									<td>
										dalle:
										<asp:DropDownList runat="server" ID="drp_time_start3" Style="width: 100px;" AutoPostBack="true" OnSelectedIndexChanged="drp_time_start3_SelectedIndexChanged">
										</asp:DropDownList>
									</td>
									<td>
										alle:
										<asp:DropDownList runat="server" ID="drp_time_end3" Style="width: 100px;">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Giovedì
									</td>
									<td>
										dalle:
										<asp:DropDownList runat="server" ID="drp_time_start4" Style="width: 100px;" AutoPostBack="true" OnSelectedIndexChanged="drp_time_start4_SelectedIndexChanged">
										</asp:DropDownList>
									</td>
									<td>
										alle:
										<asp:DropDownList runat="server" ID="drp_time_end4" Style="width: 100px;">
										</asp:DropDownList>
									</td>
								</tr>
								<tr class="alternate">
									<td class="td_title">
										Venerdì
									</td>
									<td>
										dalle:
										<asp:DropDownList runat="server" ID="drp_time_start5" Style="width: 100px;" AutoPostBack="true" OnSelectedIndexChanged="drp_time_start5_SelectedIndexChanged">
										</asp:DropDownList>
									</td>
									<td>
										alle:
										<asp:DropDownList runat="server" ID="drp_time_end5" Style="width: 100px;">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Sabato
									</td>
									<td>
										dalle:
										<asp:DropDownList runat="server" ID="drp_time_start6" Style="width: 100px;" AutoPostBack="true" OnSelectedIndexChanged="drp_time_start6_SelectedIndexChanged">
										</asp:DropDownList>
									</td>
									<td>
										alle:
										<asp:DropDownList runat="server" ID="drp_time_end6" Style="width: 100px;">
										</asp:DropDownList>
									</td>
								</tr>
								<tr class="alternate">
									<td class="td_title">
										Domenica
									</td>
									<td>
										dalle:
										<asp:DropDownList runat="server" ID="drp_time_start7" Style="width: 100px;" AutoPostBack="true" OnSelectedIndexChanged="drp_time_start7_SelectedIndexChanged">
										</asp:DropDownList>
									</td>
									<td>
										alle:
										<asp:DropDownList runat="server" ID="drp_time_end7" Style="width: 100px;">
										</asp:DropDownList>
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
			<div class="mainline">
				<div class="mainbox">
					<div class="top">
						<div style="float: left;">
							<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
					</div>
					<div class="center">
						<span class="titoloboxmodulo">Location e Lingue Abbinati</span>
						<uc1:UC_usr_rl_country_lang ID="UC_usr_rl_country_lang" runat="server" />
						<div class="nulla">
						</div>
					</div>
					<div class="nulla">
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
						<uc2:UC_usr_admin_password ID="UC_usr_admin_password1" runat="server" />
						<div class="nulla">
						</div>
					</div>
					<div class="nulla">
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
						<uc1:UC_usr_tbl_admin_availability_view ID="UC_usr_tbl_admin_availability_view1" runat="server" />
						<div class="nulla">
						</div>
					</div>
					<div class="nulla">
					</div>
					<div class="nulla">
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
			<asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
			</asp:PlaceHolder>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
