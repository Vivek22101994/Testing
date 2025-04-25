<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/areariservataprop/common/MP.Master" AutoEventWireup="true" CodeBehind="rnt_estate_calendar.aspx.cs" Inherits="RentalInRome.areariservataprop.rnt_estate_calendar" %>

<%@ Register Src="~/areariservataprop/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<%@ Register Src="~/areariservataprop/uc/UC_contactStaff.ascx" TagName="UC_contactStaff" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="" runat="server" />
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <h1 class="titolo_main" style="color: #FE6634;">Gestione prenotazioni e disponibilità della struttura:
            <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal></h1>
            <div id="fascia1">
                <div style="clear: both; margin: 3px 0 5px 30px;">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <div class="bottom_agg">
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="filt">
                            <div class="t">
                                <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
                            </div>
                            <div class="c">
                                <h1>Nuova disponibilità
                                </h1>
                                <div class="nulla">
                                </div>
                                <div class="filtro_cont">
                                    <table border="0" cellpadding="0" cellspacing="0" id="Table2">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 190px;">
                                                            <label>
                                                                Data check-in:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <input type="text" id="txt_dtStart" style="width: 120px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_dtStart" runat="server" />
                                                        </td>
                                                        <td style="width: 190px;">
                                                            <label>
                                                                Data check-out:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <input type="text" id="txt_dtEnd" style="width: 120px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_dtEnd" runat="server" />
                                                        </td>
                                                        <td style="width: 250px;">
                                                            <label>
                                                                Commento:</label>
                                                            <asp:TextBox ID="txt_subject" runat="server" CssClass="inp" Width="280px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:HiddenField ID="HF_resCount" runat="server" Value="0" Visible="false" />
                                                            <asp:HiddenField ID="HF_resCountYear" runat="server" Value="0" Visible="false" />
                                                            <asp:HiddenField ID="HF_ext_ownerdaysinyear" runat="server" Value="0" Visible="false" />
                                                            <asp:HiddenField ID="HF_resCount1" runat="server" Value="0" Visible="false" />
                                                            <asp:HiddenField ID="HF_resCountYear1" runat="server" Value="0" Visible="false" />
                                                            <asp:HiddenField ID="HF_resCount2" runat="server" Value="0" Visible="false" />
                                                            <asp:HiddenField ID="HF_resCountYear2" runat="server" Value="0" Visible="false" />
                                                            <label>
                                                                <%= "Avete preso " + HF_resCount.Value + " giorni per anno " + HF_resCountYear.Value+" rimangono altri " + (HF_ext_ownerdaysinyear.Value.ToInt32() - HF_resCount.Value.ToInt32())%>
                                                                <br />
                                                                <%= "Avete preso " + HF_resCount1.Value + " giorni per anno " + HF_resCountYear1.Value+" rimangono altri " + (HF_ext_ownerdaysinyear.Value.ToInt32() - HF_resCount1.Value.ToInt32())%>
                                                                <br />
                                                                <%= "Avete preso " + HF_resCount2.Value + " giorni per anno " + HF_resCountYear2.Value+" rimangono altri " + (HF_ext_ownerdaysinyear.Value.ToInt32() - HF_resCount2.Value.ToInt32())%>
                                                                <br />
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <label id="lblNewError" runat="server" visible="false">
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <asp:LinkButton ID="lnkNew" runat="server" CssClass="ricercaris" OnClick="lnkNew_Click">
                                                    <span>Crea</span>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="b">
                                <img src="images/filt_b1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_b2.gif" width="6" height="6" style="float: right" />
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="filt">
                            <div class="t">
                                <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
                            </div>
                            <div class="c">
                                <a class="filto_bottone" style="display: none;" id="lnk_showfilt" onclick="showFilter();">FILTRA</a>
                                <a class="filto_bottone2" id="lnk_hidefilt" onclick="hideFilter();">FILTRA</a>
                                <div class="filtro_cont">
                                    <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 180px;">
                                                            <label>
                                                                Stato Pren.:</label>
                                                            <asp:DropDownList runat="server" ID="drp_state_pid" Width="150px" CssClass="inp">
                                                            </asp:DropDownList>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Codice Pren.:</label>
                                                            <asp:TextBox ID="txt_code" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 190px;">
                                                            <label>
                                                                Data check-in:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtStart_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtStart_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtStart_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtStart_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_dtStart_from" runat="server" />
                                                            <asp:HiddenField ID="HF_dtStart_to" runat="server" />
                                                        </td>
                                                        <td style="width: 190px;">
                                                            <label>
                                                                Data check-out:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtEnd_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtEnd_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtEnd_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtEnd_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_dtEnd_from" runat="server" />
                                                            <asp:HiddenField ID="HF_dtEnd_to" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <asp:LinkButton ID="lnkFilter" runat="server" CssClass="ricercaris" OnClick="lnkFilter_Click">
                                                    <span>Filtra Risultati</span>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="b">
                                <img src="images/filt_b1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_b2.gif" width="6" height="6" style="float: right" />
                            </div>
                        </div>
                    </div>
                    <div style="clear: both">
                        <asp:HiddenField ID="HF_lds_filter" runat="server" Visible="false" />
                        <asp:HiddenField ID="HF_id_editAdmin" runat="server" Visible="false" />
                        <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TBL_RESERVATION" OrderBy="dtStart desc, dtStartTime desc">
                        </asp:LinqDataSource>
                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemDataBound="LV_ItemDataBound" OnItemCommand="LV_ItemCommand" OnPagePropertiesChanging="LV_PagePropertiesChanging">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("code")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# rntUtils.rntReservation_getStateName(Eval("state_pid").objToInt32()) + (Eval("state_pid").objToInt32() == 2 ? "&nbsp;(" + Eval("state_subject") + " - " + Eval("state_body") + ")" : "")%></span>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HL_getPdf" runat="server" Target="_blank"><span>Scarica Voucher</span></asp:HyperLink>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtStart")).formatITA(false) + " Ore " + ("" + Eval("dtStartTime")).JSTime_stringToTime().JSTime_toString(false, true)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtEnd")).formatITA(false) + " Ore " + ("" + Eval("dtEndTime")).JSTime_stringToTime().JSTime_toString(false, true)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (Eval("num_adult").objToInt32() + Eval("num_child_over").objToInt32()) + "&nbsp;(" + Eval("num_adult").objToInt32() + "+" + Eval("num_child_over").objToInt32() + ")&nbsp;+&nbsp;" + Eval("num_child_min").objToInt32()%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# "&euro;&nbsp;" + (Eval("bcom_totalForOwner").objToInt32()>0?Eval("bcom_totalForOwner").objToDecimal():Eval("pr_total").objToDecimal()).ToString("N2") + ""%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# "&euro;&nbsp;" + (Eval("pr_part_owner").objToDecimal() - (Eval("agentDiscountNotPayed").objToInt32()==1?0:Eval("agentCommissionPrice").objToDecimal())).ToString("N2") + ""%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pid_related_request" Visible="false" runat="server" Text='<%# Eval("pid_related_request") %>' />
                                        <asp:Label ID="lbl_pid_operator" Visible="false" runat="server" Text='<%# Eval("pid_operator") %>' />
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:Label ID="lbl_state" Visible="false" runat="server" Text='<%# Eval("state_pid") %>' />
                                        <asp:Label ID="lbl_dtStart" Visible="false" runat="server" Text='<%# ((DateTime)Eval("dtStart")).JSCal_dateToString() %>' />
                                        <asp:LinkButton ID="lnk_cancel" runat="server" CommandName="cancella" OnClientClick="return confirm('Sicuro di voler cancellare la prenotazione? \n\nDiventa disponibile per nuove prenotazioni.')" Style="margin-top: 6px; margin-right: 5px;">cancella</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("code")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# rntUtils.rntReservation_getStateName(Eval("state_pid").objToInt32()) + (Eval("state_pid").objToInt32() == 2 ? "&nbsp;(" + Eval("state_subject") + " - " + Eval("state_body") + ")" : "")%></span>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HL_getPdf" runat="server" Target="_blank"><span>Scarica Voucher</span></asp:HyperLink>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtStart")).formatITA(false) + " Ore " + ("" + Eval("dtStartTime")).JSTime_stringToTime().JSTime_toString(false, true)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtEnd")).formatITA(false) + " Ore " + ("" + Eval("dtEndTime")).JSTime_stringToTime().JSTime_toString(false, true)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (Eval("num_adult").objToInt32() + Eval("num_child_over").objToInt32()) + "&nbsp;(" + Eval("num_adult").objToInt32() + "+" + Eval("num_child_over").objToInt32() + ")&nbsp;+&nbsp;" + Eval("num_child_min").objToInt32()%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# "&euro;&nbsp;" + (Eval("bcom_totalForOwner").objToInt32()>0?Eval("bcom_totalForOwner").objToDecimal():Eval("pr_total").objToDecimal()).ToString("N2") + ""%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# "&euro;&nbsp;" + (Eval("pr_part_owner").objToDecimal() - (Eval("agentDiscountNotPayed").objToInt32()==1?0:Eval("agentCommissionPrice").objToDecimal())).ToString("N2") + ""%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_pid_related_request" Visible="false" runat="server" Text='<%# Eval("pid_related_request") %>' />
                                        <asp:Label ID="lbl_pid_operator" Visible="false" runat="server" Text='<%# Eval("pid_operator") %>' />
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:Label ID="lbl_state" Visible="false" runat="server" Text='<%# Eval("state_pid") %>' />
                                        <asp:Label ID="lbl_dtStart" Visible="false" runat="server" Text='<%# ((DateTime)Eval("dtStart")).JSCal_dateToString() %>' />
                                        <asp:LinkButton ID="lnk_cancel" runat="server" CommandName="cancella" OnClientClick="return confirm('Sicuro di voler cancellare la prenotazione? \n\nDiventa disponibile per nuove prenotazioni.')" Style="margin-top: 6px; margin-right: 5px;">cancella</asp:LinkButton>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table id="Table1" runat="server" style="">
                                    <tr>
                                        <td>No data was returned.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div class="page">
                                    <asp:DataPager ID="DataPager2" runat="server" PageSize="50" style="border-right: medium none;">
                                        <Fields>
                                            <asp:NumericPagerField ButtonCount="20" />
                                        </Fields>
                                    </asp:DataPager>
                                    <asp:Label ID="lbl_record_count_top" runat="server" CssClass="total" Text=""></asp:Label>
                                    <div class="nulla">
                                    </div>
                                </div>
                                <div class="table_fascia">
                                    <table border="0" cellpadding="0" cellspacing="0" style="">
                                        <tr>
                                            <th style="text-align: center;">Codice
                                            </th>
                                            <th>Stato
                                            </th>
                                            <th>Stato
                                            </th>
                                            <th>Data Check-In
                                            </th>
                                            <th>Data Check-Out
                                            </th>
                                            <th>Persone
                                            </th>
                                            <th>Totale
                                            </th>
                                            <th>Saldo
                                            </th>
                                            <th></th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server" />
                                    </table>
                                </div>
                                <div class="page">
                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;">
                                        <Fields>
                                            <asp:NumericPagerField ButtonCount="20" />
                                        </Fields>
                                    </asp:DataPager>
                                    <asp:Label ID="lbl_record_count" runat="server" CssClass="total" Text=""></asp:Label>
                                </div>
                            </LayoutTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Panel ID="pnlContent" runat="server" Width="100%" Visible="false">
                <!-- INIZIO MAIN LINE -->
                <div class="mainline">
                    <!-- BOX 1 -->
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                            </div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                            </div>
                        </div>
                        <div class="center">
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td class="td_title">Prezzo (2pax):
                                        </td>
                                        <td>&euro;&nbsp;<asp:TextBox ID="txt_price_1" runat="server" Style="width: 80px"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txt_price_1" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txt_price_1" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td class="td_title">Letto aggiuntivo (+1pax):
                                        </td>
                                        <td>&euro;&nbsp;<asp:TextBox ID="txt_price_optional_1" runat="server" Style="width: 80px"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txt_price_optional_1" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txt_price_optional_1" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <div class="salvataggio">
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click" CausesValidation="true" TabIndex="28" ValidationGroup="price"><span>Salva Modifiche</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla</span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <span class="titoloboxmodulo" id="lbl_changeSaved" runat="server" visible="false">Le modifiche sono state salvate...</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="bottom">
                            <div style="float: left;">
                                <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                            </div>
                            <div style="float: right;">
                                <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                            </div>
                        </div>
                    </div>
                    <div class="salvataggio">
                        <div class="nulla">
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <script type="text/javascript">
                var cal_dtStart_from;
                var cal_dtStart_to;

                var cal_dtEnd_from;
                var cal_dtEnd_to;

                var _JSCal_Range_;

                var cal_state_date_from;
                var cal_state_date_to;
                function setCal() {
                    cal_dtStart_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtStart_from.ClientID %>", View: "#txt_dtStart_from", Cleaner: "#del_dtStart_from", changeMonth: true, changeYear: true });
                    cal_dtStart_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtStart_to.ClientID %>", View: "#txt_dtStart_to", Cleaner: "#del_dtStart_to", changeMonth: true, changeYear: true });

                    cal_dtEnd_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtEnd_from.ClientID %>", View: "#txt_dtEnd_from", Cleaner: "#del_dtEnd_from", changeMonth: true, changeYear: true });
                    cal_dtEnd_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtEnd_to.ClientID %>", View: "#txt_dtEnd_to", Cleaner: "#del_dtEnd_to", changeMonth: true, changeYear: true });

                    _JSCal_Range_ = new JSCal.Range({ dtFormat: "d MM yy", countMin: 1, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates });
                    //cal_dtStart = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtStart.ClientID %>", View: "#txt_dtStart", Cleaner: "#del_dtStart", changeMonth: true, changeYear: true });
                    //cal_dtEnd = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtEnd.ClientID %>", View: "#txt_dtEnd", Cleaner: "#del_dtEnd", changeMonth: true, changeYear: true });
                }
            </script>
            <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
