<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pdf_rnt_reservation_voucher.aspx.cs" Inherits="RentalInRome.pdfgenerator.pdf_rnt_reservation_voucher" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @import url(/css/style_pdf.css);
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:HiddenField ID="HF_IdReservation" runat="server" Value="0" Visible="false" />
    <asp:HiddenField ID="HF_IdClient" runat="server" Value="0" Visible="false" />
    <asp:HiddenField ID="HF_IdEstate" runat="server" Value="0" Visible="false" />
    <asp:HiddenField ID="HF_pid_lang" runat="server" Value="0" Visible="false" />
    <asp:HiddenField ID="HF_numPers" runat="server" Value="0" Visible="false" />
    <asp:HiddenField ID="HF_nightsTotal" runat="server" Value="0" Visible="false" />
    <asp:HiddenField ID="HF_weeks" runat="server" Value="0" Visible="false" />
    <asp:HiddenField ID="HF_nights" runat="server" Value="0" Visible="false" />
    <asp:Literal ID="ltr_srs" runat="server" Visible="false"></asp:Literal>
    <div id="container" style="clear:both;">
        <table width="960" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td valign="middle" align="left">
                    <%if (tblEstate.pid_city == 1)
                  {%>
                    <img src="/images/css/logo.gif" title="Rent apartments in Rome, Italy" alt="Rent apartments in Rome, Italy" style="margin-bottom: 10px;" />
                    <%} %>
                    <%if (tblEstate.pid_city == 2)
                  {%>
                    <img src="/images/css/logo_RiF.gif" title="Rent apartments in Florence, Italy" alt="Rent apartments in Florence, Italy" style="margin-bottom: 10px;" />
                    <%} %>
                    <%if (tblEstate.pid_city == 3)
                  {%>
                    <img src="/images/css/logo_RiV.gif" title="Rent apartments in Venice, Italy" alt="Rent apartments in Venice, Italy" style="margin-bottom: 10px;" />
                    <%} %>
                </td>
                <td rowspan="2">
                    <telerik:RadBarcode runat="server" ID="RadBarcode1" Type="QRCode" Style="float: right;">
                        <QRCodeSettings AutoIncreaseVersion="true" />
                    </telerik:RadBarcode>
                </td>
            </tr>
            <tr>
                <td class="titTestata" valign="middle" align="left" style="text-align: left;">
                    <%=CurrentSource.getSysLangValue("lblConfirmationVoucher", "Confirmation <span class=\"orange\">Voucher</span>")%>
                </td>
            </tr>
        </table>
        <hr />
        <table width="960" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="10">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Codice_di_prenotazione", "Codice di Prenotazione")%></b>
                    <%= tblReservation.code %>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Firmatario", "Firmatario/a")%></b>
                    <%= tblClient.name_honorific+"&nbsp;"+tblClient.name_full%>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Booking_Agent", "Booking Agent")%>:</b>&nbsp;<%= tblReservation.pid_operator.objToInt32() > 3 ? AdminUtilities.usr_adminName(tblReservation.pid_operator.objToInt32(), "RentalInRome") + "&lt;" + AdminUtilities.usr_adminEmail(tblReservation.pid_operator.objToInt32(), "info@rentalinrome.com") + "&gt;" : "RentalInRome&lt;info@rentalinrome.com&gt;"%>
                </td>
            </tr>
        </table>
        <br />
        <table width="960" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="10" style="border-bottom: 1px #fe6634 dotted; padding: 0 0 10px 0; color: #fe6634; font-size: 18px;">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_dati_cliente", "DATI CLIENTE")%>:</b>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Nato_a", "Nato a")%>:</b>
                    <%= tblClient.birth_place %>
                </td>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Data_di_Nascita", "Data di nascita")%>:</b>
                    <%= tblClient.birth_date.formatCustom("#dd#/#mm#/#yy#", HF_pid_lang.Value.ToInt32(),"--/--/----")%>
                </td>
            </tr>
            <tr>
                <td colspan="5" style="border-bottom: 1px #888 dotted;">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Documento_d_identita", "Documento d'identità")%>:</b><%=CurrentSource.getSysLangValue("doc_" + tblClient.doc_type, HF_pid_lang.Value.ToInt32())%>
                    n°
                    <%= tblClient.doc_num %>
                </td>
                <td colspan="5" style="border-bottom: 1px #888 dotted;">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Data_di_scadenza", "Data di scadenza")%>:</b>
                    <%= tblClient.doc_expiry_date.formatCustom("#dd#/#mm#/#yy#", HF_pid_lang.Value.ToInt32(),"--/--/----")%>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_IndirizzoResidenza", "Indirizzo di residenza")%>:</b>
                    <%= tblClient.loc_address + ("" + tblClient.loc_city != "" ? "&nbsp;&nbsp;&nbsp;&nbsp;<b>" + CurrentSource.getSysLangValue("lblCity", _currLang, "City") + "</b>: " + tblClient.loc_city : "")%>
                </td>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Regione", "Regione")%>:</b>
                    <%= tblClient.loc_state %>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Paese", "Paese")%>:</b>
                    <%= tblClient.loc_country %>
                </td>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_CAP", "CAP")%>:</b>
                    <%= tblClient.loc_zip_code %>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Cellulare", "Cellulare")%>:</b>
                    <%= tblClient.contact_phone_mobile %><%= (!string.IsNullOrEmpty(tblClient.contact_phone_mobile) && !string.IsNullOrEmpty(tblClient.contact_phone_trip) ? "; " : "")%><%= tblClient.contact_phone_trip %>
                </td>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Telefono_dell_abitazione", "Telefono dell'abitazione")%>:</b>
                    <%= tblClient.contact_phone %>
                </td>
            </tr>
            <tr>
                <td colspan="5" runat="server" visible="false">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Indirizzo_email", "Indirizzo email")%>:</b>
                    <%= tblClient.contact_email %>
                </td>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Numero_di_fax", "numero di Fax")%>:</b>
                    <%= tblClient.contact_fax %>
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <table width="960" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="10">
                    <b><em>
                        <%=CurrentSource.getSysLangValue("pdf_dichiara_prenotazione_per_uso_turistico", "Dichiara di voler prenotare l’appartamento qui descritto esclusivamente a titolo di locazione turistica")%>:</em></b>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Nome_appartamento", "Nome appartamento")%>:</b>
                     <%= CurrentSource.rntEstate_title_agency(tblEstate, tblReservation.agentID.objToInt64(), _currLang,"---")%>
                   <%-- <%= CurrentSource.rntEstate_title(IdEstate, _currLang,"---")%>--%>
                </td>
                <td colspan="5">
                    <b>
                        <%=CurrentSource.getSysLangValue("lblAddress", "Indirizzo")%>:</b> <%= tblEstate.loc_address + (!string.IsNullOrEmpty(tblEstate.loc_inner_bell) ? ", " + tblEstate.loc_inner_bell : "") + ", " + CurrentSource.loc_cityTitle(tblEstate.pid_city.objToInt32(), _currLang, "")%>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <b>
                        <%=CurrentSource.getSysLangValue("lblForAnyInformationContactUs", "For any information or special requests, please contact us")%>:</b>
                    <%= ltr_srs.Text.Length > 2 ? ltr_srs.Text.Substring(0,ltr_srs.Text.Length-1) : ltr_srs.Text%>
                </td>
            </tr>
            <%if (!string.IsNullOrEmpty(tblReservation.srs_ext_meetingPoint))
              {%>
            <tr>
                <td colspan="10">
                    <b>
                        <%=CurrentSource.getSysLangValue("rntWelcomeMeetingPoint", "Punto d'Incontro")%>:</b>
                    <%= tblReservation.srs_ext_meetingPoint%>
                </td>
            </tr>
            <%} %>
        </table>
        <table width="960" cellpadding="0" cellspacing="0" border="0" id="pnl_visa_isRequested" runat="server" visible="false">
            <tr>
                <td colspan="10">
                    <b><%=CurrentSource.getSysLangValue("rntListOfGuests")%></b>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <img alt="" src="firma-RiR.PNG" style="height: 200px; margin-left: 409px; position: absolute; top: 0;" />
                    <table>
                        <tr>
                            <td>
                                <%=CurrentSource.getSysLangValue("reqFullName")%>
                            </td>
                            <td>
                                <%=CurrentSource.getSysLangValue("pdf_Documento_d_identita")%>
                            </td>
                        </tr>
                        <asp:ListView ID="LV_visa" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Eval("name_full")%>
                                    </td>
                                    <td>
                                        <%# CurrentSource.getSysLangValue("doc_" + Eval("doc_type"), HF_pid_lang.Value.ToInt32())%>&nbsp;#<%# Eval("doc_num")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </table>
                </td>
            </tr>
        </table>
        <hr />
        <br/>
