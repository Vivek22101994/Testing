<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="pg_estate_pdf.aspx.cs" Inherits="RentalInRome.pg_estate_pdf" %>

<%@ Register Src="uc/UC_menu_breadcrumb.ascx" TagName="UC_menu_breadcrumb" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="google-site-verification" content="NlSx8KjFFw9tuY_pwuH8kIoeoYq88Jvd26kT5mEsSbo" />
    <link rel="shortcut icon" href="<%=CurrentAppSettings.ROOT_PATH %>favicon.ico" />
    <link rel="stylesheet" type="text/css" href="/css/style.css<%="?tmp="+DateTime.Now.Ticks %>" />
    <link rel="stylesheet" type="text/css" href="/css/common.css<%="?tmp="+DateTime.Now.Ticks %>" />
    <script src="/jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>
</head>
<body style="width: 1000px;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="container">
            <div id="header">
                <a href="<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>" class="logo">
                    <img src="/images/css/logo.gif" title="Rent apartments in Rome, Italy" alt="Rent apartments in Rome, Italy" />
                </a>
                <%--<uc1:UC_static_block ID="UC_static_block2" runat="server" BlockID="11" />--%>
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_TBL_LANGs" Where="is_active == 1 &amp;&amp; is_public == 1">
                </asp:LinqDataSource>
                <div style="float: right;">
                    <%= contUtils.getBlock(3, CurrentLang.ID, "") %>
                </div>
                <div class="nulla">
                </div>
                <div id="menuMain">
                    <ul>
                        <li>
                            <a href="<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>" style="border-left: none;">HOME</a>
                        </li>
                        <li>
                            <a href="<%=CurrentSource.getPagePath("5", "stp", CurrentLang.ID.ToString()) %>">
                                <%= CurrentSource.getSysLangValue("lblAboutUs")%></a>
                        </li>
                        <li>
                            <a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString()) %>">
                                <%= CurrentSource.getSysLangValue("lblContacts")%></a>
                        </li>
                        <li>
                            <a href="<%=CurrentSource.getPagePath("9", "stp", CurrentLang.ID.ToString()) %>">
                                <%= CurrentSource.getSysLangValue("menu_Press")%></a>
                        </li>
                        <li>
                            <a href="<%=CurrentSource.getPagePath("24", "stp", CurrentLang.ID.ToString()) %>">
                                <%=CurrentSource.getSysLangValue("menuRomeVillas")%></a>
                        </li>
                        <li>
                            <a target="_blank" href="http://www.rentalcastles.com/">
                                <%=CurrentSource.getSysLangValue("menuRentCastle")%></a>
                        </li>
                        <li>
                            <a target="_blank" href="http://www.millennium-beb.com/">
                                <%=CurrentSource.getSysLangValue("menuBeB")%></a>
                        </li>
                        <li>
                            <a target="_blank" href="http://www.rentalinvenice.com">
                                <%=CurrentSource.getSysLangValue("menuVenice")%></a>
                        </li>
                        <li>
                            <a target="_blank" href="http://www.rentalinflorence.com">
                                <%=CurrentSource.getSysLangValue("menuFlorence")%></a>
                        </li>
                        <li>
                            <a href="<%=CurrentSource.getPagePath("10", "stp", CurrentLang.ID.ToString()) %>">
                                <%= CurrentSource.getSysLangValue("menu_LimousineService")%>
                            </a>
                        </li>
                    </ul>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div id="main">
                <div>
                    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
                    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
                    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
                    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
                    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
                    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
                    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
                    <asp:Literal ID="ltr_minPriceDay" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="ltr_minPriceWeek" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="ltr_otherZones" runat="server" Visible="false"></asp:Literal>
                    <asp:HiddenField ID="HF_unique" runat="server" Value="" />
                    <asp:HiddenField ID="HF_id" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_pid_zone" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_min_nights" runat="server" Value="1" />
                    <asp:HiddenField ID="HF_pr_percentage" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_pr_deposit" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_is_online_booking" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_num_persons_min" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_num_persons_max" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_num_persons_child" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_num_persons_adult" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_num_persons_optional" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_coord" runat="server" Value="" />
                    <asp:HiddenField ID="HF_sv_coords" runat="server" />
                    <asp:HiddenField ID="HF_sv_yaw" runat="server" />
                    <asp:HiddenField ID="HF_sv_pitch" runat="server" />
                    <asp:HiddenField ID="HF_sv_zoom" runat="server" />
                    <asp:Literal ID="ltr_viewBookNow" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="ltr_gmapPointsScript" runat="server" Visible="false"></asp:Literal>
                    <div id="breadcrumbscont">
                        <uc1:UC_menu_breadcrumb ID="UC_menu_breadcrumb1" runat="server" />
                    </div>
                    <div class="nulla">
                    </div>
                    <div id="schedaDett">
                        <div id="aptDetails">

                            <div class="titApt">
                                <div>
                                    <%if (HF_is_online_booking.Value == "1")
                                      {  %>
                                    <img class="ico_onlinebooking" src="/images/css/book_now_fasciatop.png" alt="" />
                                    <%} %>
                                    <h1>
                                        <%=ltr_title.Text%>
                                        <span class="zonaApt"><%=CurrentSource.locZone_title(HF_pid_zone.Value.objToInt32(), CurrentLang.ID, "")%></span>
                                    </h1>
                                </div>
                            </div>
                            <ul class="detailsList">
                                <%=(ltr_sub_title.Text.Trim() != "") ? "<li>"+ltr_sub_title.Text+"</li>" : ""%>
                                <% if (HF_num_persons_optional.Value.ToInt32() != 0)
                                   { %>
                                <li>
                                    <%= CurrentSource.getSysLangValue("lblUpTo")%> <%= HF_num_persons_max.Value %> <%= CurrentSource.getSysLangValue("lblPersons")%> (<%= HF_num_persons_adult.Value %> + <%= HF_num_persons_optional.Value%>)
                                </li>
                                <%}
                                   else
                                   { %>
                                <li>
                                    <%= HF_num_persons_max.Value %> <%= CurrentSource.getSysLangValue("lblSleeps")%>
                                </li>
                                <%} %>
                                <% if (currEstate.num_bed_single.objToInt32() != 0)
                                   { %>
                                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=(currEstate.num_bed_single.objToInt32() > 1) ? currEstate.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsSingle") : currEstate.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedSingle")%>
                                </li>
                                <%}%>
                                <% if (currEstate.num_bed_double.objToInt32() != 0)
                                   { %>
                                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=(currEstate.num_bed_double.objToInt32() > 1) ? currEstate.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDouble") : currEstate.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDouble")%>
                                </li>
                                <%}%>
                                <% if (currEstate.num_bed_double_divisible.objToInt32() != 0)
                                   { %>
                                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=(currEstate.num_bed_double_divisible.objToInt32() > 1) ? currEstate.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDoubleDivisible") : currEstate.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDoubleDivisible")%>
                                </li>
                                <%}%>
                                <% if ((currEstate.num_sofa_double.objToInt32()) != 0)
                                   { %>
                                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=((currEstate.num_sofa_double.objToInt32()) > 1) ? (currEstate.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsDouble") : (currEstate.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedDouble")%>
                                </li>
                                <%}%>
                                <% if ((currEstate.num_sofa_single.objToInt32()) != 0)
                                   { %>
                                <li style="background: none;">&nbsp;-&nbsp;&nbsp;<%=((currEstate.num_sofa_single.objToInt32()) > 1) ? (currEstate.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsSingle") : (currEstate.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedSingle")%>
                                </li>
                                <%}%>
                                <li>
                                    <%=(currEstate.num_rooms_bed > 1) ? currEstate.num_rooms_bed + " " + CurrentSource.getSysLangValue("lblBedRooms") : currEstate.num_rooms_bed + " " + CurrentSource.getSysLangValue("lblBedRoom")%>
                                </li>
                                <li>
                                    <%=(currEstate.num_rooms_bath > 1) ? currEstate.num_rooms_bath + " " + CurrentSource.getSysLangValue("lblBathRooms") : currEstate.num_rooms_bath + " " + CurrentSource.getSysLangValue("lblBathRoom")%>
                                </li>
                                <li>
                                    <%= CurrentSource.getSysLangValue("lblStayMin")%> <%=(HF_min_nights.Value.ToInt32() > 1) ? HF_min_nights.Value + " " + CurrentSource.getSysLangValue("lblNights") : HF_min_nights.Value + " " + CurrentSource.getSysLangValue("lblNight")%>
                                </li>
                            </ul>
                            <div class="nulla">
                            </div>
                            <asp:ListView ID="LV_config" runat="server">
                                <ItemTemplate>
                                    <img src="/<%# Eval("img_thumb") %>" title="<%# CurrentSource.rnt_configTitle(Eval("id").objToInt32(),CurrentLang.ID,"") %>" width="34" height="32" class="ico_tooltip_right" />
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <div id="serviceIcons">
                                        <img id="itemPlaceholder" runat="server" alt="" />
                                        <div class="nulla"></div>
                                    </div>
                                    <div class="nulla"></div>
                                </LayoutTemplate>
                            </asp:ListView>
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
                                                <strong><%= ltr_minPriceDay.Text %>&nbsp;&euro;</strong> <%= CurrentSource.getSysLangValue("lblDaily")%></li>
                                            <li>
                                                <strong><%= ltr_minPriceWeek.Text %>&nbsp;&euro;</strong> <%= CurrentSource.getSysLangValue("lblWeekly")%></li>
                                        </ul>
                                    </div>
                                </div>
                                <img src="<%=(ltr_img_banner.Text.Trim()!="")?IMG_ROOT+ltr_img_banner.Text+"?tmp="+DateTime.Now.Ticks:"/images/css/default-apt-banner.jpg"%>" alt="<%=CurrentSource.locZone_title(HF_pid_zone.Value.objToInt32(), CurrentLang.ID, "")%> - <%=ltr_title.Text%>" />
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div id="aptDesc">
                            <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="false">
                                <img src="<%=HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) %><%=CurrentAppSettings.ROOT_PATH %>images/google_maps/google_icon_logo.png" alt="" style="display: none;" />
                                <script type="text/javascript" src="http://maps.google.com/maps/api/js?v=3.1&sensor=false&key=<%=App.GOOGLE_MAPS_KEY %>&language=<%=CurrentLang.ABBR.Substring(0,2)%>"></script>
                                <div id="maps" style="height: 295px;">
                                    <div style="width: 436px; height: 286px; margin: 7px; overflow: hidden;">
                                        <asp:PlaceHolder ID="PH_mapCont" runat="server">
                                            <div id="mapCont" style="width: 430px; height: 280px; overflow: hidden;">
                                            </div>
                                            <script type="text/javascript">
                                                var map = null;
                                                var marker = null;
                                                var directionsDisplay = null;
                                                var directionsService = null;
                                                var _point = null;
                                                var overlay;

                                                function setMap() {


                                                    var _IconImage = new google.maps.MarkerImage("<%=HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) %><%=CurrentAppSettings.ROOT_PATH %>images/google_maps/google_icon_logo.png", new google.maps.Size(82, 45), new google.maps.Point(0, 0), new google.maps.Point(0, 45));
                                                    var _IconShadow = new google.maps.MarkerImage("<%=HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) %><%=CurrentAppSettings.ROOT_PATH %>images/google_maps/google_icon_shadow.png", new google.maps.Size(82, 45), new google.maps.Point(0, 0), new google.maps.Point(0, 45));
                                                    var _IconShape = {
                                                        coord: [1, 1, 1, 45, 82, 45, 82, 1],
                                                        type: 'poly'
                                                    };
                                                    var _coords = $get("<%=HF_coord.ClientID %>").value;
                                                    _point = new google.maps.LatLng(parseFloat(_coords.split('|')[0].replace(',', '.')), parseFloat(_coords.split('|')[1].replace(',', '.')));
                                                    map = new google.maps.Map(document.getElementById("mapCont"), { zoom: 15, mapTypeId: google.maps.MapTypeId.ROADMAP, center: _point, streetViewControl: false, mapTypeControl: false });
                                                    markerOptions = { position: _point, map: map, shadow: _IconShadow, icon: _IconImage, shape: _IconShape };
                                                    marker = new google.maps.Marker(markerOptions);

                                                    overlay = new google.maps.OverlayView();
                                                    overlay.draw = function () { };
                                                    overlay.setMap(map); // 'map' is new google.maps.Map(...)

				        <%= ltr_gmapPointsScript.Text %>
                                                }
                                                function hidePointToolTip() {
                                                    $("#tooltip").css({ 'display': 'none' });
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
                                                    _body += "<span class='title'>" + title + "</span>";
                                                    if (img + "" != "")
                                                        _body += "<img class='point_preview' alt='" + title + "' title='" + title + "' src='/" + img + "' />";
                                                    $("#tooltip .body").html("" + _body);
                                                    $("#tooltip").css("left", "" + (posX + 10) + "px");
                                                    $("#tooltip").css("top", "" + (posY + 10) + "px");
                                                    $("#tooltip").css("display", "");
                                                }
                                                setMap();
                                            </script>

                                        </asp:PlaceHolder>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                            <%=ltr_description.Text%>
                        </div>
                        <div class="nulla">
                        </div>
                        <div id="priceInfo">

                            <h3 class="titBar"><%= CurrentSource.getSysLangValue("lblPrices")%></h3>
                            <asp:PlaceHolder ID="PH_priceOnRequestCont" runat="server" Visible="false">
                                <%= CurrentSource.getSysLangValue("lblOnRequest")%>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="PH_priceListCont" runat="server">
                                <asp:PlaceHolder ID="PH_priceListCont_v1" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable" style="float: left;">
                                        <tr>
                                            <th style="border-left: none; background: none;">&nbsp;</th>
                                            <th colspan="2"><strong>
                                                <%= CurrentSource.rntPeriod_title(1, CurrentLang.ID, "Low Season")%></strong><br />
                                            </th>
                                            <th colspan="2"><strong>
                                                <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "Medium Season")%></strong><br />
                                            </th>
                                            <th colspan="2"><strong>
                                                <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "High Season")%></strong><br />
                                            </th>
                                            <th colspan="2"><strong>
                                                <%= CurrentSource.rntPeriod_title(3, CurrentLang.ID, "Very High Season")%></strong><br />
                                            </th>
                                        </tr>
                                        <tr>
                                            <td class="prima" style="border-left: none;">&nbsp;
                                            </td>
                                            <td class="prima">
                                                <%= CurrentSource.getSysLangValue("lblDaily")%>:
                                            </td>
                                            <td class="prima">
                                                <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                                            </td>
                                            <td class="prima">
                                                <%= CurrentSource.getSysLangValue("lblDaily")%>:
                                            </td>
                                            <td class="prima">
                                                <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                                            </td>
                                            <td class="prima">
                                                <%= CurrentSource.getSysLangValue("lblDaily")%>:
                                            </td>
                                            <td class="prima">
                                                <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                                            </td>
                                            <td class="prima">
                                                <%= CurrentSource.getSysLangValue("lblDaily")%>:
                                            </td>
                                            <td class="prima">
                                                <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                                            </td>
                                        </tr>
                                        <%= ltr_priceDetails.Text%>
                                        <% if (currEstate.longTermRent == 1 && currEstate.longTermPrMonthly.objToDecimal() > 0)
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
                                                <%=CurrentSource.getSysLangValue("lblFrom") + "&nbsp;&euro;&nbsp;" + currEstate.longTermPrMonthly.objToDecimal().ToString("N2") + "&nbsp;" + CurrentSource.getSysLangValue("lblMonthly")%>
                                            </strong></th>
                                        </tr>
                                        <%
                                           } %>
                                    </table>
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable" style="float: left;">
                                            <tr>
                                                <th colspan="3" style="border-left: none;">Seasons reference
                                                </th>
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
                                            <asp:ListView ID="LVseasonDates" runat="server">
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
                                            <th colspan="3" style="border-left: none;">Seasons reference </th>
                                        </tr>
                                        <tr>
                                            <th><strong>
                                                <%= CurrentSource.rntPeriod_title(1, CurrentLang.ID, "Low Season")%></strong><br />
                                            </th>
                                            <th><strong>
                                                <%= CurrentSource.rntPeriod_title(4, CurrentLang.ID, "Medium Season")%></strong><br />
                                            </th>
                                            <th><strong>
                                                <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "High Season")%></strong><br />
                                            </th>
                                            <th><strong>
                                                <%= CurrentSource.rntPeriod_title(3, CurrentLang.ID, "Very High Season")%></strong><br />
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="padding: 0pt; vertical-align: top;">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                    <tr>
                                                        <td class="prima" style="border-left: none;">
                                                            <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                                        </td>
                                                        <td class="prima">
                                                            <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                                        </td>
                                                    </tr>
                                                    <asp:ListView ID="LVseasonDates_1" runat="server">
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
                                            <td style="padding: 0pt; vertical-align: top;">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                    <tr>
                                                        <td class="prima" style="border-left: none;">
                                                            <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                                        </td>
                                                        <td class="prima">
                                                            <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                                        </td>
                                                    </tr>
                                                    <asp:ListView ID="LVseasonDates_2" runat="server">
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
                                            <td style="padding: 0pt; vertical-align: top;">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                    <tr>
                                                        <td class="prima" style="border-left: none;">
                                                            <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                                        </td>
                                                        <td class="prima">
                                                            <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                                        </td>
                                                    </tr>
                                                    <asp:ListView ID="LVseasonDates_4" runat="server">
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
                                            <td style="padding: 0pt; vertical-align: top;">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                    <tr>
                                                        <td class="prima" style="border-left: none;">
                                                            <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                                        </td>
                                                        <td class="prima">
                                                            <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                                        </td>
                                                    </tr>
                                                    <asp:ListView ID="LVseasonDates_3" runat="server">
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
                                            <%= currEstate.pr_dcS2_1_inDays.objToInt32() > 0 && currEstate.pr_dcS2_1_percent.objToInt32() > 0 ? "<th>Over " + currEstate.pr_dcS2_1_inDays.objToInt32() + " nights <br/><strong>-" + currEstate.pr_dcS2_1_percent.objToInt32() + "&nbsp;%</strong>&nbsp;" + CurrentSource.getSysLangValue("lblDaily") + "</th>" : ""%>
                                            <%= currEstate.pr_dcS2_2_inDays.objToInt32() > 0 && currEstate.pr_dcS2_2_percent.objToInt32() > 0 ? "<th>Over " + currEstate.pr_dcS2_2_inDays.objToInt32() + " nights <br/><strong>-" + currEstate.pr_dcS2_2_percent.objToInt32() + "&nbsp;%</strong>&nbsp;" + CurrentSource.getSysLangValue("lblDaily") + "</th>" : ""%>
                                            <%= currEstate.pr_dcS2_3_inDays.objToInt32() > 0 && currEstate.pr_dcS2_3_percent.objToInt32() > 0 ? "<th>Over " + currEstate.pr_dcS2_3_inDays.objToInt32() + " nights <br/><strong>-" + currEstate.pr_dcS2_3_percent.objToInt32() + "&nbsp;%</strong>&nbsp;" + CurrentSource.getSysLangValue("lblDaily") + "</th>" : ""%>
                                        </tr>
                                        <asp:ListView ID="LVdatesAll" runat="server">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                                <tr>
                                                    <td style="border-left: none; text-align: left;">
                                                        <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>&nbsp;-&nbsp;<%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                                                    </td>
                                                    <td>
                                                        <%# CurrentSource.rntPeriod_title(Eval("pidPeriod").objToInt32(), CurrentLang.ID, "--Season--")%>
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
					<td style="border-left: none;">
						#num_pers#
					</td>
					<td>
						#pr_1#
					</td>
					<td>
						#pr_1_w#
					</td>
                    <td>
						#pr_4#
					</td>
					<td>
						#pr_4_w#
					</td>
					<td>
						#pr_2#
					</td>
					<td>
						#pr_2_w#
					</td>
					<td>
						#pr_3#
					</td>
					<td>
						#pr_3_w#
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
                        <asp:ListView ID="LV_special_offer" runat="server" OnDataBound="LV_special_offer_DataBound">
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
                                    <span class="alt_estate_euro">-<%# Eval("pr_discount").objToDecimal().ToString("N2") %>&nbsp;%
                                    </span>
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

                    </div>
                </div>
            </div>
            <table class="indirizzi">
                <tr>
                    <td align="left" valign="middle">
                        <strong>Roma</strong><br />
                        <strong style="color: #FFF">
                            <%= CurrentSource.getSysLangValue("lblufficiooperativo")%></strong><br />
                        Via dei Riari, 55<br />
                        00165 Roma
                    </td>
                    <td align="left" valign="middle">
                        <strong>Roma</strong><br />
                        <strong style="color: #FFF">
                            <%= CurrentSource.getSysLangValue("lblufficioamm")%></strong><br />
                        Via Appia Nuova, 677
                    <br />
                        00179 Roma
                    </td>
                    <td align="left" valign="middle">
                        <strong>Roma - Cerenova</strong><br />
                        <strong style="color: #FFF">
                            <%= CurrentSource.getSysLangValue("lblufficiooperativo")%></strong><br />
                        Via Mantova, 2<br />
                        00050 Cerenova (Rm)
                    </td>
                    <td align="left" valign="middle">
                        <div class="links" style="font-size: 11px;">
                            <strong style="font-size: 12px;">
                                <%= CurrentSource.getSysLangValue("lblpartner1")%></strong>
                            <br />
                            <a href="http://www.rentalsinmauritius.com/" target="_blank">Rentals in Mauritius</a>
                            |
                        <a href="http://www.magadesign.net" target="_blank" title="web agency">web agency</a>
                        </div>
                        <div class="nulla">
                        </div>
                        <table class="pay" style="margin: 0;">
                            <tr>
                                <td valign="middle">
                                    <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/master_p.gif" width="45" height="28" />
                                </td>
                                <td valign="middle">
                                    <img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/visa_p.gif" width="43" height="28" />
                                </td>
                                <td>
                                    <%= contUtils.getBlock(9, CurrentLang.ID, "") %>
                                </td>
                            </tr>
                        </table>
                        <div class="nulla">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="middle" colspan="4"></td>
                </tr>
            </table>
            <div class="links societa" style="color: #FFF; margin: 5px 0px 15px 10px;">
                <strong>Rental in Rome S.r.l.</strong> - P. IVA: 07824541002 - 2003 - 2012 © all rights reserved
            </div>
            <div class="nulla">
            </div>
        </div>
    </form>
</body>
</html>
