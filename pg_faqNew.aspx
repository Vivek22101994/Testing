<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="pg_faqNew.aspx.cs" Inherits="RentalInRome.pg_faqNew" %>


<%@ Register Src="~/ucMain/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc1" %>

<%@ Register Src="~/ucMain/UC_Rental_Recommended.ascx" TagName="UC_Rental_Recommended" TagPrefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
   <%-- <title>
        <%=currStp.meta_title%></title>
    <meta name="description" content="<%=currStp.meta_description %>" />
    <meta name="keywords" content="<%=currStp.meta_keywords %>" />--%>
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


    <div class="content">
        <div class="container stpContainer">
            <div class="row">
                <div class="main col-sm-8">
                    <h1 class="section-title"><%= currStp.title %>   </h1>

                    <div class="center imgStp">
                        <%= ltr_img_banner.Text != "" ? "<img src=\"/" + currStp.img_banner + "\" alt=\"\" />" : ""%>
                    </div>

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

                </div>
                <div class="col-md-4">
                    <h3 class="section-title">
                        <%= CurrentSource.getSysLangValue("lblOurWebsiteIsRecommendedBy")%>
                    </h3>
                    <uc3:UC_Rental_Recommended ID="UC_Rental_Recommended" runat="server" />
                </div>
            </div>

            <div class="clearfix"></div>
            <div class="row">
                <uc1:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />
            </div>
        </div>
    </div>




</asp:Content>

