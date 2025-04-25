<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucZone.ascx.cs" Inherits="RentalInRome.ucMain.ucZone" %>
<asp:HiddenField runat="server" ID="HF_currZone" Value="0" Visible="false" />
<asp:HiddenField runat="server" ID="HF_currType" Value="bottom" Visible="false" />

<asp:HiddenField runat="server" ID="HF_animationDelay" Value="100" Visible="false" />

<asp:ListView ID="LVZoneHome" runat="server">
    <ItemTemplate>
        <div data-animation-direction="from-bottom" data-animation-delay="<%# GetAnimationDelay() %>"
            class="<%# animationDelay > 700 ?"item col-md-6 disabled" : "item col-md-6" %>">
            <div class="image">
                <a href="<%#  Eval("Path") %>">
                    <span class="location"><%# contUtils.getLabel("lblSeeAllApartmentsIn") %>
                        <br />
                        <strong><%# Eval("Title") %></strong></span>
                </a>
                <img src="/images/css/zone_thumb.jpg" alt="<%# Eval("Title") %>" alt="<%# Eval("Title") %>" id='_zoneList_img_<%# Eval("Id") %>' />
            </div>
            <div class="price zoneNameHome">
                <span><%# Eval("Title") %></span>
            </div>
            <ul class="amenities numAptsZoneHome">
                <li><i class="icon-house"></i><%# Eval("Count") + " " + contUtils.getLabel("reqApartments") %> </li>
            </ul>
        </div>
        <script type="text/javascript">
            GLOBAL_imgLoader.push(new Array("_zoneList_img_<%# Eval("Id") %>", "/<%# Eval("Img") %>"));
        </script>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div id="zoneHome">
            <a id="itemPlaceholder" runat="server" />
        </div>
    </LayoutTemplate>
</asp:ListView>
