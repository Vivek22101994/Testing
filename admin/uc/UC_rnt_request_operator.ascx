<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_request_operator.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_request_operator" %>
<asp:HiddenField runat="server" ID="HF_pid_request" Value="0" />
<asp:HiddenField runat="server" ID="HF_pid_operator" Value="0" />
<asp:HiddenField runat="server" ID="HF_operatorName" Value="0" />
<div class="boxmodulo">
	<asp:PlaceHolder ID="PH_noedit" runat="server" Visible="false">
		<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
			<tr>
				<td colspan="2">
					<span class="titoloboxmodulo" id="lbl_noedit" runat="server">Cambio Stato</span>
				</td>
			</tr>
		</table>
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="PH_view" runat="server">
		<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
			<tr>
				<td colspan="2">
					<span class="titoloboxmodulo" style="margin-top: 10px;">Account Gestore della richiesta</span>
				</td>
			</tr>
			<tr>
				<td class="td_title">
					Data:
				</td>
				<td>
					<asp:Literal ID="ltr_date" runat="server"></asp:Literal>
				</td>
			</tr>
			<tr class="alternate">
				<td class="td_title">
					Ora:
				</td>
				<td>
					<asp:Literal ID="ltr_time" runat="server"></asp:Literal>
				</td>
			</tr>
			<tr>
				<td class="td_title">
					Account:
				</td>
				<td>
					<asp:Literal ID="ltr_operator" runat="server"></asp:Literal>
				</td>
			</tr>
		</table>
</asp:PlaceHolder>
<asp:PlaceHolder ID="PH_modify" runat="server">
		<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
			<tr>
				<td colspan="2">
					<span class="titoloboxmodulo" id="lbl_operatorState" runat="server">Cambio Stato</span>
				</td>
			</tr>
			<td colspan="2">
				Visualizza le email inviate negli :
				<asp:DropDownList ID="drp_mailing_days" runat="server" CssClass="inp" AutoPostBack="true" OnSelectedIndexChanged="drp_mailing_days_SelectedIndexChanged">
				</asp:DropDownList>
			</td>
				<tr>
				<td class="td_title">
					Solo Disponibili:
				</td>
				<td>
					<asp:CheckBox ID="chk_only_availables" runat="server" Checked="true" oncheckedchanged="chk_only_availables_CheckedChanged" AutoPostBack="true" />
				</td>
			</tr>
			<tr>
				<td class="td_title">
					Nuovo Account:
				</td>
				<td>
					<asp:DropDownList runat="server" ID="drp_admin" Width="150px">
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					Invia e-mail di assegnazione al Nuovo Account :
					<asp:CheckBox ID="chk_sendMail" runat="server" Checked="true" />
				</td>
			</tr>
			<tr id="ph_sendOld" runat="server">
				<td colspan="2">
					Invia Disdetta a <%=ltr_operator.Text %>:
					<asp:CheckBox ID="chk_sendOld" runat="server" Checked="true" />
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<span class="error_text" id="lbl_error" runat="server" visible="false">!Attenzione! Selezionare Account</span>
				</td>
			</tr>
		</table>
</asp:PlaceHolder>
<div class="salvataggio">
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_modify" runat="server" OnClick="lnk_modify_Click"><span>Riassegna ad un'altro</span></asp:LinkButton>
	</div>
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click"><span>Salva Modifiche</span></asp:LinkButton>
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
<div class="nulla">
</div>
