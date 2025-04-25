<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEstatePrices.ascx.cs" Inherits="RentalInRome.uc.ucEstatePrices" %>
<asp:HiddenField runat="server" ID="HF_unique" Visible="false" />
<asp:HiddenField runat="server" ID="HF_id" Visible="false" />
<asp:HiddenField runat="server" ID="Hf_markup" Visible="false" Value="0" />
<table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable" style="float: left;">
    <tr>
        <th style="border-left: none; background: none;">&nbsp;</th>
        <th colspan="2"><strong>
            <%= CurrentSource.rntPeriod_title(1, CurrentLang.ID, "Low Season")%></strong><br />
        </th>
        <th colspan="2"><strong>
            <%= CurrentSource.rntPeriod_title(4, CurrentLang.ID, "Medium Season")%></strong><br />
        </th>
        <th colspan="2"><strong>
            <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "High Season")%></strong><br />
        </th>
        <th colspan="2"><strong>
            <%= CurrentSource.rntPeriod_title(3, CurrentLang.ID, "Very High Season")%></strong><br />
        </th>

    </tr>
    <tr>
        <td class="prima" style="border-left: none;">&nbsp;
        </td>
        <td class="prima">
            <%= CurrentSource.getSysLangValue("lblDaily")%>:
        </td>
        <td class="prima">
            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
        </td>
        <td class="prima">
            <%= CurrentSource.getSysLangValue("lblDaily")%>:
        </td>
        <td class="prima">
            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
        </td>
        <td class="prima">
            <%= CurrentSource.getSysLangValue("lblDaily")%>:
        </td>
        <td class="prima">
            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
        </td>
        <td class="prima">
            <%= CurrentSource.getSysLangValue("lblDaily")%>:
        </td>
        <td class="prima">
            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
        </td>
    </tr>
    <%= ltr_priceDetails.Text%>
    <% if (currEstateTB.longTermRent == 1 && currEstateTB.longTermPrMonthly.objToDecimal() > 0)
       { 
    %>
    <tr>
        <td style="border-left: none; border-bottom: none;" colspan="7"></td>
    </tr>
    <tr>
        <th style="border-left: none; border-top: 2px solid #333366;" colspan="2"><strong>
            <%=CurrentSource.getSysLangValue("lblbusinesshousing")%>
        </strong></th>
        <th colspan="5" style="border-top: 2px solid #333366; background: none; text-align: left;"><strong>
            <%=CurrentSource.getSysLangValue("lblFrom") + "&nbsp;&euro;&nbsp;" + currEstateTB.longTermPrMonthly.objToDecimal().ToString("N2") + "&nbsp;" + CurrentSource.getSysLangValue("lblMonthly")%>
        </strong></th>
    </tr>
    <%
       } %>
</table>
<table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable priceTablecustom" style="float: left;">
    <tr>
        <th colspan="3" style="border-left: none;">Seasons reference </th>
    </tr>
    <tr>
        <th><strong>
            <%= CurrentSource.rntPeriod_title(1, CurrentLang.ID, "Low Season")%></strong><br />
        </th>
        <th><strong>
            <%= CurrentSource.rntPeriod_title(4, CurrentLang.ID, "Medium")%></strong><br />
        </th>
        <th><strong>
            <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "High Season")%></strong><br />
        </th>
        <th><strong>
            <%= CurrentSource.rntPeriod_title(3, CurrentLang.ID, "Very High Season")%></strong><br />
        </th>
    </tr>
    <tr>
        <td style="padding: 0pt; vertical-align: top;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td class="prima" style="border-left: none;">
                        <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                    </td>
                    <td class="prima">
                        <%= CurrentSource.getSysLangValue("lblDateTo")%>
                    </td>
                </tr>
                <asp:ListView ID="LVseasonDates_1" runat="server">
                    <ItemTemplate>
                        <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                        <tr>
                            <td style="border-left: none;">
                                <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                            </td>
                            <td>
                                <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <tr id="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                </asp:ListView>
            </table>
        </td>
        <td style="padding: 0pt; vertical-align: top;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td class="prima" style="border-left: none;">
                        <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                    </td>
                    <td class="prima">
                        <%= CurrentSource.getSysLangValue("lblDateTo")%>
                    </td>
                </tr>
                <asp:ListView ID="LVseasonDates_4" runat="server">
                    <ItemTemplate>
                        <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                        <tr>
                            <td style="border-left: none;">
                                <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                            </td>
                            <td>
                                <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <tr id="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                </asp:ListView>
            </table>
        </td>
        <td style="padding: 0pt; vertical-align: top;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td class="prima" style="border-left: none;">
                        <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                    </td>
                    <td class="prima">
                        <%= CurrentSource.getSysLangValue("lblDateTo")%>
                    </td>
                </tr>
                <asp:ListView ID="LVseasonDates_2" runat="server">
                    <ItemTemplate>
                        <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                        <tr>
                            <td style="border-left: none;">
                                <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                            </td>
                            <td>
                                <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <tr id="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                </asp:ListView>
            </table>
        </td>
        <td style="padding: 0pt; vertical-align: top;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td class="prima" style="border-left: none;">
                        <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                    </td>
                    <td class="prima">
                        <%= CurrentSource.getSysLangValue("lblDateTo")%>
                    </td>
                </tr>
                <asp:ListView ID="LVseasonDates_3" runat="server">
                    <ItemTemplate>
                        <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                        <tr>
                            <td style="border-left: none;">
                                <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                            </td>
                            <td>
                                <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <tr id="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                </asp:ListView>
            </table>
        </td>
    </tr>
</table>
<asp:Literal ID="ltr_priceDetails" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="ltr_priceTemplate" runat="server" Visible="false">
				<tr>
					<td style="border-left: none;">
						#num_pers#
					</td>
					<td>
						#pr_1#
					</td>
					<td>
						#pr_1_w#
					</td>
                    <td>
						#pr_4#
					</td>
					<td>
						#pr_4_w#
					</td>
					<td>
						#pr_2#
					</td>
					<td>
						#pr_2_w#
					</td>
					<td>
						#pr_3#
					</td>
					<td>
						#pr_3_w#
					</td>
				</tr>
</asp:Literal>
<asp:ListView ID="LV_special_offer" runat="server" OnDataBound="LV_special_offer_DataBound">
    <ItemTemplate>
        <a class="commento <%# Eval("class_type") %>">
            <span class="commCont">
                <span class="userName">
                    <%# Eval("title") %>&nbsp;<%# Eval("sub_title") %></span>
                <span class="nulla"></span>
                <span class="commentoTxt">
                    <%= CurrentSource.getSysLangValue("lblDateFrom")%>:&nbsp;<%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>&nbsp;&nbsp;-&nbsp;&nbsp;<%= CurrentSource.getSysLangValue("lblDateTo")%>:&nbsp;<%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID,"") %>
                </span>
            </span>
            <span class="alt_estate_euro">-<%# Eval("pr_discount").objToDecimal().ToString("N2") %>&nbsp;%
            </span>
        </a>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div id="specOffert">
            <h3 class="titBar">
                <asp:Literal ID="ltr_title" runat="server"></asp:Literal>
            </h3>
            <div id="ListspecOffert">
                <a id="itemPlaceholder" runat="server" />
            </div>
        </div>
    </LayoutTemplate>
</asp:ListView>
