<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEstateChnlAirbnbTab.ascx.cs" Inherits="MagaRentalCE.admin.modRental.uc.ucEstateChnlAirbnbTab" %>
<asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />

<style type="text/css">
    .tabsTop.tabsChannelsTop, .tabsTop.mainairbnb {
        padding: 10px 0 5px 0;
        border-top: 1px dotted #d8d8d8;
        margin: 2px 0 10px;
    }

    span.bnblable {
        min-width: 254px;
        display: inline-block;
        margin-bottom: 10px;
    }

    .mainairbnb a {
        width: auto!important;
    }

    .tabsTop table td a, .tabsTop table td a:visited {
        display: inline-block;
    }

    .tabsTop.tabsChannelsTop, .tabsTop.mainairbnb {
        box-shadow: 0 0 5px #ccc;
        padding: 15px;
        margin-bottom: 20px;
    }

    select {
        width: 200px;
    }
</style>
<div id="tabsAirbnb" class="mainairbnb tabsTop">
    <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table;">
        <tr>
            <td><b>
                <span class="bnblable">Host:</span></b>
                <asp:Literal ID="ltr_host" runat="server" Text=" - "></asp:Literal></td>
        </tr>
        <tr>
            <td><b>
                <span class="bnblable">Airbnb ID:</span></b>
                <asp:Literal ID="ltr_airbnb_id" runat="server" Text=" - "></asp:Literal></td>
        </tr>
        <tr>
            <td><b>
                <span class="bnblable">Listing URL:</span></b>
                <asp:Literal ID="ltr_url" runat="server" Text=" - "></asp:Literal></td>
        </tr>
        <tr>
            <td><b><span class="bnblable">Current Status:</span></b>
                <asp:Literal ID="ltr_status" runat="server"></asp:Literal></td>
            <td style="border-bottom: 0; float: right;">
                <a href='/admin/rnt_estate_details.aspx?id=<%=IdEstate %>' class="scheda" title="Back to accommodation"><%= contUtils.getLabel("lblBackToAccomodation")%></a>
            </td>
        </tr>
        <tr>
            <td><b>
                <span class="bnblable">Published ?:</span></b>
                <asp:Literal ID="ltr_sync_category" runat="server" Text=" - "></asp:Literal></td>
        </tr>
        <tr>
            <td><b>
                <span class="bnblable">Property Listed/UnListed:</span></b>
                <asp:Literal ID="ltr_listed" runat="server" Text=" - "></asp:Literal></td>
        </tr>
        <tr>
            <td><b>
                <span class="bnblable">Booking Lead Time:</span></b>
                <asp:Literal ID="ltr_booking_lead_time" runat="server" Text="0"></asp:Literal></td>
        </tr>
        <tr>
            <td><b>
                <span class="bnblable">Allow Request to Book?</span></b>
                <asp:Literal ID="ltr_allow_request_to_book" runat="server" Text="No"></asp:Literal></td>
        </tr>
        <tr>
            <td><b>
                <span class="bnblable">Cancellation Policy:</span></b>
                <asp:Literal ID="ltr_cancellation_policy" runat="server" Text="No"></asp:Literal></td>
        </tr>
        <tr>
            <td><b>
                <span class="bnblable">Checkin Time:</span></b>
                <asp:Literal ID="ltr_checkin_time" runat="server" Text=" - "></asp:Literal></td>
        </tr>
         <tr>
            <td><b>
                <span class="bnblable">Checkin End Time:</span></b>
                <asp:Literal ID="ltr_checkin_end_time" runat="server" Text=" - "></asp:Literal></td>
        </tr>
        <tr>
            <td><b>
                <span class="bnblable">CheckOut Time:</span></b>
                <asp:Literal ID="ltr_checkout_time" runat="server" Text=" - "></asp:Literal></td>
        </tr>
    </table>
</div>
<div class="tabsTop mainairbnb" id="tabsAirbnb0" runat="server">
    <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table; margin-top: 10px;">
        <tr>
            <td><span class="bnblable">Host :</span>
                <asp:DropDownList ID="drp_host" runat="server">                                 
                </asp:DropDownList>
                <asp:LinkButton ID="lnk_update_host" runat="server" OnClick="lnk_update_host_Click" CssClass="btn" Style="float: right; margin-left: 20px;">Update Host</asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
<div class="tabsTop mainairbnb" id="tabsAirbnb5" runat="server">
    <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table; margin-top: 10px;">
        <tr>
            <td><span class="bnblable">Connect Existing Airbnb ID :</span>
               <asp:TextBox ID="txt_airbnb_id" runat="server"></asp:TextBox>
                <asp:LinkButton ID="lnk_update_id" runat="server" OnClick="lnk_update_id_Click" CssClass="btn" Style="float: right; margin-left: 20px;">Update Airbnb ID</asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
