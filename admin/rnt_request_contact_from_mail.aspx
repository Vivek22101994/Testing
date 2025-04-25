<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_request_contact_from_mail.aspx.cs" Inherits="RentalInRome.admin.rnt_request_contact_from_mail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

	<script type="text/javascript">
		var _editors = ['<%=txt_body.ClientID %>'];
		function removeTinyEditor() {
			removeTinyEditors(_editors);
			return false;
		}
		function setTinyEditor(IsReadOnly) {
			setTinyEditors(_editors, IsReadOnly);
		}
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:HiddenField ID="HF_unique" Value="" runat="server" Visible="false" />
			<asp:HiddenField ID="HF_id" Value="0" runat="server" />
			<asp:HiddenField ID="HF_pid_request" Value="0" runat="server" Visible="false" />
			<asp:HiddenField ID="HF_email" Value="" runat="server" Visible="false" />
			<asp:Literal ID="ltr_body_html_text" runat="server" Visible="false"></asp:Literal>
			<asp:Literal ID="ltr_body_plain_text" runat="server" Visible="false"></asp:Literal>
			<h1 class="titolo_main">Nuova Nota per il Cliente da mail</h1>
			<div class="mainline">
				<div class="mainbox">
					<div class="top">
						<div style="float: left;">
							<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
					</div>
					<div class="center">
						<span class="titoloboxmodulo">Dati Cliente</span>
						<div class="boxmodulo">
							<table>
								<tr>
									<td class="td_title">
										Richiesta:
									</td>
									<td>
										<asp:DropDownList ID="drp_relatedRequests" runat="server">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										Note:
										<br />
										<asp:TextBox runat="server" ID="txt_body" Width="400px" TextMode="MultiLine" Height="250px" />
									</td>
								</tr>
							</table>
						</div>
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
			<div class="salvataggio">
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_create" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_create_Click"><span>Salva Nota</span></asp:LinkButton>
				</div>
				<div class="bottom_salva">
					<a href="rnt_request_new_from_mail.aspx?id=<%= HF_id.Value %>"><span>Crea Nuova Richiesta da questa mail</span></a>
				</div>
				<div class="bottom_salva">
					<a href="<%=listPage %>">
						<span>Torna nel elenco delle Mail</span></a>
				</div>
				<div class="nulla">
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
