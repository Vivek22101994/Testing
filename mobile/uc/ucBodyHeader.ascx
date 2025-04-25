<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBodyHeader.ascx.cs" Inherits="RentalInRome.mobile.uc.ucBodyHeader" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<header class="header">


    <div class="loginAndLangs">
        <asp:ListView ID="LV" runat="server">
            <ItemTemplate>
                <li>
                    <asp:HyperLink ID="HL" ToolTip='<%# Eval("lang_title") %>' runat="server">
                    <img src="/<%# Eval("img_thumb") %>" title="<%# Eval("lang_title") %>" alt="<%# Eval("lang_title") %>" />
                    </asp:HyperLink>
                </li>
                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                <asp:Label ID="lbl_abbr" Visible="false" runat="server" Text='<%# Eval("abbr") %>' />
                <asp:Label ID="lbl_common_name" Visible="false" runat="server" Text='<%# Eval("common_name") %>' />
            </ItemTemplate>
            <EmptyDataTemplate>
                <div id="menuLang">
                    &nbsp;
                </div>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <div id="menuLang" style="width: auto; height:auto;">
                    <ul>
                        <li id="itemPlaceholder" runat="server" />
                    </ul>
                </div>
            </LayoutTemplate>
        </asp:ListView>
    </div>
    <a class="nav-button logoCont" href="/m<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>">
        <img src="/images/css/logo.gif" class="logo" alt="Rental in Rome" /></a>

    <a class="openSearch" data-role="touch" data-enable-swipe="1" data-touchstart="openSearchBox.touchstart"><%= contUtils.getLabel("lblSearchAvailability") %></a>






    <div class="nulla">
    </div>
</header>
<div class="searchBox" id="searchBox" data-title="Search Form" data-init="initForm">
    <a class="closeSearchBox" data-role="touch" data-enable-swipe="1" data-touchstart="closeSearchBox.touchstart"><%= contUtils.getLabel("lblCloseSearch") %></a>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ul data-style="inset" class="searchForm">
                <li class="searchBoxLine">
                    <asp:TextBox ID="txt_title" runat="server" Text=""></asp:TextBox>
                </li>
                <li class="searchBoxLine">
                    <label><%= contUtils.getLabel("lblSelectZone") %>:</label>
                    <asp:DropDownList ID="drp_zone" runat="server">
                    </asp:DropDownList>
                </li>
                <li class="searchBoxLine">
                    <span class="searchBoxCol2 checkIn">
                        <label>Check-In</label>
                        <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" />
                        <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                    </span>
                    <span class="searchBoxCol2 checkOut">
                        <label>Check-Out</label>
                        <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" />
                        <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                    </span>
                </li>
                <li class="searchBoxLine">
                    <span class="searchBoxCol4 nights">
                        <label><%= contUtils.getLabel("lblNights") %></label>
                        <asp:TextBox ID="txt_dtCount" runat="server" Style="width: 44px;"></asp:TextBox>
                    </span>
                    <span class="searchBoxCol4 guests">
                        <label><%= contUtils.getLabel("reqAdults")%></label>
                        <asp:DropDownList ID="drp_numPers_adult" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_numPers_adult_SelectedIndexChanged" Style="width: 44px;">
                        </asp:DropDownList>
                    </span>
                    <span class="searchBoxCol4 guests">
                        <label><%= contUtils.getLabel("reqChildren")%></label>
                        <asp:DropDownList ID="drp_numPers_childOver" runat="server" Style="width: 44px;">
                        </asp:DropDownList>
                    </span>
                    <span class="searchBoxCol4 guests">
                        <label><%= contUtils.getLabel("reqEnfants")%></label>
                        <asp:DropDownList ID="drp_numPers_childMin" runat="server" Style="width: 44px;">
                        </asp:DropDownList>
                    </span>
                </li>
                <li class="searchBoxLine">
                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btnBigMobile" OnClick="lnk_search_Click"><%= contUtils.getLabel("lblSearch")%></asp:LinkButton>
                </li>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
<script>
    var _JSCal_Range_<%= Unique %>;
    function setCal_<%= Unique %>(dtStartInt, dtEndInt) {
        _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", countCont: "#<%= txt_dtCount.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
    }
    <%= ltr_checkCalDates.Text %>
</script>
<script>
    window.openSearchBox = {
        touchstart: function (e) {
            var searchBox = document.getElementById("searchBox");
            //searchBox.style.height = "335px";
            searchBox.style.height = "365px";
        }
    }
    window.closeSearchBox = {
        touchstart: function (e) {
            var searchBox = document.getElementById("searchBox");
            searchBox.style.height = "0";
        }
    }
</script>
<script>
    function initForm() {
        var body = $(".km-pane");
    }
</script>
