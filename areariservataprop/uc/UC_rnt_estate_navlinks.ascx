<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_estate_navlinks.ascx.cs" Inherits="RentalInRome.areariservataprop.uc.UC_rnt_estate_navlinks" %>
<asp:HiddenField runat="server" ID="HF_id" Visible="false" />
<table class="ico" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="border-bottom: 0;">
            <a href='rnt_estate_point.aspx?id=<%= IdEstate %>' class="point" title="Punti d'interesse"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='rnt_estate_config.aspx?id=<%= IdEstate %>' class="config" title="Accessori"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='rnt_estate_gmap.aspx?id=<%= IdEstate %>' class="map" title="GoogleMaps"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='rnt_estate_calendar.aspx?id=<%= IdEstate %>' class="calendar" title="Disponibilità"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='<%= CurrentSource.getPagePath(""+IdEstate, "pg_estate", "1")  %>' target="_blank" class="scheda" title="Scheda"></a>
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
            <a href='manage_hotel_special_offer.aspx?id=<%= IdEstate%>' class="offerte" title="Offerte"></a>
        </td>
    </tr>
</table>
