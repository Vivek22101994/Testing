<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="utility.aspx.cs" Inherits="RentalInRome.admin.utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="mainline">
		<!-- BOX 1 -->
		<div class="mainbox">
			<div class="top">
				<div style="float: left;">
					<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
				<div style="float: right;">
					<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
			</div>
			<div class="center">
				<span class="titoloboxmodulo">
                    <asp:Literal ID="ltrStatus" runat="server"></asp:Literal>..</span>
				<div class="boxmodulo">
					<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkTemp" runat="server" OnClick="lnkTemp_Click">Temp</asp:LinkButton>
                            </td>
                        </tr>
                        <tr class="alternate">
							<td>
                                <asp:LinkButton ID="lnk_updateSrs" runat="server" onclick="lnk_updateSrs_Click">Aggiorna Prenotazioni di SRS ultimo mese</asp:LinkButton>
							</td>
						</tr>
                        <tr>
							<td>
                                <asp:LinkButton ID="lnk_updateEco" runat="server" onclick="lnk_updateEco_Click">Aggiorna Prenotazioni di Ecopulizie ultimo mese</asp:LinkButton>
                            </td>
						</tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnk_updateReservationPwd" runat="server" OnClick="lnk_updateReservationPwd_Click">Aggiorna Password delle pren</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnk_updateReservationInvoice" runat="server" OnClick="lnk_updateReservationInvoice_Click">Aggiorna Fatture</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnk_updateReservationPayment" runat="server" OnClick="lnk_updateReservationPayment_Click">Aggiorna Pagamenti delle pren</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnk_updateReservationCity" runat="server" OnClick="lnk_updateReservationCity_Click">Aggiorna Citta delle pren</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
				</div>
			</div>
			<div class="bottom">
				<div style="float: left;">
					<img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
				<div style="float: right;">
					<img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
			</div>
		</div>
        <div class="mainbox">
            <div class="top">
                <div style="float: left;">
                    <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                <div style="float: right;">
                    <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
            </div>
            <div class="center">
                <span class="titoloboxmodulo">Invio Conferme di prenotazione</span>
                <div class="boxmodulo">
                    <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                        <tr>
                            <td>
                                Codice Pren: <br/>
                                <asp:TextBox ID="txt_code_sendPaymentMail" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnk_sendPaymentMail" runat="server" OnClick="lnk_sendPaymentMail_Click">Invia</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="bottom">
                <div style="float: left;">
                    <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                <div style="float: right;">
                    <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
            </div>
        </div>
        <div class="mainbox">
            <div class="top">
                <div style="float: left;">
                    <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                <div style="float: right;">
                    <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
            </div>
            <div class="center">
                <span class="titoloboxmodulo">Chiama Procedura di cambiamento</span>
                <div class="boxmodulo">
                    <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                        <tr>
                            <td>
                                Codice Pren:
                                <br />
                                <asp:TextBox ID="txt_code_reservationOnChange" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnk_reservationOnChange" runat="server" OnClick="lnk_reservationOnChangeClick">Invia</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="bottom">
                <div style="float: left;">
                    <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                <div style="float: right;">
                    <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
            </div>
        </div>
    </div>
	<div class="nulla">
	</div>
</asp:Content>
