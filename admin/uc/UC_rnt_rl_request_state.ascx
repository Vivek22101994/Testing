<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_rl_request_state.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_rl_request_state" %>
<asp:HiddenField runat="server" ID="HF_pid_request" Value="0" />
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_cl_email" Value="" runat="server" />
<asp:HiddenField ID="HF_show_body" Value="" runat="server" />
<asp:Literal ID="ltr_estatesOK" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="ltr_estatesNO" runat="server" Visible="false"></asp:Literal>

<script type="text/javascript">
    function checkState() {
        var _state = $("#<%= drp_contract_state.ClientID %>").val();
        $("#txt_res_code_cont").css("display", (_state == "5" ? "" : "none"));
    }
    function addAvvOK() {
        addText($("#requestEstatesOK").html());
    }
    function addAvvNO() {
        addText($("#requestEstatesNO").html());
    }
    function addText(txt) {
        if (txt == null) { alert("si prega di aspettare caricamento completo dell'elenco dei preferiti"); return false; }
        if (txt == "") { alert("il testo da incollarea è vuoto"); return false; }
        var _Instance = tinyMCE.getInstanceById("<%= txt_mail_body.ClientID %>");
        tinyMCE.execInstanceCommand("<%= txt_mail_body.ClientID %>", "mceInsertContent", false, txt);
        return true;
    }
</script>

<asp:PlaceHolder ID="PH_view" runat="server">
	<span class="titoloboxmodulo" style="margin-top: 10px;">Stato corrente</span>
	<div class="boxmodulo">
		<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
			<tr>
				<td class="td_title">
					<strong>Data:</strong>
				</td>
				<td>
					<asp:Literal ID="ltr_date" runat="server"></asp:Literal>
				</td>
			</tr>
			<tr class="alternate">
				<td class="td_title">
					<strong>Ora:</strong>
				</td>
				<td>
					<asp:Literal ID="ltr_time" runat="server"></asp:Literal>
				</td>
			</tr>
			<tr>
				<td class="td_title">
					<strong>Stato:</strong>
				</td>
				<td>
					<asp:Literal ID="ltr_state" runat="server"></asp:Literal>
				</td>
			</tr>
			<tr class="alternate">
				<td class="td_title">
					<strong>Utente:</strong>
				</td>
				<td>
					<asp:Literal ID="ltr_user" runat="server"></asp:Literal>
				</td>
			</tr>
			<tr>
				<td class="td_title">
					<strong>Oggetto:</strong>
				</td>
				<td>
					<asp:Literal ID="ltr_subject" runat="server"></asp:Literal>
				</td>
			</tr>
			<tr>
				<td colspan="2">
				<strong>Note:</strong>
					<br />
					<asp:Literal ID="ltr_body" runat="server"></asp:Literal>
				</td>
			</tr>
		</table>
	</div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="PH_modify" runat="server">
	<div class="boxmodulo">
		<asp:HiddenField ID="HF_mod_mode" runat="server" Value="0" />
		<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
			<tr>
				<td colspan="2">
					<span id="lbl_title" runat="server" class="titoloboxmodulo" style="margin-top: 10px;"></span>
				</td>
			</tr>
			<asp:PlaceHolder ID="PH_mod_state" runat="server">
			<tr>
				<td class="td_title">
					<strong>Stato:</strong>
				</td>
				<td>
					<asp:DropDownList runat="server" ID="drp_contract_state" Width="150px" onchange="checkState()">
					</asp:DropDownList>
					<span id="txt_res_code_cont">Codice Pren.:&nbsp;<asp:TextBox ID="txt_res_code" runat="server" Width="50px"></asp:TextBox></span>
				</td>
			</tr>
            <tr>
				<td class="td_title">
					<strong>Oggetto:</strong>
				</td>
				<td>
					<asp:TextBox ID="txt_subject" runat="server" Width="300px"></asp:TextBox>
				</td>
			</tr>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="PH_add_contact" runat="server">
				<tr>
					<td class="td_title">
						<strong>Tipo:</strong>
					</td>
					<td>
						<asp:DropDownList ID="drp_subject" runat="server">
							<asp:ListItem Value="Ricevuta Mail dal Cliente"></asp:ListItem>
							<asp:ListItem Value="Inviata Mail al Cliente"></asp:ListItem>
							<asp:ListItem Value="Ricevuta Chiamata dal Cliente"></asp:ListItem>
							<asp:ListItem Value="Effettuata Chiamata al Cliente"></asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
			</asp:PlaceHolder>
            <tr>
				<td colspan="2">
					<strong>Note:</strong>
					<br />
					<asp:TextBox ID="txt_body" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
				</td>
			</tr>
		</table>
	</div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="PH_sendMail" runat="server">
    <div class="boxmodulo">
        <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
            <tr>
                <td colspan="3">
                    <span class="titoloboxmodulo" style="margin-top: 10px;">Nuova mail al cliente</span>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <strong>Nota Importante:</strong> 
                    <br/>
                    per includere Appartamenti Disponibili dalle preferenze del cliente:
                    <br />
                    Posizionare il cursore nel punto di inserimento e cliccare su <strong>Incolla Disponibili</strong>
                    <br/>
                    per Appartamenti NON Disponibili:
                    <br />
                    Posizionare il cursore nel punto di inserimento e cliccare su <strong>Incolla NON Disponibili</strong>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td class="td_title">
                    <strong>Oggetto:</strong>
                </td>
                <td>
                    <asp:TextBox ID="txt_mail_subject" runat="server" Width="300px"></asp:TextBox>
                </td>
                <td>
                    <div class="salvataggio" id="Div1" runat="server">
                        <div class="bottom_salva">
                            <a href="#" onclick="try{addAvvOK();}catch(ex){alert(ex)} return false;"><span>Incolla Disponibili</span></a>
                            <asp:LinkButton ID="lnk_copyOK" runat="server" OnClick="lnk_copyOK_Click" Visible="false"></asp:LinkButton>
                        </div>
                        <div class="bottom_salva">
                            <a href="#" onclick="try{addAvvNO();}catch(ex){alert(ex)} return false;"><span>Incolla NON Disponibili</span></a>
                            <asp:LinkButton ID="lnk_copyNO" runat="server" OnClick="lnk_copyOK_Click" Visible="false"></asp:LinkButton>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <strong>Contenuto:</strong>
                    <br />
                    <asp:TextBox ID="txt_mail_body" runat="server" TextMode="MultiLine" Width="650px" Height="400px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:PlaceHolder>
