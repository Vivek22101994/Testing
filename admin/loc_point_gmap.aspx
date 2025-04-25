<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="loc_point_gmap.aspx.cs" Inherits="RentalInRome.admin.loc_point_gmap" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?v=3.1&sensor=false&language=it"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HF_id" Value="0" runat="server" />
    <asp:HiddenField ID="HF_title" Value="" runat="server" />
    <asp:HiddenField ID="HF_isNew" Value="1" runat="server" />
    <asp:HiddenField ID="HF_gmaps_coords" runat="server" />
    <asp:HiddenField ID="HF_address" runat="server" />
    <h1 class="titolo_main">Mappa del Punto d'interesse:
    <%= HF_title.Value%>
    </h1>
    <div class="salvataggio">
        <div class="bottom_salva">
            <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
        </div>
        <div class="bottom_salva">
            <a href="loc_point_details.aspx?id=<%=HF_id.Value %>">
                <span>Torna nella scheda</span></a>
        </div>
        <div class="bottom_salva">
            <a href="<%=listPage %>">
                <span>Torna nel elenco</span></a>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
    <div class="mainline">
        <div class="mainbox">
            <div class="top">
                <div style="float: left;">
                    <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                </div>
                <div style="float: right;">
                    <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                </div>
            </div>
            <div class="center">
                <div class="boxmodulo">
                    Vissualizza nella mappa?
                    <asp:CheckBox ID="chk_gmaps_available" runat="server" />
                    <br />
                    <br />
                    <br />
                    <div id="mapCont" style="width: 700px; height: 400px;">
                    </div>
                </div>
            </div>
            <div class="bottom">
                <div style="float: left;">
                    <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                </div>
                <div style="float: right;">
                    <img src="images/mainbox4.gif" width="10" height="10" alt="" />
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
                if ($("#<%=HF_gmaps_coords.ClientID %>").val() != "") {
                    setMap();
                }
                else {
                    geocoder.geocode({ 'address': " <%= HF_address.Value%>" }, function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {
                            var pos = results[0].geometry.location;
                            currLat = pos.lat();
                            currLng = pos.lng();
                            $("#<%=HF_gmaps_coords.ClientID %>").val(currLat + "|" + currLng);
                                setMap();
                            }
                            else {
                                var addr = "<%= HF_address.Value%>";
                                if (addr.indexOf(",") > 0) {
                                    addr = addr.substr(addr.indexOf(",") + 1);
                                    geocoder.geocode({ 'address': addr }, function (results, status) {
                                        if (status == google.maps.GeocoderStatus.OK) {
                                            var pos = results[0].geometry.location;
                                            currLat = pos.lat();
                                            currLng = pos.lng();
                                            $("#<%=HF_gmaps_coords.ClientID %>").val(currLat + "|" + currLng);
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

                    var _coords = $("#<%=HF_gmaps_coords.ClientID %>").val();                   
                    _point = new google.maps.LatLng(parseFloat(_coords.split('|')[0].replace(',', '.')), parseFloat(_coords.split('|')[1].replace(',', '.')));
                    var _center = _point;
                    map = new google.maps.Map(document.getElementById("mapCont"), { zoom: 15, mapTypeId: google.maps.MapTypeId.ROADMAP, center: _center, streetViewControl: false, mapTypeControl: false });
                    var _marker = new google.maps.Marker({
                        position: _point, map: map, draggable: true,
                        shadow: _IconShadow, icon: _IconImage, shape: _IconShape
                    });



                    overlay = new google.maps.OverlayView();
                    overlay.draw = function () { };
                    overlay.setMap(map); // 'map' is new google.maps.Map(...)
                    google.maps.event.addListener(_marker, 'dragend', function (event) {
                        $("#<%=HF_gmaps_coords.ClientID %>").val(("" + event.latLng.lat()).replace('.', ',') + "|" + ("" + event.latLng.lng()).replace('.', ','));
                       
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
</asp:Content>
