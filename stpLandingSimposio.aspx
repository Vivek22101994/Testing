<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stpLandingSimposio.aspx.cs" Inherits="RentalInRome.stpLandingSimposio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Rental in Rome Symposium 2015</title>
    <link href="/jquery/plugin/shadowbox/shadowbox.css" rel="stylesheet" type="text/css" />
    <script src="/jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>
      <script src="/jquery/js/ui-all.min.js" type="text/javascript"></script>
    <script src="/jquery/datepicker-lang/jquery.ui.datepicker-<%= CurrentLang.JS_CAL_FILE %>" type="text/javascript"></script>
    <script src="/jquery/plugin/scrollTo/scrollTo.min.js" type="text/javascript"></script>
    <%--<script src="/js/shadowbox/shadowbox.js" type="text/javascript"></script
    <script src="/js/shadowbox/players/shadowbox-iframe.js" type="text/javascript"></script>
    <script src="/js/shadowbox/players/shadowbox-html.js" type="text/javascript"></script>>--%>
    <script type="text/javascript" src="/js/jquery.tooltip.js"></script>

<%--    <link href="/css/style-simposio2015.css" rel="stylesheet" type="text/css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/style_new.css" rel="stylesheet" type="text/css" />--%>

     <style type="text/css">
        @import url(/css/style-simposio2015.css<%="?tmp="+DateTime.Now.Ticks %>);
        @import url(/css/style.css<%="?tmp="+DateTime.Now.Ticks %>);
        @import url(/css/style_new.css<%="?tmp="+DateTime.Now.Ticks %>);        
        @import url(/jquery/css/ui-datepicker.css);        
        @import url(/jquery/css/ui-autocomplete.css);
        @import url(/css/styleagent.css);
    </style>
    <script type="text/javascript">
        $(function () {
            $(window).load(function () {
                $.getScript("/jquery/plugin/shadowbox/shadowbox.js", function (data, textStatus, jqxhr) {
                    Shadowbox.init({ modal: true, handleOversize: "none" });
                });
            });
        });
        function SITE_addDinamicLabel(labelId, controlId) {
            $('#' + controlId).bind("blur", function () { if ($.trim($('#' + controlId).val()) == "") { $('#' + labelId).css("display", "block"); } else { $('#' + labelId).css("display", "none"); } });
            $('#' + labelId).bind("click", function () { $('#' + controlId).focus(); $('#' + labelId).css("display", "none"); });
            $('#' + controlId).bind("focus", function () { $('#' + labelId).css("display", "none"); });
            if ($.trim($('#' + controlId).val()) == "") { $('#' + labelId).css("display", "block"); } else { $('#' + labelId).css("display", "none"); }
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="HF_unique" Value="" runat="server" />
    <div class="containerSimposio">

	<header class="headerSimposio"><div>
    	<a href="/" class="logoSimposio">
        	<img src="images/logo-rentalinrome-simposio2015.jpg" alt="Rental in Rome" />
        </a>
        <h1 class="titMainSimposio">Symposium<span>2015</span></h1>
        <div class="nulla">
        </div>
        <h2 class="subTitMainSimposio">
            <strong class="subTitMainSimposioLine2"><span>only</span> for participants</strong>
            <strong class="subTitMainSimposioLine3"><span>Special prices</span>&nbsp; for staying at</strong>
            <strong class="subTitMainSimposioLine4">one of these <span>magnificent hotels</span>:</strong>
        </h2>
        <span class="infoClickSimposio">Click on a thumbnail below for more info about the hotel:</span>
        
        <div class="nulla">
        </div>
    </div></header>
    
    <div class="nulla">
    </div>
        
    <div class="hotelThumbsSimposioCont">
    
    <div class="hotelThumbsSimposio">
    
       
        <a href="/pgHotelSimposio.aspx" rel="shadowbox" class="hotelThumbSimposio">
            <h3 class="nameHotelSimposio">Hotel American Palace Eur</h3>
            <img src="images/hotel-simposio-american-palace-thumb.jpg" alt="Hotel American Palace Eur" />
        </a>
        
        <a href="/pgHotelSimposio_2.aspx" rel="shadowbox" class="hotelThumbSimposio">
            <h3 class="nameHotelSimposio">Hotel Abitart</h3>
            <img src="images/hotel-simposio-abitart-thumb.jpg" alt="Hotel Abitart" />
        </a>
        
        <a href="/pgHotelSimposio_3.aspx" rel="shadowbox" class="hotelThumbSimposio">
            <h3 class="nameHotelSimposio">Hotel Pulitzer</h3>
            <img src="images/hotel-simposio-pulitzer-thumb.jpg" alt="Hotel Pulitzer" />
        </a>
        
        <a href="/pgHotelSimposio_4.aspx" rel="shadowbox" class="hotelThumbSimposio">
            <h3 class="nameHotelSimposio">Hotel Santa Prisca</h3>
            <img src="images/hotel-simposio-prisca-thumb.jpg" alt="Hotel Santa Prisca" />
        </a>
    
    </div>
    
    <div class="nulla">
    </div>
    </div>
    
    <div class="searchSimposioCont"><div>
    
        <h2 class="titsearchSimposio">
            <strong class="titsearchSimposioLine1">Would you rather <span>stay in an apartment</span>?</strong>
            <strong class="titsearchSimposioLine2">Look for the one which <span>best meets your needs</span></strong>
            <strong class="titsearchSimposioLine3">among our over <span>500 available apartments</span></strong>
        </h2>
        
        <div class="formHome" id="SearchHomepage">
           
                <label id="lb_QuickSearch" style="position: absolute; display: block; width: 190px; margin:19px;"> <%=CurrentSource.getSysLangValue("lblQuickSearch")%></label>
                 <asp:TextBox ID="txt_title" runat="server" CssClass="search_aptComplete quickSearchInput"></asp:TextBox>
                <asp:HiddenField ID="HF_estateId" runat="server" Value="0" />
                <asp:HiddenField ID="HF_estatePath" runat="server" Value="" />
                <%--<input type="text" value="Quick Search" class="quickSearchInput">--%>

                <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" class="checkInOutInput" />
                <a class="ico_cal" id="startCalTrigger_<%= Unique %>"></a>
                <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />

                <%--<input type="text" value="Check-In" class="checkInOutInput">
                <a class="ico_cal"></a>--%>
            
                <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" class="checkInOutInput" />
                <a class="ico_cal" id="endCalTrigger_<%= Unique %>"></a>
                <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />

                <%--<input type="text" value="Check-Out" class="checkInOutInput">
                <a class="ico_cal"></a>--%>
            
                <label>Nights</label>
                 <asp:TextBox ID="txt_dtCount" runat="server" Text="3" CssClass="nightsInput"></asp:TextBox>
                <%--<input type="text" value="3" class="nightsInput">--%>
              

                <label>Guests</label>
                 <asp:DropDownList ID="drp_num_persons_max" runat="server"  CssClass="guestSelect">
                </asp:DropDownList>
                <%--<select class="guestSelect">
                    <option>1</option>
                    <option>2</option>
                    <option>3</option>
                </select>--%>
            
                <label class="zoneSelectLabel">Zone</label>
                <asp:DropDownList ID="drp_zone" runat="server" CssClass="zoneSelect">
                </asp:DropDownList>
                <%--<select class="zoneSelect">
                    <option>- - - </option>
                    <option>Colosseo</option>
                    <option>Trastevere</option>
                    <option>Termini</option>
                </select>--%>

                <asp:LinkButton ID="lnk_search" runat="server" CssClass="searchHomeBtn" onclick="lnk_search_Click"></asp:LinkButton>
                <%--<a class="searchHomeBtn"></a>--%>
           
        </div>
        
        <img class="imgSearchAptSimposio" src="images/apartment-search-simposio.jpg" alt="Search apartment" />
        
        <div class="codeSimposioApt">
            <span>Use this code to get a <strong>5% discount</strong>:</span>
            <strong>A M ISPSO 2015</strong>
        </div>
    
    </div></div>
    
    <div class="nulla">
    </div>
        
        <script type="text/javascript">
            var _JSCal_Range_<%= Unique %>;
            function setCal_<%= Unique %>(dtStartInt, dtEndInt) {
                _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", countCont: "#<%= txt_dtCount.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
            }
    <%= ltr_checkCalDates.Text %>
            RNT = {};
            RNT.EstateOptions = {
                id: 0,
                path: "",
                label: "",
                pid_zone: 0
            };
            var _estateList_<%= Unique %> = new Array();
            function getEstateXml_<%= Unique %>(){
                var _xml = $.ajax({
                    type: "GET",
                    url: "/webservice/rnt_estate_list_xml.aspx?lang=<%= CurrentLang.ID %>&SESSION_ID=<%= CURRENT_SESSION_ID %>",
                    dataType: "xml",
                    success: function(xml) {
                        $(xml).find('item').each(function() {
                            var _estOpt = {
                                id: parseInt($(this).find('id').text(), 10),
                                path: $(this).find('path').text(),
                                label: $(this).find('title').text(),
                                pid_zone: parseInt($(this).find('pid_zone').text(), 10)
                            };
                            _estateList_<%= Unique %>.push(_estOpt);
                        });
                        setAutocomplete_<%= Unique %>();
                    }
                });
            }
            function setAutocomplete_<%= Unique %>(){
                $( ".search_aptComplete" ).autocomplete({
                    source: _estateList_<%= Unique %>,
                    search: function( event, ui ) {
                        $( "#<%= HF_estateId.ClientID%>" ).val( '0' );
                        $( "#<%= HF_estatePath.ClientID%>" ).val( '' );
                    },
                    select: function( event, ui ) {
                        window.location = "/"+ui.item.path;
                        return;
                        $( "#<%= HF_estateId.ClientID%>" ).val( ui.item.id );
                        $( "#<%= HF_estatePath.ClientID%>" ).val( ui.item.path );
                        if(ui.item.id!=0 && ui.item.path!='')
                            $( "#zoneCont_<%= Unique %>" ).hide();
		        else
		            $( "#zoneCont_<%= Unique %>" ).show();
                    }
                });
        //alert(_estateList_<%= Unique %>);
    }
            getEstateXml_<%= Unique %>();
            SITE_addDinamicLabel('lb_QuickSearch', '<%= txt_title.ClientID %>');
        </script>
    <footer class="footerSimposio"><div>
    
    	<div class="datiSocSimposio">
        	<strong style="color:#fff;">Rental in Rome S.r.l.</strong> - VAT: 07824541002 - 2003-2014 © all rights reserved
            <br />
            <strong>Operative Office:</strong> Via Marianna Dionigi, 57 - 00193 Rome (Italy)
            <br />
            <strong>Administrative Office:</strong> Via Appia Nuova, 677 - 00179 Rome (Italy)
        </div>
        
        <img src="images/tel-footer-simposio-2015.gif" class="telFooterSimposio" alt="Tel: +39 06 32 20 068" />
        
        <a class="emailFooterSimposio" href="mailto:info@rentalinrome.com">
        	<img src="images/email-footer-simposio-2015.gif" align="left" />
            info@rentalinrome.com
        </a>
            
    
    <div class="nulla">
    </div>
    </div></footer>

</div>
        <div id="divNewWin" style="display:none;">
            <a id="lnk_new_window" href="<%=CurrentSource.getPagePath("6", "stp", CurrentLang.ID.ToString()) %>" target="_blank">Test</a>
        </div>
        <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
    </form>
</body>
</html>
