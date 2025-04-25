<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_apt_in_rome_bottom.ascx.cs" Inherits="RentalInRome.ucMain.UC_apt_in_rome_bottom" %>
<asp:HiddenField runat="server" ID="HF_currZone" Value="0" Visible="false" />
<asp:HiddenField runat="server" ID="HF_currType" Value="bottom" Visible="false" />
<!-- BEGIN ZONES SECTION -->
<asp:ListView ID="LV_sxZone" runat="server" OnDataBound="LV_sxZone_DataBound">
    <ItemTemplate>
        <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="200">
            <div class="image">
                <a href="<%#  Eval("Path") %>">
                    <span class="location">See all apartments in
                                            <br />
                        <strong><%# Eval("Title") %></strong></span>
                </a>
                <img src="/<%# Eval("Img") %>" alt="<%# Eval("Title") %>" />
            </div>
            <div class="price zoneNameHome">
                <span><%# Eval("Title") %></span>
            </div>
            <ul class="amenities numAptsZoneHome">
                <li><i class="icon-house"></i><%# Eval("Count") %> </li>
            </ul>
        </div>
        <%-- <a class="zonalista <%# "" + Eval("Id")=="9"?"zona_new":""%>" href="<%# Eval("Path") %>">
                                <%# "" + Eval("Id")=="9" && 1==2 ?"<span class=\"ico_new\"></span>":""%>
                                <span class="contimg">
                                    <img src="/<%# Eval("Img") %>" alt="<%# Eval("Title") %>" />
                                </span>
                                <span class="nomeZona">
                                    <%# Eval("Title") %></span>
                                <span class="numApts">
                                    <strong>
                                        <%# Eval("Count") %>
                                    </strong>
                                </span>
                            </a>--%>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div class="main col-sm-12">
            <h1 class="section-title" data-animation-direction="from-bottom" data-animation-delay="50" style="text-align: center;">
                <asp:Literal ID="ltr_title" runat="server"></asp:Literal></h1>
            <div class="grid-style1 clearfix" id="zones-home">
                <div id="itemPlaceholder" runat="server" />
            </div>
        </div>
    </LayoutTemplate>
</asp:ListView>



<%--<div class="main col-sm-12">
                        <h1 class="section-title" data-animation-direction="from-bottom" data-animation-delay="50" style="text-align: center;">Discover other zones</h1>

                        <div class="grid-style1 clearfix" id="zones-home">
                            <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="200">
                                <div class="image">
                                    <a href="#">
                                        <span class="location">See all apartments in
                                            <br />
                                            <strong>Trevi fountain</strong></span>
                                    </a>
                                    <img src="/images/zone-1.jpg" alt="Trevi fountain" />
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
                                    <img src="/images/zone-2.jpg" alt=" Piazza Navona" />
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
                                    <img src="/images/zone-3.jpg" alt="Spanish Steps" />
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
                                    <img src="/images/zone-4.jpg" alt="Saint Peter" />
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
                                    <img src="/images/zone-5.jpg" alt="Colosseum" />
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
                                    <img src="/images/zone-6.jpg" alt="Trastevere" />
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
