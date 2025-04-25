<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_sx.ascx.cs" Inherits="RentalInRome.reservationarea.UC_sx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:HiddenField ID="HF_unique" runat="server" Visible="false" Value="" />
<asp:HiddenField ID="HF_code" runat="server" Visible="false" Value="0" />
<asp:HiddenField ID="HF_dtStart" runat="server" Visible="false" Value="1" />
<asp:HiddenField ID="HF_dtEnd" runat="server" Visible="false" Value="0" />
<asp:HiddenField ID="HF_num_adult" runat="server" Visible="false" Value="0" />
<asp:HiddenField ID="HF_num_child_over" runat="server" Visible="false" Value="0" />
<asp:HiddenField ID="HF_num_child_min" runat="server" Visible="false" Value="0" />
<asp:HiddenField ID="HF_pr_total" runat="server" Value="0" />
<asp:HiddenField ID="HF_pr_deposit" runat="server" Value="0" />
<asp:HiddenField ID="HF_pr_part_commission_tf" runat="server" Value="0" />
<asp:HiddenField ID="HF_pr_part_commission_total" runat="server" Value="0" />
<asp:HiddenField ID="HF_pr_part_agency_fee" runat="server" Value="0" />
<asp:HiddenField ID="HF_pr_part_payment_total" runat="server" Value="0" />
<asp:HiddenField ID="HF_pr_part_owner" runat="server" Value="0" />
<asp:HiddenField ID="HF_visa_isRequested" runat="server" Visible="false" Value="0" />
<asp:HiddenField ID="HiddenField1" runat="server" Visible="false" Value="0" />
<asp:Literal runat="server" ID="ltr_unique_id" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="ltr_cl_name_full" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="ltr_cl_name_honorific" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="ltr_est_code" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="ltr_est_address" Visible="false"></asp:Literal>
<div class="colYourRes">
    <div class="lineYourRes">
        <span><%=CurrentSource.getSysLangValue("pdf_Codice_di_prenotazione")%>:</span>
        <strong><%= HF_code.Value%></strong>
    </div>
    <div class="lineYourRes">
        <span><%=CurrentSource.getSysLangValue("pdf_Firmatario")%>:</span>
        <strong>
            <%= (ltr_cl_name_honorific.Text != "") ? ltr_cl_name_honorific.Text + "&nbsp;" : ""%>
            <%= ltr_cl_name_full.Text%></strong>
    </div>
    <div class="lineYourRes">
        <strong style="float: left; width: 100%; margin-bottom: 8px;"><%=CurrentSource.getSysLangValue("pdf_Nome_appartamento")%></strong>
        <strong style="float: left; font-size: 13px; color: #ff6a00;"><%= ltr_est_code.Text%></strong>
        <div class="nulla">
        </div>
        <span id="pnl_address" runat="server"><%= ltr_est_address.Text%></span>
    </div>
    <div class="lineYourRes">
        <span><%=CurrentSource.getSysLangValue("reqCheckInDate")%>:</span>
        <strong><%= HF_dtStart.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
        <div class="nulla">
        </div>
        <span><%=CurrentSource.getSysLangValue("reqCheckOutDate")%>:</span>
        <strong><%= HF_dtEnd.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
    </div>
    <div class="lineYourRes">
        <span><%=CurrentSource.getSysLangValue("lblNumPersons")%>:</span>
        <strong>
            <%= (HF_num_adult.Value.ToInt32() + HF_num_child_over.Value.ToInt32() + HF_num_child_min.Value.ToInt32())%>
            <%=CurrentSource.getSysLangValue("lblGuest")%>
        </strong>
        <br />
        <em>
            <%= HF_num_adult.Value + " " + CurrentSource.getSysLangValue("reqAdults") + " + " + "<br/>" + HF_num_child_over.Value + " " + CurrentSource.getSysLangValue("lblChildren3OrOver") + " + " + "<br/>" + HF_num_child_min.Value + " " +  CurrentSource.getSysLangValue("lblChildrenUnder3") + ""%>
        </em>
    </div>
    <div id="Div1" class="agent_request_pdf" runat="server" visible="false">
        <span style="font-size: 10px; color: #9797ac; margin-bottom: 0;">
            <%=CurrentSource.getSysLangValue("pdf_Booking_Agent")%>: </span>
        <strong style="font-size: 14px">Michael</strong><br />
        <a href="mailto:michael.smith@rentalinrome.com">michael.smith@rentalinrome.com</a><br />
        Fax +39 06 2332 8717
    </div>
    <div class="nulla">
    </div>
