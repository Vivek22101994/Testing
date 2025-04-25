<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_residence_list.aspx.cs" Inherits="RentalInRome.admin.rnt_residence_list" %>

<%@ Register Src="~/admin/uc/UC_rnt_residence_navlinks.ascx" TagName="UC_rnt_residence_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <div class="pannello_fascia1">
            <div style="clear: both">
                <h1>Elenco Residenze</h1>
                <div class="bottom_agg">
                </div>
            </div>
            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TB_RESIDENCEs" OrderBy="code" Where="is_deleted != 1">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
                    <ItemTemplate>
                        <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                            <td>
                                <span>
                                    <%# Eval("id") %></span>
                            </td>
                            <td>
                                <span class="">
                                    <%# Eval("code")%></span>
                            </td>
                            <td>
                                <span class="">
                                    <%# AdminUtilities.usr_ownerName(Eval("pid_owner").objToInt32(), " -! non abbinato !-")%></span>
                            </td>
                            <td>
                                <span class="">
                                    <%# CurrentSource.loc_cityTitle(Eval("pid_city").objToInt32(), 1, " -! non abbinato !-")%>
                                    -
                                    <%# CurrentSource.locZone_title(Eval("pid_zone").objToInt32(), 1, " -! non abbinato !-")%>
                            </td>
                            <td>
                                <%# ("" + Eval("is_active")=="1")?"<span style=\"color: Green\">SI</span>":"<span style=\"color: Red\">NO</span>"%>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <uc1:UC_rnt_residence_navlinks ID="UC_rnt_residence_navlinks1" runat="server" IdResidence='<%# Eval("id") %>' />
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
                                <span class="">
                                    <%# Eval("code")%></span>
                            </td>
                            <td>
                                <span class="">
                                    <%# AdminUtilities.usr_ownerName(Eval("pid_owner").objToInt32(), " -! non abbinato !-")%></span>
                            </td>
                            <td>
                                <span class="">
                                    <%# CurrentSource.loc_cityTitle(Eval("pid_city").objToInt32(), 1, " -! non abbinato !-")%>
                                    -
                                    <%# CurrentSource.locZone_title(Eval("pid_zone").objToInt32(), 1, " -! non abbinato !-")%>
                            </td>
                            <td>
                                <%# ("" + Eval("is_active")=="1")?"<span style=\"color: Green\">SI</span>":"<span style=\"color: Red\">NO</span>"%>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <uc1:UC_rnt_residence_navlinks ID="UC_rnt_residence_navlinks1" runat="server" IdResidence='<%# Eval("id") %>' />
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
                            <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                                <tr id="Tr1" runat="server" style="">
                                    <th>
                                        ID
                                    </th>
                                    <th id="Th1" runat="server" style="width: 350px">
                                        Nome
                                    </th>
                                    <th id="Th2" runat="server" style="width: 160px">
                                        Proprietario
                                    </th>
                                    <th id="Th3" runat="server" style="width: 250px">
                                        Città - Zona
                                    </th>
                                    <th id="Th4" runat="server" style="width: 50px">
                                        Attivo
                                    </th>
                                    <th id="Th6" runat="server" style="width: auto">
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
