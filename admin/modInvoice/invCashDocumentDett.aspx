<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="invCashDocumentDett.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invCashDocumentDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="~/admin/modContent/UCgetFile.ascx" tagname="UCgetFile" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <title>RentalInRome -
        <%=ltrTitle.Text %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var rwdUrl = null;
            function createNewClient() {
                if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

                url = "/admin/modAuth/clientDett.aspx?reloadDrp=1&type=" + $("#<%= drp_ownerType.ClientID%>").val();
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(700);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                rwdUrl.maximize();
                return false;
            }
            function rwdUrl_OnClientClose(sender, eventArgs) {
                //alert('' + eventArgs.get_argument());
                $find('<%= rapOwnerCont.ClientID %>').ajaxRequest('' + eventArgs.get_argument());
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <asp:HiddenField ID="HfcashPayed" Value="0" runat="server" />
        <asp:HiddenField ID="HfInvoiceID" Value="0" runat="server" />
        <h1 class="titolo_main"><asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>
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
            <div class="bottom_salva">
                <asp:HyperLink ID="HL_invoiceDett" runat="server" Visible="false"><span>Modifica la Fattura</span></asp:HyperLink>
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
                    <span class="titoloboxmodulo">Dati Documento</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">
                                    Attivo/Passivo:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_cashInOut" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_cashInOut_SelectedIndexChanged">
                                        <asp:ListItem Value="1">Attivo (Entrata)</asp:ListItem>
                                        <asp:ListItem Value="0">Passivo (Uscita)</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Numero doc.:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_docNum" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_docNum" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Data doc.:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_docIssueDate" runat="server">
                                        <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rdp_docIssueDate" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Importo totale:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_cashAmount" Width="100px" />&nbsp;&euro;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_cashAmount" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_cashAmount" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// formato non valido (es: 120 or 123,07)" ValidationExpression="(^(-)?\d+(\,\d\d)?$)"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_cashAmount" CssClass="errore_form" ValidationGroup="BookNew_fullCash" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_cashAmount" CssClass="errore_form" ValidationGroup="BookNew_fullCash" Display="Dynamic" ErrorMessage="<br/>// formato non valido (es: 120 or 123,07)" ValidationExpression="(^(-)?\d+(\,\d\d)?$)"></asp:RegularExpressionValidator>
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
                                    <telerik:RadAjaxPanel ID="rapOwnerCont" runat="server" OnAjaxRequest="rapOwnerCont_AjaxRequest">
                                        <asp:DropDownList ID="drp_owner" runat="server">
                                        </asp:DropDownList>
                                        <img onclick="createNewClient()" alt="agg." title="Aggiungi nuovo" src="/images/ico/agg.gif" style="cursor: pointer; height: 15px;" />
                                    </telerik:RadAjaxPanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="center" id="pnl_docPath" runat="server">
                    <span class="titoloboxmodulo" style="margin-top: 20px; margin-bottom: 0pt;">PDF del Documento</span>
                    <div class="boxmodulo">
                        <uc1:UCgetFile ID="docPath" runat="server" />
                    </div>
                </div>
                <div class="center" id="pnl_docFiles" runat="server">
                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                    <span class="titoloboxmodulo" style="margin-top: 20px; margin-bottom: 0pt;">Documenti legati</span>
                    <div class="boxmodulo">
                        <asp:HiddenField ID="HF_currFileUid" runat="server" Visible="false" />
                        <table border="0" cellpadding="0" cellspacing="0" style="">
                            <asp:ListView ID="LVfiles" runat="server" OnItemCommand="LVfiles_ItemCommand">
                            <ItemTemplate>
                                <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                    <td>
                                        <%# "" + Eval("docPath") == "" ? "" : "<a href=\"/" + Eval("docPath") + "\" target=\"_blank\" title=\"Scarica Documento\" style=\"text-decoration: none; border: 0 none; margin: 5px;\">" + System.IO.Path.GetExtension("" + Eval("docPath")).Replace(".", "") + " </a>"%>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("docNum")  %></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("uid") %>' Visible="false"></asp:Label>
                                        <asp:ImageButton ID="ibtn_up" runat="server" ToolTip="Su" AlternateText="Su" CommandName="move_up" ImageUrl="~/images/ico/Go_up.png" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="ibtn_down" runat="server" ToolTip="Giù" AlternateText="Giù" CommandName="move_down" ImageUrl="~/images/ico/Go_down.png" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnk_change" CommandName="change" ToolTip="Modifica" runat="server"><span style="color:#333366; margin:0;">Modifica</span></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnk_delete" CommandName="elimina" ToolTip="Elimina" runat="server" OnClientClick="return confirm('Vuoi eliminare documento?')"><span style="color:#333366; margin:0;">Elimina</span></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <tr>
                                    <td colspan="4">
                                        Non ci sono documenti legati
                                    </td>
                                </tr>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                    <tr style="text-align: left">
                                        <th style="width: 50px;">Tipo</th>
                                        <th style="width: 400px;">Nome</th>
                                        <th></th>
                                        <th></th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server" />
                            </LayoutTemplate>
                            </asp:ListView>
                            <tr>
                                <td colspan="4">
                                    <asp:LinkButton ID="lnk_fileNew" runat="server" CssClass="inlinebtn" OnClick="lnk_fileNew_Click">Aggiungi Documento</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="boxmodulo" id="pnl_fileEdit" runat="server" style="border: 1px dotted; margin-top: 10px;">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr><th colspan="2">Documento Legato</th></tr>
                            <tr>
                                <td class="td_title">
                                    Nome
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fileDocName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <uc1:UCgetFile ID="fileDocPath" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    
                                    <asp:LinkButton ID="lnk_fileSave" runat="server" CssClass="inlinebtn" OnClick="lnk_fileSave_Click">Salva</asp:LinkButton>
                                    <asp:LinkButton ID="lnk_fileCancel" runat="server" CssClass="inlinebtn" OnClick="lnk_fileCancel_Click">Annulla</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    </telerik:RadAjaxPanel>
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
                <asp:ListView ID="LV_cashBook" runat="server" onitemcommand="LV_cashBook_ItemCommand">
                    <ItemTemplate>
                        <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id")%>'></asp:Label>
                        <tr>
                            <td style="width:120px;">
                                <strong>Codice Movimento:</strong>
                            </td>
                            <td>
                                <strong>
                                    <%# Eval("code") %></strong>
                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="del" OnClientClick="return confirm('Stai per eliminare il pagamento?');" Style="color: #E01E15; margin-left: 20px; text-decoration: none;">Elimina</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_title">
                                Movimento su
                            </td>
                            <td>
                                <%# invUtils.getCashPlace_title(Eval("cashPlace").objToInt32(), "-non definito-")%>
                            </td>
                        </tr>
                        <tr class="alternate">
                            <td class="td_title">
                                Tipo Movimento:
                            </td>
                            <td>
                                <%# invUtils.getCashType_title(Eval("cashType").objToInt32(), "-non definito-")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_title">
                                Data Movimento:
                            </td>
                            <td>
                                <%# ((DateTime?)Eval("cashDate")).formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----") + "&nbsp;Ore&nbsp;" + ((DateTime)Eval("cashDate")).TimeOfDay.JSTime_toString(false, true)%>
                            </td>
                        </tr>
                        <tr class="alternate">
                            <td class="td_title">
                                Importo Movimento:
                            </td>
                            <td>
                                <%# Eval("cashAmount").objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_title">
                                Registrato da:
                            </td>
                            <td>
                                <%# Eval("createdUserNameFull")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_title">
                                in data:
                            </td>
                            <td>
                                <%# Eval("createdDate")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="border-top: 1px dotted;">
                                &nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="center">
                            <span class="titoloboxmodulo">Movimenti / Pagamenti Registrati</span>
                            <div class="boxmodulo">
                                <table border="0" cellpadding="0" cellspacing="0" style="">
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </div>
                            <asp:Label ID="lblPayed" runat="server" CssClass="titoloboxmodulo" Visible="false">! Pagato !</asp:Label>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
                <div class="nulla">
                </div>
                <div class="center" id="pnl_cashBookNew" runat="server">
                    <span class="titoloboxmodulo">Registra Nuovo Movimento / Pagamento</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">
                                    Movimento su
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_BookNew_cashPlace" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_BookNew_cashPlace_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Tipo Movimento:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_BookNew_cashType" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Data Movimento:
                                </td>
                                <td>
                                    <telerik:RadDateTimePicker ID="rdtp_BookNew_cashDate" runat="server">
                                        <DateInput DateFormat="dd/MM/yyyy HH:mm">
                                        </DateInput>
                                    </telerik:RadDateTimePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="rdtp_BookNew_cashDate" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Importo Movimento:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_BookNew_cashAmount" Width="100px" />&nbsp;&euro;&nbsp;
                                    <asp:LinkButton ID="lnk_BookNew_fullCash" runat="server" ValidationGroup="BookNew_fullCash" OnClick="lnk_BookNew_fullCash_Click">Intero</asp:LinkButton>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_BookNew_cashAmount" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_BookNew_cashAmount" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// formato non valido (es: 120 or 123,07)" ValidationExpression="(^(-)?\d+(\,\d\d)?$)"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="nulla">
                </div>
                <div class="center" id="pnl_cashBookHandle" runat="server">
                    <div class="boxmodulo">
                        <div class="salvataggio">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_BookNew_show" runat="server" OnClick="lnk_BookNew_show_Click"><span>+ Nuovo Movimento</span></asp:LinkButton>
                            </div>
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_BookNew_hide" runat="server" OnClick="lnk_BookNew_hide_Click"><span>Annulla Movimento</span></asp:LinkButton>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="nulla">
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
                    <span class="titoloboxmodulo">Descrizione</span>
                    <div class="boxmodulo">
                        <telerik:RadEditor runat="server" ID="re_docBody" SkinID="DefaultSetOfTools" Height="300" Width="450" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
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
