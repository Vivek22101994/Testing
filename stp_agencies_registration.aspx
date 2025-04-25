<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_agencies_registration.aspx.cs" Inherits="RentalInRome.stp_agencies_registration" %>
<%@ Register Src="uc/UC_menu_breadcrumb.ascx" TagName="UC_menu_breadcrumb" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
<style type="text/css" >
#contatti .box_client_booking > div .line {
    padding-bottom: 0;
}
.box_client_booking .line {
    display: block;
    margin-bottom: 15px;
    margin-top:0;
}
.box_client_booking .line .left {
    margin-right: 50px;
}
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server"> 
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal> 
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_unique" Value="" runat="server" />
    <asp:HiddenField ID="HF_num_persons_max" runat="server" Value="0" />
    <uc1:UC_menu_breadcrumb ID="UC_menu_breadcrumb1" runat="server" />

    
    <div id="contatti">
        
        <div id="imgWedding">
            <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/area_agenti.jpg" alt="agencies registration" style=" float: left; margin:0 0 10px -25px;" />
            <div class="nulla"></div>
        </div>
        


        
        <div class="txtWedding" style="margin:0 0 15px 15px;">
        
        <span style="margin:0; float:left;">
            <%=ltr_description.Text%>
        </span>
        
        <img style="float:right; margin-bottom:10px;" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/banner-commissioni_<%= CurrentLang.ID %>.gif" alt="commissions" />

        <div class="nulla"></div>
        </div>
        <div class="nulla">
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <div class="box_client_booking" style="margin-left: 20px;">
                    <div class="fasciatitform" runat="server" visible="false">Richiesta di iscrizione Agenzia</div>

                    <div id="pnl_request_sent"  runat="server" visible="false">
                        <%=CurrentSource.getSysLangValue("reqRequestSent")%>
                    </div>
                    <div id="pnl_request_cont" runat="server">
                        <div id="errorLi" class="line" style="color: red; margin-bottom: 30px; width: 476px; display: none;">
                            <h3 id="errorMsgLbl">
                                <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
                            <p id="errorMsg">
                                <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
                            </p>
                            <div class="nulla">
                            </div>
                        </div>
             

                        <%--COMPANY_NAME
                        EMAIL
                        ADDRESS
                        CITY
                        STATE
                        ZIPCODE
                        PHONE_NUMBER
                        FAX_NUMBER
                        SIC_DESCRIPTION (tipologia- Travel agent, Travel Agencies & Bureaus)
                        WEB ADDRESS
                        CONTACTS--%>




                        <div class="line">
                            <div id="txt_email_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("formParteContraente")%>*<a style="float: none; display: inline-block; margin-left: 10px;" class="infoBtn ico_tooltip" ttpc="help_formParteContraente"></a>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_nameCompany" runat="server" Style="width: 300px;"></asp:TextBox>
                                    <span id="val_nameCompany" class="alertErrorSmall" style="width: 300px; float: none; display: none;"><%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                                <div style="display: none;" id="help_formParteContraente">
                                    <%=CurrentSource.getSysLangValue("formParteContraente_Desc")%>
                                </div>
                            </div>
                            <div id="Div6" class="left" style="margin-right:0;">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("formContactNameFull")%>*
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_honorific" runat="server" Style="margin-right: 10px; width: 60px;">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_nameFull" runat="server" Style="width: 254px;"></asp:TextBox>
                                    <span id="val_nameFull" class="alertErrorSmall" style="width: 254px; float: none; display: none;"><%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div id="txt_name_first_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblAddress")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_locAddress" runat="server" Style="width: 452px;"></asp:TextBox>
                                    <span id="val_locAddress" class="alertErrorSmall" style="width: 402px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div id="Div4" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblCity")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_locCity" runat="server" Style="width: 155px;"></asp:TextBox>
                                    <span id="val_locCity" class="alertErrorSmall" style="float: none; display: none;"></span>
                                </div>
                            </div>

                            <div id="Div5" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("locStateProvince")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_locState" runat="server" Style="width: 140px;"></asp:TextBox>
                                    <span id="val_locState" class="alertErrorSmall" style="float: none; display: none;"></span>
                                </div>
                            </div>
                            <div id="Div1" class="left" style="margin-right: 0;">
                                <label class="desc"><%=CurrentSource.getSysLangValue("lblZipCode")%>* </label>
                                <div>
                                    <asp:TextBox ID="txt_locZipCode" runat="server" Style="width: 120px;"></asp:TextBox>
                                    <span id="val_locZipCode" class="alertErrorSmall" style="float: none; display: none;"></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div id="drp_country_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqLocation")%>*
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" Style="width: 217px;" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                                    </asp:DropDownList>
                                    <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                        <WhereParameters>
                                            <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <span id="drp_country_check" class="alertErrorSmall" style="width: 217px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></span>
                                </div>
                            </div>
                            <div id="Div2" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("sysLanguage")%>* 
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="drp_pidLang">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                         <div class="line">
                            <div id="Div11" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqEmail")%>* </label>
                                <div>
                                    <asp:TextBox ID="txt_contactEmail" runat="server" Style="width: 300px;"></asp:TextBox>
                                    <span id="val_contactEmail" class="alertErrorSmall" style="width: 300px; float: none; display: none;"></span>
                                </div>
                            </div>
                            <div id="txt_email_conf_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqEmailConfirm")%>* </label>
                                <div>
                                    <asp:TextBox ID="txt_contactEmail_conf" runat="server" Style="width: 300px;"></asp:TextBox>
                                    <span id="val_contactEmail_conf" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqEmailConfirmError")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                           <div id="Div3" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblPhone")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_contactPhone" runat="server" Style="width: 300px;"></asp:TextBox>
                                </div>
                            </div>
                            <div id="Div7" class="left" style="margin-right:0;">
                                <label class="desc">
                                    Fax
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_contactFax" runat="server" Style="width: 300px;"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div id="Div12" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("formVatNumber")%>&nbsp;&nbsp;/&nbsp;&nbsp;
                                    <%=CurrentSource.getSysLangValue("formEuRegVatNumber")%>
                                    <input type="checkbox" id="chk_docVat_isEuReg" runat="server" style="width: auto;" />
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_docVat" runat="server" Style="width: 300px;"></asp:TextBox>
                                    <span id="val_docVat" class="alertErrorSmall" style="width: 402px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div id="Div8" class="left">
                                <label class="desc">
                                   <%=CurrentSource.getSysLangValue("lblWebAddress")%> <span style="font-size: 11px; font-style: italic;">(http://...)</span>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_contactWebSite" runat="server" Style="width: 452px;" onfocus="if(this.value==''){this.value='http://www.'}" onblur="if(this.value=='http://www.'){this.value=''}"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>


                        <div class="line">
                            <div class="left check_list">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblNotes")%>
                                </label>
                                <div>
                                    <textarea id="txt_note" runat="server" rows="10" cols="50" tabindex="27" style="height: 80px; width: 452px; border:1px solid #BCBCCF"></textarea>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>


                        <div class="line">
                            <div id="Div9" class="left" style="margin-right:0;">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("formWhereDidYouHearAboutUs")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_contactComeFrom" runat="server" onchange="div_contactComeFrom_swap();">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Google" Text="Google"></asp:ListItem>
                                        <asp:ListItem Value="Newsletter" Text="Newsletter"></asp:ListItem>
                                        <asp:ListItem Value="SocialNetwork" Text="SocialNetwork"></asp:ListItem>
                                        <asp:ListItem Value="Suggerito da amici/conoscenti" Text="Suggerito da amici/conoscenti"></asp:ListItem>
                                        <asp:ListItem Value="other" Text="Other..."></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div> 
                            <div class="left" id="div_contactComeFrom" style="margin: 16px 0 0 0; display: none;">
                                <label class="desc" style="margin:0 6px;">
                                    ...
                                </label>
                                <asp:TextBox ID="txt_contactComeFrom" runat="server" Style="width: 225px;"></asp:TextBox>
                                <script type="text/javascript">
                                    function div_contactComeFrom_swap() {
                                        $('#div_contactComeFrom').css('display', ($('#<%=drp_contactComeFrom.ClientID%>').val() == 'other' ? '' : 'none'))
                                    }
                                    $(function () { div_contactComeFrom_swap(); });
                                </script>
                            </div>                                
                            <div class="nulla"></div>
                        </div>




                        <div class="line2">
                            <div class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblPrivacyPolicy")%>
                                    <a target="_blank" href="<%= CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + (CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "stp_contentonly_download.aspx?id=2&lang=" + CurrentLang.ID).urlEncode() + "&filename=" + ("RiR_" + CurrentSource.getSysLangValue("lblPrivacyPolicy").clearPathName() + ".pdf").urlEncode() %>">
                                        <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                                    </a>
                                </label>
                                <div class="div_terms">
                                    <%= contUtils.getStp(2, CurrentLang.ID, "#description#")%>
                                </div>
                                <div class="accettocheck" style="height: 30px;" id="cont_privacyAgree">
                                    <input type="checkbox" id="chk_privacyAgree" />
                                    <label for="chk_privacyAgree">
                                        <%=CurrentSource.getSysLangValue("lblAcceptPersonalDataPrivacy")%>*</label>
                                </div>
                                <span id="val_privacyAgree" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqRequiredAcceptPrivacy")%></span>
                            </div>
                            <div class="left" runat="server" visible="false">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblTermsAndConditions")%>
                                    <a target="_blank" href="<%= CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + (CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "stp_contentonly_download.aspx?id=19&lang=" + CurrentLang.ID).urlEncode() + "&filename=" + ("RiR_" + CurrentSource.getSysLangValue("lblTermsAndConditions").clearPathName() + ".pdf").urlEncode() %>">
                                        <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                                    </a>
                                </label>
                                <div class="div_terms">
                                    <%= contUtils.getStp(19, CurrentLang.ID, "#description#")%>
                                </div>
                                <div class="accettocheck" style="height: 30px;" id="cont_termsOfService">
                                    <input type="checkbox" id="chk_termsOfService" />
                                    <label for="chk_termsOfService">
                                        <%=CurrentSource.getSysLangValue("lblAcceptTermsAndConditions")%>*</label>
                                </div>
                                <span id="val_termsOfService" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqRequiredAcceptTermsOfService")%></span>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <asp:LinkButton ID="lnk_send" CssClass="btn arrow" runat="server" OnClick="lnk_send_Click" OnClientClick="return validateRequestForm()"><span>Send Request</span></asp:LinkButton>
                    </div>
            </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            
        
        <div class="nulla">
        </div>
    </div>
    <uc3:uc_apt_in_rome_bottom id="UC_apt_in_rome_bottom1" runat="server" />
    <script type="text/javascript">
        function validateRequestForm() {
            var _validate = true;
            var _lastFocus = "";
            $("#errorLi").hide();

            $("#val_contactEmail").hide();
            $("#<%= txt_contactEmail.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_contactEmail.ClientID%>").val()) == "") {
                $("#<%= txt_contactEmail.ClientID%>").addClass(FORM_errorClass);
                $("#val_contactEmail").html('This field is required. Please enter a value.');
                $("#val_contactEmail").css("display", "block");
                _validate = false;
            }
            else if (!FORM_validateEmail($("#<%= txt_contactEmail.ClientID%>").val())) {
                $("#<%= txt_contactEmail.ClientID%>").addClass(FORM_errorClass);
                $("#val_contactEmail").html('Please enter a valid email address.');
                $("#val_contactEmail").css("display", "block");
                _validate = false;
            }
            if ($.trim($("#<%= txt_contactEmail_conf.ClientID%>").val()) != $.trim($("#<%= txt_contactEmail.ClientID%> ").val())) {
                $("#<%= txt_contactEmail_conf.ClientID%>").addClass(FORM_errorClass);
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

            $("#val_privacyAgree").css("display", "none");
            $("#cont_privacyAgree").removeClass(FORM_errorClass);
            if (!$("#chk_privacyAgree").is(':checked')) {
                $("#cont_privacyAgree").addClass(FORM_errorClass);
                $("#val_privacyAgree").css("display", "block");
                _validate = false;
            }
//            $("#val_termsOfService").css("display", "none");
//            $("#cont_termsOfService").removeClass(FORM_errorClass);
//            if (!$("#chk_termsOfService").is(':checked')) {
//                $("#cont_termsOfService").addClass(FORM_errorClass);
//                $("#val_termsOfService").css("display", "block");
//                _validate = false;
//            }
            if (!_validate) {
                $("#errorLi").css("display", "block");
                $.scrollTo($("#errorLi"), 500);
                //$.scrollTo($(_lastFocus), 500);
            }
            return _validate;
        }
        $(function () {
            //$("#<% =txt_contactEmail.ClientID %>,#<% =txt_contactEmail_conf.ClientID %>").bind("cut copy paste", function (event) {event.preventDefault();});
        });
    </script>
</asp:Content>
