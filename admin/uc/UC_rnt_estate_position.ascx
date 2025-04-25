<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_estate_position.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_estate_position" %>
<asp:HiddenField runat="server" ID="HF_position" Value="xxxx" />
<asp:HiddenField runat="server" ID="HF_id" Value="-1" />
<div class="boxmodulo pannello_fascia1" style="border: medium none;">
    <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_VIEW_ESTATE_POSITION" Where="position == @position && pid_lang==1" EntityTypeName="" OrderBy="sequence">
        <WhereParameters>
            <asp:ControlParameter ControlID="HF_position" Name="position" PropertyName="Value" Type="String" />
        </WhereParameters>
    </asp:LinqDataSource>
    <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand" OnItemDataBound="LV_ItemDataBound" OnDataBound="LV_DataBound">
        <ItemTemplate>
            <tr onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                <td>
                    <span>
                        <%# Eval("sequence")  %></span>
                </td>
                <td>
                    <span>
                        <%# CurrentSource.rntEstate_titleWithzone(Eval("pid_estate").objToInt32(), CurrentLang.ID, " - ", true, "")%></span>
                </td>
                <td>
                    <span>
                        <%# Eval("title")  %></span>
                </td>
                <td>
                    <asp:PlaceHolder ID="PH_edit" runat="server">
                        <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lbl_pid_estate" runat="server" Text='<%# Eval("pid_estate") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lbl_sequence" runat="server" Text='<%# Eval("sequence") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lbl_css_class" runat="server" Text='<%# Eval("css_class") %>' Visible="false"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtn_up" runat="server" ToolTip="Su" AlternateText="Su" CommandName="move_up" ImageUrl="~/images/ico/Go_up.png" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtn_down" runat="server" ToolTip="Giù" AlternateText="Giù" CommandName="move_down" ImageUrl="~/images/ico/Go_down.png" />
                    </asp:PlaceHolder>
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnk_change" CommandName="change" ToolTip="Modifica" runat="server"><span style="color:#333366; margin:0;">Modifica</span></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnk_delete" CommandName="elimina" ToolTip="Elimina" runat="server" OnClientClick="return confirm('Vuoi eliminare questa struttura dalla posizione?')"><span style="color:#333366; margin:0;">Elimina</span></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                <td>
                    <span>
                        <%# Eval("sequence")  %></span>
                </td>
                <td>
                    <span>
                        <%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(),"---")  %></span>
                </td>
                <td>
                    <span>
                        <%# Eval("title")  %></span>
                </td>
                <td>
                    <asp:PlaceHolder ID="PH_edit" runat="server">
                        <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lbl_pid_estate" runat="server" Text='<%# Eval("pid_estate") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lbl_sequence" runat="server" Text='<%# Eval("sequence") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lbl_css_class" runat="server" Text='<%# Eval("css_class") %>' Visible="false"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtn_up" runat="server" ToolTip="Su" AlternateText="Su" CommandName="move_up" ImageUrl="~/images/ico/Go_up.png" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtn_down" runat="server" ToolTip="Giù" AlternateText="Giù" CommandName="move_down" ImageUrl="~/images/ico/Go_down.png" />
                    </asp:PlaceHolder>
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnk_change" CommandName="change" ToolTip="Modifica" runat="server"><span style="color:#333366; margin:0;">Modifica</span></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnk_delete" CommandName="elimina" ToolTip="Elimina" runat="server" OnClientClick="return confirm('Vuoi eliminare questa struttura dalla posizione?')"><span style="color:#333366; margin:0;">Elimina</span></asp:LinkButton>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        Nessuna struttura presente nella posizione
                        <br />
                        <div class="salvataggio">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click"><span>Aggiungi nuova</span></asp:LinkButton>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <div class="table_fascia">
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click"><span>Aggiungi nuova</span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
                <div class="nulla">
                </div>
                <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                    <tr id="Tr1" runat="server" style="text-align: left">
                        <th id="Th1" runat="server" style="width: 40px;">
                            Pos.
                        </th>
                        <th id="Th3" runat="server" style="width: 200px;">
                            Struttura
                        </th>
                        <th id="Th4" runat="server" style="width: 400px;">
                            Testo aggiuntivo
                        </th>
                        <th id="Th5" runat="server">
                        </th>
                        <th id="Th2" runat="server">
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </div>
        </LayoutTemplate>
    </asp:ListView>
    <table id="pnl_content" runat="server" visible="false">
        <tr>
            <td>
                Zona:<br />
                <asp:DropDownList ID="drp_zone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_zone_SelectedIndexChanged" Width="400">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Struttura:<br />
                <asp:DropDownList ID="drp_estate" runat="server" Width="400">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Classe:<br />
                <asp:DropDownList ID="drp_class_type" runat="server" Width="400">
                    <asp:ListItem Value="" Text="- - -"></asp:ListItem>
                    <asp:ListItem Value="bestPrice" Text="bestPrice"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
                                    <asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
                                        <WhereParameters>
                                            <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_lang" CommandName="change_lang" CssClass="tab_item" runat="server">
                                                <span>
                                                    <%# Eval("title") %></span>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <span>No data was returned.</span>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <div class="menu2">
                                                <a id="itemPlaceholder" runat="server" />
                                            </div>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Testo aggiuntivo:<br />
                                    <asp:TextBox ID="txt_title" runat="server" Width="400"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_error" runat="server" Visible="false" CssClass="error_text"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" ValidationGroup="invoice"><span>Salva Modifiche</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div class="nulla">
    </div>
</div>
