<%@ Page Title="" Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="reservationComplete.aspx.cs" Inherits="RentalInRome.affiliatesarea.reservationComplete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
            <asp:HiddenField ID="HF_IdEstate" runat="server" Value="0" />
            <asp:Literal ID="ltr_sel_priceDetails" runat="server" Visible="false"></asp:Literal>
            <asp:HiddenField ID="HF_tmp_prTotal" runat="server" Value="0" />
            <asp:Literal ID="ltr_estateTitle" runat="server" Visible="false"></asp:Literal>
            <input type="hidden" id="RNT_alternativeEstate_minPrice" value="<%= HF_tmp_prTotal.Value %>" />
            <script type="text/javascript">
                function show_formBookignCont() {
                    $('#formBookignCont').show();
                    $('#tmp_focus').focus();
                }
            </script>
            <a href="reservationNew.aspx" class="buttprosegui" style="margin-bottom: 10px;">&lt;&lt;&lt; back</a>
            <div class="nulla">
            </div>
            <div id="yourBookingPrice">
                <h3 class="titBar">
                    <%= CurrentSource.getSysLangValue("lblYourBooking") + ": " + ltr_estateTitle.Text%>
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
                                        <%= currOutPrice.dtStart.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></span>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left" class="col1">
                                    <%= CurrentSource.getSysLangValue("reqCheckOutDate")%>
                                </td>
                                <td valign="middle" align="left" class="col2">
                                    <span id="lb_sel_dtEnd">
                                        <%= currOutPrice.dtEnd.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></span>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left" class="col1">
                                    <%= CurrentSource.getSysLangValue("lblTotalNights")%>
                                </td>
                                <td valign="middle" align="left" class="col2">
                                    <span id="lb_sel_dtCount">
                                        <%= currOutPrice.dtCount%></span>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left" class="col1">
                                    <%= CurrentSource.getSysLangValue("lblPax")%>
                                </td>
                                <td valign="middle" align="left" class="col2">
                                    <span id="lb_sel_personsCount">
                                        <%=currOutPrice.numPers_adult + " Adults, " + currOutPrice.numPers_childOver + " Children, " + currOutPrice.numPers_childMin + " Infants"%></span>
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
                                        <%=currOutPrice.numPers_adult + " Adults, " + currOutPrice.numPers_childOver + " Children, " + currOutPrice.numPers_childMin + " Infants"%></span>
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
                    <div class="boxStep" id="promoCode" runat="server" visible="false">
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
                                        <span>
                                            <%= CurrentSource.getSysLangValue("lblYourCommission")%></span>
                                        <a href="#" class="info ico_tooltip" title="info_commissione"></a>
                                        <strong>
                                            <%=currOutPrice.agentCommissionPrice.ToString("N2") + "&nbsp;&euro;"%>
                                        </strong>
                                    </div>
                                    <%} %>
                                    <div class="nulla">
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PH_bookPriceError" runat="server">
                            <div id="lbl_priceError0" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                                <%= CurrentSource.getSysLangValue("lblAptOnRequestSelectedPeriod")%>
                            </div>
                            <div id="lbl_priceError1" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                                Instant booking:<br />
                                <%= CurrentSource.getSysLangValue("lblMinStayVHSeason")%>
                            </div>
                            <div id="lbl_priceError2" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                                <%= "Min 5 nights for the selected period " %>
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
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="PH_bookingSend" runat="server" Visible="false">
                    <div class="alertOk">
                        <%= CurrentSource.getSysLangValue("lblSendRequest")%>
                    </div>
                </asp:PlaceHolder>
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
                <div class="nulla">
                </div>
                <asp:LinkButton ID="lnk_payNow" CssClass="btn cartebig" runat="server" OnClientClick="return RNT_validateBook()" OnClick="lnk_payNow_Click"><%="<span>" + CurrentSource.getSysLangValue("reqPayWith") + "</span>"%></asp:LinkButton>
                <img src="/images/css/thawte_big.jpg" alt="thawte certificate" style="height: 47px; margin-top: 10px;" />
                <!--div class="mr_estate_pay" rel="shadowbox;height=450;width=500" href="info_payement.html"><img src="" /></div-->
                <!-- INFO PAGAMENTO -->
                <div class="infopagbottom">
                    <div class="arrow">
                    </div>
                    <div class="box">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <%= contUtils.getBlock(10, CurrentLang.ID, "")%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:Label ID="lbl_errorAlert" CssClass="error_form_book" runat="server" Visible="false"></asp:Label>
                <div class="nulla">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
