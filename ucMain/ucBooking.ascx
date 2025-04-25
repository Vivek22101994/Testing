<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBooking.ascx.cs" Inherits="RentalInRome.ucMain.ucBooking" %>
<style>
    .form-control[disabled], .form-control[readonly], fieldset[disabled] .form-control {
        cursor: auto!important;
    }

    .chzn-container-single .chzn-single div b {
        margin-top: 13px!important;
    }

    .tooltip {
        width: 500px!important;
        /*min-width: 1000px!important;*/
    }
</style>
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

        <div class="gray bookingFormDet bookingform">
            <div class="closebook">x</div>
            <h2 class="section-title">Book this Property</h2>
            <div class="form-group">
                <div class="col-sm-12">
                    <%--<input type="text" name="checkin" placeholder="Check-in" class="form-control calendar-input">--%>
                    <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" placeholder="Check-in" class="form-control calendar-input" />
                    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                    <a class="calendario" id="startCalTrigger_<%= Unique %>"></a>

                    <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" placeholder="Check-out" class="form-control calendar-input" />
                    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                    <a class="calendario" id="endCalTrigger_<%= Unique %>"></a>


                    <asp:DropDownList ID="drp_adult" runat="server" data-placeholder="Adults" onchange="RNT_calculateNumPersons()">
                    </asp:DropDownList>

                    <asp:DropDownList ID="drp_child_over" runat="server" onchange="RNT_estateCheckAvv()" data-placeholder="Children">
                    </asp:DropDownList>

                    <asp:DropDownList ID="drp_child_min" runat="server" onchange="RNT_estateCheckAvv()" data-placeholder="Infants">
                    </asp:DropDownList>

                    <span class="alert warning" id="pnlChildrenNotAllowed" runat="server" visible="false"><%= CurrentSource.getSysLangValue("lblChildrenNotAllowed")%></span>

                    <div id="promoCode" style="float: left;">
                        <asp:TextBox ID="txt_promotionalCode" runat="server" placeholder="Promotional code" class="form-control"></asp:TextBox>
                        <a href="#" onclick="bookingForm_checkPromoCode(); return false;" class="btnGo"></a>
                        <span id="lb_promotionalCode_error" style="display: none;"></span>
                    </div>
                    <div class="nulla"></div>
                    <asp:PlaceHolder ID="PH_bookPriceOK" runat="server" Visible="false">

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


                        <asp:Panel ID="pnl_dicountCont" runat="server" CssClass="sconto_div" Visible="false">
                            <span><%= CurrentSource.getSysLangValue("lblPrice")%>:</span> <strong class="newPrice"><%=(currOutPrice.prTotalRate).ToString("N2") + "&nbsp;&euro;" %></strong><br />

                            <%if (currOutPrice.pr_part_agency_fee > 0)
                              {%>
                            <span><%= CurrentSource.getSysLangValue("rnt_pr_part_agency_fee")%>:</span> <strong class="newPrice"><%=(currOutPrice.pr_part_agency_fee).ToString("N2") + "&nbsp;&euro;" %></strong><br />
                            <%} %>

                            <%if (currOutPrice.pr_discount_commission > 0)
                              {%>
                            <span class="promoCodeLbl"><%= CurrentSource.getSysLangValue("lblPromoDiscount")%>:</span> <strong class="newPrice promoCodePrice"><%="- " +(currOutPrice.pr_discount_commission).ToString("N2") + "&nbsp;&euro;"%></strong><br />
                            <%} %>

                            <%if (currOutPrice.prDiscountSpecialOffer > 0)
                              {%>
                            <span><%= CurrentSource.getSysLangValue("lblDiscountSpecialOffer")%>:</span> <strong class="newPrice"><%="- " +(currOutPrice.prDiscountSpecialOffer).ToString("N2") + "&nbsp;&euro;"%></strong><br />
                            <%} %>

                            <%if (currOutPrice.prDiscountLongStay > 0)
                              {%>
                            <span><%= CurrentSource.getSysLangValue("lblDiscountLongStay")%>:<a class="infoBtn ico_tooltip" title="<%= "- " +currOutPrice.prDiscountLongStayDesc%>" style="float: right; margin-right: 4px;"></a></span> <strong class="newPrice"><%=(currOutPrice.prDiscountLongStay).ToString("N2") + "&nbsp;&euro;"%></strong><br />
                            <%} %>

                            <%if (currOutPrice.srsPrice > 0)
                              {%>
                            <span><%= CurrentSource.getSysLangValue("lblWelcomeService")%>:</span> <strong class="newPrice"><%=(currOutPrice.srsPrice).ToString("N2") + "&nbsp;&euro;"%></strong><br />
                            <%} %>

                            <%if (currOutPrice.ecoPrice > 0)
                              {%>
                            <span>Cleaning Service X<%= currOutPrice.ecoPrice%>:</span> <strong class="newPrice"><%=(currOutPrice.ecoPrice).ToString("N2") + "&nbsp;&euro;"%></strong><br />
                            <%} %>
                        </asp:Panel>
                        <div class="total_div">
                            <span><%= CurrentSource.getSysLangValue("lblTotalPrice")%> :</span> <strong class="newPrice"><%=(currOutPrice.prTotal).ToString("N2") + "&nbsp;&euro;"%></strong><br />
                        </div>
                        <div class="anticiposaldo_div">
                            <%if (currOutPrice.agentCommissionPrice > 0)
                              {%>
                            <span>Agency Discount :</span> <strong class="newPrice"><%=(currOutPrice.agentCommissionPrice).ToString("N2") + "&nbsp;&euro;" + (currOutPrice.agentDiscountNotPayed==0?"Compreso nel totale":"Scalato dal totale") %></strong><br />

                            <%} %>

                            <span><%= CurrentSource.getSysLangValue("lblAnticipo")%>:</span> <strong class="newPrice"><%=(currOutPrice.pr_part_payment_total).ToString("N2") + "&nbsp;&euro;" %></strong><br />


                            <% if (currOutPrice.pr_part_agency_fee != 0)
                               { %>
                            <strong class="newPrice" style="float: right;">(<%=currOutPrice.pr_part_commission_total.ToString("N2")%>* +
                                                <%=currOutPrice.pr_part_agency_fee.ToString("N2")%>**)</strong><br />
                            <%} %>

                            <span><%= CurrentSource.getSysLangValue("lblSaldoArrivo")%>:</span> <strong class="newPrice"><%=(currOutPrice.pr_part_owner).ToString("N2") + "&nbsp;&euro;" %></strong><br />
                            <div class="nulla">
                            </div>
                        </div>

                        <% if (currOutPrice.agentID != 0 && currOutPrice.agentCommissionPrice != 0)
                           { %>
                        <div class="agencyComm">

                            <span><%= CurrentSource.getSysLangValue("lblYourCommission")%>  
                                <a href="#" class="info ico_tooltip" title="tooltip_info_commissione" style="display: none;"></a>:</span>
                             <strong class="newPrice"><%=(currOutPrice.agentCommissionPrice).ToString("N2") + "&nbsp;&euro;" %></strong><br />

                            <% if (currOutPrice.pr_part_forPayment != currOutPrice.pr_part_payment_total)
                               { %>
                            <div class="nulla">
                            </div>
                            <br />
                            *Have to pay <%=currOutPrice.pr_part_forPayment.ToString("N2") + "&nbsp;&euro;"%> to confirm your booking
                                    <%} %>
                        </div>
                        <%} %>

                        <% if (currOutPrice.pr_part_agency_fee != 0)
                           { %>
                        <span class="damagedeposit" style="clear: both;">
                            <%= "*&nbsp;" + CurrentSource.getSysLangValue("rnt_pr_part_commission")%>
                        </span>
                        <span class="damagedeposit" style="clear: both;">
                            <%= "**&nbsp;"+CurrentSource.getSysLangValue("rnt_pr_part_agency_fee")%>
                        </span>
                        <%} %>
                        <%= (currEstateTB.pr_deposit != 0) ? "<span class=\"damagedeposit\"><span>" + CurrentSource.getSysLangValue("lblDamageDeposit") + ":</span><strong class='newPrice'>" + currEstateTB.pr_deposit + "&euro;</strong></span>" : ""%>


                     



                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PH_bookPriceError" runat="server" Visible="false">
                        <div id="lbl_priceError0" runat="server" class="nodisp_apt">
                            <%= CurrentSource.getSysLangValue("lblAptOnRequestSelectedPeriod")%>
                        </div>
                        <div id="lbl_priceError1" runat="server" class="nodisp_apt">
                            Instant booking:<br />
                            <%= CurrentSource.getSysLangValue("lblMinStayVHSeason").Replace("5",currEstateTB.nights_minVHSeason+"")%>
                        </div>
                        <div id="lbl_priceError2" runat="server" class="nodisp_apt">
                            <%= "Min " + currEstateTB.lpb_nights_min + " nights for the selected period "%>
                        </div>
                        <div id="lbl_priceError3" runat="server" class="nodisp_apt">
                            <%= "Attention:<br />Please, click below to enquire about long term rate."%>
                        </div>
                        <div id="lbl_priceError4" runat="server" class="nodisp_apt" style="padding: 10px; width: auto;">
                            Instant booking:<br />
                            <%= CurrentSource.getSysLangValue("lblStayMin") + " " + currEstateTB.nights_min %>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PH_bookNotAvailable" runat="server" Visible="false">
                        <div class="nodisp_apt">
                            <%= CurrentSource.getSysLangValue("lblAptNotAvailableSelectedPeriod")%>
                        </div>
                    </asp:PlaceHolder>
                </div>

                <p class="center">
                    <asp:Panel ID="pnl_sendBooking" runat="server" Visible="false">
                        <asp:LinkButton ID="lnkBookNow" runat="server" CssClass="btn btn-default-color btn-book-nowr" OnClick="lnkBookNow_Click" Style="margin-left: 30%;">
                    <%= contUtils.getLabel("reqBookNow")%>
                        </asp:LinkButton>
                    </asp:Panel>
                    <asp:Panel ID="pnl_inquire" runat="server">
                        <a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString())  %>?IdEstate=<%= IdEstate %>" class="btn btn-default-color btn-enquiry" style="margin-left: 30%;">
                            <span>
                                <%= CurrentSource.getSysLangValue("lblEnquire")%></span>
                        </a>
                    </asp:Panel>
                </p>
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
            </div>
        </div>
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
                _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", countMin: <%=currEstateTB.nights_min %>, minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> , callBack: function(){RNT_estateCheckAvv();} });
             
            }
            <%= ltr_checkCalDates.Text %>
        </script>

        <script type="text/javascript">
            function setChoosen()
            {
                if ($('select').not(".ui-datepicker-month").length) {
                    console.log($('select').not(".ui-datepicker-month,.ui-datepicker-year").length);
                    $("select").not(".ui-datepicker-month,.ui-datepicker-year").chosen({
                        allow_single_deselect: true,
                        disable_search_threshold: 12
                    });
                }
            }
        </script>
        <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
    </ContentTemplate>
</asp:UpdatePanel>
