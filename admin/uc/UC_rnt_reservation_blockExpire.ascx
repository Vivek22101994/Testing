<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_blockExpire.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_blockExpire" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_isChanged" runat="server" />
<asp:HiddenField ID="HF_IdReservation" runat="server" />
<asp:HiddenField ID="HF_dttCreation" runat="server" />
<asp:HiddenField ID="HF_block_expire" runat="server" />
<asp:HiddenField ID="HF_block_expire_hours" runat="server" />
<asp:HiddenField ID="HF_block_pid_user" runat="server" />
<asp:HiddenField ID="HF_block_comments" runat="server" />
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <h3>
           Scadenza corrente
        </h3>
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia Scadenza</asp:LinkButton>
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
                        <strong>Utente:</strong>
                    </td>
                    <td>
                        <asp:Literal ID="ltr_user" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr class="alternate">
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
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Cambia Scadenza</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                <tr>
                    <td class="td_title">
                        <strong>Scadenza:</strong>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="drp_block_expire_hours" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <strong>Note:</strong>
                        <br />
                        <asp:TextBox ID="txt_body" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_body" ValidationGroup="block_expire" runat="server" ErrorMessage="<br/>campo obbligatorio" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="salvataggio" id="pnl_buttons" runat="server">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" ValidationGroup="block_expire"><span>Salva la Scadenza</span></asp:LinkButton>
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
