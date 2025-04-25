<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/common/mpMobile.Master" AutoEventWireup="true" CodeBehind="util_bookingForm.aspx.cs" Inherits="RentalInRome.mobile.util_bookingForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script>
        mainViewInit = function () {
        }
        applicationInit = function () {
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main_top" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_main" runat="server">
    <div id="mainHomeMobile" class="mainMobile" data-stretch="true">
        <asp:HiddenField ID="HF_mode" runat="server" Value="new" />
        <asp:HiddenField ID="HF_IdClient" runat="server" Value="0" />
        <h1 class="titBg" style="text-align: center; margin-bottom: 2% !important;">
            <strong class="aptName" style="font-size: 1.5em;"><%= contUtils.getLabel("lblFinishYourReservations")%></strong>
        </h1>
        <div class="nulla">
        </div>
        <asp:PlaceHolder ID="PH_oldClient" runat="server">
            <h1 class="titPag" style="margin: 2% 0 2% 4%;">Utente registrato</h1>

            <div class="nulla">
            </div>

            <div class="mobileBox boxBooking">
                <div class="lineBooking">
                    <label>User Name*</label>
                    <asp:TextBox ID="txt_old_email" runat="server" Style=""></asp:TextBox>
                </div>
                <div class="lineBooking">
                    <label><%=contUtils.getLabel("reqPassword")%>*</label>
                    <asp:TextBox ID="txt_old_password" runat="server" Style="" TextMode="Password"></asp:TextBox>
                    <asp:LinkButton ID="lnk_goPwd" CssClass="lbforgotPassl" runat="server" OnClientClick="return true" OnClick="lnk_goPwd_Click"><%=contUtils.getLabel("formPasswordRecovery")%></asp:LinkButton>
                </div>
                <asp:LinkButton ID="lnk_login" CssClass="btnBigMobile" runat="server" OnClientClick="return true" OnClick="lnk_login_Click">Login</asp:LinkButton>
                <div class="nulla">
                </div>
                <asp:LinkButton ID="lnk_goNewClient" runat="server" OnClick="lnk_goNewClient_Click" CssClass="btnBigMobile"><%=contUtils.getLabel("reqGoNewClient") %></asp:LinkButton>
            </div>
            <div class="nulla">
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="PH_bookingForm" runat="server">
            <asp:PlaceHolder ID="PH_viewClient" runat="server">
                <table cellspacing="0" cellpadding="0" class="form bookingClient">
                    <tr>
                        <td valign="middle" align="left" class="label" colspan="2">
                            <strong style="font-size: 18px;">
                                <%= contUtils.getLabel("lblWelcome")%>&nbsp;<asp:Literal ID="ltr_honorific" runat="server"></asp:Literal>&nbsp;<asp:Literal ID="ltr_name_full" runat="server"></asp:Literal></strong>
                            <br />
                            <asp:Literal ID="ltr_country" runat="server"></asp:Literal>
                            <br />
                            <asp:Literal ID="ltr_phone_mobile" runat="server"></asp:Literal>
                            <br />
                            <asp:Literal ID="ltr_email" runat="server"></asp:Literal>
                            <br />
                        </td>
                    </tr>
                </table>
                <div class="nulla">
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PH_newClient" runat="server">
                <asp:LinkButton ID="lnk_goOldClient" runat="server" OnClick="lnk_goOldClient_Click" CssClass="user client" Visible="false"><%=contUtils.getLabel("reqGoRegisteredClient") %></asp:LinkButton>
                <div class="mobileBox boxBooking">
                    <div class="lineBooking">
                        <label><%=contUtils.getLabel("reqEmail")%>*</label>
                        <asp:TextBox ID="txt_email" runat="server" type="email"></asp:TextBox>
                        <span id="txt_email_check" class="alertErrorSmall" style="float: none; display: none;"></span>
                    </div>
                    <div class="lineBooking">
                        <asp:DropDownList ID="drp_honorific" runat="server" CssClass="mrSelect" Style="width: 50px; padding: 1%; margin-bottom: 10px;">
                        </asp:DropDownList>
                        <label style="margin-top: 1%; margin-left: 10px;"><%=contUtils.getLabel("lblName")%>*</label>
                        <asp:TextBox ID="txt_name_first" runat="server"></asp:TextBox>
                        <span id="txt_name_first_check" class="alertErrorSmall" style="float: none; display: none;">
                            <%=contUtils.getLabel("reqRequiredField")%></span>
                    </div>
                    <div class="lineBooking">
                        <label><%=contUtils.getLabel("lblSurname")%>*</label>
                        <asp:TextBox ID="txt_name_last" runat="server" Style=""></asp:TextBox>
                        <span id="txt_name_last_check" class="alertErrorSmall" style="float: none; display: none;">
                            <%=contUtils.getLabel("reqRequiredField")%></span>
                    </div>
                    <div class="lineBooking">
                        <label><%=contUtils.getLabel("reqPhoneNumber")%>*</label>
                        <asp:TextBox ID="txt_phone_mobile" runat="server"></asp:TextBox>
                        <span id="txt_phone_mobile_check" class="alertErrorSmall" style="float: none; display: none;">
                            <%=contUtils.getLabel("reqRequiredField")%></span>
                    </div>
                    <div class="lineBooking">
                        <label><%=contUtils.getLabel("reqLocation")%>*</label>
                        <asp:DropDownList runat="server" ID="drp_country">
                        </asp:DropDownList>
                        <span id="drp_country_check" class="alertErrorSmall" style="float: none; display: none;">
                            <%=contUtils.getLabel("reqRequiredCountrySelect")%></span>
                    </div>
                    <div class="lineBooking" id="chk_privacyAgree_cont">
                        <input type="checkbox" id="chk_privacyAgree" class="check" />
                        <a data-role="button" data-rel="modalview" href="#modalview-privacy" class="linkTerms"><%=CurrentSource.getSysLangValue("lblAcceptPersonalDataPrivacy")%></a>
                        <span id="chk_privacyAgree_check" class="alertErrorSmall" style="display: none;">
                            <%=CurrentSource.getSysLangValue("reqRequiredAcceptPrivacy")%></span>
                    </div>
                    <div class="lineBooking" id="chk_termsOfService_cont">
                        <input type="checkbox" id="chk_termsOfService" class="check" />
                        <a data-role="button" data-rel="modalview" href="#modalview-terms" class="linkTerms"><%=CurrentSource.getSysLangValue("lblAcceptTermsAndConditions")%></a>
                        <span id="chk_termsOfService_check" class="alertErrorSmall" style="display: none;">
                            <%=CurrentSource.getSysLangValue("reqRequiredAcceptTermsOfService")%></span>
                    </div>
                    <asp:LinkButton ID="lnk_payNow" CssClass="btnBigMobile" runat="server" OnClientClick="return RNT_validateBook()" OnClick="lnk_payNow_Click"><%= CurrentSource.getSysLangValue("reqBookNow")%></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </asp:PlaceHolder>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="PH_pwdRecover" runat="server">
            <table cellspacing="0" cellpadding="0" class="form bookingClient">
                <tr>
                    <td colspan="2" valign="middle" align="left" class="label" style="padding-top: 10px;">
                        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnk_goNewClient_Click" CssClass="user newclient"><%=contUtils.getLabel("reqGoNewClient") %></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left" class="label">User Name or Email
                    </td>
                    <td valign="middle" align="left" class="input">
                        <asp:TextBox ID="txt_pwdRecover" runat="server" Style=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left" class="label" colspan="2">
                        <asp:LinkButton ID="lnk_pwdRevover" CssClass="btnBig" runat="server" OnClientClick="return true" OnClick="lnk_pwdRevover_Click"><span>Send</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
        <asp:Label ID="lbl_errorAlert" CssClass="error_form_book" runat="server" Visible="false" Style="padding: 10px; background-color: rgb(255, 0, 0); color: rgb(255, 255, 255); font-weight: bold; float: left;"></asp:Label>
        <div class="nulla">
        </div>


    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_main_bottom" runat="server">
</asp:Content>
