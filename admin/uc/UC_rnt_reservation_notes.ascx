<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_notes.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_notes" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_isChanged" runat="server" />
<asp:HiddenField ID="HF_IdReservation" runat="server" />
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <h3>
            <asp:Literal ID="ltrTitle" runat="server">Note interne</asp:Literal>
        </h3>
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia Note</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_ok" runat="server">
                <tr>
                    <td>
                        <%= Body.htmlNoWrap()%>
                    </td>
                </tr>
            </table>
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_no" runat="server">
                <tr>
                    <td>
                        Non ci sono note inseriti
                    </td>
                </tr>
            </table>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Inserisci note</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txt_body" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="salvataggio" id="pnl_buttons" runat="server">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" ValidationGroup="discount"><span>Salva</span></asp:LinkButton>
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
