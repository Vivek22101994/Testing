<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="cashBookPrintList.aspx.cs" Inherits="ModInvoice.admin.modInvoice.cashBookPrintList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Stampa Riepilogo Prima Nota</h1>
                <div class="nulla">
                </div>
                <div class="filt">
                    <div class="t">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                    <div class="c">
                        <div class="filtro_cont">
                            <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                <tr>
                                    <td>
                                        <telerik:RadAjaxPanel ID="rapFilter" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <label>Entrata/Uscita:</label>
                                                        <asp:DropDownList ID="drp_flt_cashInOut" runat="server" CssClass="inp" Style="width: 80px;">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">Entrata</asp:ListItem>
                                                            <asp:ListItem Value="0">Uscita</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <div class="nulla">
                                                        </div>
                                                        <label>Data da:</label>
                                                        <telerik:RadDatePicker ID="rdp_flt_cashDateFrom" runat="server" Width="100px" CssClass="inp">
                                                            <DateInput DateFormat="dd/MM/yyyy">
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                        <div class="nulla">
                                                        </div>
                                                        <label>Data a:</label>
                                                        <telerik:RadDatePicker ID="rdp_flt_cashDateTo" runat="server" Width="100px" CssClass="inp">
                                                            <DateInput DateFormat="dd/MM/yyyy">
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td style="width: 20px;">
                                                    </td>
                                                    <td>
                                                        <label>Escludi Causali:
                                                            <br />
                                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="select_docCaseId" Style="color: #E01E15; text-decoration: none;">escludi tutti</asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton3" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="deselect_docCaseId" Style="color: #E01E15; text-decoration: none;">includi tutti</asp:LinkButton>
                                                        </label>
                                                        <div class="nulla">
                                                        </div>
                                                        <div style="max-height: 150px; min-width: 200px; overflow-y: auto;">
                                                            <asp:CheckBoxList ID="chkList_flt_docCaseId" runat="server" CssClass="inp" TextAlign="Right" RepeatColumns="2" Style="margin: 0 5px 5px 0;">
                                                            </asp:CheckBoxList>
                                                            <div class="nulla">
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td style="width: 20px;">
                                                    </td>
                                                    <td>
                                                        <label>Escludi Movimenti su:
                                                            <br />
                                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton4" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="select_cashPlace" Style="color: #E01E15; text-decoration: none;">escludi tutti</asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton5" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="deselect_cashPlace" Style="color: #E01E15; text-decoration: none;">includi tutti</asp:LinkButton>
                                                        </label>
                                                        <div class="nulla">
                                                        </div>
                                                        <div style="max-height: 150px; min-width: 200px; overflow-y: auto;">
                                                            <asp:CheckBoxList ID="chkList_flt_cashPlace" runat="server" CssClass="inp" TextAlign="Right" RepeatColumns="2" Style="margin: 0 5px 5px 0;">
                                                            </asp:CheckBoxList>
                                                            <div class="nulla">
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td style="width: 20px;">
                                                    </td>
                                                    <td>
                                                        <label>Escludi Modalità:
                                                            <br />
                                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton6" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="select_cashType" Style="color: #E01E15; text-decoration: none;">escludi tutti</asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton7" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="deselect_cashType" Style="color: #E01E15; text-decoration: none;">includi tutti</asp:LinkButton>
                                                        </label>
                                                        <div class="nulla">
                                                        </div>
                                                        <div style="max-height: 150px; min-width: 200px; overflow-y: auto;">
                                                            <asp:CheckBoxList ID="chkList_flt_cashType" runat="server" CssClass="inp" TextAlign="Right" RepeatColumns="2" Style="margin: 0 5px 5px 0;">
                                                            </asp:CheckBoxList>
                                                            <div class="nulla">
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadAjaxPanel>
                                    </td>
                                    <td valign="bottom">
                                        <asp:LinkButton ID="lnkPreview" runat="server" CssClass="ricercaris" OnClick="lnkPreview_Click"><span>Anteprima Stampa</span></asp:LinkButton>
                                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="ricercaris" OnClick="lnkPrint_Click"><span>Scarica Pdf</span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="b">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
            <div style="clear: both" id="pnlPreview" runat="server" visible="false">
                <asp:Literal ID="ltrSrc" runat="server" Visible="false"></asp:Literal>
                <a href="<%=ltrSrc.Text%>" target="_blank" class="inlinebtn">Apri in nuova finestra</a>
                <br />
                <br />
                <iframe id="Iframe1" src="<%=ltrSrc.Text%>" style="width: 780px; height: 500px;"></iframe>
            </div>
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