<div class="tabsTop mainairbnb" id="tabsAirbnb1" runat="server">
    <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table; margin-top: 10px;">
        <tr>
            <td>
                <asp:LinkButton ID="lnk_send_content" runat="server" OnClick="lnk_send_content_Click" CssClass="scheda">Send Basic Details</asp:LinkButton>
                <asp:LinkButton ID="lnk_send_description" runat="server" OnClick="lnk_send_description_Click" CssClass="scheda">Send Description</asp:LinkButton>
                <asp:LinkButton ID="lnk_send_image" runat="server" OnClick="lnk_send_image_Click" CssClass="scheda" Style="margin-left: 10px!important;">Send Images</asp:LinkButton>
                <asp:LinkButton ID="lnk_delete_imges" runat="server" OnClick="lnk_delete_imges_Click" CssClass="scheda" Style="margin-left: 10px!important;" OnClientClick="return confirm('Do you want delete images definitely?');">Delete All Images</asp:LinkButton>
                 <asp:LinkButton ID="lnk_send_availability" runat="server" OnClick="lnk_send_availability_Click" CssClass="scheda" Style="margin-left: 10px!important;">Send Availability/Inventory</asp:LinkButton>
                <asp:LinkButton ID="lnk_send_prices" runat="server" OnClick="lnk_send_prices_Click" CssClass="scheda" Style="margin-left: 10px!important;">Send Prices</asp:LinkButton>
                <asp:LinkButton ID="lnk_Send_los_prices" runat="server" OnClick="lnk_Send_los_prices_Click" CssClass="scheda" Style="margin-left: 10px!important;">Send LOS Prices</asp:LinkButton>
                <%--<a href='/admin/modRental/EstateChnlExpedia_main.aspx?id=<%= IdEstate %>' class="scheda" title="Unit">Send Basic Details</a>   --%>
            </td>
            <td style="border-bottom: 0; float: right;">
                <asp:LinkButton ID="lnk_delete_listing" runat="server" OnClick="lnk_delete_listing_Click">Delete Listing</asp:LinkButton>
            </td>

        </tr>
    </table>
</div>
<div class="tabsTop mainairbnb" id="tabsAirbnb2">
    <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table; margin-top: 10px;" id="tbl_update" runat="server">
        <tr>
            <td><span class="bnblable">Status :</span>
                <asp:DropDownList ID="drp_status" runat="server">
                    <asp:ListItem Text="Ready for review" Value="ready for review"></asp:ListItem>
                    <asp:ListItem Text="new" Value="new"></asp:ListItem>                    
                </asp:DropDownList>
                <asp:LinkButton ID="lnk_update_details" runat="server" OnClick="lnk_update_details_Click" CssClass="btn" Style="float: right; margin-left: 20px;">Update Details</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">Synchronization Category :</span>
                <asp:DropDownList ID="drp_sync_category" runat="server">
                    <asp:ListItem Text="Synchronize All" Value="sync_all"></asp:ListItem>
                    <asp:ListItem Text="Synchronize only rates and availability" Value="sync_rates_and_availability"></asp:ListItem>
                    <%--<asp:ListItem Text="sync_undecided" Value="sync_undecided"></asp:ListItem>--%>
                    <asp:ListItem Text="Stop Synchronize(Unlink listing)" Value="null"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="lnk_update_sync_category" runat="server" OnClick="lnk_update_sync_category_Click" CssClass="btn" Style="float: right; margin-left: 20px;">Update Sync Category</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">Unlist / Relist listing :</span>
                <asp:DropDownList ID="drp_unlist_listing" runat="server">
                    <asp:ListItem Text="Unlist listing" Value="false"></asp:ListItem>
                    <asp:ListItem Text="Re-list listing" Value="true"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="lnk_unlist_listng" runat="server" OnClick="lnk_unlist_listng_Click" CssClass="btn" Style="float: right; margin-left: 20px;">Unlist/Re-list listing</asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
<div class="tabsTop mainairbnb" id="tabsAirbnb3" runat="server">
    <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table; margin-top: 10px;" id="Table1" runat="server">
        <tr>
            <td><span class="bnblable">Booking Lead Time :</span>
                <asp:DropDownList ID="drp_lead_time" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">Allow Request to Book ? </span>
                <asp:DropDownList ID="drp_allow_request_to_book" runat="server">
                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:LinkButton ID="lnk_send_data" runat="server" OnClick="lnk_send_data_Click">Updte Booking Lead Time</asp:LinkButton>
            </td>
        </tr>
    </table>
</div>

<div class="tabsTop mainairbnb" id="tabsAirbnb4" runat="server">
    <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table; margin-top: 10px;" id="Table2" runat="server">
        <tr>
            <td><span class="bnblable">Cancellation Policy :</span>
                <asp:DropDownList ID="drp_cancelltion_policy" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">Checkin Start Time :</span>
                <asp:DropDownList ID="drp_checkin_start_time" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">Checkin End Time :</span>
                <asp:DropDownList ID="drp_checkin_end_time" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">CheckOut :</span>
                <asp:DropDownList ID="drp_checkout_time" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        
    </table>
    <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table; margin-top: 10px;" id="tabsAirbnb6" runat="server">
        <tr>
            <td><span class="bnblable"><b><%=contUtils.getLabel("lblMeaningOfCancelltionPolicy") %> </b></span>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">flexible : Full refund 1 day prior to arrival, except fees</span>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">moderate : Full refund 5 days prior to arrival, except fees</span>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">strict_14_with_grace_period : Full refund within 48 hours of booking if before 14 days of checkin. After, 50% refund up until 1 week prior to arrival</span>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">strict :  50% refund up until 1 week prior to arrival</span>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable">super_strict_30 :  50% refund up until 30 days prior to arrival, except fees</span>
            </td>
        </tr>
         
        
        <tr>
            <td><span class="bnblable">super_strict_60 :  50% refund up until 60 days prior to arrival, except fees</span>
            </td>
        </tr>
        <tr>
            <td><span class="bnblable"><b>Policies with a "_new" suffix are only used in Italy. They include fees in refund.</b></span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:LinkButton ID="lnk_send_cancelltion_policy" runat="server" OnClick="lnk_send_cancelltion_policy_Click">Send Cancelltion Policy/Checkin-Checkout Time</asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
