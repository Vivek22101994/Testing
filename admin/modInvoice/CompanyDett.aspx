<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="CompanyDett.aspx.cs" Inherits="ModInvoice.admin.modInvoice.CompanyDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../modContent/UCgetImg.ascx" TagName="UCgetImg" TagPrefix="getImg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <title>MagaRentalCE - <%=ltrTitle.Text %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <asp:HiddenField ID="HF_reloadDrp" Value="0" runat="server" />
        <h1 class="titolo_main">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
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
                                    Attivo?:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isActive" runat="server">
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Prenotazioni private:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isForReservations" runat="server" style="display: inline-block; width: auto; float:none;">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;Iva:&nbsp;<asp:DropDownList ID="drp_invTaxIdPrivate" runat="server" Style="display: inline-block; width: auto; float: none;">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Agenzie:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isForAgencies" runat="server" Style="display: inline-block; width: auto; float: none;">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;Iva:&nbsp;<asp:DropDownList ID="drp_invTaxIdAgency" runat="server" Style="display: inline-block; width: auto; float: none;">
                                   </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Nome Societa:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_nameFull" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_nameFull" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                   Codice Destinatario:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_id_codice" Width="200px" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_nameFull" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>--%>
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
                                <td colspan="3">
                                    Nazione/Location:<br />
                                    <asp:DropDownList ID="drp_locCountry" runat="server" Width="300px">
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
                                    Data nascita:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_birthDate" runat="server" Width="100px" MinDate="1900-01-01">
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
                                    Telefono:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contactPhoneMobile" Width="200px" />
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
                    <span class="titoloboxmodulo">Immagine anteprima</span>
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
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
