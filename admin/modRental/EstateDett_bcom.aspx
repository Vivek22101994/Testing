<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateDett_bcom.aspx.cs" Inherits="ModRental.admin.modRental.EstateDett_bcom" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<%@ Register Src="~/admin/modContent/UCgetFile.ascx" TagName="UCgetFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

        <style type="text/css">
            .main div.mainbox {width:100%;}
            .ui-datepicker td{
                padding: 1px !important;
            }
            .RadPicker td{
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
        </style>

            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />

            

            <h1 class="titolo_main">
                Impostazioni Booking.com della struttura:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_save_Click"><span>Salva</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="/admin/rnt_estate_list.aspx"><span>Torna nel elenco</span></a>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnkUpdateAvv" runat="server" OnClick="lnkUpdateAvv_Click"><span>Invia disponibilita</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnkUpdateRates" runat="server" OnClick="lnkUpdateRates_Click"><span>Invia prezzi</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <div class="boxmodulo">
                            <table border="0" cellspacing="3" cellpadding="0">
                                <tr>
                                    <td class="td_title">HotelId:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_bcomHotelId" Width="200px"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">RoomId:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_bcomRoomId" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Nome Bcom:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_bcomName" runat="server" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Attivo:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_bcomEnabled" runat="server" Width="50px">
                                            <asp:ListItem Value="0">NO</asp:ListItem>
                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        NOTE:<br/>
                                        <b>Prezzo single:</b> prezzo vendita di Minima occupazione<br />
                                        <b>Prezzo intero:</b> prezzo vendita di Massima occupazione<br />
                                    </td>
                                </tr>
                                <asp:ListView ID="LvRates" runat="server">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_pidEstate" Visible="false" runat="server" Text='<%# Eval("pidEstate") %>' />
                                        <asp:Label ID="lbl_priceType" Visible="false" runat="server" Text='<%# Eval("priceType") %>' />
                                        <asp:Label ID="lbl_rateType" Visible="false" runat="server" Text='<%# Eval("rateType") %>' />
                                        <tr>
                                            <td class="td_title">
                                                <%# Eval("priceType").objToInt32()==BcomProps.PriceTypeSingle?"Prezzo single":"" %>
                                                <%# Eval("priceType").objToInt32()==BcomProps.PriceTypeFull?"Prezzo intero":"" %>
                                                <br />
                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeStandard?"Standard Rate":"" %>
                                                
                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeStandard2?"Standard Rate 2":"" %>
                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeStandard3?"Standard Rate 3":"" %>
                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeStandard4?"Standard Rate 4":"" %>

                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeNotRefund?"NotRefund Rate":"" %>
                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeSpecial?"Special Rate":"" %> 
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_changeIsDiscount" runat="server" Style="height: 25px;" Width="100px">
                                                    <asp:ListItem Value="0">aumento di</asp:ListItem>
                                                    <asp:ListItem Value="1">sconto di</asp:ListItem>
                                                </asp:DropDownList>
                                                <telerik:RadNumericTextBox ID="ntxt_changeAmount" runat="server" Width="50" MinValue="0" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                                <asp:DropDownList ID="drp_changeIsPercentage" runat="server" Style="height: 25px;" Width="50px">
                                                    <asp:ListItem Value="0">EUR</asp:ListItem>
                                                    <asp:ListItem Value="1">%</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <span class="titoloboxmodulo">Prezzi speciali</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td>dal:
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
                                <tr>
                                    <td>MinStay
                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox ID="ntxt_rate_minStay" runat="server" Width="50" MinValue="0" Style="text-align: right;">
                                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                </tr>
                                <asp:ListView ID="LvRateChanges" runat="server">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_pidEstate" Visible="false" runat="server" Text='<%# Eval("pidEstate") %>' />
                                        <asp:Label ID="lbl_priceType" Visible="false" runat="server" Text='<%# Eval("priceType") %>' />
                                        <asp:Label ID="lbl_rateType" Visible="false" runat="server" Text='<%# Eval("rateType") %>' />
                                        <tr>
                                            <td class="td_title">
                                                <%# Eval("priceType").objToInt32()==BcomProps.PriceTypeSingle?"Prezzo single":"" %>
                                                <%# Eval("priceType").objToInt32()==BcomProps.PriceTypeFull?"Prezzo intero":"" %>
                                                <br />
                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeStandard?"Standard Rate":"" %>

                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeStandard2?"Standard Rate 2":"" %>
                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeStandard3?"Standard Rate 3":"" %>
                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeStandard4?"Standard Rate 4":"" %>

                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeNotRefund?"NotRefund Rate":"" %>
                                                <%# Eval("rateType").objToInt32()==BcomProps.RateTypeSpecial?"Special Rate":"" %> 
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_changeIsDiscount" runat="server" Style="height: 25px;" Width="100px">
                                                    <asp:ListItem Value="-1">Default</asp:ListItem>
                                                    <asp:ListItem Value="0">aumento di</asp:ListItem>
                                                    <asp:ListItem Value="1">sconto di</asp:ListItem>
                                                </asp:DropDownList>
                                                <telerik:RadNumericTextBox ID="ntxt_changeAmount" runat="server" Width="50" MinValue="0" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                                <asp:DropDownList ID="drp_changeIsPercentage" runat="server" Style="height: 25px;" Width="50px">
                                                    <asp:ListItem Value="0">EUR</asp:ListItem>
                                                    <asp:ListItem Value="1">%</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <strong>Nota bene,</strong><br />
                                        Tutte le date nel range che selezioni vengono sovrascritte<br />
                                        Impostare MinStay a 0 per ripristinare il Default
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:LinkButton ID="lnk_rate_save" runat="server" CssClass="inlinebtn" OnClick="lnk_rate_save_Click">Aggiorna</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal nd_f" style="position: relative;"></td>
                                    <td>NON disponibile
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal opz_f" style="position: relative;"></td>
                                    <td>Prezzo Cambiato
                                    </td>
                                </tr>
                            </table>
                            <div id="ratesCalendarCont" style="margin-left: 5px; margin-top: 20px;">
                            </div>

                            <asp:ListView ID="LvRateChangesTtp" runat="server">
                                <ItemTemplate>
                                    <div id="tooltip_rateChange_<%# ((DateTime)Eval("Dt")).JSCal_dateToInt() %>" style="display: none;">
                                        <table cellspacing="3" cellpadding="0" border="0">
                                            <tr>
                                                <td class="td_title">Prezzo single
                                                        <br />
                                                    Standard Rate
                                                </td>
                                                <td>
                                                    <%# Eval("StandardSingle_changeIsDiscount").objToInt32() >= 0 ?(Eval("StandardSingle_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("StandardSingle_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("StandardSingle_changeAmount").objToInt32():"" %>
                                                    <%# Eval("StandardSingle_changeIsDiscount").objToInt32() >= 0 ?(Eval("StandardSingle_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Prezzo single
                                                        <br />
                                                    Standard Rate 2
                                                </td>
                                                <td>
                                                    <%# Eval("Standard2Single_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard2Single_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("Standard2Single_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("Standard2Single_changeAmount").objToInt32():"" %>
                                                    <%# Eval("Standard2Single_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard2Single_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="td_title">Prezzo single
                                                        <br />
                                                    Standard Rate 3
                                                </td>
                                                <td>
                                                    <%# Eval("Standard3Single_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard3Single_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("Standard3Single_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("Standard3Single_changeAmount").objToInt32():"" %>
                                                    <%# Eval("Standard3Single_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard3Single_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="td_title">Prezzo single
                                                        <br />
                                                    Standard Rate 4
                                                </td>
                                                <td>
                                                    <%# Eval("Standard4Single_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard4Single_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("Standard4Single_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("Standard4Single_changeAmount").objToInt32():"" %>
                                                    <%# Eval("Standard4Single_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard4Single_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="td_title">Prezzo single
                                                        <br />
                                                    NotRefund Rate
                                                </td>
                                                <td>
                                                    <%# Eval("NotRefundSingle_changeIsDiscount").objToInt32() >= 0 ?(Eval("NotRefundSingle_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("NotRefundSingle_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("NotRefundSingle_changeAmount").objToInt32():"" %>
                                                    <%# Eval("NotRefundSingle_changeIsDiscount").objToInt32() >= 0 ?(Eval("NotRefundSingle_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Prezzo single
                                                        <br />
                                                    Special Rate
                                                </td>
                                                <td>
                                                    <%# Eval("SpecialSingle_changeIsDiscount").objToInt32() >= 0 ?(Eval("SpecialSingle_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("SpecialSingle_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("SpecialSingle_changeAmount").objToInt32():"" %>
                                                    <%# Eval("SpecialSingle_changeIsDiscount").objToInt32() >= 0 ?(Eval("SpecialSingle_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Prezzo intero
                                                        <br />
                                                    Standard Rate
                                                </td>
                                                <td>
                                                    <%# Eval("StandardFull_changeIsDiscount").objToInt32() >= 0 ?(Eval("StandardFull_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("StandardFull_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("StandardFull_changeAmount").objToInt32():"" %>
                                                    <%# Eval("StandardFull_changeIsDiscount").objToInt32() >= 0 ?(Eval("StandardFull_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="td_title">Prezzo intero
                                                        <br />
                                                    Standard Rate 2
                                                </td>
                                                <td>
                                                    <%# Eval("Standard2Full_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard2Full_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("Standard2Full_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("Standard2Full_changeAmount").objToInt32():"" %>
                                                    <%# Eval("Standard2Full_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard2Full_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="td_title">Prezzo intero
                                                        <br />
                                                    Standard Rate 3
                                                </td>
                                                <td>
                                                    <%# Eval("Standard3Full_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard3Full_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("Standard3Full_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("Standard3Full_changeAmount").objToInt32():"" %>
                                                    <%# Eval("Standard3Full_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard3Full_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="td_title">Prezzo intero
                                                        <br />
                                                    Standard Rate 4
                                                </td>
                                                <td>
                                                    <%# Eval("Standard4Full_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard4Full_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("Standard4Full_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("Standard4Full_changeAmount").objToInt32():"" %>
                                                    <%# Eval("Standard4Full_changeIsDiscount").objToInt32() >= 0 ?(Eval("Standard4Full_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="td_title">Prezzo intero
                                                        <br />
                                                    NotRefund Rate
                                                </td>
                                                <td>
                                                    <%# Eval("NotRefundFull_changeIsDiscount").objToInt32() >= 0 ?(Eval("NotRefundFull_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("NotRefundFull_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("NotRefundFull_changeAmount").objToInt32():"" %>
                                                    <%# Eval("NotRefundFull_changeIsDiscount").objToInt32() >= 0 ?(Eval("NotRefundFull_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Prezzo intero
                                                        <br />
                                                    Special Rate
                                                </td>
                                                <td>
                                                    <%# Eval("SpecialFull_changeIsDiscount").objToInt32() >= 0 ?(Eval("SpecialFull_changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("SpecialFull_changeIsDiscount").objToInt32() >= 0 ?" "+Eval("SpecialFull_changeAmount").objToInt32():"" %>
                                                    <%# Eval("SpecialFull_changeIsDiscount").objToInt32() >= 0 ?(Eval("SpecialFull_changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <span class="titoloboxmodulo">Closed / MinStay</span>
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
                                <tr>
                                    <td>Closed
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_closed_isClosed" runat="server">
                                            <asp:ListItem Value="0" Text="Default"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Chiuso"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>MinStay
                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox ID="ntxt_closed_minStay" runat="server" Width="50" MinValue="0" Style="text-align: right;">
                                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <strong>Nota bene,</strong><br />
                                        Tutte le date nel range che selezioni vengono sovrascritte<br />
                                        Impostare MinStay a 0 per ripristinare il Default
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:LinkButton ID="lnk_closed_save" runat="server" CssClass="inlinebtn" OnClick="lnk_closed_save_Click">Aggiorna</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal nd_f" style="position: relative;"></td>
                                    <td>NON disponibile
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal prt_f" style="position: relative;"></td>
                                    <td>Closed
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal opz_f" style="position: relative;"></td>
                                    <td>MinStay Cambiato
                                    </td>
                                </tr>
                            </table>
                            <div class="nulla">
                            </div>
                            <div id="closedCalendarCont" style="margin-left: 5px; margin-top: 20px;">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                function setCal() {
                    $("#ratesCalendarCont").datepicker({ numberOfMonths: [2, 3], stepMonths: 3, selectOtherMonths: false, showOtherMonths: false, showButtonPanel: false, onChangeMonthYear: setToolTip, beforeShowDay: rateCalDates });
                    $("#closedCalendarCont").datepicker({ numberOfMonths: [2, 3], stepMonths: 3, selectOtherMonths: false, showOtherMonths: false, showButtonPanel: false, onChangeMonthYear: setToolTip, beforeShowDay: closedCalDates });
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
