<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="partnerservices.aspx.cs" Inherits="RentalInRome.reservationarea.partnerservices" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Partner Services</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_id" runat="server" Visible="false" />
    <div id="contatti">
        <div class="sx">
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <%=CurrentSource.getSysLangValue("lblYourReservation")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:UC_sx ID="UC_sx1" runat="server" />
                </div>
            </div>
        </div>
        <div class="dx" id="transfert">
            <h3 style="margin-bottom: 20px; margin-left: 0;" class="underlined">Partner Services</h3>
            <div class="box_reservation_gen">
                <ul class="listavisite">
                    <li>
                        <div class="lasx" style="width: 255px;">
                            <div class="contfoto" style="width: auto; height: auto;">
                                <a href="/pdfgenerator/lbt_voucher.aspx?type=sconto10&uid=<%=currReservationTBL.uid_2 %>" target="_blank">
                                    <img src="/images/partners/lbt/discount-banner_<%= CurrentLang.ID %>.gif" />
                                </a>
                            </div>
                        </div>
                        <div class="ladx" style="width: 258px;">
                            <div class="ladxtit">
                                <span class="titauto1">LittleBigTown gives you 10% off</span>
                                <span class="titauto2">Toy shop in the center of Rome</span>
                            </div>
                            <span class="ladxtx">On Piazza Venezia Italy's best toy shop, in the heart of the Eternal City!</span>
                            <div style="">
                                <div style="float: right;">
                                    <a href="/pdfgenerator/lbt_voucher.aspx?type=sconto10&uid=<%=currReservationTBL.uid_2 %>" target="_blank" class="ric_serv">&gt; Download your coupon</a>
                                </div>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </li>
                </ul>
            </div>
            <div class="nulla">
            </div>
        </div>
    </div>
</asp:Content>
