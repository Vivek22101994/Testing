<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBooking.ascx.cs" Inherits="RentalInRome.uc.ucBooking" %>
<%@ Register Src="UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
        <asp:HiddenField ID="HF_IdEstate" runat="server" Value="0" />
        <asp:Literal ID="ltr_sel_priceDetails" runat="server" Visible="false"></asp:Literal>
        <asp:Literal ID="ltr_viewBookNow" runat="server" Visible="false"></asp:Literal>
        <asp:HiddenField ID="HF_tmp_prTotal" runat="server" Value="0" />
        <input type="hidden" id="RNT_alternativeEstate_minPrice" value="<%= HF_tmp_prTotal.Value %>" />
        <script type="text/javascript">
            function show_formBookignCont() {
                $('#formBookignCont').show();
                $('#tmp_focus').focus();
            }
        </script>
        <div id="prices">
            <h3 class="titBar" id="priceBar">
                <%= CurrentSource.getSysLangValue("lblPrices")%></h3>
            <div class="boxStep" style="margin-left: 20px;">
                <span class="numStep"><strong>1.</strong><%= CurrentSource.getSysLangValue("lblPeriod")%></span>
                <table class="selPeriod" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="middle" align="left">
                            <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" />
                            <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                        </td>
                        <td>
                            <a class="calendario" id="startCalTrigger_<%= Unique %>"></a>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" align="left">
                            <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" />
                            <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                        </td>
                        <td>
                            <a class="calendario" id="endCalTrigger_<%= Unique %>"></a>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" align="left">
                            <%= CurrentSource.getSysLangValue("lblNights")%>:
                            <asp:TextBox ID="txt_dtCount" runat="server" Style="width: 30px;"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td valign="middle" colspan="2">
                            <table>
                                <tr>
                                    <td style="padding-top: 0;">
                                        <span class="rntCal nd_f" style="display: block; margin: 0; position: relative; height: 10px; width: 15px; margin-right: 5px;"></span>
                                    </td>
                                    <td style="padding-top: 0; font-size: 11px;">
                                        <%= CurrentSource.getSysLangValue("lblNotAvailable")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 0;">
                                        <span class="rntCal sel_f" style="display: block; margin: 0; position: relative; height: 10px; width: 15px; margin-right: 5px;"></span>
                                    </td>
                                    <td style="padding-top: 0; font-size: 11px;">
                                        <%= CurrentSource.getSysLangValue("lblDateSelected")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <script type="text/javascript">
                    var RNT_estateCheckAvvRequest;
                    function RNT_estateCheckAvvReset() {
                        window.setTimeout(function(){RNT_estateCheckAvvRequest = false;},100);
                    }
                    function RNT_estateCheckAvv() {
                        if(RNT_estateCheckAvvRequest){ return;}
                        RNT_estateCheckAvvRequest = true;
                        __doPostBack("<%= lnk_calculatePrice.ClientID %>", "calculatePrice");
                    }
                    function RNT_calculateNumPersons() {
                        if(RNT_estateCheckAvvRequest){ return;}
                        RNT_estateCheckAvvRequest = true;
                        __doPostBack("<%= lnk_calculatePrice.ClientID %>", "calculateNumPersons");
                    }
                    var _JSCal_Range_<%= Unique %>;
                    function setCal_<%= Unique %>(dtStartInt, dtEndInt) {
                        _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", countMin: <%=currEstateTB.nights_min %>, minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", countCont: "#<%= txt_dtCount.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> , callBack: function(){RNT_estateCheckAvv();} });
                    }
                    <%= ltr_checkCalDates.Text %>
                </script>
                <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
            </div>
            <div class="boxStep" style="width: 260px;" id="pnlGuestInfo" runat="server">
                <span class="numStep"><strong>2.</strong>
                    <%= CurrentSource.getSysLangValue("lblGuestInformation")%></span>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="guestInfo" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="middle" align="left" style="width: 50px;">
                                    <asp:DropDownList ID="drp_adult" runat="server" onchange="RNT_calculateNumPersons()">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%= CurrentSource.getSysLangValue("reqAdults")%>
                                </td>
                            </tr>
                            <tr id="pnl_child_over" runat="server">
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_child_over" runat="server" onchange="RNT_estateCheckAvv()">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%= CurrentSource.getSysLangValue("lblChildren3OrOver")%>
                                </td>
                            </tr>
                            <tr id="pnl_child_min" runat="server">
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_child_min" runat="server" onchange="RNT_estateCheckAvv()">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%= CurrentSource.getSysLangValue("lblChildrenUnder3")%>
                                </td>
                            </tr>
                            <tr id="pnlChildrenNotAllowed" runat="server" visible="false">
                                <td colspan="2">
                                    <span class="alert warning"><%= CurrentSource.getSysLangValue("lblChildrenNotAllowed")%></span>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="boxStep" style="display: none;">
                <span class="numStep"><strong>3.</strong>
                    <%= CurrentSource.getSysLangValue("lblExtra")%></span>
                <table class="extra" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="middle" align="left" style="width: 17px;">
                            <input name="extra1" type="checkbox" value="" />
                        </td>
                        <td>25€ Posto auto (al giorno)
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" align="left">
                            <input name="extra2" type="checkbox" value="" />
                        </td>
                        <td>50€ Auto Eletrica
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: right; width: 360px; position: relative;">
                <div id="promoCode" style="float: left;">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle" align="left" colspan="2">
                                <span>
                                    <%=CurrentSource.getSysLangValue("lblPromotionalCode")%>:</span>
                                <a class="infoBtn ico_tooltip" title="div_cms" style="float: left;"></a>
                                <div id="tooltip_div_cms" style="display: none;">
                                    <%=CurrentSource.getSysLangValue("lblDescPromotionalCode")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" style="width: 120px;">
                                <asp:TextBox ID="txt_promotionalCode" runat="server"></asp:TextBox>
                            </td>
                            <td valign="middle" align="left">
                                <a href="#" onclick="bookingForm_checkPromoCode(); return false;" class="btnGo"></a>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" colspan="2">
                                <span id="lb_promotionalCode_error" style="display: none;"></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:LinkButton ID="lnk_calculatePrice" runat="server" CssClass="btnCalcola" OnClick="lnk_calculatePrice_Click" OnClientClick="RNT_estateCheckAvv(); return false;" Style="display: none;">
					<span>
						<%= CurrentSource.getSysLangValue("lblCalculatePrice")%>
					</span>
                </asp:LinkButton>

                <script type="text/javascript">
                    function bookingForm_checkPromoCode() {
                        SITE_showLoader();
                        var _url = "/webservice/rntDiscountPromoCode.aspx";
                        _url += "?promocode=" + $("#<%= txt_promotionalCode.ClientID %>").val();
                        var _xml = $.ajax({
                            type: "GET",
                            url: _url,
                            dataType: "html",
                            success: function (html) {
                                $("#commentListCont").html(html);
                                $("#lb_promotionalCode_error").show();
                                if (html=="") {$("#lb_promotionalCode_error").html('<%= CurrentSource.getSysLangValue("lblInvalidCode")%>');SITE_hideLoader();}
                                else  {$("#lb_promotionalCode_error").html("- "+html+"%");RNT_estateCheckAvv();}
                                
                            }
                        });
                    }
                </script>
                <div class="nulla">
                </div>
                <div id="pnl_mr_rental_yousave" runat="server" visible="false" class="mr_rental_yousave">
                    <span class="period"><%= contUtils.getLabel("lblInthisPeriod")%></span>
                    <span class="yousave"><%= contUtils.getLabel("lblYouAreSaving")%></span>
                    <span class="savingprice"><%=(currOutPrice.prDiscountSpecialOffer + currOutPrice.prDiscountLongStay).ToString("N2") + "&euro;"%></span>
                </div>
            </div>
        </div>

        <div id="yourBookingPrice" runat="server" visible="false" style="float: left; margin-bottom: 15px; width: 862px;">
            <h3 class="titBar">
                <%= CurrentSource.getSysLangValue("lblYourBooking")%>
            </h3>
            <asp:PlaceHolder ID="PH_bookingPrices" runat="server">
                <div class="boxStep" style="width: 300px;">
                    <table id="infoBookPrice" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle" align="left" class="col1">
                                <%= CurrentSource.getSysLangValue("reqCheckInDate")%>
                            </td>
                            <td valign="middle" align="left" class="col2">
                                <span id="lb_sel_dtStart">
                                    <%= curr_ls.dtStart.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" class="col1">
                                <%= CurrentSource.getSysLangValue("reqCheckOutDate")%>
                            </td>
                            <td valign="middle" align="left" class="col2">
                                <span id="lb_sel_dtEnd">
                                    <%= curr_ls.dtEnd.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" class="col1">
                                <%= CurrentSource.getSysLangValue("lblTotalNights")%>
                            </td>
                            <td valign="middle" align="left" class="col2">
                                <span id="lb_sel_dtCount">
                                    <%= curr_ls.dtCount%></span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" class="col1">
                                <%= CurrentSource.getSysLangValue("lblPax")%>
                            </td>
                            <td valign="middle" align="left" class="col2">
                                <span id="lb_sel_personsCount">
                                    <%= currEstateTB.is_chidren_allowed == 1 ? curr_ls.numPers_adult + " Adults, " + curr_ls.numPers_childOver + " Children, " + curr_ls.numPers_childMin + " Infants" : curr_ls.numPers_adult + " Adults"%></span>
                            </td>
                        </tr>
                    </table>
                    <asp:PlaceHolder ID="PH_priceDetails" runat="server">
                        <a class="dettCostGior ico_tooltip_right" title="div_dett_costo">
                            <span>
                                <%= CurrentSource.getSysLangValue("lblDetailsDailyCosts")%></span>
                        </a>
                        <div id="tooltip_div_dett_costo" style="display: none;">
                            <div class="box_dett_day">
                                <strong>
                                    <%= CurrentSource.getSysLangValue("lblDetailsDailyCosts")%></strong>
                                <span id="lb_sel_personsCount_dett">
                                    <%=curr_ls.numPers_adult + " Adults, " + curr_ls.numPers_childOver + " Children, " + curr_ls.numPers_childMin + " Infants"%></span>
                            </div>
                            <%=ltr_sel_priceDetails.Text%>
                        </div>
                        <asp:Literal ID="ltrAgentDiscount_layoutTemplate" runat="server" Visible="false">
                            <table class="infoComm1" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" align="left">
                                        #mese#:
                                    </td>
                                    <td valign="middle" align="left">
                                        <strong>#currMonth#</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left">
                                        #currSumLabel#:
                                    </td>
                                    <td valign="middle" align="left">
                                        &euro;&nbsp;<strong style="color: #fe6634;">#currSum#</strong>*
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        *#currSumDesc#
                                    </td>
                                </tr>
                            </table>
                            <table class="infoComm2" cellpadding="0" cellspacing="0">
                                <tr>
                                    <th colspan="2" valign="middle" align="left">#lblCommissions#: </th>
                                </tr>
                                #discountList#
                            </table>
                        </asp:Literal>
                        <asp:Literal ID="ltrAgentDiscount_itemTemplate" runat="server" Visible="false">
                            <tr class="#cssClass#">
                                <td valign="middle" align="left" class="colSn">
                                    #toChange#
                                </td>
                                <td valign="middle" align="right" class="colDs">
                                    #discount#%
                                </td>
                            </tr>
                        </asp:Literal>
                        <div id="tooltip_info_commissione" style="display: none;">
                            <% 
                                string AgentDiscount_layoutTemplate = ltrAgentDiscount_layoutTemplate.Text.Replace("#mese#", CurrentSource.getSysLangValue("lblMonth")).Replace("#currSumLabel#", CurrentSource.getSysLangValue("lblReservationsTotalSum")).Replace("#currSumDesc#", CurrentSource.getSysLangValue("lblComprensivoDiQuestaPren")).Replace("#lblCommissions#", CurrentSource.getSysLangValue("lblCommissions"));
                                string AgentDiscount_uniqueTemplate = ltrAgentDiscount_itemTemplate.Text.Replace("#toChange#", CurrentSource.getSysLangValue("lblFixedDiscount"));
                                string AgentDiscount_startTemplate = ltrAgentDiscount_itemTemplate.Text.Replace("#toChange#", CurrentSource.getSysLangValue("lblUpTo") + " #end#");
                                string AgentDiscount_middleTemplate = ltrAgentDiscount_itemTemplate.Text.Replace("#toChange#", CurrentSource.getSysLangValue("lblFrom") + " #start# " + CurrentSource.getSysLangValue("lblTo") + " #end#");
                                string AgentDiscount_endTemplate = ltrAgentDiscount_itemTemplate.Text.Replace("#toChange#", CurrentSource.getSysLangValue("lblFrom") + " #start#"); 
                            %>
                            <%= rntUtils.getDiscountType_details(currOutPrice.agentDiscountType, currOutPrice.agentTotalResPrice, currOutPrice.agentCheckDate, CurrentLang.ID, "tuaComm", AgentDiscount_uniqueTemplate, AgentDiscount_startTemplate, AgentDiscount_middleTemplate, AgentDiscount_endTemplate, AgentDiscount_layoutTemplate)%>
                        </div>
                    </asp:PlaceHolder>
                </div>
                <div id="totale">
                    <asp:PlaceHolder ID="PH_bookPriceOK" runat="server">
                        <div class="totalebox">
                            <div>
                                <asp:Panel ID="pnl_dicountCont" runat="server" CssClass="sconto_div" Visible="false">
                                    <table cellpadding="0" cellspacing="0" width="275px">
                                        <tr>
                                            <td valign="middle" align="right">
                                                <%= CurrentSource.getSysLangValue("lblPrice")%>
                                            </td>
                                            <td valign="middle" align="right">
                                                <strong class="price1">
                                                    <%=(currOutPrice.prTotalRate).ToString("N2")%>
                                                    €</strong>
                                            </td>
                                        </tr>
                                        <%if (currOutPrice.pr_part_agency_fee > 0)
                                          {%>
                                        <tr>
                                            <td valign="middle" align="right">
                                                <%= CurrentSource.getSysLangValue("rnt_pr_part_agency_fee")%>
                                            </td>
                                            <td valign="middle" align="right">
                                                <strong class="price1">
                                                    <%=currOutPrice.pr_part_agency_fee.ToString("N2") + "&nbsp;&euro;"%>
                                                </strong>
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (currOutPrice.pr_discount_commission > 0)
                                          {%>
                                        <tr>
                                            <td valign="middle" align="right">
                                                <span class="lbl" style="float: right;">Promo Discount
                                                </span>
                                            </td>
                                            <td valign="middle" align="right">
                                                <strong class="price1">
                                                    <%="- " + currOutPrice.pr_discount_commission.ToString("N2") + "&nbsp;&euro;"%>
                                                </strong>
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (currOutPrice.prDiscountSpecialOffer > 0)
                                          {%>
                                        <tr>
                                            <td valign="middle" align="right">
                                                <span class="lbl" style="float: right;">
                                                    <%= contUtils.getLabel("lblDiscountSpecialOffer")%>
                                                </span>
                                            </td>
                                            <td valign="middle" align="right">
                                                <strong class="price1">
                                                    <%="- " + currOutPrice.prDiscountSpecialOffer.ToString("N2") + "&nbsp;&euro;"%>
                                                </strong>
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (currOutPrice.prDiscountLongStay > 0)
                                          {%>
                                        <tr>
                                            <td valign="middle" align="right">
                                                <span class="lbl" style="float: right;">
                                                    <%= contUtils.getLabel("lblDiscountLongStay")%>
                                                </span>
                                                <a class="infoBtn ico_tooltip" title="<%= currOutPrice.prDiscountLongStayDesc%>" style="float: right; margin-right: 4px;"></a>
                                            </td>
                                            <td valign="middle" align="right">
                                                <strong class="price1">
                                                    <%="- " + currOutPrice.prDiscountLongStay.ToString("N2") + "&nbsp;&euro;"%>
                                                </strong>
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (currOutPrice.srsPrice > 0)
                                          {%>
                                        <tr>
                                            <td valign="middle" align="right">Welcome Service
                                            </td>
                                            <td valign="middle" align="right">
                                                <strong class="price1">
                                                    <%=currOutPrice.srsPrice + "&nbsp;&euro;"%>
                                                </strong>
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (currOutPrice.ecoPrice > 0)
                                          {%>
                                        <tr>
                                            <td valign="middle" align="right">Cleaning Service X<%= currOutPrice.ecoCount%>
                                            </td>
                                            <td valign="middle" align="right">
                                                <strong class="price1">
                                                    <%=currOutPrice.ecoPrice + "&nbsp;&euro;"%>
                                                </strong>
                                            </td>
                                        </tr>
                                        <%} %>
                                    </table>
                                    <div class="nulla">
                                    </div>
                                </asp:Panel>
                                <div class="total_div">
                                    <table cellpadding="0" cellspacing="0" width="275px">
                                        <tr>
                                            <td valign="middle" align="right" colspan="2" class="total">
                                                <%= CurrentSource.getSysLangValue("lblTotalPrice")%>: <strong>
                                                    <%=currOutPrice.prTotal.ToString("N2")%>
                                                    €</strong>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="nulla">
                                    </div>
                                </div>
                                <div class="anticiposaldo_div">
                                    <table cellspacing="0" cellpadding="0">
                                        <%if (currOutPrice.agentCommissionPrice > 0)
                                          {%>
                                        <tr>
                                            <td valign="middle" align="right">Agency Discount
                                            </td>
                                            <td valign="middle" align="right">
                                                <strong class="price1">
                                                    <%=currOutPrice.agentCommissionPrice.ToString("N2") + "&nbsp;&euro;&nbsp;" + (currOutPrice.agentDiscountNotPayed==0?"Compreso nel totale":"Scalato dal totale")%>
                                                </strong>
                                            </td>
                                        </tr>
                                        <%} %>
                                        <tr>
                                            <td valign="middle" align="right">
                                                <%= CurrentSource.getSysLangValue("lblAnticipo")%>
                                            </td>
                                            <td valign="middle" align="right">
                                                <strong class="price1">
                                                    <%=currOutPrice.pr_part_payment_total.ToString("N2")%>
                                                    € </strong>
                                            </td>
                                        </tr>
                                        <% if (currOutPrice.pr_part_agency_fee != 0)
                                           { %>
                                        <tr style="height: 20px;">
                                            <td align="right" colspan="2" style="vertical-align: top;">(<%=currOutPrice.pr_part_commission_total.ToString("N2")%>* +
                                                <%=currOutPrice.pr_part_agency_fee.ToString("N2")%>**)
                                            </td>
                                        </tr>
                                        <%} %>
                                        <tr>
                                            <td valign="middle" align="right">
                                                <%= CurrentSource.getSysLangValue("lblSaldoArrivo")%>
                                            </td>
                                            <td valign="middle" align="right" style="width: 75px;">
                                                <strong class="price1">
                                                    <%=currOutPrice.pr_part_owner.ToString("N2")%>
                                                    €</strong>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="nulla">
                                    </div>
                                </div>
                                <% if (currOutPrice.agentID != 0 && currOutPrice.agentCommissionPrice != 0)
                                   { %>
                                <div class="agencyComm">
                                    <span><%= CurrentSource.getSysLangValue("lblYourCommission")%></span>
                                    <a href="#" class="info ico_tooltip" title="info_commissione"></a>
                                    <strong>
                                        <%=currOutPrice.agentCommissionPrice.ToString("N2") + "&nbsp;&euro;"%>
                                    </strong>
                                    <% if (currOutPrice.pr_part_forPayment != currOutPrice.pr_part_payment_total)
                                       { %>
                                    <div class="nulla">
                                    </div>
                                    <br />
                                    *Have to pay <%=currOutPrice.pr_part_forPayment.ToString("N2") + "&nbsp;&euro;"%> to confirm your booking
                                    <%} %>
                                </div>
                                <%} %>
                                <div class="nulla">
                                </div>
                            </div>
                        </div>
                        <% if (currOutPrice.pr_part_agency_fee != 0)
                           { %>
                        <span class="damagedeposit" style="clear: both;">
                            <%= "*&nbsp;" + CurrentSource.getSysLangValue("rnt_pr_part_commission")%>
                        </span>
                        <span class="damagedeposit" style="clear: both;">
                            <%= "**&nbsp;"+CurrentSource.getSysLangValue("rnt_pr_part_agency_fee")%>
                        </span>
                        <%} %>
                        <%= (currEstateTB.pr_deposit != 0) ? "<span class=\"damagedeposit\">" + CurrentSource.getSysLangValue("lblDamageDeposit") + ":<span>" + currEstateTB.pr_deposit + "&euro;</span></span>" : ""%>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PH_bookPriceError" runat="server">
                        <div id="lbl_priceError0" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            <%= CurrentSource.getSysLangValue("lblAptOnRequestSelectedPeriod")%>
                        </div>
                        <div id="lbl_priceError1" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            Instant booking:<br />
                            <%= CurrentSource.getSysLangValue("lblMinStayVHSeason").Replace("5",currEstateTB.nights_minVHSeason+"")%>
                        </div>
                        <div id="lbl_priceError2" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            <%= "Min " + currEstateTB.lpb_nights_min + " nights for the selected period "%>
                        </div>
                        <div id="lbl_priceError3" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            <%= "Attention:<br />Please, click below to enquire about long term rate."%>
                        </div>
                        <div id="lbl_priceError4" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            Instant booking:<br />
                            <%= CurrentSource.getSysLangValue("lblStayMin") + " " + currEstateTB.nights_min %>
                        </div>
                    </asp:PlaceHolder>
                </div>
                <div id="btnsTotale">
                    <asp:PlaceHolder ID="PH_bookNotAvailable" runat="server">
                        <div class="nodisp_apt">
                            <%= CurrentSource.getSysLangValue("lblAptNotAvailableSelectedPeriod")%>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
                        <asp:PlaceHolder ID="PH_bookAvailable" runat="server">
                            <a href="javascript:show_formBookignCont()" class="btn">
                                <span>
                                    <%= ltr_viewBookNow.Text%></span>
                            </a>
                        </asp:PlaceHolder>
                        <a href="#alternativeEstate" class="btn">
                            <span>
                                <%= CurrentSource.getSysLangValue("lblViewAlternatives")%></span>
                        </a>
                    </asp:PlaceHolder>
                </div>
            </asp:PlaceHolder>
            f<asp:PlaceHolder ID="PH_bookingSend" runat="server" Visible="false">
                <div class="alertOk">
                    <%= CurrentSource.getSysLangValue("lblSendRequest")%>
                </div>
            </asp:PlaceHolder>
        </div>
        <div class="box_client_booking" id="pnl_inquire" runat="server" visible="false">
            <a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString()) %>?IdEstate=<%= IdEstate %>" class="btn">
                <span>
                    <%= CurrentSource.getSysLangValue("lblEnquire")%></span>
            </a>
        </div>
        <div class="box_client_booking" id="pnl_sendBooking" runat="server" visible="false">
            <asp:HiddenField ID="HF_mode" runat="server" Value="new" />
            <asp:HiddenField ID="HF_IdClient" runat="server" Value="0" />
            <div class="titflag">
                <div>
                    <span>
                        <%= CurrentSource.getSysLangValue("lblFinishYourReservations")%></span>
                </div>
            </div>
            <asp:PlaceHolder ID="PH_bookingForm" runat="server">
                <asp:PlaceHolder ID="PH_viewClient" runat="server">
                    <div class="nulla">
                    </div>
                    <strong style="font-size: 18px;">
                        <%= CurrentSource.getSysLangValue("lblWelcome")%>&nbsp;<asp:Literal ID="ltr_honorific" runat="server"></asp:Literal>&nbsp;<asp:Literal ID="ltr_name_full" runat="server"></asp:Literal></strong>
                    <br />
                    <asp:Literal ID="ltr_country" runat="server"></asp:Literal>
                    <br />
                    <asp:Literal ID="ltr_phone_mobile" runat="server"></asp:Literal>
                    <br />
                    <asp:Literal ID="ltr_email" runat="server"></asp:Literal>
                    <br />
                    <div class="nulla">
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="PH_newClient" runat="server">
                    <asp:LinkButton ID="lnk_goOldClient" runat="server" OnClick="lnk_goOldClient_Click" CssClass="newclient"><span><%=CurrentSource.getSysLangValue("reqGoRegisteredClient") %></span></asp:LinkButton>
                    <div class="nulla">
                    </div>
                    <div class="line">
                        <div id="txt_email_cont" class="left">
                            <label class="desc">
                                <%=CurrentSource.getSysLangValue("reqEmail")%>*
                            </label>
                            <div>
                                <asp:TextBox ID="txt_email" runat="server" Style="width: 300px;"></asp:TextBox>
                                <span id="txt_email_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;"></span>
                            </div>
                        </div>
                        <div id="txt_email_conf_cont" class="left">
                            <label class="desc">
                                <%=CurrentSource.getSysLangValue("reqEmailConfirm")%>*
                            </label>
                            <div>
                                <asp:TextBox ID="txt_email_conf" runat="server" Style="width: 300px;"></asp:TextBox>
                                <span id="txt_email_conf_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqEmailConfirmError")%></span>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line">
                        <div id="txt_name_first_cont" class="left">
                            <label class="desc" style="margin-left: 60px;">
                                <%=CurrentSource.getSysLangValue("lblName")%>*
                            </label>
                            <div>
                                <asp:DropDownList ID="drp_honorific" runat="server" Style="width: 60px;">
                                </asp:DropDownList>
                                <asp:TextBox ID="txt_name_first" runat="server" Style="width: 300px;"></asp:TextBox>
                                <span id="txt_name_first_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                            </div>
                        </div>
                        <div id="Div1" class="left">
                            <label class="desc">
                                <%=CurrentSource.getSysLangValue("lblSurname")%>*
                            </label>
                            <div>
                                <asp:TextBox ID="txt_name_last" runat="server" Style="width: 300px;"></asp:TextBox>
                                <span id="txt_name_last_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line">
                        <div id="txt_phone_cont" class="left">
                            <label class="desc">
                                <%=CurrentSource.getSysLangValue("reqPhoneNumber")%>*
                            </label>
                            <div>
                                <asp:TextBox ID="txt_phone_mobile" runat="server"></asp:TextBox>
                                <span id="txt_phone_mobile_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                            </div>
                        </div>
                        <div id="drp_country_cont" class="left">
                            <label class="desc">
                                <%=CurrentSource.getSysLangValue("reqLocation")%>*
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" Style="width: 350px;" TabIndex="5">
                                </asp:DropDownList>
                                <span id="drp_country_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></span>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line2">
                        <div class="left">
                            <label class="desc">
                                <%=CurrentSource.getSysLangValue("lblPrivacyPolicy")%>
                                <asp:HyperLink ID="HL_getPdf_privacy" runat="server" Target="_blank">
                                    <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                                </asp:HyperLink>
                            </label>
                            <div class="div_terms">
                                <asp:Literal ID="ltr_privacy" runat="server"></asp:Literal>
                            </div>
                            <div class="accettocheck" style="height: 30px;" id="chk_privacyAgree_cont">
                                <input type="checkbox" id="chk_privacyAgree" />
                                <label for="chk_privacyAgree">
                                    <%=CurrentSource.getSysLangValue("lblAcceptPersonalDataPrivacy")%>*</label>
                            </div>
                            <span id="chk_privacyAgree_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                <%=CurrentSource.getSysLangValue("reqRequiredAcceptPrivacy")%></span>
                        </div>
                        <div class="left">
                            <label class="desc">
                                <%=CurrentSource.getSysLangValue("lblTermsAndConditions")%>
                                <asp:HyperLink ID="HL_getPdf_terms" runat="server" Target="_blank">
                                    <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                                </asp:HyperLink>
                            </label>
                            <div class="div_terms">
                                <asp:Literal ID="ltr_terms" runat="server"></asp:Literal>
                            </div>
                            <div class="accettocheck" style="height: 30px;" id="chk_termsOfService_cont">
                                <input type="checkbox" id="chk_termsOfService" />
                                <label for="chk_termsOfService">
                                    <%=CurrentSource.getSysLangValue("lblAcceptTermsAndConditions")%>*</label>
                            </div>
                            <span id="chk_termsOfService_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                <%=CurrentSource.getSysLangValue("reqRequiredAcceptTermsOfService")%></span>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                </asp:PlaceHolder>
                <asp:LinkButton ID="lnk_payNow" CssClass="btn cartebig" runat="server" OnClientClick="return RNT_validateBook()" OnClick="lnk_payNow_Click"><span><%=CurrentSource.getSysLangValue("reqPayWith")%></span></asp:LinkButton>
                <img src="/images/css/thawte_big.jpg" alt="Godaddy certificate" style="height: 47px; margin-top: 10px;" />
                <!--div class="mr_estate_pay" rel="shadowbox;height=450;width=500" href="info_payement.html"><img src="" /></div-->
                <!-- INFO PAGAMENTO -->
                <div class="infopagbottom" runat="server" visible="false">
                    <div class="arrow">
                    </div>
                    <div class="box">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <uc1:UC_static_block ID="UC_static_block1" runat="server" BlockID="10" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PH_oldClient" runat="server">
                <asp:LinkButton ID="lnk_goNewClient" runat="server" OnClick="lnk_goNewClient_Click" CssClass="client"><span><%=CurrentSource.getSysLangValue("reqGoNewClient") %></span></asp:LinkButton>
                <div class="nulla">
                </div>
                <div class="line">
                    <div class="left">
                        <label class="desc">User Name* </label>
                        <div>
                            <asp:TextBox ID="txt_old_email" runat="server" Style="width: 300px;"></asp:TextBox>
                        </div>
                    </div>
                    <div class="left">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("reqPassword")%>*
                        </label>
                        <div>
                            <asp:TextBox ID="txt_old_password" runat="server" Style="width: 300px;" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
                <asp:LinkButton ID="lnk_login" CssClass="btn log" runat="server" OnClientClick="return true" OnClick="lnk_login_Click"><span>Login</span></asp:LinkButton>
                <asp:LinkButton ID="lnk_goPwd" CssClass="" runat="server" OnClientClick="return true" OnClick="lnk_goPwd_Click" Style="display: block; float: left; margin: 10px 5px 5px 26px;"><span><%=CurrentSource.getSysLangValue("formPasswordRecovery")%></span></asp:LinkButton>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PH_pwdRecover" runat="server">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnk_goNewClient_Click" CssClass="client"><span><%=CurrentSource.getSysLangValue("reqGoNewClient") %></span></asp:LinkButton>
                <div class="nulla">
                </div>
                <div class="line">
                    <div class="left">
                        <label class="desc">User Name or Email </label>
                        <div>
                            <asp:TextBox ID="txt_pwdRecover" runat="server" Style="width: 300px;"></asp:TextBox>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
                <asp:LinkButton ID="lnk_pwdRevover" CssClass="btn log" runat="server" OnClientClick="return true" OnClick="lnk_pwdRevover_Click"><span>Send</span></asp:LinkButton>
            </asp:PlaceHolder>
            <asp:Label ID="lbl_errorAlert" CssClass="error_form_book" runat="server" Visible="false"></asp:Label>
            <div class="nulla">
            </div>
        </div>
        <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="false">
            <asp:LinkButton ID="lnk_book" CssClass="btn bonifico" runat="server" OnClientClick="return RNT_validateBook()" OnClick="lnk_book_Click"><span>Paying by bank transfer</span></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
        </asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>
