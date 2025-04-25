<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_admin_config_list.aspx.cs" Inherits="RentalInRome.admin.usr_admin_config_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" Select="new (id, code)" TableName="USR_ADMIN_CONFIGs" OrderBy="code">
    </asp:LinqDataSource>
    <div id="fascia1">
        <div class="pannello_fascia1">
            <div style="clear: both">
                <h1>Lista admin config</h1>
                <div class="bottom_agg">
                </div>
            </div>
            <div style="clear: both">
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
                            </td>
                            <td>
                                <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("code") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="usr_admin_config_details.aspx?id=<%# Eval("id") %>">modifica</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
                            </td>
                            <td>
                                <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("code") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="usr_admin_config_details.aspx?id=<%# Eval("id") %>">modifica</a>
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
                                    <th id="Th2" runat="server" style="width: 30px">
                                        ID
                                    </th>
                                    <th id="Th1" runat="server" style="width: 300px">
                                        Codice
                                    </th>
                                    <th id="Th3" runat="server">
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </div>
        <div style="clear: both">
        </div>
    </div>
    <div class="nulla">
    </div>
</asp:Content>
