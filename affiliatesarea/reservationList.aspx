<%@ Page Title="" Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="reservationList.aspx.cs" Inherits="RentalInRome.affiliatesarea.reservationList" %>

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
    <h3 style="margin-bottom: 20px; margin-left: 8px;" class="underlined"><%=CurrentSource.getSysLangValue("lblYourReservations")%></h3>
    <div class="nulla">
    </div>
    <div class="reslist">
        <asp:HiddenField ID="HF_lds_filter" runat="server" Visible="false" />
        <asp:HiddenField ID="HF_LDS_orderBy" runat="server" Value="state_date desc" Visible="false" />
        <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TBL_RESERVATION">
        </asp:LinqDataSource>
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
                    <th style="text-align: center;">
                        <%=CurrentSource.getSysLangValue("lblCodeBooking")%></th>
                    <th>
                        <%//=CurrentSource.getSysLangValue("lblStateBooking")%></th>
                    <th></th>
                    <th>
                        <%=CurrentSource.getSysLangValue("reqApartments")%></th>
                    <th>
                        <asp:LinkButton ID="lnk_orderBy_dtStart" runat="server" OnClick="lnk_OrderBy_Click" CommandArgument="dtStart"><%=CurrentSource.getSysLangValue("lblchkin")%></asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="lnk_orderBy_dtEnd" runat="server" OnClick="lnk_OrderBy_Click" CommandArgument="dtEnd"><%=CurrentSource.getSysLangValue("lblchkout")%></asp:LinkButton>
                    </th>
                    <th>
                        <%=CurrentSource.getSysLangValue("lblPax")%></th>
                    <th colspan="2" style="text-align: center;">
                        <img src="/images/css/icothsoldi.jpg" class="ico_tooltip" alt="" ttp="<%=CurrentSource.getSysLangValue("lblYourCommission")%>" />
                    </th>
                    <th>
                        <asp:LinkButton ID="lnk_orderBy_pr_total" runat="server" OnClick="lnk_OrderBy_Click" CommandArgument="pr_total"><%= contUtils.getLabel_title("lblTotalToPayAgency", App.LangID, "Total to pay")%></asp:LinkButton>
                    </th>
                    <th></th>
                </tr>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnDataBound="LV_DataBound" OnItemDataBound="LV_ItemDataBound" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("code")%></span>
                            </td>
                            <td>
                                <span>
                                    <img src="/images/css/rntReservationState_<%# Eval("state_pid")%>.jpg" alt="<%# CurrentSource.getSysLangValue("rntReservationState_" + Eval("state_pid"))%>" class="ico_tooltip" ttp="<%# CurrentSource.getSysLangValue("rntReservationState_" + Eval("state_pid"))%>" />
                                </span>
                            </td>
                            <td>
                                <span>
                                    <%#  ((DateTime?)Eval("dtCreation")).formatCustom("#yy# #MM#", CurrentLang.ID, "--/--")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(), "-----")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime?)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", RentalInRome.affiliatesarea.agentAuth.CurrentLangID, "----")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime?)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", RentalInRome.affiliatesarea.agentAuth.CurrentLangID, "----")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# (Eval("num_adult").objToInt32() + Eval("num_child_over").objToInt32()) + "&nbsp;(" + Eval("num_adult").objToInt32() + "+" + Eval("num_child_over").objToInt32() + ")&nbsp;+&nbsp;" + Eval("num_child_min").objToInt32()%></span>
                            </td>
                            <td style="text-align: center;">
                                <strong>
                                    <%# "&euro;&nbsp;" + Eval("agentCommissionPrice").objToDecimal().ToString("N2")%></strong>
                            </td>
                            <td style="text-align: center;">
                                <strong>
                                    <%# Eval("agentCommissionPerc").objToInt32() + "&nbsp;%"%></strong>
                            </td>
                            <td>
                                <span>
                                    <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2") + ""%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a target="_blank" href="/reservationarea/login.aspx?auth=<%# Eval("unique_id") %>" style="margin-top: 6px; margin-right: 5px;">
                                    <%=CurrentSource.getSysLangValue("lblDetails")%></a>
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
        <div class="ordina pageCount">
            <asp:DataPager ID="DataPager1" PagedControlID="LV" runat="server" PageSize="50" style="border-right: medium none;" QueryStringField="pg">
                <Fields>
                    <asp:NumericPagerField ButtonCount="20" />
                </Fields>
            </asp:DataPager>
        </div>
    </div>
</asp:Content>
