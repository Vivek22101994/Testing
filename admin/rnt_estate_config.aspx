<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master"
    AutoEventWireup="true" CodeBehind="rnt_estate_config.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_config" %>

<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titolo_main">
        Accessori della struttura:
        <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal></h1>
    <div id="fascia1">
        <div style="clear: both; margin: 3px 0 5px 30px;">
            <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
        </div>
    </div>
    <div class="nulla">

    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="" runat="server" />
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <!-- INIZIO MAIN LINE -->
            <div class="mainline">
                <!-- BOX 1 -->
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <table border="0" cellspacing="3" cellpadding="0">
                                <tr>
                                    <td>
                                        <asp:LinqDataSource ID="LDS" runat="server" OrderBy="inner_notes" ContextTypeName="RentalInRome.data.magaRental_DataContext"
                                            TableName="RNT_TB_CONFIGs" Where="inner_type == @inner_type &amp;&amp; inner_category == @inner_category">
                                            <WhereParameters>
                                                <asp:Parameter DefaultValue="1" Name="inner_type" Type="Int32" />
                                                <asp:Parameter DefaultValue="1" Name="inner_category" Type="Int32" />
                                            </WhereParameters>
                                        </asp:LinqDataSource>
                                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemDataBound="LV_ItemDataBound">
                                            <ItemTemplate>
                                                <div style="width: 90px; height: 70px; margin: 3px; padding: 2px; float: left;">
                                                    <img alt="img" src="/<%# Eval("img_thumb") %>" width="34" height="32" id="conf_<%# Eval("id") %>" />
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                    <asp:DropDownList runat="server" ID="drp_options">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' />
                                                    <br />
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("inner_notes") %>' />
                                                    <asp:Label runat="server" ID="lbl_inner_category" Text='<%# Eval("inner_category") %>'
                                                        Visible="false" />
                                                </div>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <span class="nessun">No data was returned.</span>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <div id="itemPlaceholderContainer" runat="server" style="width: 1000px">
                                                    <div id="itemPlaceholder" runat="server" />
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </LayoutTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h1>
                                            HOMEAWAY AMENITIES
                                        </h1>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="boxmodulo">
                            <table border="0" cellspacing="3" cellpadding="0">
                                <tr>
                                    <td>
                                        <asp:LinqDataSource ID="LDS_HA" runat="server" OrderBy="inner_notes" ContextTypeName="RentalInRome.data.magaRental_DataContext"
                                            TableName="RNT_TB_HACONFIG">
                                        </asp:LinqDataSource>
                                        <asp:ListView ID="LV_HA" runat="server" DataSourceID="LDS_HA" OnItemDataBound="LV_HA_ItemDataBound">
                                            <ItemTemplate>
                                                <div style="width: 180px; height: 70px; margin: 3px; padding: 2px; float: left;">
                                                    <asp:CheckBox ID="chk_HA" runat="server" />
                                                    <asp:DropDownList ID="drp_count" runat="server">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_HA_id" runat="server" Visible="false" Text='<%# Eval("id") %>' />
                                                    <br />
                                                    <br />
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("inner_notes") %>' />
                                                </div>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <span class="nessun">No data was returned.</span>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <div id="itemPlaceholderContainer" runat="server" style="width: 1000px">
                                                    <div id="itemPlaceholder" runat="server" />
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </LayoutTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="salvataggio">
                                            <div class="bottom_salva">
                                                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click" CausesValidation="true"
                                                    TabIndex="28" ValidationGroup="price"><span>Salva Modifiche</span></asp:LinkButton>
                                            </div>
                                            <div class="bottom_salva">
                                                <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla</span></asp:LinkButton>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
                <div class="salvataggio">
                    <div class="nulla">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
