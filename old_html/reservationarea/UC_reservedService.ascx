<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_reservedService.ascx.cs" Inherits="RentalInRome.reservationarea.UC_reservedService" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div id="div_service" runat="server">
    <h3 class="underlined">
    <%=CurrentSource.getSysLangValue("lblYourReservedServices")%>
       <%-- <%=CurrentSource.getSysLangValue("lblYourServices")%>--%>
        <%--  <%=CurrentSource.getSysLangValue("lblYourReservation")%>--%>
    </h3>
    <div id="infoContServ">
        <div class="infoBox" style="font-size: 11px;">
            <div class="colYourRes">
                <asp:ListView ID="LV" runat="server" OnItemDataBound="LV_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td valign="middle">
                                <span>
                                <asp:Label ID="lbl_date" runat="server"></asp:Label>
                                   </span>
                            </td>
                            <td valign="middle">
                                <span>
                                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("pidExtras") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lbl_name" runat="server"></asp:Label>
                                    </span>
                            </td>
                           <%-- <td valign="middle" align="right">

                                <span><asp:Label ID="lbl_commission" runat="server"></asp:Label><br />
                                    <asp:Label ID="lbl_price" runat="server"></asp:Label>
                                   </span>
                            </td>--%>
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
                         
                        </tr>
                    </ItemTemplate>
                    <LayoutTemplate>
                
                        <table border="0" cellpadding="0" cellspacing="0" class="servYourRes">
                            <tr style="text-align: left">
                                <th style="width: 38%" valign="middle">
                                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                        <%=CurrentSource.getSysLangValue("lblDate")%>
                                    </telerik:RadCodeBlock>
                                </th>
                                <th style="width: 38%" valign="middle">
                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                        <%=CurrentSource.getSysLangValue("lblName")%>
                                    </telerik:RadCodeBlock>
                                </th>
                                <th style="width: 20%" valign="middle" align="right">
                                    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                                        <%=CurrentSource.getSysLangValue("lblPrice")%>
                                    </telerik:RadCodeBlock>
                                </th>
                               
                            </tr>
                      
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                
                    </LayoutTemplate>
                </asp:ListView>
              
            </div>
        </div>
    </div>
</div>
