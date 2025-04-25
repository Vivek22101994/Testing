<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_staffInLang.ascx.cs" Inherits="RentalInRome.uc.UC_staffInLang" %>
<div class="stafflanglist" id="pnlCont" runat="server" visible="false">
    <img alt="" src="/<%=ltr_img_thumb.Text %>" style="width: 275px; height: 225px;" />
    <span class="name">
        <%=ltr_name_full.Text %></span>
    <span class="tel" style="font-size: 12px;">
        <a href="mailto:info@rentalinrome.com?subject=refererPage:<%= currPagePath %> - <%=ltr_name_full.Text %>" target="_blank" style="color: #FFFFFF;">
            info@rentalinrome.com
        </a>
    </span>
</div>
<asp:Literal ID="ltr_name_full" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="ltr_email" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="ltr_img_thumb" runat="server" Visible="false"></asp:Literal>

