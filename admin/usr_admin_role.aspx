<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_admin_role.aspx.cs" Inherits="RentalInRome.admin.usr_admin_role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_ADMIN" OrderBy="id" Where="id>2 && is_deleted!=1">
            </asp:LinqDataSource>
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Gestione Utenti</h1>
                        <div class="bottom_agg">
                            <asp:LinkButton ID="lnk_nuovo" runat="server" OnClick="lnk_nuovo_Click" OnClientClick="removeTinyEditor();"><span>+ Nuovo</span></asp:LinkButton>
                        </div>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnSelectedIndexChanging="LV_SelectedIndexChanging">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("id") %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (Eval("is_active") + "" == "1") ? "SI" : "NO"%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (Eval("rnt_canHaveReservation") + "" == "1") ? "SI" : "NO"%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (Eval("rnt_canHaveRequest") + "" == "1") ? "SI" : "NO"%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (Eval("hasAuthUserReport") + "" == "1") ? "SI" : "NO"%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# AdminUtilities.usrRole_title(Eval("pid_role").objToInt32(),"-non abbinato-")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("name") + " " + Eval("surname")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("email")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("login")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" />
                                        &nbsp;&nbsp;&nbsp;
                                        <a href="usr_admin_lang.aspx?id=<%# Eval("id") %>" style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" title="Lingue">Lingue</a>
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
                                            <%# (Eval("is_active") + "" == "1") ? "SI" : "NO"%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (Eval("rnt_canHaveReservation") + "" == "1") ? "SI" : "NO"%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (Eval("rnt_canHaveRequest") + "" == "1") ? "SI" : "NO"%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (Eval("hasAuthUserReport") + "" == "1") ? "SI" : "NO"%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# AdminUtilities.usrRole_title(Eval("pid_role").objToInt32(),"-non abbinato-")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("name") + " " + Eval("surname")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("email")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("login")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" />
                                        &nbsp;&nbsp;&nbsp;
                                        <a href="usr_admin_lang.aspx?id=<%# Eval("id") %>" style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" title="Lingue">Lingue</a>
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
                                            <th id="Th1" runat="server" style="width: 50px">
                                                Abilitato?
                                            </th>
                                            <th id="Th6" runat="server" style="width: 50px">
                                                Pren.?
                                            </th>
                                            <th id="Th4" runat="server" style="width: 50px">
                                                Rich.?
                                            </th>
                                            <th id="Th7" runat="server" style="width: 50px">Report? </th>
                                            <th id="Th5" runat="server" style="width: 120px">
                                                Ruolo
                                            </th>
                                            <th id="Th11" runat="server" style="width: 300px">
                                                Nome Cognome
                                            </th>
                                            <th id="Th12" runat="server" style="width: 300px">
                                                E-mail
                                            </th>
                                            <th id="Th13" runat="server" style="width: 200px">
                                                login
                                            </th>
                                            <th id="Th3" runat="server">
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                                <div class="page">
                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;">
                                        <Fields>
                                            <asp:NumericPagerField />
                                        </Fields>
                                    </asp:DataPager>
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
                                            <%# (Eval("is_active") + "" == "1") ? "SI" : "NO"%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# AdminUtilities.usrRole_title(Eval("pid_role").objToInt32(),"-non abbinato-")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("name") + " " + Eval("surname")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("email")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("login")%></span>
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
                <h1 class="titolo_main">
                    Scheda del utente
                </h1>
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click"><span>Salva Modifiche</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_delete" runat="server" OnClick="lnk_delete_Click"><span>Elimina</span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
                <div class="nulla">
                </div>
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
                                            Nome:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_name" Width="230px" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_name" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Cognome:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_surname" Width="230px" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_surname" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            E-mail:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_email" Width="230px" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_email" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Ruolo:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_pid_role" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Login:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_login" Width="230px" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_login" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Password:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_password" Width="230px" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_password" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                           Abilitato?
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_is_active" runat="server">
                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Is Agent Contact ?
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_isAgentContact" runat="server">
                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="td_title">
                                            Abbinamento Prenotazioni?
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_rnt_canHaveReservation" runat="server">
                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Abbinamento Richieste?
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_rnt_canHaveRequest" runat="server">
                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Abbinamento Agenzie?
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_rnt_canHaveAgent" runat="server">
                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Gestione Report?
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_hasAuthUserReport" runat="server">
                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table id="pnlSoloMaga" runat="server" cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td class="td_title">Cambia Account Pren.?
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_rnt_canChangeReservationAccount" runat="server">
                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
