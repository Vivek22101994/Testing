<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_gmap.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_gmap" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="//maps.google.com/maps/api/js?v=3.1&sensor=false&language=it&key=<%=App.GOOGLE_MAPS_KEY %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titolo_main">Gestione Google Maps della struttura:<asp:Literal ID="ltr_title" runat="server"></asp:Literal></h1>
    <div id="fascia1">
        <div style="clear: both; margin: 3px 0 5px 30px;">
            <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
        </div>
    </div>
    <div class="nulla">
    </div>
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HF_coord" runat="server" />
        <asp:HiddenField ID="HF_type" Value="estate" runat="server" />
        <asp:HiddenField ID="HF_referer" Value="estate" runat="server" />
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnk_chiudi" runat="server" OnClick="lnk_chiudi_Click"><span>Chiudi</span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click"><span>Salva</span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnk_saveGoSV" runat="server" OnClick="lnk_saveGoSV_Click"><span>Salva e modifica StreetView</span></asp:LinkButton>
            </div>
            <div class="nulla">
            </div>
        </div>
        <div class="mainline">
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <div class="boxmodulo">
                        <asp:Literal ID="ltr_address" runat="server"></asp:Literal>
                        <br />
                        <asp:PlaceHolder ID="PH_chk" runat="server">
                            Visualizzazione della mappa:
                            <asp:CheckBox ID="chk_is_google_map" runat="server" />
                        </asp:PlaceHolder>
                        <br />
                        <br />
                        <br />
                        <div id="mapCont" style="width: 500px; height: 400px;">
                        </div>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
        </div>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                var map = null;
                var directionsDisplay = null;
                var directionsService = null;
                var overlay;

                function initMap() {
                    geocoder = new google.maps.Geocoder();
                    if ($("#<%=HF_coord.ClientID %>").val() != "") {
                    setMap();
            }
            else {
                geocoder.geocode({ 'address': " <%= ltr_address.Text%>" }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        var pos = results[0].geometry.location;
                        currLat = pos.lat();
                        currLng = pos.lng();                       
                        $("#<%=HF_coord.ClientID %>").val(currLat + "|" + currLng);
                        setMap();
                    }
                    else {
                        var addr = "<%= ltr_address.Text%>";
                        if (addr.indexOf(",") > 0) {
                            addr = addr.substr(addr.indexOf(",") + 1);
                            geocoder.geocode({ 'address': addr }, function (results, status) {
                                if (status == google.maps.GeocoderStatus.OK) {
                                    var pos = results[0].geometry.location;
                                    currLat = pos.lat();
                                    currLng = pos.lng();
                                    $("#<%=HF_coord.ClientID %>").val(currLat + "|" + currLng);
                                    setMap();
                                    
                                }
                                else {
                                    //alert("status");
                                }
                            });
                        }
                    }
                });
            }
                     //map.setCenter(new google.maps.LatLng(currLat, currLng));
        }

                function setMap() {

                    var _IconImage = new google.maps.MarkerImage("/images/google_maps/google_icon_logo.png", new google.maps.Size(82, 31), new google.maps.Point(0, 0), new google.maps.Point(0, 31));
                    var _IconShadow = new google.maps.MarkerImage("/images/google_maps/google_icon_shadow.png", new google.maps.Size(82, 31), new google.maps.Point(0, 0), new google.maps.Point(0, 31));
                    var _IconShape = {
                        coord: [1, 1, 1, 31, 82, 31, 82, 1],
                        type: 'poly'
                    };
                    var _coords = $("#<%=HF_coord.ClientID %>").val();
                    _point = new google.maps.LatLng(parseFloat(_coords.split('|')[0].replace(',', '.')), parseFloat(_coords.split('|')[1].replace(',', '.')));
                    var _center = _point;
                    map = new google.maps.Map(document.getElementById("mapCont"), { zoom: 13, mapTypeId: google.maps.MapTypeId.ROADMAP, center: _center, streetViewControl: false, mapTypeControl: false });
                    var _marker = new google.maps.Marker({
                        position: _point, map: map, draggable: true,
                        shadow: _IconShadow, icon: _IconImage, shape: _IconShape
                    });



                    overlay = new google.maps.OverlayView();
                    overlay.draw = function () { };
                    overlay.setMap(map); // 'map' is new google.maps.Map(...)
                    google.maps.event.addListener(_marker, 'dragend', function (event) {
                        $("#<%=HF_coord.ClientID %>").val(("" + event.latLng.lat()).replace('.', ',') + "|" + ("" + event.latLng.lng()).replace('.', ','));
                    });

                    }
                    function hidePointToolTip() {
                        $("#tooltip").css({ 'display': 'none' });
                        $("#tooltip .body").html("");
                    }
                    function showPointToolTip(marker) {
                        var projection = overlay.getProjection();
                        var pixel = projection.fromLatLngToContainerPixel(marker.getPosition());
                        var contPos = $("#mapCont").offset();
                        var posX = contPos.left + pixel.x;
                        var posY = contPos.top + pixel.y;
                        //alert(posX+","+posY);
                        $("#tooltip .body").html($("#" + marker.url).html());
                        $("#tooltip").css("left", "" + (posX + 10) + "px");
                        $("#tooltip").css("top", "" + (posY + 10) + "px");
                        $("#tooltip").css("display", "");
                    }
                    initMap();
            </script>
        </telerik:RadCodeBlock>
    </telerik:RadAjaxPanel>
</asp:Content>
