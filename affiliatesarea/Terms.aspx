<%@ Page Title="" Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="Terms.aspx.cs" Inherits="RentalInRome.affiliatesarea.Terms" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    
        <%-- <div class="sx" style="width:262px;">
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:UC_sx ID="UC_sx1" runat="server" />
                </div>
            </div>
        </div>--%>
        <h3 style="margin-bottom: 20px; margin-left: 8px;" class="underlined">Terms of service</h3>

        <div class="box_client_booking">
                 <span class="titprivacyagenty" runat="server" id="lbl_DescrizioneNellaPaginaAccettazioneContratto">
                     <%=CurrentSource.getSysLangValue("lblDescrizioneNellaPaginaAccettazioneContratto")%>
                </span>
            <div class="line2">
                <div class="left">
                    <label class="desc">
                        <%=CurrentSource.getSysLangValue("lblPrivacyPolicy")%>
                        <a href="<%= CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + (CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "stp_contentonly_download.aspx?id=2&lang=" + CurrentLang.ID).urlEncode() + "&filename=" + ("RiR_" + CurrentSource.getSysLangValue("lblPrivacyPolicy").clearPathName() + ".pdf").urlEncode()%>" target="_blank">
                            <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                        </a>
                    </label>
                    <div class="div_terms">
                        <%= contUtils.getStp(2, CurrentLang.ID, "#description#")%>
                    </div>
                    <asp:PlaceHolder ID="PH_1" runat="server">
                    <div class="accettocheck" style="height: 30px;" id="chk_privacyAgree_cont">
                        <input type="checkbox" id="chk_privacyAgree" />
                        <label for="chk_privacyAgree">
                            <%=CurrentSource.getSysLangValue("lblAcceptPersonalDataPrivacy")%>*</label>
                    </div>
                    <span id="chk_privacyAgree_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                        <%=CurrentSource.getSysLangValue("reqRequiredAcceptPrivacy")%></span>
                    </asp:PlaceHolder>
                </div>
                <div class="left">
                    <label class="desc">
                        <%=CurrentSource.getSysLangValue("lblTermsAndConditions")%>
                        <a href="<%= CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + (CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "stp_contentonly_download.aspx?id=38&lang=" + CurrentLang.ID).urlEncode() + "&filename=" + ("RiR_" + CurrentSource.getSysLangValue("lblTermsAndConditions").clearPathName() + ".pdf").urlEncode()%>" target="_blank">
                            <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                        </a>
                    </label>
                    <div class="div_terms">
                        <%= contUtils.getStp(38, CurrentLang.ID, "#description#")%>
                    </div>
                    <asp:PlaceHolder ID="PH_2" runat="server">
                    <div class="accettocheck" style="height: 30px;" id="chk_termsOfService_cont">
                        <input type="checkbox" id="chk_termsOfService" />
                        <label for="chk_termsOfService">
                            <%=CurrentSource.getSysLangValue("lblAcceptTermsAndConditions")%>*</label>
                    </div>
                    <span id="chk_termsOfService_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                        <%=CurrentSource.getSysLangValue("reqRequiredAcceptTermsOfService")%></span>
                    </asp:PlaceHolder>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:LinkButton ID="lnkAccept" runat="server" CssClass="buttprosegui" OnClick="lnkAccept_Click" OnClientClick="return RNT_validateRequestForm();" Style="margin-top: 15px;"><%=CurrentSource.getSysLangValue("reqSubmit")%></asp:LinkButton>
        </div>
    <script type="text/javascript">
        function RNT_validateRequestForm() {
            var _validate = true;
            $("#chk_privacyAgree_check").hide();
            $("#chk_privacyAgree_cont").removeClass(FORM_errorClass);
            $("#chk_termsOfService_check").hide();
            $("#chk_termsOfService_cont").removeClass(FORM_errorClass);
            if (!$("#chk_privacyAgree").is(':checked')) {
                $("#chk_privacyAgree_cont").addClass(FORM_errorClass);
                $("#chk_privacyAgree_check").css("display", "block");
                _validate = false;
            }
            if (!$("#chk_termsOfService").is(':checked')) {
                $("#chk_termsOfService_cont").addClass(FORM_errorClass);
                $("#chk_termsOfService_check").css("display", "block");
                _validate = false;
            }
            return _validate;
        }
    </script>
</asp:Content>
