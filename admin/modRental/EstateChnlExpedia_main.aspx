<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlExpedia_main.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlExpedia_main" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucEstateChnlExpediaTab.ascx" TagName="ucNav" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_RoomTypeId" Value="" runat="server" />
            <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
            <%# rntUtils.getAgent_logoForDetailsPage(ChnlExpediaProps.IdAdMedia) %>
            <h1 class="titolo_main">Details of Property for Expedia: <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop" id="tabsHomeaway">
                    <uc1:ucNav ID="ucNav" runat="server" />
                </div>
            </div>
            <div class="nulla">
            </div>

            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" ValidationGroup="dati" OnClick="lnk_saveOnly_Click"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="mainline mainChnl mainExpedia" id="pnlNotConnected" runat="server">
                <div class="mainbox">
                    <div class="top">
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            Attenzione! Non e abbinato con RoomType su Expedia.
                            <br/>
                            <a href='/admin/modRental/ChnlExpediaRoomTypeList.aspx?for=<%= IdEstate%>'>Clicca qui per abbinare ad una struttura esistente, </a>
                        </div>
                    </div>
                    <div class="bottom">
                    </div>
                </div>
            </div>
            <div class="mainline mainChnl mainExpedia" id="pnlError" runat="server" visible="false">
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

            <div class="mainline mainChnl mainExpedia" id="pnlRoomTypeDetails" runat="server" visible="false">
                <div class="mainbox">
                    <div class="top">
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo" style="margin-top: 20px;">Main Data</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td class="td_title">Code 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_code" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Name 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_name" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Status 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_status" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">SmokingPref 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_smokingPref" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">MaxOccupants 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_maxOccupants" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <asp:ListView ID="LvBedType" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("BedTypeId") %></td>
                                <td><%# Eval("name") %></td>
                            </tr>
                            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("BedTypeId") %>' />
                        </ItemTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <div class="center">
                                <span class="titoloboxmodulo" style="margin-top: 20px;">Bed Types</span>
                                <div class="boxmodulo">
                                    <table>
                                        <tr>
                                            <th>ID</th>
                                            <th>Name</th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server" />
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                    </asp:ListView>
                    <asp:ListView ID="LvOccupancyByAge" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ageCategory") %></td>
                                <td><%# Eval("minAge") %></td>
                                <td><%# Eval("maxOccupants") %></td>
                            </tr>
                            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("ageCategory") %>' />
                        </ItemTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <div class="center">
                                <span class="titoloboxmodulo" style="margin-top: 20px;">Occupancy By Ages</span>
                                <div class="boxmodulo">
                                    <table>
                                        <tr>
                                            <th>AgeCategory</th>
                                            <th>MinAge</th>
                                            <th>MaxOccupants</th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server" />
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                    </asp:ListView>
                    <asp:ListView ID="LvRateThreshold" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("type") %></td>
                                <td><%# Eval("minAmount") %></td>
                                <td><%# Eval("maxAmount") %></td>
                                <td><%# Eval("source") %></td>
                            </tr>
                            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("type") %>' />
                        </ItemTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <div class="center">
                                <span class="titoloboxmodulo" style="margin-top: 20px;">Rate Thresholds</span>
                                <div class="boxmodulo">
                                    <table>
                                        <tr>
                                            <th>Type</th>
                                            <th>Min Amount</th>
                                            <th>Max Amount</th>
                                            <th>Source</th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server" />
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                    </asp:ListView>
                    <div class="bottom">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
