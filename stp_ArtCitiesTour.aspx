<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_ArtCitiesTour.aspx.cs" Inherits="RentalInRome.stp_ArtCitiesTour" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_receiverEmail" runat="server" Value="booking@liontravel.it" />
    <asp:HiddenField ID="HF_receiverEmail_" runat="server" Value="magadesign.sviluppo.1@gmail.com" />
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <div id="testoMain">
        <%= ltr_title.Text != "" ? "<h3 class='underlined'>" + ltr_title.Text + "</h3>" : ""%>
        <div class="mainContent">
            <%=ltr_description.Text %>
            <div class="nulla">
            </div>
            <hr />
            <h3 class="underlined" style="margin-left: 0; margin-top: 5px;">
                <span style="line-height: 25px; font-size: 20px;"><%=CurrentSource.getSysLangValue("lblFormRichiesta").Replace("'","\\'")%></span>
            </h3>
            <div class="nulla">
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="pnl_request_sent" class="box_client_booking" runat="server" visible="false" style="width: 545px; ">
                        <%=CurrentSource.getSysLangValue("reqRequestSent")%>
                    </div>
                    <div id="pnl_request_cont" class="box_client_booking" runat="server" style="padding: 0; width: 520px; margin-bottom: 20px;">
                        <div id="errorLi" class="line" style="color: red; margin-bottom: 30px; width: 520px; display: none;">
                            <h3 id="errorMsgLbl">
                                <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
                            <p id="errorMsg">
                                <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
                            </p>
                            <div class="nulla">
                            </div>
                        </div>
                        <table class="priceTable bookArtCitiesTour" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="middle" align="left">
                                    <label><%=CurrentSource.getSysLangValue("lblName").Replace("'","\\'")%></label>
                                    <asp:TextBox ID="txt_name_first" runat="server"></asp:TextBox>
                                    <span id="txt_name_first_check" class="alertErrorSmall" style="float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </td>
                                <td valign="middle" align="left">
                                    <label><%=CurrentSource.getSysLangValue("lblSurname").Replace("'","\\'")%>	</label>
                                    <asp:TextBox ID="txt_name_last" runat="server"></asp:TextBox>
                                    <span id="txt_name_last_check" class="alertErrorSmall" style="float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </td>

                            </tr>
                            <tr>
                                <td valign="middle" align="left">
                                    <label><%=CurrentSource.getSysLangValue("reqEmail").Replace("'","\\'")%></label>
                                    <asp:TextBox ID="txt_email" runat="server"></asp:TextBox>
                                    <span id="txt_email_check" class="alertErrorSmall" style="float: none; display: none;"></span>
                                </td>
                                <td valign="middle" align="left">
                                    <label><%=CurrentSource.getSysLangValue("reqEmailConfirm").Replace("'","\\'")%></label>
                                    <asp:TextBox ID="txt_email_conf" runat="server"></asp:TextBox>
                                    <span id="txt_email_conf_check" class="alertErrorSmall" style="float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqEmailConfirmError")%></span>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left">
                                    <label><%=CurrentSource.getSysLangValue("lblPhone").Replace("'","\\'")%> </label>
                                    <asp:TextBox ID="txt_phone_mobile" runat="server"></asp:TextBox>
                                    <span id="txt_phone_mobile_check" class="alertErrorSmall" style="float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </td>
                                <td valign="middle" align="left">
                                    <asp:CheckBox ID="chkRequestRecall" runat="server" />
                                    <%=CurrentSource.getSysLangValue("mobileFormVuoiEssereRichiamato").Replace("'","\\'")%> 	
                                </td>
                            </tr>
                            <tr>

                                <td valign="middle" align="left">
                                    <label>Formula:</label>
                                    <asp:DropDownList ID="drpFormula" runat="server">
                                        <asp:ListItem Value="CLASSIC"></asp:ListItem>
                                        <asp:ListItem Value="JEANS"></asp:ListItem>
                                        <asp:ListItem Value="EASY ITALY"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td valign="middle" align="left">
                                    <label><%=CurrentSource.getSysLangValue("lblDay").Replace("'","\\'")%>:</label>

                                    <telerik:RadDatePicker ID="rdp_tourDay" runat="server">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left" colspan="2">
                                    <label class="desc">
                                        <%=CurrentSource.getSysLangValue("lblPrivacyPolicy")%>
                                        <asp:HyperLink ID="HL_getPdf_privacy" runat="server" Target="_blank">
                                    <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                                        </asp:HyperLink>
                                    </label>
                                    <div class="div_terms" style="width: 470px; margin-bottom:20px; background:#FFF;">
                                        <asp:Literal ID="ltr_privacy" runat="server"></asp:Literal>
                                    </div>
                                    <div class="accettocheck" style="height: 20px;" id="chk_privacyAgree_cont">
                                        <input type="checkbox" id="chk_privacyAgree" style="float:left; margin-right:10px;" />
                                        <label for="chk_privacyAgree" style="width: auto; clear: none; margin-top:2px;">
                                            <%=CurrentSource.getSysLangValue("lblAcceptPersonalDataPrivacy")%>*</label>
                                    </div>
                                    <span id="chk_privacyAgree_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredAcceptPrivacy")%></span>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left"></td>
                                <td valign="middle" align="left">
                                    <asp:LinkButton ID="lnk_send" CssClass="btn" runat="server" OnClick="lnk_send_Click" OnClientClick="return RNT_validateRequestForm()"><span style="text-align: center; width: 205px;"><%=CurrentSource.getSysLangValue("lblSendRequest").Replace("'","\\'")%></span></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="nulla">
            </div>
            <hr style="margin-top: 0;" />
            <em><%=ltr_sub_title.Text %></em>
        </div>
    </div>
    <div style="float: right; margin: 20px 20px 0 0;">
        <%= ltr_img_banner.Text != "" ? "<img src=\"/" + ltr_img_banner.Text + "\" alt=\"\" />" : ""%>
    </div>
    <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />
    <script type="text/javascript">
        function RNT_validateRequestForm() {
            var _validate = true;
            $("#errorLi").hide();
            $("#txt_email_check").hide();
            $("#<%= txt_email.ClientID%>").removeClass(FORM_errorClass);
            $("#txt_email_conf_check").hide();
            $("#<%= txt_email_conf.ClientID%>").removeClass(FORM_errorClass);
            $("#txt_name_last_check").hide();
            $("#<%= txt_name_last.ClientID%>").removeClass(FORM_errorClass);
            $("#txt_name_first_check").hide();
            $("#<%= txt_name_first.ClientID%>").removeClass(FORM_errorClass);
            $("#txt_phone_mobile_check").hide();
            $("#<%= txt_phone_mobile.ClientID%>").removeClass(FORM_errorClass);
            $("#chk_privacyAgree_check").hide();
            $("#chk_privacyAgree_cont").removeClass(FORM_errorClass);

            if ($.trim($("#<%= txt_email.ClientID%>").val()) == "") {
                $("#<%= txt_email.ClientID%>").addClass(FORM_errorClass);
                $("#txt_email_check").html('This field is required. Please enter a value.');
                $("#txt_email_check").css("display", "block");
                _validate = false;
            }
            else if (!FORM_validateEmail($("#<%= txt_email.ClientID%>").val())) {
                $("#<%= txt_email.ClientID%>").addClass(FORM_errorClass);
                $("#txt_email_check").html('Please enter a valid email address.');
                $("#txt_email_check").css("display", "block");
                _validate = false;
            }
        if ($.trim($("#<%= txt_email_conf.ClientID%>").val()) != $.trim($("#<%= txt_email.ClientID%> ").val())) {
                $("#<%= txt_email_conf.ClientID%>").addClass(FORM_errorClass);
            $("#txt_email_conf_check").css("display", "block");
            _validate = false;
        }
        if ($.trim($("#<%= txt_name_first.ClientID%>").val()) == "") {
                $("#<%= txt_name_first.ClientID%>").addClass(FORM_errorClass);
                $("#txt_name_first_check").css("display", "block");
                _validate = false;
            }
            if ($.trim($("#<%= txt_name_last.ClientID%>").val()) == "") {
                $("#<%= txt_name_last.ClientID%>").addClass(FORM_errorClass);
                $("#txt_name_last_check").css("display", "block");
                _validate = false;
            }
            if ($.trim($("#<%= txt_phone_mobile.ClientID%>").val()) == "") {
                $("#<%= txt_phone_mobile.ClientID%>").addClass(FORM_errorClass);
                $("#txt_phone_mobile_check").css("display", "block");
                _validate = false;
            }
            if (!$("#chk_privacyAgree").is(':checked')) {
                $("#chk_privacyAgree_cont").addClass(FORM_errorClass);
                $("#chk_privacyAgree_check").css("display", "block");
                _validate = false;
            }
            if (!_validate) {
                $("#errorLi").css("display", "block");
                $.scrollTo($("#errorLi"), 500);
            }
            return _validate;
        }
        $(function () {
            $("#<% =txt_email.ClientID %>,#<% =txt_email_conf.ClientID %>").bind("cut copy paste", function (event) {
                event.preventDefault();
            });
        });
    </script>
</asp:Content>
