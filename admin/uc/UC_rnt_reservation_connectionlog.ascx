<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_connectionlog.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_connectionlog" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_isChanged" runat="server" />
<asp:HiddenField ID="HF_IdReservation" runat="server" />
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <h3>
            Log connessioni
        </h3>
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Visualizza Log</asp:LinkButton>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Log connessioni</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Nascondi</asp:LinkButton>
        <div class="price_div">
            <asp:ListView runat="server" ID="LV">
                <ItemTemplate>
                    <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                        <td>
                            <span>
                                <%# Eval("date_connection")%></span>
                        </td>
                        <td>
                            <span>
                                <%# Eval("ip_name")%></span>
                        </td>
                        <td>
                            <span>
                                <%# Eval("comments")%></span>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                        <td>
                            <span>
                                <%# Eval("date_connection")%></span>
                        </td>
                        <td>
                            <span>
                                <%# Eval("ip_name")%></span>
                        </td>
                        <td>
                            <span>
                                <%# Eval("comments")%></span>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <EmptyDataTemplate>
                    Nessuna connessione del cliente rilevato
                </EmptyDataTemplate>
                <InsertItemTemplate>
                </InsertItemTemplate>
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr5" runat="server" style="">
                            <th id="Th1" runat="server" style="width: 120px;">
                                Data Ora
                            </th>
                            <th id="Th2" runat="server" style="width: 120px;">
                                IP
                            </th>
                            <th id="Th8" runat="server">
                                Dettagli
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <div class="nulla">
    </div>
</div>
