<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHLEstateDetailsTab.ascx.cs" Inherits="RentalInRome.admin.modRental.uc.ucHLEstateDetailsTab" %>
<asp:HiddenField runat="server" ID="HF_id" Visible="false" />
<asp:HiddenField runat="server" ID="HF_pidOwner" />
<table class="details_tabs" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlHoliday_main.aspx?id=<%= IdEstate %>' class="scheda" title="Home"><%= contUtils.getLabel("Home")%></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlHoliday_unit.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Unit"><%= contUtils.getLabel("Unit")%></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlHoliday_price.aspx?id=<%= IdEstate %>' class="scheda <%# getDetailClass() %>" title="Bathrooms"><%= contUtils.getLabel("Price")%></a>
        </td>

        <td style="border-bottom: 0;">
            <a href='/admin/modRental/EstateChnlHoliday_media.aspx?id=<%= IdEstate %>' class="media" title="Images"><%= contUtils.getLabel("Images")%></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='/admin/rnt_estate_details.aspx?id=<%= IdEstate %>' class="scheda" title="Torna alla Struttura"><%= contUtils.getLabel("Torna alla Struttura")%></a>
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
