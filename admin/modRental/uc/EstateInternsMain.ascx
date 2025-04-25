<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EstateInternsMain.ascx.cs" Inherits="MagaRentalCE.admin.modRental.ucEstateDett.EstateInternsMain" %>

<%@ Register Src="/admin/modRental/uc/EstateInterns.ascx" TagName="EstateInterns" TagPrefix="uc1" %>
<div class="boxBooking">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />

            <asp:HiddenField ID="HF_CurrId" Value="" runat="server" />
            <ul class="tabs tabBoxDescrizioneInterns">
                <li>
                    <a href="#" id="lnk_main"><%= contUtils.getLabel("lblBedRooms")%></a>
                </li>
                <li>
                    <a href="#"><%= contUtils.getLabel("lblBathRooms")%></a>
                </li>
                <li>
                    <a href="#" id="A1"><%= contUtils.getLabel("lblKitchenType")%></a>
                </li>
                <li>
                    <a href="#"><%= contUtils.getLabel("lblLivingRoom")%></a>
                </li>
            </ul>
            <div class="panes panesBoxDescrizioneInterns">
                <div class="divIntern">
                    <uc1:EstateInterns ID="EstateInternsBedroom" runat="server" />
                </div>
                <div class="divIntern">
                    <uc1:EstateInterns ID="EstateInternsBathroom" runat="server" />
                </div>
                <div class="divIntern">
                    <uc1:EstateInterns ID="EstateInternsKitchen" runat="server" />
                </div>
                <div class="divIntern">
                    <uc1:EstateInterns ID="EstateInternsLivingRoom" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<script type="text/javascript">
    function setInternsDescTab() {

        $("ul.tabs.tabBoxDescrizioneInterns").tabs("div.panes.panesBoxDescrizioneInterns > div.divIntern");
        console.log('here');
    }
    $(document).ready(function () {
        setInternsDescTab();
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setInternsDescTab);
    });
</script>
