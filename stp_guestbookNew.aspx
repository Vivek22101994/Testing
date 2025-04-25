<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_guestbookNew.aspx.cs" Inherits="RentalInRome.stp_guestbookNew" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="ucMain/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<%@ Register Src="ucMain/uc_Search.ascx" TagName="uc_Search" TagPrefix="uc1" %>
<%@ Register Src="~/ucMain/ucBreadcrumbs.ascx" TagName="breadCrumbs" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
         <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    
    <script type="text/javascript">
        function RNT_estComment_changePage(page) {
            $("#hf_estComment_currPage").val("" + page);
            RNT_estComment_fillList("list");
        }

        function RNT_estComment_fillList(action) {
            if (action == "list") SITE_showLoader();
            var _url = "/webservice/rnt_estate_comment.aspx";
            _url += "?action=" + action;
            _url += "&SESSION_ID=<%= CURRENT_SESSION_ID %>";
            _url += "&lang=<%= CurrentLang.ID %>";
            _url += "&currEstate=0";
            _url += "&voteRange=";
            _url += "&currPage=" + $("#hf_estComment_currPage").val();
            _url += "&numPerPage=20";
            _url += "&fullView=0";
            _url += "&isNew=1";
            _url += "&displayPagination=1";

            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function (html) {
                    $("#commentListCont").html(html);
                    if (action == "list") SITE_hideLoader();
                }
            });
        }
        $(document).ready(function () {
            RNT_estComment_fillList("first");
        });
    </script>
    <style>
        #wrapper {
            z-index: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_pidReservation" runat="server" />

    <!-- BEGIN CONTENT WRAPPER -->
    <div class="content">
        <div class="container bookingFormContainer stpContainer">
            <div class="row">

                <!-- BEGIN BOOKING FORM -->
                 <uc1:uc_Search runat="server" ID="uc_Search" />
                <%--<div class="sidebar col-sm-4 bookingFormDetCont">

                    <div class="gray bookingFormDet">

                        <h2 class="section-title">Search Apartment in Rome</h2>

                        <div class="form-group">

                            <div class="col-sm-12">

                                <input class="form-control calendar-input" type="text" placeholder="Check-in" name="checkin">

                                <input class="form-control calendar-input" type="text" placeholder="Check-out" name="checkout">

                                <select id="book_adults" name="book_adults" data-placeholder="Adults">
                                    <option value="---">Adults</option>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                </select>

                                <select id="book_children" name="book_children" data-placeholder="Children">
                                    <option value="---">Children</option>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                </select>

                                <select id="book_infants" name="book_infants" data-placeholder="Infants">
                                    <option value="---">Infants</option>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                </select>

                            </div>

                            <p class="center">
                                <button type="submit" class="btn btn-default-color btn-book-now">Search apartment</button>
                            </p>
                        </div>

                    </div>

                </div>--%>
                <!-- END BOOKING FORM -->

                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">
                    <uc2:breadCrumbs ID="breadCrumbs" runat="server" />
                    <h1 class="section-title"><%= ltr_title.Text%></h1>

                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp mainGuestbook">
                        <p>
                            <%= ltr_description.Text%>
                        </p>

                        <div class="nulla">
                        </div>

                        <hr />

                        <div class="nulla">
                        </div>

                        <!-- BEGIN GUESTBOOK -->
                        <input type="hidden" id="hf_estComment_currPage" value="1" />
                        <div class="comments guestbookCont">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" class="row">
                                <ContentTemplate>
                                    <div class="col-md-12 well alert-success" id="pnl_Comment_sent" runat="server" visible="false">
                                        <%= CurrentSource.getSysLangValue("reqCommentAdded")%>
                                    </div>
                                    <div class="comments-form" id="pnl_Comment_cont" runat="server">

                                        <div class="col-sm-12">
                                            <h3><%= contUtils.getLabel("lblLeaveCommentForThisApartment") %></h3>
                                            <p>Your email address will no be published. Required fields are marked*</p>
                                        </div>

                                        <div class="form-style">
                                            <div class="col-sm-6">
                                                <asp:TextBox ID="txt_user_full_name" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-sm-6">
                                                <asp:TextBox ID="txt_user_email" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-sm-6">
                                                <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                                    <WhereParameters>
                                                        <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                                    </WhereParameters>
                                                </asp:LinqDataSource>
                                                <asp:DropDownList ID="drp_country" runat="server" CssClass="form-control" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="title"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6">
                                                <asp:DropDownList ID="drp_UsertType" runat="server">
                                                    <asp:ListItem Text="I am..." Value=""></asp:ListItem>
                                                    <asp:ListItem Text="..a man" Value="m"></asp:ListItem>
                                                    <asp:ListItem Text="..a woman" Value="f"></asp:ListItem>
                                                    <asp:ListItem Text="..a couple" Value="co"></asp:ListItem>
                                                    <asp:ListItem Text="..a family" Value="fam"></asp:ListItem>
                                                    <asp:ListItem Text="..a group of persons" Value="gr"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-12">
                                                <asp:DropDownList runat="server" ID="drp_estate" CssClass="form-control" data-placeholder="Apartment">
                                                </asp:DropDownList>
                                                <%-- <select class="form-control" id="review_apt" name="review_apt" data-placeholder="Apartment">
                                            <option value="Apartment">Apartment</option>
									    </select>--%>
                                            </div>


                                            <div class="col-sm-6">
                                                <asp:DropDownList ID="drp_voteStaff" runat="server">
                                                    <asp:ListItem Text="Staff Rating" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6">
                                                <asp:DropDownList ID="drp_voteService" runat="server">
                                                    <asp:ListItem Text="Service Rating" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6">
                                                <asp:DropDownList ID="drp_voteCleaning" runat="server">
                                                    <asp:ListItem Text="Cleaning Rating" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-sm-6">
                                                <asp:DropDownList ID="drp_voteQuality" runat="server">
                                                    <asp:ListItem Text="Quality/Price Rating" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6">
                                                <asp:DropDownList ID="drp_votePosition" runat="server">
                                                    <asp:ListItem Text="Position Rating" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6">
                                                <asp:DropDownList ID="drp_voteComfort" runat="server">
                                                    <asp:ListItem Text="Comfort Rating" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Poor" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Sufficient" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Good" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="Very Good" Value="10"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>

                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txt_user_comment" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            </div>

                                            <div class="center">
                                                <asp:LinkButton ID="lnk_send_Comment" runat="server" CssClass="btn btn-default-color btn-lg" OnClick="lnk_send_Comment_Click" OnClientClick="return commentSend_validateForm();">
                                            <%= contUtils.getLabel("lblSendFeedBack") %>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            

                            <div class="nulla">
                            </div>

                            <hr />

                            <div class="nulla">
                            </div>
                            <ul id="commentListCont">
                                <div class="loading_img">
                                </div>                                
                            </ul>

                            <!-- BEGIN COMMENTS PAGINATION -->
                            <%--<div class="pagination">
                                <ul id="previous">
                                    <li><a href="#"><i class="fa fa-chevron-left"></i></a></li>
                                </ul>
                                <ul>
                                    <li class="active"><a href="#">1</a></li>
                                    <li><a href="#">2</a></li>
                                    <li><a href="#">3</a></li>
                                    <li><a href="#">4</a></li>
                                    <li><a href="#">5</a></li>
                                    <li><a href="#">All</a></li>
                                </ul>
                                <ul id="next">
                                    <li><a href="#"><i class="fa fa-chevron-right"></i></a></li>
                                </ul>
                            </div>--%>
                            <!-- END  COMMENTS PAGINATION -->

                            <div class="nulla">
                            </div>

                            <hr style="margin-bottom: 0;" />

                            <div class="nulla">
                            </div>


                        </div>

                        <!-- END GUESTBOOK -->


                        <!-- BEGIN ZONES SECTION -->
                         <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" currType="sxZone" currZone="-1" />
                        <%--<div class="main col-sm-12">
                            <h3 class="section-title" data-animation-direction="from-bottom" data-animation-delay="50" style="text-align: center;">Apartments in Rome</h3>

                            <div class="grid-style1 clearfix" id="zones-home">
                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="200">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Trevi fountain</strong></span>
                                        </a>
                                        <img src="images/zone-1.jpg" alt="Trevi fountain" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Trevi fountain</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>47 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="300">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Piazza Navona</strong></span>
                                        </a>
                                        <img src="images/zone-2.jpg" alt=" Piazza Navona" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Piazza Navona</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>80 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="400">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Spanish Steps</strong></span>
                                        </a>
                                        <img src="images/zone-3.jpg" alt="Spanish Steps" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Spanish Steps</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>75 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="500">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Saint Peter</strong></span>
                                        </a>
                                        <img src="images/zone-4.jpg" alt="Saint Peter" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Saint Peter</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>104 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="600">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Colosseum</strong></span>
                                        </a>
                                        <img src="images/zone-5.jpg" alt="Colosseum" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Colosseum</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>64 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="700">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Trastevere</strong></span>
                                        </a>
                                        <img src="images/zone-6.jpg" alt="Trastevere" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Trastevere</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>91 Apartments</li>
                                    </ul>
                                </div>

                            </div>

                        </div>--%>
                        <!-- END ZONES SECTION -->

                    </div>


                </div>
                <!-- End BEGIN MAIN CONTENT -->


            </div>
        </div>
    </div>
    <!-- End BEGIN CONTENT WRAPPER -->
    <script type="text/javascript">
        // "FIXED BOOKING FORM"
        var topHeight = $("#header").height() - $("#top-info").height();

        $(window).scroll(function () {
            if ($(this).scrollTop() > (topHeight)) {
                $('.bookingFormDet').addClass("fixedBox");
                $(".bookingFormDet").width($(".bookingFormDetCont").width() - 30);
                if ($(this).width() > 768) {
                    $('.bookingFormDet').css({ "top": topHeight });
                }
            } else {
                $('.bookingFormDet').removeClass("fixedBox");
                $(".bookingFormDet").removeAttr('style');
            }
        });
        $(window).resize(function () {
            topHeight = $("#header").height();
            if ($(this).scrollTop() > (topHeight)) {
                $('.bookingFormDet').addClass("fixedBox");
                $(".bookingFormDet").width($(".bookingFormDetCont").width() - 30);
                if ($(this).width() > 768) {
                    $('.bookingFormDet').css({ "top": topHeight });
                }
            } else {
                $('.bookingFormDet').removeClass("fixedBox");
                $(".bookingFormDet").removeAttr('style');
            }
        });

    </script>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var commentSend_validateForm_firstTime = true;
            function commentSend_validateForm() {
                var callBack = null;
                if (commentSend_validateForm_firstTime) {
                    callBack = function () { return commentSend_validateForm(); };
                    commentSend_validateForm_firstTime = false;
                }
                var isValid = true;
                FORM_hideErrorToolTip();
                if (!FORM_validate_requiredField("<%= txt_user_comment.ClientID%>", "", "", "", callBack))
                    isValid = false;
                else if ($("#<%= txt_user_comment.ClientID%>").val().toLowerCase().indexOf("<a href") >= 0) {
                    FORM_showErrorToolTip("Links are not allowed here.", "<%=txt_user_comment.ClientID %>");
                    isValid = false;
                }
            if (!FORM_validate_requiredField("<%= txt_user_email.ClientID%>", "", "", "", callBack))
                    isValid = false;
                else if (!FORM_validate_emailField("<%= txt_user_email.ClientID%>", "", "", "", callBack))
                isValid = false;
            if (!FORM_validate_requiredField("<%= txt_user_full_name.ClientID%>", "", "", "", callBack))
                    isValid = false;

                return isValid;
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
