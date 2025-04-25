<%@ Page Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="RentalInRome.affiliatesarea.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_referer" Value="default.aspx" runat="server" />
    <div id="contatti">
        <div class="sx">
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px; min-height:100px;">
                    <div style="display: block; clear: both; width: 232px; margin: 0 13px 0 9px;">
                        <asp:PlaceHolder ID="PH_login" runat="server">
                            <strong style="border-bottom:1px solid #B9B9CD; display:block; font-size:14px; margin-bottom:5px; padding-bottom:3px;;">Please Log in</strong>
                            <div class="" style="margin: 0 0 10px;">
                                User Name:
                                <br />
                                <asp:TextBox ID="txtUser" runat="server" MaxLength="50" ValidationGroup="login" Width="225px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUser" Display="Dynamic" ErrorMessage="<br/>* Required" ValidationGroup="login"></asp:RequiredFieldValidator>
                                <br />
                                Password:
                                <br />
                                <asp:TextBox ID="txtPwd" runat="server" MaxLength="50" TextMode="Password" ValidationGroup="login" Width="225px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPwd" Display="Dynamic" ErrorMessage="<br/>* Required" ValidationGroup="login"></asp:RequiredFieldValidator>
                            </div>
                            <div class="menu_area_reservation_details">
                                <asp:LinkButton ID="lnk_login" runat="server" OnClick="lnk_login_Click" ValidationGroup="login"><span><%=CurrentSource.getSysLangValue("lblSubmit")%></span></asp:LinkButton>
                                <div class="nulla">
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        <div class="infostatoprenotazione" id="pnl_loginError" runat="server" visible="false">
                            <div class="ico_no">
                                <%=CurrentSource.getSysLangValue("lblAuthenticationError")%>
                            </div>
                        </div>
                        <div class="" style="margin: 0 0 10px;">
                            <a href="#" onclick="$('#pnl_pwdRecovery').toggle(); return false;"><%=CurrentSource.getSysLangValue("formPasswordRecovery")%></a>
                        </div>
                        <div id="pnl_pwdRecovery" class="" style="margin: 0 0 10px; display: none;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                            Enter your User Name or E-mail:
                            <br />
                            <asp:TextBox ID="txt_pwdRecoveryEmail" runat="server" MaxLength="50" ValidationGroup="pwdrec" Width="225px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_pwdRecoveryEmail" Display="Dynamic" ErrorMessage="* Required" ValidationGroup="pwdrec" style="border: none; padding: 0;"></asp:RequiredFieldValidator>
                            <div class="nulla">
                            </div>
                            <div class="menu_area_reservation_details">
                                <asp:LinkButton ID="lnk_pwdRecovery" runat="server" OnClick="lnk_pwdRecovery_Click" ValidationGroup="pwdrec"><span>Send</span></asp:LinkButton>
                                <div class="nulla">
                                </div>
                            </div>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="nulla" style="border-bottom: 1px dashed #B9B9CD;margin-bottom: 20px;padding-bottom: 17px;"></div>

                        <div class="menu_area_reservation_details">
                            <a href="mailto:info@rentalinrome.com?subject=reservationarea login" target="_blank" class="acolor2">
                                <span>
                                    <%=CurrentSource.getSysLangValue("lblContacts")%>
                                </span>
                            </a>
                        </div>
                        
                        <div class="nulla">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
