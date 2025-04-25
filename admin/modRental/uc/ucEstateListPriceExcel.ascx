<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEstateListPriceExcel.ascx.cs" Inherits="ModRental.admin.modRental.uc.ucEstateListPriceExcel" %>
<asp:HiddenField ID="HF_discount" runat="server" Value="0" />
<asp:HiddenField ID="HF_lds_filter" runat="server" />
<asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TB_ESTATE" OrderBy="code">
</asp:LinqDataSource>
<asp:ListView ID="LV" runat="server" DataSourceID="LDS">
    <ItemTemplate>
        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
        <tr>
            <th rowspan="2"><%# Eval("code") %></th>
            <th colspan="2">Low Season</th>
            <th colspan="2">High Season</th>
            <th colspan="2">Very High Season</th>
        </tr>
        <tr>
            <td>Daily</td>
            <td>Weekly</td>
            <td>Daily</td>
            <td>Weekly</td>
            <td>Daily</td>
            <td>Weekly</td>
        </tr>
        <asp:ListView ID="LvInner" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("pax") %> pax</td>
                    <td><%# Eval("pr1d").objToInt32() %></td>
                    <td><%# Eval("pr1w").objToInt32() %></td>
                    <td><%# Eval("pr2d").objToInt32() %></td>
                    <td><%# Eval("pr2w").objToInt32() %></td>
                    <td><%# Eval("pr3d").objToInt32() %></td>
                    <td><%# Eval("pr3w").objToInt32() %></td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
            </EmptyDataTemplate>
        </asp:ListView>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <meta http-equiv="Content-Type" content="application/vnd.ms-excel; charset=utf-8" />
        <table>
            <tr id="itemPlaceholder" runat="server">
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>
