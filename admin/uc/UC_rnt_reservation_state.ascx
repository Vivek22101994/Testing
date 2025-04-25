<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_state.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_state" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_show_body" Value="" runat="server" />
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_isChanged" runat="server" />
<asp:HiddenField ID="HF_id" runat="server" />
<asp:HiddenField ID="HF_IdReservation" runat="server" />
<asp:HiddenField ID="HF_state_body" runat="server" />
<asp:HiddenField ID="HF_state_subject" runat="server" />
<asp:HiddenField ID="HF_state_pid" runat="server" />
<asp:HiddenField ID="HF_" runat="server" />
<input type="hidden" id="rtn_stateHistory_state" />
<asp:HiddenField ID="HF_HAHAstateCancelledBy" runat="server" />


<script type="text/javascript">
    function rtn_toggle_stateHistory() {
        if ($("#rtn_stateHistory_state").val() == "1") {
            //$("#rtn_stateHistory_toggler").removeClass("opened");
            //$("#rtn_stateHistory_toggler").addClass("closed");
            $("#rtn_stateHistory_toggler").html("Visualizza Storico Stati");
            $("#rtn_stateHistory_cont").slideUp();
            $("#rtn_stateHistory_state").val("0");
        }
        else {
            //$("#rtn_stateHistory_toggler").removeClass("closed");
            //$("#rtn_stateHistory_toggler").addClass("opened");
            $("#rtn_stateHistory_toggler").html("Nascondi Storico Stati");
            $("#rtn_stateHistory_cont").slideDown();
            $("#rtn_stateHistory_state").val("1");
        }
    }
    $("#rtn_stateHistory_state").val("0");
</script>

