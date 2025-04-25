<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_pdf.Master" AutoEventWireup="true" CodeBehind="pg_rntEstatePdf.aspx.cs" Inherits="FourSprings.pg_rntEstatePdf" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="uc/ucEstatePrices.ascx" TagName="ucEstatePrices" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%= currEstateLN.meta_title%></title>
    <meta name="description" content="<%=currEstateLN.meta_description %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <style type="text/css">
        .bedType_ttp {
            position: absolute;
            margin-left: 20px;
            margin-top: 15px;
            display: none;
        }

        a.info:hover .bedType_ttp {
            display: inline-block;
        }

        .priceTablecustom {
             page-break-before: always;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
     <asp:HiddenField Value="gallery" ID="Hf_banner_img" runat="server" Visible="false" />
    <asp:HiddenField Value="gallery" ID="HF_galleryType" runat="server" />
    <asp:HiddenField Value="" ID="HF_unique" runat="server" />
    <asp:HiddenField Value="" ID="HF_id" runat="server" />
    <asp:HiddenField Value="" ID="Hf_agentid" runat="server" />
    <asp:HiddenField Value="0" ID="Hf_markup_per" runat="server" />


    <div class="main" id="mainDett">
        <%= !string.IsNullOrEmpty(currAgent.imgLogo)  ?
        "<a class=\"logo\" href=\""+ currAgent.contactWebSite +"\"><img alt=\"\" src=\""+ currAgent.imgLogo +"\" /></a>"
        :""
        %>
        <h3 class="sitePdfTop"><%= currAgent.contactWebSite %> </h3>
        <%--  <h3 class="sitePdfTop">www.rentalinrome.com</h3>--%>
        <div class="mainContent">
            <div class="titPdfCont">
                <h1 class="titTxtBig" id="accomodationNameDett"><%= currEstateLN.title %></h1>
                <h2 class="titAreaDet"><%= CurrentSource.locZone_title(currEstateTB.pid_zone.objToInt32(), App.LangID, "") %></h2>
            </div>
            <div class="nulla">
            </div>

            <div id="content">
                <div class="panes" id="listPanes">
                    <div class="tabList" id="tabListDescription">
                        <% if(!string.IsNullOrEmpty(Hf_banner_img.Value)) {  %>
                            <img src="/<%= Hf_banner_img.Value %>" alt="/<%= currEstateTB.code %>" style="width: 100%; page-break-inside: avoid;" />
                        <% } %>
                        <div style="height: 10px; width: 100%; display: block; float: left;" class="">
                        </div>
                        <div class="nulla">
                        </div>
                    </div>


                    <h3 class="subTitTxt"><%= contUtils.getLabel("lblAccomodationDescription") %></h3>
                    <div>
                        <div class="txt txtDett txtPdf" style="page-break-inside: avoid;">
                            <p><%= currEstateLN.description %></p>
                        </div>
                        <asp:Literal ID="ltrAmenities" runat="server" Visible="false"></asp:Literal>
                        <%= ltrAmenities.Text!=""?"<h3 class='subTitTxt'>"+contUtils.getLabel("lblAmenities")+"</h3><div id='amenitiesDett'><div id='serviceIcons'><ul>"+ltrAmenities.Text+"</ul></div><div class='nulla'></div></div>":""%>
                    </div>

                    <div class="pdfMap" id="map2" style="page-break-inside: avoid;">
                        <h3 class="titMapPdf">Location</h3>
                        <div class="tabList" id="tabListPosition" style="page-break-inside: avoid;">
                            <%if (!string.IsNullOrEmpty(currEstateTB.google_maps))
                              { %>
                            <img src="http://maps.googleapis.com/maps/api/staticmap?center=<%= currEstateTB.google_maps.Replace(",",".").Replace("|",",")%>&zoom=14&size=451x250&sensor=false&markers=icon:<%=App.HOST %>/images/google_maps/google_icon_logo.png%7C<%= currEstateTB.google_maps.Replace(",",".").Replace("|",",")%>" alt="" />
                            <%} %>
                        </div>
                    </div>

                    <div class="tabList" id="tabListAvailability" style="page-break-inside: avoid;display:none;">
                        <asp:PlaceHolder ID="pnlAvailability" runat="server">
                            <h3 class="subTitTxt">Availability</h3>
                            <div>
                                <div id="avvCalendarCont" style="width: 100%;">
                                </div>
                                <asp:Literal ID="ltrScript_checkCalDates" runat="server"></asp:Literal>
                                <script type="text/javascript">
                                    $("#avvCalendarCont").datepicker({ width: '100%', numberOfMonths: [3, 4], showOtherMonths: false, showButtonPanel: false, beforeShowDay: checkCalDates_<%= Unique %>, minDate: JSCal.intToDate(<%= (new DateTime(DateTime.Now.Year, 1, 1)).JSCal_dateToString()%>), maxDate: JSCal.intToDate(<%= (new DateTime(DateTime.Now.Year+2, 1, 1)).JSCal_dateToString()%>) });
                                </script>
                            </div>
                        </asp:PlaceHolder>
                    </div>

                    <h3 class="subTitTxt"><%= contUtils.getLabel("lblPrices") %></h3>
                    <uc1:ucEstatePrices runat="server" ID="ucEstatePrices" />
                </div>
            </div>
        </div>

        <asp:PlaceHolder ID="pnlImages" runat="server">
            <h3 class="subTitTxt">Photos</h3>
            <div class="pdfPhotosCont">
                <asp:ListView ID="LvGalleryBig" runat="server">
                    <ItemTemplate>
                        <img src="/<%# Eval("img_banner") %>" alt="<%# Eval("code") %>" style="page-break-inside: avoid;" />
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <img src="/<%# Eval("img_banner") %>" alt="<%# Eval("code") %>" style="page-break-inside: avoid;" />
                    </AlternatingItemTemplate>
                </asp:ListView>
            </div>

            <div class="nulla">
            </div>
            <asp:ListView ID="LvGallerySmall" runat="server" GroupItemCount="6">
                <ItemTemplate>
                    <img src="/<%# Eval("img_banner") %>" alt="<%# Eval("code") %>" style="width: 49%; float: left; page-break-inside: avoid;" />
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <img src="/<%# Eval("img_banner") %>" alt="<%# Eval("code") %>" style="width: 49%; float: right; page-break-inside: avoid;" />
                    <div style="height: 10px; width: 100%; display: block; float: left;" class="">
                    </div>
                    <div class="nulla">
                    </div>
                </AlternatingItemTemplate>
                <GroupTemplate>
                    <div id="itemPlaceholder" runat="server"></div>
                    <div class="nulla">
                    </div>
                    <div style="page-break-after: always;">
                    </div>
                </GroupTemplate>
            </asp:ListView>
        </asp:PlaceHolder>

        <div class="footerPdf">
            <%= !string.IsNullOrEmpty(currAgent.imgLogo)  ?
                "<img class=\"logoFooterPdf\" alt=\"\" src=\""+ currAgent.imgLogo +"\" />"
                :""
            %>           
            <div class="datiSocPdf">
                <strong><%= currAgent.nameCompany %></strong> <%= currAgent.locAddress %>
                <br />
                Tel: <%= currAgent.contactPhone != null ? currAgent.contactPhone : "-"  %> - Fax: <%= currAgent.contactFax != null ?  currAgent.contactFax : "-"  %> - Email: <%= currAgent.contactEmail != null ? currAgent.contactEmail : "-"  %>
            </div>
        </div>

        <div class="nulla">
        </div>

    </div>
</asp:Content>
