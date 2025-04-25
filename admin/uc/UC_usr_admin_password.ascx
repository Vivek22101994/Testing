<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_usr_admin_password.ascx.cs" Inherits="RentalInRome.admin.uc.UC_usr_admin_password" %>
<asp:HiddenField runat="server" ID="HF_pid_admin" Value="0" Visible="false" />
<asp:HiddenField runat="server" ID="HF_pwd" Value="" Visible="false" />
<asp:HiddenField runat="server" ID="HF_login" Value="" Visible="false" />
<span class="titoloboxmodulo" style="margin-top: 10px;" id="lbl_title" runat="server">Gestione Password del Utente</span>
<div class="boxmodulo">
<asp:PlaceHolder ID="PH_view" runat="server">
		<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
			<tr>
				<td class="td_title">
					Login di Accesso:
				</td>
				<td>
					<%= HF_login.Value %>
				</td>
			</tr>
		</table>
</asp:PlaceHolder>
<asp:PlaceHolder ID="PH_modify" runat="server">
		<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" >
			<tr>
				<td class="td_title">
					Password Corrente:
				</td>
				<td>
					<asp:TextBox ID="txt_pwd_old" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>				
				</td>
			</tr>
			<tr>
				<td class="td_title">
					Login di Accesso:
				</td>
				<td>
					<asp:TextBox ID="txt_login" runat="server" MaxLength="50"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td class="td_title">
					Nuova Password:
				</td>
				<td>
					<asp:TextBox ID="txt_pwd_new" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td class="td_title">
					Conferma Password:
				</td>
				<td>
					<asp:TextBox ID="txt_pwd_confirm" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<span class="error_text" id="lbl_error" runat="server" visible="false">!Attenzione! Selezionare Account</span>
				</td>
			</tr>
		</table>
</asp:PlaceHolder>
<div class="nulla">
</div>
	<div class="salvataggio" style="margin-bottom: 0;">
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_modify" runat="server" OnClick="lnk_modify_Click"><span>Cambia Password</span></asp:LinkButton>
	</div>
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_send_pwd" runat="server" OnClick="lnk_send_pwd_Click"><span>Invia Password al E-mail</span></asp:LinkButton>
	</div>
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click"><span>Salva Password</span></asp:LinkButton>
	</div>
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
	</div>
	<div class="nulla">
	</div>
</div>
	<div class="nulla">
	</div>
</div>