<div class="salvataggio" id="pnl_buttons" runat="server">
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_modify" runat="server" OnClick="lnk_modify_Click"><span>Cambia Stato</span></asp:LinkButton>
	</div>
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_addContact" runat="server" OnClick="lnk_addContact_Click"><span>Note per il Cliente</span></asp:LinkButton>
	</div>
    <div class="bottom_salva">
        <asp:LinkButton ID="lnk_sendMail" runat="server" OnClick="lnk_sendMail_Click"><span>Invia Mail al Cliente</span></asp:LinkButton>
    </div>
    <div class="bottom_salva">
		<asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click"><span>Salva / Invia</span></asp:LinkButton>
	</div>
	<div class="bottom_salva">
		<asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla</span></asp:LinkButton>
	</div>
	<div class="nulla">
	</div>
    <span id="lbl_resError" runat="server" visible="false" class="titoloboxmodulo" style="margin-top: 10px; color: #FF0000;"></span>
</div>
<div class="salvataggio" id="pnl_has_related" runat="server" visible="false">
	<span class="error_text">Possibile cambiare gli stati e contatti solo dalla Richiesta Primaria</span>
	<div class="nulla">
	</div>
</div>
<span class="titoloboxmodulo">Storico stati e contatti</span>
<div class="boxmodulo">
	<asp:ListView runat="server" ID="LV" DataSourceID="LDS" onitemcommand="LV_ItemCommand" onselectedindexchanging="LV_SelectedIndexChanging" onitemdatabound="LV_ItemDataBound">
		<ItemTemplate>
			<tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')" id="tr_normal" runat="server" style="">
				<td>
					<span>
						<%# Eval("date_state")%></span>
				</td>
				<td>
					<span>
						<%#  rntUtils.rntRequest_getStateName("" + Eval("pid_state"))%></span>
				</td>
				<td>
					<span>
						<%# AdminUtilities.usr_adminName((int)Eval("pid_user"),"")%></span>
				</td>
				<td>
					<span>
						<%# Eval("subject")%></span>
				</td>
				<td>
					<asp:LinkButton ID="lnk_select" runat="server" Style="margin-right: 20px;" CommandName="select" OnClientClick="return false;">note</asp:LinkButton>
					<asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
					<asp:Label runat="server" ID="lbl_pid_state" Text='<%# Eval("pid_state") %>' Visible="false"></asp:Label>
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')" id="tr_normal" runat="server" style="">
				<td>
					<span>
						<%# Eval("date_state")%></span>
				</td>
				<td>
					<span>
						<%#  rntUtils.rntRequest_getStateName("" + Eval("pid_state"))%></span>
				</td>
				<td>
					<span>
						<%# AdminUtilities.usr_adminName((int)Eval("pid_user"), "")%></span>
				</td>
				<td>
					<span>
						<%# Eval("subject")%></span>
				</td>
				<td>
					<asp:LinkButton ID="lnk_select" runat="server" Style="margin-right: 20px;" CommandName="select" OnClientClick="return false;">note</asp:LinkButton>
					<asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
					<asp:Label runat="server" ID="lbl_pid_state" Text='<%# Eval("pid_state") %>' Visible="false"></asp:Label>
				</td>
			</tr>
		</AlternatingItemTemplate>
		<EmptyDataTemplate>
			<table id="itemPlaceholderContainer" runat="server" border="0" style="">
				<tr id="Tr5" runat="server" style="">
					<th id="Th1" runat="server" style="width: 100px;">
						Data
					</th>
					<th id="Th8" runat="server" style="width: 100px;">
						Stato
					</th>
					<th id="Th11" runat="server" style="width: 130px;">
						Utente
					</th>
					<th id="Th2" runat="server">
						Commenti
					</th>
				</tr>
			</table>
		</EmptyDataTemplate>
		<InsertItemTemplate>
		</InsertItemTemplate>
		<LayoutTemplate>
			<table id="itemPlaceholderContainer" runat="server" border="0" style="">
				<tr id="Tr5" runat="server" style="">
					<th id="Th1" runat="server" style="width: 120px;">
						Data Ora
					</th>
					<th id="Th8" runat="server" style="width: 100px;">
						Stato
					</th>
					<th id="Th11" runat="server" style="width: 100px;">
						Utente
					</th>
					<th id="Th2" runat="server">
						Oggetto
					</th>
				</tr>
				<tr id="itemPlaceholder" runat="server">
				</tr>
			</table>
		</LayoutTemplate>
		<SelectedItemTemplate>
			<tr class="current" id="tr_selected" runat="server" style="cursor: pointer;">
				<td>
					<span>
						<%# Eval("date_state")%></span>
				</td>
				<td>
					<span>
						<%# rntUtils.rntRequest_getStateName("" + Eval("pid_state"))%></span>
				</td>
				<td>
					<span>
						<%# AdminUtilities.usr_adminName((int)Eval("pid_user"), "")%></span>
				</td>
				<td>
					<span>
						<%# Eval("date_state")%></span>
				</td>
				<td>
					<asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
					<asp:Label runat="server" ID="lbl_pid_state" Text='<%# Eval("pid_state") %>' Visible="false"></asp:Label>
					<asp:LinkButton ID="lnk_close" runat="server" Style="margin-right: 20px;" CommandName="close" OnClientClick="return false;">chiudi</asp:LinkButton>
				</td>
			</tr>
			<tr class="current">
				<td colspan="5">
					<div class="mainbox" style="margin: 10px;">
						<div class="mainbox" style="margin: 10px;">
                            <iframe src="rnt_request_iframe_state_body.aspx?id=<%# Eval("id") %>" style="max-height: 300px; width: 650px;">
                                <%# Eval("body")%>
                            </iframe>
						</div>
					</div>
				</td>
			</tr>
		</SelectedItemTemplate>
	</asp:ListView>
	<asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_RL_REQUEST_STATEs" Where="pid_request == @pid_request" OrderBy="date_state desc">
		<WhereParameters>
			<asp:ControlParameter ControlID="HF_pid_request" Name="pid_request" PropertyName="Value" Type="Int32" />
		</WhereParameters>
	</asp:LinqDataSource>
</div>
