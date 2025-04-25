<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master"
    AutoEventWireup="true" CodeBehind="rnt_reservation_details.aspx.cs" Inherits="RentalInRome.admin.rnt_reservation_details" %>

<%@ Register Src="uc/UC_rnt_reservation_client.ascx" TagName="UC_rnt_reservation_client" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_agentClient.ascx" TagName="UC_rnt_reservation_agentClient" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_estate.ascx" TagName="UC_rnt_reservation_estate"
    TagPrefix="uc3" %>
<%@ Register Src="uc/UC_rnt_reservation_dt_pers.ascx" TagName="UC_rnt_reservation_dt_pers"
    TagPrefix="uc4" %>
<%@ Register Src="uc/UC_rnt_reservation_state.ascx" TagName="UC_rnt_reservation_state"
    TagPrefix="uc5" %>
<%@ Register Src="uc/UC_rnt_reservation_blockExpire.ascx" TagName="UC_rnt_reservation_blockExpire"
    TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_discount.ascx" TagName="UC_rnt_reservation_discount"
    TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_notes.ascx" TagName="UC_rnt_reservation_notes"
    TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_inout.ascx" TagName="UC_rnt_reservation_inout"
    TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_agent.ascx" TagName="UC_rnt_reservation_agent"
    TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_connectionlog.ascx" TagName="UC_rnt_reservation_connectionlog"
    TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_payment.ascx" TagName="UC_rnt_reservation_payment"
    TagPrefix="uc2" %>
<%@ Register Src="modRental/uc/ucProblemSelect.ascx" TagName="ucProblemSelect" TagPrefix="uc1" %>

