<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBlogArticleList.ascx.cs" Inherits="RentalInRome.uc.ucBlogArticleList" %>

<asp:ListView ID="LV" runat="server">
    <ItemTemplate>
        <li>
            <a href="/<%# Eval ("pagePath")%>">
                <strong>
                    <%# Eval ("title") %>
                </strong>
                <span>
                    <%# ((DateTime?)Eval("publicDate")).formatCustom("#dd# #MM# #yy#", App.LangID, "")%>
                </span>
            </a>
        </li>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div class="box_blog">
            <span class="titBoxSx">
                <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
            </span>
            <ul class="postrecenti">
                <li id="itemPlaceholder" runat="server" />
            </ul>
        </div>
    </LayoutTemplate>
</asp:ListView>

