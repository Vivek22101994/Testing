<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_estateListByType.aspx.cs" Inherits="RentalInRome.stp_estateListByType" %>

<%@ Register Src="uc/UC_staffInLang.ascx" TagName="UC_staffInLang" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_search.ascx" TagName="UC_search" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title><%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text.Replace("\"","'") %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text.Replace("\"","'") %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script type="text/javascript">
        function RNT_changePage(page) {
            $("#hf_currPage").val("" + page);
            RNT_fillList("list");
        }
        function RNT_fillList(action) {
            if (action == "list") SITE_showLoader();
            var _url = "/webservice/rnt_estateListByType.aspx";
            _url += "?SESSION_ID=<%= CURRENT_SESSION_ID %>";
            _url += "&lang=<%= CurrentLang.ID %>";
            _url += "&currType=" + $("#<%= HF_currType.ClientID %>").val();
            _url += "&title=" + $("#txt_searchTitle").val();
            _url += "&orderBy=" + $("#<%= HF_orderBy.ClientID %>").val();
            _url += "&orderHow=" + $("#<%= HF_orderHow.ClientID %>").val();
            _url += "&numPerPage=" + $("#<%= HF_numPerPage.ClientID %>").val();
            _url += "&currPage=" + $("#hf_currPage").val();
            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function (html) {
                    $("#estateListCont").html(html);
                    if (action == "list") {
                        $.scrollTo($("#estateListCont"), 500);
                        SITE_hideLoader();
                    }
                    set_switchThumb();
                }
            });
        }
        $(window).load(function () {
            RNT_fillList("first");
        });
        function RNT_orderBy(order, how) {
            var _orderByCont = $("#<%= HF_orderBy.ClientID %>");
            var _orderHowCont = $("#<%= HF_orderHow.ClientID %>");
            var _oldOrderBy = _orderByCont.val();
            var _oldOrderHow = _orderHowCont.val();
            var _newOrderBy = order;
            var _newOrderHow = how == undefined ? "" : how;
            if (_newOrderHow == "") {
                if (_newOrderBy == _oldOrderBy)
                    _newOrderHow = _oldOrderHow == "asc" ? "desc" : "asc";
                else if (_newOrderBy == "price")
                    _newOrderHow = "asc";
                else if (_newOrderBy == "vote")
                    _newOrderHow = "desc";
                else if (_newOrderBy == "title")
                    _newOrderHow = "asc";
            }
            _orderByCont.val(_newOrderBy);
            _orderHowCont.val(_newOrderHow);
            RNT_fillList("list");
            $("#hl_orderBy_price").attr("class", (_newOrderBy == "price") ? _newOrderHow : "");
            $("#hl_orderBy_vote").attr("class", (_newOrderBy == "vote") ? _newOrderHow : "");
            $("#hl_orderBy_title").attr("class", (_newOrderBy == "title") ? _newOrderHow : "");
        }
        //RNT_orderBy("<%= HF_orderBy.Value %>", "<%= HF_orderHow.Value %>");
        function set_switchThumb() {

            $("a.switch_thumb").toggle(function () {
                $(this).addClass("swap");
                $("ul.display").fadeOut("fast", function () {
                    $(this).fadeIn("fast").removeClass("dett_view");
                    $(this).fadeIn("fast").addClass("thumb_view");
                });
            }, function () {
                $(this).removeClass("swap");
                $("ul.display").fadeOut("fast", function () {
                    $(this).fadeIn("fast").removeClass("thumb_view");
                    $(this).fadeIn("fast").addClass("dett_view");
                });
            });

        }
    </script>
    <style type="text/css">
        #search_int
        {
            background-image: url("../images/css/ombra_tit_box2.png");
            background-position: left bottom;
            background-repeat: no-repeat;
            margin-left: -27px;
            margin-top: 26px;
            padding-bottom: 7px;
            width: 302px;
            border: none;
            background-color: none;
            -moz-border-radius: 0;
            -webkit-border-radius: 0;
            -khtml-border-radius: 0;
            border-radius: 0;
        }
        #search_int > div
        {
            background-color: #DADAE5;
            padding: 9px 27px 18px 15px;
            width: 260px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_id" runat="server" Value="" />
    <asp:HiddenField ID="HF_currType" runat="server" Value="" />
    <asp:HiddenField ID="HF_lang" runat="server" Value="" />
    <asp:HiddenField ID="HF_unique" runat="server" Value="" />
    <asp:HiddenField ID="HF_searchTitle" runat="server" Value="" />
    <asp:HiddenField ID="HF_orderBy" runat="server" Value="" />
    <asp:HiddenField ID="HF_orderHow" runat="server" Value="" />
    <asp:HiddenField ID="HF_numPerPage" runat="server" Value="10" />
    <asp:HiddenField ID="HF_currPage" runat="server" Value="1" />
    <h3 class="underlined" style="margin-left: 14px;">
        <%= ltr_title.Text%></h3>
    <div class="nulla">
    </div>
    <div id="lista_search" class="zona">
        <div class="sx">
            <div class="txt_zone" style="margin-top: 0;">
                <div>
                    <%= ltr_description.Text%>
                </div>
            </div>
            <div id="search_int">
                <div>
                    <div class="tabTitle" style="color: #807487; float: left; font-size: 17px;">
                        <%=CurrentSource.getSysLangValue("lblSearch")%>
                    </div>
                    <uc2:UC_search ID="UC_search1" runat="server" />
                    <div class="nulla">
                    </div>
                </div>
            </div>
            <uc1:UC_staffInLang ID="UC_staffInLang" runat="server" />
            <div class="nulla">
            </div>
        </div>
        <div class="dx">
            <div class="ordina quickSearch" style="width: 555px;">
                <span class="ord_sx" style="margin-right: 5px; color: #333366; font-size: 13px;"><em>
                    <%=CurrentSource.getSysLangValue("lblQuickSearch", "Quick Search")%></em> </span>
                <a onclick='RNT_fillList("list")' class="quickSearchGo"></a>
                <input id="txt_searchTitle" type="text" value="<%= HF_searchTitle.Value %>" name="quicksearch" />
            </div>
            <div class="lista lista_dettagli" id="estateListCont">
                <input type="hidden" id="hf_currPage" value="<%=HF_currPage.Value %>" />
                <div class="loadingSrc">
                    <span>
                        <%=CurrentSource.getSysLangValue("lblLoadingData")%>
                    </span>
                </div>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
</asp:Content>
