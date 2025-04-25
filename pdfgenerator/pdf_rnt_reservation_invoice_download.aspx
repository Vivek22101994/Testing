<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pdf_rnt_reservation_invoice_download.aspx.cs" Inherits="RentalInRome.pdfgenerator.pdf_rnt_reservation_invoice_download" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        @import url(/css/style_pdf.css);
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="HF_IdInvoice" runat="server" Value="0" Visible="false" />
        <asp:HiddenField ID="HF_IdReservation" runat="server" Value="0" Visible="false" />
        <asp:HiddenField ID="HF_IdClient" runat="server" Value="0" Visible="false" />
        <asp:HiddenField ID="HF_IdEstate" runat="server" Value="0" Visible="false" />
        <asp:HiddenField ID="HF_pid_lang" runat="server" Value="2" Visible="false" />
        <div style="margin: 0 auto; width: 960px; position: relative;">
            <div id="pagato" class="sopra" style="display: block; left: 374px;">
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/paid.png" alt="" style="width: 168px;" />
            </div>

            <table width="960" border="0" cellpadding="5" cellspacing="2" style="border-bottom: 1px dotted #fe6634; margin-bottom: 20px;">
                <tr>
                    <td valign="bottom" align="left" style="padding-bottom: 20px; font-size: 12px; color: #666; line-height: 14px;">
                        <strong>Rental In Rome S.r.l.</strong><br />
                        <br />
                        Via Appia Nuova, 677
        <br />
                        Rome, Italy 00179
        <br />
                        Italy
        
    
                    </td>
                    <td valign="bottom" align="left" style="padding-bottom: 20px;">Phone: +39 06 3220068
            <br />
                        VAT ID: 07824541002
                    </td>
                    <td valign="middle" align="right" style="padding-bottom: 20px; width: 600px;">
                        <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/logo.gif" alt="" />
                    </td>
                </tr>
            </table>
            <table width="960" border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <td colspan="6" rowspan="3" valign="top" style="width: 480px; font-size: 18px;">
                        <%= tblInvoice.clientDetailsHTML()%>
                    </td>
                    <td colspan="2" bgcolor="#a09ea1" align="left" style="color: #FFF; width: 300px;">
                        <b><%=CurrentSource.getSysLangValue("pdf_InvoiceNumber", "Invoice #")%>:</b>
                    </td>
                    <td colspan="2" bgcolor="#eeeeee" align="right" style="width: 180px;">
                        <%= tblInvoice.code %>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" bgcolor="#a09ea1" align="left" style="color: #FFF;">
                        <b><%=CurrentSource.getSysLangValue("lblDate", "Date")%>:</b>
                    </td>
                    <td colspan="2" bgcolor="#eeeeee" align="right">
                        <%= tblInvoice.inv_dtInvoice.formatCustom("#MM# #dd#, #yy#", _currLang, "") %>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" bgcolor="#a09ea1" align="left" style="color: #FFF;">
                        <b><%=CurrentSource.getSysLangValue("pdf_InvoiceAmountDueEUR", "Amount Due EUR")%>:</b>
                    </td>
                    <td colspan="2" bgcolor="#eeeeee" align="right">
                        <%= "&euro;&nbsp;0,00" %>
                    </td>
                </tr>
            </table>

            <br />

            <br />
            <table width="960" border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <td bgcolor="#54495a" style="color: #FFF;" colspan="3" align="left">
                        <strong>
                            <%=CurrentSource.getSysLangValue("pdf_InvoiceItem", "Item")%></strong>
                    </td>
                    <td colspan="8" bgcolor="#54495a" style="color: #FFF;" align="left">
                        <strong>
                            <%=CurrentSource.getSysLangValue("pdf_InvoiceDescription", "Description")%></strong>
                    </td>
                    <td colspan="2" bgcolor="#54495a" style="color: #FFF;" align="left">
                        <strong>
                            <%=CurrentSource.getSysLangValue("pdf_InvoiceUnitCost", "Unit Cost")%> (€)</strong>
                    </td>
                    <td colspan="2" bgcolor="#54495a" style="color: #FFF;" align="left">
                        <strong>
                            <%=CurrentSource.getSysLangValue("pdf_InvoiceQuantity", "Quantity")%></strong>
                    </td>
                    <td colspan="2" bgcolor="#54495a" style="color: #FFF;" align="right">
                        <strong>
                            <%=CurrentSource.getSysLangValue("lblPrice", "Price")%> (€)</strong>
                    </td>
                </tr>
                <asp:Literal ID="ltr_invoiceItemTemplate" runat="server" Visible="false">
        <tr>
            <td colspan="3" bgcolor="#ffffff" align="left" >
                #code#
            </td>
            <td colspan="8" bgcolor="#ffffff" align="left" >
                #description#            
            </td>
            <td colspan="2" bgcolor="#ffffff" align="left" >
                #price_unit#
            </td>
            <td colspan="2" bgcolor="#ffffff" align="left" >
                #quantity#
            </td>
            <td colspan="2" bgcolor="#ffffff" align="right" >
                #price_tf#
            </td>
        </tr>
                </asp:Literal>
                <%=  tblInvoice.itemsListHTML(ltr_invoiceItemTemplate.Text)%>
            </table>
            <table cellpadding="0" cellspacing="0" class="boxFattura">
                <tr>

                    <td valign="middle" align="left">
                        <strong>
                            <%=CurrentSource.getSysLangValue("pdf_InvoiceSubtotal", "Subtotal")%>:</strong>
                    </td>
                    <td valign="middle" align="right" style="border-right: none;">
                        <p>
                            <strong>
                                <%= tblInvoice.pr_tf %></strong>
                        </p>
                    </td>
                </tr>
                <asp:Literal ID="ltr_taxListTemplate" runat="server" Visible="false">
        <tr>
            <td valign="middle" align="left">
                VAT - #taxPercent#%: <br/><i>#taxCode#</i>
            </td>
            <td valign="middle" align="right" style="border-right: none;">
                <p>#taxAmount#</p>
            </td>
        </tr>
                </asp:Literal>
                <%=  tblInvoice.taxListHTML(ltr_taxListTemplate.Text)%>

                <tr>
                    <td valign="middle" align="left">
                        <p>
                            <strong>
                                <%=CurrentSource.getSysLangValue("pdf_InvoiceTotal", "Total")%>:<br />
                                <%=CurrentSource.getSysLangValue("pdf_InvoiceAmountPaid", "Amount Paid")%>:</strong>
                        </p>
                    </td>
                    <td valign="middle" align="right">
                        <strong>
                            <%= tblInvoice.pr_total %></strong><br />
                        <strong>-<%= tblInvoice.pr_total %></strong>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left" style="border-bottom: none;">
                        <%=CurrentSource.getSysLangValue("pdf_PaymentType", "Payment Type")%>:<br />
                    </td>
                    <td valign="middle" align="right" style="border-right: none; border-bottom: none">
                        <strong>
                            <asp:Literal ID="ltr_paymentType" runat="server"></asp:Literal>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left" class="totFattura">
                        <strong>
                            <%=CurrentSource.getSysLangValue("pdf_InvoiceBalanceDueEUR", "Balance Due EUR")%>:</strong>
                    </td>
                    <td valign="middle" align="right" class="totFattura">
                        <strong>€ 0.00</strong>
                    </td>
                </tr>
            </table>
            <table width="960" cellpadding="20" cellspacing="0" border="0" style="margin-top: 25px;">
                <tr>
                    <td style="background-color: #a09ea1; color: #FFF; padding: 20px;" valign="middle" align="right">
                        <b>www.rentalinrome.com </b>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
