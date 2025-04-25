<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="invoiceDett.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invoiceDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <title><%=ltrTitle.Text %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <asp:HiddenField ID="HF_taxAmount" runat="server" Value="0" />
        <asp:HiddenField ID="HF_cashTaxFree" runat="server" Value="0" />
        <asp:HiddenField ID="HF_cashTaxAmount" runat="server" Value="0" />
        <asp:HiddenField ID="HF_cashTotalAmount" runat="server" Value="0" />
        <h1 class="titolo_main">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCalculate" runat="server" OnClick="lnkCalculate_Click"><span>Calcola Totali</span></asp:LinkButton></div>
            <div class="bottom_salva" id="pnl_btnSave" runat="server" visible="false">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Crea Fattura</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');">
                    <span>Chiudi</span></a>
            </div>
            <div class="bottom_salva">
                <asp:HyperLink ID="HL_pdfView" runat="server" Target="_blank" Visible="false"><span>Anteprima Pdf</span></asp:HyperLink>
            </div>
            <div class="bottom_salva">
                <asp:HyperLink ID="HL_pdfGet" runat="server" Target="_blank" Visible="false"><span>Scarica Pdf</span></asp:HyperLink>
            </div>
            <div class="bottom_salva">
                <asp:HyperLink ID="HL_cashDoc" runat="server" Visible="false"><span>Documento Prima Nota</span></asp:HyperLink>
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
                    <div class="boxmodulo" id="pnl_error" runat="server" visible="false">
                        <div style="border: 1px solid red; padding: 5px; margin-bottom: 10px;">
                            <span class="titoloboxmodulo" style="border: medium none navy; margin: 0pt;">Attenzione</span>
                            <asp:Literal ID="ltr_error" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <span class="titoloboxmodulo">Oggetti della fattura</span>
                    <div class="boxmodulo">
                        <table border="0" cellpadding="0" cellspacing="0" style="">
                            <tr style="text-align: left">
                                <th style="width: 20px"># </th>
                                <th style="width: 400px">Descrizione </th>
                                <th style="width: 50px; text-align: center;">Quantità </th>
                                <th style="width: 120px; text-align: center;">Prezzo Singolo<br />
                                    (Senza Iva) </th>
                                <th style="width: 80px; text-align: center;">Iva Applicata</th>
                                <th style="width: 120px; text-align: center;">Prezzo Totale Ivato</th>
                                <th></th>
                            </tr>
                            <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <span>
                                                <%# Eval("sequence") %></span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_description" runat="server" Text='<%# Eval("description") %>' Width="390"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:TextBox ID="txt_quantity" runat="server" Text='<%# Eval("quantityAmount") %>' Width="30"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("singleUnitPrice").objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:DropDownList ID="drp_cashTaxID" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:TextBox ID="txt_cashTotalAmount" runat="server" Text='<%# Eval("cashTotalAmount") %>' Width="100"></asp:TextBox>&nbsp;&euro;
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                            <asp:Label ID="lbl_sequence" Visible="false" runat="server" Text='<%# Eval("sequence") %>' />
                                            <asp:Label ID="lbl_cashTaxID" Visible="false" runat="server" Text='<%# Eval("cashTaxID") %>' />
                                            <asp:LinkButton ID="lnk_del" runat="server" CssClass="del" CommandName="del" OnClientClick="return confirm('sta per eliminare oggetto della fattura?')"> elimina</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td colspan="5">
                                            Non sono presenti Oggetti della fattura!
                                            <asp:LinkButton ID="lnk_add_new" runat="server" OnClick="lnk_add_new_Click">Aggiungi</asp:LinkButton>
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <tr id="itemPlaceholder" runat="server" />
                                    <tr>
                                        <td colspan="5">
                                            <asp:LinkButton ID="lnk_add_new" runat="server" OnClick="lnk_add_new_Click">Aggiungi nuovo Oggetto</asp:LinkButton>
                                        </td>
                                    </tr>
                                </LayoutTemplate>
                            </asp:ListView>
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
                    <span class="titoloboxmodulo">Dati Intestatario</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">
                                    Tipo Anagrafica:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_ownerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_ownerType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    al nome di:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_owner" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:LinkButton ID="lnk_ownerSelect" runat="server" OnClick="lnk_ownerSelect_Click">Aggiorna dall'Anagrafica</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Nome Completo:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_ownerNameFull" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_ownerNameFull" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Partita Iva:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_owner_docVat" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Codice Fiscale:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_owner_docCf" Width="200px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <span class="titoloboxmodulo" style="margin-top: 20px; margin-bottom: 0;">Residenza / Sede amministrativa</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td colspan="3">
                                    Indirizzo:<br />
                                    <asp:TextBox runat="server" ID="txt_owner_locAddress" Width="300px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_owner_locAddress" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    CAP:<br />
                                    <asp:TextBox runat="server" ID="txt_owner_locZipCode" Width="100px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_owner_locZipCode" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Città:<br />
                                    <asp:TextBox runat="server" ID="txt_owner_locCity" Width="100px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_owner_locCity" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Provincia/Stato:<br />
                                    <asp:TextBox runat="server" ID="txt_owner_locState" Width="100px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_owner_locState" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    Nazione/Location:<br />
                                    <asp:DropDownList ID="drp_owner_locCountry" runat="server" Width="300px">
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
                    <span class="titoloboxmodulo">Dati Documento</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">
                                    Attivo:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_cashInOut" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_cashInOut_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Passivo (Nota di credito)</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Causale:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_docCase" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Numero doc.:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docNum" Width="200px" ReadOnly="true" />
                                    <asp:TextBox runat="server" ID="txt_docType" Text="notaDiCredito" Width="200px" ReadOnly="true" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Data doc.:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_docIssueDate" runat="server">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rdp_docIssueDate" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Scadenza doc.:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_docExpiryDate" runat="server">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="rdp_docExpiryDate" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Importo da ivare:
                                </td>
                                <td>
                                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                        <%= HF_cashTaxFree.Value + "&nbsp;&euro;"%>
                                    </telerik:RadCodeBlock>
                                </td>
                            </tr>
                            <asp:Literal ID="ltr_taxListTemplate" runat="server" Visible="false">
                                <tr>
                                    <td class="td_title">
                                        #taxCode#:
                                    </td>
                                    <td>
                                        #taxAmount#&nbsp;&euro;
                                    </td>
                                </tr>
                            </asp:Literal>
                            <asp:Literal ID="ltr_taxList" runat="server"></asp:Literal>
                            <tr>
                                <td class="td_title">
                                    Importo totale:
                                </td>
                                <td>
                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                        <%= HF_cashTotalAmount.Value+"&nbsp;&euro;" %>
                                    </telerik:RadCodeBlock>
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
                    <span class="titoloboxmodulo">Note sul PDF</span>
                    <div class="boxmodulo">
                        <telerik:RadEditor runat="server" ID="re_notesPublic" SkinID="DefaultSetOfTools" Height="200" Width="600" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
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
