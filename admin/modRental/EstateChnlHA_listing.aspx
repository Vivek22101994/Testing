<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlHA_listing.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateChnlHA_listing" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucHAEstateDetailsTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .tabsTop.tabsChannelsTop table td a[title="Listing"], #tabsHomeaway.tabsTop table td a[title="Listing"]  {
	        background:#848484;
	        border-color:#606060;
	        color:#FFF;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <img src="/images/css/homeaway-integrated-magarental.png" class="homeAwayLogo" alt="Integrated with HomeAway" />

            <h1 class="titolo_main"><%= contUtils.getLabel("lbl_ListingStruttura")%>:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop tabsChannelsTop tabsHomeAwayTop" id="tabsHomeaway">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click"><span><%= contUtils.getLabel("lblSaveChanges")%></span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span><%= contUtils.getLabel("lblCancelChanges")%></span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="/admin/rnt_estate_list.aspx"><span><%= contUtils.getLabel("lblTornaElenco")%></span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <div class="mainbox int" id="mainListing">

                    <div class="center">
                        <span class="titoloboxmodulo">Attractions</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>

                                    <td>
                                        <asp:DataList ID="dl_attractios" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>

                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>

                                            </ItemTemplate>
                                        </asp:DataList>

                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Car</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DataList ID="dl_car" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>
                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Leisure</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DataList ID="dl_leisure" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>
                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>

                                </tr>

                            </table>
                        </div>
                    </div>

                    <div class="center">
                        <span class="titoloboxmodulo">Local Features</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>

                                    <td>
                                        <asp:DataList ID="dl_local_features" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>
                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Location Type</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>

                                    <td>
                                        <asp:DataList ID="dl_location_type" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>
                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Sports and Adventure</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DataList ID="dl_sports" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>
                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>







                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>