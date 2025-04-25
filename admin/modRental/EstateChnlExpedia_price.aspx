<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlExpedia_price.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlExpedia_price" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucEstateChnlExpediaTab.ascx" TagName="ucNav" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
            .main div.mainbox {width:100%;}
            .ui-datepicker td{
                padding: 1px !important;
            }
            .RadPicker td{
                padding: 0 !important;
            }
            .ui-datepicker .rntCal {
                margin: 0 !important;
                width: 22px !important;
            }
            .mainbox.iCalMainBox table tr td:first-child {
                padding: 10px;
            }
            .mainbox.iCalMainBox table tr td input {
                margin: 0;
            }
        </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <%# rntUtils.getAgent_logoForDetailsPage(ChnlExpediaProps.IdAdMedia) %>
            <h1 class="titolo_main">Prices for Expedia:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop" id="tabsHomeaway">
                    <uc1:ucNav ID="ucNav" runat="server" />
                    <div class="nulla" style="height: 20px;"></div>

                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click"><span>Save</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Cancel</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla"></div>
            <div class="mainline mainChnl mainExpedia expediaPrice">
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <div class="boxmodulo">
                            <table>
                                <asp:ListView ID="LvRatePlans" runat="server">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("RatePlanId") %>' />
                                        <tr>
                                            <td class="td_title"><%# Eval("RatePlanId") %> - <%# Eval("name") %>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_rate_changeIsDiscount" runat="server" Style="height: 25px;" Width="100px">
                                                    <asp:ListItem Value="0">increase of</asp:ListItem>
                                                    <asp:ListItem Value="1">discount of</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_rate_changeAmount" runat="server" Width="50" MinValue="0" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_rate_changeIsPercentage" runat="server" Style="height: 25px;" Width="50px">
                                                    <asp:ListItem Value="1">%</asp:ListItem>
                                                    <asp:ListItem Value="0">EUR</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="nulla"></div>
            </div>
            <script type="text/javascript">
                function setCal() {
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
