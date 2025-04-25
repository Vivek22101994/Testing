<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_dt_pers.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_dt_pers" %>
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_is_booking" runat="server" Value="1" />
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_nights_min" runat="server" Value="3" />
<asp:HiddenField ID="HF_num_persons_child" runat="server" Value="3" />
<asp:HiddenField ID="HF_num_persons_min" runat="server" Value="3" />
<asp:HiddenField ID="HF_num_persons_max" runat="server" Value="3" />
<asp:HiddenField ID="HF_sel_dtStart" runat="server" />
<asp:HiddenField ID="HF_sel_dtEnd" runat="server" />
<asp:HiddenField ID="HF_sel_dtCount" runat="server" />
<asp:HiddenField ID="HF_sel_num_adult" runat="server" Value="2" />
<asp:HiddenField ID="HF_sel_num_child_over" runat="server" Value="0" />
<asp:HiddenField ID="HF_sel_num_child_min" runat="server" Value="0" />
<input type="hidden" id="rtn_price_state" />
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia Periodo e Ospiti</asp:LinkButton>
        <h3>
            Periodo e Ospiti</h3>
        <div class="price_div">
            <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <span class="numStep">Periodo</span>
                    </td>
                </tr>
                <tr>
                    <td style="width: 60px;">
                        Check-In
                    </td>
                    <td>
                        <%= HF_sel_dtStart.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#",1,"")%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Check-Out
                    </td>
                    <td>
                        <%= HF_sel_dtEnd.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#",1,"")%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Notti
                    </td>
                    <td>
                        <%= HF_sel_dtCount.Value%>
                    </td>
                </tr>
            </table>
            <table class="guestInfo risric" cellpadding="0" cellspacing="0" style="margin-left: 50px;" id="pnl_persView" runat="server">
                <tr>
                    <td colspan="2">
                        <span class="numStep">Ospiti</span>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left" style="width: 50px;">
                        <%= HF_sel_num_adult.Value%>
                    </td>
                    <td>
                        Adulti
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        <%= HF_sel_num_child_over.Value%>
                    </td>
                    <td>
                        Bambini (over 3)
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        <%= HF_sel_num_child_min.Value%>
                    </td>
                    <td>
                        Bambini (under 3)
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Cambia Periodo e Ospiti</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <span class="numStep">Periodo</span>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" />
                        <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                    </td>
                    <td>
                        <a class="calendario" id="startCalTrigger_<%= Unique %>"></a>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" />
                        <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                    </td>
                    <td>
                        <a class="calendario" id="endCalTrigger_<%= Unique %>"></a>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        <%= CurrentSource.getSysLangValue("lblNights")%>:
                        <asp:TextBox ID="txt_dtCount" runat="server" Style="width: 30px;"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" colspan="2">
                        <table>
                            <tr>
                                <td style="padding-top: 0;">
                                    <span class="rntCal nd_f" style="display: block; margin: 0; position: relative; height: 10px; width: 15px; margin-right: 5px;"></span>
                                </td>
                                <td style="padding-top: 0; font-size: 11px;">
                                    <%= CurrentSource.getSysLangValue("lblNotAvailable")%>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 0;">
                                    <span class="rntCal sel_f" style="display: block; margin: 0; position: relative; height: 10px; width: 15px; margin-right: 5px;"></span>
                                </td>
                                <td style="padding-top: 0; font-size: 11px;">
                                    <%= CurrentSource.getSysLangValue("lblDateSelected")%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="guestInfo risric" cellpadding="0" cellspacing="0" style="margin-left: 50px;" id="pnl_persEdit" runat="server">
                        <tr>
                            <td colspan="2">
                                <span class="numStep">Ospiti</span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left" style="width: 50px;">
                                <asp:DropDownList ID="drp_adult" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_adult_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Adulti
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left">
                                <asp:DropDownList ID="drp_child_over" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Bambini (over 3)
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="left">
                                <asp:DropDownList ID="drp_child_min" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Bambini (under 3)
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="btnric" style="float: left; margin: 50px;">
                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click">Salva</asp:LinkButton>
            </div>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <div class="nulla">
    </div>
</div>
<asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
<script type="text/javascript">
    var _JSCal_Range_<%= Unique %>;
    function setCal_<%= Unique %>(dtStartInt, dtEndInt) {
        _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", countMin: <%= nights_min %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", countCont: "#<%= txt_dtCount.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
    }
</script>
