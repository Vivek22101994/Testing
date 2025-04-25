<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_blog.Master" AutoEventWireup="true" CodeBehind="pg_blogArticleDett.aspx.cs" Inherits="RentalInRome.pg_blogArticleDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="/uc/ucBlogArticleList.ascx" TagName="ucBlogArticleList" TagPrefix="uc1" %>
<%@ Register Src="/uc/ucBlogZoneList.ascx" TagName="ucBlogZoneList" TagPrefix="uc1" %>
<%@ Register Src="/uc/ucBlogTagCloud.ascx" TagName="ucBlogTagCloud" TagPrefix="uc1" %>
<%@ Register Src="/uc/ucBlogDateList.ascx" TagName="ucBlogDateList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title><%= currArticle.metaTitle%></title>
    <meta name="description" content="<%=currArticle.metaDescription %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script src="/jquery/plugin/jquerytools/scrollable.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HfId" Value="0" runat="server" />
    <asp:HiddenField ID="HfpidLang" Value="it" runat="server" />
    <div id="blogMain">
        <div class="blogPost" id="blogPostDettaglio">
            <h3 class="blogPostTit">
                <%= currArticle.title%>
            </h3>
            <div class="social" id="socialPostDett">
                <!-- AddThis Button BEGIN -->
                <div class="addthis_toolbox addthis_default_style ">
                    <a class="addthis_button_google_plusone"></a>
                    <a class="addthis_counter addthis_pill_style"></a>
                </div>
                <script type="text/javascript">
                    $(function () {
                        $(window).load(function () {
                            $.getScript("http://s7.addthis.com/js/250/addthis_widget.js#pubid=ra-4e33f01370a4c480", function (data, textStatus, jqxhr) {
                            });
                        });
                    });
                </script>
                <!-- AddThis Button END -->
            </div>
            <div class="postDett" id="dettPostDett">
                <span class="postAuthor" id="authorPostDett">pubblicato da: <strong>Federico</strong></span> <span class="postDate" id="datePostDett">il <em>15/05/2012</em></span>
            </div>
            <div class="nulla">
            </div>
            <!-- INIZIO GALLERY DA SISTEMARE -->
            <div class="blogPostImg">
                <img id="galleryMainImg" src="/<%=currArticle.imgBanner %>" alt="via del corso" />
            </div>
            <asp:ListView ID="LV_gallery" runat="server" GroupItemCount="5">
                <ItemTemplate>
                    <a id="mediaItem_<%# Eval("id") %>" onclick="galleryLoadItem(<%# Eval("id") %>)" class="galleryBlogThumb"><span seq="<%# Eval("id") %>" imgbig="/<%# Eval("img_banner") %>" style="display: none;"></span>
                        <img src="/<%# Eval("img_thumb") %>" alt="<%# Eval("code") %>" />
                    </a>
                </ItemTemplate>
                <GroupTemplate>
                    <div style="float: left; height: 96px; position:relative;">
                        <a id="itemPlaceholder" runat="server" />
                    </div>
                </GroupTemplate>
                <EmptyDataTemplate>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <div id="galleryBlogNav">
                        <a class="prev"></a>
                        <div class="scrollable galleryDettscroll" id="galleryBlogThumbsCont">
                            <div class="items" id="items_galleryBlogNav">
                                <div id="groupPlaceholder" runat="server" />
                            </div>
                        </div>
                        <a class="next"></a>
                    </div>
                </LayoutTemplate>
            </asp:ListView>
            <script type="text/javascript">
                function galleryZoom() {
                    var imgSrc = $("#galleryMainImg").attr("src");
                    openShadowboxImg(imgSrc);
                }
                function galleryLoadItem(index) {
                    if ($("#galleryMainImg").attr("currindex") == "" + index) return;
                    var imgSrc = $("#mediaItem_" + index + " span").attr("imgbig");
                    $("#gallery_dettaglio .zoom").hide();
                    $("#galleryMainImg").fadeTo("medium", 0.5, function () {
                        var img = new Image();
                        img.onload = function () {
                            $("#galleryMainVideoCont").html("");
                            $("#galleryMainVideoCont").hide();
                            $("#gallery_dettaglio").show();
                            $("#gallery_dettaglio .zoom").show();
                            $("#galleryMainImg").fadeTo("fast", 1);
                            $("#galleryMainImg").attr("src", imgSrc);
                            $(".mediaitemthumb").removeClass("active");
                            $("#mediaItem_" + index + " img").addClass("active");
                        };
                        img.src = imgSrc;
                    });
                    $("#galleryMainImg").attr("currindex", "" + index);
                }
            </script>
            <script type="text/javascript">
                $(function () {
                    // initialize scrollable
                    $("#galleryBlogThumbsCont").scrollable();
                });
            </script>
            <!-- FINE GALLERY DA SITEMARE -->
            <div class="blogPostTxt" id="txtPostDett">
                <%= currArticle.description%>
            </div>
            <div id="daVedereDett">
                <h3 class="daVedereDettTit">
                    Da vedere nelle vicinanze</h3>
                <div id="daVedereDettScroll_navigator">
                </div>
                <div id="daVedereDettScroll">
                    <div class="items" id="daVedereDettItems">
                        <div class="daVedereDett_item">
                            <a href="#"><span class="overflowImg">
                                <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                            </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">This luxury apartment has a stunning view of the Trevi Fountain. It is located in Piazza dei Crociferi. a square located about 20 meters from the well-known fountain in Rome.
                                <br />
                                <br />
                                The apartment is situated on the 4th floor of a historic building without elevator, in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">Leggi tutto...</span> </a>
                        </div>
                        <div class="daVedereDett_item">
                            <a href="#"><span class="overflowImg">
                                <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                            </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">This luxury apartment has a stunning view of the Trevi Fountain. It is located in Piazza dei Crociferi. a square located about 20 meters from the well-known fountain in Rome.
                                <br />
                                <br />
                                The apartment is situated on the 4th floor of a historic building without elevator, in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">Leggi tutto...</span> </a>
                        </div>
                        <div class="daVedereDett_item">
                            <a href="#"><span class="overflowImg">
                                <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                            </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">This luxury apartment has a stunning view of the Trevi Fountain. It is located in Piazza dei Crociferi. a square located about 20 meters from the well-known fountain in Rome.
                                <br />
                                <br />
                                The apartment is situated on the 4th floor of a historic building without elevator, in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">Leggi tutto...</span> </a>
                        </div>
                        <div class="daVedereDett_item">
                            <a href="#"><span class="overflowImg">
                                <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                            </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">This luxury apartment has a stunning view of the Trevi Fountain. It is located in Piazza dei Crociferi. a square located about 20 meters from the well-known fountain in Rome.
                                <br />
                                <br />
                                The apartment is situated on the 4th floor of a historic building without elevator, in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">Leggi tutto...</span> </a>
                        </div>
                        <div class="daVedereDett_item">
                            <a href="#"><span class="overflowImg">
                                <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                            </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">This luxury apartment has a stunning view of the Trevi Fountain. It is located in Piazza dei Crociferi. a square located about 20 meters from the well-known fountain in Rome.
                                <br />
                                <br />
                                The apartment is situated on the 4th floor of a historic building without elevator, in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">Leggi tutto...</span> </a>
                        </div>
                        <div class="daVedereDett_item">
                            <a href="#"><span class="overflowImg">
                                <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                            </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">This luxury apartment has a stunning view of the Trevi Fountain. It is located in Piazza dei Crociferi. a square located about 20 meters from the well-known fountain in Rome.
                                <br />
                                <br />
                                The apartment is situated on the 4th floor of a historic building without elevator, in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">Leggi tutto...</span> </a>
                        </div>
                    </div>
                </div>
                <script type="text/javascript">
                    $(document).ready(function () {
                        $("#daVedereDettScroll").scrollable({ size: 1, clickable: false, circular: true, keyboard: 'static' }).navigator("#daVedereDettScroll_navigator").autoscroll({ circular: true, autoplay: true });
                    });
                </script>
            </div>
            <div id="commentListCont">
                <h3 class="daVedereDettTit">
                    Commenti</h3>
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModBlog.DCmodBlog" TableName="dbBlogCommentTBLs" OrderBy="commentDate desc" EntityTypeName="" Where="pidArticle == @pidArticle &amp;&amp; isActive == @isActive">
                    <WhereParameters>
                        <asp:ControlParameter ControlID="HfId" Name="pidArticle" PropertyName="Value" Type="Int64" />
                        <asp:Parameter DefaultValue="1" Name="isActive" Type="Int32" />
                    </WhereParameters>
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
                    <ItemTemplate>
                        <div class="commento list">
                            <div class="commCont">
                                <div style="float: left; width: 582px;">
                                    <span class="userName">
                                        <%# Eval("nameFull")%> </span><span class="postDate">
                                            <%# ((DateTime?)Eval("commentDate")).formatCustom("#dd# #MM# #yy#", 1, "")%></span>
                                    <div class="nulla">
                                    </div>
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
                                        <span class="aptCommName">Relativo alla via:&nbsp;<a href="#">Via del Corso</a></span>
                                        <div class="nulla">
                                        </div>
                                    </asp:PlaceHolder>
                                </div>
                                <div class="nulla">
                                </div>
                                <div class="commentoTxt">
                                    <%# Eval("commentBody")%>                                
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <span class="testo_nodata" style="font-size: 14px;">No data was returned.</span>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div id="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                </asp:ListView>
                <div class="nulla">
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <h3 class="daVedereDettTit">
                            Lascia il tuo commento</h3>
                        <div class="editComment" id="pnlCommentForm" runat="server">
                            <div style="margin: 15px 0px 15px 0;" class="nomeComm">
                                <strong>Nome:</strong>
                                <asp:TextBox ID="txt_nameFull" runat="server" Width="300"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_nameFull" CssClass="errore_form" ValidationGroup="commentSend" Display="Dynamic" ErrorMessage="// obbligatorio"></asp:RequiredFieldValidator>
                            </div>
                            <div style="margin: 15px 0px 15px 0;" class="nomeComm">
                                <strong>Email:</strong>
                                <asp:TextBox ID="txt_email" runat="server" Width="300"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_email" CssClass="errore_form" ValidationGroup="commentSend" Display="Dynamic" ErrorMessage="// obbligatorio"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="rev_email" ControlToValidate="txt_email" ErrorMessage="// Formato non valido." Display="Dynamic" ValidationGroup="commentSend" CssClass="errore_form" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />
                            </div>
                            <div class="userType">
                                <asp:RadioButtonList ID="rbtList_typeCode" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="2" TextAlign="Right">
                                    <asp:ListItem Value="m" Text="Maschio" class="testo_blog" Selected="True" style="margin-right: 10px;"></asp:ListItem>
                                    <asp:ListItem Value="f" Text="Femmina" class="testo_blog"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="nulla">
                            </div>
                            <asp:TextBox ID="txt_commentBody" runat="server" TextMode="MultiLine" Style="width: 601px; height: 80px;"></asp:TextBox>
                            <div class="nulla">
                            </div>
                            <div id="Captcha">
                                <telerik:RadCaptcha ID="RadCaptcha1" runat="server" ValidatedTextBoxID="txtCaptcha" EnableRefreshImage="true" ErrorMessage="" ProtectionMode="InvisibleTextBox" ValidationGroup="commentSend">
                                    <CaptchaImage ImageCssClass="imageClass" RenderImageOnly="true" />
                                </telerik:RadCaptcha>
                                <span id="RadCaptchaCheck" runat="server" visible="false" class="alertErrorSmall" style="width: 300px; float: none;">Page not valid. You are a spammer!</span>
                            </div>
                            <div class="nulla">
                            </div>
                            <asp:LinkButton ID="lnk_commentSend" runat="server" CssClass="btn" OnClick="lnk_commentSend_Click" ValidationGroup="commentSend" Style="float: right; margin-right: 0;">Invia il tuo feedback</asp:LinkButton>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="editComment" id="pnlCommentSent" runat="server" visible="false">
                            <span class="titform">Grazie per aver inviato il tuo commento.<br />
                                <span style="color: #C1D0D4;">Sarà inserito al più presto.</span></span>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
        </div>
    </div>
    <div id="blogColDx">
        <div class="boxblogColDx" id="aptsVicini">
            <h4 class="titblogColDx">
                Appartamenti vicini
            </h4>
            <div id="aptViciniScroll">
                <a class="prev" href="#"></a>
                <a class="next" href="#"></a>
                <div id="scrollDaVedere">
                    <div class="scrollDaVedere_item">
                        <a href="#"><span class="imgDaVedereCont">
                            <img src="/images/altare-patria.jpg" alt="altare della patria" />
                        </span><span style="float: left; display: block;"><span class="nomeDaVedere">Appartamento</span> <span class="doveDaVedere">vicino a <strong>Piazza Venezia</strong></span> </span><span class="przAptBlog"><span>da<br />
                            a notte</span> <strong>150€</strong> </span><span class="nulla" style="display: block;"></span><span class="btn-prenota-rental-blog">Prenota ora su </span></a>
                    </div>
                </div>
            </div>
        </div>
        <div class="boxblogColDx">
            <uc1:ucBlogZoneList runat="server" ID="ucBlogZoneList1" />
        </div>
        <uc1:ucBlogArticleList runat="server" ID="ucBlogArticleList" />
        <uc1:ucBlogTagCloud runat="server" ID="ucBlogTagCloud" />
        <uc1:ucBlogDateList runat="server" ID="ucBlogDateList" />
    </div>
</asp:Content>
