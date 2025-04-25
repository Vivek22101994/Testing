﻿<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_other_zone_apts.aspx.cs" Inherits="RentalInRome.stp_other_zone_apts" %>

<%@ Register Src="uc/UC_menu_breadcrumb.ascx" TagName="UC_menu_breadcrumb" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">

    <script type="text/javascript">
        function RNT_changePage(page) {
            $("#<%= HF_session.ClientID %>").val("");
            $("#hf_currPage").val("" + page);
            RNT_fillList("list");
        }
        function RNT_fillList(action) {
            if (action == "list") SITE_showLoader();
            var _url = "/webservice/rnt_estate_list_zone.aspx";
            _url += "?action=" + action;
            _url += "&lang=<%= CurrentLang.ID %>";
            _url += "&mode=zone";
            _url += "&session=" + $("#<%= HF_session.ClientID %>").val();
            _url += "&currZone=19";
            _url += "&currConfig=" + $("#<%= HF_currConfig.ClientID %>").val();
            _url += "&currPage=" + $("#hf_currPage").val();
            _url += "&numPerPage=" + $("#<%= HF_numPerPage.ClientID %>").val();
            _url += "&orderBy=" + $("#<%= HF_orderBy.ClientID %>").val();
            _url += "&orderHow=" + $("#<%= HF_orderHow.ClientID %>").val();
            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function(html) {
                    $("#estateListCont").html(html);
                    if (action == "list") {
                        $.scrollTo($(".ordina"), 500);
                        SITE_hideLoader();
                    }
                }
            });
        }
        $(window).load(function() {
            RNT_fillList("first");
        });
        function RNT_orderBy(order) {
            var _orderByCont = $("#<%= HF_orderBy.ClientID %>");
            var _orderHowCont = $("#<%= HF_orderHow.ClientID %>");
            var _oldOrderBy = _orderByCont.val();
            var _oldOrderHow = _orderHowCont.val();
            var _newOrderBy = order;
            var _newOrderHow = "";
            if (_newOrderBy == _oldOrderBy)
                _newOrderHow = _oldOrderHow == "asc" ? "desc" : "asc";
            else if (_newOrderBy == "price")
                _newOrderHow = "asc";
            else if (_newOrderBy == "vote")
                _newOrderHow = "desc";
            else if (_newOrderBy == "title")
                _newOrderHow = "asc";
            _orderByCont.val(_newOrderBy);
            _orderHowCont.val(_newOrderHow);
            RNT_fillList("list");
            $("#hl_orderBy_price").attr("class", (_newOrderBy == "price") ? _newOrderHow : "");
            $("#hl_orderBy_vote").attr("class", (_newOrderBy == "vote") ? _newOrderHow : "");
            $("#hl_orderBy_title").attr("class", (_newOrderBy == "title") ? _newOrderHow : "");
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_id" runat="server" Value="" />
    <asp:HiddenField ID="HF_lang" runat="server" Value="" />
    <asp:HiddenField ID="HF_unique" runat="server" Value="" />
    <asp:HiddenField ID="HF_session" runat="server" Value="false" />
    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
    <asp:HiddenField ID="HF_numPers" runat="server" Value="0" />
    <asp:HiddenField ID="HF_numPerPage" runat="server" Value="10" />
    <asp:HiddenField ID="HF_activeZones" runat="server" Value="" />
    <asp:HiddenField ID="HF_currZone" runat="server" Value="0" />
    <asp:HiddenField ID="HF_activeConfigs" runat="server" Value="1|2" />
    <asp:HiddenField ID="HF_currConfig" runat="server" Value="" />
    <asp:Literal ID="ltr_activeConfigsControl" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_activeZonesControl" runat="server" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_orderBy" runat="server" Value="" />
    <asp:HiddenField ID="HF_orderHow" runat="server" Value="" />
    <div class="banner_lista">
        <div class="tit">
            <div>
                <h1>
                <%=CurrentSource.getSysLangValue("lblApartmentsInRome")%><br />
                <%= ltr_title.Text%></h1>
            </div>
        </div>
        <img src="/<%= ltr_img_banner.Text%>" class="ban_int" />
    </div>
    <div id="lista_search" class="zona">
        <div class="sx">
            <div class="txt_zone">
                <div>
                   
                    <%= ltr_description.Text%>
                </div>
            </div>          
            <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" currType="sxZone" currZone="-1" />
        </div>
        <div class="dx">
            <uc1:UC_menu_breadcrumb ID="UC_menu_breadcrumb1" runat="server" />
            <div class="ordina">
                <span class="ord_sx">Ordina</span>
                <div class="ord_dx">
                    <a href="javascript:RNT_orderBy('title')" id="hl_orderBy_title">
                        <span>
                            <%=CurrentSource.getSysLangValue("lblName")%></span></a>
                    <a href="javascript:RNT_orderBy('price')" id="hl_orderBy_price">
                        <span>
                            <%=CurrentSource.getSysLangValue("lblPrice")%></span></a>
                    <a href="javascript:RNT_orderBy('vote')" id="hl_orderBy_vote">
                        <span>
                            <%=CurrentSource.getSysLangValue("lblRating")%></span></a>
                </div>
            </div>
            <div class="ico">
                <a class="switch_thumb" href="#">Switch Thumb</a>
                <%--<a href="#" class="ico_map"></a>--%>
            </div>
            <div class="lista lista_dettagli" id="estateListCont">
                <input type="hidden" id="hf_currPage" value="1" />
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
