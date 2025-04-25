<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlAtraveo_features.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlAtraveo_features" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucEstateChnlAtraveoTab.ascx" TagName="ucNav" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <img src="/images/css/atraveo-integrated-magarental.png" class="homeAwayLogo" alt="Integrated with Atraveo" />
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <h1 class="titolo_main">Features of Property for Atraveo:<asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
        </h1>
    </telerik:RadCodeBlock>
    <div id="fascia1" style="margin-bottom: 10px;">
        <div class="tabsTop" id="tabsHomeaway">
            <uc1:ucNav ID="ucNav" runat="server" />
        </div>
    </div>
    <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="paneCaratt">
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click"><span>Salva Modifiche</span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
            </div>
            <div class="nulla">
            </div>
        </div>
        <div class="nulla">
        </div>
        <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
        <table class="tableBoxBooking tableCaratt" cellpadding="0" cellspacing="0">
            <asp:ListView ID="Lv" runat="server" GroupItemCount="2">
                <ItemTemplate>
                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("code") %>' />
                    <asp:Label ID="lbl_type" Visible="false" runat="server" Text='<%# Eval("type") %>' />
                    <td valign="middle" align="left" style="width: 26%;">
                        <%# Eval("title") %>
                    </td>
                    <td valign="middle" align="left" style="width: 23%;">
                        <asp:CheckBox ID="yesno" runat="server" AutoPostBack="true" OnCheckedChanged="chkChanged" Style="float: left;" />
                        <span style="float: left;">
                            <telerik:RadNumericTextBox ID="numeric" runat="server" Width="50">
                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                            </telerik:RadNumericTextBox>
                        </span>
                        <span style="float: left;">
                            <%# Eval("metrics") %>
                        </span>
                    </td>
                </ItemTemplate>
                <ItemSeparatorTemplate>
                    <td valign="middle" align="left" class="tdSpaceCaratt"></td>
                </ItemSeparatorTemplate>
                <GroupTemplate>
                    <tr>
                        <td id="itemPlaceholder" runat="server"></td>
                    </tr>
                </GroupTemplate>
            </asp:ListView>
        </table>
    </telerik:RadAjaxPanel>
    <div class="nulla">
    </div>
</asp:Content>
