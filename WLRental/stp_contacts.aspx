<%@ Page Title="" Language="C#" MasterPageFile="~/WLRental/common/MP_WLRental.Master" AutoEventWireup="true" CodeBehind="stp_contacts.aspx.cs" Inherits="RentalInRome.WLRental.stp_contacts" %>

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
    <style type="text/css">

        #aptInRome {
            display: none;
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
    <h3 class="underlined" style="margin-top: 0;">Contacts</h3>
    <div class="nulla">
    </div>
    <div id="contatti">
        <div class="sx">
            <div id="infoCont">
                <div class="infoBox">
                    <strong style="font-size: 18px;margin-top:10px;">Reservations Center</strong><br />
                    Tel: <strong style="font-size: 15px;"><%= App.WLAgent.contactPhone %></strong>
                    <br />
                    <em>(24h/day, 7/7 days)</em>
                </div>
            </div>
            <div style="float: left; padding-top: 10px; display: none;">
                <% if (CurrentLang.ID == 1 && 1 == 2)
                   { %>
                <a class="freeCall" onclick="c2c=window.open('http://gol.green-online.it/c2c.php?user=29&amp;cmp=1&amp;id=1','GreenOnLine','width=350,height=525,left=15,top=15,resizable=no,toolbar=no,location=no,titlebar=no,menubar=no,dependent=yes');c2c.focus();" href="javascript:void(0);">
                    <img border="0" align="left" alt="GreenOnLine" src="http://gol.green-online.it/links.php?user=29&amp;link=1">
                    <span>
                        <%=CurrentSource.getSysLangValue("lblCallForFree")%></span>
                </a>
                <%} %>
                <a id="_lpChatBtn" href='http://server.iad.liveperson.net/hc/1220380/?cmd=file&file=visitorWantsToChat&site=1220380&byhref=1&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a' target='chat1220380' onclick="lpButtonCTTUrl = 'http://server.iad.liveperson.net/hc/1220380/?cmd=file&file=visitorWantsToChat&site=1220380&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a&referrer='+escape(document.location); lpButtonCTTUrl = (typeof(lpAppendVisitorCookies) != 'undefined' ? lpAppendVisitorCookies(lpButtonCTTUrl) : lpButtonCTTUrl); lpButtonCTTUrl = ((typeof(lpMTag)!='undefined' && typeof(lpMTag.addFirstPartyCookies)!='undefined')?lpMTag.addFirstPartyCookies(lpButtonCTTUrl):lpButtonCTTUrl);window.open(lpButtonCTTUrl,'chat1220380','width=472,height=320,resizable=yes');return false;">
                    <img src='http://server.iad.liveperson.net/hc/1220380/?cmd=repstate&site=1220380&channel=web&&ver=1&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a' name='hcIcon' border="0" />
                </a>
            </div>
        </div>
        <div class="dx" id="contatti_dx">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="pnl_request_sent" class="box_client_booking" runat="server" visible="false" style="width: 545px;">
                        <%=CurrentSource.getSysLangValue("reqRequestSent")%>
                        <asp:HiddenField ID="HF_IdRequest" runat="server" />
                        <iframe width="0" height="0" border="0" style="width: 0; height: 0; border: none; display: none;" src="/webservice/conversionScriptsRntRequest.aspx?id=<%= HF_IdRequest.Value %>"></iframe>
                    </div>
                    <!-- ADROLL  Conversion Page -->
                    <div id="pnl_adRollScript" runat="server" visible="false">
                        <script type="text/javascript">
                            adroll_adv_id = "W5NVD6QFLRBLTMY4G444SZ";
                            adroll_pix_id = "MRHU7YJK65H5NKQVYJKNHQ";
                               
                            (function () {
                                var oldonload = window.onload;
                                window.onload = function () {
                                    __adroll_loaded = true;
                                    var scr = document.createElement("script");
                                    var host = (("https:" == document.location.protocol) ? "https://s.adroll.com" : "http://a.adroll.com");
                                    scr.setAttribute('async', 'true');
                                    scr.type = "text/javascript";
                                    scr.src = host + "/j/roundtrip.js";
                                    ((document.getElementsByTagName('head') || [null])[0] ||
                                     document.getElementsByTagName('script')[0].parentNode).appendChild(scr);
                                    if (oldonload) {
                                        oldonload();
                                    }
                                };
                            }());


                        </script>
                        <script type="text/javascript">
                            
                            try {
                                console.log('sending conversation');
                                __adroll.record_user({ "adroll_segments": "c42f565e" })
                            } catch (err) { }
                            
                        </script>

                    </div>
                    <div id="pnl_request_cont" class="box_client_booking" runat="server" style="width: 545px;">
                        <div id="errorLi" class="line" style="color: red; margin-bottom: 30px; width: 550px; display: none;">
                            <h3 id="errorMsgLbl">
                                <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
                            <p id="errorMsg">
                                <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
                            </p>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div id="txt_email_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqEmail")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_email" runat="server" Style="width: 300px;"></asp:TextBox>
                                    <span id="txt_email_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;"></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div id="txt_email_conf_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqEmailConfirm")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_email_conf" runat="server" Style="width: 300px;"></asp:TextBox>
                                    <span id="txt_email_conf_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqEmailConfirmError")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div id="txt_name_first_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqFullName")%>*
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_honorific" runat="server" Style="margin-right: 10px; width: 60px;">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_name_full" runat="server" Style="width: 350px;"></asp:TextBox>
                                    <span id="txt_name_full_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div id="txt_phone_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqPhoneNumber")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_phone_mobile" runat="server" Style="width: 300px;"></asp:TextBox>
                                    <span id="txt_phone_mobile_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
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
                                    <asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" Style="width: 350px;" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="id" TabIndex="5">
                                    </asp:DropDownList>
                                    <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                        <WhereParameters>
                                            <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <span id="drp_country_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line" id="req_myList">
                            <div class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqApartments")%>
                                </label>
                                <div>
                                    <ul id="req_myAptList_list" class="f1mylist">
                                    </ul>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                            <div class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("reqAddApartmentsDesc")%>
                                </label>
                                <div>
                                    <asp:HiddenField ID="HF_estateId" runat="server" Value="0" />
                                    <asp:HiddenField ID="HF_deleteId" runat="server" Value="0" />
                                    <asp:TextBox ID="txt_title" runat="server" CssClass="req_aptComplete" Width="195"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>

                            <script type="text/javascript">
                                function req_refreshMyList() {
                                    SITE_cursorWait_show();
                                    var _url = "/webservice/rnt_myPreferedEstateList.aspx";
                                    _url += "?currEstate=" + $( "#<%= HF_estateId.ClientID%>" ).val();
                                    _url += "&SESSION_ID=<%= CURRENT_SESSION_ID %>";
                                    _url += "&lang=<%= CurrentLang.ID %>";
                                    _url += "&deleteEstate=" + $( "#<%= HF_deleteId.ClientID%>" ).val();
                                    _url += "&mode=req";
                                    var _xml = $.ajax({
                                        type: "GET",
                                        url: _url,
                                        dataType: "html",
                                        success: function(html) {
                                            if (html != "") {
                                                $("#req_myAptList_list").html(html);
                                            }
                                            else {
                                                $("#req_myAptList_list").html(html);
                                            }
                                            SITE_cursorWait_hide();
                                        }
                                    });
                                    $( "#<%= HF_estateId.ClientID%>" ).val('0');
                                    $( "#<%= HF_deleteId.ClientID%>" ).val('0');
                                }
                                function req_deleteFromMyList(id) {
                                    $( "#<%= HF_deleteId.ClientID%>" ).val(''+id);
                                    req_refreshMyList();
                                }
                                req_refreshMyList();
                                RNT = {};
                                RNT.EstateOptions = {
                                    id: 0,
                                    path: "",
                                    label: "",
                                    pid_zone: 0
                                };
                                var _estateList_<%= Unique %> = new Array();
                                function getEstateXml_<%= Unique %>(){
                                    var _xml = $.ajax({
                                        type: "GET",
                                        url: "/webservice/rnt_estate_list_xml.aspx?lang=<%= CurrentLang.ID %>&SESSION_ID=<%= CURRENT_SESSION_ID %>",
                                        dataType: "xml",
                                        success: function(xml) {
                                            $(xml).find('item').each(function() {
                                                var _estOpt = {
                                                    id: parseInt($(this).find('id').text(), 10),
                                                    path: $(this).find('path').text(),
                                                    label: $(this).find('title').text(),
                                                    pid_zone: parseInt($(this).find('pid_zone').text(), 10)
                                                };
                                                _estateList_<%= Unique %>.push(_estOpt);
                                            });
                                            setAutocomplete_<%= Unique %>();
                                        }
                                    });
                                }
                                function setAutocomplete_<%= Unique %>(){
                                    $( ".req_aptComplete" ).autocomplete({
                                        source: _estateList_<%= Unique %>,
                                        search: function( event, ui ) {
                                            $( "#<%= HF_estateId.ClientID%>" ).val( '0' );
                                        },
                                        select: function( event, ui ) {
                                            $( "#<%= HF_estateId.ClientID%>" ).val( ui.item.id );
                                            $( ".req_aptComplete" ).val('');
                                            req_refreshMyList();
                                            return false;
                                        }
                                    });
                                    //alert(_estateList_<%= Unique %>);
                                }
                                getEstateXml_<%= Unique %>();
                            </script>

                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line">
                            <div class="left check_list" style="width: 260px;">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqOrAndArea")%>
                                </label>
                                <div>
                                    <asp:CheckBoxList ID="chkList_area" RepeatColumns="2" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="left check_list">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqOrAndPriceRange")%>
                                </label>
                                <div>
                                    <asp:CheckBoxList ID="chkList_price_range" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line">
                            <div class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqCheckInDate")%>
                                </label>
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" style="width: 90px" />
                                                <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                <a class="calendario" id="startCalTrigger_<%= Unique %>"></a>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqCheckOutDate")%>
                                </label>
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" style="width: 90px" />
                                                <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                <a class="calendario" id="endCalTrigger_<%= Unique %>"></a>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblNights")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_dtCount" runat="server" Width="40"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                            <div class="left" style="margin-top: 15px;">
                                <div>
                                    <input id="chk_date_is_flexible" runat="server" value="My travel dates are flexible" tabindex="19" type="checkbox" />
                                    <label class="choice" style="float: left; margin: 0 5px 0 0;">
                                        <%=CurrentSource.getSysLangValue("reqTravelDatesFlexible")%></label>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                            <script type="text/javascript">
                                var _JSCal_Range_<%= Unique %>;
                                function setCal_<%= Unique %>(dtStartInt, dtEndInt) {
                                    _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", countCont: "#<%= txt_dtCount.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
                                }
                                <%= ltr_checkCalDates.Text %>
                            </script>
                            <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line">
                            <div class="left" style="margin-right: 30px;">
                                <label class="desc">
                                    <%= CurrentSource.getSysLangValue("reqAdults")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_adult" runat="server" Width="50" onchange="pgEstate_calculateNumPersons()">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="left" style="margin-right: 30px;">
                                <label class="desc">
                                    <%= CurrentSource.getSysLangValue("lblChildren3OrOver")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_child_over" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="left">
                                <label class="desc">
                                    <%= CurrentSource.getSysLangValue("lblChildrenUnder3")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_child_min" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                                <div class="nulla">
                                </div>

                                <script type="text/javascript">
                                    function pgEstate_calculateNumPersons() {
                                        var _num_persons_max = parseInt($("#<%= HF_num_persons_max.ClientID %>").val(), 10);
                                        var _selNum_adult = parseInt($("#<%= drp_adult.ClientID %>").val(), 10);
                                        var _selNum_child_over = parseInt($("#<%= drp_child_over.ClientID %>").val(), 10);
                                        $("#<%= drp_child_over.ClientID %> option").remove();
                                        $("#<%= drp_child_over.ClientID %>").append("<option value='0'>---</option>");
                                        for (var i = 1; i <= (_num_persons_max - _selNum_adult); i++) {
                                            $("#<%= drp_child_over.ClientID %>").append("<option value='" + i + "'>" + i + "</option>");
                                        }
                                        if (_selNum_child_over > (_num_persons_max - _selNum_adult)) _selNum_child_over = (_num_persons_max - _selNum_adult);
                                        $("#<%= drp_child_over.ClientID %>").val("" + _selNum_child_over);
                                    }
                                </script>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line">
                            <div class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqMeansOfTransport")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_transport" runat="server" Style="width: 322px;">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line">
                            <div class="left check_list">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqServices")%>
                                </label>
                                <div>
                                    <asp:CheckBoxList ID="chkList_services" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line">
                            <div class="left check_list">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqSpecialRequest")%>
                                </label>
                                <div>
                                    <textarea id="txt_note" runat="server" rows="10" cols="50" tabindex="27" style="height: 150px; width: 540px;"></textarea>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line2" style="width: 500px;">
                            <div class="left" style="width: 490px;">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblPrivacyPolicy")%>
                                    <asp:HyperLink ID="HL_getPdf_privacy" runat="server" Target="_blank">
                                    <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                                    </asp:HyperLink>
                                </label>
                                <div class="div_terms" style="width: 470px;">
                                    <asp:Literal ID="ltr_privacy" runat="server"></asp:Literal>
                                </div>
                                <div class="accettocheck" style="height: 20px;" id="chk_privacyAgree_cont">
                                    <input type="checkbox" id="chk_privacyAgree" />
                                    <label for="chk_privacyAgree" style="width: auto;">
                                        <%=CurrentSource.getSysLangValue("lblAcceptPersonalDataPrivacy")%>*</label>
                                </div>
                                <span id="chk_privacyAgree_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqRequiredAcceptPrivacy")%></span>
                            </div>
                            <div class="left" style="width: 490px; margin-top: 20px;">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblTermsAndConditions")%>
                                    <asp:HyperLink ID="HL_getPdf_terms" runat="server" Target="_blank">
                                    <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                                    </asp:HyperLink>
                                </label>
                                <div class="div_terms" style="width: 470px;">
                                    <asp:Literal ID="ltr_terms" runat="server"></asp:Literal>
                                </div>
                                <div class="accettocheck" style="height: 20px;" id="chk_termsOfService_cont">
                                    <input type="checkbox" id="chk_termsOfService" />
                                    <label for="chk_termsOfService" style="width: auto;">
                                        <%=CurrentSource.getSysLangValue("lblAcceptTermsAndConditions")%>*</label>
                                </div>
                                <span id="chk_termsOfService_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                    <%=CurrentSource.getSysLangValue("reqRequiredAcceptTermsOfService")%></span>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <asp:LinkButton ID="lnk_send" CssClass="btn bonifico" runat="server" OnClick="lnk_send_Click" OnClientClick="return RNT_validateRequestForm()"><span>Submit</span></asp:LinkButton>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<div class="lmBox">
                <h5>
                    <%=CurrentSource.getSysLangValue("lblLastMinute")%></h5>
                <%=CurrentSource.getSysLangValue("lblLastMinuteTxt")%>
                <br />
                <br />
                <a href="<%=CurrentSource.getPagePath("12", "stp", CurrentLang.ID.ToString()) %>">
                    <%=CurrentSource.getSysLangValue("lblMoreInformation")%></a>
            </div>--%>
        </div>
        <div class="nulla">
        </div>
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
            $("#txt_name_full_check").hide();
            $("#<%= txt_name_full.ClientID%>").removeClass(FORM_errorClass);
            $("#txt_phone_mobile_check").hide();
            $("#<%= txt_phone_mobile.ClientID%>").removeClass(FORM_errorClass);
            $("#drp_country_check").hide();
            $("#<%= drp_country.ClientID%>").removeClass(FORM_errorClass);
            $("#chk_privacyAgree_check").hide();
            $("#chk_privacyAgree_cont").removeClass(FORM_errorClass);
            $("#chk_termsOfService_check").hide();
            $("#chk_termsOfService_cont").removeClass(FORM_errorClass);

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
        if ($.trim($("#<%= txt_name_full.ClientID%>").val()) == "") {
                $("#<%= txt_name_full.ClientID%>").addClass(FORM_errorClass);
            $("#txt_name_full_check").css("display", "block");
            _validate = false;
        }
        if ($.trim($("#<%= txt_phone_mobile.ClientID%>").val()) == "") {
                $("#<%= txt_phone_mobile.ClientID%>").addClass(FORM_errorClass);
            $("#txt_phone_mobile_check").css("display", "block");
            _validate = false;
        }
        if ($.trim($("#<%= drp_country.ClientID%>").val()) == "") {
                $("#<%= drp_country.ClientID%>").addClass(FORM_errorClass);
            $("#drp_country_check").css("display", "block");
            _validate = false;
        }
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
        if(!_validate){
            $("#errorLi").css("display", "block");
            $.scrollTo($("#errorLi"), 500);
        }
        return _validate;
    }
    $(function() {
        $("#<% =txt_email.ClientID %>,#<% =txt_email_conf.ClientID %>").bind("cut copy paste", function(event) {
            event.preventDefault();
        });
    });

            
    </script>

</asp:Content>