</div>
<div class="infostatoprenotazione" id="pnl_notComplete" runat="server" visible="false" style="clear: both; width: 232px; margin: 0;">
    <asp:HiddenField ID="HF_expiresInMinutes" runat="server" Value="0" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var _timerCont = 20;
            function doTimer() {
                _timerCont += 10;
                setTimeout("doTimer()", 10000);
            }
            function checkExpires() {
                var _expiresInMinutesCont = $("#<%=HF_expiresInMinutes.ClientID%>");
            var _hoursCont = $("#<%=lbl_hours.ClientID%>");
            var _minutesCont = $("#<%=lbl_minutes.ClientID%>");
            var _expiresInMinutes = parseInt(_expiresInMinutesCont.val(), 10) - _timerCont / 60;
            if (_expiresInMinutes < 0) {
                _hoursCont.val("00");
                _minutesCont.val("00");
                return;
            }
            var _hours = Math.floor(_expiresInMinutes / 60);
            var _minutes = Math.floor(_expiresInMinutes % 60);

            _hoursCont.html(_hours);
            _minutesCont.html(_minutes);
            setTimeout("checkExpires()", 3000);
        }
        checkExpires();
        doTimer();
        </script>
    </telerik:RadCodeBlock>
    <div class="ico_no">
        <%=CurrentSource.getSysLangValue("resBookingNotComplete")%>
    </div>
    <div id="Div2" class="ico_si" runat="server" visible="false">
        <%=CurrentSource.getSysLangValue("resBookingIsComplete")%>
    </div>
    <div class="time">
        <div class="tx">
            <%=CurrentSource.getSysLangValue("resYouBookingRequestExpires")%>:
        </div>
        <div class="timeline">
            <div>
                <strong id="lbl_hours" runat="server">96</strong>
                <div class="oremin" style="margin-right: 7px;">
                    <%=CurrentSource.getSysLangValue("lblHours")%>
                </div>
                <strong id="lbl_minutes" runat="server">00</strong>
                <div class="oremin">
                    <%=CurrentSource.getSysLangValue("lblMinutes")%>
                </div>
            </div>
        </div>
        <div class="nulla"></div>
    </div>
</div>

<!-- TESTO ALLERT DATI DA MANCANTI DA INSERIRE -->
<div class="txallert_menu" style="display: none;">
    <%=CurrentSource.getSysLangValue("lblAllertCompleteInfo")%>
</div>


<div class="menu_area_reservation_details" style="display: none;">
    <a href="payment.aspx" id="hl_payment" runat="server" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-payment-summary.gif);">
        <%=CurrentSource.getSysLangValue("lblPaymentSummary")%>
    </a>
    <a href="personaldata.aspx" id="hl_personaldata" runat="server" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-personal-data.gif);">
        <%=CurrentSource.getSysLangValue("lblPersonalData")%>
    </a>
    <a href="agentclientdata.aspx" id="hl_agentclientdata" runat="server" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-personal-data.gif);">Your Client details
    </a>
    <a href="arrivaldeparture.aspx" id="hl_arrivaldeparture" runat="server" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-arrival-departure.gif);">
        <%=CurrentSource.getSysLangValue("lblArrivalAndDeparture")%>
    </a>
    <a href="bedselection.aspx" id="hl_bedselection" runat="server" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-bed-selection.gif);">
        <%=CurrentSource.getSysLangValue("lblBedSelection")%> 
    </a>
    <a href="pdf.aspx" id="hl_pdf" runat="server" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-voucher-invoice.gif);">
        <%=CurrentSource.getSysLangValue("lblVoucher")%>/<%=CurrentSource.getSysLangValue("lblInvoice")%>
    </a>
    <a href="pickupservice.aspx" id="hl_pickupservice" runat="server" visible="false" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-pickup-service.gif);">Pickup service
    </a>
    <a href="transfer.aspx" id="hl_transfer" runat="server" visible="false" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-pickup-service.gif);">Pickup service </a>
    <asp:HyperLink ID="HL_visa_isRequested" runat="server" NavigateUrl="~/reservationarea/requestvisa.aspx" CssClass="resDtlBtn" Style="background-image: url(/images/css/btn-reservation-visa-info.gif);">
        Visa Information
    </asp:HyperLink>
    <a href="mailto:info@rentalinrome.com?subject=reservationarea code:<%= RentalInRome.reservationarea.resUtils.CurrentIdReservation_gl %>" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-contacts.gif);">
        <%=CurrentSource.getSysLangValue("lblContacts")%>
    </a>
    <a href="partnerservices.aspx" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-partners.gif);">
        <%=CurrentSource.getSysLangValue("lblPartnerServices")%>
    </a>
</div>
<div class="nulla">
</div>
<div class="menu_area_reservation_details" id="pnl_visa_isRequested" runat="server" style="text-align: center;">
    <div class="lineYourRes">
        <table>
            <tr>
                <td style="width: 160px;">
                    <strong style="font-size: 14px;"><%=CurrentSource.getSysLangValue("lblneedVisa")%> </strong>
                </td>
                <td>
                    <asp:DropDownList Style="width: 50px; font-size: 11px;" ID="drp_visa_isRequested" runat="server" onchange="$('.pnl_visa_isRequested').css('display',(this.value=='1'?'':'none'))">
                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                        <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="pnl_visa_isRequested" style="display: none;">
                <td>
                    For how many persons?
                </td>
                <td>
                    <asp:DropDownList Style="width: 50px; font-size: 11px;" ID="drp_visa_persons" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="pnl_visa_isRequested" style="display: none;">
                <td colspan="2">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnk_changeMode_Click" CommandArgument="visa_new">
                                                <span>continue</span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
</div>
<div id="thawteseal" style="text-align: center;" title="Click to Verify
- This site chose Thawte SSL for secure e-commerce and confidential
communications.">
    <div>
        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <span id="siteseal"><script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=KQsv4yBM0BqPJHlzehZDvQMMOuVk7bQKurfWOSZRP9jcl3REyDbv0elwOgp0"></script></span>
        </telerik:RadCodeBlock>
    </div>
    <div>
        <a href="https://www.godaddy.com/" target="_blank" style="color: #000000; text-decoration: none; font: bold 10px
arial,sans-serif; margin: 0px; padding: 0px;">ABOUT SSL CERTIFICATES</a>
    </div>
</div>
