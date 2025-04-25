<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="pg_tour_details.aspx.cs" Inherits="RentalInRome.pg_tour_details" %>

<%@ Register Src="uc/UC_menu_breadcrumb.ascx" TagName="UC_menu_breadcrumb" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
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
    <asp:Literal runat="server" ID="ltr_price_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_lang" runat="server" Value="2" />
    <asp:HiddenField ID="HF_id" runat="server" Value="" />
    <uc1:UC_menu_breadcrumb ID="UC_menu_breadcrumb1" runat="server" />
    <h1 class="underlined" style="margin-top:10px; font-size:25px; line-height:25px;">
        <%= ltr_title.Text%>
        </h1>
        <div class="nulla">
        </div>
        <div id="tourDett">
            <%= ltr_price_description.Text%>
            <table class="priceTable" cellspacing="0" cellpadding="0" style="width:420px; margin-left:20px;" runat="server" visible="false">
                <tr>
                    <th valign="middle" colspan="3" align="left">
                        RATES
                    </th>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        For groups up to 3 people
                    </td>
                    <td valign="middle" align="center">
                        6 Hours tour € 215
                    </td>
                    <td valign="middle" align="center">
                        Extra hour € 40
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        For groups from 4 to 6 people
                    </td>
                    <td valign="middle" align="center">
                        6 Hours tour € 255
                    </td>
                    <td valign="middle" align="center">
                        Extra hour € 45
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        For groups from 7 to 8 people
                    </td>
                    <td valign="middle" align="center">
                        6 Hours tour € 285
                    </td>
                    <td valign="middle" align="center">
                        Extra hour € 50
                    </td>
                </tr>
                
            </table>
        
        <div class="txtGen">
            <%= ltr_description.Text%>
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
        
        </div>
    <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />
</asp:Content>
