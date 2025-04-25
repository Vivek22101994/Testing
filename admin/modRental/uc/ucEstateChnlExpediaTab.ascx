<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEstateChnlExpediaTab.ascx.cs" Inherits="MagaRentalCE.admin.modRental.uc.ucEstateChnlExpediaTab" %>
<asp:HiddenField ID="HF_RoomTypeId" Value="" runat="server" />
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
            <a href='/admin/modRental/EstateChnlExpedia_main.aspx?id=<%= IdEstate %>' class="scheda" title="Unit"><%= contUtils.getLabel("Details")%></a>
            <%if (RoomTypeId != "")
              { %>
            <a href='/admin/modRental/EstateChnlExpedia_ratePlans.aspx?id=<%= IdEstate %>' class="scheda" title="RatePlans">RatePlans</a>
            <a href='/admin/modRental/EstateChnlExpedia_price.aspx?id=<%= IdEstate %>' class="scheda" title="Price">Price</a>
            <a href='/admin/modRental/EstateChnlExpedia_rates.aspx?id=<%= IdEstate %>' class="scheda" title="Price">Expedia Prices</a>
            <%} %>

            <%if (IdEstate > 0)
              { %>

            <a href='/admin/rnt_estate_details.aspx?id=<%= IdEstate %>' class="scheda" title="Torna alla Struttura"><%= contUtils.getLabel("Torna alla Struttura")%></a>

            <%} %> 

        </td>

        <td style="border-bottom: 0; width: 35%;" align="right">
            <%if (RoomTypeId != "")
              { %>
            <a style="float: right;" href='/admin/modRental/EstateChnlExpedia_main.aspx?id=<%= IdEstate %>&unset=true' class="scheda" title="Rimuovi" onclick="return confirm('Sei sicuro di voler Rimuovere Abbinamento?')">Rimuovi abbinamento</a>
            <asp:LinkButton ID="lnk_send_availablity" runat="server" OnClick="lnk_send_availablity_Click" Style="float: right;">Invia disponibilità</asp:LinkButton>
            <asp:LinkButton ID="lnk_send_price" runat="server" OnClick="lnk_send_price_Click" Style="float: right;">Invia prezzi</asp:LinkButton>
            <%} %>
            <%if (RoomTypeId == "")
              { %>
            <a style="float: right;" href='/admin/modRental/ChnlExpediaRoomTypeList.aspx?for=<%= IdEstate%>' class="scheda" title="Abbina">Abbina ad una struttura esistente</a>
            <%} %>

        </td>

    </tr>
    <tr>
        <td style="border-bottom: 0; width: 65%;">
            <asp:LinkButton ID="lnk_send_content" runat="server" OnClick="lnk_send_content_Click">Send Content</asp:LinkButton>
            <asp:LinkButton ID="lnk_send_status" runat="server" OnClick="lnk_send_status_Click">Get Status</asp:LinkButton>
            <asp:LinkButton ID="lnk_create_room" runat="server" OnClick ="lnk_create_room_Click" Text="Create Room" style="display:none;"></asp:LinkButton>
            <asp:LinkButton ID="lnk_manage_room" runat="server" OnClick ="lnk_manage_room_Click" Text="Manage Rooms" style="display:none;"></asp:LinkButton>
            <asp:LinkButton ID="lnk_send_room_amenity" runat="server" OnClick ="lnk_send_room_amenity_Click" Text="Send Room Amenity" style="display:none;"></asp:LinkButton>
            <asp:LinkButton ID="lnk_manage_ratePlans" runat="server" OnClick ="lnk_manage_ratePlans_Click" Text="Manage RatePlans" style="display:none;"></asp:LinkButton>
             <%--<a href='/admin/modRental/EstateChnlExpedia_room.aspx?id=<%= IdEstate %>' class="scheda" title="Price" id="lnk_rooms">Manage Rooms</a>
            <a href='/admin/modRental/EstateChnlExpedia_manageRateplans.aspx?id=<%= IdEstate %>' class="scheda" title="Price" id="lnk_ratePlans">Manage RatePlans</a>--%>
        </td>
    </tr>
</table>
<div class="nulla"></div>
