<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cashBookPrintPreview.aspx.cs" Inherits="ModInvoice.admin.modInvoice.cashBookPrintPreview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        .line_separator { border-bottom: 1px solid #000000; }
        span { margin-left: 4px; display: inline-block;}
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width: 750px;">
    <table cellspacing="0" cellpadding="0" style="width: 740px;">
        <tr>
            <th class="line_bottom align_center txt-xl">Riepilogo Prima Nota </th>
        </tr>
        <tr>
            <th class="align_center txt-l">
                <asp:Literal ID="ltrFilter" runat="server"></asp:Literal>
            </th>
        </tr>
        <tr>
            <td>
                <asp:ListView ID="LVlist" runat="server">
                    <ItemTemplate>
                        <tr class="" style="background-color: #<%# Eval("cashInOut") + "" == "1" ? "ecf8d8" : "fee1e1"%>;">
                            <td class="line_separator">
                                <span>
                                    <%# Eval("cashInOut") + "" == "1" ? "E" : "U"%></span>
                            </td>
                            <td class="line_separator">
                                <span>
                                    <%# ((DateTime?)Eval("cashDate")).formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----")%>
                                </span>
                            </td>
                            <td class="line_separator align_right">
                                <span>
                                    <%# Eval("cashAmount").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                            <td class="line_separator">
                                <span>
                                    <%# invUtils.getCashPlace_title(Eval("cashPlace").objToInt32(), "-non definito-")%></span>
                            </td>
                            <td class="line_separator">
                                <span>
                                    <%# invUtils.getCashType_title(Eval("cashType").objToInt32(), "-non definito-")%></span>
                            </td>
                            <td class="line_separator">
                                <%# Eval("cashPayed").objToDecimal() == Eval("docAmount").objToDecimal() ? "<span style=\"color: #00DD00; font-weight: bold;\">SI</span>" : "<span style=\"color: #FE6634; font-weight: bold;\">NO</span>"%>
                            </td>
                            <td class="line_separator">
                                <span>
                                    <%# Eval("docNum")%></span>
                            </td>
                            <td class="line_separator">
                                <span>
                                    <%# ((DateTime?)Eval("docIssueDate")).formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----")%>
                                </span>
                            </td>
                            <td class="line_separator">
                                <span>
                                    <%# Eval("docCaseCode")%></span>
                            </td>
                            <td class="line_separator">
                                <span>
                                    <%# Eval("ownerType")%></span>
                            </td>
                            <td class="line_separator">
                                <span>
                                    <%# Eval("ownerNameFull")%></span>
                            </td>
                            <td class="line_separator align_right">
                                <span>
                                    <%# Eval("docAmount").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <th class="line_bottom"></th>
                                <th class="line_bottom">Data</th>
                                <th class="line_bottom align_right">Importo</th>
                                <th class="line_bottom">Movimento su</th>
                                <th class="line_bottom">Modalità</th>
                                <th class="line_bottom">Pag? </th>
                                <th class="line_bottom">N.Doc </th>
                                <th class="line_bottom">Data Doc </th>
                                <th class="line_bottom">Causale</th>
                                <th class="line_bottom">Tipo anag. </th>
                                <th class="line_bottom">Al nome di </th>
                                <th class="line_bottom align_right">Importo Doc</th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <td class="align_right">
                <table cellspacing="0" cellpadding="0" style="width: 40%; float: right; margin-top: 40px;">
                    <tr>
                        <th class="line_bottom line_top align_right txt-l" colspan="2">RIEPILOGO</th>
                    </tr>
                    <tr>
                        <th>Entrate</th>
                        <td class="align_right">
                            &euro;&nbsp;<asp:Literal ID="ltr_cashAmount_1" runat="server"></asp:Literal>
                            <asp:Literal ID="ltr_count_1" runat="server" Visible="false"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th>Uscite</th>
                        <td class="align_right">
                            &euro;&nbsp;<asp:Literal ID="ltr_cashAmount_0" runat="server"></asp:Literal>
                            <asp:Literal ID="ltr_count_0" runat="server" Visible="false"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th class="line_bottom line_top">Situazione (Differenza)</th>
                        <th class="line_bottom line_top align_right">
                            &euro;&nbsp;<asp:Literal ID="ltr_cashAmount_diff" runat="server"></asp:Literal>
                            <asp:Literal ID="ltr_count_diff" runat="server" Visible="false"></asp:Literal>
                        </th>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
