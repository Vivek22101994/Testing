<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_sx.ascx.cs" Inherits="RentalInRome.affiliatesarea.UC_sx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Literal ID="ltr_nameFull" runat="server" Visible="false"></asp:Literal>
<div style="display:block; clear:both; width:232px; margin:0 13px 0 9px;">
   <%= ltr_nameFull.Text%>
</div>

<!--- TESTO ALLERT DATI DA MANCANTI DA INSERIRE --->
<div class="txallert_menu" style="display:none;">
     <%=CurrentSource.getSysLangValue("lblAllertCompleteInfo")%>
</div>


<div class="menu_area_reservation_details">
    <a href="reservationList.aspx" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-contacts.gif);">Prenotazioni</a>
    <a href="login.aspx?logout=true" class="resDtlBtn" style="background-image: url(/images/css/btn-reservation-contacts.gif);">Logout</a></li>
    <a href="mailto:info@rentalinrome.com?subject=reservationarea code:<%= RentalInRome.reservationarea.resUtils.CurrentIdReservation_gl %>" class="resDtlBtn" style="background-image:url(/images/css/btn-reservation-contacts.gif);">
        <%=CurrentSource.getSysLangValue("lblContacts")%>
    </a>
</div>
<div class="nulla">
</div>
<div id="thawteseal" style="text-align: center;" title="Click to Verify
- This site chose Thawte SSL for secure e-commerce and confidential
communications.">
    <div>
        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <script type="text/javascript" src="https://seal.thawte.com/getthawteseal?host_name=rentalinrome.com&amp;size=S&amp;lang=en"></script>
        </telerik:RadCodeBlock>
    </div>
    <div>
        <a href="https://www.thawte.com/products/" target="_blank" style="color: #000000; text-decoration: none; font: bold 10px
arial,sans-serif; margin: 0px; padding: 0px;">ABOUT SSL CERTIFICATES</a></div>
</div>
