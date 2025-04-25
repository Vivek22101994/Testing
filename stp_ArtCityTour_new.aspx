<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_ArtCityTour_new.aspx.cs" Inherits="RentalInRome.stp_ArtCityTour_new" %>
<%@ Register Src="~/ucMain/ucBreadcrumbs.ascx" TagName="breadCrumbs" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
       <title><%= currStp.meta_title %></title>
    <meta name="description" content="<%=currStp.meta_description %>" />
    <meta name="keywords" content="<%=currStp.meta_keywords %>" />
    <style>
        table.priceTable {
    float: right;
    width: 100%;
    background-color: #E9E9F3;
    border: 1px solid #D5D5E1;
    margin-bottom:20px;
    box-sizing:border-box;
}
table.priceTable tr td {
    padding:10px;
    border-bottom:1px dotted #FFF; 
    font-size:11px;
}
table.priceTable tr th {
    padding:10px;
    color:#333366;
    background-color:#D5D5E1;
    font-size:12px;
}

@media (max-width:500px){
        table.priceTable tr td, table.priceTable tr th {
            padding: 10px 0 !important;
            font-size: 9px !important;
        }
}

        img[src="/images/Stp/art-cities-tour-rentalinrome.png"], img[src="/images/Stp/art-cities-tour-easy-rentalinrome.png"] {
            right: inherit !important;
            left: -342px;
        }

        .stpContainer .main hr {
            margin: 35px 0;
        }
        .sidebar.col-sm-4.artCityColSx {
            text-align: center;
            float: left;
        }

        .stpContainer .main.col-sm-8 {
            float: right;
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

    <div class="nulla">
    </div>

    <!-- BEGIN CONTENT WRAPPER -->
    <div class="content">
        <div class="container bookingFormContainer stpContainer">
            <div class="row">

               
                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">
                    <uc1:breadCrumbs ID="breadCrumbs" runat="server" />
                    <%= ltr_title.Text != "" ? "<h1 class='section-title'>" + ltr_title.Text + "</h1>" : ""%>


                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp">
                        
                        <p>
                            <%=ltr_description.Text %>
                        </p>

                        <div class="nulla">
                        </div>

                    </div>


                </div>
                <!-- End BEGIN MAIN CONTENT -->

                 <!-- BEGIN COL SX -->
                
                <div class="sidebar col-sm-4 artCityColSx">
                    <%= ltr_img_banner.Text != "" ? "<img src=\"/" + ltr_img_banner.Text + "\" alt=\"\" />" : ""%>
                </div>
                <!-- END COL SX -->



            </div>
        </div>
    </div>
    <!-- End BEGIN CONTENT WRAPPER -->

    <div class="nulla">
    </div>




</asp:Content>