<%@ Register Src="modRental/uc/ucReservationTmpPriceChange.ascx" TagName="ucReservationTmpPriceChange" TagPrefix="uc2" %>
<%@ Register Src="modRental/uc/ucResRefund.ascx" TagName="ucResRefund" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function refreshDates() {
            window.location = "rnt_request_details.aspx?id=<%=HF_id.Value %>";
        }
        function RNT_openSelection(IdEstate) {
            var _url = "rnt_reservation_form.aspx?IdEstate=" + IdEstate + "&IdRes=0&IdRequest=<%= HF_id.Value %>";
            //alert(_url);
            OpenShadowbox(_url, 800, 0);
        }
    </script>
    <style type="text/css">
        .left {
            float: left;
        }

        .line {
            border-bottom: 1px dashed #CCCCCC;
            clear: both;
            margin: 8px 0;
            margin-bottom: 15px;
            margin-top: 15px;
        }

        .hide {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel_main" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" runat="server" />
            <asp:HiddenField ID="HF_uid" runat="server" />
            <asp:HiddenField ID="HF_code" runat="server" />
            <asp:HiddenField ID="HF_IdRequest" runat="server" />
            <asp:HiddenField ID="HF_is_booking" runat="server" Value="1" />
            <asp:HiddenField ID="HF_dtDate" runat="server" />
            <asp:HiddenField ID="HF_dtPart" runat="server" />
            <asp:HiddenField ID="HF_dtStart" runat="server" />
            <asp:HiddenField ID="HF_dtEnd" runat="server" />
            <asp:HiddenField ID="HF_dtCount" runat="server" Value="0" />
            <asp:HiddenField ID="HF_saveOK" runat="server" Value="0" />
            <asp:HiddenField ID="HF_priceOK" runat="server" Value="0" />
            <asp:HiddenField ID="HF_periodOK" runat="server" Value="0" />
            <asp:HiddenField ID="HF_avvOK" runat="server" Value="0" />
            <div id="main">
                <span class="titlight">Dettagli della Prenotazioni #<%= HF_code.Value %></span><div class="mainline">
                    <div class="prices">
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <div class="price_div">
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_mailCreation"
                                    runat="server">

                                    <tr>
                                        <td colspan="2">
                                            <div class="salvataggio" id="Div5" runat="server">
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnk_sendCreationMail_Click"><span>Invio Mail di Creazione</span></asp:LinkButton>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_mailConfirm"
                                                runat="server">
                                                <tr>
                                                    <td colspan="2">Gestione Email Prenotazione
                                           
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chk_mailConfirm_cl" runat="server" Text="Cliente" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chk_mailConfirm_ad" runat="server" Text="Admin" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chk_mailConfirm_own" runat="server" Text="Poprietario" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chk_mailConfirm_srs" runat="server" Text="Accoglienza" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chk_mailConfirm_eco" runat="server" Text="Pulizie" />
                                                    </td>
                                                </tr>


                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="div_extraOwner" runat="server">
                                        <td>
                                            <table>
                                                <tr>
                                                    <td colspan="2">Gestione Servizi Extra
                                           
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>

                                                        <asp:CheckBoxList ID="chk_extrasercices" runat="server" RepeatColumns="1"></asp:CheckBoxList>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chk_servicemailConfirm_cl" runat="server" Text="Cliente" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chk_servicemailConfirm_ad" runat="server" Text="Admin" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td colspan="2">
                                            <div class="salvataggio" id="pnl_buttons" runat="server">
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_sendPaymentMail" runat="server" OnClick="lnk_sendPaymentMail_Click"><span>Invia Conferma della Pren.</span></asp:LinkButton>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="salvataggio" id="Div1" runat="server">
                                                <div class="bottom_salva">
                                                    <asp:HyperLink ID="HL_viewPdf" runat="server" Target="_blank"><span>Anteprima del Voucher</span></asp:HyperLink>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="salvataggio" id="Div3" runat="server">
                                                <div class="bottom_salva">
                                                    <asp:HyperLink ID="HL_getPdf" runat="server" Target="_blank"><span>Scarica Pdf del Voucher</span></asp:HyperLink>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="salvataggio">
                                                <div class="bottom_salva">
                                                    <a href="<%= CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_voucherfo.aspx?uid=" + tblReservation.unique_id + tblReservation.uid_2 %>" target="_blank"><span>Anteprima del Voucher per il Prop.</span></a>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="salvataggio" id="pnl_reservationarea" runat="server">
                                                <div class="bottom_salva">
                                                    <asp:HyperLink ID="HL_reservationarea" runat="server" Target="_blank"><span>Area Riservata della pren</span></asp:HyperLink>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="salvataggio" id="pnl_requestRenewalCont" runat="server">
                                                <div class="bottom_salva" id="pnl_requestRenewal" runat="server">
                                                    <asp:LinkButton ID="lnk_requestRenewal" runat="server" OnClick="lnk_requestRenewal_Click"><span>CANCELLATO - Richiedi Rinnovo dell'opzione</span></asp:LinkButton><br />
                                                </div>
                                                <asp:Label ID="lbl_requestRenewal_NO" runat="server" Style="font-weight: bold; float: left;">CANCELLATO - Rinnovo NON possibile</asp:Label><br />
                                                <asp:Label ID="lbl_requestRenewal" runat="server" Style="font-weight: bold; float: left;">CANCELLATO - Richiesta Rinnovo dell'opzione</asp:Label>
                                                <asp:Label ID="lbl_requestRenewal_OLD" runat="server" Style="font-weight: bold; float: left;">è presente in archivio una Richiesta Rinnovo dell'opzione</asp:Label>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="salvataggio" id="pnlSendMailForComments" runat="server">
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnkSendMailForComments" runat="server" OnClick="lnkSendMailForComments_Click"><span>Invia Invito per lasciare un commento</span></asp:LinkButton><br />
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>


                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <uc3:UC_rnt_reservation_estate ID="UC_estate" runat="server" />
                        <uc5:UC_rnt_reservation_state ID="UC_state" runat="server" />
                        <uc4:UC_rnt_reservation_dt_pers ID="UC_dt_pers" runat="server" />
                        <uc2:UC_rnt_reservation_blockExpire ID="UC_block" runat="server" />
                        <uc2:UC_rnt_reservation_notes ID="UC_notesInner" runat="server" />
                        <uc2:UC_rnt_reservation_notes ID="UC_notesEco" Title="Note pulizia (Eco)" runat="server" />
                        <uc1:ucProblemSelect ID="ucProblemSelect" runat="server" />
                        <uc2:UC_rnt_reservation_notes ID="UC_notesClient" Title="Note sul voucher" runat="server" />
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <div id="Div4" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
                            </div>
                            <div>
                                <h3>Come dormono
                                </h3>
                                <a href="<%=App.HOST_SSL %>/reservationarea/bedselection.aspx?usr=<%=UserAuthentication.CurrentUserID %>&authtmp=<%= HF_uid.Value %>"
                                    target="_blank" class="changeapt topright">Cambia</a>
                                <div class="price_div">
                                    <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                        <tr>
                                            <td>
                                                <asp:Literal ID="ltr_comeDormono" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <uc2:UC_rnt_reservation_inout ID="UC_inout" runat="server" />
                        <uc2:UC_rnt_reservation_discount ID="UC_discount" runat="server" />
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
                            </div>
                            <h3>Prezzi</h3>
                            <asp:HiddenField ID="HF_requestFullPay" runat="server" Value="0" />
                            <asp:HiddenField ID="HF_requestFullPayAccepted" runat="server" Value="0" />
                            <div class="price_div">
                                <uc2:ucReservationTmpPriceChange ID="ucPrice" runat="server" />
                                <div class="nulla">
                                </div>

                                <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                                    <asp:PlaceHolder ID="PH_requestFullPayAccepted" runat="server">
                                        <%= "<tr><th colspan=\"2\" style=\"padding: 5px;\">Abilitato pagamento del Saldo</th></tr>"%>
                                        <tr>
                                            <td colspan="2">
                                                <div class="salvataggio">
                                                    <div class="bottom_salva">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnk_requestFullPayNotAccepted_Click"><span>Disabilita pagamento del Saldo</span></asp:LinkButton>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PH_requestFullPayNotAccepted" runat="server">
                                        <%= HF_requestFullPay.Value == "1" ? "<tr><th colspan=\"2\" style=\"padding: 5px;\">Richiesto pagamento del Saldo</th></tr>" : ""%>
                                        <tr>
                                            <td colspan="2">
                                                <div class="salvataggio">
                                                    <div class="bottom_salva">
                                                        <asp:LinkButton ID="lnk_requestFullPayAccepted" runat="server" OnClick="lnk_requestFullPayAccepted_Click"><span>Abilita pagamento del Saldo</span></asp:LinkButton>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PH_ccData" runat="server">
                                        <tr>
                                            <td colspan="2">
                                                <div class="salvataggio">
                                                    <div class="bottom_salva">
                                                        <asp:HyperLink ID="HL_ccData" runat="server" Target="_blank" Style="display: none;"><span>Dati della Carta</span></asp:HyperLink>
                                                        <asp:TextBox ID="txtCC1" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtCC2" runat="server" TextMode="MultiLine" ReadOnly="true" CssClass="ccdata"></asp:TextBox>+
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </table>
                                <div style="clear: both">
                                    <asp:UpdatePanel ID="pnl_email" runat="server">
                                        <ContentTemplate>
                                            <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <div class="salvataggio">
                                                            <div class="bottom_salva">
                                                                <span>inviare e-mail saldo pagamenti </span>
                                                                <asp:DropDownList ID="drp_isSendbalancePaymentEmail" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_isSendbalancePaymentEmail_SelectedIndexChanged">
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>


                                <div style="float: right; width: 210px;">
                                </div>
                                <div class="nulla">
                                </div>
                                <asp:LinkButton ID="lnk_pricesDaTariffa" runat="server" CssClass="changeapt" OnClick="lnk_pricesDaTariffa_Click">Prezzi manuali</asp:LinkButton>
                            </div>
                            <div class="nulla">
                            </div>
                            <asp:LinkButton Visible="false" ID="lnk_calculatePrice" runat="server" CssClass="changeapt"
                                OnClick="lnk_calculatePrice_Click">Ricalcola Prezzi</asp:LinkButton>
                            <a id="rnt_paymentHistory_toggler" class="changeapt" href="javascript:rnt_toggle_paymentHistory()">Visualizza Storico Pagamenti</a>
                            <div class="nulla">
                            </div>
                            <div class="price_div" id="rnt_paymentHistory_cont" style="display: none;">
                                <uc2:UC_rnt_reservation_payment ID="ucPayment" runat="server" />
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <h3>Rimborsi</h3>
                            <div class="price_div">
                                <uc2:ucResRefund ID="ucResRefund" runat="server" />
                            </div>
                        </div>
                        <div class="nulla">
                        </div>

                        <div class="fasciatitBar" style="overflow: hidden; position: relative;" runat="server" id="div_service">
                            <div id="Div6" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
                            </div>
                            <h3>servizi extra</h3>

                            <div class="price_div">
                                <asp:ListView ID="LV_extraServices" runat="server" OnItemDataBound="LV_extraServices_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <span>
                                                    <asp:Label ID="lbl_date" runat="server"></asp:Label>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("pidExtras") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lbl_name" runat="server"></asp:Label>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("numPersAdult")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("numPersChild")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("Price")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("Commission")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("bookingId")%></span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <LayoutTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" style="">
                                            <tr style="text-align: left">
                                                <th style="width: 150px">Date
                                                </th>
                                                <th style="width: 300px">Nome
                                                </th>
                                                <th style="width: 70px">Adulti
                                                </th>
                                                <th style="width: 70px">Bambini
                                                </th>
                                                <th style="width: 100px">Totale Importo
                                                </th>
                                                <th style="width: 100px">Importo pagato
                                                </th>
                                                <th style="width: 100px">Prenotazione Id
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                </asp:ListView>


                            </div>
                            <div id="div_creditCard" runat="server">
                                <a id="rnt_ccData_toogler" class="changeapt" href="javascript:rnt_toggle_ccData()">Visualizza i dati della carta di Credito</a>
                                <div class="nulla">
                                </div>
                                <div class="price_div" id="div_cc" style="display: none;">
                                    <asp:Label ID="lbl_cc" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>

                        <div class="fasciatitBar" style="overflow: hidden; position: relative;" runat="server" id="div_request">
                            <div id="Div8" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
                            </div>
                            <h3>Richieste di Servizio</h3>

                            <div class="price_div">
                                <asp:ListView ID="LV_requests" runat="server" OnItemDataBound="LV_requests_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <span>
                                                    <asp:Label ID="lbl_date" runat="server"></asp:Label>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("pidExtra") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lbl_name" runat="server"></asp:Label>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("note")%></span>
                                            </td>

                                        </tr>
                                    </ItemTemplate>
                                    <LayoutTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" style="">
                                            <tr style="text-align: left">
                                                <th style="width: 150px">Date
                                                </th>
                                                <th style="width: 300px">Nome
                                                </th>
                                                <th style="width: 500px">Nota
                                                </th>

                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                </asp:ListView>


                            </div>

                        </div>
                        <div class="nulla">
                        </div>
                        <uc2:UC_rnt_reservation_connectionlog ID="UC_connectionlog" runat="server" />
                        <uc2:UC_rnt_reservation_agent ID="ucAgent" runat="server" />
                        <uc2:UC_rnt_reservation_agentClient ID="ucAgentClient" runat="server" />
                        <uc2:UC_rnt_reservation_client ID="UC_client" runat="server" />
                    </div>
                    <div class="fasciatitBar" runat="server" visible="false" style="overflow: hidden; position: relative;">
                        <h3>Salvataggio Dati</h3>
                        <div class="price_div" id="pnl_saveNO" runat="server">
                            <table class="selPeriod" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_stateError" runat="server" CssClass="numStep" Style="color: #FF0000;"
                                            Text="Selezionare lo stato!"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_clientError" runat="server" CssClass="numStep" Style="color: #FF0000;"
                                            Text="Compilare i dati del cliente"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_estateError" runat="server" CssClass="numStep" Style="color: #FF0000;"
                                            Text="Selezionare la struttura"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_priceError" runat="server" CssClass="numStep" Style="color: #FF0000;"
                                            Text="Calcolare i prezzi"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_periodError" runat="server" CssClass="numStep" Style="color: #FF0000;"
                                            Text="Selezionare il periodo e ospiti"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_avvError" runat="server" CssClass="numStep" Style="color: #FF0000;"
                                            Text="Appartamento non disponibile nel periodo!"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_priceError1" runat="server" CssClass="numStep" Style="color: #FF0000;"
                                            Text="Attenzione! MinStay in Altissima stagione è di 5 notti"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_priceError2" runat="server" CssClass="numStep" Style="color: #FF0000;"
                                            Text="Attenzione! Non rispetta Limitazione delle notti per prenotazioni future"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="pnl_notifyNew" runat="server" visible="false">
                                    <td>Invia Mail di richiesta del pagamento al cliente
                                            <br />
                                        per confermare la Prenotazione
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_sendMailCreated" runat="server">
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="btnric" style="float: left; margin: 50px;" id="pnl_btnSave" runat="server">
                                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click"><span>Salva</span></asp:LinkButton>
                            </div>
                        </div>
                        <div class="price_div" id="pnl_saveOK" runat="server" visible="false">
                            <table class="selPeriod" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label1" runat="server" CssClass="numStep" Style="color: #FF0000;"
                                            Text="Tutti i dati sono stati salvati"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="btnric" style="float: left; margin: 50px;" id="Div2" runat="server">
                                <a href="javascript:parent.refreshDates();"><span>chiudi</span></a>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                </div>

                <div class="nulla">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="hidden" id="rnt_paymentHistory_state" />
    <script type="text/javascript">
        function rnt_toggle_paymentHistory() {
            if ($("#rnt_paymentHistory_state").val() == "1") {
                //$("#rnt_paymentHistory_toggler").removeClass("opened");
                //$("#rnt_paymentHistory_toggler").addClass("closed");
                $("#rnt_paymentHistory_toggler").html("Visualizza Storico Pagamenti");
                $("#rnt_paymentHistory_cont").slideUp();
                $("#rnt_paymentHistory_state").val("0");
            }
            else {
                //$("#rnt_paymentHistory_toggler").removeClass("closed");
                //$("#rnt_paymentHistory_toggler").addClass("opened");
                $("#rnt_paymentHistory_toggler").html("Nascondi Storico Pagamenti");
                $("#rnt_paymentHistory_cont").slideDown();
                $("#rnt_paymentHistory_state").val("1");
            }
        }
        $("#rnt_paymentHistory_state").val("0");
    </script>
    <input type="hidden" id="rnt_ccData_state" />
    <script type="text/javascript">
        function rnt_toggle_ccData() {
            if ($("#rnt_ccData_state").val() == "1") {
                //$("#rnt_paymentHistory_toggler").removeClass("opened");
                //$("#rnt_paymentHistory_toggler").addClass("closed");
                $("#rnt_ccData_toogler").html("Visualizza i dati della carta di credito");
                $("#div_cc").slideUp();
                $("#rnt_ccData_state").val("0");
            }
            else {
                //$("#rnt_paymentHistory_toggler").removeClass("closed");
                //$("#rnt_paymentHistory_toggler").addClass("opened");
                $("#rnt_ccData_toogler").html("Nascondere i dati della carta di credito");
                $("#div_cc").slideDown();
                $("#rnt_ccData_state").val("1");
            }
        }
        $("#rnt_ccData_state").val("0");
    </script>

</asp:Content>
