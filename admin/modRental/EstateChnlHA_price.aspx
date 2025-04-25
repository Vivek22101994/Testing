<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlHA_price.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlHA_price" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucHAEstateDetailsTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
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
            .RadInput_Default, .RadInputMgr_Default {
                float: right;
                margin-right: 10px;
            }
            .tabsTop.tabsChannelsTop table td a[title="Price"], #tabsHomeaway.tabsTop table td a[title="Price"]  {
	            background:#848484;
	            border-color:#606060;
	            color:#FFF;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <%# rntUtils.getAgent_logoForDetailsPage(ChnlHomeAwayProps.IdAdMedia) %>

            <h1 class="titolo_main"><%= contUtils.getLabel("lbl_DettagliPosizioneApt")%>:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop tabsChannelsTop tabsHomeAwayTop" id="tabsHomeaway">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>

            <div class="nulla">
            </div>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click"><span><%= contUtils.getLabel("lblSaveChanges")%></span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span><%= contUtils.getLabel("lblCancelChanges")%></span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div> 
            <div class="nulla"></div>
            <div class="mainline mainChannels mainHolidayLettings mainPriceHL">
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <div class="boxmodulo">                          
                            <div class="nulla"></div>
                            <table>
                                <tr>
                                    <td class="td_title" style="width:50%;">Rate 
                                    </td>
                                    <td style="width:30%">
                                        <asp:DropDownList ID="drp_changeIsDiscount" runat="server" Style="height: 25px;" Width="94%">
                                            <asp:ListItem Value="0">aumento di</asp:ListItem>
                                            <asp:ListItem Value="1">sconto di</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:10%">
                                        <telerik:RadNumericTextBox ID="ntxt_changeAmount" runat="server" Width="94%" MinValue="0" Style="text-align: right;">
                                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                    <td style="width:10%">
                                        <asp:DropDownList ID="drp_changeIsPercentage" runat="server" Style="height: 25px;" Width="94%">
                                            <asp:ListItem Value="1">%</asp:ListItem>
                                            <asp:ListItem Value="0">EUR</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="nulla"></div>
                <div class="mainbox iCalMainBox" style="display:none;">
                    <div class="center">
                        <span class="titoloboxmodulo" style="margin-top:20px;">Prezzi speciali</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td>dal:
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadDatePicker ID="rdp_rate_dtStart" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>al:
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadDatePicker ID="rdp_rate_dtEnd" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>MinStay
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadNumericTextBox ID="ntxt_rate_minStay" runat="server" Width="50" MinValue="0" Style="text-align: right; float:right;">
                                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="td_title" style="width:50%;">Rate 
                                    </td>
                                    <td style="width:30%;">
                                        <asp:DropDownList ID="drp_rate_changeIsDiscount" runat="server" Style="height: 25px;" Width="94%">
                                            <asp:ListItem Value="-1">Default</asp:ListItem>
                                            <asp:ListItem Value="0">aumento di</asp:ListItem>
                                            <asp:ListItem Value="1">sconto di</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:10%;">
                                        <telerik:RadNumericTextBox ID="ntxt_rate_changeAmount" runat="server" Width="94%" MinValue="0" Style="text-align: right;">
                                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                    <td style="width:10%;">
                                        <asp:DropDownList ID="drp_rate_changeIsPercentage" runat="server" Style="height: 25px;" Width="94%">
                                            <asp:ListItem Value="1">%</asp:ListItem>
                                            <asp:ListItem Value="0">EUR</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="4" style="text-align: center;">
                                        <strong>Nota bene,</strong><br />
                                        Tutte le date nel range che selezioni vengono sovrascritte<br />
                                        Impostare MinStay a 0 per ripristinare il Default
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: center;">
                                        <asp:LinkButton ID="lnk_rate_save" runat="server" CssClass="inlinebtn">Aggiorna</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal nd_f" style="position: relative;" colspan="4">
                                        NON disponibile
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal opz_f" style="position: relative;" colspan="4">
                                        Prezzo Cambiato
                                    </td>
                                </tr>
                            </table>
                            <div id="ratesCalendarCont" style="margin-left: 5px; margin-top: 20px; float: left;">
                            </div>

                            <asp:ListView ID="LvRateChangesTtp" runat="server">
                                <ItemTemplate>
                                    <div id='tooltip_rateChange_<%# ((DateTime)Eval("changeDate")).JSCal_dateToInt() %>' style="display: none;">
                                        <table cellspacing="3" cellpadding="0" border="0">
                                            <tr>
                                                <td class="td_title">Rate
                                                </td>
                                                <td>
                                                    <%# Eval("changeIsDiscount").objToInt32() >= 0 ?(Eval("changeIsDiscount").objToInt32()==1?"sconto di":"aumento di"):"Default" %>
                                                    <%# Eval("changeIsDiscount").objToInt32() >= 0 ?" "+Eval("changeAmount").objToInt32():"" %>
                                                    <%# Eval("changeIsDiscount").objToInt32() >= 0 ?(Eval("changeIsPercentage").objToInt32()==1?" %":" EUR"):"" %>
                                                </td>
                                            </tr>                                           
                                        </table>
                                    </div>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>
                <div class="nulla"></div>
                <div class="mainbox iCalMainBox" style="display:none;">
                    <div class="center">
                        <span class="titoloboxmodulo" style="margin-top:20px;">Closed / MinStay</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td style="width:50%;">dal:
                                    </td>
                                    <td style="width:50%;">
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
                                        <asp:DropDownList ID="drp_closed_isClosed" runat="server" Style="width:105px;">
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
                                        <%--<asp:LinkButton ID="lnk_closed_save" runat="server" CssClass="inlinebtn" OnClick="lnk_closed_save_Click">Aggiorna</asp:LinkButton>--%>
                                        <asp:LinkButton ID="lnk_closed_save" runat="server" CssClass="inlinebtn" >Aggiorna</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal nd_f" style="position: relative;" colspan="2">
                                        NON disponibile
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal prt_f" style="position: relative;" colspan="2">
                                        Closed
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rntCal opz_f" style="position: relative;" colspan="2">
                                        MinStay Cambiato
                                    </td>
                                </tr>
                            </table>
                            <div class="nulla">
                            </div>
                            <div id="closedCalendarCont" style="margin-left: 5px; margin-top: 20px; float: left;">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                function setCal() {
                    $("#ratesCalendarCont").datepicker({ numberOfMonths: [2, 3], stepMonths: 3, selectOtherMonths: false, showOtherMonths: false, showButtonPanel: false, onChangeMonthYear: setToolTip, beforeShowDay: rateCalDates });
                    //$("#closedCalendarCont").datepicker({ numberOfMonths: [2, 3], stepMonths: 3, selectOtherMonths: false, showOtherMonths: false, showButtonPanel: false, onChangeMonthYear: setToolTip, beforeShowDay: closedCalDates });
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
