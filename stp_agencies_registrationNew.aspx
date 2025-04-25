<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_agencies_registrationNew.aspx.cs" Inherits="RentalInRome.stp_agencies_registrationNew" %>

<%@ Register Src="/ucMain/UC_apt_in_rome_bottom.ascx" TagName="uc_Zones" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title><%= currStp.meta_title %></title>
    <meta name="description" content="<%=currStp.meta_description %>" />
    <meta name="keywords" content="<%=currStp.meta_keywords %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <div class="content">
        <asp:HiddenField ID="HF_unique" runat="server" />
        <div class="container bookingFormContainer stpContainer">
            <div class="row">

                <!-- BEGIN BOOKING FORM -->
                <div class="sidebar col-sm-4 bookingFormDetCont">
                    <div class="col-sm-12">
                        <img src="/images/area_agenti.jpg" class="imgSidebar" alt="agency registration" />
                        <br />
                        <img src="/images/banner-commissioni_2.gif" class="imgSidebar" alt="your commission" />
                        <br />
                        <br />
                        <p class="center">
                            <a class="btn btn-fullcolor btn-lg " href="/images/brochure/rentalinrome_agency_eng.pdf"><i class="fa fa-file-pdf-o"></i>Agency area step by step</a>
                            <br />
                            <br />
                        </p>
                    </div>
                </div>
                <!-- END BOOKING FORM -->

                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">

                    <h1 class="section-title"><%= currStp.title %> </h1>

                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp mainGuestbook mainAgencyRegistration">

                        <!-- BEGIN GUESTBOOK -->

                        <div class="comments guestbookCont agencyRegistrationCont">

                            <div class="col-sm-12">
                                <%= currStp.description %>
                                <%-- <h3>Please register and take an advantage of the exclusive rates
                                    <br />
                                    applied on our apartments </h3>
                                <br />
                                <p>
                                    Please register as an agency and gain access to your personal area in order to obtain <strong style="color: #333366;">from 5% to 10% discount</strong> on the rates for our apartment's rent.<br />
                                    Thanks to Rental in Rome's team, you will be able to check on your own the availability of the apartments and book them directly online from your personal area.
                                    <br />
                                    <br />
                                    Each apartment has been checked and licensed by Rental in Rome in order to guarantee a professional service and an excellent stay.
                                </p>--%>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="comments-form">
                                <ContentTemplate>

                                    <div id="pnl_request_sent" class="form-style" runat="server" visible="false">
                                        <%=CurrentSource.getSysLangValue("reqRequestSent")%>
                                    </div>

                                    <div class="form-style" id="pnl_request_cont" runat="server">

                                        <div id="errorLi" class="alert alert-danger" style="display: none;">
                                            <h3 id="errorMsgLbl">
                                                <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
                                            <p id="errorMsg">
                                                <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
                                            </p>
                                        </div>

                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txt_nameCompany" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span id="val_nameCompany" class="alertErrorSmall" style="float: none; display: none;"><%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                            <%-- <input type="text" name="AgReg_Contracting_party" placeholder="Contracting party*" class="form-control" />--%>
                                        </div>

                                        <div class="col-sm-2">
                                            <asp:DropDownList ID="drp_honorific" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txt_nameFull" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span id="val_nameFull" class="alertErrorSmall" style="float: none; display: none;"><%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>

                                        <div class="nulla">
                                        </div>

                                        <div class="col-sm-12">
                                            <asp:TextBox ID="txt_locAddress" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span id="val_locAddress" class="alertErrorSmall" style="float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>

                                        <div class="nulla">
                                        </div>

                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txt_locCity" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span id="val_locCity" class="alertErrorSmall" style="float: none; display: none;"></span>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txt_locState" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span id="val_locState" class="alertErrorSmall" style="float: none; display: none;"></span>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txt_locZipCode" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span id="val_locZipCode" class="alertErrorSmall" style="float: none; display: none;"></span>
                                        </div>

                                        <div class="nulla">
                                        </div>

                                        <div class="col-sm-6" id="drp_country_cont">
                                            <asp:DropDownList runat="server" ID="drp_country" CssClass="form-control" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                                <WhereParameters>
                                                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                                </WhereParameters>
                                            </asp:LinqDataSource>
                                            <span id="drp_country_check" class="alertErrorSmall" style="width: 217px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></span>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:DropDownList runat="server" ID="drp_pidLang" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="nulla">
                                        </div>

                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txt_email" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span id="val_contactEmail" class="alertErrorSmall" style="float: none; display: none;"></span>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txt_email_conf" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span id="val_contactEmail_conf" class="alertErrorSmall" style="float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqEmailConfirmError")%></span>
                                        </div>

                                        <div class="nulla">
                                        </div>

                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txt_contactPhone" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txt_contactFax" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="nulla">
                                        </div>

                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txt_docVat" runat="server" class="form-control"></asp:TextBox>
                                            <span id="val_docVat" class="alertErrorSmall" style="width: 402px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>
                                        <div class="checkbox col-md-6">
                                            <label>
                                                <input type="checkbox" id="chk_docVat_isEuReg" runat="server" />
                                                <%=CurrentSource.getSysLangValue("formEuRegVatNumber")%>
                                            </label>
                                        </div>

                                        <div class="nulla">
                                        </div>

                                        <div class="col-sm-12">
                                            <asp:TextBox ID="txt_contactWebSite" runat="server" CssClass="form-control" onfocus="if(this.value==''){this.value='http://www.'}" onblur="if(this.value=='http://www.'){this.value=''}"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="drp_contactComeFrom" CssClass="form-control" runat="server" onchange="div_contactComeFrom_swap();">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="Google" Text="Google"></asp:ListItem>
                                                    <asp:ListItem Value="Newsletter" Text="Newsletter"></asp:ListItem>
                                                    <asp:ListItem Value="SocialNetwork" Text="SocialNetwork"></asp:ListItem>
                                                    <asp:ListItem Value="Suggerito da amici/conoscenti" Text="Suggerito da amici/conoscenti"></asp:ListItem>
                                                    <asp:ListItem Value="other" Text="Other..."></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6" id="div_contactComeFrom" style="display: none;">
                                                <asp:TextBox ID="txt_contactComeFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                <script type="text/javascript">
                                                    function div_contactComeFrom_swap() {
                                                        $('#div_contactComeFrom').css('display', ($('#<%=drp_contactComeFrom.ClientID%>').val() == 'other' ? '' : 'none'))
                                                    }
                                                    $(function () { div_contactComeFrom_swap(); });
                                                </script>
                                            </div>
                                        </div>

                                        <div class="nulla">
                                        </div>

                                        <div class="col-sm-12">
                                            <textarea id="txt_note" runat="server" rows="10" cols="50" class="form-control"></textarea>
                                        </div>

                                        <div class="nulla">
                                        </div>

                                        <div class="col-sm-12 login complete-booking">

                                            <div class="form-style" style="margin: 0;">
                                                <div class="checkbox" id="cont_termsOfService">
                                                    <label>
                                                        <input type="checkbox" id="chk_termsOfService" />
                                                        <%-- I have read the <a href="#">"Privacy Policy"</a> and authorize the use of my personal data in compliance with Legislative Decree 196/03*--%>
                                                        <%=CurrentSource.getSysLangValue("lblTermsAndConditions")%> <a href="<%= CurrentSource.getPagePath("19","stp",CurrentLang.ID+"") %>" target="_blank">"  <%= CurrentSource.getPageName("19","stp",CurrentLang.ID+"") %> "</a>
                                                    </label>
                                                </div>
                                                <span id="val_termsOfService" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                                    <%=CurrentSource.getSysLangValue("reqRequiredAcceptTermsOfService")%></span>
                                            </div>

                                            <asp:LinkButton ID="lnk_send" CssClass="btn btn-fullcolor btn-lg" runat="server" OnClick="lnk_send_Click" OnClientClick="return validateRequestForm();"> <i class="fa fa-check"></i> <%= contUtils.getLabel("lblSendRequest") %></> </asp:LinkButton>
                                            <%--<button type="submit" class="btn btn-fullcolor btn-lg "><i class="fa fa-check"></i>Send request</button>--%>
                                        </div>

                                        <div class="nulla">
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="nulla">
                            </div>

                        </div>


                        <!-- BEGIN ZONES SECTION -->
                        <div class="main col-sm-12">
                            <%--<h3 class="section-title" data-animation-direction="from-bottom" data-animation-delay="50" style="text-align: center;">Apartments in Rome</h3>--%>
                            <uc1:uc_Zones ID="Zones" runat="server" />
                        </div>
                        <!-- END ZONES SECTION -->

                    </div>


                </div>
                <!-- End BEGIN MAIN CONTENT -->


            </div>
        </div>
    </div>

    <script type="text/javascript">
        function validateRequestForm() {
            var _validate = true;
            var _lastFocus = "";
            $("#errorLi").hide();

            $("#val_contactEmail").hide();
            $("#<%= txt_email.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_email.ClientID%>").val()) == "") {
                $("#<%= txt_email.ClientID%>").addClass(FORM_errorClass);
                $("#val_contactEmail").html('This field is required. Please enter a value.');
                $("#val_contactEmail").css("display", "block");
                _validate = false;
            }
            else if (!FORM_validateEmail($("#<%= txt_email.ClientID%>").val())) {
                $("#<%= txt_email.ClientID%>").addClass(FORM_errorClass);
                $("#val_contactEmail").html('Please enter a valid email address.');
                $("#val_contactEmail").css("display", "block");
                _validate = false;
            }
        if ($.trim($("#<%= txt_email_conf.ClientID%>").val()) != $.trim($("#<%= txt_email_conf.ClientID%> ").val())) {
                $("#<%= txt_email_conf.ClientID%>").addClass(FORM_errorClass);
            $("#val_contactEmail_conf").css("display", "block");
            _validate = false;
        }

        $("#val_nameFull").hide();
        $("#<%= txt_nameFull.ClientID%>").removeClass(FORM_errorClass);
        if ($.trim($("#<%= txt_nameFull.ClientID%>").val()) == "") {
                $("#<%= txt_nameFull.ClientID%>").addClass(FORM_errorClass);
            $("#val_nameFull").css("display", "block");
            _validate = false;
        }
        $("#val_nameCompany").hide();
        $("#<%= txt_nameCompany.ClientID%>").removeClass(FORM_errorClass);
        if ($.trim($("#<%= txt_nameCompany.ClientID%>").val()) == "") {
                $("#<%= txt_nameCompany.ClientID%>").addClass(FORM_errorClass);
            $("#val_nameCompany").css("display", "block");
            _validate = false;
        }

        $("#val_locAddress").hide();
        $("#<%= txt_locAddress.ClientID%>").removeClass(FORM_errorClass);
        if ($.trim($("#<%= txt_locAddress.ClientID%>").val()) == "") {
                $("#<%= txt_locAddress.ClientID%>").addClass(FORM_errorClass);
            $("#val_locAddress").css("display", "block");
            if (_validate)
                _lastFocus = "#<%= txt_locAddress.ClientID%>";
            _validate = false;
        }

        $("#val_locCity").hide();
        $("#<%= txt_locCity.ClientID%>").removeClass(FORM_errorClass);
        if ($.trim($("#<%= txt_locCity.ClientID%>").val()) == "") {
                $("#<%= txt_locCity.ClientID%>").addClass(FORM_errorClass);
            $("#val_locCity").css("display", "block");
            if (_validate)
                _lastFocus = "#<%= txt_locCity.ClientID%>";
                _validate = false;
            }

            $("#val_locState").hide();
            $("#<%= txt_locState.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_locState.ClientID%>").val()) == "") {
                $("#<%= txt_locState.ClientID%>").addClass(FORM_errorClass);
                $("#val_locState").css("display", "block");
                if (_validate)
                    _lastFocus = "#<%= txt_locState.ClientID%>";
                _validate = false;
            }

            $("#val_locZipCode").hide();
            $("#<%= txt_locZipCode.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_locZipCode.ClientID%>").val()) == "") {
                $("#<%= txt_locZipCode.ClientID%>").addClass(FORM_errorClass);
                $("#val_locZipCode").css("display", "block");
                if (_validate)
                    _lastFocus = "#<%= txt_locZipCode.ClientID%>";
                _validate = false;
            }

            $("#drp_country_check").hide();
            $("#<%= drp_country.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= drp_country.ClientID%>").val()) == "") {
                $("#<%= drp_country.ClientID%>").addClass(FORM_errorClass);
                $("#drp_country_check").css("display", "block");
                _validate = false;
            }

            $("#val_docVat").hide();
            $("#<%= txt_docVat.ClientID%>").removeClass(FORM_errorClass);
            if ($("#<%= chk_docVat_isEuReg.ClientID%>").is(':checked')) {
                if ($("#<%= txt_docVat.ClientID%>").length != 0 && $.trim($("#<%= txt_docVat.ClientID%>").val()) == "") {
                    $("#<%= txt_docVat.ClientID%>").addClass(FORM_errorClass);
                    $("#val_docVat").css("display", "block");
                    _validate = false;
                }
            }

            //$("#val_privacyAgree").css("display", "none");
            //$("#cont_privacyAgree").removeClass(FORM_errorClass);
            //if (!$("#chk_privacyAgree").is(':checked')) {
            //    $("#cont_privacyAgree").addClass(FORM_errorClass);
            //    $("#val_privacyAgree").css("display", "block");
            //    _validate = false;
            //}

            $("#val_termsOfService").css("display", "none");
            $("#cont_termsOfService").removeClass(FORM_errorClass);
            if (!$("#chk_termsOfService").is(':checked')) {
                $("#cont_termsOfService").addClass(FORM_errorClass);
                $("#val_termsOfService").css("display", "block");
                _validate = false;
            }

            if (!_validate) {
                $("#errorLi").css("display", "block");
                $.scrollTo($("#errorLi"), 500);
                //$.scrollTo($(_lastFocus), 500);
            }
            return _validate;
        }

    </script>
</asp:Content>
