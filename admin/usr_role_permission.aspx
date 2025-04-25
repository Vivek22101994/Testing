<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_role_permission.aspx.cs" Inherits="RentalInRome.admin.usr_role_permission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_TBL_ROLE" OrderBy="id">
            </asp:LinqDataSource>
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Gestione permessi</h1>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnSelectedIndexChanging="LV_SelectedIndexChanging">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span><%# Eval("id") %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("title")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("id") %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("title")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table id="Table1" runat="server" style="">
                                    <tr>
                                        <td>
                                            No data was returned.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div class="table_fascia">
                                    <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                                        <tr id="Tr1" runat="server" style="text-align: left">
                                            <th id="Th2" runat="server" style="width: 40px">
                                                ID
                                            </th>
                                            <th id="Th5" runat="server" style="width: 300px">
                                                Ruolo
                                            </th>
                                            <th id="Th3" runat="server">
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                            </LayoutTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <SelectedItemTemplate>
                                <tr class="current">
                                    <td>
                                        <span>
                                            <%# Eval("id") %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("title")%></span>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </SelectedItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Panel ID="pnlContent" runat="server" Width="100%" Visible="false">
                <h1 class="titolo_main">Permessi di <%=txt_title.Text%> </h1>
                <!-- INIZIO MAIN LINE -->
                <div class="mainline">
                    <!-- BOX 1 -->
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                        </div>
                        <div class="center">
                            <div class="boxmodulo">
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td class="td_title">
                                            Ruolo:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_title" ReadOnly="true" Width="230px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Richieste
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_rnt_onlyOwnedRequests" runat="server">
                                                <asp:ListItem Value="0">Tutti</asp:ListItem>
                                                <asp:ListItem Value="1">Solo sua</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Prenotazioni
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_rnt_onlyOwnedReservations" runat="server">
                                                <asp:ListItem Value="0">Tutti</asp:ListItem>
                                                <asp:ListItem Value="1">Solo sua</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Planner checkin/checkout
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_rnt_onlyOwnedPlannerCheckinCheckout" runat="server">
                                                <asp:ListItem Value="0">Tutti</asp:ListItem>
                                                <asp:ListItem Value="1">Solo sua</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Agenzie
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_rnt_onlyOwnedAgents" runat="server">
                                                <asp:ListItem Value="0">Tutti</asp:ListItem>
                                                <asp:ListItem Value="1">Solo sua</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Dati anagrafici
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_rnt_onlyOwnUserDetails" runat="server">
                                                <asp:ListItem Value="0">Tutti</asp:ListItem>
                                                <asp:ListItem Value="1">Solo sua</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            User Abbianti:
                                            <br/>
                                            <asp:ListView ID="LV_admins" runat="server">
                                                <ItemTemplate>
                                                    <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                                                        <td>
                                                            <span>
                                                                <%# Eval("name") + " " + Eval("surname")%></span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <%# Eval("email")%></span>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                                                        <td>
                                                            <span>
                                                                <%# Eval("name") + " " + Eval("surname")%></span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <%# Eval("email")%></span>
                                                        </td>
                                                     </tr>
                                                </AlternatingItemTemplate>
                                                <EmptyDataTemplate>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                                        <tr id="Tr5" runat="server" style="">
                                                            <th id="Th1" runat="server" style="width: 250px;">
                                                                Nome Cognome
                                                            </th>
                                                            <th id="Th4" runat="server" style="width: 100px;">
                                                                Email
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                            </asp:ListView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ListView ID="LV_permission" runat="server" OnItemCommand="LV_permission_ItemCommand" OnItemDataBound="LV_permission_ItemDataBound" OnDataBound="LV_permission_DataBound">
                                                <ItemTemplate>
                                                    <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                                                        <td>
                                                            <span>
                                                                <%# Eval("title")%></span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:DropDownList ID="drp_has" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_has_SelectedIndexChanged">
                                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:CheckBox ID="chk_can_read" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:CheckBox ID="chk_can_edit" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:CheckBox ID="chk_can_create" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:CheckBox ID="chk_can_delete" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td runat="server" visible="false">
                                                            <span>
                                                                <asp:CheckBox ID="chk_only_owned" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("code") %>' Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                                                        <td>
                                                            <span>
                                                                <%# Eval("title")%></span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:DropDownList ID="drp_has" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_has_SelectedIndexChanged">
                                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:CheckBox ID="chk_can_read" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:CheckBox ID="chk_can_edit" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:CheckBox ID="chk_can_create" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:CheckBox ID="chk_can_delete" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td runat="server" visible="false">
                                                            <span>
                                                                <asp:CheckBox ID="chk_only_owned" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("code") %>' Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                                <EmptyDataTemplate>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                                        <tr id="Tr5" runat="server" style="">
                                                            <th id="Th1" runat="server" style="width: 250px;">
                                                               Permesso
                                                            </th>
                                                            <th id="Th4" runat="server" style="width: 100px;">
                                                                Abilitato?
                                                            </th>
                                                            <th id="Th6" runat="server" style="width: 60px;">
                                                                Legge
                                                            </th>
                                                            <th id="Th7" runat="server" style="width: 60px;">
                                                                Modifica
                                                            </th>
                                                            <th id="Th8" runat="server" style="width: 60px;">
                                                                Crea
                                                            </th>
                                                            <th id="Th9" runat="server" style="width: 60px;">
                                                                Elimina
                                                            </th>
                                                            <th id="Th10" runat="server" visible="false" style="width: 80px;">
                                                                Solo sua
                                                            </th>
                                                            <th id="Th2" runat="server">
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                            </asp:ListView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="bottom">
                            <div style="float: left;">
                                <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                        </div>
                    </div>
                </div>
                <div class="nulla">
                </div>
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click"><span>Salva Modifiche</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
