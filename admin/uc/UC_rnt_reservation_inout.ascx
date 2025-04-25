<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_inout.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_inout" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_isChanged" runat="server" />
<asp:HiddenField ID="HF_IdReservation" runat="server" />
<asp:HiddenField ID="HF_uid" runat="server" />
<asp:HiddenField ID="HF_dtEnd" runat="server" />
<asp:HiddenField ID="HF_dtEndTime" runat="server" />
<asp:HiddenField ID="HF_limo_in_datetime" runat="server" />
<asp:HiddenField ID="HF_limo_out_datetime" runat="server" />

<asp:HiddenField ID="HF_dtIn" runat="server" />
<asp:HiddenField ID="HF_dtOut" runat="server" />
<asp:HiddenField ID="HF_in_pointType" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_in_transportType" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_in_pickupPlace" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_in_pickupPlaceName" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_in_pickupDetails" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_in_pickupDetailsType" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_out_pointType" runat="server" Value="" />
<asp:HiddenField ID="HF_out_transportType" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_out_pickupPlace" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_out_pickupPlaceName" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_out_pickupDetails" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_out_pickupDetailsType" runat="server" Value="" Visible="false" />
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <h3>
            Dati Arrivo / Partenza
        </h3>
        <a href="<%=App.HOST_SSL %>/reservationarea/arrivaldeparture.aspx?usr=<%=UserAuthentication.CurrentUserID %>&authtmp=<%= HF_uid.Value %>" target="_blank" class="changeapt topright">Cambia</a>
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click" Visible="false">Cambia</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px; float: left; margin-right: 30px;">
                <tr>
                    <td colspan="2">
                        <strong>Arrivo in città</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        Punto di arrivo
                    </td>
                    <td>
                        <%= in_pointType + "&nbsp;" + in_pickupPlaceName%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Dettagli
                    </td>
                    <td>
                        <%= in_pickupDetails%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Orario Arrivo a Roma
                    </td>
                    <td>
                        <%= limo_in_datetime!=null? limo_in_datetime.Value.TimeOfDay.JSTime_toString(false, true) + " il "+ limo_in_datetime.formatCustom("#dd# #MM# #yy#", 1, ""): " -non specificato- "%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Orario checkin al Apt
                    </td>
                    <td>
                        <%= dtIn != null ? dtIn.Value.TimeOfDay.JSTime_toString(false, true) + " il " + dtIn.formatCustom("#dd# #MM# #yy#", 1, "") : " -non specificato- "%>
                    </td>
                </tr>
            </table>
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px; float: left;">
                <tr>
                    <td colspan="2">
                        <strong>Partenza da città</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        Punto di partenza
                    </td>
                    <td>
                        <%= out_pointType + "&nbsp;" + out_pickupPlaceName%> 
                    </td>
                </tr>
                <tr>
                    <td>
                        Dettagli
                    </td>
                    <td>
                        <%= out_pickupDetails%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Orario di partenza
                    </td>
                    <td>
                        <%= limo_out_datetime != null ? limo_out_datetime.Value.TimeOfDay.JSTime_toString(false, true) + " il " + limo_out_datetime.formatCustom("#dd# #MM# #yy#", 1, "") : " -non specificato- "%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Orario checkout dal Apt
                    </td>
                    <td>
                        <%= dtOut != null ? dtOut.Value.TimeOfDay.JSTime_toString(false, true) + " il " + dtOut.formatCustom("#dd# #MM# #yy#", 1, "") : " -non specificato- "%>
                    </td>
                </tr>
            </table>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Cambia dati Arrivo / Partenza</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            In fase di sviluppo
        </div>
    </asp:Panel>
    <div class="nulla">
    </div>
    <div style="margin-top: 15px;">
        <h3 onclick="loadIFrameEasyShuttle_<%= Unique %>()" style="cursor: pointer;">
            Transfer EasyShuttle
        </h3>
        <div class="price_div" id="iframeEasyShuttleCont_<%= Unique %>" style="display: none;">
            
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
    <div style="margin-top: 15px;">
        <h3 onclick="$('#iframeSrsCont_<%= Unique %>').toggle();" style="cursor: pointer;">
            Accoglienza Srs
        </h3>
        <div class="price_div" id="iframeSrsCont_<%= Unique %>" style="display: none;">
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
</div>
<script type="text/javascript">
    function loadIFrameEasyShuttle_<%= Unique %>() {
        var iFrameSrc = 'https://www.romeasyshuttle.com/webwervice/reservationEvents.aspx?login=rentalinrome&password=rome2012&eventid=<%= IdReservation %>';
        window.open(iFrameSrc);

        var iFrameHtml = '<iframe border="0" style="width: 700px; height: auto; border: none;" src="' + iFrameSrc + '"></iframe>';
        $("#iframeEasyShuttleCont_<%= Unique %>").html(iFrameHtml);
    }
    function loadIFrameSrs_<%= Unique %>() {
        //var iFrameSrc = 'https://www.shortrentalsolution.com/webservice/reservationEvents.aspx?ref_id=1&eventid=<%= IdReservation %>';
        var iFrameSrc = 'https://www.shortrentalsolution.com/webservice/reservationEvents.aspx?ref_id=1&eventid=<%= IdReservation %>';
        var iFrameHtml = '<iframe border="0" style="width: 700px; height: auto; border: none;" src="' + iFrameSrc + '"></iframe>';
        $("#iframeSrsCont_<%= Unique %>").html(iFrameHtml);
    }
    $(document).ready(function () {
        $(window).load(function () {
            loadIFrameEasyShuttle_<%= Unique %>();
            loadIFrameSrs_<%= Unique %>();
        });
    });
</script>
