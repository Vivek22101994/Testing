<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlExpedia_ratePlans.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlExpedia_ratePlans" %>

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
            <h1 class="titolo_main">RatePlans of Property for Expedia: <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
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
            <asp:ListView ID="LvRatePlans" runat="server">
                <ItemTemplate>
                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("RatePlanId") %>' />
                    <div class="mainbox">
                        <h1 class="titolo_main">RatePlan #<%# Eval("RatePlanId") %></h1>
                        <div class="center">
                            <span class="titoloboxmodulo" style="margin-top: 20px;">Main Data</span>
                            <div class="boxmodulo">
                                <table>
                                    <tr>
                                        <td class="td_title">Code 
                                        </td>
                                        <td>
                                            <%# Eval("code") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Name 
                                        </td>
                                        <td>
                                            <%# Eval("name") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Status 
                                        </td>
                                        <td>
                                            <%# Eval("status")+""=="1"?"Active":"Inactive" %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Type 
                                        </td>
                                        <td>
                                            <%# Eval("type") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">DistributionModel 
                                        </td>
                                        <td>
                                            <%# Eval("distributionModel") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">RateAcquisitionType 
                                        </td>
                                        <td>
                                            <%# Eval("rateAcquisitionType") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">ParentId 
                                        </td>
                                        <td>
                                            <%# Eval("parentId") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">RateLinkStart 
                                        </td>
                                        <td>
                                            <%# Eval("rateLinkStart") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">RateLinkEnd 
                                        </td>
                                        <td>
                                            <%# Eval("rateLinkEnd") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">isAvailStatusLinked 
                                        </td>
                                        <td>
                                            <%# Eval("isAvailStatusLinked") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">areMinMaxLOSLinked 
                                        </td>
                                        <td>
                                            <%# Eval("areMinMaxLOSLinked") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">isCTALinked 
                                        </td>
                                        <td>
                                            <%# Eval("isCTALinked") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">isCTDLinked 
                                        </td>
                                        <td>
                                            <%# Eval("isCTDLinked") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">rateLinkExceptions 
                                        </td>
                                        <td>
                                            <%# Eval("rateLinkExceptions") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">pricingModel 
                                        </td>
                                        <td>
                                            <%# Eval("pricingModel") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">occupantsForBaseRate 
                                        </td>
                                        <td>
                                            <%# Eval("occupantsForBaseRate") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">depositRequired 
                                        </td>
                                        <td>
                                            <%# Eval("depositRequired") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">minLOSDefault 
                                        </td>
                                        <td>
                                            <%# Eval("minLOSDefault") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">maxLOSDefault 
                                        </td>
                                        <td>
                                            <%# Eval("maxLOSDefault") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">minAdvBookDays 
                                        </td>
                                        <td>
                                            <%# Eval("minAdvBookDays") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">maxAdvBookDays 
                                        </td>
                                        <td>
                                            <%# Eval("maxAdvBookDays") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">bookDateStart 
                                        </td>
                                        <td>
                                            <%# Eval("bookDateStart") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">bookDateEnd 
                                        </td>
                                        <td>
                                            <%# Eval("bookDateEnd") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">travelDateStart 
                                        </td>
                                        <td>
                                            <%# Eval("travelDateStart") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">travelDateEnd 
                                        </td>
                                        <td>
                                            <%# Eval("travelDateEnd") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">mobileOnly 
                                        </td>
                                        <td>
                                            <%# Eval("mobileOnly") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">createDateTime 
                                        </td>
                                        <td>
                                            <%# Eval("createDateTime") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">updateDateTime 
                                        </td>
                                        <td>
                                            <%# Eval("updateDateTime") %>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <asp:ListView ID="LvRatePlanLinkDefinition" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("linkType") %></td>
                                    <td><%# Eval("linkValue") %></td>
                                </tr>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("uid") %>' />
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div class="center">
                                    <span class="titoloboxmodulo" style="margin-top: 20px;">RatePlan LinkDefinitions</span>
                                    <div class="boxmodulo">
                                        <table>
                                            <tr>
                                                <th>LinkType</th>
                                                <th>LinkValue</th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </table>
                                    </div>
                                </div>
                            </LayoutTemplate>
                        </asp:ListView>
                        <asp:ListView ID="LvDayOfWeekBookingRestriction" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("type") %></td>
                                    <td><%# Eval("sun") %></td>
                                    <td><%# Eval("mon") %></td>
                                    <td><%# Eval("tue") %></td>
                                    <td><%# Eval("wed") %></td>
                                    <td><%# Eval("thu") %></td>
                                    <td><%# Eval("fri") %></td>
                                    <td><%# Eval("sat") %></td>
                                </tr>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("uid") %>' />
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div class="center">
                                    <span class="titoloboxmodulo" style="margin-top: 20px;">Day Of Week Booking Restrictions</span>
                                    <div class="boxmodulo">
                                        <table>
                                            <tr>
                                                <th>type</th>
                                                <th>sun</th>
                                                <th>mon</th>
                                                <th>tue</th>
                                                <th>wed</th>
                                                <th>thu</th>
                                                <th>fri</th>
                                                <th>sat</th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </table>
                                    </div>
                                </div>
                            </LayoutTemplate>
                        </asp:ListView>
                        <asp:ListView ID="LvCompensation" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("_default") %></td>
                                    <td><%# Eval("percent") %></td>
                                    <td><%# Eval("minAmount") %></td>
                                    <td><%# Eval("from") %></td>
                                    <td><%# Eval("to") %></td>
                                    <td><%# Eval("sun") %></td>
                                    <td><%# Eval("mon") %></td>
                                    <td><%# Eval("tue") %></td>
                                    <td><%# Eval("wed") %></td>
                                    <td><%# Eval("thu") %></td>
                                    <td><%# Eval("fri") %></td>
                                    <td><%# Eval("sat") %></td>
                                </tr>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("uid") %>' />
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div class="center">
                                    <span class="titoloboxmodulo" style="margin-top: 20px;">Compensations</span>
                                    <div class="boxmodulo">
                                        <table>
                                            <tr>
                                                <th>Default</th>
                                                <th>percent</th>
                                                <th>minAmount</th>
                                                <th>from</th>
                                                <th>to</th>
                                                <th>sun</th>
                                                <th>mon</th>
                                                <th>tue</th>
                                                <th>wed</th>
                                                <th>thu</th>
                                                <th>fri</th>
                                                <th>sat</th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </table>
                                    </div>
                                </div>
                            </LayoutTemplate>
                        </asp:ListView>
                        <asp:ListView ID="LvCancelPolicy" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("_default") %></td>
                                    <td><%# Eval("startDate") %></td>
                                    <td><%# Eval("endDate") %></td>
                                    <td><%# Eval("cancelWindow") %></td>
                                    <td><%# Eval("nonRefundable") %></td>
                                    <td><%# Eval("createDateTime") %></td>
                                    <td><%# Eval("updateDateTime") %></td>
                                    <td>
                                        <asp:ListView ID="LvPenalty" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("insideWindow") %></td>
                                                    <td><%# Eval("flatFee") %></td>
                                                    <td><%# Eval("perStayFee") %></td>
                                                </tr>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("uid") %>' />
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table>
                                                    <tr>
                                                        <th>insideWindow</th>
                                                        <th>flatFee</th>
                                                        <th>perStayFee</th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server" />
                                                </table>
                                            </LayoutTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("uid") %>' />
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div class="center">
                                    <span class="titoloboxmodulo" style="margin-top: 20px;">Cancel Policys</span>
                                    <div class="boxmodulo">
                                        <table>
                                            <tr>
                                                <th>Default</th>
                                                <th>startDate</th>
                                                <th>endDate</th>
                                                <th>cancelWindow</th>
                                                <th>nonRefundable</th>
                                                <th>createDateTime</th>
                                                <th>updateDateTime</th>
                                                <th>Penalties</th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </table>
                                    </div>
                                </div>
                            </LayoutTemplate>
                        </asp:ListView>
                    </div>

                    <div class="nulla">
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <div class="mainline mainChnl mainExpedia">
                        <div id="itemPlaceholder" runat="server" />
                    </div>
                </LayoutTemplate>
            </asp:ListView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
