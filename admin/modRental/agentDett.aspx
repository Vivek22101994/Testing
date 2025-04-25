<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="agentDett.aspx.cs" Inherits="ModRental.admin.modRental.agentDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modContent/UCgetImg.ascx" TagName="UCgetImg" TagPrefix="getImg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <link href="../../jquery/css/colpick.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script src="../../jquery/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var $j = jQuery.noConflict();
    </script>
    <script src="../../jquery/js/colpick.js" type=""></script>
    <script type="text/javascript">
        function setColorPicker() {

            $j('#picker_mainColor').colpick({
                flat: true,
                layout: 'hex',
                color: document.getElementById("<%=HF_picker_mainColor.ClientID%>").value.replace("#", ""),
                onSubmit: function (hsb, hex, rgb, el) {
                    document.getElementById("<%=HF_picker_mainColor.ClientID%>").value = "#" + hex;
                    //$("#divEx").css("background-color", "#" + hex);
                }
            });

                $j('#picker_supportColor').colpick({
                    flat: true,
                    layout: 'hex',
                    color: document.getElementById("<%=HF_picker_supportColor.ClientID%>").value.replace("#", ""),
                    onSubmit: function (hsb, hex, rgb, el) {
                        document.getElementById("<%=HF_picker_supportColor.ClientID%>").value = "#" + hex;
                        //$("#divEx").css("color", "#" + hex);
                    }
                });
                }

                function setDef() {
                    var bgcolor = document.getElementById("<%=HF_picker_mainColor.ClientID%>").value;
                    if (bgcolor != "") {
                        $("#divEx").css("background-color", bgcolor);
                    }
                    var fontcolor = document.getElementById("<%=HF_picker_supportColor.ClientID%>").value;
                    if (fontcolor != "") {
                        $("#divEx").css("color", fontcolor);
                    }
                }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <h1 class="titolo_main">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click" OnClientClick="return validateSave();"><span>Salva Modifiche</span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSendMail" runat="server" OnClick="lnkSendMail_Click"><span>Invia mail di benvenuto</span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');">
                    <span>Chiudi</span></a>
            </div>
            <div class="nulla">
            </div>
            <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                <script type="text/javascript">
                    function validateSave() {
                        var _error = "";
                        if ($("#<%= drp_pidDiscountType.ClientID%>").val() == "") {
                            _error += "<br />Seleziona 'Tipo Commissioni'";
                        }
                        if ($("#<%= drp_paymentType.ClientID%>").val() == "") {
                            _error += "<br />Seleziona 'Tipo Pagamento'";
                        }
                        if (_error != "") {
                            radalert("Attenzione!" + _error, 340, 110);
                            return false;
                        }
                        return true;
                    }
                </script>
            </telerik:RadScriptBlock>
        </div>
        <asp:Panel ID="pnlHomeAway" runat="server" CssClass="salvataggio">
            <div class="bottom_salva">
                <a href="/admin/modRental/ChnlHomeAwayFeatureValuesList.aspx" target="_blank" style="margin-right: 10px;">
                    <span>HomeAwayFeatureValues</span>
                </a>
            </div>
            <div class="bottom_salva">
                <a href="/admin/modRental/ChnlHomeAwayAcceptedPaymentForms.aspx" target="_blank" style="margin-right: 10px;">
                    <span>AcceptedPaymentForms</span>
                </a>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlExpedia" runat="server" CssClass="salvataggio">
           
            <a href="/admin/modRental/ChnlExpediaAmenitiesList.aspx" target="_blank">
                <asp:Literal ID="Literal114" runat="server" Text="#lbl#ExpediaExtras" OnPreRender="renderLiteral_setLabel"></asp:Literal></a>
            <a href="/admin/modRental/ChnlExpediaRoomAmenitiesList.aspx" target="_blank">
                <asp:Literal ID="Literal118" runat="server" Text="#lbl#ExpediaRoomExtras" OnPreRender="renderLiteral_setLabel"></asp:Literal></a>
            <%--<a href="/admin/modRental/chnlExpediaPropertyType.aspx" target="_blank">
                <asp:Literal ID="Literal117" runat="server" Text="#lbl#ExpediaPropertyType" OnPreRender="renderLiteral_setLabel"></asp:Literal></a>
            <a href="/admin/modRental/chnlExpediaRoomType.aspx" target="_blank">
                <asp:Literal ID="Literal115" runat="server" Text="#lbl#ExpediaRoomType" OnPreRender="renderLiteral_setLabel"></asp:Literal></a>--%>
            <a href="/admin/modRental/chnlExpediaBeds.aspx" target="_blank">
                <asp:Literal ID="Literal116" runat="server" Text="#lbl#ExpediaBeds" OnPreRender="renderLiteral_setLabel"></asp:Literal></a>
            <%--<asp:LinkButton ID="lnk_send_content" runat="server" OnClick="lnk_send_content_Click">Send Content</asp:LinkButton>
            <asp:LinkButton ID="lnk_get_status" runat="server" OnClick="lnk_get_status_Click">Get Status</asp:LinkButton>
            <asp:LinkButton ID="lnk_create_room" runat="server" OnClick="lnk_create_room_Click">Create/Update Rooms</asp:LinkButton>
            <asp:LinkButton ID="lnk_send_room_amenities" runat="server" OnClick="lnk_send_room_amenities_Click">Create/Update Amenities</asp:LinkButton>
            <asp:LinkButton ID="lnk_send_rate_plans" runat="server" OnClick="lnk_send_rate_plans_Click">Send RatePlans</asp:LinkButton>
            <asp:LinkButton ID="lnk_send_rates" runat="server" OnClick="lnk_send_rates_Click">Send Rates</asp:LinkButton>
            <asp:LinkButton ID="lnk_send_availability" runat="server" OnClick="lnk_send_availability_Click">Send Availability</asp:LinkButton>--%>
        </asp:Panel>

        <asp:Panel ID="pnlRentalsUnited" runat="server" CssClass="salvataggio">
            <div class="bottom_salva">
                <a href="/admin/modRental/ChnlRentalsUnitedFeatureValuesList.aspx" target="_blank" style="margin-right: 10px;">
                    <span>RentalsUnitedFeatureValues</span>
                </a>
            </div>
            <div class="bottom_salva">
                <a href="/admin/modRental/ChnlRentalsUnitedAcceptedPaymentForms.aspx" target="_blank" style="margin-right: 10px;">
                    <span>AcceptedPaymentForms</span>
                </a>
            </div>
            <div class="bottom_salva">
                <a href="/admin/modRental/ChnlRentalsUnitedInternsTypeList.aspx" target="_blank"><span>RentalsUnited Interns Types</span></a>
            </div>

            <div class="bottom_salva">
                <a href="/admin/modRental/ChnlRentalsUnitedInternsSubTypeList.aspx" target="_blank"><span>RentalsUnited Interns Sub Types</span></a>
            </div>
        </asp:Panel>


        <asp:Panel ID="pnlAirbnb" runat="server">
           <%-- <a href="/admin/modRental/chnlAirbnbPropertyTypeList.aspx" target="_blank">
                <asp:Literal ID="Literal119" runat="server" Text="Airbnb Property Type" OnPreRender="renderLiteral_setLabel"></asp:Literal></a>--%>
            
            <a href="/admin/modRental/ChnlAirbnbAmenity.aspx" target="_blank">
                <asp:Literal ID="Literal120" runat="server" Text="Airbnb Amenity" OnPreRender="renderLiteral_setLabel"></asp:Literal></a>
        </asp:Panel>

        <asp:Panel ID="pnlAtraveo" runat="server" CssClass="salvataggio">
            <div class="bottom_salva">
                <a href="/admin/modRental/ChnlAtraveoFeatureValuesList.aspx" target="_blank"><span>Atraveo FeatureValues
                </span></a>
            </div>
        </asp:Panel>
        <div class="nulla">
        </div>
        <div class="mainline">
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Dati Essenziali</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title" style="width: auto;">Mail di benvenuto:
                                </td>
                                <td>
                                    <asp:Literal ID="ltrMailSent" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Attivo?:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isActive" runat="server">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Accettato Contratto?:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_hasAcceptedContract" runat="server" Enabled="false">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Tipo Commissioni:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_pidDiscountType" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Tipo Pagamento:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_paymentType" runat="server">
                                        <asp:ListItem Value="">scegli</asp:ListItem>
                                        <asp:ListItem Value="0|0">Commissione</asp:ListItem>
                                        <asp:ListItem Value="1|0">Commissione - Sconto</asp:ListItem>
                                        <asp:ListItem Value="0|1">Saldo</asp:ListItem>
                                        <asp:ListItem Value="1|1">Saldo - Sconto</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Nome Agenzia:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_nameCompany" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_nameCompany" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Nome Contatto:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_honorific" runat="server" Style="margin-right: 10px; width: 60px;">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txt_nameFull" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_nameFull" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Account (Produttore):
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_pidReferer" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td class="td_title">Commissioni:
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="txt_cashDiscount" runat="server" Width="50" MaxValue="50" MinValue="0">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>&nbsp;%
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_cashDiscount" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td class="td_title">Il cliente paga all'agenzia?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_customerPaysAgency" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList><em>Questo parametro cambia anche il voucher!</em>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">il pagamento viene effettuato prima che il cliente arrivi?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_paymentBefore" runat="server" OnSelectedIndexChanged="drp_paymentBefore_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Giorni:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_days" Width="200px" Enabled="false" onkeypress="return numericvalidation(event);" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">A chi fattura Rental:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_fattura" runat="server">
                                        <asp:ListItem Value="1">Agenzia</asp:ListItem>
                                        <asp:ListItem Value="0">Cliente</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">la Fattura è 100% della prenotazione
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_ivoiceComplete" runat="server" onchange="set_txt_invoicePercentage(this.value)" OnSelectedIndexChanged="drp_ivoiceComplete_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Percentuale fattura:
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="txt_invoicePercentage" runat="server" Width="50" MinValue="0">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>&nbsp;%
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">il voucher viene emesso?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_voucher" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">invia link del voucher al prop?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_sentVoucherToOwner" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">invio comunicazioni ai clienti?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isMsgsEnabled" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">invio email manuale scheda prenotazione ?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isSendMannualEmail" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td class="td_title">Applica Agency Fee?:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isAgencyFeeApplied" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo" style="margin-top: 20px; margin-bottom: 0;">Residenza</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td colspan="3">Indirizzo:<br />
                                    <asp:TextBox runat="server" ID="txt_locAddress" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td>CAP:<br />
                                    <asp:TextBox runat="server" ID="txt_locZipCode" Width="100px" />
                                </td>
                                <td>Città:<br />
                                    <asp:TextBox runat="server" ID="txt_locCity" Width="100px" />
                                </td>
                                <td>Provincia/Stato:<br />
                                    <asp:TextBox runat="server" ID="txt_locState" Width="100px" />
                                </td>
                            </tr>                            
                            <tr>
                                 <td>Provincia:<br />
                                    <asp:TextBox runat="server" ID="txt_loc_province" Width="100px" />
                                </td>
                                <td>Nazione/Location:<br />
                                    <asp:DropDownList ID="drp_locCountry" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>Lingua:<br />
                                    <asp:DropDownList runat="server" ID="drp_pidLang">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div>
                <div class="mainbox">
                    <div class="top">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Limitazione offerte</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">Num persone:
                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox ID="ntxt_limite_numPersons" runat="server" Width="50">
                                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Percentuale:
                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox ID="ntxt_limite_discountLimit" runat="server" Width="50" Type="Percent">
                                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
            <div id="div_period" runat="server">
                <div class="mainbox">
                    <div class="top">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">CONTRATTI</span>
                        <div class="boxmodulo">
                            <div class="btn_save">
                                <asp:LinkButton ID="lnkNewContract" runat="server" OnClick="lnkNewContract_Click"><span>Nuovo contratto</span></asp:LinkButton>
                            </div>
                            <asp:ListView ID="LvContracts" runat="server" OnItemCommand="LvContracts_ItemCommand">
                                <ItemTemplate>
                                    <tr style="">
                                        <td align="center">
                                            <span><%# Eval("contractNumber") %></span>
                                        </td>
                                        <td align="center">
                                            <span><%# Eval("contractType") %></span>
                                        </td>
                                        <td align="center">
                                            <span><%# Eval("commissionAmount") %></span>
                                        </td>
                                        <td align="center">
                                            <span><%# ((DateTime?)Eval("dtStart")).formatITA(false) %></span>
                                        </td>
                                        <td align="center">
                                            <span><%# ((DateTime?)Eval("dtEnd")).formatITA(false) %></span>
                                        </td>
                                        <td>
                                            <a href="agentContractDett.aspx?id=<%# Eval("id") %>&idagent=<%= IdAgent %>">Dettagli</a>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="del" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');">Elimina</asp:LinkButton>
                                        </td>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <table id="Table1" runat="server">
                                        <tr>
                                            <th style="width: 200px" align="center">Contract num.
                                            </th>
                                            <th style="width: 200px" align="center">Contact type
                                            </th>
                                            <th style="width: 100px" align="center">Costo
                                            </th>
                                            <th style="width: 200px" align="center">inizio
                                            </th>
                                            <th style="width: 200px" align="center">fine
                                            </th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                    <div class="bottom">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Documento</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">Partita Iva:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docVat" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Codice Fiscale:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docCf" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Data nascita:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_birthDate" runat="server" Width="100px">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Luogo nascita:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_birthPlace" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Tipo:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_docType" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Numero:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docNum" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Rilasciato da:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docIssuePlace" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">in data:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_docIssueDate" runat="server" Width="100px">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Scadenza il:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_docExpiryDate" runat="server" Width="100px">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Contatti</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">E-mail:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contactEmail" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Telefono:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contactPhone" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Fax:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contactFax" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Web Address:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contactWebSite" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Come ha conosciuto?<br />
                                    <asp:TextBox runat="server" ID="txt_contactComeFrom" Width="300px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="center" runat="server" id="pnl_auth" visible="false">
                    <span class="titoloboxmodulo">Accessi</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">Login:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_authUsr" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Password:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_authPwd" Width="200px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="center" runat="server" id="pnlInv" visible="false">
                    <span class="titoloboxmodulo">Fatturazione</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">Societa fatturazione:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_invCompanyId" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Iva applicata:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_invTaxId" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Note sulla fattura</span>
                    <div class="boxmodulo">
                        <telerik:RadEditor runat="server" ID="re_notesInvoice" SkinID="DefaultSetOfTools" Height="200" Width="600" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
                            <CssFiles>
                                <telerik:EditorCssFile Value="" />
                            </CssFiles>
                        </telerik:RadEditor>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Note Interne</span>
                    <div class="boxmodulo">
                        <telerik:RadEditor runat="server" ID="re_notesInner" SkinID="DefaultSetOfTools" Height="200" Width="600" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
                            <CssFiles>
                                <telerik:EditorCssFile Value="" />
                            </CssFiles>
                        </telerik:RadEditor>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainbox" id="pnlSuperAdmin" runat="server">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Dati nascosti</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">IdAdMedia:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_IdAdMedia" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left" class="td_title">ApiKey:
                                </td>
                                <td valign="middle" align="left">
                                    <asp:TextBox runat="server" ID="txt_uid" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left" class="td_title">MagaRental Api/Xml 
                                                    <asp:Literal ID="Literal69" runat="server" Text="#lbl#lblPhoto" OnPreRender="renderLiteral_setLabel"></asp:Literal>:
                                </td>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_chnlMGetPhotos" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left" class="td_title">MagaRental Api/Xml 
                                                    <asp:Literal ID="Literal70" runat="server" Text="#lbl#AdminMenuDesc" OnPreRender="renderLiteral_setLabel"></asp:Literal>:
                                </td>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_chnlMGetTexts" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left" class="td_title">MagaRental Api/Xml 
                                                    <asp:Literal ID="Literal105" runat="server" Text="#lbl#Amenities" OnPreRender="renderLiteral_setLabel"></asp:Literal>:
                                </td>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_chnlMGetAmenities" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left" class="td_title">MagaRental Api/Xml 
                                                    <asp:Literal ID="Literal106" runat="server" Text="#lbl#Address" OnPreRender="renderLiteral_setLabel"></asp:Literal>:
                                </td>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_chnlMGetAddress" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>

            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Logo Agenzia:</span>
                    <div class="boxmodulo">
                        <getImg:UCgetImg ID="imgLogo" runat="server" />
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>

            <div class="mainbox wldett">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Dati White Label:</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title" style="width: auto;">Nome:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_wl_name" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title" style="width: auto;">Logo:
                                </td>
                                <td>
                                    <getImg:UCgetImg ID="img_wl_logo" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td class="td_title" style="width: auto;">Map Marker:
                                </td>
                                <td>
                                    <getImg:UCgetImg ID="img_wl_mapmarker" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td class="td_title" style="width: auto;">Select Template:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_wl_css" runat="server">
                                        <asp:ListItem Text="style" Value="style"></asp:ListItem>
                                        <asp:ListItem Text="style_green" Value="style_green"></asp:ListItem>
                                        <asp:ListItem Text="style_red" Value="style_red"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr style="display: none">
                                <td class="td_title" style="width: auto;">Main color:
                                </td>
                                <td>
                                    <asp:HiddenField ID="HF_picker_mainColor" Value="0" runat="server" />
                                    <div id="picker_mainColor"></div>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="td_title" style="width: auto;">Support color:
                                </td>
                                <td>
                                    <asp:HiddenField ID="HF_picker_supportColor" Value="0" runat="server" />
                                    <div id="picker_supportColor"></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>

            <div class="mainbox wlpricedett">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Change Price White Label:</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title" style="width: auto;">Rate:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_wl_changeIsDiscount" runat="server" Style="height: 25px;" Width="100px">
                                        <asp:ListItem Value="0">aumento di</asp:ListItem>
                                        <asp:ListItem Value="1">sconto di</asp:ListItem>
                                    </asp:DropDownList>
                                    <telerik:RadNumericTextBox ID="ntxt_wl_changeAmount" runat="server" Width="50" MinValue="0" Style="text-align: right;">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>

        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
