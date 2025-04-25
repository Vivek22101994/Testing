<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_new.aspx.cs" Inherits="RentalInRome.stp_new" %>

<%@ Register Src="/ucMain/uc_Search.ascx" TagName="UC_Search" TagPrefix="uc1" %>
<%@ Register Src="/ucMain/UC_apt_in_rome_bottom.ascx" TagName="ucZone" TagPrefix="uc1" %>
<%@ Register Src="~/ucMain/ucBreadcrumbs.ascx" TagName="breadCrumbs" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title><%= currStp.meta_title %></title>
    <meta name="description" content="<%=currStp.meta_description %>" />
    <meta name="keywords" content="<%=currStp.meta_keywords %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>

    <div class="nulla">
    </div>

    <!-- BEGIN CONTENT WRAPPER -->
    <div class="content">
        <div class="container bookingFormContainer stpContainer">
            <div class="row">

                <!-- BEGIN BOOKING FORM -->
                <uc1:UC_Search ID="ucSearch" runat="server" />
                <!-- END BOOKING FORM -->

                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">
                    <uc2:breadCrumbs ID="breadCrumbs" runat="server" />
                    <%= ltr_title.Text != "" ? "<h1 class='section-title'>" + ltr_title.Text + "</h1>" : ""%>


                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp">
                        <div class="center imgStp">
                            <%= ltr_img_banner.Text != "" ? "<img src=\"/" + ltr_img_banner.Text + "\" alt=\"\" />" : ""%>
                        </div>

                        <div class="nulla">
                        </div>

                        <p>
                            <%=ltr_description.Text %>
                        </p>

                        <div class="nulla">
                        </div>

                        <hr />

                        <div class="nulla">
                        </div>

                        <!-- BEGIN ZONES SECTION -->
                        <uc1:ucZone ID="uc_Zone" runat="server" />
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

    <div class="nulla">
    </div>




</asp:Content>
