<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_service.ascx.cs"
    Inherits="RentalInRome.reservationarea.UC_service" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div id="div_service" runat="server">
    <h3 class="underlined">
    <%=CurrentSource.getSysLangValue("lblServiceAddedCart")%>


       <%-- <%=CurrentSource.getSysLangValue("lblYourServices")%>--%>
        <%--  <%=CurrentSource.getSysLangValue("lblYourReservation")%>--%>
    </h3>

    <div id="infoContServAdd">
        <div class="infoBox" style="font-size: 11px;">
            <div class="colYourRes">
                <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand" OnItemDataBound="LV_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td valign="middle">
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');"
                                    CssClass="servDel"></asp:LinkButton>
                            </td>
                            <td valign="middle">
                                <span>
                                    <asp:Label ID="lbl_date" runat="server" Text='<%# Eval("date") %>'></asp:Label>
                                </span>
                            </td>
                            <td valign="middle">
                                <span>
                                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("serviceId") %>' Visible="false"></asp:Label>
                                    <%# Eval("serviceName")%></span>
                            </td>
                            <td valign="middle" align="right">
                                <span class="servPayNowSx" id="div_payNow" runat="server">
                                    <em><%= CurrentSource.getSysLangValue("lbl_payNow")%>:</em>
                                    <asp:Label ID="lbl_commission" runat="server"></asp:Label>
                                </span>
                                <span class="servTotalSx">
                                    <em><%= CurrentSource.getSysLangValue("rnt_pr_total")%>:</em>
                                    <asp:Label ID="lbl_price" runat="server"></asp:Label>
                                </span>  
                            </td>
                           <%-- <td valign="middle" align="right" id="td_commission" runat="server">
                                <span>
                                    <%# Eval("commission")%></span>
                            </td>--%>
                            

                        </tr>
                    </ItemTemplate>
                    <LayoutTemplate>

                        <table border="0" cellpadding="0" cellspacing="0" style="" class="servYourRes">
                            <tr style="text-align: left">
                                <th style="width: 9%"></th>
                                <th style="width: 33%" valign="middle">
                                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                        <%=CurrentSource.getSysLangValue("lblDate")%>
                                    </telerik:RadCodeBlock>
                                </th>
                                <th style="width: 36%" valign="middle">
                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                        <%=CurrentSource.getSysLangValue("lblName")%>
                                    </telerik:RadCodeBlock>
                                </th>
                                <th style="width: 36%" valign="middle" align="right">
                                    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                                        <%=CurrentSource.getSysLangValue("lblPrice")%>
                                    </telerik:RadCodeBlock>
                                </th>
                                <%--<th style="width: 18%" valign="middle" align="right" id="th_commission" runat="server">
                                    <telerik:RadCodeBlock ID="RadCodeBlock4" runat="server">
                                        <%=CurrentSource.getSysLangValue("lbl_payNow")%>
                                    </telerik:RadCodeBlock>
                                </th>--%>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>

                    </LayoutTemplate>
                </asp:ListView>

            </div>
        </div>
    </div>

    <a class="btnCalcola" href="extraservice_booking.aspx">
        <span>
            <%=CurrentSource.getSysLangValue("lblCompletePayment")%>
        </span>
    </a>
    <%-- <asp:LinkButton ID="lnk_book" runat="server"><%=CurrentSource.getSysLangValue("lblCompletePayment")%></asp:LinkButton>--%>
</div>
