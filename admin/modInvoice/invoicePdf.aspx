<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="invoicePdf.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invoicePdf" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        @import url(/css/stylePdfInvoice.css);
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="HF_IdInvoice" runat="server" Value="0" Visible="false" />
    <asp:HiddenField ID="HF_pidLang" runat="server" Value="it" Visible="false" />
    <div style="margin: 0 auto; width: 960px; position: relative;">
        <div id="pagato" class="sopra" style="display: none; left: 374px;">
            <img src="/images/css/paid.png" alt="" style="width: 168px;" />
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
                <td valign="bottom" align="left" style="padding-bottom: 20px;">
                    Phone: +39069905199
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
                    <table cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <strong style="color: #fe6634;">
                                    <%= tblInvoice.ownerNameFull%></strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= tblInvoice.owner_locAddress%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= tblInvoice.owner_locZipCode + "&nbsp;" + tblInvoice.owner_locCity + "&nbsp;" + (tblInvoice.owner_locState != "" ? "(" + tblInvoice.owner_locState + ")" : "") + "&nbsp;" + tblInvoice.owner_locCountry%>
                            </td>
                        </tr>
                        <%= tblInvoice.owner_docCf != "" ? "<tr><td>Codice F.: "+tblInvoice.owner_docCf+"</td></tr>" : ""%>
                        <%= tblInvoice.owner_docCf != "" ? "<tr><td>P. Iva: "+tblInvoice.owner_docVat+"</td></tr>" : ""%>
                    </table>
                </td>
                <td colspan="2" bgcolor="#a09ea1" align="left" style="color: #FFF; width: 300px;">
                    <b>
                        Numero Nota di Credito:</b>
                </td>
                <td colspan="2" bgcolor="#eeeeee" align="right" style="width: 180px;">
                    <%= tblInvoice.docNum%>
                </td>
            </tr>
            <tr>
                <td colspan="2" bgcolor="#a09ea1" align="left" style="color: #FFF;">
                    <b>
                        Data Nota di Credito:</b>
                </td>
                <td colspan="2" bgcolor="#eeeeee" align="right">
                    <%= tblInvoice.docIssueDate.formatCustom("#dd#/#mm#/#yy#", 1, "")%>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table width="960" border="0" cellpadding="5" cellspacing="2">
            <tr>
                <td colspan="8" bgcolor="#54495a" style="color: #FFF;" align="left">
                    <strong>Descrizione</strong>
                </td>
                <td colspan="2" bgcolor="#54495a" style="color: #FFF;" align="left">
                    <strong>Prezzo(€)</strong>
                </td>
                <td colspan="2" bgcolor="#54495a" style="color: #FFF;" align="left">
                    <strong>Quantità</strong>
                </td>
                <td colspan="2" bgcolor="#54495a" style="color: #FFF;" align="right">
                    <strong>Totale(€)</strong>
                </td>
            </tr>
            <asp:Literal ID="ltr_invoiceItemTemplate" runat="server" Visible="false">
            <tr>
                <td colspan="8" bgcolor="#ffffff" align="left" >
                    #description#            
                </td>
                <td colspan="2" bgcolor="#ffffff" align="left" >
                    #singleUnitPrice#
                </td>
                <td colspan="2" bgcolor="#ffffff" align="left" >
                    #quantityAmount#
                </td>
                <td colspan="2" bgcolor="#ffffff" align="right" >
                    #cashTaxFree#
                </td>
            </tr>
            </asp:Literal>
            <%=  tblInvoice.itemListHTML(ltr_invoiceItemTemplate.Text)%>
        </table>
        <table cellpadding="0" cellspacing="0" class="boxFattura">
            <tr>
                <td valign="middle" align="left">
                    <strong>Imponibile:</strong>
                </td>
                <td valign="middle" align="right" style="border-right: none;">
                    <p>
                        <strong>
                            <%= tblInvoice.cashTaxFree %></strong>
                    </p>
                </td>
            </tr>
            <asp:Literal ID="ltr_taxListTemplate" runat="server" Visible="false">
             <tr>
                <td valign="middle" align="left">
                    <strong>#taxCode#:</strong>
                </td>
                <td valign="middle" align="right" style="border-right: none;">
                    <p><strong>#taxAmount#</strong></p>
                </td>
            </tr>
            </asp:Literal>
            <%=  tblInvoice.taxListHTML(ltr_taxListTemplate.Text)%>
            <tr>
                <td valign="middle" align="left" class="totFattura">
                    <strong>Importo Totale:</strong>
                </td>
                <td valign="middle" align="right" class="totFattura">
                    <strong>
                        <%= tblInvoice.cashTotalAmount %></strong>
                </td>
            </tr>
        </table>
        <div style="clear: both; width: 960px;">
        </div>
        <table width="960" cellpadding="20" cellspacing="0" border="0" style="border-top: 1px dotted #fe6634; margin-top: 25px;">
            <tr>
                <td style="padding: 20px; font-weight: bold;" valign="middle" align="left">
                    <%= tblInvoice.notesPublic %>
                </td>
            </tr>
        </table>
        <table width="960" cellpadding="20" cellspacing="0" border="0" style="margin-top: 25px;">
            <tr>
                <td style="background-color: #a09ea1; color: #FFF; padding: 20px;" valign="middle" align="right">
                    <b>www.rentalinrome.com</b>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
