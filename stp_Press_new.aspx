<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_Press_new.aspx.cs" Inherits="RentalInRome.stp_Press_new" %>

<%@ Register Src="/ucMain/ucBreadcrumbs.ascx" TagName="breadCrumbs" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
         <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
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
                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-12">
                    <uc1:breadCrumbs ID="breadcrumbs" runat="server" />
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
