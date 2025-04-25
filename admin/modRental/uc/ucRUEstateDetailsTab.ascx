<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRUEstateDetailsTab.ascx.cs" Inherits="RentalInRome.admin.modRental.uc.ucRUEstateDetailsTab" %>
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

    div.salvataggio {
        float: left;
    }

        div.salvataggio.btnCopia {
            float: right;
            width: auto;
        }

    .homeAwayLogo.RUlogo {
        background: #fff none repeat scroll 0 0;
        padding: 10px;
    }
</style>
<table class="details_tabs" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlRU_listing.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Listing">Listing</a>

            <a href='/admin/modRental/EstateChnlRU_adContent.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Ad content">AdContent</a>

            <a href='/admin/modRental/EstateChnlRU_unit.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Unit">Unit</a>

             <a href='/admin/modRental/EstateChnlRU_price.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Price">Price</a>

            <a href='/admin/modRental/EstateChnlRU_bathrooms.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Bathrooms">Bathrooms</a>

            <a href='/admin/modRental/EstateChnlRU_bedrooms.aspx?id=<%= IdEstate %>' class="scheda" title="Bedrooms">Bedrooms</a>

            <a href='/admin/modRental/EstateChnlRU_location.aspx?id=<%= IdEstate %>' class="scheda" title="Location">Location</a>

            <%-- <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_point.aspx?id=<%= IdEstate %>' class="scheda" title="Rates"><%= contUtils.getLabel("Rates")%></a>
            --%>


            <a href='/admin/modRental/EstateChnlRU_media.aspx?id=<%= IdEstate %>' class="media" title="Images">Images</a>

            <a href='/admin/modRental/EstateChnlRU_cancallationPolicy.aspx?id=<%= IdEstate %>' class="media" title="Cancellation Policy">Cancellation Policy</a>

            <a href='/admin/rnt_estate_details.aspx?id=<%= IdEstate %>' class="scheda" title="Torna alla Struttura"><%= contUtils.getLabel("lblBackToAccomodation")%></a>
        </td>

        <td style="border-bottom: 0; width: 1%;" align="right">
            <a style="float: right;" href='/admin/modRental/EstateChnlRU_unit.aspx?id=<%= IdEstate %>&updatefrommagarental=true' class="scheda" title="Copia da MagaRental" onclick="return confirm('Warning, all the data in this page will be deleted!');">Copy from MagaRental</a>

        </td>
    </tr>
</table>
