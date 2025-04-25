<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHeader.ascx.cs" Inherits="RentalInRome.reservationarea.mobile.ucHeader" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:HiddenField ID="HF_type" runat="server" Value="inner" Visible="false" />
<asp:HiddenField ID="HF_title" runat="server" Value="" Visible="false" />
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
<asp:PlaceHolder ID="PH_welcomeMessage" runat="server">
    <div id="infoCont" data-role="content">
        <div class="infoBox" style="font-size: 11px;">
            <div style="display: block; clear: both; width: 232px; margin: 0;">
                <%=CurrentSource.getSysLangValue("mobileWelcome")%>, <strong style="font-size: 14px;">
                    <%= (ltr_cl_name_honorific.Text != "") ? ltr_cl_name_honorific.Text + "&nbsp;" : ""%>
                    <%= ltr_cl_name_full.Text%></strong>
                <br />
                Your reservation number: <strong>
                    <%= HF_code.Value%></strong>
                <br />
                <strong style="font-size: 14px;">
                    <%= ltr_est_code.Text%></strong>
                <br />
                <%=CurrentSource.getSysLangValue("lblDateFrom")%>: <strong>
                    <%= HF_dtStart.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
                <br />
                <%=CurrentSource.getSysLangValue("lblDateTo")%>: <strong>
                    <%= HF_dtEnd.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
                <br />
                <strong>
                    <%= HF_num_adult.Value + " Adults + " + HF_num_child_over.Value + " Children + " + HF_num_child_min.Value + " Infants"%></strong>
                <br />
                <div class="nulla">
                </div>
            </div>
        </div>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="PH_homePage" runat="server" Visible="false">
    <div data-role="header">
        <h1 style="margin: 0.6em auto 0.8em"><%=CurrentSource.getSysLangValue("lblYourReservation")%></h1>
    </div>
    <div data-role="content">
        <a href="details.aspx" id="A1" runat="server" data-role="button" data-icon="star">
            Reservation details
        </a>
        <a href="payment.aspx" id="hl_payment" runat="server" data-role="button" data-icon="star">
            <%=CurrentSource.getSysLangValue("lblPaymentSummary")%>
        </a>
        <a href="personaldata.aspx" id="hl_personaldata" runat="server" data-role="button" data-icon="star">
            <%=CurrentSource.getSysLangValue("lblPersonalData")%>
        </a>
        <a href="agentclientdata.aspx" id="hl_agentclientdata" runat="server" data-role="button" data-icon="star">
            Your Client details 
        </a>
        <a href="arrivaldeparture.aspx" id="hl_arrivaldeparture" runat="server" data-role="button" data-icon="star">
            <%=CurrentSource.getSysLangValue("lblArrivalAndDeparture")%>
        </a>
        <a href="bedselection.aspx" id="hl_bedselection" runat="server" data-role="button" data-icon="star">
            Bed selection 
        </a>
        <a href="pdf.aspx" id="hl_pdf" runat="server" data-role="button" data-icon="star">
            Voucher/<%=CurrentSource.getSysLangValue("lblInvoice")%>
        </a>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="PH_innerPage" runat="server">
    <div data-role="header">
        <a href="Default.aspx" data-role="button" data-icon="home">Home</a>
        <span class="ui-title" style="margin: 0.6em auto 0.8em 30%">
            <%=HF_title.Value%></span>
    </div>
</asp:PlaceHolder>
