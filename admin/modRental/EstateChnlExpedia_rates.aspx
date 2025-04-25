<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlExpedia_rates.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlExpedia_rates" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucEstateChnlExpediaTab.ascx" TagName="ucNav" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .main div.mainbox {
            width: 100%;
        }

        .ui-datepicker td {
            padding: 1px !important;
        }

        .RadPicker td {
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
            <h1 class="titolo_main">Prices Of Expedia:
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
            <%-- <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_get_rates" runat="server" OnClick="lnk_get_rates_Click"><span>Salva</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>--%>
            <div class="nulla"></div>
            <div class="mainline mainChnl mainRatesChnl">
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <span class="titoloboxmodulo" style="margin-top: 20px;margin-bottom:10px;"><%=contUtils.getLabel("lbl_fetch_rates_from_expedia") %> :
                        </span>
                        <div class="nulla"></div>
                        <span style="color: green;">* <%=contUtils.getLabel("lbl_expedia_rate_note") %></span>
                        <div class="boxmodulo" style="margin-top: 20px;">
                            <table style="width: 400px;">
                                <tr>
                                    <td>from:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="rdp_dtStart" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>to:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="rdp_dtEnd" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:LinkButton ID="lnk_get_rates" runat="server" OnClick="lnk_get_rates_Click" CssClass="inlinebtn"><span><%=contUtils.getLabel("lbl_fetch_rates_from_expedia") %></span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="nulla"></div>
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <span class="titoloboxmodulo" style="margin-top: 20px;"><%=contUtils.getLabel("lbl_view_rates_of_expedia") %> :</span>
                        <div class="boxmodulo">
                            <table style="width: 400px;">
                                <tr>
                                    <td>from:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="rdp_view_dtStart" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>to:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="rdp_view_dtEnd" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>RatePlanId:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_ratePlanId" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnk_view_rates" runat="server" OnClick="lnk_view_rates_Click" CssClass="inlinebtn"><span><%=contUtils.getLabel("lbl_view_rates_expedia") %></span></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_view_availability" runat="server" OnClick="lnk_view_availability_Click" CssClass="inlinebtn"><span><%=contUtils.getLabel("lbl_view_availabilities_expedia") %></span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="nulla"></div>

                <div class="mainbox iCalMainBox" id="div_rates" runat="server" visible="false">
                    <div class="center">
                        <span class="titoloboxmodulo" style="margin-top: 20px; margin-bottom: 20px;">Rates of Expedia :</span>
                        <div class="boxmodulo">
                            <asp:ListView ID="LV_rates" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Eval("date") !=null ? Convert.ToDateTime(Eval("date")).ToString("dd/MM/yyyy"):"" %></td>

                                        <td><%#Eval("RatePlanId") %></td>

                                        <td><%#Eval("price")!=null? Eval("price")+"".Replace("|",","):""%></td>

                                        <td><%#Eval("minLos")%></td>

                                        <td><%#Eval("maxLos")%></td>
                                        <td><%#Eval("isClosed")%></td>

                                        <td><%#Eval("isClosedOnArrival")%></td>

                                        <td><%#Eval("isClosedOnDeparture")%></td>

                                        <td><%#Eval("currency")%></td>
                                    </tr>
                                </ItemTemplate>
                                <LayoutTemplate>
                                    <div class="table_fascia">
                                        <table border="0" cellpadding="0" cellspacing="0" style="">
                                            <tr style="text-align: left">
                                                <th style="width: 150px">Date
                                                </th>
                                                <th style="width: 150px">RatePlanId
                                                </th>
                                                <th style="width: 250px">Price(Format : occupany-price)
                                                </th>
                                                <th style="width: 100px">MinLOS
                                                </th>
                                                <th style="width: 100px">MaxLOS
                                                </th>
                                                <th style="width: 100px">Closed?
                                                </th>
                                                <th style="width: 150px">Closed On Arrival?
                                                </th>
                                                <th style="width: 150px">Closed On Departure?
                                                </th>
                                                <th style="width: 150px">Currency
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </div>
                                </LayoutTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>

                <div class="nulla"></div>

                <div class="mainbox iCalMainBox" id="div_availability" runat="server" visible="false">
                    <div class="center">
                        <span class="titoloboxmodulo" style="margin-top: 20px; margin-bottom: 20px;">Availabities of Expedia :</span>
                        <div class="boxmodulo">
                            <asp:ListView ID="LV_availability" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Eval("date") !=null ? Convert.ToDateTime(Eval("date")).ToString("dd/MM/yyyy"):"" %></td>

                                        <td><%#Eval("baseAllocation") %></td>

                                        <td><%#Eval("flexibleAllocation") %></td>

                                        <td><%#Eval("totalInventoryAvailable") %></td>

                                        <td><%#Eval("totalInventorySold") %></td>
                                    </tr>
                                </ItemTemplate>
                                <LayoutTemplate>
                                    <div class="table_fascia">
                                        <table border="0" cellpadding="0" cellspacing="0" style="">
                                            <tr style="text-align: left">
                                                <th style="width: 150px">Date
                                                </th>
                                                <th style="width: 100px">baseAllocation
                                                </th>
                                                <th style="width: 100px">flexibleAllocation
                                                </th>
                                                <th style="width: 100px">totalInventoryAvailable
                                                </th>
                                                <th style="width: 100px">totalInventorySold
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </div>
                                </LayoutTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                function setCal() {
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

