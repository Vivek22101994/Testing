<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/mobile/masterPage.Master" AutoEventWireup="true" CodeBehind="details.aspx.cs" Inherits="RentalInRome.reservationarea.mobile.details" %>

<%@ Register Src="ucHeader.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Visible="false" Value="" />
    <asp:HiddenField ID="HF_code" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_dtStart" runat="server" Visible="false" Value="1" />
    <asp:HiddenField ID="HF_dtEnd" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_num_adult" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_num_child_over" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_num_child_min" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_pr_total" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_deposit" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_commission_tf" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_commission_total" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_agency_fee" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_payment_total" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_owner" runat="server" Value="0" />
    <asp:HiddenField ID="HF_visa_isRequested" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HiddenField1" runat="server" Visible="false" Value="0" />
    <asp:Literal runat="server" ID="ltr_unique_id" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_cl_name_full" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_cl_name_honorific" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_est_code" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_est_address" Visible="false"></asp:Literal>
    <uc1:ucHeader ID="ucHeader" runat="server" />
    <div id="infoCont" data-role="content">
        <div class="infoBox" style="font-size: 11px;">
            <div style="display: block; clear: both; width: 232px; margin: 0;">
                <%=CurrentSource.getSysLangValue("pdf_Codice_di_prenotazione")%>: <strong>
                    <%= HF_code.Value%></strong>
                <br />
                <br />
                <%=CurrentSource.getSysLangValue("pdf_Firmatario")%>:
                <br />
                <strong style="font-size: 14px;">
                    <%= (ltr_cl_name_honorific.Text != "") ? ltr_cl_name_honorific.Text + "&nbsp;" : ""%>
                    <%= ltr_cl_name_full.Text%></strong>
                <br />
                <br />
                <%=CurrentSource.getSysLangValue("pdf_Nome_appartamento")%>:
                <br />
                <strong style="font-size: 14px;">
                    <%= ltr_est_code.Text%></strong>
                <br />
                <strong id="pnl_address" runat="server">
                    <%= ltr_est_address.Text%></strong>
                <br />
                <br />
                <%=CurrentSource.getSysLangValue("reqCheckInDate")%>: <strong>
                    <%= HF_dtStart.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
                <br />
                <%=CurrentSource.getSysLangValue("reqCheckOutDate")%>: <strong>
                    <%= HF_dtEnd.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
                <br />
                <%=CurrentSource.getSysLangValue("lblNumPersons")%>: <strong>
                    <%= "" + (HF_num_adult.Value.ToInt32() + HF_num_child_over.Value.ToInt32() + HF_num_child_min.Value.ToInt32())%>
                    Guests</strong>
                <br />
                <%= HF_num_adult.Value + " " + CurrentSource.getSysLangValue("reqAdults") + " + " + HF_num_child_over.Value + " " + CurrentSource.getSysLangValue("lblChildren3OrOver") + " + " + HF_num_child_min.Value + " " + CurrentSource.getSysLangValue("lblChildrenUnder3") + ""%>
                <br />
                <br />
                <div class="nulla">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