<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <h3>
           Stato corrente
        </h3>
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia Stato</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                <tr>
                    <td class="td_title">
                        <strong>Data:</strong>
                    </td>
                    <td>
                        <asp:Literal ID="ltr_date" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr class="alternate">
                    <td class="td_title">
                        <strong>Ora:</strong>
                    </td>
                    <td>
                        <asp:Literal ID="ltr_time" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="td_title">
                        <strong>Stato:</strong>
                    </td>
                    <td>
                        <asp:Literal ID="ltr_state" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr class="alternate">
                    <td class="td_title">
                        <strong>Utente:</strong>
                    </td>
                    <td>
                        <asp:Literal ID="ltr_user" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr runat="server">
                    <td class="td_title">
                        <strong>Commenti:</strong>
                    </td>
                    <td>
                        <asp:Literal ID="ltr_subject" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <strong>Note:</strong>
                        <br />
                        <asp:Literal ID="ltr_body" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
        <div class="nulla">
        </div>
        <a id="rtn_stateHistory_toggler" class="changeapt" href="javascript:rtn_toggle_stateHistory()">Visualizza Storico Stati</a>
        <div class="nulla">
        </div>
        <div class="price_div" id="rtn_stateHistory_cont" style="display: none;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:ListView runat="server" ID="LV" DataSourceID="LDS" OnItemCommand="LV_ItemCommand" OnSelectedIndexChanging="LV_SelectedIndexChanging" OnItemDataBound="LV_ItemDataBound">
                <ItemTemplate>
                    <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                        <td>
                            <span>
                                <%# Eval("date_state")%></span>
                        </td>
                        <td>
                            <span>
                                <%#  rntUtils.rntReservation_getStateName(Eval("pid_state").objToInt32(), "Automatico")%></span>
                        </td>
                        <td>
                            <span>
                                <%# AdminUtilities.usr_adminName((int)Eval("pid_user"),"")%></span>
                        </td>
                        <td>
                            <span>
                                <%# (""+Eval("subject")).htmlEncode()%></span>
                        </td>
                        <td runat="server" visible="false">
                            <asp:LinkButton ID="lnk_select" runat="server" Style="margin-right: 20px;" CommandName="select" OnClientClick="return false;">note</asp:LinkButton>
                            <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                            <asp:Label runat="server" ID="lbl_pid_state" Text='<%# Eval("pid_state") %>' Visible="false"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                        <td>
                            <span>
                                <%# Eval("date_state")%></span>
                        </td>
                        <td>
                            <span>
                                <%#  rntUtils.rntReservation_getStateName(Eval("pid_state").objToInt32(), "Automatico")%></span>
                        </td>
                        <td>
                            <span>
                                <%# AdminUtilities.usr_adminName((int)Eval("pid_user"), "")%></span>
                        </td>
                        <td runat="server" >
                            <span>
                                <%# (""+Eval("subject")).htmlEncode()%></span>
                        </td>
                        <td runat="server" visible="false">
                            <asp:LinkButton ID="lnk_select" runat="server" Style="margin-right: 20px;" CommandName="select" OnClientClick="return false;">note</asp:LinkButton>
                            <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                            <asp:Label runat="server" ID="lbl_pid_state" Text='<%# Eval("pid_state") %>' Visible="false"></asp:Label>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <EmptyDataTemplate>
                    <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr5" runat="server" style="">
                            <th id="Th1" runat="server" style="width: 100px;">
                                Data
                            </th>
                            <th id="Th8" runat="server" style="width: 100px;">
                                Stato
                            </th>
                            <th id="Th11" runat="server" style="width: 130px;">
                                Utente
                            </th>
                            <th id="Th2" runat="server" >
                                Commenti
                            </th>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                </InsertItemTemplate>
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr5" runat="server" style="">
                            <th id="Th1" runat="server" style="width: 120px;">
                                Data Ora
                            </th>
                            <th id="Th8" runat="server" style="width: 100px;">
                                Stato
                            </th>
                            <th id="Th11" runat="server" style="width: 100px;">
                                Utente
                            </th>
                            <th id="Th2" runat="server" >
                                Commenti
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <SelectedItemTemplate>
                    <tr class="current" id="tr_selected" runat="server" style="cursor: pointer;">
                        <td>
                            <span>
                                <%# Eval("date_state")%></span>
                        </td>
                        <td>
                            <span>
                                <%#  rntUtils.rntReservation_getStateName(Eval("pid_state").objToInt32())%></span>
                        </td>
                        <td>
                            <span>
                                <%# AdminUtilities.usr_adminName((int)Eval("pid_user"), "")%></span>
                        </td>
                        <td runat="server" visible="false">
                            <span>
                                <%# (""+Eval("subject")).htmlEncode()%></span>
                        </td>
                        <td runat="server" visible="false">
                            <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                            <asp:Label runat="server" ID="lbl_pid_state" Text='<%# Eval("pid_state") %>' Visible="false"></asp:Label>
                            <asp:LinkButton ID="lnk_close" runat="server" Style="margin-right: 20px;" CommandName="close" OnClientClick="return false;">chiudi</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="current">
                        <td colspan="5">
                            <div class="mainbox" style="margin: 10px;">
                                <div class="mainbox" style="margin: 10px;">
                                    <%# (""+Eval("body")).htmlEncode()%></span>
                                </div>
                            </div>
                        </td>
                    </tr>
                </SelectedItemTemplate>
            </asp:ListView>
            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_RL_RESERVATION_STATEs" Where="pid_reservation == @pid_reservation" OrderBy="date_state desc">
                <WhereParameters>
                    <asp:ControlParameter ControlID="HF_IdReservation" Name="pid_reservation" PropertyName="Value" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Cambia Stato</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                <tr>
                    <td colspan="2">
                        <span id="lbl_title" runat="server" class="titoloboxmodulo" style="margin-top: 10px;"></span>
                    </td>
                </tr>
                <tr>
                    <td class="td_title">
                        <strong>Stato:</strong>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="drp_state" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr runat="server" id="drp_reasonBox" visible="false">
                 <td class="td_title">
                    <strong>Ragionare:</strong>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="drp_reason" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
                <tr id="pnlCommentEdit" runat="server" visible="false">
                    <td class="td_title">
                        <strong>Commenti:</strong>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_subject" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td colspan="2">
                        <strong>Note:</strong>
                        <br />
                        <asp:TextBox ID="txt_body" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="salvataggio" id="pnl_buttons" runat="server">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" ValidationGroup="state"><span>Salva lo Stato</span></asp:LinkButton>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <div class="nulla">
    </div>
</div>
