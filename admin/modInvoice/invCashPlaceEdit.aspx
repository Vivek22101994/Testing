<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="invCashPlaceEdit.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invCashPlaceEdit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HFIdItem" Value="0" runat="server" />
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Gestione Posti di movimento</h1>
                <div class="bottom_agg">
                    <asp:LinkButton ID="lnkNew" runat="server" OnClick="lnkNew_Click"><span>+ Nuovo</span></asp:LinkButton>
                </div>
                <div id="Div1" class="bottom_agg" runat="server" visible="false">
                    <a onclick="return CloseRadWindow('reload');">
                        <span>Chiudi</span></a>
                </div>
            </div>
            <div style="clear: both">
                <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("code")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("title") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("type")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("chargeFeePart1")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("chargeFeePart2")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("chargeFeeFull")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("chargeFeeInvoice") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("code")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("title") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("type")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("chargeFeePart1")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("chargeFeePart2")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("chargeFeeFull")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("chargeFeeInvoice") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" style="">
                            <tr>
                                <td>
                                    No data
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="table_fascia">
                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                <tr style="text-align: left">
                                    <th style="width: 30px">ID </th>
                                    <th style="width: 50px">Codice </th>
                                    <th style="width: 200px">Movimento su</th>
                                    <th style="width: 200px">Tipo Movimento</th>
                                    <th style="width: 50px">Attivo? </th>
                                    <th style="width: 50px">Fee Acconto </th>
                                    <th style="width: 50px">Fee Saldo </th>
                                    <th style="width: 50px">Fee Intero </th>
                                    <th style="width: 50px">Fee in fattura? </th>
                                    <th></th>
                                    <th></th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server" />
                            </table>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
            <telerik:RadWindow runat="server" ID="rwdDett" Modal="true" AutoSize="true" ShowContentDuringLoad="false" MinWidth="450" OnClientClose="rwdDett_OnClientClose" VisibleStatusbar="false" VisibleTitlebar="false">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
                        <div class="mainline">
                            <div class="mainbox">
                                <div class="top">
                                    <div class="sx">
                                    </div>
                                    <div class="dx">
                                    </div>
                                </div>
                                <div class="center">
                                    <div class="boxmodulo">
                                        <table>
                                            <tr>
                                                <td class="td_title">
                                                    Codice:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_code" Width="300px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Movimento su:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_title" Width="300px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Tipo Movimento:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_type" runat="server">
                                                        <asp:ListItem Value="cash" Text="Cash"></asp:ListItem>
                                                        <asp:ListItem Value="bank" Text="Bank"></asp:ListItem>
                                                        <asp:ListItem Value="paypal" Text="PayPal"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Fee Acconto:
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="ntxt_chargeFeePart1" runat="server" Width="50" Type="Percent" Style="text-align: right;">
                                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                                    </telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Fee Saldo:
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="ntxt_chargeFeePart2" runat="server" Width="50" Type="Percent" Style="text-align: right;">
                                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                                    </telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Fee Intero:
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="ntxt_chargeFeeFull" runat="server" Width="50" Type="Percent" Style="text-align: right;">
                                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                                    </telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Fee in fattura?
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_chargeFeeInvoice" runat="server">
                                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Desc Fee in Fattura:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_chargeFeeInvoiceDesc" Width="300px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Iva del Fee:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_chargeFeeTaxId" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Attivo?
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_isActive" runat="server">
                                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                                        <asp:ListItem Value="0">NO</asp:ListItem>
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
                            <div class="salvataggio">
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton>
                                </div>
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                                </div>
                                <div class="bottom_salva">
                                    <a onclick="return rwdDettClose();">
                                        <span>Chiudi</span></a>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                        </div>
                    </telerik:RadAjaxPanel>
                </ContentTemplate>
            </telerik:RadWindow>
        </telerik:RadAjaxPanel>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function rwdDettClose() {
                $find("<%= pnlFascia.ClientID%>").ajaxRequest('rwdDett_Closing');
                //RadAjaxManager_ajaxRequest('rwdDett_Closing');
                return false;
            }
            function rwdDett_OnClientClose(sender, eventArgs) {
                return rwdDettClose();
            } 
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
