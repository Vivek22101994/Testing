<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_client.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_client" %>
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_id" runat="server" />
<asp:HiddenField ID="HF_IdReservation" runat="server" />
<asp:HiddenField ID="HF_IdRequest" runat="server" />
<asp:HiddenField ID="HF_pid_lang" runat="server" Value="1" />
<asp:HiddenField ID="HF_name_honorific" runat="server" />
<asp:HiddenField ID="HF_name_full" runat="server" />
<asp:HiddenField ID="HF_contact_email" runat="server" />
<asp:HiddenField ID="HF_pid_discount" runat="server" Value="1" />
<asp:HiddenField ID="HF_temp_id" runat="server" />
<div class="box_client_booking" id="pnl_sendBooking" runat="server" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:HiddenField ID="HF_mode" runat="server" Value="new" />
    <asp:HiddenField ID="HF_IdClient" runat="server" Value="0" />
    <div class="titflag">
        <div>
            <span>Dati del Cliente</span>
        </div>
    </div>
    <asp:PlaceHolder ID="PH_bookingForm" runat="server">
        <asp:PlaceHolder ID="PH_viewClient" runat="server">
            <asp:Literal ID="ltr_email" runat="server" Visible="false"></asp:Literal>
            <asp:Literal ID="ltr_honorific" runat="server" Visible="false"></asp:Literal>
            <asp:Literal ID="ltr_name_full" runat="server" Visible="false"></asp:Literal>
            <asp:Literal ID="ltr_phone_mobile" runat="server" Visible="false"></asp:Literal>
            <asp:Literal ID="ltr_country" runat="server" Visible="false"></asp:Literal>
            <asp:LinkButton ID="lnk_editClient" runat="server" OnClick="lnk_editClient_Click" CssClass="newclient"><span>Cambia i dati del cliente</span></asp:LinkButton>
            <asp:LinkButton ID="lnk_changeClient" runat="server" OnClick="lnk_changeClient_Click" CssClass="newclient"><span>Cambia il cliente</span></asp:LinkButton>
            <div class="nulla">
            </div>
            <div class="line">
                <div id="Div2" class="left">
                    <label class="desc">
                        E-mail
                    </label>
                    <div>
                        <input type="text" style="width: 300px" value="<%=ltr_email.Text %>" readonly="readonly" />
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="line">
                <div id="Div4" class="left">
                    <label class="desc">
                       Nome completo
                    </label>
                    <div>
                        <input type="text" style="width: 400px" value="<%=ltr_honorific.Text+"&nbsp;"+ltr_name_full.Text %>" readonly="readonly" />
                    </div>
                </div>
                <div id="Div5" class="left">
                    <label class="desc">
                        Lingua
                    </label>
                    <div>
                        <input type="text" style="width: 120px" value="<%=contUtils.getLang_title(HF_pid_lang.Value.ToInt32()) %>" readonly="readonly" />
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="line">
                <div id="Div6" class="left">
                    <label class="desc">
                        Cellulare
                    </label>
                    <div>
                        <input type="text" style="width: 120px" value="<%=ltr_phone_mobile.Text %>" readonly="readonly" />
                    </div>
                </div>
                <div id="Div7" class="left">
                    <label class="desc">
                        Paese
                    </label>
                    <div>
                        <input type="text" style="width: 350px" value="<%=ltr_country.Text %>" readonly="readonly" />
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="PH_newClient" runat="server">
            <asp:LinkButton ID="lnk_cancel_1" runat="server" OnClick="lnk_cancel_Click" CssClass="newclient"><span>Annulla</span></asp:LinkButton>
            <asp:LinkButton ID="lnk_goOldClient" runat="server" OnClick="lnk_goOldClient_Click" CssClass="newclient"><span>Clicca qui - Per selezionare un cliente registrato</span></asp:LinkButton>
            <div class="nulla">
            </div>
            <div class="line">
                <div id="txt_email_cont" class="left">
                    <label class="desc">
                        E-mail*
                    </label>
                    <div>
                        <asp:TextBox ID="txt_email" runat="server" Style="width: 300px;"></asp:TextBox>
                    </div>
                </div>
                <div id="txt_email_conf_cont" class="left">
                    <label class="desc">
                        Conferma E-mail*
                    </label>
                    <div>
                        <asp:TextBox ID="txt_email_conf" runat="server" Style="width: 300px;"></asp:TextBox>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="line">
                <div id="txt_name_first_cont" class="left">
                    <label class="desc">
                        Nome completo*
                    </label>
                    <div>
                        <asp:DropDownList ID="drp_honorific" runat="server" Style="margin-right: 10px; width: 60px;">
                        </asp:DropDownList>
                        <asp:TextBox ID="txt_name_full" runat="server" Style="width: 350px;"></asp:TextBox>
                    </div>
                </div>
                <div id="Div1" class="left">
                    <label class="desc">
                        Lingua*
                    </label>
                    <div>
                        <asp:DropDownList runat="server" ID="drp_lang" OnDataBound="drp_lang_DataBound" DataSourceID="LDS_lang" DataTextField="title" DataValueField="id">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="LDS_lang" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="title" TableName="CONT_TBL_LANGs" Where="is_active == 1">
                        </asp:LinqDataSource>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="line">
                <div id="txt_phone_cont" class="left">
                    <label class="desc">
                        Cellulare*
                    </label>
                    <div>
                        <asp:TextBox ID="txt_phone_mobile" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div id="drp_country_cont" class="left">
                    <label class="desc">
                        Paese*
                    </label>
                    <div>
                        <asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" Style="width: 350px;" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                            <WhereParameters>
                                <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <asp:LinkButton ID="lnk_saveNew" CssClass="btn bonifico" runat="server" OnClientClick="return true" OnClick="lnk_saveNew_Click"><span>Salva i dati del Cliente</span></asp:LinkButton>
        </asp:PlaceHolder>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="PH_oldClient" runat="server">
        <asp:LinkButton ID="lnk_cancel_2" runat="server" OnClick="lnk_cancel_Click" CssClass="newclient"><span>Annulla</span></asp:LinkButton>
        <asp:LinkButton ID="lnk_goNewClient" runat="server" OnClick="lnk_goNewClient_Click" CssClass="client"><span>Clicca qui - Per inserire nuovo cliente</span></asp:LinkButton>
        <div class="nulla">
        </div>
        <div class="divric">
            <!-- <h4>Filtra Utenti</h4>-->
            <div style="float: left;">
                <div class="camporic">
                    <label>
                        E-mail
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="txt_flt_email" Width="250px" />
                    </div>
                </div>
                <div class="camporic">
                    <label>
                        Nome/Cognome
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="txt_flt_name_full" Width="250px" />
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="btnric">
                <asp:LinkButton ID="lnk_filter" runat="server" OnClick="lnk_filter_Click">Filtra</asp:LinkButton>
            </div>
            <div class="nulla">
            </div>
        </div>
        <div class="risric">
            <asp:ListView ID="LV_flt" runat="server" DataSourceID="LDS_flt" OnItemDataBound="LV_flt_ItemDataBound" OnItemCommand="LV_flt_ItemCommand">
                <ItemTemplate>
                    <tr>
                        <td style="overflow: hidden; word-wrap: normal;">
                            <%# Eval("name_full") %>
                        </td>
                        <td style="overflow: hidden; word-wrap: normal;">
                            <%# Eval("contact_email")%>
                        </td>
                        <td>
                            <%# Eval("loc_country")%>
                        </td>
                        <td>
                            <asp:Label ID="lbl_is_active" runat="server" Visible="false" Text='<%# Eval("is_active")%>'></asp:Label>
                            <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id")%>'></asp:Label>
                            <asp:Label ID="lbl_not_active" runat="server" CssClass="sel" Text="Non Attivo"></asp:Label>
                            <asp:LinkButton ID="lnk_select" CssClass="sel" CommandName="seelziona" runat="server">Seleziona</asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    Non ci sono clienti con i parametri di ricerca...
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table cellpadding="2" cellspacing="0" border="0" style="table-layout: fixed;">
                        <tr>
                            <th>
                                Nome Cognome
                            </th>
                            <th width="270">
                                Email
                            </th>
                            <th width="120">
                                Nazione
                            </th>
                            <th width="80">
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server" />
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            <asp:LinqDataSource ID="LDS_flt" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_TBL_CLIENTs" OrderBy="name_full">
            </asp:LinqDataSource>
            <div class="nulla">
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:Label ID="lbl_errorAlert" CssClass="error_form_book" runat="server" Visible="false"></asp:Label>
    <div class="nulla">
    </div>
</div>
