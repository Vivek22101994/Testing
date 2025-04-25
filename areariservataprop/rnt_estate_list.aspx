<%@ Page Title="" Language="C#" MasterPageFile="~/areariservataprop/common/MP.Master" AutoEventWireup="true" CodeBehind="rnt_estate_list.aspx.cs" Inherits="RentalInRome.areariservataprop.rnt_estate_list" %>

<%@ Register Src="~/areariservataprop/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<%@ Register Src="~/areariservataprop/uc/UC_contactStaff.ascx" TagName="UC_contactStaff" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:HiddenField ID="HF_is_filtered" runat="server" />
    <asp:HiddenField ID="HF_url_filter" runat="server" />
    <div id="fascia1">
        <div class="pannello_fascia1">
            <div style="clear: both">
                <h1>Elenco Strutture</h1>
                <div class="nulla">
                </div>
            </div>
            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TB_ESTATE" OrderBy="code">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnDataBound="LV_DataBound">
                    <ItemTemplate>
                        <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                            <td>
                                <span>
                                    <%# Eval("id") %></span>
                            </td>
                            <td>
                                <span class="">
                                    <%# ("" + Eval("category") == "apt") ? "Appartamento" : ""%>
                                    <%# ("" + Eval("category") == "villa") ? "Villa" : ""%>
                                </span>
                            </td>
                            <td>
                                <span class="">
                                    <%# "" + Eval("pid_owner") == "385" ? Eval("inner_notes") : Eval("code")%></span>
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
                                <%# ("" + Eval("is_online_booking") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <uc1:uc_rnt_estate_navlinks id="UC_rnt_estate_navlinks1" runat="server" idestate='<%# Eval("id") %>' />
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
                                    <%# ("" + Eval("category") == "apt") ? "Appartamento" : ""%>
                                    <%# ("" + Eval("category") == "villa") ? "Villa" : ""%>
                                </span>
                            </td>
                            <td>
                                <span class="">
                                    <%# "" + Eval("pid_owner") == "385" ? Eval("inner_notes") : Eval("code")%></span>
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
                                <%# ("" + Eval("is_online_booking") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <uc1:uc_rnt_estate_navlinks id="UC_rnt_estate_navlinks1" runat="server" idestate='<%# Eval("id") %>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" style="">
                            <tr>
                                <td>
                                    Nessuna struttura trovata...
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <InsertItemTemplate>
                    </InsertItemTemplate>
                    <LayoutTemplate>
                        <div class="page">
                            <asp:DataPager ID="DataPager2" runat="server" style="border-right: medium none;" QueryStringField="pg" PageSize="50">
                                <Fields>
                                    <asp:NumericPagerField ButtonCount="5555" />
                                </Fields>
                            </asp:DataPager>
                            <asp:Label ID="lbl_record_count_top" runat="server" CssClass="total" Text=""></asp:Label>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="table_fascia">
                            <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                                <tr id="Tr1" runat="server" style="">
                                    <th>
                                        ID
                                    </th>
                                    <th>
                                        Tipo
                                    </th>
                                    <th id="Th1" runat="server" style="width: 350px">
                                        Nome struttura
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
                                    <th id="Th9" runat="server" style="width: 50px">
                                        Booking Online
                                    </th>
                                    <th id="Th6" runat="server" style="width: auto">
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                        <div class="page">
                            <asp:DataPager ID="DataPager1" runat="server" style="border-right: medium none;" QueryStringField="pg" PageSize="50">
                                <Fields>
                                    <asp:NumericPagerField ButtonCount="5555" />
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
</asp:Content>
