<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_limo_request.aspx.cs" Inherits="RentalInRome.stp_limo_request" %>

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
    <link href="/jquery/css/ui-timepicker-custom.css" rel="stylesheet" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="/jquery/js/ui-timepicker-custom.js"></script>

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
    <div id="breadcrumbs">
        <uc1:UC_menu_breadcrumb ID="UC_menu_breadcrumb1" runat="server" />
    </div>
    <h3 class="underlined">
        <%=CurrentSource.getSysLangValue(" lblContacts")%></h3>
    <div class="nulla">
    </div>
    <div id="contatti">
        <div class="sx">
            <div id="infoCont">
                <div class="infoBox">
                    <%= ltr_description.Text %>
                </div>
            </div>
            <div style="float: left; padding-top: 10px;">
                <a class="freeCall" onclick="c2c=window.open('http://gol.green-online.it/c2c.php?user=29&amp;cmp=1&amp;id=1','GreenOnLine','width=350,height=525,left=15,top=15,resizable=no,toolbar=no,location=no,titlebar=no,menubar=no,dependent=yes');c2c.focus();" href="javascript:void(0);">
                    <img border="0" align="left" alt="GreenOnLine" src="http://gol.green-online.it/links.php?user=29&amp;link=1">
                    <span>
                        <%=CurrentSource.getSysLangValue("lblCallForFree")%></span>
                </a>
                <a id="_lpChatBtn" href='http://server.iad.liveperson.net/hc/1220380/?cmd=file&file=visitorWantsToChat&site=1220380&byhref=1&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a' target='chat1220380' onclick="lpButtonCTTUrl = 'http://server.iad.liveperson.net/hc/1220380/?cmd=file&file=visitorWantsToChat&site=1220380&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a&referrer='+escape(document.location); lpButtonCTTUrl = (typeof(lpAppendVisitorCookies) != 'undefined' ? lpAppendVisitorCookies(lpButtonCTTUrl) : lpButtonCTTUrl); lpButtonCTTUrl = ((typeof(lpMTag)!='undefined' && typeof(lpMTag.addFirstPartyCookies)!='undefined')?lpMTag.addFirstPartyCookies(lpButtonCTTUrl):lpButtonCTTUrl);window.open(lpButtonCTTUrl,'chat1220380','width=472,height=320,resizable=yes');return false;">
                    <img src='http://server.iad.liveperson.net/hc/1220380/?cmd=repstate&site=1220380&channel=web&&ver=1&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a' name='hcIcon' border="0" />
                </a>
            </div>
        </div>
        <div class="dx">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="pnl_request_sent" class="box_client_booking" runat="server" visible="false" style="width: 545px;">
                        <%=CurrentSource.getSysLangValue("reqRequestSent")%>
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
                            <div id="drp_trip_mode_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblArrivalAndDeparture")%>*
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_trip_mode" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="1" Text="Arrival to Rome"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Departure from Rome"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Arrival to Rome + Departure from Rome"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="mode_in place" style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line mode_in place">
                            <div id="Div1" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lbl_ArrivalToRome")%>
                                </label>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in place">
                            <div id="drp_in_place_cont" class="left">
                                <label>
                                   <%=CurrentSource.getSysLangValue("lblArrivalplace")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_in_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="air" Text="Airport"></asp:ListItem>
                                        <asp:ListItem Value="sea" Text="Seaport"></asp:ListItem>
                                        <asp:ListItem Value="train" Text="Railway station"></asp:ListItem>
                                        <asp:ListItem Value="other" Text="Other"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in air hide">
                            <div id="drp_in_air_place_cont" class="left">
                                <label>
                                   <%=CurrentSource.getSysLangValue("lbl_AirportName")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_in_air_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="(FCO) Fiumicino airport" Text="(FCO) Fiumicino airport"></asp:ListItem>
                                        <asp:ListItem Value="(CPO) Ciampino airport" Text="(CPO) Ciampino airport"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in air dett hide">
                            <div id="Div3" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_Airline")%> / <%=CurrentSource.getSysLangValue("lbl_FlightNum")%>:
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_in_air_dett" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in sea hide">
                            <div id="Div2" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_SeaportName")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_in_sea_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Civitavecchia seaport" Text="Civitavecchia seaport"></asp:ListItem>
                                        <asp:ListItem Value="Fiumicino seaport" Text="Fiumicino seaport"></asp:ListItem>
                                        <asp:ListItem Value="Gaeta seaport" Text="Gaeta seaport"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in sea dett hide">
                            <div id="Div4" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblShippingCompany")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_in_sea_dett" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in train hide">
                            <div id="Div5" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_RailwayStationName")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_in_train_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Roma Termini railway station" Text="Roma Termini railway station"></asp:ListItem>
                                        <asp:ListItem Value="Roma Tiburtina railway station" Text="Roma Tiburtina railway station"></asp:ListItem>
                                        <asp:ListItem Value="Roma Ostiense railway station" Text="Roma Ostiense railway station"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in train dett hide">
                            <div id="Div6" class="left">
                                <label>
                                   <%=CurrentSource.getSysLangValue("lbl_TrainNum")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_in_train_dett" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in other hide">
                            <div id="Div7" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblOtherPlace")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_in_other_dett" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in date">
                            <div id="txt_dtBirth_cont" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblArrivalDate")%>
                                </label>
                                <div>
                                    <input type="text" id="txt_in_date" style="width: 120px" />
                                    <img id="trigger_in_date" class="datepicker" src="/images/icons/calendar.png" alt="Pick a date." />
                                    <asp:HiddenField ID="HF_in_date" runat="server" />
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in date">
                            <div id="Div8" class="left">
                                <label>
                                     <%=CurrentSource.getSysLangValue("lblArrivaltime")%>
                                </label>
                                <div>
                                    <span id="in_time_view" runat="server" title="click to select the time" style="cursor: pointer;"></span>
                                    <img id="in_time_toggler" src="images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />
                                    <asp:HiddenField ID="HF_in_time_cont" runat="server" />
                                    <script type="text/javascript">
                                        var in_time = new timePicker({ timeCont: '<%= HF_in_time_cont.ClientID %>', timeView: '<%= in_time_view.ClientID %>', timeToggler: 'in_time_toggler', viewAsToggler: true });
                                    </script>

                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in pick" id="req_myList">
                            <div class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_DestinationApartment")%>
                                </label>
                                <div>
                                    <asp:HiddenField ID="HF_estateId" runat="server" Value="0" />
                                    <asp:HiddenField ID="HF_deleteId" runat="server" Value="0" />
                                    <asp:TextBox ID="txt_dest_apt" runat="server" CssClass="req_aptComplete" Width="195"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_in pick">
                            <div class="left">
                                <label >
                                 <%=CurrentSource.getSysLangValue("lbl_OrOtherAddress")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_dest_other" runat="server" Width="195"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="mode_out place" style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line mode_out place">
                            <div id="Div9" class="left">
                                <label class="desc">
                                   <%=CurrentSource.getSysLangValue("lbl_DepartureFromRome")%>
                                </label>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out place">
                            <div id="drp_out_place_cont" class="left">
                                <label>
                                     <%=CurrentSource.getSysLangValue("lblDeparturePlace")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_out_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="air" Text="Airport"></asp:ListItem>
                                        <asp:ListItem Value="sea" Text="Seaport"></asp:ListItem>
                                        <asp:ListItem Value="train" Text="Railway station"></asp:ListItem>
                                        <asp:ListItem Value="other" Text="Other"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out air hide">
                            <div id="drp_out_air_place_cont" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_AirportName")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_out_air_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="(FCO) Fiumicino airport" Text="(FCO) Fiumicino airport"></asp:ListItem>
                                        <asp:ListItem Value="(CPO) Ciampino airport" Text="(CPO) Ciampino airport"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out air dett hide">
                            <div id="Div10" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_Airline")%> / <%=CurrentSource.getSysLangValue("lbl_FlightNum")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_out_air_dett" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out sea hide">
                            <div id="Div11" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_SeaportName")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_out_sea_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Civitavecchia seaport" Text="Civitavecchia seaport"></asp:ListItem>
                                        <asp:ListItem Value="Fiumicino seaport" Text="Fiumicino seaport"></asp:ListItem>
                                        <asp:ListItem Value="Gaeta seaport" Text="Gaeta seaport"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out sea dett hide">
                            <div id="Div12" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblShippingCompany")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_out_sea_dett" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out train hide">
                            <div id="Div13" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_RailwayStationName")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_out_train_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Roma Termini railway station" Text="Roma Termini railway station"></asp:ListItem>
                                        <asp:ListItem Value="Roma Tiburtina railway station" Text="Roma Tiburtina railway station"></asp:ListItem>
                                        <asp:ListItem Value="Roma Ostiense railway station" Text="Roma Ostiense railway station"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out train dett hide">
                            <div id="Div14" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_TrainNum")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_out_train_dett" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out other hide">
                            <div id="Div15" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblOtherPlace")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_out_other_dett" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out date">
                            <div id="Div16" class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblDepartureDate")%>
                                </label>
                                <div>
                                    <input type="text" id="txt_out_date" style="width: 120px" />
                                    <img id="trigger_out_date" class="datepicker" src="/images/icons/calendar.png" alt="Pick a date." />
                                    <asp:HiddenField ID="HF_out_date" runat="server" />
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out date">
                            <div id="Div17" class="left">
                                <label>
                                    <%= CurrentSource.getSysLangValue("lblDepartureTimeAirPortTrain")%>
                                </label>
                                <div>
                                    <span id="out_time_view" runat="server" title="click to select the time" style="cursor: pointer;"></span>
                                    <img id="out_time_toggler" src="images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />
                                    <asp:HiddenField ID="HF_out_time_cont" runat="server" />

                                    <script type="text/javascript">
                                        var out_time = new timePicker({ timeCont: '<%= HF_out_time_cont.ClientID %>', timeView: '<%= out_time_view.ClientID %>', timeToggler: 'out_time_toggler', viewAsToggler: true });
                                    </script>

                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out pick" id="Div18">
                            <div class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblPickupApt")%>
                                </label>
                                <div>
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                    <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
                                    <asp:TextBox ID="txt_pick_apt" runat="server" CssClass="req_aptComplete" Width="195"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line mode_out pick">
                            <div class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lbl_OrOtherAddress")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_pick_other" runat="server" Width="195"></asp:TextBox>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line">
                            <div id="Div19" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblNumPersons")%>
                                </label>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div class="left" style="margin-right: 30px;">
                                <label>
                                    <%= CurrentSource.getSysLangValue("reqAdults")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_num_adult" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="left" style="margin-right: 30px;">
                                <labe>
                                    <%= CurrentSource.getSysLangValue("lblChildren3OrOver")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_num_child_over" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="left">
                                <label>
                                    <%= CurrentSource.getSysLangValue("lblChildrenUnder3")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_num_child_min" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div style="border-top: 1px dotted #CCCCCC; clear: both; display: block; margin: 15px 0;">
                        </div>
                        <div class="line">
                            <div id="Div20" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblLuggage")%>
                                </label>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                            <div class="left" style="margin-right: 30px;">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblLargeSuitcases")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_num_case_l" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="left" style="margin-right: 30px;">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblMediumSuitcases")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_num_case_m" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="left">
                                <label>
                                    <%=CurrentSource.getSysLangValue("lblSmallSuitcases")%>
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_num_case_s" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                                <div class="nulla">
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
                                    <%=CurrentSource.getSysLangValue("lblPrivacyPolicy")%></label>
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
                                    <%=CurrentSource.getSysLangValue("lblTermsAndConditions")%></label>
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
            <div class="lmBox">
                <h5>
                    <%=CurrentSource.getSysLangValue("lblLastMinute")%></h5>
                <%=CurrentSource.getSysLangValue("lblLastMinuteTxt")%>
                <br />
                <br />
                <a href="<%=CurrentSource.getPagePath("12", "stp", CurrentLang.ID.ToString()) %>">
                    <%=CurrentSource.getSysLangValue("lblMoreInformation")%></a>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />

    <script type="text/javascript">
        function checkArrivalDeparture() {
            var _modeInt = $("#<%=drp_trip_mode.ClientID %>").val();
            $(".mode_out").addClass("hide");
            $(".mode_in").addClass("hide");
            if (_modeInt == "1") {
                setMode("mode_in");
            }
            else if (_modeInt == "2") {
                setMode("mode_out");
            }
            else {
                setMode("mode_in");
                setMode("mode_out");
            }

            function setMode(_mode) {
                if (_mode == "") return;
                var _place = "";
                var _place_select = "";
                $("." + _mode + ".pick").removeClass("hide");
                $("." + _mode + ".dest").removeClass("hide");
                $("." + _mode + ".date").removeClass("hide");
                $("." + _mode + ".time").removeClass("hide");
                $("." + _mode + ".place").removeClass("hide");
                _place = _mode == "mode_in" ? $("#<%=drp_in_place.ClientID %>").val() : $("#<%=drp_out_place.ClientID %>").val();

                if (_place != "")
                    $("." + _mode + "." + _place).removeClass("hide");
                if (_mode == "mode_in") {
                    if (_place == "air")
                        _place_select = "#<%=drp_in_air_place.ClientID %>";
                    if (_place == "sea")
                        _place_select = "#<%=drp_in_sea_place.ClientID %>";
                    if (_place == "train")
                        _place_select = "#<%=drp_in_train_place.ClientID %>";
                }
                else {
                    if (_place == "air")
                        _place_select = "#<%=drp_out_air_place.ClientID %>";
                    if (_place == "sea")
                        _place_select = "#<%=drp_out_sea_place.ClientID %>";
                    if (_place == "train")
                        _place_select = "#<%=drp_out_train_place.ClientID %>";
                }

                if ($(_place_select).val() == "" || _place == "")
                    $("." + _mode + "." + _place + ".dett").addClass("hide");
                else
                    $("." + _mode + "." + _place + ".dett").removeClass("hide");
            }

        }
        checkArrivalDeparture();
    </script>

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
    </script>

</asp:Content>
