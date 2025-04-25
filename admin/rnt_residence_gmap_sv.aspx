<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_residence_gmap_sv.aspx.cs" Inherits="RentalInRome.admin.rnt_residence_gmap_sv" %>

<%@ Register Src="~/admin/uc/UC_rnt_residence_navlinks.ascx" TagName="UC_rnt_residence_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false&language=<%=CurrentLang.ABBR.Substring(0,2)%>&key=<%=CurrentAppSettings.GOOGLE_MAPS_KEY %>"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HF_type" Value="estate" runat="server" />
    <asp:HiddenField ID="HF_referer" Value="estate" runat="server" />
    <asp:HiddenField ID="HF_id" Value="0" runat="server" />
    <asp:HiddenField ID="HF_coord" runat="server" />
    <asp:HiddenField ID="HF_yaw" runat="server" />
    <asp:HiddenField ID="HF_pitch" runat="server" />
    <asp:HiddenField ID="HF_zoom" runat="server" />
    <h1 class="titolo_main">Gestione StreetView della residenza:
    <asp:Literal ID="ltr_title" runat="server"></asp:Literal></h1>
    <div id="fascia1">
        <div style="clear: both; margin: 3px 0 5px 30px;">
            <uc1:UC_rnt_residence_navlinks ID="UC_rnt_residence_navlinks1" runat="server" />
        </div>
    </div>
    <div class="nulla">
    </div>
    <div class="salvataggio">
        <div class="bottom_salva">
            <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" OnClientClick="return setValues()"><span>Salva</span></asp:LinkButton>
        </div>
        <div class="bottom_salva">
            <asp:LinkButton ID="lnk_chiudi" runat="server" OnClick="lnk_chiudi_Click"><span>Chiudi</span></asp:LinkButton>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="mainline">
        <div class="mainbox">
            <div class="top">
                <div style="float: left;">
                    <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                <div style="float: right;">
                    <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
            </div>
            <div class="center">
                <div class="boxmodulo">
                    <asp:Literal ID="ltr_address" runat="server"></asp:Literal>
                    <br />
                    <asp:PlaceHolder ID="PH_chk" runat="server">Visualizzazione StreetView:
                        <asp:CheckBox ID="chk_is_street_view" runat="server" />
                    </asp:PlaceHolder>
                    <br />
                    <br />
                    <div id="pano" style="width: 500px; height: 400px;">
                    </div>
                    <div id="map_canvas" style="width: 500px; height: 400px;">
                    </div>
                </div>
            </div>
            <div class="bottom">
                <div style="float: left;">
                    <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                <div style="float: right;">
                    <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
		var map;
		var _coords = $get("<%=HF_coord.ClientID %>").value;
		var _point = new google.maps.LatLng(parseFloat(_coords.split('|')[0].replace(',', '.')), parseFloat(_coords.split('|')[1].replace(',', '.')));
		var sv = new google.maps.StreetViewService();
		var marker = new google.maps.Marker({
			position: _point,
			title: ""
		});

		var panorama;

		function initialize() {

			panorama = new google.maps.StreetViewPanorama(document.getElementById("pano"));

			// Set up the map
			var mapOptions = {
				center: _point,
				zoom: 16,
				mapTypeId: google.maps.MapTypeId.ROADMAP,
				streetViewControl: false
			};
			map = new google.maps.Map(document.getElementById('map_canvas'),
				mapOptions);
			marker.setMap(map);  
			// getPanoramaByLocation will return the nearest pano when the
			// given radius is 50 meters or less.
			google.maps.event.addListener(map, 'click', function(event) {
				sv.getPanoramaByLocation(event.latLng, 50, processSVData);
			});
			google.maps.event.addListener(panorama, 'pov_changed', function() {
				var headingCell = document.getElementById('<%=HF_yaw.ClientID %>');
				var pitchCell = document.getElementById('<%=HF_pitch.ClientID %>');
				var zoomCell = document.getElementById('<%=HF_zoom.ClientID %>');
				headingCell.value = panorama.getPov().heading;
				pitchCell.value = panorama.getPov().pitch;
				zoomCell.value = panorama.getPov().zoom;
			});
		}

		function processSVData(data, status) {
			if (status == google.maps.StreetViewStatus.OK) {
				marker.setPosition(data.location.latLng);
				google.maps.event.addListener(marker, 'click', function() {

					var markerPanoID = data.location.pano;
					// Set the Pano to use the passed panoID
					panorama.setPano(markerPanoID);
					panorama.setPov({
						heading: 270,
						pitch: 0,
						zoom: 1
					});
					document.getElementById('<%=HF_coord.ClientID %>').value=panorama.getPosition().lat() + "|" + panorama.getPosition().lng();
					panorama.setVisible(true);
				});
			}
		}
		initialize();
	function setValues()
	{
		var headingCell = document.getElementById('<%=HF_yaw.ClientID %>');
		var pitchCell = document.getElementById('<%=HF_pitch.ClientID %>');
		var zoomCell = document.getElementById('<%=HF_zoom.ClientID %>');
		var coords = document.getElementById('<%=HF_coord.ClientID %>');
		headingCell.value = panorama.getPov().heading;
		pitchCell.value = panorama.getPov().pitch;
		zoomCell.value = panorama.getPov().zoom;
		coords.value = panorama.getPosition().lat() + "|" + panorama.getPosition().lng();
		return true;
	}
		<asp:PlaceHolder ID="PH_sc_setInitialPov" runat="server">
		function setInitialPov() {
			panorama.setPosition(_point);
			panorama.setPov({
				heading: <%=HF_yaw.Value %>,
				pitch: <%=HF_pitch.Value %>,
				zoom: <%=HF_zoom.Value %>
			});
			panorama.setVisible(true);
		}
		</asp:PlaceHolder>
		<asp:Literal ID="ltr_sc_setInitialPov" runat="server"></asp:Literal>
    </script>

    <a href="javascript:setInitialPov()" style="display: none;">ss</a>
</asp:Content>
