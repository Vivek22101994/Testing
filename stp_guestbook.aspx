<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_guestbook.aspx.cs" Inherits="RentalInRome.stp_guestbook" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="uc/UC_header.ascx" TagName="UC_header" TagPrefix="uc1" %>
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

    <script type="text/javascript">
        function RNT_estComment_changePage(page) {
            $("#hf_estComment_currPage").val("" + page);
            RNT_estComment_fillList("list");
        }
        function RNT_estComment_fillList(action) {
            if (action == "list") SITE_showLoader();
            var _url = "/webservice/rnt_estate_comment.aspx";
            _url += "?action=" + action;
            _url += "&SESSION_ID=<%= CURRENT_SESSION_ID %>";
            _url += "&currEstate=0";
            _url += "&voteRange=";
            _url += "&currPage=" + $("#hf_estComment_currPage").val();
            _url += "&numPerPage=20";
            _url += "&fullView=1";
            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function(html) {
                    $("#commentListCont").html(html);
                    if (action == "list") SITE_hideLoader();
                }
            });
        }
        $(document).ready(function() {
            RNT_estComment_fillList("first");
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_pidReservation" runat="server" />
    <div id="guestBook" style="margin: 10px 0 0 20px;">
        <h3 class="underlined">
            <%= ltr_title.Text%>
        </h3>
        <span style="display: block; float: left; margin-bottom: 20px;">
            <%= ltr_description.Text%>
        </span>
        <div class="nulla">
        </div>
        <div style="display: block">
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
            </asp:ScriptManagerProxy>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <h5 class="titBar" style="height: 24px; padding-top: 4px; margin-bottom: 0">
                        <%= CurrentSource.getSysLangValue("lblSignTheGuestbook")%>
                    </h5>
                    <div class="nulla">
                    </div>
                    <div class="editComment" id="pnl_sent" runat="server" visible="false">
                       <%= CurrentSource.getSysLangValue("reqCommentAdded")%>
                    </div>
                    <div class="editComment" id="pnl_cont" runat="server">
                        <div class="nomeComm" style="margin: 15px 15px 15px 0;">
                            <strong>
                                <%= CurrentSource.getSysLangValue("lblName")%>:</strong>
                            <asp:TextBox ID="txt_name_full" runat="server" Style="width: 200px;"></asp:TextBox>
                            <span id="txt_name_full_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                        </div>
                        <div class="nomeComm" style="margin: 15px 15px 15px 0;">
                            <strong>Email:</strong>
                            <asp:TextBox ID="txt_email" runat="server" Style="width: 200px;"></asp:TextBox>
                            <span id="txt_email_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;"></span>
                        </div>
                        <div class="userType">
                            <span>
                                <asp:RadioButton ID="rbt_pers_m" runat="server" GroupName="pers" />
                                <a class="ico_tooltip" title="div_man">
                                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/user-m.gif" alt="<%= CurrentSource.getSysLangValue("lblMan")%>" /></a>
                                <div id="tooltip_div_man" style="display: none;">
                                    <%= CurrentSource.getSysLangValue("lblMan")%>
                                </div>
                            </span>
                            <span>
                                <asp:RadioButton ID="rbt_pers_f" runat="server" GroupName="pers" />
                                <a class="ico_tooltip" title="div_woman">
                                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/user-f.gif" alt="<%= CurrentSource.getSysLangValue("lblWoman")%>:" /></a>
                                <div id="tooltip_div_woman" style="display: none;">
                                    <%= CurrentSource.getSysLangValue("lblWoman")%>
                                </div>
                            </span>
                            <span>
                                <asp:RadioButton ID="rbt_pers_co" runat="server" GroupName="pers" />
                                <a class="ico_tooltip" title="div_couple">
                                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/user-co.gif" alt="<%= CurrentSource.getSysLangValue("lblCouple")%>" /></a>
                                <div id="tooltip_div_couple" style="display: none;">
                                    <%= CurrentSource.getSysLangValue("lblCouple")%>
                                </div>
                            </span>
                            <span>
                                <asp:RadioButton ID="rbt_pers_fam" runat="server" GroupName="pers" />
                                <a class="ico_tooltip" title="div_family">
                                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/user-fam.gif" alt="<%= CurrentSource.getSysLangValue("lblFamily")%>" /></a>
                                <div id="tooltip_div_family" style="display: none;">
                                    <%= CurrentSource.getSysLangValue("lblFamily")%>
                                </div>
                            </span>
                            <span>
                                <asp:RadioButton ID="rbt_pers_gr" runat="server" GroupName="pers" />
                                <a class="ico_tooltip" title="div_groups">
                                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/user-gr.gif" alt="<%= CurrentSource.getSysLangValue("lblGroups")%>" /></a>
                                <div id="tooltip_div_groups" style="display: none;">
                                    <%= CurrentSource.getSysLangValue("lblGroups")%>
                                </div>
                            </span>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="nomeComm">
                            <span>
                                <%= CurrentSource.getSysLangValue("lblCountry")%>
                            </span>
                            <asp:DropDownList runat="server" ID="drp_country" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="title">
                            </asp:DropDownList>
                            <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                <WhereParameters>
                                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                </WhereParameters>
                            </asp:LinqDataSource>
                            <span id="drp_country_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                <%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></span>
                        </div>
                        
                        <div class="nomeComm">
                            <span>
                                <%= CurrentSource.getSysLangValue("lblApartmentComment")%>
                            </span>
                            <asp:DropDownList runat="server" ID="drp_estate">
                            </asp:DropDownList>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="vote_num">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <strong><%= CurrentSource.getSysLangValue("lblRating")%></strong>
                                    </td>
                                    <td align="center">
                                        <%= CurrentSource.getSysLangValue("lblRatingCarente")%>
                                    </td>
                                    <td align="center">
                                        <%= CurrentSource.getSysLangValue("lblRatingSufficiente")%>
                                    </td>
                                    <td align="center">
                                        <%= CurrentSource.getSysLangValue("lblRatingBuono")%>
                                    </td>
                                    <td align="center">
                                        <%= CurrentSource.getSysLangValue("lblRatingOttimo")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= CurrentSource.getSysLangValue("lblRatingForStaff")%>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteStaff_4" runat="server" GroupName="voteStaff" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteStaff_6" runat="server" GroupName="voteStaff" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteStaff_8" runat="server" GroupName="voteStaff" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteStaff_10" runat="server" GroupName="voteStaff" /></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= CurrentSource.getSysLangValue("lblRatingForService")%>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteService_4" runat="server" GroupName="voteService" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteService_6" runat="server" GroupName="voteService" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteService_8" runat="server" GroupName="voteService" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteService_10" runat="server" GroupName="voteService" /></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= CurrentSource.getSysLangValue("lblRatingForCleaning")%>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteCleaning_4" runat="server" GroupName="voteCleaning" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteCleaning_6" runat="server" GroupName="voteCleaning" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteCleaning_8" runat="server" GroupName="voteCleaning" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteCleaning_10" runat="server" GroupName="voteCleaning" /></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= CurrentSource.getSysLangValue("lblRatingForComfort")%>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteComfort_4" runat="server" GroupName="voteComfort" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteComfort_6" runat="server" GroupName="voteComfort" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteComfort_8" runat="server" GroupName="voteComfort" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteComfort_10" runat="server" GroupName="voteComfort" /></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= CurrentSource.getSysLangValue("lblRatingForQualityPrice")%>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteQualityPrice_4" runat="server" GroupName="voteQualityPrice" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteQualityPrice_6" runat="server" GroupName="voteQualityPrice" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteQualityPrice_8" runat="server" GroupName="voteQualityPrice" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_voteQualityPrice_10" runat="server" GroupName="voteQualityPrice" /></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= CurrentSource.getSysLangValue("lblRatingForPosition")%>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_votePosition_4" runat="server" GroupName="votePosition" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_votePosition_6" runat="server" GroupName="votePosition" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_votePosition_8" runat="server" GroupName="votePosition" /></label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:RadioButton ID="rbtn_votePosition_10" runat="server" GroupName="votePosition" /></label>
                                    </td>
                                </tr>
                            </table>

                        </div>
                        <div class="nulla">
                        </div>
                        <div class="positive">
                            <strong><%= CurrentSource.getSysLangValue("lblRatingCommentPositive")%></strong>
                            <asp:TextBox ID="txt_body" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        
                        <div class="negative">
                            <strong><%= CurrentSource.getSysLangValue("lblRatingCommentNegative")%> </strong>
                            <asp:TextBox ID="txt_bodyNegative" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <div class="nulla">
                        </div>
                        <span id="txt_body_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                            Links are not allowed here.</span>
                        <div class="nulla">
                        </div>
                        <div id="Captcha">
                        <telerik:RadCaptcha ID="RadCaptcha1" runat="server" ValidatedTextBoxID="txtCaptcha" EnableRefreshImage="true" ErrorMessage="" ValidationGroup="gbook">
                            <CaptchaImage ImageCssClass="imageClass" RenderImageOnly="true" />
                        </telerik:RadCaptcha>
                        <div class="RadCaptchaText">
                            <asp:TextBox ID="txtCaptcha" runat="server"></asp:TextBox>
                            <span>Insert the text you see in the image.</span>
                            <span id="RadCaptchaCheck" runat="server" visible="false" class="alertErrorSmall" style="width: 300px; float: none;">Page not valid. The code you entered is not valid.</span>
                        </div> </div>
                        <div class="nulla">
                        </div>
                        <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click" ValidationGroup="gbook" OnClientClick="return RNT_validateRequestForm()" CssClass="btn" Style="float: right; margin-right: 0;">
						<span>
							<%= CurrentSource.getSysLangValue("lblSendYourFeedback")%>
						</span>
                        </asp:LinkButton>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="nulla">
            </div>
        </div>

        <script type="text/javascript">
            function RNT_validateRequestForm() {
                var _validate = true;
                $("#txt_email_check").hide();
                $("#<%= txt_email.ClientID%>").removeClass(FORM_errorClass);
                $("#txt_name_full_check").hide();
                $("#<%= txt_name_full.ClientID%>").removeClass(FORM_errorClass);
                $("#drp_country_check").hide();
                $("#<%= drp_country.ClientID%>").removeClass(FORM_errorClass);
                $("#txt_body_check").hide();
                $("#<%= txt_body.ClientID%>").removeClass(FORM_errorClass);

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
                if ($.trim($("#<%= txt_name_full.ClientID%>").val()) == "") {
                    $("#<%= txt_name_full.ClientID%>").addClass(FORM_errorClass);
                    $("#txt_name_full_check").css("display", "block");
                    _validate = false;
                }
                if ($.trim($("#<%= drp_country.ClientID%>").val()) == "") {
                    $("#<%= drp_country.ClientID%>").addClass(FORM_errorClass);
                    $("#drp_country_check").css("display", "block");
                    _validate = false;
                }
                if ($("#<%= txt_body.ClientID%>").val().toLowerCase().indexOf("<a href") >= 0 || $("#<%= txt_bodyNegative.ClientID%>").val().toLowerCase().indexOf("<a href") >= 0) {
                    $("#<%= txt_body.ClientID%>").addClass(FORM_errorClass);
                    $("#txt_body_check").css("display", "block");
                    _validate = false;
                }
                return _validate;
            }
        </script>

        <div id="commentListCont">
            <input type="hidden" id="hf_estComment_currPage" value="1" />
            <div class="loading_img">
            </div>
        </div>
    </div>
    <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />
</asp:Content>
