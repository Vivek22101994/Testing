<%@ Page Title="" Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="invoiceList.aspx.cs" Inherits="RentalInRome.affiliatesarea.invoiceList" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <style type="text/css">
    .res_state { width: 20px; height: 20px; display: block;}
    .res_3{ background-color: #f00;}
    .res_4{ background-color: #0f0;}
    .res_6{ background-color: #00f;}
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <h3 style="margin-bottom: 20px; margin-left: 8px;" class="underlined"><%=CurrentSource.getSysLangValue("lblInvoiceList")%></h3>
    <div class="nulla">
    </div>
    <div class="reslist">
        <div class="filt" runat="server" visible="false">
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
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <label>
                                                <%=CurrentSource.getSysLangValue("lblCodeBooking")%></label>
                                            <asp:TextBox ID="txt_flt_code" runat="server" Width="50" CssClass="inp"></asp:TextBox>
                                        </td>
                                        <td>
                                            <label>
                                                <%=CurrentSource.getSysLangValue("lblMonth")%></label>
                                            <asp:DropDownList runat="server" ID="drp_flt_month" CssClass="inp" Width="120px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <label>State</label>
                                            <asp:CheckBoxList ID="chkList_flt_state" runat="server" CssClass="inp" RepeatColumns="3">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="middle">
                                <asp:LinkButton ID="lnk_filter" runat="server" CssClass="buttprosegui" OnClick="lnk_filter_Click"><%=CurrentSource.getSysLangValue("lblBeginSearch")%></asp:LinkButton>
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
        <div class="table_fascia tfagency">
            <table border="0" cellpadding="0" cellspacing="0" style="">
                <tr>
                    <th style="text-align: center;"><%=CurrentSource.getSysLangValue("pdf_InvoiceNumber")%></th>
                    <th style="text-align: center;">Invoice Date</th>
                    <th style="text-align: center;">Reservation Code</th>
                    <th style="text-align: right;">Paid Amount</th>
                    <th></th>
                </tr>
                <asp:ListView ID="LV" runat="server">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("code")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime)Eval("inv_dtInvoice")).formatCustom("#dd# #MM# #yy#",CurrentLang.ID,"")%></span>
                            </td>
                            <td>
                                <span>
                                    <%#  Eval("rnt_reservation_code")%></span>
                            </td>
                            <td style="text-align: right;">
                                <span>
                                    <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a target="_blank" href="<%#CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + (CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=" + Eval("uid")).urlEncode() + "&filename=" + ("RiR-reservation_invoice-code_" + Eval("code") + ".pdf").urlEncode()%>" style="margin-top: 6px; margin-right: 5px;">
                                    Download
                                </a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" style="">
                            <tr>
                                <td>
                                    No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <tr id="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                </asp:ListView>
            </table>
        </div>
    </div>
</asp:Content>
