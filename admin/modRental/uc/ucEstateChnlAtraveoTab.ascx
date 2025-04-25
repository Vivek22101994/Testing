<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEstateChnlAtraveoTab.ascx.cs" Inherits="MagaRentalCE.admin.modRental.uc.ucEstateChnlAtraveoTab" %>
<asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
<style type="text/css">

    #tabsHomeaway.tabsTop table td a, #tabsHomeaway.tabsTop table td a:visited {
        display: block;
        float: left;
        margin: 10px 0 0 10px;
        padding: 7px;
        width: auto;
    }
</style>
<table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table;">
    <tr>
        <td style="border-bottom: 0; width: 65%;">
            <a href='/admin/modRental/EstateChnlAtraveo_main.aspx?id=<%= IdEstate %>' class="scheda" title="Unit"><%= contUtils.getLabel("Details")%></a>
            <a href='/admin/modRental/EstateChnlAtraveo_features.aspx?id=<%= IdEstate %>' class="scheda" title="Features"><%= contUtils.getLabel("Features")%></a>
            <a href='/admin/modRental/EstateChnlAtraveo_price.aspx?id=<%= IdEstate %>' class="scheda" title="Price"><%= contUtils.getLabel("Price")%></a>
            <a href='/admin/modRental/EstateChnlAtraveo_fees.aspx?id=<%= IdEstate %>' class="scheda" title="Fees">Fees</a>
            <a href='/admin/modRental/EstateChnlAtraveo_media.aspx?id=<%= IdEstate %>' class="media" title="Images"><%= contUtils.getLabel("Images")%></a>

            <%if (IdEstate > 0)
              { %>

            <a href='/admin/rnt_estate_details.aspx?id=<%= IdEstate %>' class="scheda" title="Torna alla Struttura"><%= contUtils.getLabel("Torna alla Struttura")%></a>

            <%} %> 

        </td>

        <td style="border-bottom: 0; width: 35%;" align="right">
            <a style="float: right;" href='/admin/modRental/EstateChnlAtraveo_main.aspx?id=<%= IdEstate %>&updatefrommagarental=true' class="scheda" title="Copia da MagaRental" onclick="return confirm('Attenzione, tutti i dati presenti nella scheda verranno cancellati!');">Copia da MagaRental</a>

        </td>

    </tr>
</table>
<div class="nulla"></div>
