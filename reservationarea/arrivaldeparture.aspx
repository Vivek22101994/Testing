<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="arrivaldeparture.aspx.cs" Inherits="RentalInRome.reservationarea.arrivaldeparture" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Arrival and Departure</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function checkInTime() {
                var _limoHourInt = parseInt(_limoHour.val(), 10);
                var _dtHourInt = parseInt(_dtHour.val(), 10);
                if (_limoHourInt >= _dtHourInt) _dtHour.val((_limoHourInt + 1) + "");
            }
            function checkOutTime() {
                var _limoHourInt = parseInt(_limoHour.val(), 10);
                var _dtHourInt = parseInt(_dtHour.val(), 10);
                if (_limoHourInt <= _dtHourInt) _dtHour.val((_limoHourInt - 1) + "");
            }
            function OnError(sender, args) {
                args.set_cancel(true);
                var elm = sender.get_element();
                //var timeView = elm.get_timeView();
                var oldTime = sender.get_selectedDate();
                //timeView.setTime(oldTime.getHours(), oldTime.getMinutes(), oldTime.getSeconds(), time);
                sender.set_selectedDate(oldTime);
            }
            function DateChanged_PostBack(sender, args) {
                var oldDate = args.get_oldDate();
                var selDate = args.get_newDate();
                if (selDate == null || selDate == oldDate) return;
                var dtMin = sender.get_minDate();
                var dtMax = sender.get_maxDate();
                if (selDate < dtMin) sender.set_selectedDate(dtMin);
                else if (selDate > dtMax) sender.set_selectedDate(dtMax);
                else InitiateAsyncRequest('');
            }
            function inDateChanged(sender, args) {
                DateChanged_NONPostBack(sender, args, "in");
            }
            function outDateChanged(sender, args) {
                DateChanged_NONPostBack(sender, args, "out");
            }
            JSCal.intToDateTime = function (arg) {
                arg = "" + arg;
                var _dateStr = arg.substring(0, 8);
                var _timeStr = arg.substring(8, 14);
                var dt = JSCal.intToDate(parseInt(_dateStr, 10));
                var _h = parseInt(_timeStr.substring(0, 2), 10);
                var _m = parseInt(_timeStr.substring(2, 4), 10);
                var _s = parseInt(_timeStr.substring(4, 6), 10);
                //dt = new Date();
                dt.setHours(_h, _m, _s, 0);
                //alert(dt + "\n" + _dateStr + " " + _timeStr);
                return dt;
            };
            function DateChanged_NONPostBack(sender, args, inOut) {
                var oldDate = args.get_oldDate();
                var selDate = args.get_newDate();
                if (selDate == null || selDate == oldDate) return;
                var _h = selDate.getHours() + "";
                if (_h.length == 1) _h = "0" + _h;
                var _m = selDate.getMinutes() + "";
                if (_m.length == 1) _m = "0" + _m;
                var _s = selDate.getSeconds() + "";
                if (_s.length == 1) _s = "0" + _s;
                if(inOut == "in"){
                    var selTime = parseInt("" + $("#<%=drp_dtIn.ClientID %>").val() + _h + _m + _s);
                    var minTime = parseInt($("#<%=HF_dtInMin.ClientID %>").val());
                    if (selTime < minTime) { var dtMin = JSCal.intToDateTime(minTime); sender.set_selectedDate(dtMin);} 
                    //alert("min:" + minTime + "\nmax:" + maxTime + "\nsel:" + selTime);
                }
                else if (inOut == "out") {
                    var selTime = parseInt("" + $("#<%=drp_dtOut.ClientID %>").val() + _h + _m + _s);
                    var maxTime = parseInt($("#<%=HF_dtOutMax.ClientID %>").val());
                    if (selTime > maxTime) { var dtMax = JSCal.intToDateTime(maxTime); sender.set_selectedDate(dtMax); }
                    //alert("selTime:" + selTime + "\nmaxTime:" + maxTime);
                }
            }
            function TimeView_OnClientTimeSelecting(sender, args) {
                var oldTime = args.get_oldTime();
                var selTime = args.get_newTime();
                alert("sel:" + selTime + "\nold:" + oldTime);
                //args.set_cancel(true);
            }
            function InitiateAsyncRequest(argument) {
                $find('<%=RadAjaxManager.GetCurrent(Page).ClientID%>').ajaxRequest(argument);
                return false;
            }
        </script>
        <script type="text/javascript">
            function ValidateSubmit() {
                var time;
                var _validate = true;
                $("#errorLi").hide();

                time = $find("<%= rtpLimoIn.ClientID %>").get_timeView().getTime();
                $("#rfv_rtpLimoIn").hide();
                if (time==null) {
                    $("#rfv_rtpLimoIn").css("display", "inline-block");
                    _validate = false;
                }
                time = $find("<%= rtpIn.ClientID %>").get_timeView().getTime();
                $("#rfv_rtpIn").hide();
                if (time == null) {
                    $("#rfv_rtpIn").css("display", "inline-block");
                    _validate = false;
                }
                time = $find("<%= rtpLimoOut.ClientID %>").get_timeView().getTime();
                $("#rfv_rtpLimoOut").hide();
                if (time == null) {
                    $("#rfv_rtpLimoOut").css("display", "inline-block");
                    _validate = false;
                }
                time = $find("<%= rtpOut.ClientID %>").get_timeView().getTime();
                $("#rfv_rtpOut").hide();
                if (time == null) {
                    $("#rfv_rtpOut").css("display", "inline-block");
                    _validate = false;
                }

                if (!_validate) {
                    $("#errorLi").css("display", "block");
                    $.scrollTo($("#errorLi"), 500);
                }
                return _validate;
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <script type="text/javascript">
            function set_transportType(In, Out) {
                var currIn = $("#<%=  HF_transportTypeIn.ClientID%>").val();
                In = In + "";
                if (In == "0" || In == "") In = currIn == "" ? "0" : currIn;
                var currOut = $("#<%=  HF_transportTypeOut.ClientID%>").val();
                Out = Out + "";
                if (Out == "0" || Out == "") Out = currOut == "" ? "0" : currOut;

                var transportTypePrice = In == Out && $("#<%=  drp_in_place.ClientID%>").val() == $("#<%=  drp_out_place.ClientID%>").val() ? "RoundTrip" : "OneWay";

                $(".transportTypeCont").removeClass("active");
                $(".transportTypePrice").removeClass("active");

                $("#transportTypeCont_In_" + In).addClass("active");
                $(".transportTypeCont_In_" + In + "_" + transportTypePrice).addClass("active");

                $("#transportTypeCont_Out_" + Out).addClass("active");
                $(".transportTypeCont_Out_" + Out + "_" + transportTypePrice).addClass("active");
                $("#<%=  HF_transportTypeIn.ClientID%>").val(In + "");
                $("#<%=  HF_transportTypeOut.ClientID%>").val(Out + "");
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        #contatti .smallinput input {width: 20px;}
        #contatti .rbtList label {margin-right: 15px;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManagerProxy1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <div id="contatti">
        <div class="sx" runat="server">
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <%=CurrentSource.getSysLangValue("lblYourReservation")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:UC_sx ID="UC_sx1" runat="server" />
                </div>
            </div>
        </div>
        <div class="dx reservation_details_dx" runat="server" id="Panel1">
            <asp:HiddenField ID="HF_limo_paymentUrl" runat="server" />
            <asp:HiddenField ID="HF_limo_easyShuttleSave" runat="server" />
            <asp:HiddenField ID="HF_limo_easyShuttleID" runat="server" />
            <asp:HiddenField ID="HF_limo_easyShuttleInUID" runat="server" />
            <asp:HiddenField ID="HF_limo_easyShuttleOutUID" runat="server" />
            <asp:HiddenField ID="HF_transportTypeIn" runat="server" />
            <asp:HiddenField ID="HF_transportTypeOut" runat="server" />
            <asp:HiddenField ID="HF_in_pickupPlace_lastLoaded" runat="server" Visible="false" />
            <asp:HiddenField ID="HF_out_pickupPlace_lastLoaded" runat="server" Visible="false" />
            <asp:HiddenField ID="HF_inTimeDefMin" runat="server" Value="113000" Visible="false" />
            <asp:HiddenField ID="HF_outTimeDefMax" runat="server" Value="110000" Visible="false" />
            <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
            <asp:HiddenField ID="HF_idZone" runat="server" Value="" Visible="false" />
            <asp:HiddenField ID="HF_dtMin" runat="server" Value="" Visible="false" />
            <asp:HiddenField ID="HF_dtMax" runat="server" Value="" Visible="false" />

            <asp:HiddenField ID="HF_in_pickupDetailsType" runat="server" Value="" Visible="false" />
            <asp:HiddenField ID="HF_dtInMin" runat="server" Value="" />
            <asp:HiddenField ID="HF_out_pickupDetailsType" runat="server" Value="" Visible="false" />
            <asp:HiddenField ID="HF_dtOutMax" runat="server" Value="" />
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
            </h3>
            <div class="nulla">
            </div>
            <span class="tit_sez">
                <%=CurrentSource.getSysLangValue("pdf_ArrivalAndDeparture")%></span>
            <div class="nulla">
            </div>
            <div class="box_client_booking" style="width: 545px;">
                <div style="display: none;" id="tooltip_divTimePickerHelp">
                    Use 24 hour format<br />
                    example: 1645<br />
                    (16:45 = 4:45 PM)
                </div>
                <div>
                    <div id="errorLi" class="line" style="color: red; margin-bottom: 30px; width: 550px; display: none;">
                        <h3 id="errorMsgLbl">
                            <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
                        <p id="errorMsg">
                            <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
                        </p>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line mode_in place" style="padding-bottom: 5px; border-bottom-color: #333366;">
                        <div id="Div1" class="left">
                            <label class="desc" style="border: none; margin: 0;">
                                <%= "<h3>Arrival to " + CurrentSource.loc_cityTitle(RentalInRome.reservationarea.resUtils.currCity, 2, "Rome") + "</h3>"%>
                            </label>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line mode_in place">
                        <div id="Div20" class="left">
                            <label>
                                <%=CurrentSource.getSysLangValue("lblArrivalDate")%>
                            </label>
                            <div>
                                <asp:DropDownList ID="drp_dtLimoIn" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpInMode_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="Div18" class="left">
                            <label>
                                <%=CurrentSource.getSysLangValue("lblArrivaltime")%><a style="float: none; display: inline-block;" class="infoBtn ico_tooltip" ttp="divTimePickerHelp"></a>
                                <span id="rfv_rtpLimoIn" class="alertErrorSmall" style="display: none; float: none; margin-left: 15px;">* Fill your arrival time</span>
                            </label>
                            <div>
                                <telerik:RadTimePicker ID="rtpLimoIn" runat="server" Width="80" OnSelectedDateChanged="rtpLimoIn_SelectedDateChanged" TimeView-TimeFormat="HH:mm" TimeView-Interval="00:30:00" TimeView-Columns="6" TimeView-HeaderText="For more specific time use the input on the left">
                                    <ClientEvents OnDateSelected="DateChanged_PostBack" />
                                    <DateInput runat="server" ClientEvents-OnError="OnError" />
                                </telerik:RadTimePicker>
                            </div>
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
                                <asp:DropDownList ID="drp_in_place" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpInMode_SelectedIndexChanged" Style="width: auto;">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="line">
                        <div class="left">
                            <label id="lbl_in_placeDett" runat="server">lbl_in_placeDett </label>
                            <div>
                                <asp:TextBox ID="txt_in_pickupDetails" runat="server" CssClass="limoLarge"></asp:TextBox>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="line mode_in place" style="border-bottom:none; margin-bottom:0; padding-bottom:0;">
                        <div id="Div23" class="left">
                            <label>Transport type to the apartment </label>
                            <div>
                                <br />
                                <asp:RadioButtonList ID="rbtList_inPoint_transportType" runat="server" CssClass="rbtList smallinput" RepeatColumns="4" AutoPostBack="true" OnSelectedIndexChanged="drpInMode_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div id="pnl_in_isRequested" runat="server" class="left">
                            <div>
                                <asp:ListView runat="server" ID="LVtransportTypeIn" OnItemCommand="LVtransportType_ItemCommand">
                                    <ItemTemplate>
                                        <li class="transportTypeCont" id="transportTypeCont_In_<%# Eval("id") %>">
                                            <div class="lasx" style="width: 190px; background-image: url(/<%# ("" + Eval("code")).Contains("van") ? "images/arearis_transf_van.jpg" : "images/arearis_transf_mercedes.jpg"%>);">
                                                <span class="titauto1" style="margin-top: 10px;">
                                                    <%# Eval("title") %>
                                                </span>
                                                <span class="titauto2">
                                                    <%# "min " + Eval("numGuestMin") + " pers"%><br />
                                                    <%# "max " + Eval("numGuestMax") + " pers"%>
                                                </span>
                                                <span class="CreditCardReq" style="display: none;">Solo carta di credito</span>
                                            </div>
                                            <div class="ladx smallinput" style="width: 170px;">
                                                <span class="ladxtx" style="font-size: 14px; line-height: 20px; margin-top: 0;">
                                                    <asp:LinkButton ID="lnkOneWay" runat="server" CommandName="inOneWay">
                                                        <%# " OneWay: " + Eval("prTotalOneWay").objToDecimal().ToString("N2") + " &euro;"%>
                                                    </asp:LinkButton>
                                                    <span class="nulla"></span>
                                                    <asp:LinkButton ID="lnkRoundTrip" runat="server" CommandName="inRoundTrip">
                                                        <%# " RoundTrip: " + (Eval("prTotalRoundTrip").objToDecimal()*2).ToString("N2") + " &euro;"%>
                                                    </asp:LinkButton>
                                                </span>
                                            </div>
                                            <div class="nulla">
                                            </div>
                                        </li>
                                        <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label1" Text='<%# Eval("tripDirection") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label2" Text='<%# Eval("pax") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label3" Text='<%# Eval("prSingleOneWay") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label4" Text='<%# Eval("prTotalOneWay") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label10" Text='<%# Eval("prSingleRoundTrip") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label11" Text='<%# Eval("prTotalRoundTrip") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbl_code" Text='<%# Eval("code") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label6" Text='<%# Eval("title") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label7" Text='<%# Eval("imgPreview") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label8" Text='<%# Eval("numGuestMin") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label9" Text='<%# Eval("numGuestMax") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <LayoutTemplate>
                                        <ul class="listaauto">
                                            <li id="itemPlaceholder" runat="server" />
                                        </ul>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <div class="nulla">
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>

                    <div id="pnl_luggagesCont" runat="server" class="line" style="border-bottom:none; margin-bottom:none;">
                        <div style="padding-bottom: 5px; border-bottom-color: #333366;" class="line mode_in place">
                            <div class="left">
                                <label style="border: none; margin: 0;" class="desc">
                                    <h3>
                                        Luggages</h3>
                                </label>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        
                        <div class="luggage small">
                            <span class="luggageType">Small</span> 
                            <span class="luggageDim"><strong>Max. Dim.:</strong> 40 x 55 x 20cm</span>
                            <div class="nulla">
                            </div>
                            <strong class="luggageQtaLbl"><%=contUtils.getLabel("lblQuantity")%>:</strong>
                            <asp:DropDownList ID="drp_limo_num_case_s" runat="server">
                                <asp:ListItem Value="0"></asp:ListItem>
                                <asp:ListItem Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                                <asp:ListItem Value="3"></asp:ListItem>
                                <asp:ListItem Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="luggage medium">
                            <span class="luggageType">Medium</span> <span class="luggageDim"><strong>Max. Dim.:</strong>
                                45 x 64 x 24cm</span>
                            <div class="nulla">
                            </div>
                            <strong class="luggageQtaLbl"><%=contUtils.getLabel("lblQuantity")%>:</strong>
                            <asp:DropDownList ID="drp_limo_num_case_m" runat="server">
                                <asp:ListItem Value="0"></asp:ListItem>
                                <asp:ListItem Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                                <asp:ListItem Value="3"></asp:ListItem>
                                <asp:ListItem Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="luggage big">
                            <span class="luggageType">Large</span> <span class="luggageDim"><strong>Max. Dim.:</strong>
                                50 x 74 x 29cm</span>
                            <div class="nulla">
                            </div>
                            <strong class="luggageQtaLbl"><%=contUtils.getLabel("lblQuantity")%>:</strong>
                            <asp:DropDownList ID="drp_limo_num_case_l" runat="server">
                                <asp:ListItem Value="0"></asp:ListItem>
                                <asp:ListItem Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                                <asp:ListItem Value="3"></asp:ListItem>
                                <asp:ListItem Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>

                    <div class="line" style="padding-bottom: 5px; border-bottom-color: #333366; padding-top: 5px; border-top-color: #333366;">
                    </div>
                    <div class="line mode_in date">
                        <div id="txt_dtBirth_cont" class="left">
                            <label>
                                Check-In date
                            </label>
                            <div>
                                <asp:DropDownList ID="drp_dtIn" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="Div8" class="left">
                            <label>
                                <%=CurrentSource.getSysLangValue("pdf_orario_arrivo_appartamento", "Orario d’arrivo all’appartamento")%><a style="float: none; display: inline-block;" class="infoBtn ico_tooltip" ttp="divTimePickerHelp"></a>
                                <span id="rfv_rtpIn" class="alertErrorSmall" style="display: none; float: none; margin-left: 15px;">* Fill your <%=CurrentSource.getSysLangValue("pdf_orario_arrivo_appartamento", "Orario d’arrivo all’appartamento")%></span>
                            </label>
                            <div>
                                <telerik:RadTimePicker ID="rtpIn" runat="server" Width="80" TimeView-TimeFormat="HH:mm" TimeView-Interval="00:30:00" TimeView-Columns="6" TimeView-HeaderText="For more specific time use the input on the left">
                                    <ClientEvents OnDateSelected="inDateChanged" />
                                    <DateInput ID="DateInput1" runat="server" ClientEvents-OnError="OnError" />
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div id="pnl_inTimeEstimated" class="left" runat="server" visible="false">
                            <asp:HiddenField ID="HF_inTimeEstimated" runat="server" />
                            <asp:HiddenField ID="HF_inTimeEstimatedFormat" runat="server" Value="* estimated time of arrival at the apartment is at #time# o'clock #date#" />
                            <label>
                                <%=HF_inTimeEstimated.Value%>
                            </label>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line" style="padding-bottom: 5px; border-bottom-color: #333366; padding-top: 5px; border-top-color: #333366;">
                        <div id="Div21" class="left">
                            <label>
                                <%= CurrentSource.getSysLangValue("terms_checkin")%>
                            </label>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line mode_out place" style="padding-bottom: 5px; border-bottom-color: #333366;">
                        <div id="Div9" class="left">
                            <label class="desc" style="border: none; margin: 0;">
                                <%= "<h3>Departure from " + CurrentSource.loc_cityTitle(RentalInRome.reservationarea.resUtils.currCity, 2, "Rome") + "</h3>"%>
                            </label>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line mode_in place">
                        <div id="Div26" class="left">
                            <label>
                                Departure date
                            </label>
                            <div>
                                <asp:DropDownList ID="drp_dtLimoOut" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOutMode_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="Div27" class="left">
                            <label>
                                <%= CurrentSource.getSysLangValue("lblDepartureTimeAirPortTrain")%><a style="float: none; display: inline-block;" class="infoBtn ico_tooltip" ttp="divTimePickerHelp"></a>
                                <span id="rfv_rtpLimoOut" class="alertErrorSmall" style="display: none; float: none; margin-left: 15px;">* Fill your Departure time</span>
                            </label>
                            <div>
                                <telerik:RadTimePicker ID="rtpLimoOut" runat="server" Width="80" OnSelectedDateChanged="rtpLimoOut_SelectedDateChanged" TimeView-TimeFormat="HH:mm" TimeView-Interval="00:30:00" TimeView-Columns="6" TimeView-HeaderText="For more specific time use the input on the left">
                                    <ClientEvents OnDateSelected="DateChanged_PostBack" />
                                    <DateInput ID="DateInput3" runat="server" ClientEvents-OnError="OnError" />
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line mode_out place">
                        <div id="drp_out_place_cont" class="left">
                            <label>
                                Departure place
                            </label>
                            <div>
                                <asp:DropDownList ID="drp_out_place" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOutMode_SelectedIndexChanged" Style="width: auto;">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="line">
                        <div class="left">
                            <label id="lbl_out_placeDett" runat="server"> </label>
                            <div>
                                <asp:TextBox ID="txt_out_pickupDetails" runat="server" CssClass="limoLarge"></asp:TextBox>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <asp:PlaceHolder ID="PH_outAir" runat="server">
                    <div class="line">
                        <div id="Div19" class="left">
                            <label>
                                Flight type
                            </label>
                            <div>
                                <asp:DropDownList ID="drp_out_air_detailsType" runat="server" CssClass="limoLarge" AutoPostBack="true" OnSelectedIndexChanged="drpOutMode_SelectedIndexChanged">
                                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="National flight" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="International flight" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    </asp:PlaceHolder>
                    <div class="line mode_in place">
                        <div id="Div25" class="left">
                            <label>
                                Transport type from the apartment
                            </label>
                            <div>
                                <br />
                                <asp:RadioButtonList ID="rbtList_outPoint_transportType" runat="server" CssClass="rbtList smallinput" RepeatColumns="4" AutoPostBack="true" OnSelectedIndexChanged="drpOutMode_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div id="pnl_out_isRequested" runat="server" class="left">
                            <div>
                                <asp:ListView runat="server" ID="LVtransportTypeOut" OnItemCommand="LVtransportType_ItemCommand">
                                    <ItemTemplate>
                                        <li class="transportTypeCont" id="transportTypeCont_Out_<%# Eval("id") %>">
                                            <div class="lasx" style="width: 190px; background-image: url(/<%# ("" + Eval("code")).Contains("van") ? "images/arearis_transf_van.jpg" : "images/arearis_transf_mercedes.jpg"%>);">
                                                <span class="titauto1" style="margin-top: 10px;">
                                                    <%# Eval("title") %>
                                                </span>
                                                <span class="titauto2">
                                                    <%# "min " + Eval("numGuestMin") + " pers"%><br />
                                                    <%# "max " + Eval("numGuestMax") + " pers"%>
                                                </span>
                                                <span class="CreditCardReq" style="display: none;">Solo carta di credito</span>
                                            </div>
                                            <div class="ladx smallinput" style="width: 170px;">
                                                <span class="ladxtx" style="font-size: 14px; line-height: 20px; margin-top: 0;">
                                                   <asp:LinkButton ID="lnkOneWay" runat="server" CommandName="outOneWay">
                                                        <%# " OneWay: " + Eval("prTotalOneWay").objToDecimal().ToString("N2") + " &euro;"%>
                                                    </asp:LinkButton>
                                                    <span class="nulla"></span>
                                                    <asp:LinkButton ID="lnkRoundTrip" runat="server" CommandName="outRoundTrip">
                                                        <%# " RoundTrip: " + (Eval("prTotalRoundTrip").objToDecimal()*2).ToString("N2") + " &euro;"%>
                                                    </asp:LinkButton>
                                                </span>
                                            </div>
                                            <div class="nulla">
                                            </div>
                                        </li>
                                        <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label1" Text='<%# Eval("tripDirection") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label2" Text='<%# Eval("pax") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label3" Text='<%# Eval("prSingleOneWay") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label4" Text='<%# Eval("prTotalOneWay") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label10" Text='<%# Eval("prSingleRoundTrip") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label11" Text='<%# Eval("prTotalRoundTrip") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbl_code" Text='<%# Eval("code") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label6" Text='<%# Eval("title") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label7" Text='<%# Eval("imgPreview") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label8" Text='<%# Eval("numGuestMin") %>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="Label9" Text='<%# Eval("numGuestMax") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <LayoutTemplate>
                                        <ul class="listaauto">
                                            <li id="itemPlaceholder" runat="server" />
                                        </ul>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <div class="nulla">
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line" style="padding-bottom: 5px; border-bottom-color: #333366; padding-top: 5px; border-top-color: #333366;">
                        <div id="Div24" class="left">
                            <label>
                                <%= CurrentSource.getSysLangValue("terms_checkout")%>
                            </label>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line mode_out date">
                        <div id="Div16" class="left">
                            <label>
                                Check-Out date
                            </label>
                            <div>
                                <asp:DropDownList ID="drp_dtOut" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="Div17" class="left">
                            <label>
                                <%= CurrentSource.getSysLangValue("lblAptCheckOutTime")%><a style="float: none; display: inline-block;" class="infoBtn ico_tooltip" ttp="divTimePickerHelp"></a>
                                <span id="rfv_rtpOut" class="alertErrorSmall" style="display: none; float: none; margin-left: 15px;">* Fill your Check-Out time</span>
                            </label>
                            <div>
                                <telerik:RadTimePicker ID="rtpOut" runat="server" Width="80" TimeView-TimeFormat="HH:mm" TimeView-Interval="00:30:00" TimeView-Columns="6" TimeView-HeaderText="For more specific time use the input on the left">
                                    <ClientEvents OnDateSelected="outDateChanged" />
                                    <DateInput ID="DateInput2" runat="server" ClientEvents-OnError="OnError" />
                                </telerik:RadTimePicker>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div id="pnl_outTimeEstimated" class="left" runat="server" visible="false">
                            <asp:HiddenField ID="HF_outTimeEstimated" runat="server" />
                            <asp:HiddenField ID="HF_outTimeEstimatedFormat" runat="server" Value="* estimated Check-Out time from the apartment is at #time# o'clock #date#" />
                            <label>
                                <%=HF_outTimeEstimated.Value%>
                            </label>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div id="pnl_pickupRequestSummaryHeader" runat="server" class="line" style="padding-bottom: 5px; border-bottom-color: #333366;">
                        <div id="Div31" class="left">
                            <label class="desc" style="border: none; margin: 0;">
                                <%= "<h3>Pickup service request</h3>"%>
                            </label>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="line" id="pnl_pickupRequestSummary" runat="server">
                        <div id="Div22" class="left">
                            <table cellpadding="0" cellspacing="0" style="width: 500px;">
                                <tr>
                                    <th>From</th>
                                    <th>To</th>
                                    <th>Vehicle</th>
                                    <th>Price</th>
                                </tr>
                                <asp:Literal ID="ltr_pickupRequestSummary" runat="server"></asp:Literal>
                            </table>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <asp:LinkButton ID="lnk_saveInOutData" CssClass="btn bonifico" runat="server" OnClick="lnk_save_Click" OnClientClick="return ValidateSubmit();"><span>Submit</span></asp:LinkButton>
                    <asp:Label ID="lbl_ok" runat="server" Text="OK" Style="margin: 10px 5px 5px 0pt; display: block; float: left; font-size: 20px; font-weight: bold; color: green;" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div id="ltrTmp_shuttleRequest_cont" style="display: none;">
                <asp:Literal ID="ltrTmp_shuttleRequest" runat="server"></asp:Literal>
            </div>
            <div id="ltrTmp_shuttleResponse_cont" style="display: none;">
                <asp:Literal ID="ltrTmp_shuttleResponse" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
