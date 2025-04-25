<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="invCashTaxEdit.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invCashTaxEdit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HFIdItem" Value="0" runat="server" />
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Gestione Modalità Pagamento </h1>
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
                                    <%# decimal.Multiply(Eval("taxAmount").objToDecimal(), 100).objToInt32()%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isPossibleVatFreeInvoice").objToInt32()==1?"SI":"NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
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
                                    <%# decimal.Multiply(Eval("taxAmount").objToDecimal(), 100).objToInt32()%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isPossibleVatFreeInvoice").objToInt32()==1?"SI":"NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" style="">
                            <tr>
                                <td>No data
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="table_fascia">
                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                <tr style="text-align: left">
                                    <th style="width: 30px">ID </th>
                                    <th style="width: 200px">Nome </th>
                                    <th style="width: 50px">Iva </th>
                                    <th style="width: auto;">Possibilità di emettere fatture Iva esente?</th>
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
                                                <td class="td_title">Nome:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_code" Width="300px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Iva:
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="ntxt_taxAmount" runat="server" Width="50" Type="Percent">
                                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                    </telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td class="td_title">Possibilità di emettere fatture Iva esente?
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_possibleVatFreeInvoice" runat="server">
                                                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>--%>
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
