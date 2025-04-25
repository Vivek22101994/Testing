<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHeader.ascx.cs" Inherits="RentalInRome.ucMain.ucHeader" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_estateId" runat="server" Value="0" />
<asp:HiddenField ID="HF_estatePath" runat="server" Value="" />
<!-- BEGIN HEADER -->
<header id="header">
    <div id="top-bar">
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <ul id="top-info">
                        <li style="margin-left: 0;">Tel.: +39 06 3220068 </li>
                        <li>Email: <a href="mailto:info@rentalinrome.com">info@rentalinrome.com</a></li>
                    </ul>

                    <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_TBL_LANGs" Where="is_active == 1 &amp;&amp; is_public == 1">
                    </asp:LinqDataSource>
                    <ul id="top-buttons">
                        <li class="linkareaospiti">
                            <i style="margin-right: 5px;" aria-hidden="true" class="fa fa-user"></i> 
                            <%=CurrentSource.getSysLangValue("lblGuestsArea")%>: 
                            <a href="/reservationarea/login.aspx"><strong>Login</strong></a>
                        </li>
                        <li class="linkareaclienti" id="pnl_agentAuthNO" runat="server" visible="false">
                            <i style="margin-right: 5px;" aria-hidden="true" class="fa fa-users"></i>
                            <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%>:
                                    <a href="/affiliatesarea/login.aspx?referer=<%=CurrentSource.getPagePath(((mainBasePage)this.Page).PAGE_REF_ID + "", ((mainBasePage)this.Page).PAGE_TYPE, CurrentLang.ID.ToString()).urlEncode() %>"><strong>Login</strong></a>
                            &nbsp;|&nbsp;
                                        <a href="<%=CurrentSource.getPagePath("35", "stp", CurrentLang.ID.ToString()) %>"><strong><%=CurrentSource.getSysLangValue("lblRegister")%></strong></a>
                        </li>
                        <li class="linkareaclienti" id="pnl_agentAuthOK" runat="server" visible="false">
                            <i style="margin-right: 5px;" aria-hidden="true" class="fa fa-users"></i>
                            <%=CurrentSource.getSysLangValue("lblWelcome")%>, 
                                        <strong style="color: #FE6634;">
                                            <asp:Literal ID="ltr_agentAuth_nameFull" runat="server"></asp:Literal>
                                        </strong>
                            &nbsp;|&nbsp;
                                        <a href="/affiliatesarea"><%=CurrentSource.getSysLangValue("lblAffiliatesArea")%></a>
                            &nbsp;|&nbsp;
                                <a href="/affiliatesarea/login.aspx?logout=true">Logout</a>
                        </li>

                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemDataBound="LV_ItemDataBound">
                            <ItemTemplate>
                                <li id="liLang" runat="server">
                                    <asp:HyperLink ID="HL" ToolTip='<%# Eval("lang_title") %>' runat="server" Text='<%# Eval("lang_title") %>'></asp:HyperLink>
                                </li>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:Label ID="lbl_common_name" Visible="false" runat="server" Text='<%# Eval("common_name") %>' />
                            </ItemTemplate>
                            <LayoutTemplate>
                                <li>
                                    <div class="language-switcher">
                                        <span>
                                            <i class="fa fa-globe"></i>
                                            <asp:Literal ID="ltrCurrLang" runat="server"></asp:Literal>
                                        </span>
                                        <ul>
                                            <li id="itemPlaceholder" runat="server" />
                                        </ul>
                                    </div>
                                </li>
                            </LayoutTemplate>
                        </asp:ListView>
                    </ul>
                </div>
            </div>
        </div>
    </div>

    <div id="nav-section">
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <a href="<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>" class="nav-logo">
                        <img src="/images/logo.png" alt="Rental in Rome" /></a>

                    <!-- BEGIN SOCIAL NETWORKS -->
                    <ul class="social-networks">
                        <li><a href="https://www.facebook.com/rentalinrome" target="_blank"><i class="fa fa-facebook"></i></a></li>
                        <li><a href="https://twitter.com/rentalinrome" target="_blank"><i class="fa fa-twitter"></i></a></li>
                         <li><a href="https://www.instagram.com/rentalinrome/" target="_blank"><i class="fa fa-instagram"></i></a></li>
                   
                    </ul>
                    <!-- END SOCIAL NETWORKS -->

                    <!-- BEGIN SEARCH -->
                    <div id="sb-search" class="sb-search">
                        <div>
                            <input class="sb-search-input search_aptComplete" placeholder="<%= contUtils.getLabel("lblPropertyName", App.LangID, "") %>" type="text" value="" name="search" id="search">
                            <input class="sb-search-submit" type="submit" value="">
                            <i class="fa fa-search sb-icon-search"></i>
                        </div>
                    </div>

                    <script type="text/javascript">
                        var _estateList_<%= Unique %> = new Array();

                        function getEstateXml_<%= Unique %>() {
                            var _xml = $.ajax({
                                type: "GET",
                                url: "/webservice/rnt_estate_list_xml.aspx?lang=<%= CurrentLang.ID %>&SESSION_ID=<%= CURRENT_SESSION_ID %>",
                                dataType: "xml",
                                success: function (xml) {
                                    $(xml).find('item').each(function () {
                                        var _estOpt = {
                                            id: parseInt($(this).find('id').text(), 10),
                                            path: $(this).find('path').text(),
                                            label: $(this).find('title').text(),
                                            pid_zone: parseInt($(this).find('pid_zone').text(), 10)
                                        };
                                        _estateList_<%= Unique %>.push(_estOpt);
                                    });

                                    setAutocomplete_<%= Unique %>();
                                }
                            });
                        }

                        function setAutocomplete_<%= Unique %>() {
                            console.log('here');
                            $(".search_aptComplete").typeahead({
                                source: _estateList_<%= Unique %>,
                                items: 50,
                                displayText: function (item) {
                                    return item.label;
                                },
                                afterSelect: function (item) {
                                    window.location.href = "/" + item.path;
                                }
                            });

                            $(".search_aptComplete").attr('placeholder', '<%= contUtils.getLabel("lblPropertyName", App.LangID,"") %>');
                        }
                        getEstateXml_<%= Unique %>();
                    </script>
                    <!-- END SEARCH -->

                    <!-- BEGIN MAIN MENU -->
                    <nav class="navbar">
                        <button id="nav-mobile-btn"><i class="fa fa-bars"></i></button>

                        <ul class="nav navbar-nav">
                            <li>
                                <a class="active" href="<%=CurrentSource.getPagePath("43", "stp", CurrentLang.ID.ToString()) %>">Covid 19</a>
                            </li>
                            <li>
                                <a href="<%=CurrentSource.getPagePath("24", "stp", CurrentLang.ID.ToString()) %>"><%=CurrentSource.getSysLangValue("lblVilla")%></a>
                            </li>
                            <li>
                                <a href="<%=CurrentSource.getPagePath("42", "stp", CurrentLang.ID.ToString()) %>"><%=CurrentSource.getSysLangValue("reqApartments")%></a>
                            </li>
                            <li><a href="<%=CurrentSource.getPagePath("9", "stp", CurrentLang.ID.ToString()) %>"><%= CurrentSource.getSysLangValue("menu_Press")%></a>
                            <li><a href="<%=CurrentSource.getPagePath("11", "stp", CurrentLang.ID.ToString()) %>"><%=CurrentSource.getSysLangValue("lblGuestbook")%></a>
                            </li>
                            <%-- <li>
                                <a target="_blank" href="http://www.rentalcastles.com/"><%=CurrentSource.getSysLangValue("menuRentCastle")%></a>
                            </li>
                            <li>
                                <a target="_blank" href="http://www.millennium-beb.com/"><%=CurrentSource.getSysLangValue("menuBeB")%></a>
                            </li>--%>
                            <li>
                                <a href="<%=CurrentSource.getPagePath("10", "stp", CurrentLang.ID.ToString()) %>"><%= CurrentSource.getSysLangValue("menu_LimousineService")%></a>
                            </li>
                            <li><a href="<%=CurrentSource.getPagePath("5", "stp", CurrentLang.ID.ToString()) %>"><%= CurrentSource.getSysLangValue("lblAboutUs")%></a>
                            </li>
                            <li><a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString()) %>"><%= CurrentSource.getSysLangValue("lblContacts")%></a>
                            </li>
                        </ul>

                    </nav>
                    <!-- END MAIN MENU -->

                </div>
            </div>
        </div>
    </div>
</header>
<!-- END HEADER -->




