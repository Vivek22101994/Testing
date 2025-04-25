<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlRU_location.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateChnlRU_location" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucRUEstateDetailsTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tabsTop.tabsChannelsTop table td a[title="Location"], #tabsHomeaway.tabsTop table td a[title="Location"] {
            background: #848484;
            border-color: #606060;
            color: #FFF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <img src="/images/css/ru-integrated-magarental.png" class="homeAwayLogo RUlogo" alt="Integrated with RentalsUnited" />

            <h1 class="titolo_main"><%= contUtils.getLabel("lbl_AccomodationPositionDetails")%>:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop tabsChannelsTop tabsHomeAwayTop" id="tabsHomeaway">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>

            <%-- <div class="copiaIncolla">
                <asp:LinkButton ID="lnk_copyLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_copyLang_Click" CssClass="btnCopia">copia</asp:LinkButton>
                <asp:LinkButton Visible="false" ID="lnk_pasteLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_pasteLang_Click" CssClass="btnCopia">incolla</asp:LinkButton><asp:HiddenField ID="HF_copyLang" Value="0" runat="server" Visible="false" />
            </div>--%>

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
            <div class="mainline mainChannels mainHomeAway mainLocation">
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="tableBoxBooking tableLocation" cellpadding="0" cellspacing="0" style="width: 60%; min-width: 700px;">

                                        <tr>
                                            <td valign="middle" align="left" width="40%">AddressLine1: </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_address1" Width="94%" TextMode="MultiLine" Height="30px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">AddressLine2:</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_address2" Width="94%" TextMode="MultiLine" Height="30px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">Postal Code:
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_postal_code" Style="width: 50px;" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">City:

                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_city" Style="width: 50px;" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>

                                            <td valign="middle" align="left">Allow Travelers To Zoom:
                                            </td>
                                            <td>

                                                <asp:DropDownList ID="drp_allow_zoom" runat="server">
                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                </asp:DropDownList>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">Default Zoom Level:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_default_zoom_level" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">Max Zoom Level: </td>
                                            <td>
                                                <asp:DropDownList ID="drp_max_zoom_level" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">Show exact location: </td>
                                            <td>
                                                <asp:DropDownList ID="drp_show_exact_location" runat="server">
                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">GeoCode Longitude: </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_longitude" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">GeoCode Latitude: </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_latitude" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
                                    <asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
                                        <WhereParameters>
                                            <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_lang" CommandName="change_lang" CssClass="tab_item" runat="server">
                                                <span>
                                                    <%# Eval("title") %></span>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("abbr") %>' />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <span>No data was returned.</span>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <div class="menu2">
                                                <a id="itemPlaceholder" runat="server" />
                                            </div>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="0" cellspacing="0">

                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo"><%= contUtils.getLabel("lbl_Visualizzazione")%></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><strong>Description:</strong><br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="re_description" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            <br />
                                                            <strong>Other Activities:</strong><br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="re_other_activties" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>


                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:ListView ID="LV_nearestPlaces" runat="server" OnItemDataBound="LV_nearestPlaces_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                    <%# Eval("haPlaceType") %>
                                                </td>
                                                <td>

                                                    <%# Eval("title") %>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_distance" runat="server"></asp:DropDownList>

                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_unit" runat="server">
                                                        <asp:ListItem Text="Km" Value="Km"></asp:ListItem>
                                                        <asp:ListItem Text="Miles" Value="Miles"></asp:ListItem>
                                                        <asp:ListItem Text="Metres" Value="Metres"></asp:ListItem>
                                                        <asp:ListItem Text="Minutes" Value="Minutes"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <LayoutTemplate>
                                            <table class="tableBoxBooking tablePlaceType" id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                                                <tr>
                                                    <th align="left">Place Type</th>
                                                    <th align="left">Place Name
                                                    </th>
                                                    <th align="left">Distance
                                                    </th>
                                                    <th align="left"></th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
