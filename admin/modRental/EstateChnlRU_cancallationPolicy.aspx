<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlRU_cancallationPolicy.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateChnlRU_cancallationPolicy" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucRUEstateDetailsTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tabsTop.tabsChannelsTop table td a[title="Cancellation Policy"], #tabsHomeaway.tabsTop table td a[title="Cancellation Policy"]  {
	        background:#848484;
	        border-color:#606060;
	        color:#FFF;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <img src="/images/css/ru-integrated-magarental.png" class="homeAwayLogo RUlogo" alt="Integrated with RentalsUnited" />

            <h1 class="titolo_main">Dettagli della posizione di appartamento:
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
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click" ValidationGroup="dati"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="/admin/rnt_estate_list.aspx"><span>Torna nel elenco</span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline mainChannels mainHomeAway mainCancellationPolicy">
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
                                    <table class="tableBoxBooking tableCancellationPolicy" cellpadding="3" cellspacing="0" style="width: 60%;">
                                      
                                        <tr>
                                            <td valign="middle" align="left">Cancalltion Penalty Type </td>
                                            <td>
                                                <asp:DropDownList ID="drp_penalty_type" runat="server">
                                                    <asp:ListItem Text="PERCENT_ORDER" Value="PERCENT_ORDER"></asp:ListItem>
                                                    <asp:ListItem Text="PERCENT_LINE_ITEMS" Value="PERCENT_LINE_ITEMS"></asp:ListItem>
                                                    <asp:ListItem Text="CHARGE" Value="CHARGE"></asp:ListItem>
                                                </asp:DropDownList>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">DeadLine Days Befor Checkin</td>
                                            <td>
                                                <telerik:RadNumericTextBox  MinValue="0" Width="50"
                                                    MaxValue="999999999"
                                                    ID="ntxt_days"
                                                    runat="server">
                                                    <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">Cancalltion Policy Charge</td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_amount" runat="server" Width="50" MinValue="0" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                          <tr>
                                            <td valign="middle" align="left">Description </td>
                                            <td>
                                                <asp:TextBox ID="txt_description" runat="server" TextMode="MultiLine"></asp:TextBox>

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
