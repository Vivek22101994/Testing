<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_estate_navlinks.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_estate_navlinks" %>
<asp:HiddenField runat="server" ID="HF_id" Visible="false" />
<table class="ico" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_calendar.aspx?id=<%= IdEstate %>' class="calendar" title="Disponibilità del Proprietario"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_service.aspx?id=<%= IdEstate %>' class="service" title="Servizi Ecopulizie e Srs"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_point.aspx?id=<%= IdEstate %>' class="point" title="Punti d'interesse"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_config.aspx?id=<%= IdEstate %>' class="config" title="Accessori"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateMedia.aspx?id=<%= IdEstate %>' class="media" title="Multimedia"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_gmap.aspx?id=<%= IdEstate %>' class="map" title="GoogleMaps"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_price.aspx?id=<%= IdEstate %>' class="prezzi" title="Prezzi-Periodi"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_interns.aspx?id=<%= IdEstate %>' class="interns" title="Interns"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_details.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Scheda"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_text.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="text"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_details_save.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Scheda Min. Stay"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateMedia.aspx?id=<%= IdEstate %>&type=homeaway' class="media" title="HomeAway Multimedia"></a>
        </td>
        <%
            using (ModRental.DCmodRental dc = new ModRental.DCmodRental())
            {
                if (dc.dbRntChannelManagerTBLs.Where(x => x.isActive == 1).Count() > 0)
                { %>
        <td style="border-bottom: 0;">
            <a href="/admin/modRental/EstateDett_iCal.aspx?id=<%= IdEstate %>" class="ical" title="iCal"></a>
        </td>
        <%}
            }
        %>
        <td style="border-bottom: 0;">
            <a href="/admin/modRental/EstateDett_iCalImport.aspx?id=<%= IdEstate %>" class="ical icalimport" title="Importazione da iCal"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateDett_bcom.aspx?id=<%= IdEstate %>' class="bcom" title="Impostazioni Booking.com"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlHoliday_unit.aspx?id=<%= IdEstate %>' class="hl" title="Impostazioni HolidayLettings"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlHA_unit.aspx?id=<%= IdEstate %>' class="ha" title="Impostazioni HomeAway"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlAtraveo_main.aspx?id=<%= IdEstate %>' class="atraveo" title="Impostazioni Atraveo"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlExpedia_main.aspx?id=<%= IdEstate %>' class="expedia" title="Impostazioni Expedia"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlRU_unit.aspx?id=<%= IdEstate %>' class="ru" title="Impostazioni RentalsUnited"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlMagaRentalXml_main.aspx?id=<%= IdEstate %>' class="mg" title="Impostazioni MagarentlXML"></a>
        </td>
    </tr>
</table>
<table id="Table1" class="ico" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
    <tr>
        <td style="border-bottom: 0;">
            <asp:LinkButton ID="ibtn_delete" runat="server" CssClass="elimina" ToolTip="Elimina" OnClientClick="return confirm('Attenzione!\nSta per eliminare Struttura?\nSenza possibilità di ripristinare!')">
            </asp:LinkButton>
        </td>
        <td>
            <a href='manage_hotel_special_offer.aspx?id=<%= IdEstate %>' class="offerte" title="Offerte"></a>
        </td>
    </tr>
</table>
