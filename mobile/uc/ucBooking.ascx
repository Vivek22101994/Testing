<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBooking.ascx.cs" Inherits="RentalInRome.mobile.uc.ucBooking" %>
<asp:HiddenField runat="server" ID="HF_unique" Visible="false" />
<asp:HiddenField runat="server" ID="HF_id" Visible="false" />
<asp:HiddenField runat="server" ID="HF_IdRequest" Visible="false" />
<asp:HiddenField runat="server" ID="HF_CURRENT_SESSION_ID" Visible="false" />
<script src="/jquery/plugin/jquery.json-2.4.min.js" type="text/javascript"></script>
<script type="text/javascript">
    Number.prototype.formatMoney = function(c, d, t){
        var n = this, 
            c = isNaN(c = Math.abs(c)) ? 2 : c, 
            d = d == undefined ? "." : d, 
            t = t == undefined ? "," : t, 
            s = n < 0 ? "-" : "", 
            i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", 
            j = (j = i.length) > 3 ? j % 3 : 0;
        return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
    };
    var RNT_estateCheckAvvRequest;
    function RNT_estateCheckAvv(isFirst) {
        if(RNT_estateCheckAvvRequest)
            RNT_estateCheckAvvRequest.abort();
        var _url = "/webservice/rntEstateCheckAvv.aspx";
        _url += "?SESSION_ID=<%= CURRENT_SESSION_ID %>";
        _url += "&IdEstate=<%= currEstateTB.id %>";
        _url += "&lang=<%= CurrentLang.ID %>";
        _url += "&dtS=" + $("#<%= HF_dtStart.ClientID %>").val();
        _url += "&dtE=" + $("#<%= HF_dtEnd.ClientID %>").val();
        _url += "&numPers_adult=" + $("#<%= drp_numPers_adult.ClientID %>").val();
        _url += "&numPers_childOver="+ $("#<%= drp_numPers_children.ClientID %>").val();
        _url += "&numPers_childMin="+ $("#<%= drp_numPers_infants.ClientID %>").val();
        //if (!isFirst)
        //    SITE_showLoader();
        $("#pnl_avvPriceCont").addClass("loading");
        $("#pnl_avvPriceCont").html('<img src="/images/css-common/loading_small.gif" alt="loading" style="width: auto; height: 35px;" />');
        $("#pnl_avvPriceTotal").html('');
        
        $("#inquireAptLink").hide();
        $("#<%= lnkBookNow.ClientID %>").hide();
        $("#pnlSpecialOffer").hide();
        RNT_estateCheckAvvRequest = $.ajax({
            type: "GET",
            url: _url,
            success: function (json) {
                if (json == "error") {
                    window.location = "/<%= currEstateLN.page_path %>"; 
                    return;
                }
                else {
                    //$.evalJSON(encoded);
                    //alert($.evalJSON(json).error);
                    respData = $.evalJSON(json);
                    if (!respData.isAvv || !respData.hasPrice) {
                        if(respData.outPrice.outError == 1){
                            $("#pnl_avvPriceCont").html('<%= contUtils.getLabel("lblStayMin")%> '+respData.outPrice.outErrorDesc+' <%= contUtils.getLabel("lblNights")%>');
                        }
                        else if(respData.outPrice.outError == 5){
                            $("#pnl_avvPriceCont").html('<%= contUtils.getLabel("lblErrorCheckinDateNotAvv").Replace("'","\\'").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ")%>');
                        }
                        else{
                            $("#pnl_avvPriceCont").html('<%= contUtils.getLabel("lblAptNotAvailableSelectedPeriod").Replace("'","\\'").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ")%>');
                        }
                    $("#pnl_avvPriceCont").addClass("avvError");
                        
                        //hidePriceDetails();
                    $("#linkDetailsShow").hide();
                    $("#linkDetailsHide").hide();
                    $("#detailsSx").html("");  
                }
                else {
                    $("#pnl_avvPriceCont").removeClass("avvError");
                    var tmp = '';
                    tmp += '€' +respData.outPrice.pr_part_forPayment + '';
                    $("#pnl_avvPriceCont").html(tmp);
                    tmp = '';
                    tmp += '€' +respData.prTotal + '';
                    $("#pnl_avvPriceTotal").html(tmp);
                    tmp = '';
                    tmp +='Total rate: <strong class="newPrice">€' + respData.outPrice.prTotalRate.formatMoney(2, ',', '.')  + '</strong><br/>';
                    if(respData.outPrice.prDiscountSpecialOffer!=0)
                        tmp +='<span style="color: #7ea769">Special offer discount: <strong class="newPrice" style="color: #7ea769">€-' + respData.outPrice.prDiscountSpecialOffer.formatMoney(2, ',', '.')  + '</strong></span><br/>';
                    if(respData.outPrice.prDiscountLongStay!=0)
                        tmp +='' + respData.outPrice.prDiscountLongStayDesc  + ': <strong class="newPrice">€-' + respData.outPrice.prDiscountLongStay.formatMoney(2, ',', '.')  + '</strong><br/>';
                        //if(respData.outPrice.prDiscountLongRange!=0)
                        //    tmp +='' + respData.outPrice.prDiscountLongRangeDesc  + ': <strong class="newPrice">€-' + respData.outPrice.prDiscountLongRange.formatMoney(2, ',', '.')  + '</strong><br/>';
                        //if(respData.outPrice.prDiscountLastMinute!=0)
                        //    tmp +='' + respData.outPrice.prDiscountLastMinuteDesc  + ': <strong class="newPrice">€-' + respData.outPrice.prDiscountLastMinute.formatMoney(2, ',', '.')  + '</strong><br/>';
                    if(respData.outPrice.pr_part_agency_fee!=0)
                        tmp +='Agency Fee: <strong class="newPrice">€' + respData.outPrice.pr_part_agency_fee.formatMoney(2, ',', '.')  + '</strong><br/>';
                    tmp += 'Total price:<strong class="newPrice">€' +respData.prTotal + '</strong>';

                    $("#detailsSx").html(tmp);  

                    console.log(respData.isInstantBooking);

                    if(respData.isInstantBooking)
                        $("#<%= lnkBookNow.ClientID %>").show();
                    else
                    {
                        window.location.href='<%= App.HOST +"/m"+ CurrentSource.getPagePath("6", "stp", CurrentLang.ID.ToString()) %>';
                        return;
                    }
                        
                    if(respData.priceChange.id>0){
                        var changeAmount = "-"+respData.priceChange.amount+"<span>"+respData.priceChange.symbol+"</span>";
                        var changePeriod = "from <strong>"+respData.priceChange.start+"</strong> to <strong>"+respData.priceChange.end+"</strong>";
                        $("#pnlSpecialOffer .discountListItem").html(changeAmount);
                        $("#pnlSpecialOffer .periodSpOffItem").html(changePeriod);
                        $("#pnlSpecialOffer").show();
                    }
                }
            }
                $("#pnl_avvPriceCont").removeClass("loading");
            }
        });
}
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="summary" id="summaryDett">
            <div class="summaryLine">
                <span class="summLabel"><%= contUtils.getLabel("reqCheckInDate")%>:</span>
                <span class="periodDates">
                    <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" style="" />
                    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                </span>
            </div>
            <div class="summaryLine">
                <span class="summLabel"><%= contUtils.getLabel("reqCheckOutDate")%>:</span>
                <span class="periodDates">
                    <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" style="" />
                    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                </span>
            </div>
            <div class="summaryLine">
                <span class="summLabel"><%= contUtils.getLabel("lblTotalNights")%>:</span>
                <span class="periodDates">
                    <asp:TextBox ID="txt_dtCount" runat="server" Style="width: 70px; margin: 0; float: right;"></asp:TextBox>
                </span>
            </div>
            <div class="summaryLine">
                <span class="summLabel"><%= contUtils.getLabel("reqAdults")%>:</span>
                <span class="periodDates">
                    <asp:DropDownList ID="drp_numPers_adult" runat="server" onchange="RNT_estateCheckAvv()" Style="width: 71px; float: right;">
                    </asp:DropDownList>
                </span>
            </div>
            <div class="summaryLine">
                <span class="summLabel"><%= contUtils.getLabel("reqChildren")%>:</span>
                <span class="periodDates">
                    <asp:DropDownList ID="drp_numPers_children" runat="server" AutoPostBack="true" Style="width: 71px; float: right;">
                    </asp:DropDownList>
                </span>
            </div>
            <div class="summaryLine">
                <span class="summLabel"><%= contUtils.getLabel("reqEnfants")%>:</span>
                <span class="periodDates">
                    <asp:DropDownList ID="drp_numPers_infants" runat="server" AutoPostBack="true" Style="width: 71px; float: right;">
                    </asp:DropDownList>
                </span>
            </div>
            <div id="SummTotal" class="summaryLine">
                <span class="summLabel"><%= contUtils.getLabel("lblTotalPrice")%>:</span>
                <strong id="pnl_avvPriceTotal" class="summTotal"></strong>
            </div>
            <div id="SummPart" class="summaryLine">
                <span class="summLabel"><%= contUtils.getLabel("lblAnticipo")%>:</span>
                <strong id="pnl_avvPriceCont" class="summTotal acconto"></strong>
            </div>
            <div class="summaryLine" id="detailsSx" style="display: none;"></div>
        </div>
        <div class="nulla">
        </div>
        <asp:LinkButton ID="lnkBookNow" runat="server" CssClass="btnBigMobile btnBookNowDett" OnClick="lnkBookNow_Click"><%= CurrentSource.getSysLangValue("reqBookNow")%></asp:LinkButton>

    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    var _JSCal_Range_<%= Unique %>;
    function setCal_<%= Unique %>(dtStartInt, dtEndInt) {
        _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", countMin: <%= currEstateTB.nights_min %>, minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", countCont: "#<%= txt_dtCount.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %>, callBack: function(){RNT_estateCheckAvv();}  });
    }
    <%= ltr_checkCalDates.Text %>
</script>
<asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>

