<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_residence_navlinks.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_residence_navlinks" %>
<asp:HiddenField runat="server" ID="HF_id" Value="0" Visible="false" />
<table class="ico" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="border-bottom: 0;">
            <a href='rnt_residence_config.aspx?id=<%= IdResidence %>' class="config" title="Accessori"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='rnt_residence_media.aspx?id=<%= IdResidence %>' class="media" title="Multimedia"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='rnt_residence_gmap.aspx?id=<%= IdResidence %>' class="map" title="GoogleMaps"></a>
        </td>
        <td style="border-bottom: 0;">
            <a href='rnt_residence_details.aspx?id=<%= IdResidence %>' class="scheda" title="Scheda"></a>
        </td>
    </tr>
</table>
