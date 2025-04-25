<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="invoicePrintList.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invoicePrintList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Stampa Riepilogo Fatture </h1>
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
                                                        <label>Attivo/Passivo:</label>
                                                        <asp:DropDownList ID="drp_flt_cashInOut" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">Attivo (Entrata)</asp:ListItem>
                                                            <asp:ListItem Value="0">Passivo (Uscita)</asp:ListItem>
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
                                                    <td style="width:20px;"></td>
                                                    <td>
                                                        <label>
                                                            Escludi Anagrafica: 
                                                            <br />
                                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnk_chkList_ownerIdsSelectAll" runat="server" OnClick="lnk_chkList_ownerIdsSelectAll_Click" CommandArgument="select" Style="color: #E01E15; text-decoration: none;">escludi tutti</asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnk_chkList_ownerIdsSelectAll_Click" Style="color: #E01E15; text-decoration: none;">includi tutti</asp:LinkButton>
                                                        </label>
                                                        <div class="nulla">
                                                        </div>
                                                        <div style="max-height: 150px; min-width: 400px; overflow-y: auto;">
                                                            <asp:CheckBoxList ID="chkList_ownerIds" runat="server" RepeatColumns="2">
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
</asp:Content>
