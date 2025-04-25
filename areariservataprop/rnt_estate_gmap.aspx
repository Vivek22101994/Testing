<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/areariservataprop/common/MP.Master" AutoEventWireup="true" CodeBehind="rnt_estate_gmap.aspx.cs" Inherits="RentalInRome.areariservataprop.rnt_estate_gmap" %>

<%@ Register Src="~/areariservataprop/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<%@ Register Assembly="GMaps" Namespace="Subgurim.Controles" TagPrefix="cc1" %>
<%@ Register Src="~/areariservataprop/uc/UC_contactStaff.ascx" TagName="UC_contactStaff" TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">

	<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false&language=<%=CurrentLang.ABBR.Substring(0,2)%>&key=<%=CurrentAppSettings.GOOGLE_MAPS_KEY %>"></script>

</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
	<asp:HiddenField ID="HF_coord" runat="server" />
    <asp:HiddenField ID="HF_type" Value="estate" runat="server" />
    <asp:HiddenField ID="HF_referer" Value="estate" runat="server" />
    <asp:HiddenField ID="HF_id" Value="0" runat="server" />
    <h1 class="titolo_main" style="color: #FE6634;">Gestione Google Maps della struttura:<asp:Literal ID="ltr_title" runat="server"></asp:Literal></h1>
    <div id="fascia1">
        <div style="clear: both; margin: 3px 0 5px 30px;">
            <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
        </div>
    </div>
    <div class="nulla"></div>
    <uc1:UC_contactStaff ID="UC_contactStaff" runat="server" />
    <div class="mainline">
        <div class="mainbox">
            <div class="top">
            	<div style="float:left;"><img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
            	<div style="float:right;"><img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
            </div>
            <div class="center">
                <div class="boxmodulo"> 
                    <asp:Literal ID="ltr_address" runat="server"></asp:Literal>  
                    <br/>
					<asp:PlaceHolder ID="PH_chk" runat="server">
						Visualizzazione della mappa:
						<asp:CheckBox ID="chk_is_google_map" runat="server" />
                    </asp:PlaceHolder> 
                    <br />
                     
                    <br/>  
                    <br/>  
                    <cc1:GMap ID="GMap1" Width="700" Height="400" serverEventsType="AspNetPostBack" enableServerEvents="True" OnServerEvent="GMap1_ServerEvent"  runat="server" />
                </div>
            </div>
            <div class="bottom">
    		        <div style="float:left;"><img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
    		        <div style="float:right;"><img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
   	        </div>
        </div>
    </div>  
</asp:content>
