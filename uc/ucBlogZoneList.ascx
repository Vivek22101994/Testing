<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBlogZoneList.ascx.cs" Inherits="RentalInRome.uc.ucBlogZoneList" %>

<asp:ListView ID="LV" runat="server">
    <ItemTemplate>
        <li>
            <a href="/<%# Eval ("pagePath")%>">
                <%# Eval ("title") %>
            </a>
        </li>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <h4 class="titblogColDx">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
        </h4>
        <ul class="menublogColDx">
            <li id="itemPlaceholder" runat="server" />
        </ul>
    </LayoutTemplate>
</asp:ListView>

