<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="pg_tour_details_new.aspx.cs" Inherits="RentalInRome.pg_tour_details_new" %>

<%@ Register Src="~/ucMain/UC_apt_in_rome_bottom.ascx" TagName="ucRomeApts" TagPrefix="uc1" %>
<%@ Register Src="~/ucMain/ucBreadcrumbs.ascx" TagName="ucBreadcrumbs" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">

    <style type="text/css">
        .bannerTour {
            clear: both;
            display: block;
            float: left;
            margin: 30px 0;
            width: 100%;
        }

            .bannerTour > img {
                clear: both;
                float: left;
                margin: 25px 0 0;
                width: 100%;
            }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">

    <asp:HiddenField ID="HF_lang" runat="server" />
    <asp:HiddenField ID="HF_id" runat="server" />
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_price_description" Visible="false"></asp:Literal>
    <div class="content">
        <div class="container bookingFormContainer stpContainer">
            <div class="row">

                <!-- BEGIN BOOKING FORM -->
                <div class="sidebar gray col-sm-4 servicesColSx pickupPricesDx">

                    <div class="servicePrices">
                        <h2 class="section-title"><%=contUtils.getLabel("mobilePriceRates") %></h2>


                        <%= ltr_price_description.Text.Replace("priceTable","table-bordered tablePrices") %>

                        <%--      <table cellpadding="0" cellspacing="0" class="table-bordered tablePrices">
                            <tbody>
                                <tr>
                                    <th colspan="2" valign="middle" align="center"><%=contUtils.getLabel("lblGroups") +" "+contUtils.getLabel("lblUpTo") %>
                                        <br />
                                        <em>3 <%=contUtils.getLabel("lblPax") %></em></th>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <strong>5 <%=contUtils.getLabel("lblHours") %> tour</strong><br />
                                        € 180
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong><%=contUtils.getLabel("lblExtra") +" "+contUtils.getLabel("lblHours") %> </strong>
                                        <br />
                                        € 40
                                    </td>

                                </tr>
                                <tr>
                                    <th colspan="2" valign="middle" align="center"><%=contUtils.getLabel("lblGroups") +" "+contUtils.getLabel("lblFrom") %>
                                        <br />
                                        <em>4 to 6 <%=contUtils.getLabel("lblPax") %></em></th>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <strong>5 <%=contUtils.getLabel("lblHours") %> tour</strong><br />
                                        € 210
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong><%=contUtils.getLabel("lblExtra") +" "+contUtils.getLabel("lblHours") %> </strong>
                                        <br />
                                        € 45
                                    </td>

                                </tr>
                                <tr>
                                    <th colspan="2" valign="middle" align="center"><%=contUtils.getLabel("lblGroups") +" "+contUtils.getLabel("lblFrom") %><br />
                                        <em>7 to 8 <%=contUtils.getLabel("lblPax") %></em></th>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <strong>5 <%=contUtils.getLabel("lblHours") %> tour</strong><br />
                                        € 240
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong><%=contUtils.getLabel("lblExtra") +" "+contUtils.getLabel("lblHours") %></strong><br />
                                        € 50
                                    </td>
                                </tr>
                            </tbody>
                        </table>--%>


                        <p class="center">
                            <a class="btn btn-fullcolor" href="<%= CurrentSource.getPagePath("4","stp",App.LangID+"") %>">
                                <%= contUtils.getLabel("lblAnyQuestion") %> 
                            </a>
                        </p>

                    </div>
                </div>
                <!-- END BOOKING FORM -->


                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">
                    <uc1:ucBreadcrumbs ID="ucBreadcrumbs" runat="server" />
                    <h1 class="section-title" style="margin-bottom: 25px;"><%= ltr_title.Text %></h1>

                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp">

                        <div class="nulla">
                        </div>

                        <p>
                            <%= ltr_description.Text %>
                        </p>

                        <div class="nulla">
                        </div>

                        <hr />

                        <div class="nulla">
                        </div>

                        <asp:ListView ID="LV_tour_item" runat="server" DataSourceID="LDS_tour_item">
                            <ItemTemplate>
                                <div class="tour">
                                    <div class="bannerTour">
                                        <h3>
                                            <%# Eval("title") %>
                                        </h3>
                                        <img src="/<%# Eval("img_banner") %>" alt="<%= ltr_title.Text %> - <%# Eval("title") %>" />
                                    </div>
                                    <div id="Div1" class="galleryTour" runat="server" visible="false">
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <a href="#">
                                            <img src="images/tour/tivoli/villa-adriana-t1.jpg" alt="" />
                                        </a>
                                        <div class="nulla">
                                        </div>
                                    </div>
                                    <div class="txtTour">
                                        <%# Eval("description") %>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <div id="itemPlaceholder" runat="server" />
                            </LayoutTemplate>
                        </asp:ListView>
                        <asp:LinqDataSource ID="LDS_tour_item" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_VIEW_TOUR_ITEMs" Where="is_acitve == 1 && pid_lang == @pid_lang && pid_tour == @pid_tour">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="HF_lang" Name="pid_lang" PropertyName="Value" Type="Int32" />
                                <asp:ControlParameter ControlID="HF_id" Name="pid_tour" PropertyName="Value" Type="Int32" />
                            </WhereParameters>
                        </asp:LinqDataSource>

                        <!-- BEGIN ZONES SECTION -->
                        <div class="main col-sm-12">
                            <uc1:ucRomeApts ID="ucRomeApts" runat="server" />
                        </div>
                        <!-- END ZONES SECTION -->

                    </div>


                </div>
                <!-- End BEGIN MAIN CONTENT -->

            </div>



        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tablePrices").removeAttr("style");
        });
    </script>
</asp:Content>
