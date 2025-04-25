<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="cont_stp_list.aspx.cs" Inherits="RentalInRome.admin.cont_stp_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" Select="new (id, page_name)" TableName="CONT_TB_STP" OrderBy="page_name">
    </asp:LinqDataSource>
    <div id="fascia1">
        <div class="pannello_fascia1">
            <div style="clear: both">
                <h1>Lista pagine statiche </h1>
                <div class="bottom_agg">
                    <a id="lnkNew" href="cont_stp_details.aspx?id=0"><span>+ Nuovo</span></a>
                   <%--  <asp:LinkButton ID="lnkNew" runat="server" OnClick="lnkNew_Click"><span>+ Nuovo</span></asp:LinkButton>--%>
                </div>
               
            </div>
            <div style="clear: both">
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
                    <ItemTemplate>
                        <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
                            </td>
                            <td>
                                <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("page_name") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="cont_stp_details.aspx?id=<%# Eval("id") %>">modifica</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
                            </td>
                            <td>
                                <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("page_name") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="cont_stp_details.aspx?id=<%# Eval("id") %>">modifica</a>
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
                                        Nome Pagina
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
