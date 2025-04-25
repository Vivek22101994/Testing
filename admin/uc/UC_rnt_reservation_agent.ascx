<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_agent.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_agent" %>

<%@ Register Src="/admin/modRental/uc/resSendEmailCustom.ascx" TagName="ucMannualEmail" TagPrefix="uc1" %>

<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_isChanged" runat="server" />
<asp:HiddenField ID="HF_IdReservation" runat="server" />
<asp:HiddenField ID="HF_IdAgent" runat="server" />
<asp:Literal ID="ltr_content" runat="server" Visible="false"></asp:Literal>
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <h3>Agenzia
        </h3>
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia Agenzia</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_ok" runat="server">
                <tr>
                    <td>
                        <%= ltr_content.Text%>
                    </td>
                </tr>
            </table>
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_no" runat="server">
                <tr>
                    <td>Non è abbinata a nessuna agenzia
                    </td>
                </tr>
            </table>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>Abbina ad una agenzia</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                <tr>
                    <td colspan="2">
                        <asp:DropDownList ID="drp_agent" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="salvataggio" id="pnl_buttons" runat="server">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" ValidationGroup="discount"><span>Salva</span></asp:LinkButton>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <div class="nulla">
    </div>
    <div class="price_div">
        <asp:PlaceHolder ID="PH_mannualSendEmail" runat="server" Visible="false">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="" cellpadding="0" cellspacing="0" style="width: 90%">
                        <tr>
                            <td>
                                <div class="salvataggio">
                                    <div class="bottom_salva">
                                        <asp:LinkButton runat="server" ID="lnkSendCustomeNotificaiton" OnClick="lnkSendCustomeNotificaiton_Click"> <span>  avviso al cliente booking di addebito </span> </asp:LinkButton>
                                    </div>
                                    <div>
                                        <uc1:ucMannualEmail ID="UC_MannualEmail" runat="server" Visible="false" />
                                    </div>
                                    <div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:PlaceHolder>
    </div>
</div>