<%--        <img src="/images/banner/Banner-Rome-Free-Lounge.jpg" style="width:100%; clear:both;" alt="Bag Storage only for the Rental In Rome guests for 3 Euro Full Day" />--%>
        <br/><br/>
        <hr />
        <table width="960" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="10">
                    <%=CurrentSource.getSysLangValue("pdf_primo_terms", "TERMINI E CONDIZIONI 1")%>
                </td>
            </tr>
        </table>
        <hr />
        <table width="960" cellpadding="4" cellspacing="3" border="0">
            <tr>
                <td colspan="12" bgcolor="#54495a" style="color: #FFF;">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_PERIODO_DI_LOCAZIONE", "PERIODO DI LOCAZIONE")%></b>
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#a09ea1">
                    <b>
                        <%=CurrentSource.getSysLangValue("lblNumPersons", "Num. Persone")%>:</b>
                        <%= HF_numPers.Value%>
                        <br />
                    <%= tblReservation.num_adult.objToInt32() + " " + CurrentSource.getSysLangValue("reqAdults") + " + " + tblReservation.num_child_over.objToInt32() + " " + CurrentSource.getSysLangValue("lblChildren3OrOver") + " + " + tblReservation.num_child_min.objToInt32() + " " + CurrentSource.getSysLangValue("lblChildrenUnder3") + ""%>
                    <asp:Literal ID="ltr_comeDormono" runat="server" Visible="false"></asp:Literal>
                    <%= ltr_comeDormono.Text != "" ? "<br /><table>" + ltr_comeDormono.Text + "</table>" : ""%>
                </td>
                <td colspan="3" bgcolor="#a09ea1">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_n_settimane", "Settimane")%>:</b> 
                    <%= HF_weeks.Value%>
                </td>
                <td colspan="3" bgcolor="#a09ea1">
                    <b>
                        <%=CurrentSource.getSysLangValue("lblNights", "Notti")%>:</b> 
                    <%= HF_nights.Value%>
                </td>
                <td colspan="3" bgcolor="#a09ea1">
                    <b>
                        <%=CurrentSource.getSysLangValue("lblTotalNights", "Totale Notti")%>:</b> 
                        <%= HF_nightsTotal.Value%>
                </td>
            </tr>
            <tr>
                <td colspan="6" bgcolor="#e6e6e6">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Arrivo", "Arrivo")%>:</b>
                    <%= tblReservation.dtStart.formatCustom("#dd#/#mm#/#yy#", _currLang,"--/--/----")%>
                </td>
                <td colspan="6" bgcolor="#e6e6e6">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Partenza", "Partenza")%>:</b>
                    <%= tblReservation.dtEnd.formatCustom("#dd#/#mm#/#yy#", _currLang, "--/--/----")%>
                </td>
            </tr>
            <tr>
                <td colspan="6" bgcolor="#e6e6e6">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Aeroporto_Stazione_Porto", "Aeroporto/Stazione/Porto")%>:</b> 
                        <%= tblReservation.limo_inPoint_pickupPlaceName%>
                    <br />
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Orario_Arrivo", "Orario Arrivo")%>:</b> 
                    <%=tblReservation.limo_in_datetime.HasValue ? tblReservation.limo_in_datetime.Value.TimeOfDay.JSTime_toString(false, true) : "- - -"%>
                    <br />
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Numero_Volo_Treno", "N° Volo/Treno")%>:</b> 
                    <%= tblReservation.limo_inPoint_details%>
                </td>
                <td colspan="6" bgcolor="#e6e6e6">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Aeroporto_Stazione_Porto", "Aeroporto/Stazione/Porto")%>:</b>
                    <%= tblReservation.limo_outPoint_pickupPlaceName%>
                    <br />
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Orario_Partenza", "Orario Arrivo")%>:</b>
                    <%=tblReservation.limo_out_datetime.HasValue ? tblReservation.limo_out_datetime.Value.TimeOfDay.JSTime_toString(false, true) : ""%>
                    <br />
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Numero_Volo_Treno", "N° Volo/Treno")%>:</b>
                        <%= tblReservation.limo_outPoint_details%>
                </td>
            </tr>
            <tr>
                <td colspan="12" bgcolor="#e6e6e6">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_orario_arrivo_appartamento", "Orario d’arrivo all’appartamento")%>:</b> 
                        <%= !tblReservation.dtIn.HasValue ? tblReservation.dtStartTime.JSTime_stringToTime().JSTime_toString(false, true) : tblReservation.dtIn.Value.TimeOfDay.JSTime_toString(false, true) + (tblReservation.dtIn.Value.Date == tblReservation.dtStart.Value.Date ? "" : " " + tblReservation.dtIn.formatCustom("#dd#/#mm#/#yy#", _currLang, "--/--/----"))%>
                        <br />
                    <%=CurrentSource.getSysLangValue("pdf_info_orario_arrivo_appartamento", "Se non comunicato, il check-in verrà effettuato entro 1h30 dall’arrivo in aeroporto o 30 min. in stazione (non prima delle 11.30).")%>
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td colspan="12" bgcolor="#e6e6e6">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Mezzo_per_appartamento", "Mezzo per arrivare in appartamento")%>:</b><br />
                    <input type="checkbox" checked="checked" />
                    <%=CurrentSource.getSysLangValue("pdf_Taxi_Limo_Service", "Taxi / Limo Service")%>
                    &nbsp;&nbsp;
                    <!--Taxi / Limo Service -->
                    <input type="checkbox" />
                    <%=CurrentSource.getSysLangValue("pdf_Bus_Treno_Trasp_Pubblico", "Bus/Treno+Trasp. Pubblico")%>
                    &nbsp;&nbsp;
                    <!--Bus/Treno+Trasp._Pubblico-->
                    <input type="checkbox" />
                    <%=CurrentSource.getSysLangValue("pdf_Bus_Treno_Taxi", "Bus/Treno+Taxi")%>
                    &nbsp;&nbsp;
                    <!--Bus/Treno+Taxi -->
                    <input type="checkbox" />
                    <%=CurrentSource.getSysLangValue("pdf_Macchina", "Macchina(Propria o Noleggio)")%>
                    &nbsp;&nbsp;
                    <!--Macchina(Propria o Noleggio)-->
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <em>
                        <%=CurrentSource.getSysLangValue("pdf_according_to_italian_law17", "According to Italian Law n°17 of May 1993 and L.R. of August 5 1998, n°33.")%></em>
                </td>
            </tr>
        </table>
        <hr />
        <table width="960" cellpadding="4" cellspacing="3" border="0">
            <tr>
                <td colspan="12" bgcolor="#54495a" style="color: #FFF;">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_MODELLO_PAGAMENTI", "MODELLO PAGAMENTI")%></b>
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td colspan="10" bgcolor="#a09ea1" style="color: #fff;">
                    Venerdì 12/02/2011
                </td>
                <td colspan="2" bgcolor="#a09ea1" align="right" style="color: #fff;">
                    <b>50 €</b>
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td colspan="10" bgcolor="#a09ea1" style="color: #fff;">
                    Sabato 12/02/2011
                </td>
                <td colspan="2" bgcolor="#a09ea1" align="right" style="color: #fff;">
                    <b>80 €</b>
                </td>
            </tr>
            <%if (tblReservation.bcom_totalForOwner.objToDecimal() == 0)
                {%>
            <tr>
                <td colspan="10" bgcolor="#e6e6e6">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Totale_costo_ alloggio", "Costo Totale dell'alloggio")%></b>
                </td>
                <td colspan="2" bgcolor="#e6e6e6" align="right">
                    <b><%= (tblReservation.pr_reservation.objToDecimal() - tblReservation.pr_discount_owner.objToDecimal() - tblReservation.pr_discount_commission.objToDecimal()).ToString("N2")+"&nbsp;&euro;"%></b>
                </td>
            </tr>
            <%} %>
             <%if (tblReservation.pr_srsPrice.objToDecimal() > 0)
                {%>
             <tr>
                <td colspan="10" bgcolor="#e6e6e6">
                    <b>Welcome Service</b>
                </td>
                <td colspan="2" bgcolor="#e6e6e6" align="right">
                    <b>
                        <%= tblReservation.pr_srsPrice.objToDecimal().ToString("N2")+"&nbsp;&euro;"%></b>
                </td>
            </tr>
            <%} %>
            <%if (tblReservation.pr_ecoPrice.objToDecimal() > 0)
                {%>
             <tr>
                <td colspan="10" bgcolor="#e6e6e6">
                    <b>Cleaning Service X<%= tblReservation.pr_ecoCount.objToInt32()%></b>
                </td>
                <td colspan="2" bgcolor="#e6e6e6" align="right">
                    <b>
                        <%= tblReservation.pr_ecoPrice.objToDecimal().ToString("N2")+"&nbsp;&euro;"%></b>
                </td>
            </tr>
            <%} %>
            <tr>
                <td colspan="10" bgcolor="#e6e6e6">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Quota_Agenzia", "Quota Agenzia")%></b>
                </td>
                <td colspan="2" bgcolor="#e6e6e6" align="right">
                    <b>
                        <%= tblReservation.pr_part_agency_fee.objToDecimal().ToString("N2")+"&nbsp;&euro;"%></b>
                </td>
            </tr>
            <tr>
                <td colspan="10" bgcolor="#fe6634" style="color: #FFF; font-size: 13px;">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_TOTALE", "TOTALE")%></b>
                </td>
                <td colspan="2" bgcolor="#fe6634" align="right" style="color: #FFF; font-size: 13px;">
                    <b>
                        <%= (tblReservation.bcom_totalForOwner.objToDecimal() == 0?tblReservation.pr_total.objToDecimal():tblReservation.bcom_totalForOwner.objToDecimal()).ToString("N2")+"&nbsp;&euro;"%></b>
                </td>
            </tr>
        </table>
        <br />
        <table width="960" cellpadding="0" cellspacing="5" border="0">
            <asp:PlaceHolder ID="PH_payedNO" runat="server">
                <tr>
                    <td valign="top" colspan="6" style="color: #fe6634; font-size: 14px;">
                        <b>
                            <%=CurrentSource.getSysLangValue("pdf_Parte_pagamento_48_ore_dal", "Parte del pagamento entro 48 ore dal")%>
                            <%= tblReservation.dtCreation.formatCustom("#dd#/#mm#/#yy#", _currLang,"--/--/----")%>:</b>
                    </td>
                    <td colspan="4" align="right" style="font-size: 13px;">
                        <b>
                            <%= tblReservation.pr_part_payment_total.objToDecimal().ToString("N2")+"&nbsp;&euro;"%></b>
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="color: #fe6634; font-size: 14px;">
                        <b>
                            <%=CurrentSource.getSysLangValue("pdf_Saldo_da_versare_all_arrivo", "Saldo da versare all'arrivo")%> 
                            <%= tblReservation.dtStart.formatCustom("#dd#/#mm#/#yy#", _currLang, "--/--/----")%> 
                            <%= tblEstate.is_srs==1? CurrentSource.getSysLangValue("pdf_contanti_e_carta_di_credito", "cash or credit card"): CurrentSource.getSysLangValue("pdf_in_contanti", "cash")%>
                            <%= tblReservation.requestFullPayAccepted==1 && CommonUtilities.getSYS_SETTING("rntPayWithBankWithinDays").ToInt32()>0? "<br/>"+CurrentSource.getSysLangValue("pdf_saldo_con_bonifico_entro_xx_giorni").Replace("#xx#",""+CommonUtilities.getSYS_SETTING("rntPayWithBankWithinDays").ToInt32()):""%>
                            :</b>
                    </td>
                    <td colspan="4" align="right" style="font-size: 13px;">
                        <b>
                            <%= tblReservation.pr_part_owner.objToDecimal().ToString("N2")+"&nbsp;&euro;"%></b>
                    </td>
                </tr>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PH_payedOK" runat="server">
                <tr>
                    <td valign="top" colspan="6" style="color: #fe6634; font-size: 14px;">
                        <b><%= _currLang == 1 ? "Somma pagata:" : "Total Paid:"%></b>
                    </td>
                    <td colspan="4" align="right" style="font-size: 13px;">
                        <b>
                            <%= tblReservation.pr_total.objToDecimal().ToString("N2")+"&nbsp;&euro;"%></b>
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="color: #fe6634; font-size: 14px;">
                        <b><%=CurrentSource.getSysLangValue("pdf_Saldo_da_versare_all_arrivo", "Saldo da versare all'arrivo")%>:</b>
                    </td>
                    <td colspan="4" align="right" style="font-size: 13px;">
                        <b>0,00&nbsp;&euro;</b>
                    </td>
                </tr>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PH_payedCustom" runat="server">
                <asp:HiddenField ID="HF_prPayed" runat="server" Visible="false" />
                <asp:HiddenField ID="HF_prToPay" runat="server" Visible="false" />
                <tr>
                    <td valign="top" colspan="6" style="color: #fe6634; font-size: 14px;">
                        <b>
                            <%= _currLang == 1 ? "Somma pagata:" : "Total Paid:"%></b>
                    </td>
                    <td colspan="4" align="right" style="font-size: 13px;">
                        <b>
                            <%= HF_prPayed.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%></b>
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="color: #fe6634; font-size: 14px;">
                        <b>
                            <%=CurrentSource.getSysLangValue("pdf_Saldo_da_versare_all_arrivo", "Saldo da versare all'arrivo")%> 
                            <%= tblReservation.dtStart.formatCustom("#dd#/#mm#/#yy#", _currLang, "--/--/----")%>
                            <%= tblEstate.is_srs==1? CurrentSource.getSysLangValue("pdf_contanti_e_carta_di_credito", "cash or credit card"): CurrentSource.getSysLangValue("pdf_in_contanti", "cash")%>
                            <%= tblReservation.requestFullPayAccepted==1 && CommonUtilities.getSYS_SETTING("rntPayWithBankWithinDays").ToInt32()>0? "<br/>"+CurrentSource.getSysLangValue("pdf_saldo_con_bonifico_entro_xx_giorni").Replace("#xx#",""+CommonUtilities.getSYS_SETTING("rntPayWithBankWithinDays").ToInt32()):""%>
                            :</b>
                    </td>
                    <td colspan="4" align="right" style="font-size: 13px;">
                        <b>
                            <%= HF_prToPay.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%></b>
                    </td>
                </tr>
            </asp:PlaceHolder>
            <tr id="pnl_PayedPartPaymentDifference" runat="server">
                <td colspan="6" style="color: #fe6634; font-size: 14px;">
                    <b><%=CurrentSource.getSysLangValue("rntPayedPartPaymentDifference", "Already payed difference")%>:</b>
                    <br />
                    <em style="font-size: 11px"><%= CurrentSource.getSysLangValue("rntPayedPartPaymentDifferenceDescription")%></em>
                </td>
                <td colspan="4" align="right" style="font-size: 13px;">
                    <asp:HiddenField ID="HF_PayedPartPaymentDifference" runat="server" Value="0" Visible="false" />
                    <b>
                        <%= HF_PayedPartPaymentDifference.Value + "&nbsp;&euro;"%></b>
                </td>
            </tr>
            <tr id="pnl_deposit" runat="server">
                <td colspan="6" style="color: #fe6634; font-size: 14px;">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Deposito_Cauzionale", "Deposito Cauzionale da consegnare all’arrivo")%>
                        <%= tblReservation.dtStart.formatCustom("#dd#/#mm#/#yy#", _currLang,"--/--/----")%>:</b>
                    <br />
                    
                    <em style="font-size: 11px">
                        <%= tblEstate.pr_depositWithCard == 1 ? CurrentSource.getSysLangValue("rntCreditCardAsDamageDeposit", "Carta di credito") : CurrentSource.getSysLangValue("pdf_Cauzione_assegno", "Cauzione anche con assegno")%></em>
                </td>
                <td colspan="4" align="right" style="font-size: 13px;">
                    <b>
                        <%= tblReservation.pr_deposit.objToDecimal().ToString("N2")+"&nbsp;&euro;"%></b>
                </td>
            </tr>
            <tr id="pnl_overnight_tax" runat="server">
                <td valign="top" colspan="10">
                    <em>
                        <%=CurrentSource.getSysLangValue("pdf_Note_Contributo_di_soggiorno", "Nota: Il Comune di Roma a partire dal 01/01/11 ha applicato un contributo di soggiorno per un importo pari a 2 euro per persona per notte per un massimo di 10 giorni, ad eccezione di bambini fino a 10 anni di età, da versare in contanti il giorno dell'arrivo.")%>
                    </em>
                </td>
            </tr>
            <%if (tblEstate.isCedolareSecca == 1 && CurrentSource.getSysLangValue("voucherCedolareSecca") != "")
              { %>
            <tr>
                <td valign="top" colspan="10">
                    <em>
                        <%=CurrentSource.getSysLangValue("voucherCedolareSecca")%>
                    </em>
                </td>
            </tr>
            <%}%>
            <%if (!string.IsNullOrEmpty(tblReservation.notesClient))
              { %>
            <tr>
                <td valign="top" colspan="10">
                    <em>
                        <%=tblReservation.notesClient%>
                    </em>
                </td>
            </tr>
            <%}%>
        </table>
        <table width="960" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="10" style="font-size: 12px;">
                    <%=CurrentSource.getSysLangValue("pdf_note_affitto_mensili", "Per affitti di un mese o più, la tariffa mensile dovrà essere pagata all’inizio di ogni mese.")%>
                </td>
            </tr>
        </table>
        <hr />
        <table width="960" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="10" style="font-size: 13px;">
                    <b style="color: #333366; font-size: 14px;">
                        <%=CurrentSource.getSysLangValue("lblTermsAndConditions", "Termini e Condizioni")%></b>
                </td>
            </tr>
            <tr>
                <td colspan="10" style="font-size: 12px;">
                    <%//=CurrentSource.getSysLangValue("pdf_termini_1", "CANCELLARE UL E LI SOTTOSTANTI")%>
                    <div><asp:Literal ID="ltr_terms" runat="server"></asp:Literal></div>
                    
                </td>
            </tr>
        </table>
        <hr />
        <table width="960" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
            <tr>
                <td colspan="10" style="font-size: 13px;">
                    <b>
                        <%=CurrentSource.getSysLangValue("pdf_Modalità_di_Pagamento", "Modalità di Pagamento")%></b>
                </td>
            </tr>
            <tr>
                <td colspan="10" style="font-size: 12px;">
                    <%=CurrentSource.getSysLangValue("pdf_dati_pagamento", "DATI PAGAMENTO")%>
                </td>
            </tr>
        </table>
        <table width="960" cellpadding="20" cellspacing="0" border="0" style="margin-top: 25px;">
            <tr>
                <td style="background-color: #FE6634; color: #FFF; padding: 20px;" valign="middle" align="right">
                    <b>www.rentalinrome.com </b>
                    <br />
                    <b>+39 06 3220068 </b>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
