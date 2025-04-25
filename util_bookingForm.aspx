<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="util_bookingForm.aspx.cs" Inherits="RentalInRome.util_bookingForm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="ucMain/ucBooking.ascx" TagName="ucBooking" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <style>
        #wrapper {
            z-index: auto;
        }

        .tooltip-inner {
            white-space: normal !important;
        }

        .bookingFormContainer .box_client_booking {
            text-align: center;
        }

            .bookingFormContainer .box_client_booking > strong {
                color: #333366;
            }

        /*#siteseal {
            bottom: -50px;
            float: left;
            left: 15px;
            position: absolute;
            width: 30%;
        }*/

        #siteseal {
           float:left;
           margin-top: 20px;
            }
    </style>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <script>
        fbq('track', 'Purchase');
</script>
    <div class="content">
        <div class="container bookingFormContainer">
            <div class="row">
                <!-- BEGIN BOOKING FORM -->
                <div class="sidebar col-sm-4 bookingFormDetCont">
                    <uc1:ucBooking ID="ucBooking" runat="server" />                                     
                </div>
                <!-- END BOOKING FORM -->
               
                

                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">

                    <%-- <h1 class="section-title">Complete Your Booking</h1>--%>

                    <%--   <div  id="pnlErrorMsg">
                          
                        </div>

                    <div class="form-group">
                        <div class="col-md-6">
                            <asp:TextBox ID="txt_email" runat="server" class="form-control" placeholder="Email"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <asp:TextBox ID="txt_email_conf" runat="server" class="form-control" placeholder="Confirm Email"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="drp_honorific" runat="server">
                            </asp:DropDownList>
                         
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txt_name_first" runat="server" class="form-control" placeholder="Name"></asp:TextBox>                          
                        </div>
                        <div class="col-md-6">
                            <asp:TextBox ID="txt_name_last" runat="server" class="form-control" placeholder="Surname"></asp:TextBox>                          
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-6">
                            <asp:TextBox ID="txt_phone_mobile" runat="server" class="form-control" placeholder="Phone"></asp:TextBox>
                           
                        </div>
                        <div class="col-md-6">
                            <asp:DropDownList runat="server" ID="drp_country" data-placeholder="Country">
                            </asp:DropDownList>
                          
                        </div>
                    </div>

                    <div class="nulla">
                    </div>

                    <div class="row regUser" style="text-align: center;margin-top:50px;" >

                        <a class="btn btn-default-color" href="#">
                            <i class="fa fa-user"></i>Already registered? CLick here
                        </a>
                    </div>--%>

                    <h2 class="section-title">Review Your order</h2>

                    <!-- order Detail -->

                    <div class="grid-style1 clearfix">
                        <div class="item col-md-12">
                            <!-- Set width to 4 columns for grid view mode only -->
                            <div class="image">
                                <a href="/<%=currEstateLN.page_path %>" target="_blank">
                                    <h3><%=currEstateLN.title %>
                                        <br />
                                        <small><%= CurrentSource.locZone_title(currEstateTB.pid_zone.objToInt32(),App.LangID,"")%></small></h3>
                                    <span class="location">See details page</span>
                                </a>
                                <img src="/<%=currEstateTB.img_banner %>" alt="">
                            </div>
                            <!--<div class="price">
									<i class="fa fa-home"></i>For Sale
									<span>€950,000</span>
								</div>-->
                            <ul class="amenities">
                                <li><i class="fa fa-users"></i><%= contUtils.getLabel("lblUpTo",1,"")+" " +currEstateTB.num_persons_max+" "+ contUtils.getLabel("lblPersons")%></li>
                                <li><i class="icon-bedrooms"></i><%=currEstateTB.num_rooms_bed %></li>
                                <li><i class="icon-bathrooms"></i><%=currEstateTB.num_rooms_bath %></li>
                            </ul>
                        </div>
                        <div class="col-md-12">
                            <ul class="categories">
                                <li>Apartment Reference:	<span class="pull-right"><%=currEstateLN.title %></span></li>
                                <li><%= CurrentSource.getSysLangValue("reqCheckInDate")%>:	<span class="pull-right"><%= currResTmp.dtStart.formatCustom("#dd#/#mm#/#yy#", App.LangID, "- - -")%></span></li>
                                <li><%= CurrentSource.getSysLangValue("reqCheckOutDate")%>:	<span class="pull-right"><%= currResTmp.dtEnd.formatCustom("#dd#/#mm#/#yy#", App.LangID, "- - -")%></span></li>
                                <li><%= CurrentSource.getSysLangValue("lblTotalNights")%>:	<span class="pull-right"><%= (currResTmp.dtEnd.Value - currResTmp.dtStart.Value).TotalDays.objToInt32() %> Nights</span></li>
                                <li><%= CurrentSource.getSysLangValue("lblPax")%>:	<span class="pull-right"><%= currEstateTB.is_chidren_allowed == 1 ? currResTmp.numPers_adult + " Adults, " + currResTmp.numPers_childOver + " Children, " + currResTmp.numPers_childMin + " Infants" : currResTmp.numPers_adult + " Adults"%></span></li>

                                <li><%= CurrentSource.getSysLangValue("lblDamageDeposit") %>: <span class="pull-right"><%= currResTmp.pr_deposit.objToDecimal().ToString("N2")+" €"%></span></li>

                                <li><%=CurrentSource.getSysLangValue("lblPrice")%>: <span class="pull-right"><%=  currResTmp.prTotalRate.objToDecimal().ToString("N2")+" €"%></span></li>

                                <%if (currResTmp.pr_discount_commission > 0)
                                  {%>
                                <li class="promoCodeLbl"><%=CurrentSource.getSysLangValue("lblPromoDiscount")%>: <span class="pull-right newPrice promoCodePrice"><%= "- " +  currResTmp.pr_discount_commission.objToDecimal().ToString("N2")+" €"%></span></li>
                                <%} %>

                                <%if (currResTmp.prDiscountSpecialOffer > 0)
                                  {%>
                                <li><%=CurrentSource.getSysLangValue("lblDiscountSpecialOffer")%>: <span class="pull-right"><%="- " +  currResTmp.prDiscountSpecialOffer.objToDecimal().ToString("N2")+" €"%></span></li>
                                <%} %>

                                <%if (currResTmp.prDiscountLongStay > 0)
                                  {%>
                                <li><%=CurrentSource.getSysLangValue("lblDiscountLongStay")%>:<a class="infoBtn ico_tooltip" title="<%= currResTmp.prDiscountLongStayDesc%>" style="float: right; margin-right: 4px;"></a> <span class="pull-right"><%="- " +  currResTmp.prDiscountLongStay.objToDecimal().ToString("N2")+" €"%></span></li>
                                <%} %>

                                <%if (currResTmp.pr_srsPrice > 0)
                                  {%>
                                <li><%=CurrentSource.getSysLangValue("lblWelcomeService")%>:<span class="pull-right"><%=currResTmp.pr_srsPrice.objToDecimal().ToString("N2")+" €"%></span></li>
                                <%} %>

                                <%if (currResTmp.pr_ecoPrice > 0)
                                  {%>
                                <li>Cleaning Service X<%= currResTmp.pr_ecoPrice%>:<span class="pull-right"><%=  currResTmp.pr_ecoPrice.objToDecimal().ToString("N2")+" €"%></span></li>
                                <%} %>

                                <%if (currResTmp.agentCommissionPrice > 0)
                                  {%>
                                <li><%=CurrentSource.getSysLangValue("Agency Discount")%>:<span class="pull-right"><%=  currResTmp.agentCommissionPrice.objToDecimal().ToString("N2")+" €" + (currResTmp.agentCommissionNotInTotal==0?"Compreso nel totale":"Scalato dal totale")%></span></li>
                                <%} %>
                                <%if (currResTmp.pr_part_agency_fee > 0)
                                  {%>
                                <li><%=CurrentSource.getSysLangValue("rnt_pr_part_commission")%>: <span class="pull-right"><%=currResTmp.pr_part_commission_total.objToDecimal().ToString("N2")+" €" %></span></li>


                                <li><%=CurrentSource.getSysLangValue("rnt_pr_part_agency_fee")%>: <span class="pull-right"><%= currResTmp.pr_part_agency_fee.objToDecimal().ToString("N2")+" €"%></span></li>
                                <%} %>
                                <li><strong><%= CurrentSource.getSysLangValue("pdf_TOTALE")+" "+ CurrentSource.getSysLangValue("lblAnticipo")%>:	<span class="pull-right"><%=currResTmp.pr_part_payment_total.objToDecimal().ToString("N2")+" €" %></span></strong></li>
                                <li><strong><%= CurrentSource.getSysLangValue("lblSaldoArrivo")%>  :	<span class="pull-right"><%=currResTmp.pr_part_owner.objToDecimal().ToString("N2")+" €" %></span></strong></li>


                                <%if (currResTmp.agentID != 0 && currResTmp.agentCommissionPrice != 0)
                                  {%>
                                <li class="assigned"><%=CurrentSource.getSysLangValue("lblYourCommission")%>:<span class="pull-right"><%= currResTmp.agentCommissionPrice.objToDecimal().ToString("N2")+" €"%></span></li>
                                <% if (currResTmp.pr_part_forPayment != currResTmp.pr_part_payment_total)
                                   { %>
                                <li class="assigned"><%=CurrentSource.getSysLangValue("lblPayNow")%>:<span class="pull-right"><%= currResTmp.pr_part_forPayment.objToDecimal().ToString("N2")+" €"%></span></li>
                                <%}
                                  }%>

                                <li class="assigned"><%= CurrentSource.getSysLangValue("lblTotalPrice")%>:	<span class="pull-right"><%=currResTmp.pr_total.objToDecimal().ToString("N2")+" €" %> </span></li>
                            </ul>
                        </div>
                    </div>

                    <!-- end order details -->

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="box_client_booking" id="pnl_sendBooking" runat="server">
                                <asp:HiddenField ID="HF_mode" runat="server" Value="new" />
                                <asp:HiddenField ID="HF_IdClient" runat="server" Value="0" />
                                <h1 class="section-title">
                                    <%= CurrentSource.getSysLangValue("lblFinishYourReservations")%></h1>

                                <asp:PlaceHolder ID="PH_bookingForm" runat="server">
                                    <asp:PlaceHolder ID="PH_viewClient" runat="server">
                                        <div class="nulla">
                                        </div>
                                        <strong style="font-size: 18px;">
                                            <%= CurrentSource.getSysLangValue("lblWelcome")%>&nbsp;<asp:Literal ID="ltr_honorific" runat="server"></asp:Literal>&nbsp;<asp:Literal ID="ltr_name_full" runat="server"></asp:Literal></strong>
                                        <br />
                                        <asp:Literal ID="ltr_country" runat="server"></asp:Literal>
                                        <br />
                                        <asp:Literal ID="ltr_phone_mobile" runat="server"></asp:Literal>
                                        <br />
                                        <asp:Literal ID="ltr_email" runat="server"></asp:Literal>
                                        <br />
                                        <div class="nulla">
                                        </div>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PH_newClient" runat="server">
                                        <%-- <asp:LinkButton ID="lnk_goOldClient" runat="server" OnClick="lnk_goOldClient_Click" CssClass="newclient"><span><%=CurrentSource.getSysLangValue("reqGoRegisteredClient") %></span></asp:LinkButton>--%>
                                        <div class="nulla">
                                        </div>
                                        <div id="pnlErrorMsg">
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txt_email" runat="server" class="form-control" placeholder="Email"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txt_email_conf" runat="server" class="form-control" placeholder="Confirm Email"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:DropDownList ID="drp_honorific" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txt_name_first" runat="server" class="form-control" placeholder="Name"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txt_name_last" runat="server" class="form-control" placeholder="Surname"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txt_phone_mobile" runat="server" class="form-control" placeholder="Phone"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:DropDownList runat="server" ID="drp_country" data-placeholder="Country">
                                                </asp:DropDownList>
                                            </div>
                                        </div>


                                        <div class="nulla">
                                        </div>
                                        <div class="row regUser" style="text-align: center; margin-top: 50px;">
                                            <asp:LinkButton ID="lnk_goOldClient" runat="server" OnClick="lnk_goOldClient_Click" CssClass="btn btn-default-color"><i class="fa fa-user"></i><%=CurrentSource.getSysLangValue("reqGoRegisteredClient") %></asp:LinkButton>
                                            <%--<a class="btn btn-default-color" href="#">
                                        <i class="fa fa-user"></i>Already registered? CLick here
                                    </a>--%>
                                        </div>
                                    </asp:PlaceHolder>
                                    <%-- <asp:LinkButton ID="lnk_payNow" CssClass="btn cartebig" runat="server" OnClientClick="return RNT_validateBook()" OnClick="lnk_payNow_Click"><span><%=CurrentSource.getSysLangValue("reqPayWith")%></span></asp:LinkButton>--%>


                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="PH_oldClient" runat="server">
                                    <div class="text-center col-md-12" style="margin: 10px 0;">
                                        <asp:LinkButton ID="lnk_goNewClient" runat="server" OnClick="lnk_goNewClient_Click" CssClass="btn btn-default-color">
                                        <i class="fa fa-user"></i><%=CurrentSource.getSysLangValue("reqGoNewClient") %>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                    <div class="form-group" style="margin-top: 20px;">
                                        <div class="col-md-6">
                                            <label class="desc">User Name* </label>
                                            <div class="nulla">
                                            </div>

                                            <asp:TextBox ID="txt_old_email" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>
                                        <div class="col-md-6">
                                            <label class="desc">
                                                <%=CurrentSource.getSysLangValue("reqPassword")%>*
                                            </label>
                                            <div class="nulla">
                                            </div>
                                            <asp:TextBox ID="txt_old_password" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>

                                        </div>
                                        <div class="nulla">
                                        </div>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                    <div class="text-center col-md-12" style="margin: 10px 0;">
                                        <asp:LinkButton ID="lnk_login" CssClass="btn btn-default-color" runat="server" OnClientClick="return ValidateOldClient();" OnClick="lnk_login_Click"><span>Login</span></asp:LinkButton>
                                        <div class="alert alert-danger" id="chk_login" style="display: none">Please Login First</div>
                                        <div class="nulla">
                                        </div>
                                        <asp:LinkButton ID="lnk_goPwd" CssClass="" runat="server" OnClientClick="return true" OnClick="lnk_goPwd_Click" Style="display: block; margin: 10px 0 0 0;"><span><%=CurrentSource.getSysLangValue("formPasswordRecovery")%></span></asp:LinkButton>
                                    </div>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="PH_pwdRecover" runat="server">
                                    <div class="text-center col-md-12" style="margin: 10px 0;">
                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnk_goNewClient_Click" CssClass="btn btn-default-color">
                                             <i class="fa fa-user"></i><%=CurrentSource.getSysLangValue("reqGoNewClient") %>

                                        </asp:LinkButton>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <label class="desc">User Name or Email </label>
                                            <div class="nulla">
                                            </div>
                                            <asp:TextBox ID="txt_pwdRecover" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>
                                        <div class="nulla">
                                        </div>
                                    </div>
                                    <div class="text-center col-md-12" style="margin: 10px 0;">
                                        <asp:LinkButton ID="lnk_pwdRevover" CssClass="btn btn-default-color" runat="server" OnClientClick="return true" OnClick="lnk_pwdRevover_Click"><span>Send</span></asp:LinkButton>
                                    </div>
                                </asp:PlaceHolder>
                                <asp:Label ID="lbl_errorAlert" CssClass="error_form_book" runat="server" Visible="false"></asp:Label>
                                <div class="nulla">
                                </div>
                            </div>

                            <!-- Instant Booking -->
                            <h2 class="section-title">Book now!</h2>
                            <!-- end -->

                            <div class="col-sm-12 login complete-booking">
                                <div class="form-style" id="pnl_termsAndPrivacy" runat="server">
                                    <div class="checkbox" id="pnl_privacyAgree">
                                        <label>
                                            <input type="checkbox" id="chk_privacyAgree"><a target="_blank" href="<%=CurrentSource.getPagePath("2","stp", App.LangID+"") %>"> <%=CurrentSource.getSysLangValue("lblAcceptPersonalDataPrivacy")%>*</a>
                                            <%--<input type="checkbox" name="terms">--%>
                                            <%--  I have read the <a href="#">"Privacy Policy"</a> and authorize the use of my personal data in compliance with Legislative Decree 196/03*--%>
                                        </label>
                                    </div>
                                    <div class="checkbox" id="pnl_termsOfService">
                                        <label>

                                            <input type="checkbox" id="chk_termsOfService">
                                            <a target="_blank" href="<%=CurrentSource.getPagePath("19","stp", App.LangID+"") %>">
                                                <%= contUtils.getLabel("lblAcceptTermsAndConditions")%></a>

                                            <%--   <input type="checkbox" name="terms">  <a target="_blank" href="<%=CurrentSource.getPagePath("2","stp", App.LangID+"") %>">--%>
                                            <%-- I have read and accept the <a href="#">"Terms and Conditions"</a>--%>
                                        </label>
                                    </div>
                                </div>
                                <!--<div>
                                <label>
                                    <input type="radio" name="privacyAccept" />
                                        I have read and accept the <a href="#">"Terms and Conditions"</a></label>
                                <br /><br />

                            </div>-->

                                
                                <asp:LinkButton ID="lnkBookNow" runat="server" CssClass="btn btn-fullcolor btn-lg" OnClick="lnkBookNow_Click" OnClientClick="return bookingForm_validateForm();"><i class="fa fa-check"></i>Complete Your Booking</asp:LinkButton>
                                <%-- <button type="submit" class="btn btn-fullcolor btn-lg "><i class="fa fa-check"></i>Complete Your Booking</button>--%>

                                <div class="nulla">
                                </div>

                                <span id="siteseal"><script async type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=KQsv4yBM0BqPJHlzehZDvQMMOuVk7bQKurfWOSZRP9jcl3REyDbv0elwOgp0"></script></span>

                            </div>
                            <!-- end end -->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!-- End BEGIN MAIN CONTENT -->


            </div>
        </div>
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var ValidateOldClient_firstTime = true;
            function ValidateOldClient() {
                var isold_Valid = true;
                var callBack_old = null;

                FORM_hideErrorToolTip();

                if (ValidateOldClient_firstTime) {
                    callBack_old = function () { return ValidateOldClient(); };
                    ValidateOldClient_firstTime = false;
                }
                if (!FORM_validate_requiredField("<%= txt_old_email.ClientID%>", "", "", "", callBack_old))
                    isold_Valid = false;

                if (!FORM_validate_requiredField("<%= txt_old_password.ClientID%>", "", "", "", callBack_old))
                    isold_Valid = false;
                console.log("validate Old Client Required   " + isold_Valid);
                return isold_Valid;
            }


            var bookingForm_validateForm_firstTime = true;
            function bookingForm_validateForm() {
                var isValid = true;
                var mode = $("#<%=HF_mode.ClientID%>").val();
                if (mode == "new") {

                    var callBack = null;
                    if (bookingForm_validateForm_firstTime) {
                        callBack = function () { return bookingForm_validateForm(); };
                        bookingForm_validateForm_firstTime = false;
                    }

                    FORM_hideErrorToolTip();
                    $("#chk_login").hide();
                    var privacy_message = "<%=contUtils.getLabel("reqRequiredAcceptPrivacy")%>";
                    var terms_message = "<%=contUtils.getLabel("reqRequiredAcceptTermsOfService")%>";

                    if (!FORM_validate_checkBoxField("chk_termsOfService", "pnl_termsOfService", "", terms_message, callBack))
                        isValid = false;

                    if (!FORM_validate_checkBoxField("chk_privacyAgree", "pnl_privacyAgree", "", privacy_message, callBack))
                        isValid = false;

                    if (!FORM_validate_requiredField_chzn("<%= drp_country.ClientID%>", "", "", "", callBack))
                        isValid = false;

                    if (!FORM_validate_requiredField("<%= txt_phone_mobile.ClientID%>", "", "", "", callBack))
                        isValid = false;

                    if (!FORM_validate_requiredField("<%= txt_name_last.ClientID%>", "", "", "", callBack))
                        isValid = false;
                    if (!FORM_validate_requiredField("<%= txt_name_first.ClientID%>", "", "", "", callBack))
                        isValid = false;

                    if (!FORM_validate_requiredField("<%= txt_email.ClientID%>", "", "", "", callBack))
                        isValid = false;
                    else if (!FORM_validate_emailField("<%= txt_email.ClientID%>", "", "", "", callBack))
                        isValid = false;

                    
                if (!isValid) {
                    //$("#pnlErrorMsg").css("display", "block");
                    // $.scrollTo($("#pnlErrorMsg").offset().top, 300);
                }
                console.log(isValid);
            }
            else {
                console.log("client ID -" + $("#<%=HF_IdClient.ClientID %>").val());

                    if ($("#<%=HF_IdClient.ClientID %>").val() > 0) {
                        isValid = true;
                        $("#chk_login").hide();
                    }
                    else {
                        $("#chk_login").show();
                        isValid = false;
                    }
                }

                return isValid;
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
