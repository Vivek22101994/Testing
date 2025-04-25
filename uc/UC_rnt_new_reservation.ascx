<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_new_reservation.ascx.cs" Inherits="RentalInRome.uc.UC_rnt_new_reservation" %>
<%@ Register Src="UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
        <asp:HiddenField ID="HF_IdEstate" runat="server" Value="0" />
        <asp:HiddenField ID="HF_IdRequest" Value="0" runat="server" />
        <asp:HiddenField ID="HF_is_online_booking" runat="server" Value="0" />
        <asp:HiddenField ID="HF_pr_percentage" runat="server" Value="0" />
        <asp:HiddenField ID="HF_pr_deposit" runat="server" Value="0" />
        <asp:HiddenField ID="HF_pr_depositWithCard" runat="server" Value="0" />
        <asp:HiddenField ID="HF_lpb_nights_min" runat="server" Value="0" />
        <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
        <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
        <asp:HiddenField ID="HF_dtCount" runat="server" Value="0" />
        <asp:HiddenField ID="HF_num_adult" runat="server" Value="2" />
        <asp:HiddenField ID="HF_num_child_over" runat="server" Value="0" />
        <asp:HiddenField ID="HF_num_child_min" runat="server" Value="0" />
        <asp:Literal ID="ltr_srs_ext_meetingPoint" runat="server" Visible="false"></asp:Literal>
        <asp:Literal ID="ltr_sel_priceDetails" runat="server" Visible="false"></asp:Literal>
        <asp:Literal ID="ltr_viewBookNow" runat="server" Visible="false"></asp:Literal>
        <asp:Literal ID="ltr_cl_browserInfo" runat="server" Visible="false"></asp:Literal>
        <asp:Literal ID="ltr_cl_browserIP" runat="server" Visible="false"></asp:Literal>
        <asp:HiddenField ID="HF_tmp_prTotal" runat="server" Value="0" />
        <input type="hidden" id="RNT_alternativeEstate_minPrice" value="<%= HF_tmp_prTotal.Value %>" />
        <script type="text/javascript">
            function show_formBookignCont() {
                $('#formBookignCont').show();
                $('#tmp_focus').focus();
            }
        </script>
        <div id="yourBookingPrice">
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
                                    <%= HF_dtStart.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" class="col1">
                                <%= CurrentSource.getSysLangValue("reqCheckOutDate")%>
                            </td>
                            <td valign="middle" align="left" class="col2">
                                <span id="lb_sel_dtEnd">
                                    <%= HF_dtEnd.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" class="col1">
                                <%= CurrentSource.getSysLangValue("lblTotalNights")%>
                            </td>
                            <td valign="middle" align="left" class="col2">
                                <span id="lb_sel_dtCount">
                                    <%= HF_dtCount.Value%></span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" class="col1">
                                <%= CurrentSource.getSysLangValue("lblPax")%>
                            </td>
                            <td valign="middle" align="left" class="col2">
                                <span id="lb_sel_personsCount">
                                    <%=HF_num_adult.Value + " Adults, " + HF_num_child_over.Value + " Children, " + HF_num_child_min.Value + " Infants"%></span>
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
                                    <%=HF_num_adult.Value + " Adults, " + HF_num_child_over.Value + " Children, " + HF_num_child_min.Value + " Infants"%></span>
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
                <div class="boxStep" id="promoCode">
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
                                <a href="javascript:$('#lb_promotionalCode_error').show();$('#<%=txt_promotionalCode.Text%>').val('');" class="btnGo"></a>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" colspan="2">
                                <span id="lb_promotionalCode_error" style="display: none;">
                                    <%= CurrentSource.getSysLangValue("lblInvalidCode")%></span>
                            </td>
                        </tr>
                    </table>
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
                                        <%if (currOutPrice.prDiscountLongStay> 0)
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
                                            <td valign="middle" align="right">
                                                Welcome Service
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
                                            <td valign="middle" align="right">
                                                Cleaning Service X<%= currOutPrice.ecoCount%>
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
                                            <td valign="middle" align="right">
                                                Agency Discount
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
                                        <% if (currOutPrice.pr_part_agency_fee!=0){ %>
                                        <tr style="height: 20px;">
                                            <td align="right" colspan="2" style="vertical-align: top;">
                                                (<%=currOutPrice.pr_part_commission_total.ToString("N2")%>* +
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
                        <% if (currOutPrice.pr_part_agency_fee!=0){ %>
                        <span class="damagedeposit" style="clear: both;">
                            <%= "*&nbsp;" + CurrentSource.getSysLangValue("rnt_pr_part_commission")%>
                        </span>
                        <span class="damagedeposit" style="clear: both;">
                            <%= "**&nbsp;"+CurrentSource.getSysLangValue("rnt_pr_part_agency_fee")%>
                        </span>
                        <%} %>
                        <%= (pr_deposit != 0) ? "<span class=\"damagedeposit\">" + CurrentSource.getSysLangValue("lblDamageDeposit") + ":<span>" + pr_deposit + "&euro;</span></span>" : ""%>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PH_bookPriceError" runat="server">
                        <div id="lbl_priceError0" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            <%= CurrentSource.getSysLangValue("lblAptOnRequestSelectedPeriod")%>
                        </div>
                        <div id="lbl_priceError1" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            Instant booking:<br />
                            <%= CurrentSource.getSysLangValue("lblMinStayVHSeason").Replace("5","5")%>
                        </div>
                        <div id="lbl_priceError2" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            <%= "Min "+lpb_nights_min+" nights for the selected period " %>
                        </div>
                        <div id="lbl_priceError3" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            <%= "Attention:<br />Please, click below to enquire about long term rate."%>
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
            <asp:PlaceHolder ID="PH_bookingSend" runat="server" Visible="false">
                <div class="alertOk">
                    <%= CurrentSource.getSysLangValue("lblSendRequest")%>
                </div>
            </asp:PlaceHolder>
        </div>
        <div class="box_client_booking" id="pnl_inquire" runat="server">
            <a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString()) %>?IdEstate=<%= IdEstate %>" class="btn">
                <span>
                    <%= CurrentSource.getSysLangValue("lblEnquire")%></span>
            </a>
        </div>
        <div class="box_client_booking" id="pnl_sendBooking" runat="server">
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
                                <%=CurrentSource.getSysLangValue("reqEmail")%>* </label>
                            <div>
                                <asp:TextBox ID="txt_email" runat="server" Style="width: 300px;"></asp:TextBox>
                                <span id="txt_email_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;"></span>
                            </div>
                        </div>
                        <div id="txt_email_conf_cont" class="left">
                            <label class="desc">
                                <%=CurrentSource.getSysLangValue("reqEmailConfirm")%>* </label>
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
                                <%=CurrentSource.getSysLangValue("lblName")%>* </label>
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
                                <%=CurrentSource.getSysLangValue("lblSurname")%>* </label>
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
                                <%=CurrentSource.getSysLangValue("reqPhoneNumber")%>* </label>
                            <div>
                                <asp:TextBox ID="txt_phone_mobile" runat="server"></asp:TextBox>
                                <span id="txt_phone_mobile_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                            </div>
                        </div>
                        <div id="drp_country_cont" class="left">
                            <label class="desc">
                                <%=CurrentSource.getSysLangValue("reqLocation")%>* </label>
                            <div>
                                <asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" Style="width: 350px;" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                                </asp:DropDownList>
                                <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                    <WhereParameters>
                                        <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                    </WhereParameters>
                                </asp:LinqDataSource>
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
                <asp:LinkButton ID="lnk_payNow" CssClass="btn cartebig" runat="server" OnClientClick="return RNT_validateBook()" OnClick="lnk_payNow_Click"><span></span></asp:LinkButton>
                <img src="/images/css/thawte_big.jpg" alt="thawte certificate" style="height: 47px; margin-top: 10px;" />
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
                            <%=CurrentSource.getSysLangValue("reqPassword")%>* </label>
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
        <script type="text/javascript">
        function validateForm_<%= Unique%>() {
            if ($("#<%= HF_mode.ClientID %>").val() == "new") {
            }
            else {
            }
        }
        </script>
        <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="false">
            <asp:LinkButton ID="lnk_book" CssClass="btn bonifico" runat="server" OnClientClick="return RNT_validateBook()" OnClick="lnk_book_Click"><span>Paying by bank transfer</span></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
        </asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>
