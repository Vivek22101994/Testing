<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master"
    AutoEventWireup="true" CodeBehind="extraservice_booking.aspx.cs" Inherits="RentalInRome.reservationarea.extraservice_booking" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<%@ Register Src="UC_reservedService.ascx" TagName="UC_reservedService" TagPrefix="uc3" %>
<%@ Register Src="UC_breadcrumb.ascx" TagName="UC_breadcrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script src="/jquery/plugin/jquery.creditCardValidator.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <uc4:UC_breadcrumb ID="UC_breadcrumb1" runat="server" />
    <asp:HiddenField ID="HFResId" runat="server" />
    <asp:HiddenField ID="HFBookingId" runat="server" />
    <div id="mainPayment" class="main">
        <div class="sx">
            <h3 class="underlined">
                <%=CurrentSource.getSysLangValue("lblYourReservation")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:UC_sx ID="UC_sx1" runat="server" />
                </div>
            </div>
            <uc3:UC_reservedService ID="UC_reservedService1" runat="server" />
        </div>
        <div class="dx">
            <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
            <div class="bookingBox bookingFormPg">
                <%-- <a class="backToList" href="/<%= currEstateLN.page_path %>?r=s" style="width: auto;
                            margin-bottom: 10px;">
                            <%= contUtils.getLabel("lblBackToTheApt")%></a>--%>
                <h1 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                    <%= contUtils.getLabel("lblYourBooking")%>
                </h1>
                <div class="nulla">
                </div>
                <strong class="tit_prenotazione">
                    <%= contUtils.getLabel("lblBookingForm")%></strong>
                <div style="display: none;" id="pnlErrorMsg" class="testo_errore">
                    <%= contUtils.getLabel("lblRequiredFieldsNeedToBeFilled")%>
                </div>
                <div class="form_prenotazione">
                    <span id="nomeLabel">
                        <%= contUtils.getLabel("lblName")%>*
                        <asp:TextBox ID="txt_nameFirst" runat="server"></asp:TextBox></span>
                    <span id="cognomeLabel">
                        <%= contUtils.getLabel("lblSurname")%>*
                            <asp:TextBox ID="txt_nameLast" runat="server"></asp:TextBox></span>
                </div>
                <div class="form_prenotazione">
                    <span id="indirizzoLabel">
                        <%= contUtils.getLabel("lblAddress")%>*
                        <asp:TextBox ID="txt_loc_address" runat="server"></asp:TextBox></span>
                </div>
                <div class="form_prenotazione">
                    <span id="cittaLabel">
                        <%= contUtils.getLabel("lblCity")%>*
                        <asp:TextBox ID="txt_loc_city" runat="server"></asp:TextBox></span>
                    <span id="provLabel">
                        <%= contUtils.getLabel("locStateProvince")%>
                        <asp:TextBox ID="txt_loc_state" runat="server"></asp:TextBox></span>
                </div>
                <div class="form_prenotazione">
                    <span id="nazioneLabel">
                        <%= contUtils.getLabel("lblCountry")%>*
                        <asp:DropDownList runat="server" ID="drp_loc_country" CssClass="field select large"
                            OnDataBound="drp_loc_country_DataBound" DataSourceID="LDS_country" DataTextField="title"
                            DataValueField="title" TabIndex="5">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext"
                            OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                            <WhereParameters>
                                <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                    </span>
                    <span id="capLabel">
                        <%= contUtils.getLabel("lblZipCode")%>
                        <asp:TextBox ID="txt_loc_zip_code" runat="server"></asp:TextBox></span>
                </div>
                <div class="form_prenotazione">
                    <span id="emailLabel">
                        <%= contUtils.getLabel("reqEmail")%>*<asp:TextBox ID="txt_contact_email" runat="server"></asp:TextBox></span>
                    <span id="telLabel">
                        <%= contUtils.getLabel("reqPhoneNumber")%>*<asp:TextBox ID="txt_contact_phone_mobile"
                            runat="server"></asp:TextBox></span>
                </div>
                <div class="form_prenotazione">
                    <span id="cfLabel">
                        <%= contUtils.getLabel("lblTaxCode")%><asp:TextBox ID="txt_doc_cf_num" runat="server"></asp:TextBox></span>
                    <span id="pivaLabel">
                        <%= contUtils.getLabel("formVatNumber")%><asp:TextBox ID="txt_doc_vat_num" runat="server"></asp:TextBox></span>
                </div>
                <div class="nulla">
                </div>
                <div id="Div1" class="bookingNotes" runat="server" visible="false">
                    <strong class="tit_prenotazione" style="font-size: 14px; font-weight: bold;"><em
                        style="color: #8F382E;">
                        <%= contUtils.getLabel("lblNotes")%></em></strong>
                    <span class="testo_prenotazione">Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh
                            euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Lorem ipsum dolor
                            sit amet, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam
                            erat volutpat.
                    </span>
                    <a class="readMore" target="_blank">
                        <%= contUtils.getLabel("lblTermsAndConditions")%></a>
                </div>
            </div>
            <div class="bookingBox riepilogo">
                <strong class="tit_prenotazione">
                    <%= contUtils.getLabel("lblExtraServices")%></strong>
                <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand" OnItemDataBound="LV_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');"
                                    CssClass="servDel"></asp:LinkButton>
                            </td>
                            <td>
                                <span>
                                    <asp:Label ID="lbl_date" runat="server" Text='<%# Eval("date") %>'></asp:Label>
                                    <asp:Label ID="lbl_datevalue" runat="server" Text='<%# Eval("dateValue") %>' Visible="false"></asp:Label>
                            </td>
                            <td>
                                <span>
                                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("serviceId") %>' Visible="false"></asp:Label>
                                  
                                    <%# Eval("serviceName")%></span>
                            </td>
                            <td align="right">
                                <%-- <span>
                                    <asp:Label ID="lbl_commission" runat="server"></asp:Label><br />
                                    <asp:Label ID="lbl_price" runat="server"></asp:Label>
                                   
                                </span>--%>
                                <span class="servPayNowSx" id="div_payNow" runat="server">
                                    <em>
                                        <%= CurrentSource.getSysLangValue("lbl_payNow")%>:</em>
                                    <asp:Label ID="lbl_commission" runat="server"></asp:Label>
                                </span>
                                <span class="servTotalSx">
                                    <em>
                                        <%= CurrentSource.getSysLangValue("rnt_pr_total")%>:</em>
                                    <asp:Label ID="lbl_price" runat="server"></asp:Label>
                                </span>
                                <asp:HiddenField ID="HF_anticipo" runat="server" />
                                <asp:HiddenField ID="HF_commission" runat="server" />
                                <asp:Label ID="lbl_priceType" runat="server" Text='<%# Eval("priceTypeId")%>' Visible="false"></asp:Label>
                                <asp:Label ID="lbl_adults" runat="server" Text='<%# Eval("adults")%>' Visible="false"></asp:Label>
                                <asp:Label ID="lbl_childern" runat="server" Text='<%# Eval("children")%>' Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" style="">
                            <tr style="text-align: left">
                                <th style="width: 5%"></th>
                                <th style="width: 15%">
                                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                        <%=CurrentSource.getSysLangValue("lblDate")%>
                                    </telerik:RadCodeBlock>
                                </th>
                                <th style="width: 52%">
                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                        <%=CurrentSource.getSysLangValue("lblName")%>
                                    </telerik:RadCodeBlock>
                                </th>
                                <th style="width: 25%" align="right">
                                    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                                        <%=CurrentSource.getSysLangValue("lblPrice")%>
                                    </telerik:RadCodeBlock>
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>

                <strong class="txt_totalPrice">
                    <%= contUtils.getLabel("lblTotalPrice")%></strong>
                <asp:Label CssClass="lbl_totalPrice" ID="lbl_totalPrice" runat="server" Text="0"></asp:Label>

                <div class="nulla">
                </div>

                <strong class="txt_totalPrice txt_PayNow">
                    <%= contUtils.getLabel("lbl_payNow")%></strong>
                <asp:Label CssClass="lbl_totalPrice lbl_payNow" ID="lbl_payNow" runat="server" Text="0,00"></asp:Label>

            </div>
            <div class="bookingBox paymentMethod">
                <strong class="tit_prenotazione">
                    <%= contUtils.getLabel("lblSystemPayment")%></strong>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <%--<table class="selPaymentMethod">
                            <tbody>
                                <tr>
                                    <td valign="middle" align="left">
                                        <asp:RadioButton ID="rbtPagamento_CC" runat="server" GroupName="rbtPagamento" AutoPostBack="true"
                                                    OnCheckedChanged="rbtPagamento_OnCheckedChanged" Checked="true" />
                                        <label class="testo_scelta_prenotazioni">
                                            <%= contUtils.getLabel("lblCreditCard")%></label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>--%>
                        <div id="pnlPagamento_CC" class="" runat="server">
                            <input type="hidden" id="rbtPagamento" value="cc" />
                            <span class="testo_scelta_prenotazioni txt_selected_method">
                                <%= contUtils.getLabel("lblPleaseInsertCardDetails")%></span><br />
                            <br />
                            <div class="nulla">
                            </div>
                            <div class="form_prenotazione">
                                <span id="titolareCartaLabel">
                                    <%= contUtils.getLabel("lblHolder")%>*
                                    <input type="text" id="txt_cc_titolareCarta" runat="server" /></span>
                                <span id="numeroCartaLabel">
                                    <%= contUtils.getLabel("lblCreditCardNumber")%>*
                                        <input type="text" id="txt_cc_numeroCarta" runat="server" /></span>
                            </div>
                            <div class="form_prenotazione">
                                <span id="tipoCartaLabel">
                                    <%= contUtils.getLabel("lblCardType")%>*
                                    <asp:DropDownList ID="drp_cc_tipoCarta" runat="server">
                                        <asp:ListItem Value="" Text="- - -"></asp:ListItem>
                                        <asp:ListItem Value="visa" Text="Visa"></asp:ListItem>
                                        <asp:ListItem Value="mastercard" Text="MasterCard"></asp:ListItem>
                                        <%-- <asp:ListItem Value="amex" Text="American Express"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </span>
                                <span id="dataLabel">
                                    <%= contUtils.getLabel("lblDueDate")%>*
                                    <select id="drp_cc_annoScadenza" runat="server" style="width: 85px;">
                                        <option value="2013">2013</option>
                                        <option value="2014">2014</option>
                                        <option value="2015">2015</option>
                                        <option value="2016">2016</option>
                                        <option value="2017">2017</option>
                                        <option value="2018">2018</option>
                                        <option value="2019">2019</option>
                                        <option value="2020">2020</option>
                                        <option value="2021">2021</option>
                                        <option value="2022">2022</option>
                                        <option value="2023">2023</option>
                                    </select>
                                    <select id="drp_cc_meseScadenza" runat="server" style="width: 85px; margin-right: 10px;">
                                        <option value="1">1</option>
                                        <option value="2">2</option>
                                        <option value="3">3</option>
                                        <option value="4">4</option>
                                        <option value="5">5</option>
                                        <option value="6">6</option>
                                        <option value="7">7</option>
                                        <option value="8">8</option>
                                        <option value="9">9</option>
                                        <option value="10">10</option>
                                        <option value="11">11</option>
                                        <option value="12">12</option>
                                    </select>
                                </span>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="nulla">
                </div>
                <div class="secureVerify">
                    <span id="siteseal"><script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=KQsv4yBM0BqPJHlzehZDvQMMOuVk7bQKurfWOSZRP9jcl3REyDbv0elwOgp0"></script></span>
                    <div class="nulla">
                    </div>
                    <div id="pnl_bookingForm_privacyAgree" class="testo_scelta_prenotazioni" style="margin-top: 10px;">
                        <%-- <a target="_blank" href="<%=contUtils.getStp_pagePath(3, App.LangID) %>" style="margin-bottom: 5px;
                                display: block;">--%>
                        <a href="#">
                            <%= contUtils.getLabel("lblTermsAndConditions")%></a>
                        <!-- PUT REAL TERMS AND CONDITIONS HERE -->
                        <div class="nulla">
                        </div>
                        <input type="checkbox" value="si" id="chk_bookingForm_privacyAgree" name="accettazione" />
                        <label for="chk_bookingForm_privacyAgree">
                            <%= contUtils.getLabel("lblAcceptTermsAndConditions")%>
                        </label>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
                <div class="nulla">
                </div>
                <asp:LinkButton ID="lnkBookNow" runat="server" CssClass="btnCalcola" OnClick="lnkBookNow_Click"
                    OnClientClick="return bookingForm_validateForm();" Style="margin-top: 20px;">
                <span>
               <%= contUtils.getLabel("lblFinishYourReservations")%>
                </span>
                </asp:LinkButton>
                <asp:Label ID="lbl_error" runat="server"></asp:Label>
                <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
            </div>
        </div>
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript">
            var bookingForm_validateForm_firstTime = true;
            function bookingForm_validateForm() {
                var callBack = null;
                if (bookingForm_validateForm_firstTime) {
                    callBack = function () { return bookingForm_validateForm(); };
                    bookingForm_validateForm_firstTime = false;
                }
                var isValid = true;
                FORM_hideErrorToolTip();
                if (!FORM_validate_checkBoxField("chk_bookingForm_privacyAgree", "pnl_bookingForm_privacyAgree", "", "Accettare il trattamento dei dati personali", callBack))
                    isValid = false;


                if (!FORM_validate_requiredField("<%= drp_cc_annoScadenza.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if (!FORM_validate_requiredField("<%= drp_cc_meseScadenza.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if ($('#<%= drp_cc_annoScadenza.ClientID%>').val() != "" && $('#<%= drp_cc_meseScadenza.ClientID%>').val() != "") {
                    $("#<%= drp_cc_meseScadenza.ClientID%>").removeClass("errorInput");
                        var ccExpYear = parseInt($('#<%= drp_cc_annoScadenza.ClientID%>').val(), 10);
                        var ccExpMonth = parseInt($('#<%= drp_cc_meseScadenza.ClientID%>').val(), 10);
                        var expDate = new Date(ccExpYear, ccExpMonth);
                        var today = new Date();
                        //alert(ccExpYear+"/"+ ccExpMonth);
                        if (expDate < today) {
                            FORM_showErrorToolTip("<%= contUtils.getLabel("lblCcExpired")%>", "<%= drp_cc_meseScadenza.ClientID%>");
                            $("#<%= drp_cc_meseScadenza.ClientID%>").addClass("errorInput");
                            isValid = false;
                        }
                    }
                    if (!FORM_validate_requiredField("<%= drp_cc_tipoCarta.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if (!FORM_validate_requiredField("<%= txt_cc_numeroCarta.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if ($('#<%= drp_cc_tipoCarta.ClientID%>').val() != "" && $('#<%= txt_cc_numeroCarta.ClientID%>').val() != "") {
                    $("#<%= txt_cc_numeroCarta.ClientID%>").removeClass("errorInput");
                        var length_valid;
                        var luhn_valid;
                        $('#<%= txt_cc_numeroCarta.ClientID%>').validateCreditCard(function (result) { length_valid = result.length_valid; luhn_valid = result.luhn_valid; }, { accept: [$('#<%= drp_cc_tipoCarta.ClientID%>').val()] })
                        if (!length_valid || !luhn_valid) {
                            FORM_showErrorToolTip("<%= contUtils.getLabel("lblCcNotValid")%>", "<%= txt_cc_numeroCarta.ClientID%>");
                            $("#<%= txt_cc_numeroCarta.ClientID%>").addClass("errorInput");
                            isValid = false;
                        }
                    }
                    if (!FORM_validate_requiredField("<%= txt_cc_titolareCarta.ClientID%>", "", "", "", callBack))
                    isValid = false;

                if (!FORM_validate_requiredField("<%= txt_contact_phone_mobile.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if (!FORM_validate_requiredField("<%= txt_contact_email.ClientID%>", "", "", "", callBack))
                    isValid = false;
                else if (!FORM_validate_emailField("<%= txt_contact_email.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if (!FORM_validate_requiredField("<%= drp_loc_country.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if (!FORM_validate_requiredField("<%= txt_loc_city.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if (!FORM_validate_requiredField("<%= txt_loc_address.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if (!FORM_validate_requiredField("<%= txt_nameLast.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if (!FORM_validate_requiredField("<%= txt_nameFirst.ClientID%>", "", "", "", callBack))
                    isValid = false;
                if (!isValid) {

                    // $("#pnlErrorMsg").css("display", "block");
                    $.scrollTo($("#pnl_errorTooltip").offset().top - 50, 500);
                }


                return isValid;

            }
        </script>

    </telerik:RadCodeBlock>
</asp:Content>
