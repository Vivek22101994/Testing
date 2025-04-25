<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_header.ascx.cs" Inherits="RentalInRome.uc.UC_header" %>
<%@ Register Src="UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<div id="header">
    <a href="<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>" class="logo">
        <img src="/images/css/logo.gif" title="Rent apartments in Rome, Italy" alt="Rent apartments in Rome, Italy">
    </a>
    <%--<uc1:UC_static_block ID="UC_static_block2" runat="server" BlockID="11" />--%>
    <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_TBL_LANGs" Where="is_active == 1 &amp;&amp; is_public == 1">
    </asp:LinqDataSource>
    <div id="menuLang">
        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemDataBound="LV_ItemDataBound">
            <ItemTemplate>
                <li>
                    <asp:HyperLink ID="HL" ToolTip='<%# Eval("lang_title") %>' runat="server">
                    <img src="/<%# Eval("img_thumb") %>" title="<%# Eval("lang_title") %>" alt="<%# Eval("lang_title") %>" />
                    </asp:HyperLink>
                </li>
                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                <asp:Label ID="lbl_common_name" Visible="false" runat="server" Text='<%# Eval("common_name") %>' />
            </ItemTemplate>
            <EmptyDataTemplate>
                <div id="menuLang">
                    &nbsp;</div>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <ul>
                    <li id="itemPlaceholder" runat="server" />
                </ul>
            </LayoutTemplate>
        </asp:ListView>
        <div class="linkareaclienti" id="pnl_agentAuthNO" runat="server" visible="false">
            <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%>:
            <a href="/affiliatesarea/login.aspx?referer=<%=CurrentSource.getPagePath(((mainBasePage)this.Page).PAGE_REF_ID + "", ((mainBasePage)this.Page).PAGE_TYPE, CurrentLang.ID.ToString()).urlEncode() %>">Login</a>
            &nbsp;|&nbsp;
            <a href="<%=CurrentSource.getPagePath("35", "stp", CurrentLang.ID.ToString()) %>">Register</a>
        </div>
        <div class="linkareaclienti" id="pnl_agentAuthOK" runat="server" visible="false">
            <%=CurrentSource.getSysLangValue("lblWelcome")%>, 
            <strong style="color:#FE6634;">
                <asp:Literal ID="ltr_agentAuth_nameFull" runat="server"></asp:Literal>
            </strong>
            &nbsp;|&nbsp;
            <a href="/affiliatesarea"><%=CurrentSource.getSysLangValue("lblAffiliatesArea")%></a>
            &nbsp;|&nbsp;
            <a href="/affiliatesarea/login.aspx?logout=true">Logout</a>
        </div>
    </div>
    <div style="float: right;">
        <uc1:UC_static_block ID="UC_static_block1" runat="server" BlockID="3" />
    </div>
    <div class="nulla">
    </div>
    <div id="menuMain">
        <ul>
            <li>
                <a href="<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>" style="border-left: none;">HOME</a>
            </li>
            <li>
                <a href="<%=CurrentSource.getPagePath("5", "stp", CurrentLang.ID.ToString()) %>">
                    <%= CurrentSource.getSysLangValue("lblAboutUs")%></a>
            </li>
            <li>
                <a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString()) %>">
                    <%= CurrentSource.getSysLangValue("lblContacts")%></a>
            </li>
            <li>
                <a href="<%=CurrentSource.getPagePath("9", "stp", CurrentLang.ID.ToString()) %>">
                    <%= CurrentSource.getSysLangValue("menu_Press")%></a>
            </li>
            <li>
                <a href="<%=CurrentSource.getPagePath("24", "stp", CurrentLang.ID.ToString()) %>">
                    <%=CurrentSource.getSysLangValue("menuRomeVillas")%></a>
            </li>
            <li>
                <a target="_blank" href="http://www.rentalcastles.com/">
                    <%=CurrentSource.getSysLangValue("menuRentCastle")%></a>
            </li>
            <li>
                <a target="_blank" href="http://www.millennium-beb.com/">
                    <%=CurrentSource.getSysLangValue("menuBeB")%></a>
            </li>
            <li>
                <a target="_blank" href="http://www.rentalinvenice.com">
                    <%=CurrentSource.getSysLangValue("menuVenice")%></a>
            </li>
            <li>
                <a target="_blank" href="http://www.rentalinflorence.com">
                    <%=CurrentSource.getSysLangValue("menuFlorence")%></a>
            </li> <li>
                <a target="_blank" href="http://www.rentalincortina.com/">
                    <%=CurrentSource.getSysLangValue("menuCortina")%></a>
            </li>
            <li>
                <a href="<%=CurrentSource.getPagePath("10", "stp", CurrentLang.ID.ToString()) %>">
                    <%= CurrentSource.getSysLangValue("menu_LimousineService")%>
                </a>
            </li>
        </ul>
    </div>
    <div class="nulla">
    </div>
</div>
