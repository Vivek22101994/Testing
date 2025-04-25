<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_estate_list.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_estate_list" %>
<asp:HiddenField runat="server" ID="HF_id" Value="0" />
<asp:HiddenField runat="server" ID="HF_sort" Value="code" />
<asp:HiddenField ID="HF_lds_filter" runat="server" />
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TB_ESTATE" OrderBy="sequence">
</asp:LinqDataSource>
<asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnDataBound="LV_DataBound" OnPagePropertiesChanging="LV_PagePropertiesChanging" OnItemCommand="LV_ItemCommand">
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
                 <asp:LinkButton ID="lnk_setBP" CommandName="setBP" runat="server"><%# ("" + Eval("is_best_price")=="1")?"<span style=\"color: Green\">SI</span>":"<span style=\"color: Red\">NO</span>"%></asp:LinkButton>
            </td>
            <td>
                <%# ("" + Eval("is_exclusive") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("is_srs") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("is_ecopulizie") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("is_online_booking") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("pr_has_overnight_tax") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("is_HomeAway")=="1")?"<span style=\"color: Green\">SI</span>":"<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("bcomEnabled")=="1" && "" + Eval("bcomHotelId")!="" && "" + Eval("bcomRoomId")!="")?"<span style=\"color: Green\">Pubblicato</span>":(("" + Eval("bcomEnabled")=="1")?"<span style=\"color: Orange\">Attivo</span>":"<span style=\"color: Red\">Disattivato</span>")%>
            </td>
            <td>
                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" IdEstate='<%# Eval("id") %>' />
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
                <asp:LinkButton ID="lnk_setBP" CommandName="setBP" runat="server"><%# ("" + Eval("is_best_price")=="1")?"<span style=\"color: Green\">SI</span>":"<span style=\"color: Red\">NO</span>"%></asp:LinkButton>
            </td>
            <td>
                <%# ("" + Eval("is_exclusive") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("is_srs") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("is_ecopulizie") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("is_online_booking") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("pr_has_overnight_tax") == "1") ? "<span style=\"color: Green\">SI</span>" : "<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("is_HomeAway")=="1")?"<span style=\"color: Green\">SI</span>":"<span style=\"color: Red\">NO</span>"%>
            </td>
            <td>
                <%# ("" + Eval("bcomEnabled")=="1" && "" + Eval("bcomHotelId")!="" && "" + Eval("bcomRoomId")!="")?"<span style=\"color: Green\">Pubblicato</span>":(("" + Eval("bcomEnabled")=="1")?"<span style=\"color: Orange\">Attivo</span>":"<span style=\"color: Red\">Disattivato</span>")%>
            </td>
            <td>
                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" IdEstate='<%# Eval("id") %>' />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" style="">
            <tr>
                <td>No data was returned.
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
                    <th>ID
                    </th>
                    <th>Tipo
                    </th>
                    <th id="Th1" runat="server" style="width: 350px">
                        <asp:LinkButton runat="server" CssClass="lnk_view" Text="Nome &#9660;" ID="lnk_code" OnClick="lnk_code_Click"></asp:LinkButton>
                    </th>
                    <th id="Th2" runat="server" style="width: 160px">Proprietario
                    </th>
                    <th id="Th3" runat="server" style="width: 250px">Città - Zona
                    </th>
                    <th id="Th4" runat="server" style="width: 50px">Attivo
                    </th>
                    <th id="Th13" runat="server" style="width: 50px">B.P
                    </th>
                    <th id="Th5" runat="server" style="width: 50px">Esclusiva
                    </th>
                    <th id="Th7" runat="server" style="width: 50px">SRS
                    </th>
                    <th id="Th8" runat="server" style="width: 50px">Ecopulizie
                    </th>
                    <th id="Th9" runat="server" style="width: 50px">Booking Online
                    </th>
                    <th id="Th10" runat="server" style="width: 50px">Tassa di soggiorno
                    </th>
                    <th id="Th11" runat="server" style="width: 50px">Home Away
                    </th>
                    <th id="Th12" runat="server" style="width: 50px">Booking.com
                    </th>
                    <th id="Th6" runat="server" style="width: auto"></th>
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
