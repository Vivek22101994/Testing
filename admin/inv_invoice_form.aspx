<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inv_invoice_form.aspx.cs" Inherits="RentalInRome.admin.inv_invoice_form" %>

<%@ Register Src="~/admin/uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_inv_invoice_client.ascx" TagName="UC_inv_invoice_client" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_inv_invoiceItems.ascx" TagName="UC_inv_invoiceItems" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_inv_invoiceNotes.ascx" TagName="UC_inv_invoiceNotes" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        @import url(/admin/css/adminStyle.css);
        @import url(/jquery/css/ui-core.css);
        @import url(/jquery/css/ui-datepicker.css);
        html, body { background-color: #FFF; }
    </style>
    <script src="../js/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <script src="../js/tiny_mce/init.js" type="text/javascript"></script>
    <script src="../jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../jquery/js/ui--core.min.js" type="text/javascript"></script>
    <script src="../jquery/js/ui-effects.min.js" type="text/javascript"></script>
    <script src="../jquery/js/ui-datepicker.min.js" type="text/javascript"></script>
    <script src="../jquery/datepicker-lang/jquery.ui.datepicker-it.js" type="text/javascript"></script>

    <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
    <script type="text/javascript">
        var cal_invoice_date_<%=Unique%>;
        function setCal_<%= Unique %>() {
            cal_invoice_date_<%=Unique%> = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%=HF_invoice_date.ClientID%>", View: "#txt_invoice_date", changeMonth: true, changeYear: true, yearRange: '2010:2030' });

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10">
            <ProgressTemplate>
                <uc1:UC_loader ID="UC_loader1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel_main" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="HF_id" runat="server" />
                <asp:HiddenField ID="HF_code" runat="server" Value="0" />
                <asp:HiddenField ID="HF_inv_dtInvoice" runat="server" Value="0" />
                <asp:HiddenField ID="HF_pr_total" runat="server" Value="0" />
                <asp:HiddenField ID="HF_pr_tf" runat="server" Value="0" />
                <asp:HiddenField ID="HF_pr_tax" runat="server" Value="0" />
                <asp:HiddenField ID="HF_is_complete" runat="server" Value="0" />
                <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
                <div id="main">
                    <span class="titlight">Dettagli della Fattura #<%= HF_code.Value%></span><div class="mainline">
                        <div class="prices">
                            <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                                <div id="Div2" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
                                </div>
                                <h3>Dati Documento
                                </h3>
                                <div class="price_div">
                                    <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 110px;"># Fattura
                                            </td>
                                            <td align="right">
                                                <%= HF_code.Value%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Data Fattura
                                            </td>
                                            <td align="right">
                                                <%=HF_inv_dtInvoice.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", 1, "")%>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class ="nulla" style="margin-top:20px;"></div>
                                    <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 110px;"># Fattura
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txt_invoice_number" runat="server"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:LinkButton ID="lnk_update_invoice_number" runat="server" OnClick="lnk_update_invoice_number_Click" CssClass="btn">Update</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Data Fattura
                                            </td>
                                            <td align="left">
                                                <input type="text" id="txt_invoice_date" />
                                                <asp:HiddenField ID="HF_invoice_date" runat="server" />
                                            </td>
                                            <td align="left">
                                                <asp:LinkButton ID="lnk_update_invoice_date" runat="server" OnClick="lnk_update_invoice_date_Click" CssClass="btn">Update</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                            <uc2:UC_inv_invoice_client ID="UC_cl" runat="server" />
                            <div class="nulla">
                            </div>
                            <uc2:UC_inv_invoiceItems ID="UC_items" runat="server" />
                            <div class="nulla">
                            </div>

                            <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                                <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
                                </div>
                                <h3>Importi</h3>
                                <div class="price_div">
                                    <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 110px;">Imponibile
                                            </td>
                                            <td align="right">
                                                <%=tblInvoice.pr_tf + "&nbsp;&euro;"%>
                                            </td>
                                        </tr>
                                        <asp:Literal ID="ltr_taxListTemplate" runat="server" Visible="false">
                                     <tr>
                                        <td valign="middle" align="left">
                                            Iva - #taxCode#%:
                                        </td>
                                        <td valign="middle" align="right" style="border-right: none;">
                                            #taxAmount#
                                        </td>
                                    </tr>
                                        </asp:Literal>
                                        <%=  tblInvoice.taxListHTML(ltr_taxListTemplate.Text)%>
                                        <tr>
                                            <td style="width: 110px;">Importo Fattura
                                            </td>
                                            <td align="right">
                                                <%=tblInvoice.pr_total + "&nbsp;&euro;"%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                            <uc2:UC_inv_invoiceNotes ID="UC_inv_invoiceNotes" runat="server" />
                            <div class="nulla">
                            </div>
                            <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                                <div class="price_div">
                                    <div class="btnric" style="float: left; margin: 5px 20px;" id="pnl_lnkUpdate" runat="server">
                                        <asp:LinkButton ID="lnkUpdate" runat="server" OnClick="lnkUpdate_Click" OnClientClick="return confirm('Stai per aggiornare i dati del cliente?');" ToolTip="Aggiorna i dati del cliente dalla prenotazione"><span>Aggiorna dalla pren</span></asp:LinkButton>
                                    </div>
                                    <div id="Div1" class="btnric" style="float: left; margin: 5px 20px;" runat="server">
                                        <a href="javascript:parent.Shadowbox.close();"><span>chiudi</span></a>
                                    </div>
                                    <div class="btnric" style="float: left; margin: 5px 20px;">
                                        <asp:HyperLink ID="HL_view" runat="server" Target="_blank" Enabled="false">
                                        <span>
                                        Anteprima
                                        </span>
                                        </asp:HyperLink>
                                    </div>
                                    <div class="btnric" style="float: left; margin: 5px 20px;">
                                        <asp:HyperLink ID="HL_pdf" runat="server" Target="_blank" Enabled="false">
                                        <span>
                                        PDF
                                        </span>
                                        </asp:HyperLink>
                                    </div>
                                    <div class="btnric" style="float: left; margin: 5px 20px;" id="pnl_lnkNotaDiCredito" runat="server">
                                        <asp:LinkButton ID="lnkNotaDiCredito" runat="server" OnClick="lnkNotaDiCredito_Click" OnClientClick="return confirm('Stai per creare o aggiornare la note di credito?');" ToolTip=""></asp:LinkButton>
                                    </div>
                                    <div class="btnric" style="float: left; margin: 5px 20px;">
                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return confirm('Stai per cancellare la fattura senza la possibilita di ripristinare?');"><span>Cancella</span></asp:LinkButton>
                                    </div>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
