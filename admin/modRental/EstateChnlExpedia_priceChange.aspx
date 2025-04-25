<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlExpedia_priceChange.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlExpedia_priceChange" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucEstateChnlExpediaTab.ascx" TagName="ucNav" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .main div.mainbox {
            width: 100%;
            margin-bottom: 40px;
        }

        .ui-datepicker td {
            padding: 1px !important;
        }

        .RadPicker td {
            padding: 0 !important;
        }

        .ui-datepicker .rntCal {
            margin: 0 !important;
            width: 22px !important;
        }

        .mainbox.iCalMainBox table tr td:first-child {
            padding: 10px;
        }

        .mainbox.iCalMainBox table tr td input {
            margin: 0;
        }

        /*.mainBookingCom .mainbox.iCalMainBox .boxmodulo > table {
                    width: 1200px;
                }*/

        .boxmodulo table tr td input {
            float: left;
        }
    </style>
    <script type="text/javascript">
        function toogleRateChnageRatePlnas(obj) {
            $('.rateChange input').each(function () { this.checked = obj.checked });

        }
        function toogleCloseChangeRatePlnas(obj) {
            $('.closeChange input').each(function () { this.checked = obj.checked });

        }

    </script>
    <asp:HiddenField ID="HF_RoomTypeId" Value="" runat="server" />
    <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
   
    <%# rntUtils.getAgent_logoForDetailsPage(ChnlExpediaProps.IdAdMedia) %>
    <h1 class="titolo_main">Special Price Change of Property for Expedia:
        <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
    </h1>
    <div id="fascia1">
        <div class="tabsTop" id="tabsHomeaway">
            <uc1:ucNav ID="ucNav" runat="server" />
        </div>
    </div>
    <div class="nulla">
    </div>
    <div class="salvataggio">
        <div class="bottom_salva">
            <asp:LinkButton ID="lnkUpdateAvv" runat="server" OnClick="lnkUpdateAvv_Click"><span>Invia disponibilita</span></asp:LinkButton>
        </div>
        <div class="bottom_salva">
            <asp:LinkButton ID="lnkUpdateRates" runat="server" OnClick="lnkUpdateRates_Click"><span>Invia prezzi</span></asp:LinkButton>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="mainline" id="pnlNotConnected" runat="server">
        <div class="mainbox">
            <div class="top">
            </div>
            <div class="center">
                <div class="boxmodulo">
                    Attenzione! Non e abbinato con RoomType su Expedia.
                            <br />
                    <a href='/admin/modRental/ChnlExpediaRoomTypeList.aspx?for=<%= IdEstate%>'>Clicca qui per abbinare ad una struttura esistente, </a>
                </div>
            </div>
            <div class="bottom">
            </div>
        </div>
    </div>
    <div class="mainline" id="pnlError" runat="server" visible="false">
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
    <div class="mainline mainChannels mainBookingCom mainBookingComHome" id="pnlPriceChange" runat="server">
        <div class="mainbox iCalMainBox">
            <div class="center">
                <span class="titoloboxmodulo" style="margin-top: 20px;">Prezzi speciali :</span>
                <div class="boxmodulo">
                    <table>
                        <tr>
                            <td style="width: 484px;">dal:
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="rdp_rate_dtStart" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                    <DateInput DateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>al:
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="rdp_rate_dtEnd" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                    <DateInput DateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>Days:
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_day1" runat="server" Text="Monday" Style="margin-top: 5px;" />
                                <asp:CheckBox ID="chk_day2" runat="server" Text="Tuesday" Style="margin-top: 5px;" />
                                <asp:CheckBox ID="chk_day3" runat="server" Text="Wednesday" Style="margin-top: 5px;" />
                                <asp:CheckBox ID="chk_day4" runat="server" Text="Thursday" Style="margin-top: 5px;" />
                                <asp:CheckBox ID="chk_day5" runat="server" Text="Friday" Style="margin-top: 5px;" />
                                <asp:CheckBox ID="chk_day6" runat="server" Text="Saturday" Style="margin-top: 5px;" />
                                <asp:CheckBox ID="chk_day7" runat="server" Text="Sunday" Style="margin-bottom: 5px;" />
                            </td>
                        </tr>
                    </table>
                    <table id="tbl_ratePlan" runat="server" visible="false">
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text=" Check/Uncheck All RatePlans" onclick="toogleRateChnageRatePlnas(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>Mon
                            </td>
                            <td>Tues
                            </td>
                            <td>Wed
                            </td>
                            <td>Thurs
                            </td>
                            <td>Fri
                            </td>
                            <td>Sat
                            </td>
                            <td>Sun
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListView ID="LvRateChanges" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_send_price" runat="server" class="rateChange" /></td>
                                            <asp:Label ID="lbl_ratePlanId" runat="server" Visible="false" Text='<%# Eval("RatePlanId") %>'></asp:Label>
                                            <td><%# Eval("RatePlanId") %> - <%# Eval("name") %> - <%# Eval("rateAcquisitionType")%> - <%# Eval("status").objToInt32() == 1 ? "Active":"InActive"%></td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_changeAmount" runat="server" Width="50" MinValue="0" Style="text-align: right;" Value="0">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox></td>
                                            <td>
                                                <asp:DropDownList ID="drp_changeIsPercentage" runat="server" Style="height: 25px;" Width="55px">
                                                    <asp:ListItem Value="0">EUR</asp:ListItem>
                                                    <asp:ListItem Value="1">%</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_changeIsDiscount" runat="server" Style="height: 25px;" Width="100px">
                                                    <asp:ListItem Value="-1">Default</asp:ListItem>
                                                    <asp:ListItem Value="0">aumento di</asp:ListItem>
                                                    <asp:ListItem Value="1">sconto di</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay1" runat="server" TextAlign="Left" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay4" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay5" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay6" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay7" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:LinkButton ID="lnk_rate_save" runat="server" CssClass="inlinebtn" OnClick="lnk_rate_save_Click">Aggiorna</asp:LinkButton>
                            </td>
                        </tr>
                        <%-- <tr>
                                    <td class="rntCal nd_f" style="position: relative;" colspan="2">NON disponibile
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal opz_f" style="position: relative;" colspan="2">Prezzo Cambiato
                                    </td>
                                </tr>--%>
                    </table>
                    <div id="ratesCalendarCont" style="margin-left: 5px; margin-top: 20px; float: left;">
                    </div>
                </div>
            </div>
        </div>
        <div class="mainbox iCalMainBox">
            <div class="center">
                <span class="titoloboxmodulo" style="margin-top: 20px;">Closed / Restrictions :</span>
                <div class="boxmodulo">
                    <table>
                        <tr>
                            <td>dal:
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="rdp_closed_dtStart" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                    <DateInput DateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>al:
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="rdp_closed_dtEnd" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                    <DateInput DateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                    </table>
                    <table id="tbl_rateRestrictions" runat="server" visible="false">
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="CheckBox2" runat="server" Text=" Check/Uncheck All RatePlans" onclick="toogleCloseChangeRatePlnas(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="chk_is_close_ratePlan" runat="server" />
                                Rate Plan closed?</td>
                            <td>
                                <asp:CheckBox ID="chk_is_close_onArrival" runat="server" />
                                closed on Arrival ?</td>
                            <td>
                                <asp:CheckBox ID="chk_is_close_onDepartue" runat="server" />
                                closed on Departure ?</td>
                            <td>
                                <asp:CheckBox ID="chk_is_minStay" runat="server" />
                                Min Stay</td>
                            <td>
                                <asp:CheckBox ID="chk_is_maxStay" runat="server" />
                                Max Stay</td>
                            <td>Mon
                            </td>
                            <td>Tues
                            </td>
                            <td>Wed
                            </td>
                            <td>Thurs
                            </td>
                            <td>Fri
                            </td>
                            <td>Sat
                            </td>
                            <td>Sun
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListView ID="LVRateRestrictions" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_send_price" runat="server" class="closeChange" /></td>
                                            <asp:Label ID="lbl_ratePlanId" runat="server" Visible="false" Text='<%# Eval("RatePlanId") %>'></asp:Label>
                                            <td><%# Eval("RatePlanId") %> - <%# Eval("name") %> - <%# Eval("rateAcquisitionType")%> - <%# Eval("status").objToInt32() == 1 ? "Active":"InActive"%></td>

                                            <td>
                                                <asp:DropDownList ID="drp_closed_isClosed" runat="server" Style="float: right; width: 105px;">
                                                    <asp:ListItem Value="-1" Text="Default"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Chiuso"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Open"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_closedOnArrival_isClosed" runat="server" Style="float: right; width: 105px;">
                                                    <asp:ListItem Value="-1" Text="Default"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Chiuso"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Open"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_closedOnDeparture_isClosed" runat="server" Style="float: right; width: 105px;">
                                                    <asp:ListItem Value="-1" Text="Default"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Chiuso"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Open"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_closed_minStay" runat="server" Width="50" MinValue="0" Style="text-align: right;" Value="0">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_closed_maxStay" runat="server" Width="50" MinValue="0" Style="text-align: right;" Value="0">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay4" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay5" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay6" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_inDay7" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:LinkButton ID="lnk_closed_save" runat="server" CssClass="inlinebtn" OnClick="lnk_closed_save_Click">Aggiorna</asp:LinkButton>
                            </td>
                        </tr>
                        <%--   <tr>
                                    <td class="rntCal nd_f" style="position: relative;" colspan="2">NON disponibile
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal prt_f" style="position: relative;" colspan="2">Closed
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal opz_f" style="position: relative;" colspan="2">MinStay Cambiato
                                    </td>
                                </tr>--%>
                    </table>
                    <div class="nulla">
                    </div>
                    <div id="closedCalendarCont" style="margin-left: 5px; margin-top: 20px; float: left;">
                    </div>

                </div>
            </div>
        </div>
        <div class="mainbox iCalMainBox">
            <div class="center">
                <span class="titoloboxmodulo" style="margin-top: 20px;">Allotment :</span>
                <div class="boxmodulo">
                    <table>
                        <tr>
                            <td style="width: 484px;">dal:
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="rdp_allotment_dtStart" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                    <DateInput DateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>al:
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="rdp_allotment_dtEnd" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                    <DateInput DateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>Allotment:
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="ntxt_changeAllotment" runat="server" Width="50" MinValue="0" Style="text-align: right;" Value="0">
                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                </telerik:RadNumericTextBox></td>
                            <td>
                                <asp:DropDownList ID="drp_change" runat="server" Style="height: 25px;" Width="100px">
                                    <asp:ListItem Value="-1">Default</asp:ListItem>
                                    <asp:ListItem Value="0">aumento di</asp:ListItem>
                                    <asp:ListItem Value="1">sconto di</asp:ListItem>
                                    <asp:ListItem Value="2">Fixed</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:LinkButton ID="lnk_send_allotment" runat="server" CssClass="inlinebtn" OnClick="lnk_send_allotment_Click">Aggiorna</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
