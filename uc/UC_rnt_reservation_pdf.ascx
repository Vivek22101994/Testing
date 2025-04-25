<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_pdf.ascx.cs" Inherits="RentalInRome.uc.UC_rnt_reservation_pdf" %>
<asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_IdReservation" runat="server" Value="0" Visible="false" />
<asp:Literal ID="ltr_error" runat="server"></asp:Literal>
<h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
    <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
</h3>
<div class="nulla">
</div>
<span class="tit_sez">
    Fatture/Voucher</span>
<div class="nulla">
</div>
<div id="pnl_request_cont" class="box_client_booking" runat="server" style="width: 545px;">
    <asp:HyperLink ID="HL_voucher" runat="server" CssClass="btnDownload inattivo" Target="_blank" Enabled="false">
        <span style="width: 415px;">
        Scarica il Voucher
        </span>
    </asp:HyperLink>
    <div class="nulla">
    </div>
    <asp:HyperLink ID="HL_invoice" runat="server" CssClass="btnDownload inattivo" Target="_blank" Enabled="false" Style="margin-top: 10px;">
        <span style="width: 415px;">
        Scarica la Fattura
        </span>
    </asp:HyperLink>
    <div class="nulla">
    </div>
    <%=ltr_error.Text %>
</div>
