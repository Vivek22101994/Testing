<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlExpedia_manageRateplans.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlExpedia_manageRateplans" %>

<%@ Register Src="~/admin/modRental/uc/ucEstateChnlExpediaTab.ascx" TagName="ucNav" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
            <%# rntUtils.getAgent_logoForDetailsPage(ChnlExpediaProps.IdAdMedia) %>
            <h1 class="titolo_main">Details of Property for Expedia:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop" id="tabsHomeaway">
                    <uc1:ucNav ID="ucNav" runat="server" />
                </div>
            </div>
            <div class="nulla">
            </div>
            <style type="text/css">
                .boxmodulo input[type="text"], .boxmodulo select {
                    height: 30px;
                    box-sizing: border-box;
                    width: 100%;
                }
            </style>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" ValidationGroup="dati" OnClick="lnk_saveOnly_Click"><span>Create RatePlan</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_list" runat="server" ValidationGroup="dati" OnClick="lnk_list_Click"><span>Go to list</span></asp:LinkButton>
                </div>
                <%--<div class="bottom_salva">
                    <asp:LinkButton ID="lnk_update_room" runat="server" ValidationGroup="dati" OnClick="lnk_update_room_Click"><span>Update Rooms</span></asp:LinkButton>
                </div>--%>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>

            <div class="mainline mainChnl mainExpedia" id="pnlError" runat="server" visible="false">
                <div class="mainbox">
                    <div class="top">
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <asp:Literal ID="ltrErorr" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="bottom">
                    </div>
                </div>
            </div>

            <div class="mainline mainChnl mainExpedia" id="pnlRoomTypeDetails" runat="server">
                <div class="mainbox">
                    <div class="top">
                    </div>
                    <div class="center">
                        <h1 class="titolo_main" style="margin-top: 20px;">Add RatePlan</h1>
                        <div class="boxmodulo">

                            <table class="tableBoxBooking" cellpadding="0" cellspacing="10" style="width: 60%; min-width: 700px;">
                                <tr>
                                    <td valign="middle" align="left">Name </td>
                                    <td valign="middle" align="right" colspan="3">
                                        <asp:TextBox ID="txt_name" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left">RateAcquisitionType </td>
                                    <td valign="middle" align="right" colspan="3">
                                        <asp:DropDownList ID="drp_rateAcquisitionType" runat="server">
                                            <asp:ListItem Text="NetRate" Value="NetRate"></asp:ListItem>
                                            <asp:ListItem Text="SellLAR" Value="SellLAR"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left">DistributionRules </td>
                                    <td valign="middle" align="right" colspan="3">
                                        <div style="margin-bottom: 15px;">
                                            Hotel Collect                                      
                                        <asp:CheckBox ID="chk_hotelCollect" runat="server" />
                                        </div>
                                        Hotel Collect Partner Code
                                        <asp:TextBox ID="txt_hotellCollect" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <div style="margin-bottom: 15px;">
                                            Expedia Collect 
                                        <asp:CheckBox ID="chk_expediaCollect" runat="server" />
                                        </div>
                                        Expedia Collect Partner Code
                                        <asp:TextBox ID="txt_expediaCollect" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left">Status </td>
                                    <td valign="middle" align="right" colspan="3">
                                        <asp:DropDownList ID="drp_status" runat="server">
                                            <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left">Pricing Model </td>
                                    <td valign="middle" align="right" colspan="3">
                                        <asp:DropDownList ID="drp_pricingModel" runat="server">
                                            <asp:ListItem Text="PerDayPricingByLengthOfStay" Value="PerDayPricingByLengthOfStay"></asp:ListItem>
                                            <asp:ListItem Text="PerDayPricing" Value="PerDayPricing"></asp:ListItem>
                                            <asp:ListItem Text="PerDayPricingByDayOfArrival" Value="PerDayPricingByDayOfArrival"></asp:ListItem>
                                            <asp:ListItem Text="OccupancyBasedPricing" Value="OccupancyBasedPricing"></asp:ListItem>
                                            <asp:ListItem Text="OccupancyBasedPricingByDayOfArrival" Value="OccupancyBasedPricingByDayOfArrival"></asp:ListItem>
                                            <asp:ListItem Text="OccupancyBasedPricingByLengthOfStay" Value="OccupancyBasedPricingByLengthOfStay"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left">Type </td>
                                    <td valign="middle" align="right" colspan="3">
                                        <asp:DropDownList ID="drp_type" runat="server">
                                            <asp:ListItem Text="Standalone" Value="Standalone"></asp:ListItem>
                                            <asp:ListItem Text="Package" Value="Package"></asp:ListItem>
                                            <asp:ListItem Text="Corporate" Value="Corporate"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Cancellation Policy
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left">Deadline Hours</td>
                                    <td valign="middle" align="right" colspan="3">
                                        <asp:DropDownList ID="drp_deadline" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td valign="middle" align="left">Stay Fee</td>
                                    <td valign="middle" align="right" colspan="3">
                                        <asp:DropDownList ID="drp_stay_fee" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>

                </div>
            </div>
            </div>
            </div>
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
