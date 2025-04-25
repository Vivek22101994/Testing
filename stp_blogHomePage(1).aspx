<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_blog.Master" AutoEventWireup="true" CodeBehind="stp_blogHomePage.aspx.cs" Inherits="RentalInRome.stp_blogHomePage" %>

<%@ Register Src="/uc/ucBlogArticleList.ascx" TagName="ucBlogArticleList" TagPrefix="uc1" %>
<%@ Register Src="/uc/ucBlogZoneList.ascx" TagName="ucBlogZoneList" TagPrefix="uc1" %>
<%@ Register Src="/uc/ucBlogTagCloud.ascx" TagName="ucBlogTagCloud" TagPrefix="uc1" %>
<%@ Register Src="/uc/ucBlogDateList.ascx" TagName="ucBlogDateList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%= currStp.meta_title%><%= currTag != 0 ? " - " + blogUtils.getTag_title(currTag, App.LangID, "") : ""%></title>
    <meta name="description" content="<%= currTag != 0 ? contUtils.getLabel("blogMetaDescriptionListaTag").Replace("#tag#", blogUtils.getTag_title(currTag, App.LangID, "")) : currStp.meta_description%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script src="/jquery/plugin/jquerytools/scrollable.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_currTag" runat="server" Visible="false" />
    <div id="blogMain">
        <h1 class="titMain">
            <%= currStp.title%>
        </h1>
        <asp:ListView ID="LV" runat="server">
            <ItemTemplate>
                <div class="blogPost">
                    <h3 class="blogPostTit">
                        <a href="/<%# Eval ("pagePath")%>">
                            <%# Eval ("title") %></a></h3>
                    <div class="postDett">
                        <div style="float: left;">
                            <span class="postAuthor">pubblicato da: <strong>Federico</strong></span> <span class="postDate">il <em>
                                <%# ((DateTime?)Eval("publicDate")).formatCustom("#dd# #MM# #yy#", App.LangID, "")%></em></span>
                        </div>
                        <span class="commentsNum">10</span>
                    </div>
                    <div class="nulla">
                    </div>
                    <%# contUtils.getImg(Eval("imgBanner") + "", "", "<div class=\"blogPostImg\"><img src=\"#imgPath#\" alt=\"\" /></div>", true)%>
                    <div class="blogPostTxt">
                        <%# Eval("summary")%></span>
                    </div>
                    <a class="btn" href="/<%# Eval ("pagePath")%>"><span>Leggi tutto </span></a>
                    <div class="nulla">
                    </div>
                </div>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div class="box_testo_brand" style="border-bottom: 0;">
                    <span class="tx">
                        <%=contUtils.getLabel("blogNonCiSonoArticoli")%>
                    </span>
                </div>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <div id="itemPlaceholder" runat="server" />
            </LayoutTemplate>
        </asp:ListView>
    </div>
    <div id="blogColDx">
        <div class="boxblogColDx">
            <uc1:ucBlogZoneList runat="server" ID="ucBlogZoneList1" />
        </div>
        <div class="boxblogColDx" style="position: relative;">
            <h4 class="titblogColDx">
                Da vedere a Roma</h4>
            <div id="daVedereCont">
                <div id="scrollDaVedere_navigator">
                </div>
                <div id="scrollDaVedere">
                    <div class="items" id="items_daVedere">
                        <div class="scrollDaVedere_item">
                            <a href="#"><span class="imgDaVedereCont">
                                <img src="/images/altare-patria.jpg" alt="altare della patria" />
                            </span><span class="nomeDaVedere">Altare della Patria</span> <span class="doveDaVedere">vicino a <strong>Piazza Venezia</strong></span> </a>
                        </div>
                        <div class="scrollDaVedere_item">
                            <a href="#"><span class="imgDaVedereCont">
                                <img src="/images/altare-patria.jpg" alt="altare della patria" />
                            </span><span class="nomeDaVedere">Altare della Patria</span> <span class="doveDaVedere">vicino a <strong>Piazza Venezia</strong></span> </a>
                        </div>
                        <div class="scrollDaVedere_item">
                            <a href="#"><span class="imgDaVedereCont">
                                <img src="/images/altare-patria.jpg" alt="altare della patria" />
                            </span><span class="nomeDaVedere">Altare della Patria</span> <span class="doveDaVedere">vicino a <strong>Piazza Venezia</strong></span> </a>
                        </div>
                        <div class="scrollDaVedere_item">
                            <a href="#"><span class="imgDaVedereCont">
                                <img src="/images/altare-patria.jpg" alt="altare della patria" />
                            </span><span class="nomeDaVedere">Altare della Patria</span> <span class="doveDaVedere">vicino a <strong>Piazza Venezia</strong></span> </a>
                        </div>
                    </div>
                </div>
                <script type="text/javascript">
                    $(document).ready(function () {
                        $("#scrollDaVedere").scrollable({ size: 1, clickable: false, circular: true, keyboard: 'static' }).navigator("#scrollDaVedere_navigator").autoscroll({ circular: true, autoplay: true });
                    });
                </script>
            </div>
            <a class="btn" href="#" id="altroDaVedere"><span>Altro </span></a>
            
            <uc1:ucBlogArticleList runat="server" ID="ucBlogArticleList" />
            <uc1:ucBlogDateList runat="server" ID="ucBlogDateList" />
            <uc1:ucBlogTagCloud runat="server" ID="ucBlogTagCloud" />
        </div>
    </div>
</asp:Content>
