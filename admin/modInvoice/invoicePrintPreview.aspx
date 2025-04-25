<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="invoicePrintPreview.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invoicePrintPreview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body { font-size: 12px;}
        .txt-l { font-size: 1.4em;}
        .txt-xl { font-size: 1.6em;}
        table { width: 100%;}
        th { text-align: left;}
        th, td { padding-top: 1px; padding-bottom: 1px;}
        .align_center { text-align: center;}
        .align_right { text-align: right;}
        .line_bottom { border-bottom: 1px solid #0000ff; padding-bottom: 2px; }
        .line_top { border-top: 1px solid #0000ff; padding-top: 2px; }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width: 750px;">
    <table cellspacing="0" cellpadding="0" style="width: 740px;">
        <tr>
            <th class="line_bottom align_center txt-xl">
                Riepilogo Fatture
            </th>
        </tr>
        <tr>
            <th class="align_center txt-l">
                <asp:Literal ID="ltrFilter" runat="server"></asp:Literal>
            </th>
        </tr>
        <asp:ListView ID="LVowner" runat="server">
            <ItemTemplate>
                <tr>
                    <th>
                        <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                        <span class="line_bottom txt-l">
                            <%# Eval("nameFull")%>
                        </span>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:ListView ID="LVlist" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="line_separator">
                                        <%# Eval("docNum")%>
                                    </td>
                                    <td class="line_separator">
                                        <%# ((DateTime?)Eval("docIssueDate")).formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----")%>
                                    </td>
                                    <td class="align_right line_separator">
                                        <%# "&euro;&nbsp;" + Eval("cashTaxFree").objToDecimal().ToString("N2")%>
                                    </td>
                                    <td class="align_right line_separator">
                                        <%# "&euro;&nbsp;" + Eval("cashTaxAmount").objToDecimal().ToString("N2")%>
                                    </td>
                                    <td class="align_right line_separator">
                                        <%# "&euro;&nbsp;" + Eval("cashTotalAmount").objToDecimal().ToString("N2")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <th class="line_bottom" style="width:120px;">
                                            Num. Fattura
                                        </th>
                                        <th class="line_bottom" style="width: 120px;">
                                            Data
                                        </th>
                                        <th class="line_bottom align_right" style="width: 150px;">
                                            Imponibile
                                        </th>
                                        <th class="line_bottom align_right" style="width: 150px;">
                                            Imposta
                                        </th>
                                        <th class="line_bottom align_right" style="width: 200px;">
                                            Totale
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server" />
                                    <tr>
                                        <th class="line_top">
                                        </th>
                                        <th class="line_top">
                                        </th>
                                        <th class="line_top align_right">
                                            &euro;&nbsp;<asp:Literal ID="ltr_cashTaxFree" runat="server"></asp:Literal>
                                        </th>
                                        <th class="line_top align_right">
                                            &euro;&nbsp;<asp:Literal ID="ltr_cashTaxAmount" runat="server"></asp:Literal>
                                        </th>
                                        <th class="line_top align_right">
                                            &euro;&nbsp;<asp:Literal ID="ltr_cashTotalAmount" runat="server"></asp:Literal>
                                        </th>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:ListView>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <tr id="itemPlaceholder" runat="server" />
            </LayoutTemplate>
        </asp:ListView>
        <tr>
            <td class="align_right">
                <table cellspacing="0" cellpadding="0" style="width: 40%; float: right; margin-top: 40px;">
                    <tr>
                        <th class="line_bottom line_top align_right txt-l" colspan="2">RIEPILOGO</th>
                    </tr>
                    <tr>
                        <th>Imponibile</th>
                        <td class="align_right">
                            &euro;&nbsp;<asp:Literal ID="ltr_cashTaxFree" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th>Imposta</th>
                        <td class="align_right">
                            &euro;&nbsp;<asp:Literal ID="ltr_cashTaxAmount" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th class="line_bottom line_top">Totale</th>
                        <th class="line_bottom line_top align_right">
                            &euro;&nbsp;<asp:Literal ID="ltr_cashTotalAmount" runat="server"></asp:Literal>
                        </th>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
