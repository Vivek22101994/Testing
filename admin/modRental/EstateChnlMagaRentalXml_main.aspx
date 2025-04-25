<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlMagaRentalXml_main.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateChnlMagaRentalXml_main" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <style type="text/css">
                .main div.mainbox {
                    width: 100%;
                }

                .tabsTop.tabsChannelsTop table td a[title="Home"], #tabsHomeaway.tabsTop table td a[title="Home"] {
                    background: #848484;
                    border-color: #606060;
                    color: #FFF;
                }
            </style>
            <style type="text/css">
                #tabsHomeaway.tabsTop table td a, #tabsHomeaway.tabsTop table td a:visited {
                    display: block;
                    float: left;
                    margin: 10px 0 0 10px;
                    padding: 7px;
                    width: auto;
                }

                div.salvataggio {
                    float: left;
                }

                    div.salvataggio.btnCopia {
                        float: right;
                        width: auto;
                    }

                .homeAwayLogo.RUlogo {
                    background: #fff none repeat scroll 0 0;
                    padding: 10px;
                }
            </style>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <%# rntUtils.getAgent_logoForDetailsPage(ChnlMagaRentalXmlProps.IdAdMedia) %>
            <h1 class="titolo_main"><%= contUtils.getLabel("lbl_GeneralDataOf")%> MagaRentalXml:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop tabsChannelsTop tabsMagaRentalXmlLettingsTop" id="tabsHomeaway">
                    <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table;">
                        <tr>
                            <td style="border-bottom: 0; width: 65%;">
                                <a href='/admin/modRental/EstateChnlMagaRentalXml_main.aspx?id=<%= IdEstate %>' class="scheda" title="Unit"><%= contUtils.getLabel("Details")%></a>
                                <a href='/admin/modRental/EstateChnlMagaRentalXml_media.aspx?id=<%= IdEstate %>' class="media" title="Images"><%= contUtils.getLabel("Images")%></a>
                                <a href='/admin/modRental/Estate_Details_Property_Info.aspx?id=<%= IdEstate %>' class="scheda" title="Torna alla Struttura">Back to property</a>
                            </td>

                            <td style="border-bottom: 0; width: 35%;" align="right">
                                <a style="float: right;" href='/admin/modRental/EstateChnlMagaRentalXml_main.aspx?id=<%= IdEstate %>&updatefrommagarental=true' class="scheda" title="Copia da MagaRental">Copy from MagaRental</a>

                            </td>

                        </tr>
                    </table>
                    <div class="nulla"></div>
                </div>
            </div>
            <div class="nulla">
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
            <div class="mainline" id="pnlError" runat="server" visible="false">
                <div class="mainbox">
                    <div class="top">
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <asp:Literal ID="ltrErorr" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="bottom">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline mainChannels mainHolidayLettings mainHomeHL">
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <span class="titoloboxmodulo">URLs for MagaRentalXml</span>
                        <div class="boxmodulo">
                            <table class="tableBoxBooking tableHomeHL" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="middle">Search:
                                    </td>
                                    <td valign="middle" align="left">
                                        <%# App.HOST_SSL+"/chnlutils/magarental/search?auth=ApiKey&ln=en" %>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="middle">Listings:
                                    </td>
                                    <td valign="middle" align="left">
                                        <%# App.HOST_SSL+"/chnlutils/magarental/listings?auth=ApiKey&ln=en" %>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="middle">Bookings:
                                    </td>
                                    <td valign="middle" align="left">
                                        <%# App.HOST_SSL+"/chnlutils/magarental/bookings?auth=ApiKey&ln=en" %>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="middle">Booking request:
                                    </td>
                                    <td valign="middle" align="left">
                                        <%# App.HOST_SSL+"/chnlutils/magarental/bookingrequest?auth=ApiKey&ln=en" %>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="middle">Quote request:
                                    </td>
                                    <td valign="middle" align="left">
                                        <%# App.HOST_SSL+"/chnlutils/magarental/quoterequest?auth=ApiKey&ln=en" %>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline mainChannels mainMagaRentalXml mainAdContent">
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
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
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
                                    <table cellpadding="0" cellspacing="10">
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">Title:<span class="error_text" id='count_txt_title' style="display: none"><span class="char"></span>&nbsp;caratteri e&nbsp; <span class="word"></span>&nbsp;parole </span>
                                                            <br />
                                                            <asp:TextBox runat="server" ID="txt_title" MaxLength="100" Style="width: 99%; margin-left: 0; float: left;" onkeyup="CountWords(this,'count_txt_title')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Summary:<span class="error_text" id='count_txt_summary' style="display: none"><span class="char"></span>&nbsp;caratteri e&nbsp; <span class="word"></span>&nbsp;parole </span>
                                                            <br />
                                                            <asp:TextBox runat="server" ID="txt_summary" Width="400px" TextMode="MultiLine" Height="115px" onkeyup="CountWords(this,'count_txt_summary')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Description:<br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="re_description" SkinID="DefaultSetOfTools" Height="400" Width="800" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>

                                                </table>
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

