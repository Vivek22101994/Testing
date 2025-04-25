<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_estate_service.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_estate_service" %>
<asp:HiddenField runat="server" ID="HF_service" Value="xxxx" />
<asp:HiddenField runat="server" ID="HF_IdEstate" Value="-1" />
<asp:HiddenField runat="server" ID="HF_id" Value="-1" />
<div class="boxmodulo pannello_fascia1" style="border: medium none;">
    <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_RL_ESTATE_SERVICE_PERIOD" Where="service == @service && (@pid_estate==-1 || pid_estate==@pid_estate)" EntityTypeName="" OrderBy="dtStart">
        <WhereParameters>
            <asp:ControlParameter ControlID="HF_service" Name="service" PropertyName="Value" Type="String" />
            <asp:ControlParameter ControlID="HF_IdEstate" Name="pid_estate" PropertyName="Value" Type="Int32" />
        </WhereParameters>
    </asp:LinqDataSource>
    <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand">
        <ItemTemplate>
            <tr onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                <td>
                    <span>
                        <%# (Eval("service").ToString()) == "eco" ? "Ecopulizie" : ""%>
                        <%# (Eval("service").ToString()) == "srs" ? "Srs" : ""%>
                    </span>
                </td>
                <td>
                    <span>
                        <%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(),"---")  %></span>
                </td>
                <td>
                    <span>
                        <%# ((DateTime)Eval("dtStart")).formatITA(true)%></span>
                </td>
                <td>
                    <span>
                        <%# ((DateTime)Eval("dtEnd")).formatITA(true)%></span>
                </td>
                <td>
                    <asp:Label ID="lbl_pid_estate" runat="server" Text='<%# Eval("pid_estate") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                    <asp:LinkButton ID="lnk_change" CommandName="change" ToolTip="Modifica" runat="server"><span style="color:#333366; margin:0;">Modifica</span></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnk_delete" CommandName="elimina" ToolTip="Elimina" runat="server" OnClientClick="return confirm('Vuoi eliminare questo periodo del servizio?')"><span style="color:#333366; margin:0;">Elimina</span></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                <td>
                    <span>
                        <%# (Eval("service").ToString()) == "eco" ? "Ecopulizie" : ""%>
                        <%# (Eval("service").ToString()) == "srs" ? "Srs" : ""%>
                    </span>
                </td>
                <td>
                    <span>
                        <%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(),"---")  %></span>
                </td>
                <td>
                    <span>
                        <%# ((DateTime)Eval("dtStart")).formatITA(true)%></span>
                </td>
                <td>
                    <span>
                        <%# ((DateTime)Eval("dtEnd")).formatITA(true)%></span>
                </td>
                <td>
                    <asp:Label ID="lbl_pid_estate" runat="server" Text='<%# Eval("pid_estate") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                    <asp:LinkButton ID="lnk_change" CommandName="change" ToolTip="Modifica" runat="server"><span style="color:#333366; margin:0;">Modifica</span></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnk_delete" CommandName="elimina" ToolTip="Elimina" runat="server" OnClientClick="return confirm('Vuoi eliminare questo periodo del servizio?')"><span style="color:#333366; margin:0;">Elimina</span></asp:LinkButton>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        Nessun Periodo di servizio
                        <br />
                        <div class="salvataggio">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click"><span>Aggiungi nuovo periodo</span></asp:LinkButton>
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
                        <asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click"><span>Aggiungi nuovo periodo</span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
                <div class="nulla">
                </div>
                <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                    <tr id="Tr1" runat="server" style="text-align: left">
                        <th id="Th1" runat="server" style="width: 60px;">
                            Servizio
                        </th>
                        <th id="Th3" runat="server" style="width: 200px;">
                            Struttura
                        </th>
                        <th id="Th4" runat="server" style="width: 120px;">
                            Data Inizio
                        </th>
                        <th id="Th6" runat="server" style="width: 120px;">
                            Data Fine
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
            <td class="td_title">
                Data Inizio:
            </td>
            <td>
                <input id="txt_dtStart" type="text" readonly="readonly" style="width: 150px" />
                <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
            </td>
        </tr>
        <tr>
            <td class="td_title">
                Data Fine:
            </td>
            <td>
                <input id="txt_dtEnd" type="text" readonly="readonly" style="width: 150px" />
                <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lbl_error" runat="server" Visible="false" CssClass="error_text"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
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
