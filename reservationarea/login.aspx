<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="RentalInRome.reservationarea.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation - Login</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_id" runat="server" Visible="false" />
    <div id="contatti">
        <div class="sx">
            <asp:HiddenField ID="HF_mode" runat="server" Value="new" />
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <%=CurrentSource.getSysLangValue("lblYourReservation")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px; padding: 10px 14px 8px; width: auto;">
                    <div style="display: block; clear: both; width: 232px; margin: 0;">

                        <asp:PlaceHolder ID="PH_login" runat="server">
                            <strong style="font-size: 14px;">Please Log in</strong>
                            <br />
                            <br />
                            <div class="agent_request_pdf" style="margin: 0 0 15px;">
                                Reservation code:
                            <br />
                                <asp:TextBox ID="txt_resCode" runat="server" Style="width: 200px;"></asp:TextBox>
                                <br />
                                Reservation password:
                            <br />
                                <asp:TextBox ID="txt_resPwd" runat="server" TextMode="Password" Style="width: 200px;"></asp:TextBox>
                            </div>
                            <div class="menu_area_reservation_details">
                                <asp:LinkButton ID="lnk_login" runat="server" OnClick="lnk_login_Click"><span>Submit</span></asp:LinkButton>
                                <div class="nulla"></div>
                                
                            </div>
                             
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PH_ForGet" runat="server">

                            <div class="text-center col-md-12" style="margin: 10px 0;">
                                 <asp:LinkButton ID="lnk_goPwd" CssClass="" runat="server" OnClientClick="return true" OnClick="lnk_goPwd_Click" Style="display: block; margin: 10px 0 0 0;"><span><%=CurrentSource.getSysLangValue("formPasswordRecovery")%></span></asp:LinkButton>

                             </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PH_pwdRecover" runat="server">
                            <%--<div class="text-center col-md-12" style="margin: 10px 0;">
                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnk_goNewClient_Click" CssClass="btn btn-default-color">
                                             <i class="fa fa-user"></i><%=CurrentSource.getSysLangValue("reqGoNewClient") %>

                                        </asp:LinkButton>
                                    </div>
                                    <div class="nulla">
                                    </div>--%>
                            <div class="form-group">
                                <div class="agent_request_pdf" style="margin: 0 0 15px;">
                                    Reservation Code 
                                    <div class="nulla">
                                    </div>
                                    <asp:TextBox ID="txt_pwdRecover" runat="server" CssClass="form-control" Style="width: 200px;"></asp:TextBox>

                                </div>
                            </div>
                            <div class="menu_area_reservation_details" style="margin: 10px 0; ">
                                <asp:LinkButton ID="lnk_pwdRevover"  runat="server" OnClientClick="return true" OnClick="lnk_pwdRevover_Click"><span>Send</span></asp:LinkButton>
                            </div>
                            <div class="nulla">
                                </div>
                              
                        </asp:PlaceHolder>
                        <asp:Label ID="lbl_errorAlert" CssClass="error_form_book" runat="server" Visible="false"></asp:Label>
                        <div class="infostatoprenotazione" id="pnl_hasExpired" runat="server" visible="false">
                            <div class="ico_no">
                                <%=CurrentSource.getSysLangValue("resYouBookingRequestHasExpired")%>
                            </div>
                            <div class="time" id="pnl_RenewalRequested" runat="server" visible="false">
                                <div class="tx">
                                    Renewal Requested
                                </div>
                            </div>
                            <div class="menu_area_reservation_details">
                                <asp:LinkButton ID="lnk_requestRenewal" runat="server" OnClick="lnk_requestRenewal_Click"><span style="width: 165px;">Renewal Request </span></asp:LinkButton>
                                <div class="nulla"></div>

                            </div>
                        </div>
                        <div class="infostatoprenotazione" id="pnl_loginError" runat="server" visible="false">
                            <div class="ico_no">
                                Authentication error
                            </div>

                        </div>
                        <div class="menu_area_reservation_details">
                            <a href="mailto:info@rentalinrome.com?subject=reservationarea login" target="_blank">
                                <span>
                                    <%=CurrentSource.getSysLangValue("lblContacts")%>
                                </span>
                            </a>
                        </div>
                        <div id="thawteseal" style="text-align: center;" title="Click to Verify - This site chose Thawte SSL for secure e-commerce and confidential communications.">
                            <div>
                                <script type="text/javascript" src="https://seal.thawte.com/getthawteseal?host_name=rentalinrome.com&amp;size=S&amp;lang=en"></script>
                            </div>
                            <div>
                                <a href="https://www.thawte.com/products/" target="_blank" style="color: #000000; text-decoration: none; font: bold 10px arial,sans-serif; margin: 0px; padding: 0px;">ABOUT SSL CERTIFICATES</a>
                            </div>
                        </div>

                       
                    </div>

                </div>

            </div>
        </div>
    </div>
</asp:Content>
