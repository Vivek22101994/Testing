<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_mailbody.ascx.cs" Inherits="RentalInRome.admin.uc.UC_mailbody" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" Visible="false" />
<asp:HiddenField ID="HF_id" Value="0" runat="server" Visible="false" />
<asp:HiddenField ID="HF_pid_request" Value="0" runat="server" Visible="false" />
<asp:HiddenField ID="HF_pid_request_state" Value="0" runat="server" Visible="false" />
<asp:HiddenField ID="HF_email" Value="" runat="server" Visible="false" />
<asp:Literal ID="ltr_body_html_text" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="ltr_body_plain_text" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="ltr_body_text" runat="server" Visible="false"></asp:Literal>
		<div class="mainbox" style="margin: 10px;">
			<div class="mainbox" style="margin: 10px;">
			<iframe src="mail_iframe_message_body.aspx?id=<%= IdMessage %>" style="height: 300px; width: 1150px;">
				<%= ltr_body_text.Text%>
			</iframe>
			</div>
		</div>
		<div class="nulla">
		</div>
		<div class="mainbox" style="margin: 10px; background-color: #F0F0F0;">
			<div class="mainbox" style="margin: 10px; background-color: inherit;">
				<table>
					<tr>
						<td colspan="2">
							<strong id="lbl_title" runat="server"></strong>
						</td>
					</tr>
					<tr id="pnl_new_request" runat="server">
						<td colspan="2">
							<a href="rnt_request_new_from_mail.aspx?id=<%= IdMessage %>"><strong>Crea Nuova Richiesta da questa mail</strong></a>
						</td>
					</tr>
					<tr id="pnl_new_contact" runat="server">
						<td colspan="2">
							<a href="rnt_request_contact_from_mail.aspx?id=<%= IdMessage %>"><strong>Aggiungi Note per il Cliente da questa mail</strong></a>
						</td>
					</tr>
				</table>
			</div>
		</div>
	