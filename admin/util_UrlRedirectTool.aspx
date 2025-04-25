<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="util_UrlRedirectTool.aspx.cs" Inherits="RentalInRome.admin.util_UrlRedirectTool" %>

<%@ Register Src="uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>
                            Elenco Regole Redirect delle pagine del Vecchio Sito</h1>
                        <div class="bottom_agg">
                            <asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click"><span>+ Nuova regola</span></asp:LinkButton>
                        </div>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" OnSelectedIndexChanging="LV_SelectedIndexChanging" OnItemCommand="LV_ItemCommand">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("from")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("to")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_name" Visible="false" runat="server" Text='<%#Eval("from") %>'></asp:Label>
                                        <asp:LinkButton ID="lnk_edit" runat="server" CommandName="select" Style="margin-top: 2px; margin-right: 10px;">modifica</asp:LinkButton>
                                        <asp:LinkButton ID="lnk_delete" runat="server" CommandName="elimina" Style="margin-top: 2px; margin-right: 10px;" OnClientClick="return confirm('Sta per eliminare il Record?')">elimina</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("from")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("to")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_name" Visible="false" runat="server" Text='<%#Eval("from") %>'></asp:Label>
                                        <asp:LinkButton ID="lnk_edit" runat="server" CommandName="select" Style="margin-top: 2px; margin-right: 10px;">modifica</asp:LinkButton>
                                        <asp:LinkButton ID="lnk_delete" runat="server" CommandName="elimina" Style="margin-top: 2px; margin-right: 10px;" OnClientClick="return confirm('Sta per eliminare il Record?')">elimina</asp:LinkButton>
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
                            <InsertItemTemplate>
                            </InsertItemTemplate>
                            <LayoutTemplate>
                                <div class="table_fascia">
                                    <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0" cellpadding="0" style="">
                                        <tr id="Tr1" runat="server" style="">
                                            <th id="Th1" runat="server" width="300px">
                                                From
                                            </th>
                                            <th id="Th4" runat="server" width="300px">
                                                To
                                            </th>
                                            <th id="Th5" runat="server">
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
                                        <span class="selez">
                                            <asp:TextBox ID="txt_from" runat="server" Text='<%# Eval("from")%>' Style="font-size: 11px; width: 250px; height: 12px;"></asp:TextBox>
                                        </span>
                                    </td>
                                    <td>
                                        <span class="selez">
                                            <asp:TextBox ID="txt_to" runat="server" Text='<%# Eval("to")%>' Style="font-size: 11px; width: 250px; height: 12px;"></asp:TextBox>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_name" Visible="false" runat="server" Text='<%# Eval("from") %>'></asp:Label>
                                        <asp:LinkButton ID="lnk_save" runat="server" CommandName="salva" Style="margin-top: 2px; margin-right: 10px;">salva</asp:LinkButton>
                                    </td>
                                </tr>
                            </SelectedItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
