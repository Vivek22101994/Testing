<%@ Page Title="" Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="clientList.aspx.cs" Inherits="RentalInRome.affiliatesarea.clientList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <title>
        <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%></title>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "clientDett.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(700);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                rwdUrl.maximize();
            }
            return false;
        }
        function rwdUrl_OnClientClose(sender, eventArgs) {
            $find('<%= pnlFascia.ClientID %>').ajaxRequest('rwdUrl_Closing');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
            <%=CurrentSource.getSysLangValue("lblManageYourClients")%>
        </telerik:RadCodeBlock>
    </h3>
    <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="reslist" OnAjaxRequest="pnlFascia_AjaxRequest">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <asp:Literal ID="ltrLDSfiltter" runat="server" Visible="false"></asp:Literal>
            <asp:Literal ID="ltrLDSorderBy" runat="server" Visible="false" Text="nameFull"></asp:Literal>
            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModAuth.DCmodAuth" TableName="dbAuthClientTBLs" OrderBy="docIssueDate desc" EntityTypeName="">
            </asp:LinqDataSource>
            <a href="#" onclick="return setUrl('dett', '0')" class="buttprosegui" style="margin-top: 15px;">
                <%=CurrentSource.getSysLangValue("lblCreateNew")%>
            </a>
            <div class="table_fascia">
                <table border="0" cellpadding="0" cellspacing="0" style="">
                    <tr style="text-align: left">
                        <th style="width: 50px">
                            <%=CurrentSource.getSysLangValue("lblCode")%>
                        </th>
                        <th style="width: 200px">
                            <%=CurrentSource.getSysLangValue("reqFullName")%>
                        </th>
                        <th style="width: 200px">
                            <%=CurrentSource.getSysLangValue("reqEmail")%>
                        </th>
                        <th style="width: 130px">
                            <%=CurrentSource.getSysLangValue("lblRegistrationDate")%> </th>
                        <th></th>
                    </tr>
                    <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand">
                        <ItemTemplate>
                            <tr class="ag_altern" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                <td>
                                    <span>
                                        <%# Eval("code") %></span>
                                </td>
                                <td>
                                    <span class="ag_userico"><%# Eval("nameFull")%></span>
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("contactEmail")%></span>
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("createdDate")%></span>
                                </td>
                                <td style="text-align:right;">
                                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                    <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" style="text-decoration: none; border: 0 none;">Details</a>
                               &nbsp;|&nbsp;
                                    <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('You are about to delete the client?');" Style="text-decoration: none; border: 0 none;">Delete</asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                <td>
                                    <span>
                                        <%# Eval("code") %></span>
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("nameFull")%></span>
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("contactEmail")%></span>
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("createdDate")%></span>
                                </td>
                                <td style="text-align:right;">
                                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                    <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" style="text-decoration: none; border: 0 none;">Details </a>
                                &nbsp;|&nbsp;
                                    <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('You are about to delete the client?');" Style="text-decoration: none; border: 0 none;">Delete</asp:LinkButton>
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
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </LayoutTemplate>
                    </asp:ListView>
                </table>
            </div>
            <div class="ordina pageCount">
                <asp:DataPager ID="DataPager1" runat="server" PageSize="50" PagedControlID="LV" style="border-right: medium none;">
                    <Fields>
                        <asp:NumericPagerField ButtonCount="20" />
                    </Fields>
                </asp:DataPager>
            </div>
        </telerik:RadCodeBlock>
    </telerik:RadAjaxPanel>
</asp:Content>
