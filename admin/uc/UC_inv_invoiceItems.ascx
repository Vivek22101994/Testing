<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_inv_invoiceItems.ascx.cs" Inherits="RentalInRome.admin.uc.UC_inv_invoiceItems" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_isChanged" runat="server" />
<asp:HiddenField ID="HF_IdInvoice" runat="server" />
<asp:HiddenField ID="HF_addingBlocked" runat="server" />
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <h3>
        Oggetti della fattura
    </h3>
    <asp:Panel ID="pnl_view" runat="server">
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia</asp:LinkButton>
        <div class="price_div">
            <table border="0" cellpadding="0" cellspacing="0" style="">
                <tr style="text-align: left">
                    <th style="width: 100px">
                        Item
                    </th>
                    <th style="width: 400px">
                        Description
                    </th>
                    <th style="width: 70px; text-align: center;">
                        Unit Cost (€)
                    </th>
                    <th style="width: 50px; text-align: center;">
                        Quantity
                    </th>
                    <th style="width: 70px; text-align: center;">
                        Price (€)
                    </th>
                    <th>
                    </th>
                </tr>
                <asp:ListView ID="LV_view" runat="server">
                    <ItemTemplate>
                        <tr class="">
                            <td>
                                <span>
                                    <%# Eval("code") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("description") %></span>
                            </td>
                            <td style="text-align: center;">
                                <span>
                                    <%# Eval("price_unit").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td style="text-align: center;">
                                <span>
                                    <%# Eval("quantity")%></span>
                            </td>
                            <td style="text-align: center;">
                                <span>
                                    <%# Eval("price_tf").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate">
                            <td>
                                <span>
                                    <%# Eval("code") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("description") %></span>
                            </td>
                            <td style="text-align: center;">
                                <span>
                                    <%# Eval("price_unit").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td style="text-align: center;">
                                <span>
                                    <%# Eval("quantity")%></span>
                            </td>
                            <td style="text-align: center;">
                                <span>
                                    <%# Eval("price_tf").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <tr>
                            <td colspan="8">
                                Non sono presenti Oggetti della fattura!
                                <asp:LinkButton ID="lnk_add_new" runat="server" OnClick="lnk_add_new_Click">Aggiungi</asp:LinkButton>
                            </td>
                        </tr>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <tr id="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <SelectedItemTemplate>
                        <tr class="current">
                            <td>
                                <span>
                                    <%# Eval("code") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("description") %></span>
                            </td>
                            <td style="text-align: center;">
                                <span>
                                    <%# Eval("price_unit").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td style="text-align: center;">
                                <span>
                                    <%# Eval("quantity")%></span>
                            </td>
                            <td style="text-align: center;">
                                <span>
                                    <%# Eval("price_tf").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                            </td>
                        </tr>
                    </SelectedItemTemplate>
                </asp:ListView>
            </table>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <asp:LinkButton ID="lnk_add_new" runat="server" CssClass="changeapt topright" OnClick="lnk_add_new_Click"><span>Aggiungi</span></asp:LinkButton>
        <div class="price_div">
            <table border="0" cellpadding="0" cellspacing="0" style="">
                <tr style="text-align: left">
                    <th style="width: 20px">
                        #
                    </th>
                    <th style="width: 100px">
                        Item
                    </th>
                    <th style="width: 400px">
                        Description
                    </th>
                    <th style="width: 50px; text-align: center;">
                        Quantity
                    </th>
                    <th style="width: 80px; text-align: center;">
                        Iva Applicata
                    </th>
                    <th style="width: 120px; text-align: center;">
                        Prezzo Totale Ivato
                    </th>
                    <th>
                    </th>
                </tr>
                <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <span>
                                    <%# Eval("sequence") %></span>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_code" runat="server" Text='<%# Eval("code") %>' Width="90"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_description" runat="server" Text='<%# Eval("description") %>' Width="390"></asp:TextBox>
                            </td>
                            <td style="text-align: center;">
                                <asp:TextBox ID="txt_quantity" runat="server" Text='<%# Eval("quantity") %>' Width="30"></asp:TextBox>
                            </td>
                            <td style="text-align: center;">
                                <asp:DropDownList ID="drp_cashTaxID" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: center;">
                                <asp:TextBox ID="txt_price_total" runat="server" Text='<%# Eval("price_total") %>' Width="100"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:Label ID="lbl_sequence" Visible="false" runat="server" Text='<%# Eval("sequence") %>' />
                                <asp:Label ID="lbl_cashTaxID" Visible="false" runat="server" Text='<%# Eval("price_tax_id") %>' />
                                <asp:LinkButton ID="lnk_del" runat="server" CssClass="del" CommandName="elimina" OnClientClick="return confirm('sta per eliminare oggetto della fattura?')">X</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <tr>
                            <td colspan="8">
                                Non sono presenti Oggetti della fattura!
                                <asp:LinkButton ID="lnk_add_new" runat="server" OnClick="lnk_add_new_Click">Aggiungi</asp:LinkButton>
                            </td>
                        </tr>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <tr id="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                </asp:ListView>
            </table>
            <div class="nulla">
            </div>
            <div class="btnric" style="float: left; margin: 5px 20px; color: #F00;" id="pnl_error" runat="server" visible="false">
                <h2>
                    Ci sono errori da correggere</h2>
                <asp:Literal ID="ltr_error" runat="server"></asp:Literal>
            </div>
            <div class="btnric" style="float: left; margin: 5px 20px;">
                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click"><span>Salva</span></asp:LinkButton>
            </div>
            <div class="btnric" style="float: left; margin: 5px 20px;">
                <asp:LinkButton ID="LinkButton3" runat="server" OnClick="lnk_cancel_Click"><span>Annulla</span></asp:LinkButton>
            </div>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
</div>
