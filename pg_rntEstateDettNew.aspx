<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="pg_rntEstateDettNew.aspx.cs" Inherits="RentalInRome.pg_rntEstateDettNew" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="ucMain/ucBooking.ascx" TagName="ucBooking" TagPrefix="uc2" %>
<%@ Register Src="~/ucMain/ucBreadcrumbs.ascx" TagName="breadCrumbs" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title><%= currEstateLN.meta_title%></title>
    <meta name="description" content="<%=currEstateLN.meta_description %>" />
    <meta name="keywords" content="<%=currEstateLN.meta_keywords %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <link type="text/css" rel="stylesheet" href="/css/lightgallery.css" />
    <script type="text/javascript" src="/js/lightgallery.min.js"></script>
    <script type="text/javascript" src="/js/lg-fullscreen.min.js"></script>
    <script type="text/javascript" src="/js/froogaloop2.min.js"></script>
    <style>
        .videopop {
            display: none;
            opacity: 1;
            background: #000;
            width: 100%;
            height: 100%;
            position: fixed;
            top: 0;
            left: 0;
            z-index: 1050;
            -webkit-transition: opacity 0.15s ease 0s;
            -o-transition: opacity 0.15s ease 0s;
            transition: opacity 0.15s ease 0s;
        }

            .videopop .videocon {
                position: absolute;
                top: 50%;
                left: 50%;
                -webkit-transform: translate(-50%, -50%);
                -ms-transform: translate(-50%, -50%);
                -o-transform: translate(-50%, -50%);
                transform: translate(-50%, -50%);
                display: block;
                width: 100%;
                text-align: center;
            }

                .videopop .videocon iframe {
                    display: inline-block;
                    max-width: 100%;
                }
    </style>
    <script type="text/javascript">
        function RNT_estComment_changePage(page) {
            $("#hf_estComment_currPage").val("" + page);
            RNT_estComment_fillList("list");        }

        function RNT_estComment_fillList(action) {
            if (action == "list") SITE_showLoader();
            var _url = "/webservice/rnt_estate_comment.aspx";
            _url += "?action=" + action;
            _url += "&lang=<%= CurrentLang.ID %>";
            _url += "&currEstate=" + $("#<%= HF_id.ClientID%>").val();
            _url += "&voteRange=";
            _url += "&currPage=1";
            _url += "&numPerPage=20";
            _url += "&fullView=0";
            _url += "&isNew=1";            
            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function (html) {
                    $("#commentListCont").html(html);
                    if (action == "list") SITE_hideLoader();
                }
            });
        }
        $(document).ready(function () {
            RNT_estComment_fillList("first");            
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".btn-read-more").click(function () {
                $(".property-text").animate({ height: $(".property-text").get(0).scrollHeight }, 500);
                $(".property-text").removeClass('hidetxt');
                $(".btn-read-more").addClass('hidetxt');
                $(".btn-read-less").removeClass('hidetxt');
            });
            $(".btn-read-less").click(function () {
                $(".property-text").animate({ height: "63px" }, 500);
                $(".property-text").addClass('hidetxt');
                $(".btn-read-more").removeClass('hidetxt');
                $(".btn-read-less").addClass('hidetxt');
            });
        });
    </script>
    <script type="text/javascript">

        function setValueForRequest(_startdate, _endDate, numAdult, numChild, numInfant) {
            $("#<%= HF_dtStart.ClientID %>").val(_startdate);
            $("#<%= HF_dtEnd.ClientID %>").val(_endDate);
            $("#<%= HF_num_adult.ClientID %>").val(numAdult);
            $("#<%= HF_num_child.ClientID %>").val(numChild);
            $("#<%= HF_num_infant.ClientID %>").val(numInfant);
            console.log($("#<%= HF_dtStart.ClientID %>").val() + " - " + $("#<%= HF_dtEnd.ClientID %>").val() + "-" + $("#<%= HF_num_adult.ClientID %>").val() + "-" + $("#<%= HF_num_child.ClientID %>").val() + "-" + $("#<%= HF_num_infant.ClientID %>").val())
        }
        function RNT_alternativeEstate_fill(numPers, dtS, dtE, minPrice,numAdult, numChildOver, numChildInfant) {
            setValueForRequest(dtS, dtE ,numAdult, numChildOver, numChildInfant);
            $("#alternativeEstate").show();
            RNT_alternativeEstate_showLoading();
            var _url = "/webservice/rnt_estate_list_search.aspx";
            _url += "?action=first&mode=alternative&isNew=1";
            _url += "&lang=<%= CurrentLang.ID %>";
            _url += "&session=false";
            _url += "&dtS=" + dtS;
            _url += "&dtE=" + dtE;
            _url += "&numPers=" + numPers;
            _url += "&currEstate=" + $("#<%= HF_id.ClientID %>").val();
            _url += "&minPrice=" + minPrice;
            _url += "&currZone=" + "<%=currEstateTB.pid_zone%>";
            _url += "&currPage=1";
            _url += "&numPerPage=9";
            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function (html) {
                    $("#similar-properties").html(html);                                     
                    RNT_alternativeEstate_hideLoading();
                    var cnt = $("#alternative_cnt").val();  
                    console.log("cnt"+cnt);

                    if(cnt > 6)
                    {
                        $("#alternative_load_more").show();
                    }
                    else
                    {
                        $("#alternative_load_more").hide();
                    }
                }
            });
        }
        function RNT_alternativeEstate_showLoading() {
            $("#similar-properties").hide();
            $("#alternativeEstate_loading").show();
        }
        function RNT_alternativeEstate_hideLoading() {
            $("#alternativeEstate_loading").hide();
            $("#similar-properties").show();
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" />
    <asp:HiddenField ID="HF_id" runat="server" Value="0" />
    <asp:HiddenField ID="HF_agentId" runat="server" Value="0" />
    <asp:Literal ID="ltr_gmapPointsScript" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_hidePriceColScript" runat="server" Visible="false"></asp:Literal>
    <div class="content propertyDetailsCont">
        <div class="container">

            <div class="row">

                <!-- BEGIN TOP INFO -->
                <div class="property-topinfo">
                    <ul class="amenities">
                        <li><i data-original-title="Occupancy" data-toggle="tooltip" data-placement="bottom" title="" class="fa fa-users"></i><%= contUtils.getLabel("lblUpTo") + " "+  currEstateTB.num_persons_max.objToInt32()  +" "+ contUtils.getLabel("lblPersons") %> <i data-original-title="(<%= currEstateTB.num_persons_adult.objToInt32() %><%= currEstateTB.num_persons_optional.objToInt32() > 0 ? (" + " +  currEstateTB.num_persons_optional.objToInt32()) : ""%>)" data-toggle="tooltip" data-placement="bottom" title="" class="fa fa-question-circle"></i></li>
                        <li class="topAmenity"><i data-original-title="Bedrooms" data-toggle="tooltip" data-placement="bottom" title="" class="icon-bedrooms"></i><%= currEstateTB.num_rooms_bed.objToInt32() %></li>
                        <li class="topAmenity"><i data-original-title="Bathrooms" data-toggle="tooltip" data-placement="bottom" title="" class="icon-bathrooms"></i><%= currEstateTB.num_rooms_bath.objToInt32() %></li>
                    </ul>

                    <div id="property-id">ID: #<%= currEstateTB.id %></div>
                </div>
                <!-- END TOP INFO -->

                <div class="nulla">
                </div>

                <!-- BEGIN PHOTOGALLERY -->

                <div class="galleryDettCont">
                    <div class="galleryDett" id="detailSlides">
                        <asp:ListView ID="LV_images" runat="server" OnItemDataBound="LV_images_ItemDataBound">
                            <ItemTemplate>
                                <div id="div_image" runat="server">
                                    <a href="/<%#Eval("img_banner")%>" class="galleryImg galleryItemDet galleryItemDet_<%#Container.DisplayIndex + 1 %>" rel="prettyPhoto[gallery]">
                                        <img alt="" id="img_banner<%#Container.DisplayIndex + 1 %>" src="/<%#Eval("img_banner") %>" />
                                        <span class="zoomPhotoDet">+</span>
                                    </a>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                        <div id="div_default_image1" runat="server" visible="false">
                            <a href="javascript:void(0);" class="galleryImg galleryItemDet galleryItemDet_1">
                                <img src="/images/dettgallerydef1.jpg" alt="" style="margin-top: 0px;" />
                            </a>
                        </div>
                        <div id="div_default_image2" runat="server" visible="false">
                            <a href="javascript:void(0);" class="galleryImg galleryItemDet galleryItemDet_2">
                                <img src="/images/dettgallerydef1.jpg" alt="" style="margin-top: -92.5px;" />
                            </a>
                        </div>
                        <div id="div_default_image3" runat="server" visible="false">
                            <a href="javascript:void(0);" class="galleryImg galleryItemDet galleryItemDet_3">
                                <img src="/images/dettgallerydef3.jpg" alt="" style="margin-top: 0px;" />
                            </a>
                        </div>
                        <div id="div_default_image4" runat="server" visible="false">
                            <a href="javascript:void(0);" class="galleryImg galleryItemDet galleryItemDet_4">
                                <img src="/images/dettgallerydef4.jpg" alt="" style="margin-top: 0px;" />
                            </a>
                        </div>

                        <asp:ListView ID="LV_all_images" runat="server" OnItemDataBound="LV_all_images_ItemDataBound">
                            <ItemTemplate>
                                <div id="div_image" runat="server">
                                    <a class="morePhotosDet galleryImg" href="/<%#Eval("img_banner")%>" rel="prettyPhoto[gallery1]">
                                        <i class="fa fa-photo"></i>
                                        <span><%= contUtils.getLabel("lblMore") %></span>
                                    </a>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                        <a class="morePhotosDet videoDett" id="video-gallery">
                            <i class="fa fa-video-camera"></i>
                            <span><%= contUtils.getLabel("lblVideo") %></span>
                        </a>
                    </div>
                </div>
                <asp:ListView ID="LV_Videos" runat="server" OnItemDataBound="LV_Videos_ItemDataBound">
                    <ItemTemplate>
                        <div id="div_video" runat="server">
                            <div class="videopop">
                                <div class="lg-toolbar lg-group">
                                    <span id="closevidpop" class="lg-close lg-icon"></span>
                                </div>
                                <div class="videocon">
                                    <%# Eval("video_embed") %>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
                <!-- END PHOTOGALLERY -->

                <div class="nulla">
                </div>

                <!-- BEGIN BOOKING FORM -->


                <!-- END BOOKING FORM -->

                <!-- BEGIN MAIN CONTENT -->
                <script>
                    
                    $(document).on("click", ".fixbooknow, .closebook", function(){
                        $(".sidebar.bookingFormDetCont .bookingform").toggle();
                        $(document).on("click",".calendar-input", function(){
                            var psos = $(this).offset().top;
                            var psor = $(".bookingform").offset().top;
                            console.log( psos , psor, psos-psor);
                            $("#ui-datepicker-div.ui-datepicker").css('top', psos-psor+40);
                        });
                    });
                </script>
                <div class="fixbooknow">
                    <%= contUtils.getLabel("reqBookNow")%>
                </div>
                <div class="main col-sm-8" style="float: right;">
                    <uc1:breadCrumbs ID="breadCrumbs" runat="server" />
                    <h1 class="property-title"><%= currEstateLN.title %>  <small><%= CurrentSource.locZone_title(currEstateTB.pid_zone.objToInt32(),App.LangID,"")%></small></h1>

                    <%if (currEstateTB.is_best_price.objToInt32() == 1)
                      { %>
                    <img src="/images/css/best-price-guarantee.png" class="bestPrice bestPriceDetail" alt="Best Price Guarantee" />
                    <% } %>
                    <div class="nulla"></div>
                    <div class="spOffers" id="spOffersCont" runat="server">
                        <span class="spOfferTitle"><%=CurrentSource.getSysLangValue("lblSpecialOffersAndLastMinute")%></span>
                        <asp:ListView ID="LV_special_offer" runat="server">
                            <ItemTemplate>
                                <div class="spOffer">
                                    <div class="discountDetail"><%# Eval("pr_discount").objToDecimal().ToString("N0") %><span>%</span></div>
                                    <span class="offerPeriodDetail"><%= CurrentSource.getSysLangValue("lblDateFrom")%>:<%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%><br />
                                        <%= CurrentSource.getSysLangValue("lblDateTo")%>:<%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID,"") %></span>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>

                    </div>
                    <div class="nulla"></div>
                    <!-- BEGIN PROPERTY FEATURES LIST -->
                    <ul class="property-features col-sm-6">
                        <% if (currEstateTB.num_persons_optional.objToInt32() > 0)
                           { %>
                        <li><i class="icon-house"></i>
                            <%= CurrentSource.getSysLangValue("lblUpTo")%>
                            <%= currEstateTB.num_persons_max%>
                            <%= CurrentSource.getSysLangValue("lblPersons")%>
                    (<%= currEstateTB.num_persons_adult%>
                    +
                    <%= currEstateTB.num_persons_optional%>)
                        </li>
                        <%}
                           else
                           { %>
                        <li><i class="icon-house"></i>
                            <%= currEstateTB.num_persons_max%>
                            <%= CurrentSource.getSysLangValue("lblSleeps")%>
                        </li>
                        <%} %>

                        <li><i class="icon-bedrooms"></i><%=(currEstateTB.num_rooms_bed > 1) ? currEstateTB.num_rooms_bed + " " + CurrentSource.getSysLangValue("lblBedRooms") : currEstateTB.num_rooms_bed + " " + CurrentSource.getSysLangValue("lblBedRoom")%></li>
                        <li><i class="icon-bathrooms"></i><%=(currEstateTB.num_rooms_bath > 1) ? currEstateTB.num_rooms_bath + " " + CurrentSource.getSysLangValue("lblBathRooms") : currEstateTB.num_rooms_bath + " " + CurrentSource.getSysLangValue("lblBathRoom")%></li>
                        <li><i class="icon-key"></i><%= CurrentSource.getSysLangValue("lblStayMin")%>
                            <%=(currEstateTB.nights_min.objToInt32() > 1) ? currEstateTB.nights_min + " " + CurrentSource.getSysLangValue("lblNights") : currEstateTB.nights_min + " " + CurrentSource.getSysLangValue("lblNight")%></li>
                    </ul>


                    <ul class="property-features col-sm-6">

                        <% if (currEstateTB.num_bed_single.objToInt32() != 0)
                           { %>

                        <li class="tooltip_top" title="<img src='/images/css/letto-singolo.gif' />">
                            <i class="icon-bedrooms"></i><%=(currEstateTB.num_bed_single.objToInt32() > 1) ? currEstateTB.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsSingle") : currEstateTB.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedSingle")%>
                        </li>
                        <%}%>

                        <% if (currEstateTB.num_bed_double.objToInt32() != 0)
                           { %>
                        <li class="tooltip_top" title="<img src='/images/css/letto-matrimonale-standard.gif' />">
                            <i class="icon-bedrooms "></i><%=(currEstateTB.num_bed_double.objToInt32() > 1) ? currEstateTB.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDouble") : currEstateTB.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDouble")%>
                        </li>

                        <%}%>
                        <% if (currEstateTB.num_bed_double_divisible.objToInt32() != 0)
                           { %>
                        <li class="tooltip_top" title="<img src='/images/css/letto-matrimonale-divisibile.gif' />">
                            <i class="icon-bedrooms"></i><%=(currEstateTB.num_bed_double_divisible.objToInt32() > 1) ? currEstateTB.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDoubleDivisible") : currEstateTB.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDoubleDivisible")%>
                        </li>
                        <%}%>
                        <% if (currEstateTB.num_sofa_double.objToInt32() != 0)
                           { %>
                        <li class="tooltip_top" title="<img src='/images/css/divano-letto.gif' />">
                            <i class="icon-bedrooms "></i><%=((currEstateTB.num_sofa_double.objToInt32()) > 1) ? (currEstateTB.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsDouble") : (currEstateTB.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedDouble")%>
                        </li>
                        <%}%>

                        <% if (currEstateTB.num_sofa_single.objToInt32() != 0)
                           { %>
                        <li class="tooltip_top" title="<img src='/images/css/poltrona-letto.gif' />">
                            <i class="icon-bedrooms ico_tooltip" ttpc="ttp_bedSofaSingle"></i><%=((currEstateTB.num_sofa_single.objToInt32()) > 1) ? (currEstateTB.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsSingle") : (currEstateTB.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedSingle")%>
                        </li>
                        <%}%>
                        <% if (currEstateTB.num_bed_double_2level.objToInt32() != 0)
                           { %>
                        <li class="tooltip_top" title="<img src='/images/css/letto-castello.gif' />">
                            <i class="icon-bedrooms"></i><%=((currEstateTB.num_bed_double_2level.objToInt32()) > 1) ? (currEstateTB.num_bed_double_2level.objToInt32()) + " " + CurrentSource.getSysLangValue("lblBunkBeds") : (currEstateTB.num_bed_double_2level.objToInt32()) + " " + CurrentSource.getSysLangValue("lblBunkBed")%>
                        </li>
                        <%}%>
                    </ul>
                    <!-- END PROPERTY FEATURES LIST -->

                    <!-- BEGIN PROPERTY DETAIL SLIDERS WRAPPER -->

                    <div class="property-text hidetxt">
                        <%=currEstateLN.description %>
                    </div>

                    <a class="btn btn-default-color btn-read-more"><%= CurrentSource.getSysLangValue("lblReadMore")%> </a>
                    <a class="btn btn-default-color btn-read-less hidetxt"><%= CurrentSource.getSysLangValue("lblReadLess")%>  </a>

                    <div class="nulla">
                    </div>
                    <!-- BEGIN PROPERTY AMENITIES LIST -->

                    <asp:ListView ID="LV_config1" runat="server" ViewStateMode="Disabled">
                        <ItemTemplate>
                            <li class="enabled">
                                <img alt="" src="/<%# Eval("img_thumb") %>">
                                <%# CurrentSource.rnt_configTitle(Eval("id").objToInt32(),CurrentLang.ID,"") %> </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <ul class="property-amenities-list col-md-4">
                                <li id="itemPlaceholder" runat="server" alt="" />
                            </ul>
                        </LayoutTemplate>
                    </asp:ListView>
                    <asp:ListView ID="LV_config2" runat="server" ViewStateMode="Disabled">
                        <ItemTemplate>
                            <li class="enabled">
                                <img alt="" src="/<%# Eval("img_thumb") %>" title="<%# CurrentSource.rnt_configTitle(Eval("id").objToInt32(),CurrentLang.ID,"") %>">
                                <%# CurrentSource.rnt_configTitle(Eval("id").objToInt32(),CurrentLang.ID,"") %> </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <ul class="property-amenities-list col-md-4">
                                <li id="itemPlaceholder" runat="server" alt="" />
                            </ul>
                        </LayoutTemplate>
                    </asp:ListView>
                    <asp:ListView ID="LV_config3" runat="server" ViewStateMode="Disabled">
                        <ItemTemplate>
                            <li class="enabled">
                                <img alt="" src="/<%# Eval("img_thumb") %>">
                                <%# CurrentSource.rnt_configTitle(Eval("id").objToInt32(),CurrentLang.ID,"") %> </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <ul class="property-amenities-list col-md-4">
                                <li id="itemPlaceholder" runat="server" alt="" />
                            </ul>
                        </LayoutTemplate>
                    </asp:ListView>

                    <!-- END PROPERTY AMENITIES LIST -->


                    <h2 class="section-title"><%= CurrentSource.getSysLangValue("lblPropertyLocation")%>   </h2>
                    <!-- PROPERTY MAP HOLDER -->

                    <asp:PlaceHolder ID="PH_mapsContAll" runat="server">
                        <div id="maps" style="margin-bottom: 15px;">
                            <script type="text/javascript" src="https://www.google.com/jsapi"></script>
                            <div style="float: left;">
                                <ul class="menuMaps">
                                    <asp:PlaceHolder ID="PH_mapCont_toggler" runat="server">
                                        <li>
                                            <a id="mapCont_toggler" href="javascript:setMapMode('map');" class="selected">
                                                <span class="map">
                                                    <%= CurrentSource.getSysLangValue("lblMap")%></span>
                                            </a>
                                        </li>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PH_gmapSvcont_toggler" runat="server">
                                        <li>
                                            <a id="gmapSvcont_toggler" href="javascript:setMapMode('sv');">
                                                <span class="streetView"><%= CurrentSource.getSysLangValue("lblStreetView")%></span>
                                            </a>
                                        </li>
                                    </asp:PlaceHolder>
                                </ul>
                            </div>
                            <div class="nulla">
                            </div>
                            <div>
                                <asp:PlaceHolder ID="PH_gmapSvcont" runat="server">
                                    <div id="gmapSvcont" style="height: 450px; width: 100%; display: none;">
                                    </div>
                                    <script type="text/javascript">
                                        var panorama;

                                        var sv_service;
                                        var sv_coords;
                                        var sv_point;
                                        var panoramaSvOptions;
                                        var panoramaOptions;
                                        function setInitialPov() {
                                            panorama.setPosition(sv_point);
                                            panorama.setPov(panoramaSvOptions);
                                            panorama.setVisible(true);
                                        }
                                        function initializeStreetView() {
                                            sv_service = new google.maps.StreetViewService();
                                            sv_coords = "<%=currEstateTB.sv_coords %>";
                                            sv_point = new google.maps.LatLng(parseFloat(sv_coords.split('|')[0].replace(',', '.')), parseFloat(sv_coords.split('|')[1].replace(',', '.')));
                                            panoramaSvOptions = {    heading: <%= currEstateTB.sv_yaw.objToDecimal().ToString().Replace(",", ".") %>,    pitch: <%=currEstateTB.sv_pitch.objToDecimal().ToString().Replace(",", ".") %>,    zoom: <%=currEstateTB.sv_zoom.objToDecimal().ToString().Replace(",", ".") %>  };
                                            panoramaOptions = {  position: sv_point,  pov: panoramaSvOptions};
                                            panorama = new  google.maps.StreetViewPanorama(document.getElementById("gmapSvcont"), panoramaOptions);
                                            panorama.setVisible(true);
                                        }
                                    </script>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="PH_gmapSvcontNo" runat="server">
                                    <script type="text/javascript">
                                        function initializeStreetView() {
                                        }
                                    </script>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="PH_mapCont" runat="server">
                                    <div id="mapCont" style="height: 450px; width: 100%;">
                                    </div>
                                    <script type="text/javascript">
                                        var map = null;
                                        var marker = null;
                                        var directionsDisplay = null;
                                        var directionsService = null;
                                        var _point = null;
                                        var overlay;

                                        function setMap() {

                                            console.log("set map start");
                                            var _IconImage = new google.maps.MarkerImage("<%=HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) %><%=CurrentAppSettings.ROOT_PATH %>images/google_maps/google_icon_logo.png", new google.maps.Size(82, 45), new google.maps.Point(0, 0), new google.maps.Point(0, 45));
                                            var _IconShadow = new google.maps.MarkerImage("<%=HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) %><%=CurrentAppSettings.ROOT_PATH %>images/google_maps/google_icon_shadow.png", new google.maps.Size(82, 45), new google.maps.Point(0, 0), new google.maps.Point(0, 45));
                                            var _IconShape = {
                                                coord: [1, 1, 1, 45, 82, 45, 82, 1],
                                                type: 'poly'
                                            };
                                            var _coords = "<%=currEstateTB.google_maps %>";
                                            _point = new google.maps.LatLng(parseFloat(_coords.split('|')[0].replace(',', '.')), parseFloat(_coords.split('|')[1].replace(',', '.')));
                                            map = new google.maps.Map(document.getElementById("mapCont"), { zoom: 15, mapTypeId: google.maps.MapTypeId.ROADMAP, center: _point, streetViewControl: false, mapTypeControl: false });
                                            markerOptions = { position: _point, map: map, shadow: _IconShadow, icon: _IconImage, shape: _IconShape };
                                            marker = new google.maps.Marker(markerOptions);

                                            overlay = new google.maps.OverlayView();
                                            overlay.draw = function() {};
                                            overlay.setMap(map); // 'map' is new google.maps.Map(...)
                                            
                                            console.log("set map end");
				        <%= ltr_gmapPointsScript.Text %>
                                        }


                                     

                                        function hidePointToolTip(){
                                            $("#tooltip").css({'display': 'none'});
                                            $("#tooltip .body").html("");
                                        }
                                        function showPointToolTip(marker, title, img) {
                                            var projection = overlay.getProjection(); 
                                            var pixel = projection.fromLatLngToContainerPixel(marker.getPosition());
                                            var contPos = $("#mapCont").offset();
                                            var posX = contPos.left + pixel.x;
                                            var posY = contPos.top + pixel.y;
                                            //alert(posX+","+posY);
                                            var _body = "";
                                            _body += "<span class='title'>"+title+"</span>";
                                            if(img+""!="")
                                                _body += "<img class='point_preview' alt='"+title+"' title='"+title+"' src='/"+img+"' />";
                                            $("#tooltip .body").html(""+_body);
                                            $("#tooltip").css("left",""+(posX + 10)+"px");
                                            $("#tooltip").css("top",""+(posY + 10)+"px");
                                            $("#tooltip").css("display","");
                                        }
                                        function mapsLoaded(){
                                            setMap();
                                            google.maps.event.trigger(map, 'resize');
                                            initializeStreetView();
                                        }
                                        
                                        $(function () {
                                            // $.getScript("https://www.google.com/jsapi", function (data, textStatus, jqxhr) {
                                            //$(document).ready(function() {
                                            $(window).load(function () {
                                                console.log("start map");
                                                google.load("maps", "3.1", {"callback" : mapsLoaded, other_params: "sensor=false&key=<%=App.GOOGLE_MAPS_KEY %>&language=<%=CurrentLang.ABBR.Substring(0,2)%>"});                                               
                                                console.log("end map");
                                            });
                                        });
                                    </script>
                                </asp:PlaceHolder>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <script type="text/javascript">
                            function setMapMode(mode) {
                                if (mode == "map") {
                                    $("#gmapSvcont").hide();
                                    $("#gmapSvcont_toggler").attr("class", "");
                                    $("#mapCont").show();
                                    $("#mapCont_toggler").attr("class", "selected");
                                    google.load("maps", "3.1", {"callback" : mapsLoaded, other_params: "sensor=false&language=<%=CurrentLang.ABBR.Substring(0,2)%>"});                                                
                                    google.maps.event.trigger(map, 'resize'); map.setCenter(_point);
                                }
                                if (mode == "sv") {
                                    $("#gmapSvcont").show();
                                    $("#gmapSvcont_toggler").attr("class", "selected");
                                    $("#mapCont").hide();
                                    $("#mapCont_toggler").attr("class", "");
                                    panorama.setVisible(true);
                                }
                            }
                        </script>
                    </asp:PlaceHolder>

                    <div class="share-wraper col-sm-12">
                        <h5>Share this Property:</h5>
                        <ul class="social-networks">
                            <li><a href="http://www.facebook.com/sharer.php?s=100&amp;p%5Burl%5D=http%3A%2F%2Fwww.rentalinrome.com" target="_blank"><i class="fa fa-facebook"></i></a></li>
                            <li><a href="https://twitter.com/intent/tweet?url=http://www.rentalinrome.com" target="_blank"><i class="fa fa-twitter"></i></a></li>
                            <li><a href="https://plus.google.com/share?url=http://www.rentalinrome.com" target="_blank"><i class="fa fa-google"></i></a></li>
                            <li><a href="http://pinterest.com/pin/create/button/?url=http://www.rentalinrome.com" target="_blank"><i class="fa fa-pinterest"></i></a></li>
                            <li><a href="mailto:?subject=Check%20out%20this%20property%20I%20found!&amp;body=http://www.rentalinrome.com"><i class="fa fa-envelope"></i></a></li>
                        </ul>

                        <a href="javascript:window.print();" class="print-button">
                            <i class="fa fa-print"></i>
                        </a>
                    </div>
                    <asp:Literal ID="ltr_minPriceDay" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="ltr_minPriceWeek" runat="server" Visible="false"></asp:Literal>
                    <div id="priceInfo" class="col-md-12">
                        <h3 class="titBar">
                            <%= CurrentSource.getSysLangValue("lblPrices")%></h3>
                        <asp:PlaceHolder ID="PH_priceOnRequestCont" runat="server" Visible="false">
                            <%= CurrentSource.getSysLangValue("lblOnRequest")%>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PH_priceListCont" runat="server">
                            <asp:PlaceHolder ID="PH_priceListCont_v1" runat="server">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table-bordered priceTable" style="float: left;">
                                    <tr>
                                        <th style="border-left: none; background: none;">&nbsp;</th>
                                        <th colspan="2" runat="server" visible="false" id="th_datePeriod1"><strong>
                                            <%= CurrentSource.rntPeriod_title(1, CurrentLang.ID, "Low Season")%></strong><br />
                                        </th>
                                        <th colspan="2" runat="server" visible="false" id="th_datePeriod4"><strong>
                                            <%= CurrentSource.rntPeriod_title(4, CurrentLang.ID, "Medium Season")%></strong><br />
                                        </th>
                                        <th colspan="2" runat="server" visible="false" id="th_datePeriod2"><strong>
                                            <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "High Season")%></strong><br />
                                        </th>
                                        <th colspan="2" runat="server" visible="false" id="th_datePeriod3"><strong>
                                            <%= CurrentSource.rntPeriod_title(3, CurrentLang.ID, "Very High Season")%></strong><br />
                                        </th>
                                    </tr>
                                    <tr>
                                        <td class="prima" style="border-left: none;">&nbsp;
                                        </td>
                                        <td class="prima" id="td_day_1" runat="server" visible="false">
                                            <%= CurrentSource.getSysLangValue("lblDaily")%>:
                                        </td>
                                        <td class="prima" id="td_week_1" runat="server" visible="false">
                                            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                                        </td>
                                        <td class="prima" id="td_day_4" runat="server" visible="false">
                                            <%= CurrentSource.getSysLangValue("lblDaily")%>:
                                        </td>
                                        <td class="prima" id="td_week_4" runat="server" visible="false">
                                            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                                        </td>
                                        <td class="prima" id="td_day_2" runat="server" visible="false">
                                            <%= CurrentSource.getSysLangValue("lblDaily")%>:
                                        </td>
                                        <td class="prima" id="td_week_2" runat="server" visible="false">
                                            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                                        </td>
                                        <td class="prima" id="td_day_3" runat="server" visible="false">
                                            <%= CurrentSource.getSysLangValue("lblDaily")%>:
                                        </td>
                                        <td class="prima" id="td_week_3" runat="server" visible="false">
                                            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                                        </td>
                                    </tr>
                                    <%= ltr_priceDetails.Text%>
                                    <% if (currEstateTB.longTermRent == 1 && currEstateTB.longTermPrMonthly.objToDecimal() > 0)
                                       { 
                                    %>
                                    <tr>
                                        <td style="border-left: none; border-bottom: none;" colspan="7"></td>
                                    </tr>
                                    <tr>
                                        <th style="border-left: none; border-top: 2px solid #333366;" colspan="2"><strong>
                                            <%=CurrentSource.getSysLangValue("lblbusinesshousing")%>
                                        </strong></th>
                                        <th colspan="5" style="border-top: 2px solid #333366; background: none; text-align: left;"><strong>
                                            <%=CurrentSource.getSysLangValue("lblFrom") + "&nbsp;&euro;&nbsp;" + currEstateTB.longTermPrMonthly.objToDecimal().ToString("N2") + "&nbsp;" + CurrentSource.getSysLangValue("lblMonthly")%>
                                        </strong></th>
                                    </tr>
                                    <%
                                       } %>
                                </table>
                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table-bordered priceTable" style="float: left;">
                                        <tr>
                                            <th colspan="4" style="border-left: none;">Seasons reference </th>
                                        </tr>
                                        <tr>
                                            <td class="prima" style="border-left: none;">
                                                <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                            </td>
                                            <td class="prima">
                                                <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                            </td>
                                            <td class="prima">Season
                                            </td>
                                        </tr>
                                        <asp:ListView ID="LVseasonDates" runat="server" ViewStateMode="Disabled">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                                <tr>
                                                    <td style="border-left: none;">
                                                        <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                    </td>
                                                    <td>
                                                        <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                    </td>
                                                    <td>
                                                        <%# CurrentSource.rntPeriod_title(Eval("pidPeriod").objToInt32(), CurrentLang.ID, "--Season--")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <tr id="itemPlaceholder" runat="server" />
                                            </LayoutTemplate>
                                        </asp:ListView>
                                    </table>
                                </asp:PlaceHolder>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table-bordered priceTable" style="float: left;">
                                    <tr>
                                        <th colspan="<%= SeasonDateHeaderColSpan%>" style="border-left: none;"><%= contUtils.getLabel("mobileSeasonReference") %></th>
                                    </tr>
                                    <tr>
                                        <th runat="server" visible="false" id="th_PricePeriod1"><strong>
                                            <%= CurrentSource.rntPeriod_title(1, CurrentLang.ID, "Low Season")%></strong><br />
                                        </th>
                                        <th runat="server" visible="false" id="th_PricePeriod4"><strong>
                                            <%= CurrentSource.rntPeriod_title(4, CurrentLang.ID, "Medium Season")%></strong><br />
                                        </th>
                                        <th runat="server" visible="false" id="th_PricePeriod2"><strong>
                                            <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "High Season")%></strong><br />
                                        </th>
                                        <th runat="server" visible="false" id="th_PricePeriod3"><strong>
                                            <%= CurrentSource.rntPeriod_title(3, CurrentLang.ID, "Very High Season")%></strong><br />
                                        </th>
                                    </tr>
                                    <tr>
                                        <td style="padding: 0pt; vertical-align: top;" runat="server" visible="false" id="td_colPeriod1">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td class="prima" style="border-left: none;">
                                                        <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                                    </td>
                                                    <td class="prima">
                                                        <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                                    </td>
                                                </tr>
                                                <asp:ListView ID="LVseasonDates_1" runat="server" ViewStateMode="Disabled">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                                        <tr>
                                                            <td style="border-left: none;">
                                                                <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                            </td>
                                                            <td>
                                                                <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <tr id="itemPlaceholder" runat="server" />
                                                    </LayoutTemplate>
                                                </asp:ListView>
                                            </table>
                                        </td>
                                        <td style="padding: 0pt; vertical-align: top;" runat="server" visible="false" id="td_colPeriod4">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td class="prima" style="border-left: none;">
                                                        <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                                    </td>
                                                    <td class="prima">
                                                        <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                                    </td>
                                                </tr>
                                                <asp:ListView ID="LVseasonDates_4" runat="server" ViewStateMode="Disabled">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                                        <tr>
                                                            <td style="border-left: none;">
                                                                <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                            </td>
                                                            <td>
                                                                <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <tr id="itemPlaceholder" runat="server" />
                                                    </LayoutTemplate>
                                                </asp:ListView>
                                            </table>
                                        </td>
                                        <td style="padding: 0pt; vertical-align: top;" runat="server" visible="false" id="td_colPeriod2">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td class="prima" style="border-left: none;">
                                                        <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                                    </td>
                                                    <td class="prima">
                                                        <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                                    </td>
                                                </tr>
                                                <asp:ListView ID="LVseasonDates_2" runat="server" ViewStateMode="Disabled">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                                        <tr>
                                                            <td style="border-left: none;">
                                                                <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                            </td>
                                                            <td>
                                                                <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <tr id="itemPlaceholder" runat="server" />
                                                    </LayoutTemplate>
                                                </asp:ListView>
                                            </table>
                                        </td>
                                        <td style="padding: 0pt; vertical-align: top;" runat="server" visible="false" id="td_colPeriod3">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td class="prima" style="border-left: none;">
                                                        <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                                    </td>
                                                    <td class="prima">
                                                        <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                                    </td>
                                                </tr>
                                                <asp:ListView ID="LVseasonDates_3" runat="server" ViewStateMode="Disabled">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                                        <tr>
                                                            <td style="border-left: none;">
                                                                <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                            </td>
                                                            <td>
                                                                <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <tr id="itemPlaceholder" runat="server" />
                                                    </LayoutTemplate>
                                                </asp:ListView>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="PH_priceListCont_v2" runat="server" Visible="false">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable" style="float: left;">
                                    <tr>
                                        <th style="border-left: none; background: none; width: 220px;">&nbsp;</th>
                                        <th></th>
                                        <th>
                                            <%= CurrentSource.getSysLangValue("lblDaily")%>
                                        </th>
                                        <%= currEstateTB.pr_dcS2_1_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_1_percent.objToInt32() > 0 ? "<th>Over " + currEstateTB.pr_dcS2_1_inDays.objToInt32() + " nights <br/><strong>-" + currEstateTB.pr_dcS2_1_percent.objToInt32() + "&nbsp;%</strong>&nbsp;" + CurrentSource.getSysLangValue("lblDaily") + "</th>" : ""%>
                                        <%= currEstateTB.pr_dcS2_2_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_2_percent.objToInt32() > 0 ? "<th>Over " + currEstateTB.pr_dcS2_2_inDays.objToInt32() + " nights <br/><strong>-" + currEstateTB.pr_dcS2_2_percent.objToInt32() + "&nbsp;%</strong>&nbsp;" + CurrentSource.getSysLangValue("lblDaily") + "</th>" : ""%>
                                        <%= currEstateTB.pr_dcS2_3_inDays.objToInt32() > 0 && currEstateTB.pr_dcS2_3_percent.objToInt32() > 0 ? "<th>Over " + currEstateTB.pr_dcS2_3_inDays.objToInt32() + " nights <br/><strong>-" + currEstateTB.pr_dcS2_3_percent.objToInt32() + "&nbsp;%</strong>&nbsp;" + CurrentSource.getSysLangValue("lblDaily") + "</th>" : ""%>
                                    </tr>
                                    <asp:ListView ID="LVdatesAll" runat="server" ViewStateMode="Disabled">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pid_period") %>' Visible="false"></asp:Label>
                                            <tr>
                                                <td style="border-left: none; text-align: left;">
                                                    <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>&nbsp;-&nbsp;<%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                </td>
                                                <td>
                                                    <%# CurrentSource.rntPeriod_title(Eval("pid_period").objToInt32(), CurrentLang.ID, "--Season--")%>
                                                </td>
                                                <td>
                                                    <asp:Literal ID="ltr_price" runat="server"></asp:Literal>
                                                </td>
                                                <asp:Literal ID="ltr_pr_dcS2_1" runat="server"></asp:Literal>
                                                <asp:Literal ID="ltr_pr_dcS2_2" runat="server"></asp:Literal>
                                                <asp:Literal ID="ltr_pr_dcS2_3" runat="server"></asp:Literal>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </table>
                            </asp:PlaceHolder>
                        </asp:PlaceHolder>
                        <asp:Literal ID="ltr_priceDetails" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="ltr_priceTemplate" runat="server" Visible="false">
                <tr>
                    <td style="border-left: none;">#num_pers#
                    </td>
                    <td class="td_pr_1">#pr_1#
                    </td>
                    <td class="td_pr_1_w" >#pr_1_w#
                    </td>
                    <td class="td_pr_4" >#pr_4#
                    </td>
                    <td class="td_pr_4_w" >#pr_4_w#
                    </td>
                    <td class="td_pr_2" >#pr_2#
                    </td>
                    <td class="td_pr_2_w" >#pr_2_w#
                    </td>
                    <td class="td_pr_3" >#pr_3#
                    </td>
                    <td class="td_pr_3_w">#pr_3_w#
                    </td>
                </tr>
                        </asp:Literal>

                    </div>
                    <!-- BEGIN SIMILAR PROPERTIES -->

                    <h1 class="section-title"><%=CurrentSource.getSysLangValue("lblAlternateProperties")%> </h1>

                    <div id="alternativeEstate_loading" class="loadingSrc">
                        <span>
                            <%=CurrentSource.getSysLangValue("lblLoadingData")%>
                        </span>
                    </div>
                    <div class="grid-style1 clearfix" id="similar-properties">
                    </div>

                    <p class="center" id="alternative_load_more">
                        <a data-load-amount="3" data-grid-id="similar-properties" class="btn btn-default-color" href="#"><%=CurrentSource.getSysLangValue("lblLoadMore")%> </a>
                    </p>
                    <!-- END PROPERTIES ASSIGNED -->


                    <!-- BEGIN AGENT INFORMATION -->
                    <asp:Literal ID="ltr_userImage" runat="server" Visible="false"></asp:Literal>

                    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_num_adult" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_num_child" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_num_infant" runat="server" Value="0" />

                    <asp:PlaceHolder ID="PH_agency" runat="server" Visible="false">
                        <h1 class="section-title">Contact our agent </h1>
                        <div class="property-agent-info">
                            <div class="agent-detail col-md-4">
                                <div class="image">
                                    <img alt="" src="<%=ltr_userImage.Text %>" />
                                </div>

                                <div class="info">
                                    <header>
                                        <h2>
                                            <asp:Literal ID="ltr_userName" runat="server"></asp:Literal>
                                            <small>www.rentalinrome.com</small></h2>
                                    </header>

                                    <ul class="contact-us">
                                        <li><i class="fa fa-envelope"></i><a href="mailto:<%=ltr_email.Text %>">
                                            <asp:Literal ID="ltr_email" runat="server"></asp:Literal>
                                        </a></li>
                                        <li><i class="fa fa-map-marker"></i>
                                            <asp:Literal ID="ltr_address" runat="server"></asp:Literal></li>
                                        <li><i class="fa fa-phone"></i>+
                                        <asp:Literal ID="ltr_Phone" runat="server"></asp:Literal></li>
                                    </ul>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="pnlContact" runat="server" class="col-md-8">
                                <ContentTemplate>
                                    <div class="form-style" runat="server" id="pnl_request_sent" visible="false">
                                        <div class="well alert-success">
                                            <%=CurrentSource.getSysLangValue("reqRequestSent")%>
                                        </div>
                                    </div>
                                    <div class="form-style" runat="server" id="pnl_request_cont">
                                        <div class="col-sm-12">
                                            <asp:TextBox ID="txt_FirstName" runat="server" CssClass="form-control required"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-12">
                                            <asp:TextBox ID="txt_email" runat="server" CssClass="form-control required"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-12">
                                            <asp:TextBox ID="txt_subject" runat="server" CssClass="form-control required subject"></asp:TextBox>
                                            <asp:TextBox ID="txt_notes" runat="server" CssClass="form-control required" TextMode="MultiLine"></asp:TextBox>
                                        </div>

                                        <div class="center">
                                            <asp:LinkButton ID="lnk_send" runat="server" OnClick="lnk_send_Click" CssClass="btn btn-default-color" OnClientClick="return validateAgentRequest();"> 
                                                <i class="fa fa-envelope"></i> <%= contUtils.getLabel("lblSendYourFeedback") %>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <!-- END AGENT INFORMATION -->

                    </asp:PlaceHolder>
                    <!-- END AGENT INFORMATION -->

                    <!-- BEGIN REVIEWS -->
                    <h1 class="section-title" style="display:none;"><%= CurrentSource.getSysLangValue("lblGuestbook")%> </h1>
                    <input type="hidden" id="hf_estComment_currPage" value="1" />
                    <div class="comments" style="display:none;">
                        <ul id="commentListCont">
                            <div class="loading_img">
                            </div>
                        </ul>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" class="row">
                            <ContentTemplate>
                                <div class="col-md-12 well alert-success" id="pnl_Comment_sent" runat="server" visible="false">
                                    <%= CurrentSource.getSysLangValue("reqCommentAdded")%>
                                </div>
                                <div class="comments-form" id="pnl_Comment_cont" runat="server">

                                    <div class="col-sm-12">
                                        <%= contUtils.getLabel("lblLeaveCommentForThisApartment") %>
                                    </div>

                                    <div class="form-style">
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txt_user_full_name" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txt_user_email" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-6">
                                            <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                                <WhereParameters>
                                                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                                </WhereParameters>
                                            </asp:LinqDataSource>
                                            <asp:DropDownList ID="drp_country" runat="server" CssClass="form-control" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="id"></asp:DropDownList>
                                        </div>

                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="drp_UsertType" runat="server">
                                                <asp:ListItem Text="I am..." Value=""></asp:ListItem>
                                                <asp:ListItem Text="..a man" Value="m"></asp:ListItem>
                                                <asp:ListItem Text="..a woman" Value="f"></asp:ListItem>
                                                <asp:ListItem Text="..a couple" Value="co"></asp:ListItem>
                                                <asp:ListItem Text="..a family" Value="fam"></asp:ListItem>
                                                <asp:ListItem Text="..a group of persons" Value="gr"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="drp_voteStaff" runat="server">
                                                <asp:ListItem Text="Staff Rating" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="drp_voteService" runat="server">
                                                <asp:ListItem Text="Service Rating" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="drp_voteCleaning" runat="server">
                                                <asp:ListItem Text="Cleaning Rating" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="drp_voteQuality" runat="server">
                                                <asp:ListItem Text="Quality/Price Rating" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="drp_votePosition" runat="server">
                                                <asp:ListItem Text="Position Rating" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="drp_voteComfort" runat="server">
                                                <asp:ListItem Text="Comfort Rating" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                            </asp:DropDownList>

                                        </div>

                                        <div class="col-sm-12">
                                            <asp:TextBox ID="txt_user_comment" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        </div>

                                        <div class="center">
                                            <asp:LinkButton ID="lnk_send_Comment" runat="server" CssClass="btn btn-default-color btn-lg" OnClick="lnk_send_Comment_Click" OnClientClick="return commentSend_validateForm();">
                                            <%= contUtils.getLabel("lblSendFeedBack") %>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- END REVIEWS-->

                </div>
                <!-- END MAIN CONTENT -->
                <div class="sidebar col-sm-4 bookingFormDetCont" style="float: left;">
                    <uc2:ucBooking ID="ucBooking" runat="server" />

                    <div class="nulla">
                    </div>

                    <div class="gray bookingFormDet zoneDescDett">
                        <h3 class="section-title"><%= contUtils.getLabel("lblZoneInfo") %>
                            <br />
                            <strong><%=currZone.title %></strong>
                        </h3>
                        <div class="nulla">
                        </div>
                        <div class="imgZoneCont">
                            <img alt="zone" src="/<%=currZone.img_banner %>" />
                        </div>
                        <div class="nulla">
                        </div>
                        <p>
                            <%=currZone.description %>
                        </p>
                        <div class="nulla">
                        </div>
                    </div>

                </div>


            </div>
        </div>
    </div>

    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {

            var imageHeight, wrapperHeight, overlap, container;// = $('.galleryItemDet');

            function centerImage() {
                $('.galleryItemDet').each(function () {
                    container = $(this);
                    imageHeight = container.find('img').height();
                    wrapperHeight = container.height();
                    overlap = (wrapperHeight - imageHeight) / 2;
                    container.find('img').css('margin-top', overlap);                   
                });
            }

            $(window).on("load resize", centerImage);

            var el = document.getElementById('wrapper');
            if (el != null && el.addEventListener) {
                el.addEventListener("webkitTransitionEnd", centerImage, false); // Webkit event
                el.addEventListener("transitionend", centerImage, false); // FF event
                el.addEventListener("oTransitionEnd", centerImage, false); // Opera event
            }

        });

        function ChoosenSelect() {
            $(".chzn-select").chosen();
        }
    </script>
    <script type="text/javascript">
        var jq = $.noConflict();
        /* ARROW ANIMATION CLICK TO SCROLL TO LIST  */

        jq(".animatedArrow").click(function () {
            jq('html, body').animate({
                scrollTop: jq(".maincontent").offset().top - 150
            }, 1000);
        });
        jq(document).on('click', '#video-gallery', function(event) {
            $(".videopop").show();
        });
        jq(document).on('click', '#closevidpop', function(event) {
            $(".videopop").fadeOut();
        });
        
        jq('#detailSlides').lightGallery({
            selector: '.galleryImg',
            mode: 'lg-slide',
            download: false,
            pager: true,
            currentPagerPosition: "right"
        });
        function showlogin(){
            jq("#loginBox").fadeIn(300);
        }
    </script>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var commentSend_validateForm_firstTime = true;
            function commentSend_validateForm() {
                var callBack = null;
                if (commentSend_validateForm_firstTime) {
                    callBack = function () { return commentSend_validateForm(); };
                    commentSend_validateForm_firstTime = false;
                }
                var isValid = true;
                FORM_hideErrorToolTip();
                if (!FORM_validate_requiredField("<%= txt_user_comment.ClientID%>", "", "", "", callBack))
                    isValid = false;
                else if ($("#<%= txt_user_comment.ClientID%>").val().toLowerCase().indexOf("<a href") >= 0) {
                    FORM_showErrorToolTip("Links are not allowed here.", "<%=txt_user_comment.ClientID %>");
                    isValid = false;
                }
            if (!FORM_validate_requiredField("<%= txt_user_email.ClientID%>", "", "", "", callBack))
                    isValid = false;
                else if (!FORM_validate_emailField("<%= txt_user_email.ClientID%>", "", "", "", callBack))
                isValid = false;
            if (!FORM_validate_requiredField("<%= txt_user_full_name.ClientID%>", "", "", "", callBack))
                    isValid = false;

                return isValid;
            }

            var validateAgentRequest_first = true;
            function validateAgentRequest() {
                var callBack = null;
                if (validateAgentRequest_first) {
                    callBack = function () { return validateAgentRequest(); };
                    validateAgentRequest_first = false;
                }

                var isValid = true;
                FORM_hideErrorToolTip(); 

                if (!FORM_validate_requiredField("<%= txt_notes.ClientID%>", "", "", "", callBack))
                    isValid = false;

                if (!FORM_validate_requiredField("<%= txt_subject.ClientID%>", "", "", "", callBack))
                    isValid = false;

                if (!FORM_validate_requiredField("<%= txt_email.ClientID%>", "", "", "", callBack))
                    isValid = false;
                else if (!FORM_validate_emailField("<%= txt_email.ClientID%>", "", "", "", callBack))
                    isValid = false;

                if (!FORM_validate_requiredField("<%= txt_FirstName.ClientID%>", "", "", "", callBack))
                    isValid = false;

                return isValid;
            }


            <%= ltr_hidePriceColScript.Text %> 
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
