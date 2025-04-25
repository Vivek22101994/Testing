<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_discount.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_discount" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_isChanged" runat="server" />
<asp:HiddenField ID="HF_IdReservation" runat="server" />
<asp:HiddenField ID="HF_pr_total" runat="server" />
<asp:HiddenField ID="HF_pr_percentage" runat="server" />
<asp:HiddenField ID="HF_pr_discount_owner" runat="server" />
<asp:HiddenField ID="HF_pr_discount_commission" runat="server" />
<asp:HiddenField ID="HF_pr_discount_desc" runat="server" />
<asp:HiddenField ID="HF_pr_discount_user" runat="server" />
<asp:HiddenField ID="HF_pr_discount_custom" runat="server" />
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <h3>
           Sconto applicato
        </h3>
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia Sconto</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_ok" runat="server">
                <tr>
                    <td class="td_title">
                        <strong>Sconto:</strong>
                    </td>
                    <td>
                        <%= (HF_pr_discount_owner.Value.ToDecimal() + HF_pr_discount_commission.Value.ToDecimal()).ToString("N2") + "&nbsp;&euro;"%>
                    </td>
                </tr>
                <tr class="alternate">
                    <td class="td_title">
                        <strong>Per:</strong>
                    </td>
                    <td>
                       <%= (HF_pr_discount_owner.Value.ToDecimal() != 0 && HF_pr_discount_commission.Value.ToDecimal() != 0) ? "Prop (" + HF_pr_discount_owner.Value.ToDecimal().ToString("N2") + ") + Commissioni (" + HF_pr_discount_commission.Value.ToDecimal().ToString("N2") + ")" : ""%>
                       <%= (HF_pr_discount_owner.Value.ToDecimal()!=0 && HF_pr_discount_commission.Value.ToDecimal()==0)?"Proprietario":""%>
                       <%= (HF_pr_discount_owner.Value.ToDecimal()==0 && HF_pr_discount_commission.Value.ToDecimal()!=0)?"Commissioni":""%>
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
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_no" runat="server">
                <tr>
                    <td>
                        Nessuno sconto applicato
                    </td>
                </tr>
            </table>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Applica Sconto
        </h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                <tr>
                    <td class="td_title">
                        <strong>Modalità:</strong>
                    </td>
                    <td>
                        <asp:DropDownList ID="drp_custom" runat="server" style="font-size: 12px;" 
                            onselectedindexchanged="drp_custom_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Predefinito" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Personalizzato" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <asp:PlaceHolder ID="PH_default" runat="server">
                <tr>
                    <td class="td_title">
                        <strong>Sconto:</strong>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_pr_discount" Width="70px" />&nbsp;&euro;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_pr_discount" ErrorMessage="<br/>//obbligatorio" ValidationGroup="discount" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txt_pr_discount" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d+(\,\d\d)?$)" ValidationGroup="discount" Display="Dynamic"></asp:RegularExpressionValidator>
                    </td>
                </tr>
               <tr>
                    <td class="td_title">
                        <strong>Proprietario?</strong>
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_owner" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="td_title">
                        <strong>Commissioni?</strong>
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_commission" runat="server" />
                    </td>
                </tr>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="PH_custom" runat="server">
               <tr>
                    <td class="td_title">
                        <strong>Sconto Proprietario:</strong>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_pr_discount_owner" Width="70px" />&nbsp;&euro;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_pr_discount_owner" ErrorMessage="<br/>//obbligatorio" ValidationGroup="discount" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_pr_discount_owner" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d+(\,\d\d)?$)" ValidationGroup="discount" Display="Dynamic"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="td_title">
                        <strong>Sconto Commissioni:</strong>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_pr_discount_commission" Width="70px" />&nbsp;&euro;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_pr_discount_commission" ErrorMessage="<br/>//obbligatorio" ValidationGroup="discount" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_pr_discount_commission" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d+(\,\d\d)?$)" ValidationGroup="discount" Display="Dynamic"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                </asp:PlaceHolder>
                <tr>
                    <td colspan="2">
                        <strong>Note:</strong>
                        <br />
                        <asp:TextBox ID="txt_body" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_body" ValidationGroup="discount" runat="server" ErrorMessage="<br/>campo obbligatorio" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    </td>
                </tr>
            </table>
             </ContentTemplate>
        </asp:UpdatePanel>
                         <div class="salvataggio" id="pnl_buttons" runat="server">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" ValidationGroup="discount"><span>Salva</span></asp:LinkButton>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
       </div>
   </asp:Panel>
    <div class="nulla">
    </div>
</div>
