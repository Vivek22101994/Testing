<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="cont_tour_item_list.aspx.cs" Inherits="RentalInRome.admin.cont_tour_item_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_VIEW_TOUR_ITEMs" OrderBy="title" Where="pid_lang==1">
    </asp:LinqDataSource>
    <div id="fascia1">
        <div class="pannello_fascia1">
            <div style="clear: both">
                <h1>Elenco Escursioni-Tour</h1>
                <div class="bottom_agg">
                    <a href="cont_tour_item_details.aspx?id=0">
                        <span>+ Nuovo</span></a>
                </div>
            </div>
            <div style="clear: both">
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
                    <ItemTemplate>
                        <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                            <td>
                                <span>
                                    <%# Eval("title") %></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="cont_tour_item_details.aspx?id=<%# Eval("id") %>">modifica</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                            <td>
                                <span>
                                    <%# Eval("title") %></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="cont_tour_item_details.aspx?id=<%# Eval("id") %>">modifica</a>
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
                                    <th id="Th1" runat="server" style="width: 300px">
                                        Tour
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
