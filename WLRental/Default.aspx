<%@ Page Title="" Language="C#" MasterPageFile="/WLRental/common/MP_WLRental.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RentalInRome.WLRental.Default" %>

<%--<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc1" %>
--%>
<%@ Register Src="uc/UC_search.ascx" TagName="UC_search" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <%--<title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" /> --%>
    <title><%= App.WLAgent.WL_name %></title>
    <meta name="description" content="<%= App.WLAgent.WL_name %>" />
    <meta name="keywords" content="<%= App.WLAgent.WL_name %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script type="text/javascript" src="/jquery/plugin/jquery.overlay.min.js"></script>
    <script type="text/javascript" src="/jquery/plugin/jquery.xslider.min.js"></script>

    <script type="text/javascript">
        var imgList = ''
        + '    <ul id="photos">'
        + '        <li><a href="colosseumapartments.htm" class="imgHome">'
        + '            <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/colosseo-apartment.jpg" alt="rome apartments, colosseum area">'
        + '        </a></li>'
        + '        <li><a href="saintpeterapartments.htm" class="imgHome">'
        + '            <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/sanpietro-apartment.jpg" alt="rome apartments, saint peter area">'
        + '        </a></li>'
        + '        <li><a href="spagnastepsapartments.htm" class="imgHome">'
        + '            <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/spagna-apartment.jpg" alt="rome apartments, spanish steps area">'
        + '        </a></li>'
        + '        <li><a href="campodefiori.htm" class="imgHome">'
        + '            <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/campodeifiori-apartment.jpg" alt="rome apartments, campo dei fiori area">'
        + '        </a></li>'
        + '        <li><a href="piazzanavona.htm" class="imgHome">'
        + '            <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/piazzanavona-apartment.jpg" alt="rome apartments, piazza navona area">'
        + '        </a></li>'
        + '        <li><a href="trastevereapartments.htm" class="imgHome">'
        + '            <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/trastevere-apartment.jpg" alt="rome apartments, trastevere area">'
        + '        </a></li>'
        + '        <li><a href="trevifountainapartments.htm" class="imgHome">'
        + '            <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/trevi-apartment.jpg" alt="rome apartments, trevi area">'
        + '        </a></li>'
        + '        <li><a href="parioli.htm" class="imgHome">'
        + '            <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/parioli-apartment.jpg" alt="rome apartments, parioli area">'
        + '        </a></li>'
        + '        <li><a href="stazionetermini.htm" class="imgHome">'
        + '            <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/termini-apartment.jpg" alt="rome apartments, termini area">'
        + '        </a></li>'
        + '    </ul>';

        $(document).ready(function () {
            $(window).load(function () {
                $('#overflow').html(imgList);
                $('#photos').xslider({ height: 244, effect: 'fade', timeout: 3000, autoPlay: true, navigation: null });
                $('#scrollEvento').xslider({ effect: 'fade', timeout: 3000, autoPlay: true, navigation: null });
                $('#scrollPress').xslider({ effect: 'fade', timeout: 3000, autoPlay: true, navigation: null });
                $('#SpecialOfferSelected').xslider({ effect: 'fade', timeout: 3000, autoPlay: true, navigation: null });
                $('#homeAttentionApts').xslider({ height: 54, width: 693, effect: 'fade', timeout: 3000, autoPlay: true, navigation: null });
                $('#specialOffersHome').xslider({ height: 250, width: 882, effect: 'slide', timeout: 5000, autoPlay: true, navigation: '#specialOffersHomeNav' });
            });
        });
    </script>



    <style>
        #scrollEvento li, #scrollPress li {
            width: 310px;
            height: 105px;
        }

        #offersDx .xslider {
            width: 312px;
            height: 207px;
            display: inline-block;
        }

        #SpecialOfferSelected {
            position: relative;
            display: inline-block;
            width: 312px;
            height: 207px;
            overflow: hidden;
        }

            #SpecialOfferSelected li {
                position: relative;
                display: inline-block;
                width: 312px;
                height: 207px;
                overflow: hidden;
            }
    </style>

    <script type="text/javascript">
        var banner3Content = ''
            + '<a class="banner3" href="<%=CurrentSource.getPagePath("13", "stp", CurrentLang.ID.ToString()) %>" style="margin-left: 7px;">'
            + '    <img src="/images/css/long-term-<%=CurrentLang.ABBR.Substring(0,2)%>.gif" alt="rome apartments at shocking prices" />'
            + '</a>'
            + '<div class="banner3" style="display:none;">'
            + '    <img src="<%=CurrentAppSettings.ROOT_PATH %><%= CurrentSource.getSysLangValue("lblimgairportransfer")%>" alt="rome tour" border="0" usemap="#Map" />'
            + '    <map name="Map" id="Map">'
            + '        <area shape="rect" coords="0,3,203,65" href="<%=CurrentSource.getPagePath("10", "stp", CurrentLang.ID.ToString()) %>" alt="rome tour" />'
            + '        <area shape="rect" coords="205,0,286,86" href="http://www.youtube.com/watch?v=QuME84eQOBg" target="_blank" alt="pickup video" />'
            + '    </map>'
            + '</div>'
            + '<a class="banner3" href="<%=CurrentSource.getPagePath("23", "stp", CurrentLang.ID.ToString()) %>">'
            + '    <img src="<%=CurrentAppSettings.ROOT_PATH %><%= CurrentSource.getSysLangValue("lblfullrefund")%>" alt="rome apartments at shocking prices" />'
            + '</a>'
            + '';
        $(document).ready(function () {
            $(window).load(function () {
               // $('#banner3Cont').html(banner3Content);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_spDiscountCheckHomePage" runat="server" Value="1" />
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_summary" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <div id="bannerHome">

        <div id="contattiHome">
            <strong class="titCon">Reservations Center</strong>
            Tel: <strong style="font-size: 15px;"><%= App.WLAgent.contactPhone %></strong>
            <br />
            <em>(24h/day, 7/7 days)</em>
        </div>
        <div id="overflow">
            <ul id="photos">
                <li>
                    <a href="colosseumapartments.htm" class="imgHome">
                        <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>WLRental/images/homepage_zone.jpg" alt="rome apartments, colosseum area">
                    </a>
                </li>
                <li>
                    <a href="saintpeterapartments.htm" class="imgHome">
                        <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>WLRental/images/homepage_zone.jpg" alt="rome apartments, saint peter area">
                    </a>
                </li>
                <li>
                    <a href="spagnastepsapartments.htm" class="imgHome">
                        <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>WLRental/images/homepage_zone.jpg" alt="rome apartments, spanish steps area">
                    </a>
                </li>
                <li>
                    <a href="campodefiori.htm" class="imgHome">
                        <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>WLRental/images/homepage_zone.jpg" alt="rome apartments, campo dei fiori area">
                    </a>
                </li>
                <li>
                    <a href="piazzanavona.htm" class="imgHome">
                        <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>WLRental/images/homepage_zone.jpg" alt="rome apartments, piazza navona area">
                    </a>
                </li>
                <li>
                    <a href="trastevereapartments.htm" class="imgHome">
                        <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>WLRental/images/homepage_zone.jpg" alt="rome apartments, trastevere area">
                    </a>
                </li>
                <li>
                    <a href="trevifountainapartments.htm" class="imgHome">
                        <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>WLRental/images/homepage_zone.jpg" alt="rome apartments, trevi area">
                    </a>
                </li>
                <li>
                    <a href="parioli.htm" class="imgHome">
                        <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>WLRental/images/homepage_zone.jpg" alt="rome apartments, parioli area">
                    </a>
                </li>
                <li>
                    <a href="stazionetermini.htm" class="imgHome">
                        <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>WLRental/images/homepage_zone.jpg" alt="rome apartments, termini area">
                    </a>
                </li>
            </ul>
        </div>
    </div>
    <div class="nulla">
    </div>
    <div id="aptHome">
        <h1 class="tabTitle">
            <%=CurrentSource.getSysLangValue("lblApartmentsInRome")%></h1>
        <div class="nulla">
        </div>
        <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" currType="zoneHome" />
        <div id="searchHome">
            <div class="tabTitle" style="float: left;">
                <%=CurrentSource.getSysLangValue("lblSearch")%>
            </div>
            <uc1:UC_search ID="UC_search1" runat="server" />
        </div>
    </div>
    <div class="nulla">
    </div>
    <input type="hidden" id="rnt_otherOffer_state" />
    <script type="text/javascript">
        function rnt_toggle_otherOffer() {
            if ($("#rnt_otherOffer_state").val() == "1") {
                $("#rnt_otherOffer_toggler").removeClass("opened");
                $("#rnt_otherOffer_toggler").addClass("closed");
                $("#rnt_otherOffer_cont").slideUp();
                $("#rnt_otherOffer_state").val("0");
            }
            else {
                $("#rnt_otherOffer_toggler").removeClass("closed");
                $("#rnt_otherOffer_toggler").addClass("opened");
                $("#rnt_otherOffer_cont").slideDown();
                $("#rnt_otherOffer_state").val("1");
            }
        }
        $("#rnt_otherOffer_state").val("0");
    </script>
    <!--
    <div id="news_home">
        <div class="tit">
            <span>
                <%=CurrentSource.getSysLangValue("lblSpecialOffers1")%></span>
            <a id="rnt_otherOffer_toggler" class="btnaltreZone" href="javascript:rnt_toggle_otherOffer()">
                <%=CurrentSource.getSysLangValue("lblotheroffer")%></a>
            <div class="nulla">
            </div>
            <div style="position: absolute;">
                <div id="rnt_otherOffer_cont" class="slide_down_cont" style="display: none; width: auto; margin-left: -10px; margin-top: 8px;">
                    <a href="<%=CurrentSource.getPagePath("25", "stp", CurrentLang.ID.ToString()) %>">
                        <%= CurrentSource.getSysLangValue("lnkLowCostApartmentsInRome")%></a>
                    <a href="<%=CurrentSource.getPagePath("26", "stp", CurrentLang.ID.ToString()) %>">
                        <%= CurrentSource.getSysLangValue("lnkQualityApartmentsInRome")%></a>
                    <a href="<%=CurrentSource.getPagePath("27", "stp", CurrentLang.ID.ToString()) %>">
                        <%= CurrentSource.getSysLangValue("lnkHighQualityApartmentsInRome")%></a>
                    <a href="<%=CurrentSource.getPagePath("28", "stp", CurrentLang.ID.ToString()) %>">
                        <%= CurrentSource.getSysLangValue("lnkLuxuryApartmentsInRome")%></a>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
            <a href="/apartmentsinromefrom59.htm">*Apartments from €59 x day</a>
            <a href="/top150luxury.htm">*Top 150 Luxury apartments in Rome</a>
            <a href="/terraceapartments.htm">*Rome Terrace apartments</a>
        </asp:PlaceHolder>
        <div class="cont" style="height: 54px; width: 691px; overflow: hidden;">
            <asp:ListView ID="LV_homepage_attention" runat="server" GroupItemCount="3">
                <ItemTemplate>
                    <div class="item <%# Eval("css_class") %>">
                        <a href=" <%# CurrentSource.getPagePath(""+Eval("pid_estate"),"pg_estate",CurrentLang.ID.ToString()) %>">
                            <%# CurrentSource.rntEstate_titleWithzone(Eval("pid_estate").objToInt32(), CurrentLang.ID, " - ", true, "")%>
                            <br />
                            <%# Eval("title") %>
                        </a>
                    </div>
                </ItemTemplate>
                <GroupTemplate>
                    <li style="height: 54px; width: 693px; display: block; float: left;">
                        <div id="itemPlaceholder" runat="server" />
                    </li>
                </GroupTemplate>
                <EmptyDataTemplate>
                    <div>
                    </div>
                    <div>
                    </div>
                    <div>
                    </div>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <ul id="homeAttentionApts" style="list-style: none outside none;">
                        <li id="groupPlaceholder" runat="server" />
                    </ul>
                </LayoutTemplate>
            </asp:ListView>
        </div>
    </div>
    -->

    <%--

    <div id="news_home">
        <a href="<%=CurrentSource.getPagePath("37", "stp", CurrentLang.ID.ToString()) %>" class="specialOffers" title="special offers">
            <img src="/images/banner/banner_special_offers_<%=CurrentLang.ABBR.Substring(0,2)%>.gif" alt="" />
        </a>
    </div>
    --%>
    <asp:PlaceHolder ID="PH_spDiscountList" runat="server">
        <h3 class="specialOffersTitHome"><%=CurrentSource.getSysLangValue("lblSpecialOffers1")%></h3>
        <a class="specialOffersLinkHome" href="<%=CurrentSource.getPagePath("37", "stp", CurrentLang.ID.ToString()) %>"><%=CurrentSource.getSysLangValue("listAllPages")%></a>
        <hr style="width: 862px; border: none; border-bottom: 1px dashed #d6d4e1; margin: 0 18px; display: block; clear: both; float: left;" />
        <asp:ListView ID="LV_spDiscountList" runat="server" GroupItemCount="4" ViewStateMode="Disabled">
            <ItemTemplate>
                <a class="content_block specOff" href="/<%# Eval("page_path") %>#specOffert">
                    <span class="testi">
                        <span class="tit">
                            <h2><%# Eval("title") %></h2>
                            <span class="nulla"></span>
                            <%# Eval("is_online_booking")+""=="1"?"<img class='ico_book_list' src='/WLRental/images/book-now.gif'>":"" %>
                            <span class="cat"><%# Eval("zone") %></span>
                        </span>
                    </span>
                    <span class="nulla"></span>
                    <span class="dettaglio">
                        <img class="foto" width="162" height="89px" src="/<%# Eval("img_preview_1") %>" alt="<%# Eval("title") %>" />
                        <span class="nulla"></span>
                        <span class="dx_dett">
                            <span class="dateSpecOff">
                                <span class="fromTo"><%= CurrentSource.getSysLangValue("lblDateFrom") %></span><span class="data"><%# ((DateTime)Eval("spDiscountDateStart")).formatCustom("#dd#.#mm#.#yy#", CurrentLang.ID, "") %></span><span class="fromTo"><%= CurrentSource.getSysLangValue("lblDateTo") %></span><span class="data"><%# ((DateTime)Eval("spDiscountDateEnd")).formatCustom("#dd#.#mm#.#yy#", CurrentLang.ID, "") %></span>
                            </span>
                            <span class="euro">-<%# Eval("spDiscountAmount") %>%</span>
                        </span>
                        <span class="nulla"></span>
                    </span>
                    <span class="nulla"></span>
                </a>
            </ItemTemplate>
            <GroupTemplate>
                <li id="ListspecOffert">
                    <a id="itemPlaceholder" runat="server" />
                </li>
            </GroupTemplate>
            <EmptyDataTemplate>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <ul id="specialOffersHome" class="display dett_view">
                    <li id="groupPlaceholder" runat="server" />
                </ul>
            </LayoutTemplate>
        </asp:ListView>
    </asp:PlaceHolder>
    <div class="nulla">
    </div>
    <div id="banner3Cont">
    </div>
    <div class="nulla">
    </div>
    <%--<div id="colSx">
        <div id="serviziHome">
            <h2 class="tabTitle" style="float: left;">
                <%=CurrentSource.getSysLangValue("reqServices")%>
            </h2>
            <div id="serv">

                <a class="servizio" href="<%=CurrentSource.getPagePath("7", "stp", CurrentLang.ID.ToString()) %>">
                    <span>
                        <%=CurrentSource.getSysLangValue("lblmatrimonioaroma")%>
                    </span>
                </a>
                <a class="servizio" href="<%=CurrentSource.getPagePath("12", "stp", CurrentLang.ID.ToString()) %>">
                    <span style="font-size: 15px; color: #c3451c;">
                        <%=CurrentSource.getSysLangValue("lbllastminutebooking")%></span>
                </a>
                <a class="servizio" href="http://www.romeasyoffice.com/" target="_blank" style="display: none;">
                    <span>
                        <%=CurrentSource.getSysLangValue("lblofficerome")%></span>
                </a>
                <a class="servizio" href="<%=CurrentSource.getPagePath("14", "stp", CurrentLang.ID.ToString()) %>" style="display: none;">
                    <span>
                        <%=CurrentSource.getSysLangValue("lblcatalogo")%></span>
                </a>
            </div>
        </div>

    </div>--%>
    <%--<div id="offersDx">

        <asp:PlaceHolder ID="PH_proprietariIta" runat="server" Visible="false">
            <a style="width: 312px; float: left; display: block; height: 207px;" href="<%=CurrentSource.getPagePath("8", "stp", CurrentLang.ID.ToString()) %>">
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/banner_owner_<%= CurrentLang.ID %>.jpg" style="border: none;" align="right" />
            </a>
        </asp:PlaceHolder>
        <asp:Literal ID="PH_proprietariOtherLang" runat="server">
        </asp:Literal>
        <asp:Literal ID="ltr_specialOfferTemplate" runat="server" Visible="false">
           <div class="bannerdx1line" style="margin-left:0;">
                <a href="#pagePath#" class="boxapt">
                    <img src="/images/css/book_now_fasciatop.png" class="icobooknow" />
                    <span class="boxsconto"><span><span>
                        #prPercentage#
                        #dateFromTo#
                    </span></span></span>
                    <span class="boxaptintmask">
                            <span class="dx">
                            #title#
                            #zoneTitle#
                            </span>
                            <span class="sx">
                                <span class="day">#lblPriceFrom#</span>
                                #price#
                                <span class="day" style="text-align: right; padding-right: 10px; margin-top: -3px;">#lblMinPriceUpTo4Guests#</span>
                            </span>
                    </span>
                    <span class="boxaptint">
                        #imgPreview#
                    </span>
                </a>
                <!--RIPETI A-->
           </div>
        </asp:Literal>
       <%-- <a style="width: 312px; float: right; display: none; height: 207px;" href="<%=CurrentSource.getPagePath("35", "stp", CurrentLang.ID.ToString()) %>">
            <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/area_agenzie_<%= CurrentLang.ID %>.jpg" style="border: none;" />
        </a>


        <div class="nulla">
        </div>

    </div>--%>

    <div class="nulla">
    </div>




</asp:Content>
