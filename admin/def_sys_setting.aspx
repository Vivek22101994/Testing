<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="def_sys_setting.aspx.cs" Inherits="RentalInRome.admin.def_sys_setting" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:HiddenField ID="HF_id" Value="0" runat="server" />
			<asp:HiddenField ID="HF_lang" Value="1" runat="server" />
			<h1 class="titolo_main">Variabili del sistema </h1>
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
                                    <td>
                                        <span class="titoloboxmodulo" style="margin: 10px 0 0;">Abbinamento delle Richieste Correlate</span>
                                    </td>
                                </tr>
								<tr>
									<td>
										Limite di Ultime Ore che vengono considerate:
										<br/>
										(inserire 0 per toglere il limite)
									</td>
								</tr>
								<tr>
									<td>
										<asp:TextBox runat="server" ID="txt_rnt_request_relation_in_hours" Width="30px" /> ore
										<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txt_rnt_request_relation_in_hours" ValidationGroup="rnt_request_relation_in_hours" ErrorMessage="<br/>//valore obbligatorio"></asp:RequiredFieldValidator>
										<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ControlToValidate="txt_rnt_request_relation_in_hours" ValidationGroup="rnt_request_relation_in_hours" ErrorMessage="<br/>//solo numeri consentiti"
											ValidationExpression="(^(-)?\d*?$)"></asp:RegularExpressionValidator>
									</td>
								</tr>
								<tr>
									<td>
										Prendere in considerazione le date Check-In e Check-Out:
									</td>
								</tr>
								<tr>
									<td>
										<asp:DropDownList ID="drp_rnt_request_relation_is_view_date" runat="server">
											<asp:ListItem Text="SI" Value="1"></asp:ListItem>
											<asp:ListItem Text="NO" Value="0"></asp:ListItem>
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td>
										Considerare Correlate se seguenti dati sono uguali:
									</td>
								</tr>
								<tr>
									<td>
										<asp:DropDownList ID="drp_rnt_request_relation_view_fld" runat="server">
											<asp:ListItem Text="solo E-Mail" Value="email"></asp:ListItem>
											<asp:ListItem Text="solo Nome/Cognome" Value="name_full"></asp:ListItem>
											<asp:ListItem Text="E-Mail o Nome/Cognome" Value="email or name_full"></asp:ListItem>
											<asp:ListItem Text="E-Mail e Nome/Cognome" Value="email and name_full"></asp:ListItem>
										</asp:DropDownList>
									</td>
								</tr>
                                <tr>
                                    <td>
                                        <span class="titoloboxmodulo" style="margin: 10px 0 0;">Scadenze delle Opzioni da Confermare</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       Prenotazioni dal gestionale
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_rnt_reservationExpire_defaultHours" Width="30px" />
                                        ore
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="txt_rnt_reservationExpire_defaultHours" ValidationGroup="rnt_request_relation_in_hours" ErrorMessage="<br/>//valore obbligatorio"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic" ControlToValidate="txt_rnt_reservationExpire_defaultHours" ValidationGroup="rnt_request_relation_in_hours" ErrorMessage="<br/>//solo numeri consentiti" ValidationExpression="(^(-)?\d*?$)"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Prenotazioni Online(InstantBooking)
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_rnt_reservationExpire_defaultHoursOnline" Width="30px" />
                                        ore
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="txt_rnt_reservationExpire_defaultHoursOnline" ValidationGroup="rnt_request_relation_in_hours" ErrorMessage="<br/>//valore obbligatorio"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic" ControlToValidate="txt_rnt_reservationExpire_defaultHoursOnline" ValidationGroup="rnt_request_relation_in_hours" ErrorMessage="<br/>//solo numeri consentiti" ValidationExpression="(^(-)?\d*?$)"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
									<td>
										<div class="salvataggio">
											<div class="bottom_salva">
												<asp:LinkButton ID="lnk_modify_rnt_request_relation_in_hours" runat="server" OnClick="lnk_modify_rnt_request_relation_in_hours_Click"><span>Modifica</span></asp:LinkButton>
											</div>
											<div class="bottom_salva">
												<asp:LinkButton ID="lnk_salva_rnt_request_relation_in_hours" runat="server" ValidationGroup="rnt_request_relation_in_hours" OnClick="lnk_salva_rnt_request_relation_in_hours_Click"><span>Salva</span></asp:LinkButton>
											</div>
											<div class="bottom_salva">
												<asp:LinkButton ID="lnk_cancel_rnt_request_relation_in_hours" runat="server" OnClick="lnk_cancel_rnt_request_relation_in_hours_Click"><span>Annulla</span></asp:LinkButton>
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
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
