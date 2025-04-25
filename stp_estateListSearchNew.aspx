<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_estateListSearchNew.aspx.cs" Inherits="RentalInRome.stp_estateListSearchNew" %>

<%@ Register Src="/ucMain/ucBreadcrumbs.ascx" TagName="breadCrumbs" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%= currStp.meta_title%></title>
    <meta name="description" content="<%=currStp.meta_description %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <style>
        .calendar-input {
            cursor: pointer !important;
        }
    </style>
            <script src="https://maps.google.com/maps/api/js?sensor=false&key=<%=App.GOOGLE_MAPS_KEY %>" type="text/javascript"></script>

    <script type="text/javascript">
               
        var markersArray = [];
        var map;
        var overlay;
        function AjaxList_initializeMap() {

            var mapOptions = {
                center: new google.maps.LatLng(41.941514100000000000, 12.464773700000004000),
                zoom: 11,
                scrollwheel:false,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
            overlay = new google.maps.OverlayView();
            overlay.draw = function () { };
            overlay.setMap(map); // 'map' is new google.maps.Map(...)
        }

        var isLoadMore=true;
        var prevHeight=0;
        $(window).scroll(function () {
            if ($(window).scrollTop() >= $("#property-listing").height() - 200) {
                // console.log( "scrollTop" + $(window).scrollTop()  + ' property-listing-->'+  $("#property-listing").height());
                prevHeight = $("#property-listing").height() - 200;
                console.log("prevHeight :-" + prevHeight);
                if(isLoadMore)
                    AjaxList_LoadMore();
            }
        });

        function ChangeOrder(val)
        {
            if($("#sort_by").val()=="")
            {
                $('#divError').show();
                $('#divError').html('<%= contUtils.getLabel("lblChooseOrderBy") %>');
                return;    
            }
            else
                $('#divError').hide();

            if(val == "desc")
            {
               
                $('#lidesc').addClass('active');
                $('#liasc').removeClass('active');
            }
            else
            {
                $('#lidesc').addClass('active');
                $('#liasc').removeClass('active');
            }
            $("#<%= HF_orderBy.ClientID %>").val(val);
            AjaxList_changeOrderBy(1);
        }
       
        function AjaxList_redrawMap() {
            try {
                google.maps.event.trigger(map, 'resize');
                map.fitBounds(bounds);
                //  if (map.getZoom() > 11) map.setZoom(11);
            } catch (ex) { }
        }
        function AjaxList_mapAddMarker(location, contentString, pagepath) {
            marker = new google.maps.Marker({
                position: location,
                map: map
            });
            with ({ tmp_marker: marker, tmp_contentString: contentString, tmp_pagepath: pagepath }) {
                google.maps.event.addListener(marker, 'mouseover', function () { console.log("aa"); AjaxList_mapInfoShow(tmp_marker, tmp_contentString); });
                google.maps.event.addListener(marker, 'mouseout', function () { AjaxList_mapInfoHide(); });
                google.maps.event.addListener(marker, 'click', function () { window.location.href = tmp_pagepath });
            }
            markersArray.push(marker);
            //addinfo(contentString, marker, pagepath)
        }
        function AjaxList_mapInfoHide() {
            $("#tooltip").css({ 'display': 'none' });
            $("#tooltip .body").html("");
        }
        function AjaxList_mapInfoShow(marker, content) {
            var projection = overlay.getProjection();
            var pixel = projection.fromLatLngToContainerPixel(marker.getPosition());
            var contPos = $("#map-canvas").offset();
            var posX = contPos.left + pixel.x;
            var posY = contPos.top + pixel.y;
            //alert(posX+","+posY);
            
            $("#tooltip .body").html("" + content);
            $("#tooltip").css("left", "" + (posX + 10) + "px");
            $("#tooltip").css("top", "" + (posY + 10) + "px");
            $("#tooltip").css("display", "");            
        }
    </script>
    <script type="text/javascript">
        function AjaxList_changePage(page) {
            $("#<%= HF_currPage.ClientID %>").val("" + page);
            AjaxList_fillList("list");
        }
        function AjaxList_changePrevPage(page) {
            var prevPage = $("#<%= HF_currPage.ClientID %>").val() - 1;
            $("#<%= HF_currPage.ClientID %>").val("" + prevPage);
            AjaxList_fillList("list");
        }
      
        function AjaxList_LoadMore()
        {
            isLoadMore=false; 
            var currCount=$("#<%= HF_numPerPage.ClientID %>").val();
            $("#<%= HF_numPerPage.ClientID %>").val(parseInt(currCount) +9);
            AjaxList_fillList("list",true);
        }

        function AjaxList_changeNextPage(page) {
            var nextPage = $("#<%= HF_currPage.ClientID %>").val() + 1;
            console.log(nextPage);
            $("#<%= HF_currPage.ClientID %>").val("" + nextPage);
            AjaxList_fillList("list");
        }
        function AjaxList_changeNumPerPage(page) {
            $("#<%= HF_numPerPage.ClientID %>").val("" + page);
            AjaxList_fillList("list");
        }

        function AjaxList_changeOrderBy(param) 
        {            
            $("#<%= HF_orderBy.ClientID %>").val($("#sort_by").val());
            var orderHow= $("#lidesc").hasClass("active") ? "desc" :"asc";
            $("#<%= HF_orderHow.ClientID %>").val(orderHow);
            if(param == 1)
                AjaxList_fillList("list",true);
        }

        function AjaxList_changeOrderBy_Old(orderBy, orderHow) {
            var _oldOrderBy = $("#<%= HF_orderBy.ClientID %>").val();
            var _oldOrderHow = $("#<%= HF_orderHow.ClientID %>").val();
            var _newOrderBy = orderBy;
            var _newOrderHow = "";
            if (_newOrderBy == _oldOrderBy)
                _newOrderHow = _oldOrderHow == "asc" ? "desc" : "asc";
            else if (_newOrderBy == "price")
                _newOrderHow = "asc";
            else if (_newOrderBy == "vote")
                _newOrderHow = "desc";
            else if (_newOrderBy == "title")
                _newOrderHow = "asc";
            $("#<%= HF_orderBy.ClientID %>").val(_newOrderBy);
            $("#<%= HF_orderHow.ClientID %>").val(_newOrderHow);
            AjaxList_fillList("list");
        }
        function AjaxList_setOrderBy(orderBy, orderHow) {
            //$("#fltOrderByHow a").attr("class", "");
            //$("#fltOrderBy_" + orderBy).attr("class", "" + orderHow + " current");
            if(orderHow=="asc")
            {
                $("#liasc").addClass("active");
                $("#lidesc").removeClass("active");
            }
            else if(orderHow=="desc"){
                $("#liasc").removeClass("active");
                $("#lidesc").addClass("active");
            }
        }

        function AjaxList_fillList(action, fromLoadMore) {

            if (!fromLoadMore) {
                console.log($("#<%= HF_numPerPage.ClientID %>").val());
                $("#<%= HF_numPerPage.ClientID %>").val("9");
            }
          
            try {
                bounds = new google.maps.LatLngBounds();
            } catch (ex) { }
            if (action == "list") SITE_showLoader();
            var url = "/webserviceNew/rntEstateSearch.aspx";
            var jsondata = {};
            jsondata.lang = "<%= App.LangID %>";
            jsondata.SESSION_ID = "<%= CURRENT_SESSION_ID %>";
            jsondata.dtS = $("#<%= HF_dtStart.ClientID %>").val();
            jsondata.dtE = $("#<%= HF_dtEnd.ClientID %>").val();
            jsondata.numPers_adult = $("#<%= HF_numPers_adult.ClientID %>").val();
            jsondata.numPers_childOver = $("#<%= HF_numPers_childOver.ClientID %>").val();
            jsondata.numPers_childMin = $("#<%= HF_numPers_childMin.ClientID %>").val();
            
            jsondata.currZoneList = $("#<%= HF_currZoneList.ClientID %>").val();
            jsondata.currConfigList = $("#<%= HF_currExtrasList.ClientID %>").val();            
          
            jsondata.prRange = $("#<%= HF_currPriceRange.ClientID %>").val();
            jsondata.voteRange= $("#<%= HF_currVoteRange.ClientID %>").val();
            
            
            jsondata.txt_title =$("#<%= txtTitle.ClientID %>").val();
            jsondata.currPage = $("#<%= HF_currPage.ClientID %>").val();
            jsondata.numPerPage = $("#<%= HF_numPerPage.ClientID %>").val();
            jsondata.orderByHow = $("#<%= HF_orderBy.ClientID %>").val() + "|" + $("#<%= HF_orderHow.ClientID %>").val();
            jsondata.orderBy = $("#<%= HF_orderBy.ClientID %>").val();
            jsondata.orderHow = $("#<%= HF_orderHow.ClientID %>").val();
            $.ajax({
                type: "POST",
                url: url,
                dataType: "json",
                data: jsondata,
                success: function (data) {
                    isLoadMore=true;
                    // $('#currSearchPeriod').html('for <%= HF_dtStart.Value.JSCal_stringToDate().formatITA(false) +" to "+HF_dtEnd.Value.JSCal_stringToDate().formatITA(false) %>')
                    AjaxList_initializeMap();
                    if (data.list.length == 0) {
                        console.log('No data');
                        $('#property-listing').html("");
                        $('#property-listing').hide();
                        //$('#list_pages').html("");                        
                        document.getElementById('div_noresult').style.display = "block";
                        $('#previous').html('');
                        $('#next').html('');
                        //document.getElementById('linkFiltersToggler').style.display = "none";
                        SITE_hideLoader();
                        return;
                    }

                    var bookNow = "<%= contUtils.getLabel("reqBookNow") %>";
                    var listViewList = "";
                    document.getElementById('div_noresult').style.display = "none";

                    var cnt=1;
                    var tempList="";
                    var items = data.list;
                    if(items.length < 3)
                    {
                        listViewList='<div class="row">'; 
                    }

                    $.each(data.list, function (key, value) {
                        var extrasList = '';
                        $.each(value.extrasList, function (extrakey, extra) {
                            extrasList += '<span>' + extra + '</span>';
                        });
                        var scrollableItems='';
                        $.each(value.imgList, function (extrakey, img) {
                            scrollableItems += '<div class="item"><img src="/' + img + '"  alt="" /> </div>';
                        });
                        
                        var listView = '<div class="item col-md-4">\
                                <!-- Set width to 4 columns for grid view mode only -->\
                                <div class="image">  <div id="listItemGallery_'+value.id+'" class="owl-carousel owl-theme listItemGallery">'+  scrollableItems +' </div></div>\
                                <a href="' + value.pagePath + '" class="price" rel="nofollow">\
                                    <i class="fa fa-credit-card"></i>' + bookNow +'\
                                    <span>' + value.priceFormatted + '</span>\
                                </a>\
                                <div class="info">\
                                    <h3>\
                                        <a href="' + value.pagePath + '">' + value.title + '</a>\
                                        <small>' + value.locZoneFull + '</small>\
                                    </h3>';
                        
                        if(value.isBP)
                            listView+= "<img src='/images/css/best-price-guarantee.png' class='bestPrice' alt='Best Price Guarantee' />";
                        
                        listView+='<img alt="rating: '+ value.importance_vote +'" src="/images/vote'+ value.importance_vote +'.gif">\
                                    <p>' + value.summary + '</p>\
                                    <ul class="amenities">\
                                        <li data-toggle="tooltip" title="<%=contUtils.getLabel("lbl_maxPerson")%>" ><i class="fa fa-users"></i>' + value.num_persons_max + '</li>\
                                        <li data-toggle="tooltip" title="<%=contUtils.getLabel("lblBedRooms")%>"><i class="icon-bedrooms"></i>' + value.num_rooms_bed + '</li>\
                                        <li data-toggle="tooltip" title="<%=contUtils.getLabel("lblBathRooms")%>"><i class="fa fa-tint"></i>' + value.num_rooms_bath + '</li>\
                                    </ul>\
                                </div>\
                            </div>';

                        if(items.length < 3)
                        {
                            listViewList+= listView ; 
                        }
                        else
                        {
                            if(cnt ==1)
                            {
                                tempList+='<div class="row">' +listView ; 
                            }
                            else if(cnt==2)
                            {
                                tempList +=listView;
                            }
                            else if(cnt ==3)
                            {
                                tempList += listView +"</div>";
                                listViewList += tempList;
                                tempList="";
                                cnt=0;
                            }
                            cnt =cnt+1;
                        }

                        var infoContent = '\
                        <div class="tabList" id="tabListPhoto1">\
                            <div class="listItemPhoto">\
                                <div style="width:190px;height:190px;border:1px solid #aca3a3;border-radius:2px;">\
                            	    <div class="items">\
                                        <div class="photoItem">\
                                          <img src="' + value.imgPreview + '" style="height:100%;width:100%"/>\
                                        </div>\
                                        </div>\
                                </div>\
                        	    <div class="nulla">\
                                </div>\
                               <a class="photoItemLink" href="' + value.pagePath + '">\
                                    <span class="photoItemDesc">\
                                        <strong class="photoItemTit">'+ value.title + '</strong>\
                                        <em class="photoItemZone">'+ value.locZoneFull + '</em>\
                                    </span>\
                                    <strong class="photoItemPrice">' + value.priceFormatted + '</strong>\
                                </a>\
                            </div></div>';
                        if (value.gmGmapCoords != "") {
                            try {
                                var pos_str = value.gmGmapCoords.split("|");
                                var pos = new google.maps.LatLng(pos_str[0].replace(',', '.'), pos_str[1].replace(',', '.'));
                                //console.log("gmGmapCoords:" + value.gmGmapCoords);
                                AjaxList_mapAddMarker(pos, infoContent, value.pagePath);
                                bounds.extend(pos);
                            } catch (ex) { }
                        }
                    });

                    if(items.length < 3)
                    {
                        listViewList+= "</div>"  ; 
                    }
                    else if(items.length % 3 >0)
                    {
                        listViewList +=tempList +"</div>" ;
                    }

                    $('#property-listing').show();
                    $('#property-listing').html(listViewList);

                    if(parseInt(data.totalCount) > $("#<%= HF_numPerPage.ClientID %>").val())
                        $('#scrollMore').show();
                    else
                    {
                        isLoadMore=false;  
                        $('#scrollMore').hide();
                    }

                    //  $('#list_pages').html(data.pages.pagesHtml);
                    //$('#previous').html('');
                    //$('#next').html('');

                    //if (jsondata.currPage > 0) {
                    //    if (jsondata.currPage > 1) {
                    //        var prevPage = parseInt(jsondata.currPage) - 1;
                    //        $('#previous').html('<li><a href="javascript:AjaxList_changePage(' + prevPage + ')"><i class="fa fa-chevron-left"></i></a></li>');

                    //    }
                    //    if (jsondata.currPage < data.pageSize) {
                    //        var nextPage = parseInt(jsondata.currPage) + 1;
                    //        $('#next').html('<li><a href="javascript:AjaxList_changePage(' + nextPage + ')"><i class="fa fa-chevron-right"></i></a></li>');
                    //    }
                    //}
                    AjaxList_setOrderBy(data.orderBy, data.orderHow);


                    currPriceRange = data.priceRange;                    
                    var minPrice_arr=[];
                    var maxPrice_arr=[];
                    var prMin=parseInt(data.priceRange.min)/100;
                    var prMax=parseInt(data.priceRange.max)/100;
                  
                    prMin=parseInt(prMin);
                    prMax=parseInt(prMax); 
                    var index=0;
                    for (i = prMin; i <= prMax; i++) {
                        minPrice_arr[index]= i *100 ;
                        if(i==prMax)
                        {
                            //console.log(maxPrice_arr);
                            maxPrice_arr[index]= data.priceRange.max ;
                            // console.log(maxPrice_arr);
                        }
                        else
                            maxPrice_arr[index]= (i+1) *100 ;
                        
                        index =index+1;
                    } 
                   
                    currVoteRange = data.voteRange;
                    var minRating_arr=[];
                    var maxRating_arr=[];

                    var rateMin=parseInt(data.voteRange.min);
                    var rateMax=parseInt(data.voteRange.max);
                                    
                    var index=0;
                    for (i = rateMin; i <= rateMax; i++) {
                        minRating_arr[index]= i;
                        maxRating_arr[index]= i;                        
                        index = index+1;
                    } 
                    

                    AjaxList_fillFltListPrice("dropdown", "filter_minPrice", minPrice_arr,'<%= contUtils.getLabel("lblMinPrice")%>',data.priceRange.start);
                    AjaxList_fillFltListPrice("dropdown", "filter_maxPrice", maxPrice_arr,'<%= contUtils.getLabel("lblMaxPrice")%>',data.priceRange.end);
                    onChangeMinPrice();

                    AjaxList_fillFltListRating("dropdown", "filter_minRating", minRating_arr,'<%= contUtils.getLabel("lblMinRating")%>',data.voteRange.start);
                    AjaxList_fillFltListRating("dropdown", "filter_maxRating", maxRating_arr,'<%= contUtils.getLabel("lblMaxRating")%>',data.voteRange.end);
                    onChangeMinRating();
                    
                    AjaxList_fillFltList("checkbox", "fltZoneList", data.fltZoneList,'','<%=contUtils.getLabel("lblZonesOfRome") %>');
                    AjaxList_fillFltList("checkbox", "fltExtrasList", data.fltExtrasList,'', '<%=contUtils.getLabel("lblAmenities") %>');
                    
                    $("#sort_by").val(data.orderBy);

                    if(data.orderHow=='desc')
                    {
                        $("#lidesc").attr('class','active');
                        $("#liasc").removeAttr('class');
                    }
                    else
                    {
                        $("#lidesc").removeAttr('class');
                        $("#liasc").attr('class','active');
                    }
 
                    AjaxList_redrawMap();
                    setScrollable();
                    if (action == "list") {
                        SITE_hideLoader();
                    }

                    setToolTip();
                }
            });
        }
       

        var currPriceRange;
        var currVoteRange;
        function onChangeMinPrice(){
            $("#filter_minPrice").on('change',function(){
                var maxPrice_arr=[];
                var prMin=parseInt($(this).val())/100;
                var prMax=parseInt(currPriceRange.max)/100;
             
                prMin=parseInt(prMin)+1;
                prMax=parseInt(prMax)+1; 

                var index=0;
                for (i = prMin; i <= prMax; i++) {
                    if(i==prMax)
                    {                        
                        maxPrice_arr[index]= currPriceRange.max;
                    }
                    else
                        maxPrice_arr[index]= i *100 ;

                    index =index+1;
                } 
                AjaxList_fillFltListPrice("dropdown", "filter_maxPrice", maxPrice_arr,'<%= contUtils.getLabel("lblMaxPrice")%>',0);
            });

        }

        function onChangeMinRating(){
            $("#filter_minRating").on('change',function(){
                var maxRating_arr = [];
                
                var prMin= $(this).val();                        
                prMin = prMin == "" ? currVoteRange.min : prMin;
                
                var prMax= currVoteRange.max;                
                
                var index = 0;
                for (i = prMin; i <= prMax; i++) 
                {                
                    maxRating_arr[index]= i;
                    index = index+1;
                } 
                AjaxList_fillFltListRating("dropdown", "filter_maxRating", maxRating_arr,'<%= contUtils.getLabel("lblMaxRating")%>',0);
            });
        }

        function AjaxList_fillFltListPrice(type, container, data , defVal,selectedVal) {
            if (type == "dropdown") {
                var html = "";
                if(defVal !="" && defVal !="undefined")
                    html += '<option value="">-' + defVal + '-</option>';
                 
                for(index=0; index < data.length; index++)
                {
                    if(container=="filter_maxPrice" && index==data.length-1)
                    {
                        // for last item in max price drp 
                        //console.log(parseInt(data[index]/100)*100  +" Last max price ");
                        if(selectedVal== data[index])
                            html += '<option value="' + data[index] + '" selected="selected" > € ' + parseInt(data[index]/100)*100 +  ' + </option>';
                        else
                            html += '<option value="' + data[index] + '" > € ' + parseInt(data[index]/100)*100 + ' + </option>';
                    }
                    else
                    {
                        if(selectedVal== data[index])
                            html += '<option value="' + data[index] + '" selected="selected" > € ' + data[index] + '</option>';
                        else
                            html += '<option value="' + data[index] + '" > € ' + data[index] + '</option>';
                    }
                }

                if (html == "") $("#" + container + "Title").hide();
                //    else $("#" + container + "Title").show();
                //$("#" + container + "Cont").html("<select id='" + container + "' name='" + container + "' onchange='setFltValues()'>" + html + "</select>");

                $("#" + container + "Cont").html("<select id='" + container + "' name='" + container + "'>" + html + "</select>");
                if($("#" + container).length){
                    $("#" + container).chosen({
                        allow_single_deselect: true,
                        disable_search_threshold: 12
                    });
                }
            }
        }

        function AjaxList_fillFltListRating(type, container, data , defVal,selectedVal) {
            if (type == "dropdown") {
                var html = "";
                if(defVal !="" && defVal !="undefined")
                    html += '<option value="">-' + defVal + '-</option>';
                 
                for(index = 0; index < data.length; index++)
                {
                    if(selectedVal== data[index])
                        html += '<option value="' + data[index] + '" selected="selected" >' + data[index] +  '</option>';
                    else
                        html += '<option value="' + data[index] + '" >' + data[index] + '</option>';
                }

                if (html == "") $("#" + container + "Title").hide();
              
                $("#" + container + "Cont").html("<select id='" + container + "' name='" + container + "'>" + html + "</select>");
                if($("#" + container).length){
                    $("#" + container).chosen({
                        allow_single_deselect: true,
                        disable_search_threshold: 12
                    });
                }
            }
        }
        function AjaxList_fillFltList(type, container, data , defVal, title) { 
            if (type == "checkbox") {
                var html = "";
                //if(container=="fltZoneList"){
                //html+='\
                //<div class="checkbox col-md-4">\
                //<label>\
                //<input type="checkbox" name="terms" id="rowchkall" class="' + container + '_all "value="All" onclick="setAll()" /><%= contUtils.getLabel("lblAll") %>\
                //</label>\
                //</div>';
                //}
                var index = 0;
                $.each(data, function (key, value) {
                    index++;
                    html += '\
                        <div class="checkbox col-md-4">\
                            <label>\
                                <input type="checkbox" name="terms" id="' + container + '_' + index + '" class="' + container + '" value="' + value.value + '" ' + (value.selected ? ' checked="checked"' : '') + '/>\
                                ' + value.text + '\
                            </label>\
                        </div>';
                    //<input type="checkbox" name="terms" id="' + container + '_' + index + '" class="' + container + '" onclick="setFltValues()" value="' + value.value + '" ' + (value.selected ? ' checked="checked"' : '') + '/>\
                });
                if (html == "") 
                { 
                    $("#" + container + "Title").hide(); 
                    $("#" + container + "Cont").hide(); 
                }
                else 
                { 
                    $("#" + container + "Title").show(); 
                    $("#" + container + "Cont").show(); 
                    html = '<h3 class="section-title">'+ title +'</h3><div class="nulla"></div>' + html;
                }
                $("#" + container + "Cont").html(html);
                //if($("." + container).length){
                //    $("." + container).chosen({
                //        checkedClass: 'fa fa-check-square-o',
                //        uncheckedClass: 'fa fa-square-o'
                //    });
                //}
                if ($('input[type="checkbox"]').length) {
                    $('input[type="checkbox"]').checkbox({
                        checkedClass: 'fa fa-check-square-o',
                        uncheckedClass: 'fa fa-square-o'
                    });
                }
            }
            else if (type == "dropdown") {
                var html = "";
                if(defVal !="" && defVal !="undefined")
                    html += '<option value="">-' + defVal + '-</option>';
                var index = 0;
                $.each(data, function (key, value) {
                    index++;
                    html += '<option value="' + value.value + '" ' + (value.selected ? ' selected="selected"' : '') + '>' + value.text + '</option>';
                });
                if (html == "") $("#" + container + "Title").hide();
                else $("#" + container + "Title").show();
                //$("#" + container + "Cont").html("<select id='" + container + "' name='" + container + "' onchange='setFltValues()'>" + html + "</select>");
                $("#" + container + "Cont").html("<select id='" + container + "' name='" + container + "'>" + html + "</select>");
                if($("#" + container).length){
                    $("#" + container).chosen({
                        allow_single_deselect: true,
                        disable_search_threshold: 12
                    });
                }
            }
        }
     
        function fltZoneList_setValues() {
            var _arrStr = "";
            var _arrSep = "";
            $(".fltZoneList").each(function () {
                if ($(this).is(':checked')) {
                    _arrStr += _arrSep + "" + $(this).val();
                    _arrSep = "|";
                }
            });
            $("#<%= HF_currZoneList.ClientID %>").val(_arrStr);
        }
    
        function fltExtrasList_setValues() {
            var _arrStr = "";
            var _arrSep = "";
            $(".fltExtrasList").each(function () {                
                if ($(this).is(':checked')) {
                    _arrStr += _arrSep + "" + $(this).val();
                    _arrSep = "|";
                }
            });
            $("#<%= HF_currExtrasList.ClientID %>").val(_arrStr);
        }

        function fltBedroomList_setValues() {
            var val = $("#filter_sleepingrooms").val();
            $("#<%= HF_currBedroomsList.ClientID %>").val(val);
        }
        function fltBathroomList_setValues() {
            var val = $("#filter_bathrooms").val();            
            $("#<%= HF_currBathroomsList.ClientID %>").val(val);
        }

       
        function setFltValues() {
            fltZoneList_setValues();
            fltExtrasList_setValues();

            $("#<%= HF_currPriceRange.ClientID %>").val($("#filter_minPrice").val() +"|"+  $("#filter_maxPrice").val());
            $("#<%= HF_currVoteRange.ClientID %>").val($("#filter_minRating").val() +"|"+  $("#filter_maxRating").val());
            console.log("New Price" +  $("#<%= HF_currPriceRange.ClientID %>").val());
            AjaxList_fillList("list",false);
        }


        function setAll(){    
            alert("setAll");
            // $("input:checkbox.fltZoneList").prop('checked',$("#rowchkall").is(':checked'));
            //$("input:checkbox.fltZoneList").setChecked($("#rowchkall").is(':checked'));
            $("input:checkbox.fltZoneList").checkbox('click');
        }
    </script>

    <style>
        .chzn-container-single .chzn-single div b {
            margin-top: 13px!important;
        }

        #property-listing {
            min-height: 300px;
        }

        @media (max-width:767) {
            #property-listing {
                min-height: auto;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" />
    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
    <asp:HiddenField ID="HF_currPage" runat="server" Value="1" />
    <asp:HiddenField ID="HF_numPerPage" runat="server" Value="9" />
    <asp:HiddenField ID="HF_activeZones" runat="server" Value="" />
    <asp:HiddenField ID="HF_currExtrasList" runat="server" Value="" />
    <asp:HiddenField ID="HF_currCity" runat="server" Value="" />
    <asp:HiddenField ID="HF_currZoneList" runat="server" Value="" />
    <%--<asp:HiddenField ID="HF_currConfigList" runat="server" Value="" />--%>

    <asp:HiddenField ID="HF_currPriceRange" runat="server" Value="0|0" />
    <asp:HiddenField ID="HF_currVoteRange" runat="server" Value="0|0" />

    <asp:HiddenField ID="HF_numPers_adult" runat="server" Value="0" />
    <asp:HiddenField ID="HF_numPers_childOver" runat="server" Value="0" />
    <asp:HiddenField ID="HF_numPers_childMin" runat="server" Value="0" />

    <asp:HiddenField ID="HF_numSleeps" runat="server" Value="0" />

    <asp:HiddenField ID="HF_orderBy" runat="server" Value="" />
    <asp:HiddenField ID="HF_orderHow" runat="server" Value="" />

    <asp:HiddenField ID="HF_currBedroomsList" runat="server" Value="" />
    <asp:HiddenField ID="HF_currBathroomsList" runat="server" Value="" />
    <asp:HiddenField ID="HF_currSleepList" runat="server" Value="" />
    <!-- BEGIN HOME ADVANCED SEARCH -->
    
<script>
    $(document).ready(function(){
        $(".new-search").on("click", function(){
            $(".find-rome").slideToggle();
        });
    });
</script>
    <div id="home-advanced-search" class="listSearch open">
        <div id="opensearch"></div>
        <div class="container" style="overflow: visible;">
            <div class="row">
                <div class="col-sm-12">
                    <div>
                        <div class="new-search">
                            <i class="fa fa-calendar"></i>NEW SEARCH
                        </div>
                        <div class="form-group find-rome">
                            <div class="form-control-large r-pad">
                                <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" class="form-control calendar-input" placeholder="Check-in" />
                                <a class="cal" id="startCalTrigger_<%= Unique %>"></a>
                                <asp:HiddenField ID="HF_dtStartTmp" runat="server" Value="0" />
                            </div>

                            <div class="form-control-large l-pad">
                                <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" class="form-control calendar-input" placeholder="Check-out" />
                                <a class="cal" id="endCalTrigger_<%= Unique %>"></a>
                                <asp:HiddenField ID="HF_dtEndTmp" runat="server" Value="0" />
                            </div>

                            <div class="form-control-small r-pad">
                                <asp:TextBox ID="txt_dtCount_night" runat="server" class="form-control" placeholder="Nights"></asp:TextBox>
                            </div>

                            <div class="form-control-small l-pad">
                                <asp:DropDownList ID="drp_numPers_adult" runat="server" data-placeholder="Adults">
                                </asp:DropDownList>
                            </div>

                            <div class="form-control-small r-pad">
                                <asp:DropDownList ID="drp_numPers_children" runat="server" data-placeholder="Children">
                                </asp:DropDownList>
                            </div>

                            <div class="form-control-small l-pad">
                                <asp:DropDownList ID="drp_numPers_infants" runat="server" data-placeholder="Infants">
                                </asp:DropDownList>
                            </div>
                            <a href="#" onclick="$('#<%= HF_currPage.ClientID %>').val('1'); return submitForm();" class="btn btn-fullcolor"><%= contUtils.getLabel("lblSearch")%></a>
                        </div>

                        <script type="text/javascript">
                            function submitForm(){
                                $("#<%= HF_numPers_adult.ClientID %>").val($("#<%= drp_numPers_adult.ClientID %>").val());
                                $("#<%= HF_numPers_childOver.ClientID %>").val($("#<%= drp_numPers_children.ClientID %>").val());
                                $("#<%= HF_numPers_childMin.ClientID %>").val($("#<%= drp_numPers_infants.ClientID %>").val());
                                $("#<%= HF_dtStart.ClientID %>").val($("#<%= HF_dtStartTmp.ClientID %>").val());
                                $("#<%= HF_dtEnd.ClientID %>").val($("#<%= HF_dtEndTmp.ClientID %>").val());       
                                
                                $("#<%= HF_currZoneList.ClientID %>").val("");
                                $("#<%= HF_currExtrasList.ClientID %>").val("");
                                $("#<%= txtTitle.ClientID %>").val("");

                                $("#<%= HF_currPriceRange.ClientID %>").val("0|0");
                                $("#<%= HF_currVoteRange.ClientID %>").val("0|0");
                                
                                AjaxList_fillList("list",false);
                                return false;
                            }
                                  
                            var _JSCal_Range;
    
                            function setCal() {
                                _JSCal_Range = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.JSCal_dateToInt() %>, startCont: "#<%= HF_dtStartTmp.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEndTmp.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endTrigger: "#endCalTrigger_<%= Unique %>", countCont: "#<%= txt_dtCount_night.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
                            }
                            <%= ltr_checkCalDates.Text %>

                        </script>
                        <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <!-- END HOME ADVANCED SEARCH -->

    <!-- BEGIN CONTENT WRAPPER -->
    <div class="content listContent">
        <div class="container">
            <div class="row">
                <!-- BEGIN MAP -->
                <div class="sidebar col-sm-5 mapListCont" id="mapList">
                    <div class="mapList" id="map-canvas" style="width: 100%; height: 100%;">
                    </div>
                </div>

                <!-- END MAP -->

                <!-- BEGIN LIST  MAIN CONTENT -->
                <div class="main col-sm-7">
                    <uc1:breadCrumbs ID="breadcrumbs" runat="server" />

                    <h1 class="section-title" data-animation-direction="from-top" data-animation-delay="50">
                        <%= contUtils.getLabel("lblSearchResult") %>
                        <div style="font-size: 14px; color: rgb(51, 51, 51);" id="currSearchPeriod">
                        </div>
                    </h1>
                    <div class="clearfix" id="listing-header">
                        <div class="form-control-small monon">
                            <select id="sort_by" name="sort_by" data-placeholder="Sort">
                                <option value=""></option>
                                <option value="price"><%= contUtils.getLabel("lblOrderBy") %> <%= contUtils.getLabel("lblPrice") %></option>
                                <option value="name"><%= contUtils.getLabel("lblOrderBy") %> <%= contUtils.getLabel("lblName") %></option>
                            </select>
                        </div>

                        <div class="sort monon">
                            <ul>
                                <li id="lidesc" onclick="ChangeOrder('desc')"><i class="fa fa-chevron-down" title="" data-placement="top" data-toggle="tooltip" data-original-title="Sort Descending"></i></li>
                                <li id="liasc" onclick="ChangeOrder('asc')" class="active"><i class="fa fa-chevron-up" title="" data-placement="top" data-toggle="tooltip" data-original-title="Sort Ascending"></i></li>
                            </ul>
                        </div>

                        <div class="sort  alert alert-danger" id="divError" style="display: none">
                        </div>

                        <button id="filtersListBtn" class="btn btn-default-color btn_filters_list openFiltersList" type="button">
                            <%= contUtils.getLabel("lblFiltraIRisultati") %> <i class="fa fa-sort-desc"></i>
                        </button>

                        <div class="view-mode monon">
                            <span><%= contUtils.getLabel("lblViewMode") %></span>
                            <ul>
                                <li class="active" data-target="property-listing" data-view="list-style"><i class="fa fa-th-list"></i></li>
                                <li data-target="property-listing" data-view="grid-style1"><i class="fa fa-th"></i></li>
                            </ul>
                        </div>
                    </div>

                    <!-- BEGIN FILTERS -->
                    <div id="filtersListCont" class="sidebar gray filtersListCont">

                        <h2 class="section-title" style="padding-left: 15px;"><%= contUtils.getLabel("lblFiltraIRisultati") %></h2>

                        <div class="form-group">

                            <div class="col-sm-12">
                                <%--<input type="text" id="txt_title" class="form-control" placeholder="<%=contUtils.getLabel("lblApartmentName") %>" />--%>
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="col-md-3" id="filter_minPriceCont">
                            </div>
                            <div class="col-md-3" id="filter_maxPriceCont">
                            </div>


                            <div class="col-md-3" id="filter_minRatingCont">
                            </div>
                            <div class="col-md-3" id="filter_maxRatingCont">
                            </div>
                            <div class="nulla">
                            </div>
                            <hr />
                            <div class="nulla">
                            </div>
                            <div class="row" data-animation-direction="from-bottom" data-animation-delay="100" id="fltZoneListCont">
                            </div>

                            <div class="nulla">
                            </div>

                            <hr />

                            <div class="nulla">
                            </div>

                            <div class="row" data-animation-direction="from-bottom" data-animation-delay="100" id="fltExtrasListCont">
                            </div>


                            <div class="nulla">
                            </div>

                            <div class="col-md-4">
                                <button type="submit" class="btn btn-default-color" onclick="setFltValues(); return false;"><%= contUtils.getLabel("lblFiltraIRisultati") %></button>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                    </div>
                    <!-- END FILTERS -->


                    <!-- BEGIN PROPERTY LISTING -->
                    <div class="list-style" id="property-listing">
                        <div class="loadingSrc">
                            <span>
                                <%=CurrentSource.getSysLangValue("lblLoadingData")%>
                            </span>
                        </div>

                    </div>
                    <!-- END PROPERTY LISTING -->
                    <div id="div_noresult" class="col-md-6 grid-style1" style="display: none; margin: 20px">
                        <div class="alert alert-danger"><%=contUtils.getLabel("lblApartmentSearchError")%></div>
                    </div>

                    <div class="center" id="scrollMore" style="display: none">
                        <div class="row">
                            <h4>
                                <%=contUtils.getLabel("lblScrolltoLoadMore")%>     <i class="fa fa-caret-down" aria-hidden="true"></i>
                            </h4>
                        </div>
                    </div>
                    <!-- BEGIN PAGINATION -->
                    <%--<div class="pagination">
                        <ul id="previous">
                        </ul>
                        <ul id="list_pages">
                        </ul>
                        <ul id="next">
                        </ul>
                    </div>--%>
                    <!-- END PAGINATION -->

                </div>
                <!-- END LIST MAIN CONTENT -->



            </div>
        </div>
    </div>
    <!-- END CONTENT WRAPPER -->

    <a href="#" class="backToTop">
        <img src="/images/css/back-to-top.png" alt="Back to top" />
    </a>


    <script type="text/javascript">
        /* ADAPT MAP TO HEADER RESIZING */
        $(window).scroll(function () {

            if ($(this).scrollTop() > 2) {
                $('#mapList').addClass("mapHiddenTop");
            } else {
                $('#mapList').removeClass("mapHiddenTop");
            }

            if ($(this).scrollTop() > 320) {
                $('#mapList').addClass("mapShrinkTop");
            } else {
                $('#mapList').removeClass("mapShrinkTop");
            }

        });

        // "BACK TO TOP BUTTON"

        $(window).scroll(function () {
            if ($(this).scrollTop() > 500) {
                $('.backToTop').fadeIn();
            } else {
                $('.backToTop').fadeOut();
            }
        });
        $('.backToTop').click(function () {
            $('html, body').animate({ scrollTop: 0 }, 800);
            return false;
        });


        /* OPEN/CLOSE FILTERS */
        $("#filtersListBtn").click(function () {
            $("#filtersListCont").slideToggle();
            if ($(this).hasClass("openFiltersList")) {
                $(this).removeClass("openFiltersList");
                $(this).addClass("closeFiltersList");
                $(".btn_filters_list .fa").removeClass("fa-sort-desc");
                $(".btn_filters_list .fa").addClass("fa-sort-asc");
            } else if ($(this).hasClass("closeFiltersList")) {
                $(this).removeClass("closeFiltersList");
                $(this).addClass("openFiltersList");
                $(".btn_filters_list .fa").removeClass("fa-sort-asc");
                $(".btn_filters_list .fa").addClass("fa-sort-desc");
            }
        });
      
        $(document).ready(function () {
            $(window).load(function () {                
                try {
                    AjaxList_initializeMap();
                } catch (ex) { }
              
                
                $("#sort_by").change(function() {
                    $("#divError").hide();
                    AjaxList_changeOrderBy(1);
                });

                AjaxList_fillList("first",false);
             
            });
        });

        function ChoosenSelect() {
            $(".chzn-select").chosen();
        }
        function setScrollable(){
            $(".listItemGallery").owlCarousel({ navigation : true, slideSpeed : 300, paginationSpeed : 400, singleItem:true });
        }
	  

        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest)
        //function EndRequest(sender, args) {
        //    if (args.get_error() == undefined) {
        //        ChoosenSelect();
        //    }
        //}
        
    </script>

</asp:Content>

