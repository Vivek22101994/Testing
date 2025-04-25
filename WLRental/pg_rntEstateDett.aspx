<%@ Page Title="" Language="C#" MasterPageFile="~/WLRental/common/MP_WLRental.Master" AutoEventWireup="true" CodeBehind="pg_rntEstateDett.aspx.cs" Inherits="RentalInRome.WLRental.pg_rntEstateDett" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_menu_breadcrumb.ascx" TagName="UC_menu_breadcrumb" TagPrefix="uc1" %>
<%@ Register Src="uc/ucBooking.ascx" TagName="ucBooking" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%= currEstateLN.meta_title%></title>
    <meta name="description" content="<%=currEstateLN.meta_description %>" />
    <meta name="keywords" content="<%=currEstateLN.meta_keywords %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script type="text/javascript">
        function RNT_estComment_changePage(page) {
            $("#hf_estComment_currPage").val("" + page);
            RNT_estComment_fillList("list");
        }
        function RNT_estComment_fillList(action) {
            if (action == "list") SITE_showLoader();
            var _url = "/webservice/rnt_estate_comment.aspx";
            _url += "?action=" + action;
            _url += "&lang=<%= CurrentLang.ID %>";
            _url += "&currEstate=" + $("#<%= HF_id.ClientID%>").val();
            _url += "&voteRange=";
            _url += "&currPage=1";
            _url += "&numPerPage=99";
            _url += "&fullView=0";
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
            setPriceColumns();
        });
    </script>

    <script type="text/javascript">
        function RNT_alternativeEstate_fill(numPers, dtS, dtE, minPrice) {
            $("#alternativeEstate").show();
            RNT_alternativeEstate_showLoading();
            var _url = "/webservice/rnt_estate_list_search.aspx";
            _url += "?action=first&mode=alternative";
            _url += "&lang=<%= CurrentLang.ID %>";
            _url += "&session=false";
            _url += "&dtS=" + dtS;
            _url += "&dtE=" + dtE;
            _url += "&numPers=" + numPers;
            _url += "&currEstate=" + $("#<%= HF_id.ClientID %>").val();
            _url += "&minPrice=" + minPrice;
            _url += "&currZone=" + $("#<%= HF_pid_zone.ClientID %>").val();
            _url += "&currPage=1";
            _url += "&numPerPage=3";
            _url += "&isWL=1";
            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function (html) {
                    $("#alternativeEstate_cont").html(html);
                    RNT_alternativeEstate_hideLoading();
                }
            });
        }
        function RNT_alternativeEstate_showLoading() {
            $("#alternativeEstate_cont").hide();
            $("#alternativeEstate_loading").show();
        }
        function RNT_alternativeEstate_hideLoading() {
            $("#alternativeEstate_loading").hide();
            $("#alternativeEstate_cont").show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:Literal ID="ltr_minPriceDay" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_minPriceWeek" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_otherZones" runat="server" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_unique" runat="server" Value="" />
    <asp:HiddenField ID="HF_id" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pid_zone" runat="server" Value="0" />
     <asp:HiddenField ID="HF_banner_image" runat="server" Value="0" />
    <asp:Literal ID="ltr_gmapPointsScript" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_hidePriceColScript" runat="server" Visible="false"></asp:Literal>
    <div id="breadcrumbscont">
        <asp:PlaceHolder ID="PH_back_to_list" runat="server" Visible="false">
            <a href="<%=CurrentSource.getPagePath("6", "stp", CurrentLang.ID.ToString()) %>" class="backToList">
                <span style="font-size: 10px;">&lt;</span>
                <%= CurrentSource.getSysLangValue("lblBackToList")%></a>
            <span style="display: block; float: left; margin-right: 10px;">| </span>
        </asp:PlaceHolder>
        <uc1:UC_menu_breadcrumb ID="UC_menu_breadcrumb1" runat="server" />
    </div>
    <div id="altreZone">
        <div class="tabTitle">
            <a id="rnt_otherZone_toggler" class="btnaltreZone" href="javascript:rnt_toggle_otherZone()">
                <%= CurrentSource.getSysLangValue("lblOtherArea")%>
            </a>
        </div>
        <div class="nulla">
        </div>
        <div style="position: absolute;">
            <div id="rnt_otherZone_cont" class="slide_down_cont" style="display: none;">
                <%= ltr_otherZones.Text%>
            </div>
        </div>
    </div>
    <input type="hidden" id="rnt_otherZone_state" />
    <script type="text/javascript">
        function rnt_toggle_otherZone() {
            if ($("#rnt_otherZone_state").val() == "1") {
                $("#rnt_otherZone_toggler").removeClass("opened");
                $("#rnt_otherZone_toggler").addClass("closed");
                $("#rnt_otherZone_cont").slideUp();
                $("#rnt_otherZone_state").val("0");
            }
            else {
                $("#rnt_otherZone_toggler").removeClass("closed");
                $("#rnt_otherZone_toggler").addClass("opened");
                $("#rnt_otherZone_cont").slideDown();
                $("#rnt_otherZone_state").val("1");
            }
        }
        $("#rnt_otherZone_state").val("0");
    </script>
    <div class="nulla">
    </div>
    <style type="text/css">
        @import url(/jquery/plugin/ad-gallery/ad-gallery.css);
    </style>
    <input type="hidden" id="videoApt_current" />
    <script type="text/javascript">
        var gallery;
        var floorplans;
        $(function () {
            $(window).load(function () {
                $.getScript("/jquery/plugin/ad-gallery/ad-gallery.js", function (data, textStatus, jqxhr) {
                    gallery = $('#gallery').adGallery({ width: 840, height: 480, effect: 'fade' });
                    floorplans = $('#floorplans').adGallery({ width: 840, height: 480, effect: 'fade' });
                });
            });
        });
        function show_galleryContainer() {
            $("#galleryContainer").show()
            $("#floorplansContainer").hide()
        }
        function hide_galleryContainer() {
            $("#floorplansContainer").hide()
            $("#galleryContainer").hide()
        }
        function show_floorplansContainer() {
            $("#galleryContainer").hide()
            $("#floorplansContainer").show()
        }
        function hide_floorplansContainer() {
            $("#floorplansContainer").hide()
            $("#galleryContainer").hide()
        }
        function toggle_videoApt(curr) {
            if ($("#videoApt_current").val() == "" + curr) {
                $("#videoApt_" + curr + "_toggler").removeClass("btnMediaSel");
                $("#videoApt_" + curr + "_toggler span").removeClass("videoclose");
                $("#bannerApt").show();
                $("#videoApt_" + curr).hide();
                $("#videoApt_current").val("0");
            }
            else {
                var _old = $("#videoApt_current").val();
                $("#videoApt_" + _old + "_toggler").removeClass("btnMediaSel");
                $("#videoApt_" + _old + "_toggler span").removeClass("videoclose");
                $("#videoApt_" + _old).hide();
                $("#bannerApt").hide();
                $("#videoApt_" + curr + "_toggler").addClass("btnMediaSel");
                $("#videoApt_" + curr + "_toggler span").addClass("videoclose");
                $("#videoApt_" + curr).show();
                $("#videoApt_current").val("" + curr);
            }
        }
        $("#videoApt_current").val("0"); 
    </script>
    <div id="schedaDett">
        <div id="galleryContainer">
            <a href="#" onclick="hide_galleryContainer(); return false;" class="closeGallery">
                <span></span>
                <%= CurrentSource.getSysLangValue("lblCalClose")%>
            </a>
            <div id="gallery" class="ad-gallery" style="margin: 10px;">
                <div class="ad-image-wrapper">
                </div>
                <div class="ad-controls" style="display: none;">
                </div>
                <div class="ad-nav" style="margin: 0 15px; width: 800px;">
                    <div class="ad-thumbs" style="width: 800px">
                        <ul class="ad-thumb-list">
                            <asp:ListView ID="LV_gallery" runat="server" ViewStateMode="Disabled">
                                <ItemTemplate>
                                    <li>
                                        <a href="<%# IMG_ROOT+Eval("img_banner")+"?tmp="+DateTime.Now.Ticks %>" title="<%# Eval("code") %>">
                                            <img src="/images/css/blankimg.png" realsrc="<%# IMG_ROOT+Eval("img_thumb")+"?tmp="+DateTime.Now.Ticks %>" style="height: 50px;" alt="">
                                        </a>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</li>
                                </ItemTemplate>
                                <LayoutTemplate>
                                    <li id="itemPlaceholder" runat="server" />
                                </LayoutTemplate>
                            </asp:ListView>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="floorplansContainer">
            <a href="#" onclick="hide_floorplansContainer(); return false;" class="closeGallery">
                <span></span>
                <%= CurrentSource.getSysLangValue("lblCalClose")%>
            </a>
            <div id="floorplans" class="ad-gallery" style="margin: 10px;">
                <div class="ad-image-wrapper">
                </div>
                <div class="ad-controls" style="display: none;">
                </div>
                <div class="ad-nav" style="margin: 0 15px; width: 800px;">
                    <div class="ad-thumbs" style="width: 800px">
                        <ul class="ad-thumb-list">
                            <asp:ListView ID="LV_floorplans" runat="server" ViewStateMode="Disabled">
                                <ItemTemplate>
                                    <li>
                                        <a href="<%# IMG_ROOT+Eval("img_banner")+"?tmp="+DateTime.Now.Ticks %>" title="<%# Eval("code") %>">
                                            <img src="/images/css/blankimg.png" realsrc="<%# IMG_ROOT+Eval("img_thumb")+"?tmp="+DateTime.Now.Ticks %>" style="height: 50px;" alt="">
                                        </a>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</li>
                                </ItemTemplate>
                                <LayoutTemplate>
                                    <li id="itemPlaceholder" runat="server" />
                                </LayoutTemplate>
                            </asp:ListView>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="aptDetails">
            <div class="titApt">
                <div>
                    <%if (currEstateTB.is_online_booking == 1)
                      {  %>
                    <img class="ico_onlinebooking" src="/WLRental/images/book_now_fasciatop.png" alt="" />
                    <%} %>
                    <h1>
                        <%=currEstateLN.title%>
                        <span class="zonaApt">
                            <%=CurrentSource.locZone_title(HF_pid_zone.Value.objToInt32(), CurrentLang.ID, "")%></span>
                    </h1>
                </div>
            </div>
            <!-- AddThis Button BEGIN -->
            <div class="addthis_toolbox addthis_default_style ">
                <a class="addthis_button_google_plusone"></a>
                <a class="addthis_counter addthis_pill_style"></a>
            </div>
            <script type="text/javascript">
                $(function () {
                    $(window).load(function () {
                        $.getScript("http://s7.addthis.com/js/250/addthis_widget.js#pubid=ra-4e33f01370a4c480", function (data, textStatus, jqxhr) {
                        });
                    });
                });
            </script>
            <!-- AddThis Button END -->
            <div id="votazione">
            </div>
            <div id="ttp_bedDouble" style="display: none;">
                <img src="/images/css/letto-matrimonale-standard.gif" alt="" />
            </div>
            <div id="ttp_bedDoubleD" style="display: none;">
                <img src="/images/css/letto-matrimonale-divisibile.gif" alt="" />
            </div>
            <div id="ttp_bedSingle" style="display: none;">
                <img src="/images/css/letto-singolo.gif" alt="" />
            </div>
            <div id="ttp_bedSofaDouble" style="display: none;">
                <img src="/images/css/divano-letto.gif" alt="" />
            </div>
            <div id="ttp_bedSofaSingle" style="display: none;">
                <img src="/images/css/poltrona-letto.gif" alt="" />
            </div>
            <div id="ttp_bedDouble2level" style="display: none;">
                <img src="/images/css/letto-castello.gif" alt="" />
            </div>
            <ul class="detailsList">
                <%=(("" + currEstateLN.sub_title).Trim() != "") ? "<li>" + currEstateLN.sub_title + "</li>" : ""%>
                <% if (currEstateTB.num_persons_optional.objToInt32() != 0)
                   { %>
                <li>
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
                <li>
                    <%= currEstateTB.num_persons_max%>
                    <%= CurrentSource.getSysLangValue("lblSleeps")%>
                </li>
                <%} %>
                <% if (currEstateTB.num_bed_single.objToInt32() != 0)
                   { %>
                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=(currEstateTB.num_bed_single.objToInt32() > 1) ? currEstateTB.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsSingle") : currEstateTB.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedSingle")%>
                    <a ttpc="ttp_bedSingle" class="infoBtn ico_tooltip" style="float: none; display: inline-block;"></a>
                </li>
                <%}%>
                <% if (currEstateTB.num_bed_double.objToInt32() != 0)
                   { %>
                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=(currEstateTB.num_bed_double.objToInt32() > 1) ? currEstateTB.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDouble") : currEstateTB.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDouble")%>
                    <a ttpc="ttp_bedDouble" class="infoBtn ico_tooltip" style="float: none; display: inline-block;"></a>
                </li>
                <%}%>
                <% if (currEstateTB.num_bed_double_divisible.objToInt32() != 0)
                   { %>
                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=(currEstateTB.num_bed_double_divisible.objToInt32() > 1) ? currEstateTB.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDoubleDivisible") : currEstateTB.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDoubleDivisible")%>
                    <a ttpc="ttp_bedDoubleD" class="infoBtn ico_tooltip" style="float: none; display: inline-block;"></a>
                </li>
                <%}%>
                <% if ((currEstateTB.num_sofa_double.objToInt32()) != 0)
                   { %>
                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=((currEstateTB.num_sofa_double.objToInt32()) > 1) ? (currEstateTB.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsDouble") : (currEstateTB.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedDouble")%>
                    <a ttpc="ttp_bedSofaDouble" class="infoBtn ico_tooltip" style="float: none; display: inline-block;"></a>
                </li>
                <%}%>
                <% if ((currEstateTB.num_sofa_single.objToInt32()) != 0)
                   { %>
                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=((currEstateTB.num_sofa_single.objToInt32()) > 1) ? (currEstateTB.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsSingle") : (currEstateTB.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedSingle")%>
                    <a ttpc="ttp_bedSofaSingle" class="infoBtn ico_tooltip" style="float: none; display: inline-block;"></a>
                </li>
                <%}%>
                <% if ((currEstateTB.num_bed_double_2level.objToInt32()) != 0)
                   { %>
                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=((currEstateTB.num_bed_double_2level.objToInt32()) > 1) ? (currEstateTB.num_bed_double_2level.objToInt32()) + " " + CurrentSource.getSysLangValue("lblBunkBeds") : (currEstateTB.num_bed_double_2level.objToInt32()) + " " + CurrentSource.getSysLangValue("lblBunkBed")%>
                    <a ttpc="ttp_bedDouble2level" class="infoBtn ico_tooltip" style="float: none; display: inline-block;"></a>
                </li>
                <%}%>
                <li>
                    <%=(currEstateTB.num_rooms_bed > 1) ? currEstateTB.num_rooms_bed + " " + CurrentSource.getSysLangValue("lblBedRooms") : currEstateTB.num_rooms_bed + " " + CurrentSource.getSysLangValue("lblBedRoom")%>
                </li>
                <li>
                    <%=(currEstateTB.num_rooms_bath > 1) ? currEstateTB.num_rooms_bath + " " + CurrentSource.getSysLangValue("lblBathRooms") : currEstateTB.num_rooms_bath + " " + CurrentSource.getSysLangValue("lblBathRoom")%>
                </li>
                <li>
                    <%= CurrentSource.getSysLangValue("lblStayMin")%>
                    <%=(currEstateTB.nights_min.objToInt32() > 1) ? currEstateTB.nights_min + " " + CurrentSource.getSysLangValue("lblNights") : currEstateTB.nights_min + " " + CurrentSource.getSysLangValue("lblNight")%>
                </li>
            </ul>
            <div class="nulla">
            </div>
            <asp:ListView ID="LV_config" runat="server" ViewStateMode="Disabled">
                <ItemTemplate>
                    <img src="/<%# Eval("img_thumb") %>" id="conf_<%# Eval("id") %>" title="<%# CurrentSource.rnt_configTitle(Eval("id").objToInt32(),CurrentLang.ID,"") %>" width="34" height="32" class="ico_tooltip_right" />
                </ItemTemplate>
                <EmptyDataTemplate>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <div id="serviceIcons">
                        <img id="itemPlaceholder" runat="server" alt="" />
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </LayoutTemplate>
            </asp:ListView>
            <% if (!clUtils.getConfig(CURRENT_SESSION_ID).myPreferedEstateList.Contains(IdEstate))
               {%>
            <a href="javascript:refreshMyList(<%= IdEstate%>);" class="btn addToMyList_toggler" title="<%=CurrentSource.getSysLangValue("lblAddToWishList")%>">
                <span>
                    <%=CurrentSource.getSysLangValue("lblAddToWishList")%>
                </span>
            </a>
            <%
               }%>
            <a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString())%>?IdEstate=<%=IdEstate%>" class="btn_concludi">
                <span>
                    <%= CurrentSource.getSysLangValue("lblEnquire")%></span>
            </a>
        </div>
        <div id="media">
            <div id="bannerApt">
                <div class="prezzi_apt_min" id="pnl_pricePreviewCont" runat="server">
                    <div class="uno">
                        <%= CurrentSource.getSysLangValue("lblPricesFrom")%>
                    </div>
                    <div class="due">
                        <ul style="list-style-type: none;">
                            <li>
                                <strong>
                                    <%= ltr_minPriceDay.Text %>&nbsp;&euro;</strong>
                                <%= CurrentSource.getSysLangValue("lblDaily")%></li>
                            <li>
                                <strong>
                                    <%= ltr_minPriceWeek.Text %>&nbsp;&euro;</strong>
                                <%= CurrentSource.getSysLangValue("lblWeekly")%></li>
                        </ul>
                    </div>
                </div>
                <img src="/WLRental/images/default-apt-banner.jpg" realsrc="<%=((bannerImage + "").Trim() != "") ? IMG_ROOT + bannerImage + "?tmp=" + DateTime.Now.Ticks : "/images/css/default-apt-banner.jpg"%>" alt="<%=CurrentSource.locZone_title(HF_pid_zone.Value.objToInt32(), CurrentLang.ID, "")%> - <%=currEstateLN.title%>" />
            </div>
            <asp:ListView ID="LV_videoCont" runat="server" ViewStateMode="Disabled">
                <ItemTemplate>
                    <div id="videoApt_<%# Container.DataItemIndex+1 %>" class="videoAptCont" style="display: none;">
                        <a class="closeGallery btnClose" href="#" onclick="toggle_videoApt('<%# Container.DataItemIndex+1 %>'); return false;">
                            <span></span>
                            <%= CurrentSource.getSysLangValue("lblCalClose")%></a>
                        <div class="videoApt">
                            <%# Eval("video_embed") %>
                        </div>
                    </div>
                </ItemTemplate>
                <LayoutTemplate>
                    <li id="itemPlaceholder" runat="server" />
                </LayoutTemplate>
            </asp:ListView>
            <div id="tastiMedia">
                <ul class="mediaul">
                    <% if (LV_gallery.Items.Count > 0)
                       { %>
                    <li>
                        <a href="#" onclick="show_galleryContainer(); return false;" class="btnfoto">
                            <%= CurrentSource.getSysLangValue("lblPhotogallery")%>
                        </a>
                    </li>
                    <%} %>
                    <asp:ListView ID="LV_videoToggler" runat="server" ViewStateMode="Disabled" Visible="false">
                        <ItemTemplate>
                            <li>
                                <a href="#" onclick="toggle_videoApt('<%# Container.DataItemIndex+1 %>'); return false;" class="videoToggler btnvideo" id="videoApt_<%# Container.DataItemIndex+1 %>_toggler">Video
                                    <%# Container.DataItemIndex+1 %>
                                </a>
                            </li>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <li id="itemPlaceholder" runat="server" />
                        </LayoutTemplate>
                    </asp:ListView>
                </ul>
            </div>
            <% if (LV_floorplans.Items.Count > 0)
               { %>
            <div id="tastiMedia2">
                <ul class="mediaul">
                    <li>
                        <a href="#" onclick="show_floorplansContainer(); return false;" class="btnfoto">
                            <%= CurrentSource.getSysLangValue("lblFloorplans")%>
                        </a>
                    </li>
                </ul>
            </div>
            <%} %>
        </div>
        <div class="nulla">
        </div>
        <div id="aptDesc">
            <asp:PlaceHolder ID="PH_mapsContAll" runat="server">
                <div id="maps">
                    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
                    <div style="float: left; height: 33px; width: 443px;">
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
                                        <span class="streetView">Street view</span>
                                    </a>
                                </li>
                            </asp:PlaceHolder>
                            <li style="display: none;">
                                <a href="#">
                                    <span class="zona">
                                        <%= CurrentSource.getSysLangValue("lblZone")%></span>
                                </a>
                            </li>
                            <li></li>
                        </ul>
                        <div id="selectZone" style="display: none;">
                            <select name="scegliZone">
                                <option value="Centro">#Voglio andare a...</option>
                                <option value="Colosseo">#Colosseo</option>
                                <option value="PiazzaSpagna">#Piazza di Spagna</option>
                                <option value="FontanaTrevi">#Fontana di Trevi</option>
                            </select>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div style="width: 436px; height: 286px; margin: 7px;">
                        <asp:PlaceHolder ID="PH_gmapSvcont" runat="server">
                            <div id="gmapSvcont" style="width: 430px; height: 280px; display: none;">
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
                            <div id="mapCont" style="width: 430px; height: 280px;">
                            </div>
                            <script type="text/javascript">
                                var map = null;
                                var marker = null;
                                var directionsDisplay = null;
                                var directionsService = null;
                                var _point = null;
                                var overlay;

                                function setMap() {

                                    
                                    var _IconImage = new google.maps.MarkerImage("<%=HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) %><%=CurrentAppSettings.ROOT_PATH %><%= WL.getWLMapMarker() %>", new google.maps.Size(82, 45), new google.maps.Point(0, 0), new google.maps.Point(0, 45));
                                    var _IconShadow = new google.maps.MarkerImage("<%=HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) %><%=CurrentAppSettings.ROOT_PATH %>images/google_maps/google_icon_shadow.png", new google.maps.Size(82, 45), new google.maps.Point(0, 0), new google.maps.Point(0, 45));
                                    var _IconShape = {
                                        coord: [1, 1, 1, 45, 82, 45, 82, 1],
                                        type: 'poly'
                                    };
                                    var _coords = "<%=currEstateTB.google_maps %>";
                                    _point = new google.maps.LatLng(parseFloat(_coords.split('|')[0].replace(',', '.')), parseFloat(_coords.split('|')[1].replace(',', '.')));
                                    map = new google.maps.Map(document.getElementById("mapCont"), { zoom: 15, mapTypeId: google.maps.MapTypeId.ROADMAP, center: _point, streetViewControl: false, mapTypeControl: false });
                                    //markerOptions = { position: _point, map: map, shadow: _IconShadow, icon: _IconImage, shape: _IconShape };
                                    markerOptions = { position: _point, map: map, icon: _IconImage, shape: _IconShape };
                                    marker = new google.maps.Marker(markerOptions);

                                    overlay = new google.maps.OverlayView();
                                    overlay.draw = function() {};
                                    overlay.setMap(map); // 'map' is new google.maps.Map(...)

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
                                    initializeStreetView();
                                }

                                <%= ltr_hidePriceColScript.Text %> 
                                $(function () {
                                    $(window).load(function () {
                                        google.load("maps", "3.1", {"callback" : mapsLoaded, other_params: "sensor=false&key=<%=App.GOOGLE_MAPS_KEY %>&language=<%=CurrentLang.ABBR.Substring(0,2)%>"});

                                        //$.getScript("http://maps.google.com/maps/api/js?v=3.1&sensor=false&language=<%=CurrentLang.ABBR.Substring(0,2)%>", function (data, textStatus, jqxhr) {
                                        //});
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
            <%=currEstateLN.description%>
            <div style="float: right;">
                <% if (!clUtils.getConfig(CURRENT_SESSION_ID).myPreferedEstateList.Contains(IdEstate))
                   {%>
                <a id="a1" href="javascript:refreshMyList(<%= IdEstate%>);" class="btn addToMyList_toggler" title="<%=CurrentSource.getSysLangValue("lblAddToWishList")%>">
                    <span>
                        <%=CurrentSource.getSysLangValue("lblAddToWishList")%>
                    </span>
                </a>
                <%
                   }%>
                <a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString()) %>?IdEstate=<%= IdEstate %>" class="btn_concludi">
                    <span>
                        <%= CurrentSource.getSysLangValue("lblEnquire")%></span>
                </a>
            </div>
        </div>
        <div class="nulla">
        </div>
        <uc2:ucBooking ID="ucBooking" runat="server" />
        <div id="priceInfo">
            <h3 class="titBar">
                <%= CurrentSource.getSysLangValue("lblPrices")%></h3>
            <asp:PlaceHolder ID="PH_priceOnRequestCont" runat="server" Visible="false">
                <%= CurrentSource.getSysLangValue("lblOnRequest")%>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PH_priceListCont" runat="server">
                <asp:PlaceHolder ID="PH_priceListCont_v1" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable" style="float: left;">
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
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable" style="float: left;">
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
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable" style="float: left;">
                        <tr>
                            <th colspan="<%= SeasonDateHeaderColSpan%>" style="border-left: none;">Seasons reference </th>
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
            <!--
			<div class="infoBox">
				<h6>
					Titolo dell'informazione aggiuntiva</h6>
				<p>
					3 notti<br /> 
					ECCEZIONI 
					<br />
					4 notti between 20 apr e 24 apr 
					<br />
					4 notti between 28 dic e 03 gen
				</p>
			</div>-->
        </div>
        <asp:ListView ID="LV_special_offer" runat="server" ViewStateMode="Disabled">
            <ItemTemplate>
                <a class="commento <%# Eval("class_type") %>">
                    <span class="commCont">
                        <span class="userName">
                            <%# Eval("title") %>&nbsp;<%# Eval("sub_title") %></span>
                        <span class="nulla"></span>
                        <span class="commentoTxt">
                            <%= CurrentSource.getSysLangValue("lblDateFrom")%>:&nbsp;<%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>&nbsp;&nbsp;-&nbsp;&nbsp;<%= CurrentSource.getSysLangValue("lblDateTo")%>:&nbsp;<%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID,"") %>
                        </span>
                    </span>
                    <span class="alt_estate_euro">-<%# Eval("pr_discount").objToDecimal().ToString("N2") %>&nbsp;% </span>
                </a>
            </ItemTemplate>
            <EmptyDataTemplate>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <div id="specOffert">
                    <h3 class="titBar">
                        <asp:Literal ID="ltr_title" runat="server"></asp:Literal>
                    </h3>
                    <div id="ListspecOffert">
                        <a id="itemPlaceholder" runat="server" />
                    </div>
                </div>
            </LayoutTemplate>
        </asp:ListView>
        <div id="vieCorrelate" runat="server" visible="false" style="display: block;" viewstatemode="Disabled">
            <h3 class="titBar">Vie da visitare nelle vicinanze </h3>
            <a class="commento" href="#">
                <img alt="Nome via" class="aptPhoto" src="/images/via-del-corso.jpg">
                <span class="commCont">
                    <span class="userName">Via del Corso</span>
                    <span class="commentsNum">commenti: <strong>10</strong></span>
                    <span class="nulla"></span>
                    <span class="commentoTxt">
                        <p>Oggi inzia una nuova rubrica per i nostri “followers", fatta di foto e piccole descrizioni attraverso cui cercheremo di farvi sentire più vicina la nostra città. </p>
                    </span>
                </span>
                <span class="alt_estate_euro">dettagli</span>
            </a>
            <a class="commento" href="#">
                <img alt="Nome via" class="aptPhoto" src="/images/via-del-corso.jpg">
                <span class="commCont">
                    <span class="userName">Via del Corso</span>
                    <span class="commentsNum">commenti: <strong>10</strong></span>
                    <span class="nulla"></span>
                    <span class="commentoTxt">
                        <p>Oggi inzia una nuova rubrica per i nostri “followers", fatta di foto e piccole descrizioni attraverso cui cercheremo di farvi sentire più vicina la nostra città. </p>
                    </span>
                </span>
                <span class="alt_estate_euro">dettagli</span>
            </a>
            <a class="commento" href="#">
                <img alt="Nome via" class="aptPhoto" src="/images/via-del-corso.jpg">
                <span class="commCont">
                    <span class="userName">Via del Corso</span>
                    <span class="commentsNum">commenti: <strong>10</strong></span>
                    <span class="nulla"></span>
                    <span class="commentoTxt">
                        <p>Oggi inzia una nuova rubrica per i nostri “followers", fatta di foto e piccole descrizioni attraverso cui cercheremo di farvi sentire più vicina la nostra città. </p>
                    </span>
                </span>
                <span class="alt_estate_euro">dettagli</span>
            </a>
        </div>
        <div id="guestBook">
            <h3 class="titBar">
                <%= CurrentSource.getSysLangValue("lblGuestbook")%></h3>
            <a href="<%=CurrentSource.getPagePath("11", "stp", CurrentLang.ID.ToString()) %>?IdEstate=<%= IdEstate %>" class="signGuest">
                <%= CurrentSource.getSysLangValue("lblLeaveCommentForThisApartment")%>
            </a>
            <div id="commentListCont">
                <input type="hidden" id="hf_estComment_currPage" value="1" />
                <div class="loading_img">
                </div>
            </div>
        </div>
        <div id="alternativeEstate" style="display: none;">
            <h3 class="titBar">
                <%= CurrentSource.getSysLangValue("lblViewAlternatives")%></h3>
            <div id="alternativeEstate_loading" class="loadingSrc">
                <span>
                    <%=CurrentSource.getSysLangValue("lblLoadingData")%>
                </span>
            </div>
            <div id="alternativeEstate_cont">
            </div>
        </div>
    </div>
</asp:Content>
