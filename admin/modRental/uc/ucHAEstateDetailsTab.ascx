<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHAEstateDetailsTab.ascx.cs" Inherits="RentalInRome.admin.modRental.uc.ucHAEstateDetailsTab" %>
<asp:HiddenField runat="server" ID="HF_id" Visible="false" />
<asp:HiddenField runat="server" ID="HF_main_id" />
<asp:HiddenField runat="server" ID="HF_pidOwner" />
<style type="text/css">

    #tabsHomeaway.tabsTop table td a, #tabsHomeaway.tabsTop table td a:visited {
        display: block;
        float: left;
        margin: 10px 0 0 10px;
        padding: 7px;
        width: auto;
    }
</style>
<table class="details_tabs" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlHA_listing.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Listing">Listing</a>
        
            <a href='/admin/modRental/EstateChnlHA_adContent.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Ad content">AdContent</a>
       
            <a href='/admin/modRental/EstateChnlHA_unit.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Unit">Unit</a>
        
            <a href='/admin/modRental/EstateChnlHA_bathrooms.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Bathrooms">Bathrooms</a>
        
            <a href='/admin/modRental/EstateChnlHA_bedrooms.aspx?id=<%= IdEstate %>' class="scheda" title="Bedrooms">Bedrooms</a>
        
            <a href='/admin/modRental/EstateChnlHA_location.aspx?id=<%= IdEstate %>' class="scheda" title="Location">Location</a>
            <a href='/admin/modRental/EstateChnlHA_media.aspx?id=<%= IdEstate %>' class="media" title="Images">Images</a>
            <a href='/admin/modRental/EstateChnlHA_price.aspx?id=<%= IdEstate %>' class="media" title="Images">Price</a>
       
            <a href='/admin/modRental/EstateChlnHA_cancallationPolicy.aspx?id=<%= IdEstate %>' class="media" title="Cancellation Policy">Cancellation Policy</a>
       
            <a href='/admin/rnt_estate_details.aspx?id=<%= IdEstate %>' class="scheda" title="Torna alla Struttura">Torna alla Struttura</a>
            <a style="float: right;" href='/admin/modRental/EstateChnlHA_unit.aspx?id=<%= IdEstate %>&updatefrommagarental=true' class="scheda" title="Copia da MagaRental" onclick="return confirm('Attenzione, tutti i dati presenti nella scheda verranno cancellati!');">Copia da MagaRental</a>
    </tr>
</table>