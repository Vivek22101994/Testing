<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="EstateExtraOwnerDett.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateExtraOwnerDett" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <h1 class="titolo_main">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
           <%-- <div class="bottom_salva">
                <asp:LinkButton ID="lnkSendMail" runat="server" OnClick="lnkSendMail_Click"><span>Invia mail di benvenuto</span></asp:LinkButton></div>--%>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');">
                    <span>Chiudi</span></a>
            </div>
            <div class="nulla">
            </div>
            
        </div>
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
                                <td class="td_title">
                                    Mail di benvenuto:
                                </td>
                                <td>
                                    <asp:Literal ID="ltrMailSent" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Attivo?:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isActive" runat="server">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Accettato Contratto?:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_hasAcceptedContract" runat="server" Enabled="false">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td class="td_title">
                                    Tipo Commissioni:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_pidDiscountType" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Tipo Pagamento:
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
                            </tr>--%>
                            <tr>
                                <td class="td_title">
                                    Nome Agenzia:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_nameCompany" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_nameCompany" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Nome Contatto:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_honorific" runat="server" Style="margin-right: 10px; width: 60px;">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txt_nameFull" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_nameFull" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Account (Produttore):
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_pidReferer" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="Tr1" runat="server" visible="false">
                                <td class="td_title">
                                    Commissioni:
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="txt_cashDiscount" runat="server" Width="50" MaxValue="50" MinValue="0">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>&nbsp;%
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_cashDiscount" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
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
                                <td colspan="3">
                                    Indirizzo:<br />
                                    <asp:TextBox runat="server" ID="txt_locAddress" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    CAP:<br />
                                    <asp:TextBox runat="server" ID="txt_locZipCode" Width="100px" />
                                </td>
                                <td>
                                    Città:<br />
                                    <asp:TextBox runat="server" ID="txt_locCity" Width="100px" />
                                </td>
                                <td>
                                    Provincia/Stato:<br />
                                    <asp:TextBox runat="server" ID="txt_locState" Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Nazione/Location:<br />
                                    <asp:DropDownList ID="drp_locCountry" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Lingua:<br />
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
                                <td class="td_title">
                                    Partita Iva:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docVat" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Codice Fiscale:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docCf" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Data nascita:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_birthDate" runat="server" Width="100px">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Luogo nascita:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_birthPlace" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Tipo:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_docType" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Numero:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docNum" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Rilasciato da:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docIssuePlace" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    in data:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_docIssueDate" runat="server" Width="100px">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Scadenza il:
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
                                <td class="td_title">
                                    E-mail:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contactEmail" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Telefono:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contactPhone" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Fax:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contactFax" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Web Address:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contactWebSite" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Come ha conosciuto?<br/>
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
                                <td class="td_title">
                                    Login:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_authUsr" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Password:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_authPwd" Width="200px" />
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
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td colspan="2">
                                    <asp:HiddenField ID="HfLang" Value="1" runat="server" />
                                    <asp:ListView ID="LvLangs" runat="server" OnItemCommand="LvLangs_ItemCommand">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLang" CommandName="change_lang" runat="server" CssClass='<%# HfLang.Value == "" + Eval("id") ? "tab_item_current" : "tab_item"%>'>
                                                <span>
                                                    <%# Eval("title") %></span>
                                            </asp:LinkButton>
                                            <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <div class="menu2">
                                                <a id="itemPlaceholder" runat="server" />
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Condizioni del servizio:
                                </td>
                                    <td>
                                    <telerik:RadEditor runat="server" StripFormattingOnPaste="AllExceptNewLines" ID="txt_condition"
                                        SkinID="DefaultSetOfTools" Height="400" Width="800" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                        <CssFiles>
                                            <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                        </CssFiles>
                                    </telerik:RadEditor>
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