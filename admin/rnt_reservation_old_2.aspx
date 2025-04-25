<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_reservation_old_2.aspx.cs" Inherits="RentalInRome.admin.rnt_reservation_old_2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        select
        {
            font-family: verdana;
            font-size: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal ID="ltr_tooltip_template" runat="server" Visible="false">
	
	
	
    </asp:Literal>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="1" runat="server" />
            <h1 class="titolo_main">
                Gestione Prenotazioni e Disponibilità delle Strutture</h1>
            <!-- INIZIO MAIN LINE -->
            <div class="mainline">
                <!-- BOX 1 -->
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <asp:Literal ID="ltr_empty" runat="server" Visible="false">IG</asp:Literal>
                        <asp:Literal ID="ltr_filter_estates" runat="server" Visible="false"></asp:Literal>
                        <asp:HiddenField runat="server" ID="HF_pid_estate" Value="0" />
                        <asp:HiddenField ID="HF_unique" Value="" runat="server" />
                        <table border="0" cellpadding="0" cellspacing="0" style="margin-bottom: 20px;">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px;">
                                        <tr>
                                            <td>
                                                <label class="barraTop">
                                                    Visualizza:
                                                    <span>
                                                        <select id="drp_dateMode" onchange="changeDateMode(this.value)">
                                                            <option value="1">Periodo</option>
                                                            <option value="2">Mese</option>
                                                        </select>
                                                    </span>
                                                    <span style="display: none;">
                                                        <input type="radio" id="rb_dateMode_period" onchange="changeDateMode('1')">
                                                        <label>
                                                            Periodo</label>
                                                        <input type="radio" id="rb_dateMode_month" onchange="changeDateMode('2')">
                                                        <label>
                                                            Mese</label>
                                                    </span>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="pnl_flt_month" style="display: none;">
                                                <asp:RadioButtonList ID="rbl_month" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                                </asp:RadioButtonList>
                                            </td>
                                            <td id="pnl_flt_period" style="display: none;">
                                                <table class="inp">
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                dal:</label>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txt_dtStart_<%= Unique %>" style="width: 120px" />
                                                            <img src="" id="del_date_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                al (compreso):</label>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txt_dtEnd_<%= Unique %>" style="width: 120px" />
                                                            <img src="" id="del_date_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:HiddenField ID="HF_dtStart" runat="server" />
                                                <asp:HiddenField ID="HF_dtEnd" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="salvataggio">
                                                    <div class="bottom_salva">
                                                        <a href="javascript:refreshDates();">
                                                            <span>Aggiorna date</span></a>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                con stato:&nbsp;
                                                <asp:DropDownList ID="drp_flt_state" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="HF_dateMode" runat="server" Value="1" />
                                    <script type="text/javascript">
                                        function refreshDates() {
                                            window.location = "rnt_reservation.aspx?dtStart=" + $("#<%=HF_dtStart.ClientID %>").val() + "&dtEnd=" + $("#<%=HF_dtEnd.ClientID %>").val();
                                        }
                                        function closeFromForm() {
                                            Shadowbox.close();
                                            refreshDates();
                                        }
                                        function changeDateMode(_dateMode) {
                                            $get("<%= HF_dateMode.ClientID %>").value = _dateMode;
                                            $get("drp_dateMode").options[parseInt(_dateMode) - 1].selected = true;
                                            //$get("rb_dateMode_month").checked = _dateMode == "2";
                                            $get("pnl_flt_month").style.display = _dateMode == "1" ? "none" : "";
                                            $get("pnl_flt_period").style.display = _dateMode == "1" ? "" : "none";
                                        }
                                    </script>
                                </td>
                                <td id="pnl_flt_estate_zone" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                                <tr>
                                                    <td>
                                                        <label class="barraTop">
                                                            Zone:</label>
                                                        <asp:ListBox ID="lbx_zone" runat="server" SelectionMode="Multiple" Rows="10" Style="min-width: 180px;"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                                <tr>
                                                    <td>
                                                        <label class="barraTop">
                                                            Appartamenti:</label>
                                                        <asp:ListBox ID="lbx_estate" runat="server" SelectionMode="Multiple" Rows="10" Style="min-width: 180px;"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td id="pnl_flt_prop" runat="server">
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px;">
                                        <tr>
                                            <td colspan="2">
                                                <label class="barraTop">
                                                    Filtro caratteristiche:</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Min. #.Posti Letto
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_min_num_persons_max" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Min. #.Camere
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_min_num_rooms_bed" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Min. #.Stelle
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_min_importance_stars" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Aria Condizionata
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_has_air_condition" runat="server">
                                                    <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                In Esclusiva
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_is_exclusive" runat="server">
                                                    <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Attico
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_is_loft" runat="server">
                                                    <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Internet
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_has_internet" runat="server">
                                                    <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Adsl o Wifi" Value="2or3"></asp:ListItem>
                                                    <asp:ListItem Text="solo Adsl" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="solo Wifi" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Adsl e Wifi" Value="2and3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="Tr1" runat="server" visible="false">
                                            <td class="td_title">
                                                Solo Con Internet
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList6" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td id="Td1" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                                <tr>
                                                    <td>
                                                        <label class="barraTop">
                                                            Codice:</label>
                                                        <asp:TextBox ID="txt_code" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="salvataggio">
                                                            <div class="bottom_salva">
                                                                <asp:LinkButton ID="lnk_searchCode" runat="server" OnClick="lnk_searchCode_Click"><span>cerca il codice</span></asp:LinkButton>
                                                            </div>
                                                            <div class="nulla">
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_codeError" runat="server" Text="Il codice fornito inesistente." Visible="false" Style="color: red; font-size: 15px;"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:LinqDataSource ID="LDS_availability" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_LK_RESERVATION_STATEs" OrderBy="title" Where="title != @title && id!=3 && type==1">
                                        <WhereParameters>
                                            <asp:Parameter Name="title" DefaultValue="" Type="String" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <asp:ListView ID="LV_availability" DataSourceID="LDS_availability" runat="server" EnableViewState="false">
                                        <ItemTemplate>
                                            <td>
                                                <span class="<%# (""+Eval("css_class")).ToLower() %>" style="width: 30px;"></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("abbr") + "&nbsp;-&nbsp;" + Eval("title")%></span>
                                            </td>
                                            <td style="width: 10px;">
                                            </td>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            empty
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span class="res_free" style="width: 30px;"></span>
                                                    </td>
                                                    <td>
                                                        <span>Disponibile</span>
                                                    </td>
                                                    <td style="width: 10px;">
                                                    </td>
                                                    <td id="itemPlaceholder" runat="server" />
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                    <div id="resList_cont">
                                        <table cellspacing="0" cellpadding="0" border="1" style="font-size: 10px;">
                                            <tbody id="resList_body">
                                                <tr>
                                                    <td class="nomiTab">
                                                    </td>
                                                    <td id="resList_tabDays">
                                                        <%=ltr_dayList.Text%>
                                                    </td>
                                                </tr>
                                                <%=ltr_estateList.Text %>
                                            </tbody>
                                        </table>
                                    </div>
                                    <asp:Literal ID="ltr_dayList" runat="server" Visible="false"></asp:Literal>
                                    <asp:Literal ID="ltr_estateList" runat="server" Visible="false"></asp:Literal>
                                    <script type="text/javascript">

                                        RNT = {};
                                        RNT.EstateOptions = {
                                            id: 0,
                                            code: "",
                                            pid_zone: 0,
                                            pid_state: -1,
                                            stateList: new Array(),
                                            is_exclusive: -1,
                                            is_loft: -1,
                                            num_persons_max: -1,
                                            num_rooms_bed: -1,
                                            importance_stars: -1,
                                            has_air_condition: -1,
                                            has_adsl: -1,
                                            has_wifi: -1,
                                            //has_air_condition: -1,
                                            //has_air_condition: -1,
                                            scheduleStr: "",
                                            scheduleCont: "",
                                            resList: new Array(),
                                            is_nd: false,
                                            is_opz: false,
                                            is_can: false,
                                            is_prt: false,
                                            is_mv: false,
                                            is_free: false,
                                            onClose: undefined
                                        };
                                        RNT.EstateList = function (opt) {
                                            this.ReservationList = new Array();
                                            this.Items = new Array();
                                            this.FilteredItems = new Array();
                                            this.Opt = {};
                                            var obj = this;
                                            obj.Opt = $.extend(obj.Opt, RNT.EstateListOptions, opt);
                                            obj.daysCount = Math.ceil((Calendar.intToDate(obj.Opt.dtEnd).getTime() - Calendar.intToDate(obj.Opt.dtStart).getTime()) / (1000 * 60 * 60 * 24));
                                            this.dtStartCont = "<%=HF_dtStart.ClientID%>";
                                            this.dtEndCont = "<%=HF_dtEnd.ClientID%>";
                                            this.zoneListCont = "<%=lbx_zone.ClientID%>";
                                            this.estateListCont = "<%=lbx_estate.ClientID%>";
                                            this.state_Cont = "<%=drp_flt_state.ClientID%>";
                                            this.is_exclusive_Cont = "<%=drp_is_exclusive.ClientID%>";
                                            this.is_loft_Cont = "<%=drp_is_loft.ClientID%>";
                                            this.num_persons_max_Cont = "<%=drp_min_num_persons_max.ClientID%>";
                                            this.num_rooms_bed_Cont = "<%=drp_min_num_rooms_bed.ClientID%>";
                                            this.importance_stars_Cont = "<%=drp_min_importance_stars.ClientID%>";
                                            this.has_air_condition_Cont = "<%=drp_has_air_condition.ClientID%>";
                                            this.has_internet_Cont = "<%=drp_has_internet.ClientID%>";
                                            this.filter_estateProps = function () {
                                                if (obj.FilteredItems.length == 0) {
                                                    $("#resList_loading").hide();
                                                    $("#resList_alert").css("display", "");
                                                    $("#resList_alert_txt").html("Zona/e senza appartamenti!");
                                                    $(".estate_row").hide();
                                                    return;
                                                }
                                                for (var i = 0; i < obj.Items.length; i++) { // per ogni apt
                                                    var _est = obj.Items[i];
                                                    if ($.inArray(_est.Opt.id, obj.FilteredItems) == -1) {
                                                        $("#" + _est.Opt.id + "_trCont").hide();
                                                        continue;
                                                    }
                                                    if (1 == 1
														&& ($("#" + obj.state_Cont).val() == "-1" || $.inArray(parseInt($("#" + obj.state_Cont).val(), 10), _est.Opt.stateList) != -1)
														&& ($("#" + obj.is_exclusive_Cont).val() == "-1" || $("#" + obj.is_exclusive_Cont).val() == "" + _est.Opt.is_exclusive)
														&& ($("#" + obj.is_loft_Cont).val() == "-1" || $("#" + obj.is_loft_Cont).val() == "" + _est.Opt.is_loft)
														&& ($("#" + obj.num_persons_max_Cont).val() == "-1" || parseInt($("#" + obj.num_persons_max_Cont).val(), 10) <= _est.Opt.num_persons_max)
														&& ($("#" + obj.num_rooms_bed_Cont).val() == "-1" || parseInt($("#" + obj.num_rooms_bed_Cont).val()) <= _est.Opt.num_rooms_bed)
														&& ($("#" + obj.importance_stars_Cont).val() == "-1" || parseInt($("#" + obj.importance_stars_Cont).val()) <= _est.Opt.importance_stars)
														&& ($("#" + obj.has_air_condition_Cont).val() == "-1" || $("#" + obj.has_air_condition_Cont).val() == "" + _est.Opt.has_air_condition)
														&& ($("#" + obj.has_internet_Cont).val() == "-1"
															|| ($("#" + obj.has_internet_Cont).val() == "2or3" && (_est.Opt.has_adsl == 1 || _est.Opt.has_wifi == 1))
															|| ($("#" + obj.has_internet_Cont).val() == "2" && _est.Opt.has_adsl == 1)
															|| ($("#" + obj.has_internet_Cont).val() == "3" && _est.Opt.has_wifi == 1)
															|| ($("#" + obj.has_internet_Cont).val() == "2and3" && _est.Opt.has_adsl == 1 && _est.Opt.has_wifi == 1)
															)
														) {
                                                        $("#" + _est.Opt.id + "_trCont").show();
                                                    }
                                                    else {
                                                        $("#" + _est.Opt.id + "_trCont").hide();
                                                    }
                                                }
                                                $("#resList_loading").hide();
                                                $("#resList_alert").hide();
                                            }
                                            this.filter_zoneList = function () {
                                                $("#resList_loading").show();
                                                obj.FilteredItems = new Array();
                                                var _zoneEstList = new Array();
                                                var _zoneList = $("#" + obj.zoneListCont).val() || []; // zone selezionate al momento
                                                if (_zoneList.length == 0) {
                                                    $("#resList_loading").hide();
                                                    $("#resList_alert").show();
                                                    $("#resList_alert_txt").html("Selezionare la zona!");
                                                    return;
                                                }
                                                var _estateList = $("#" + obj.estateListCont).val() || []; // apt selezionati al momento
                                                $("#" + obj.estateListCont + " option").remove(); // rimuovi tutti apt dal drp
                                                for (var i = 0; i < obj.Items.length; i++) { // per ogni apt
                                                    var _est = obj.Items[i];
                                                    // se apt in zone selezionate aggiungi agli apt di zone e in drp apt
                                                    if (_zoneList.length == 0 || $.inArray("" + _est.Opt.pid_zone, _zoneList) != -1) {
                                                        $("#" + obj.estateListCont).append("<option value='" + _est.Opt.id + "'>" + _est.Opt.code + "</option>")
                                                        _zoneEstList.push(_est.Opt.id);
                                                    }
                                                }
                                                // se drp apt vuoto visualizza -Zona/e senza appartamenti-
                                                if ($("#" + obj.estateListCont + " option").length == 0) {
                                                    $("#resList_loading").hide();
                                                    $("#resList_alert").css("display", "");
                                                    $("#resList_alert_txt").html("Zona/e senza appartamenti!");
                                                    $("#" + obj.estateListCont).append("<option value='-1'>-Zona/e senza appartamenti-</option>");
                                                    $(".estate_row").hide();
                                                    return;
                                                }
                                                obj.FilteredItems = _zoneEstList;
                                                obj.filter_estateProps();
                                            }
                                            this.filter_estateList = function () {
                                                $("#resList_loading").show();
                                                obj.FilteredItems = new Array();
                                                var _estateList = $("#" + obj.estateListCont).val() || [];
                                                if (_estateList.length > 0) {
                                                    for (var i = 0; i < _estateList.length; i++) {
                                                        obj.FilteredItems.push(parseInt(_estateList[i]));
                                                    }
                                                    obj.filter_estateProps();
                                                }
                                                else
                                                    obj.filter_zoneList();
                                            }
                                            this.fillSchedules = function () {
                                                for (var i = 0; i < obj.Items.length; i++) {
                                                    var _est = obj.Items[i];
                                                    _est.fillSchedule(obj.Opt.dtStart, obj.daysCount);
                                                }
                                            }
                                        }
                                        RNT.Estate = function (opt) {
                                            this.Opt = {};
                                            var obj = this;
                                            obj.Opt = $.extend(obj.Opt, RNT.EstateOptions, opt);
                                        }
                                        function RNT_setSelection(IdEstate, dtDate, dtPart) {
                                            $(".dtSel").removeClass("dtSel");
                                            $(".openSel").removeClass("openSel");
                                            $(".closeSel").removeClass("closeSel");
                                            var _dt = Calendar.intToDate(dtDate + 1);
                                            var _dtAfter = "" + Calendar.dateToInt(_dt);
                                            _dt = Calendar.intToDate(dtDate - 1);
                                            var _dtBefore = "" + Calendar.dateToInt(_dt);
                                            alert("dt:" + dtDate + ", dtB:" + _dtBefore, +" dtA:" + _dtAfter + ", IdEstate:" + IdEstate + ", dtPart:" + dtPart);
                                            if (dtPart == 1) {
                                                $("td[IdEstate='" + IdEstate + "'][dtDate='" + _dtAfter + "'][dtPart='" + 2 + "']").addClass("dtSel openSel");
                                                $("td[IdEstate='" + IdEstate + "'][dtDate='" + dtDate + "'][dtPart='" + dtPart + "']").addClass("dtSel closeSel");
                                            } else {
                                                $("td[IdEstate='" + IdEstate + "'][dtDate='" + _dtBefore + "'][dtPart='" + dtPart + "']").addClass("dtSel openSel");
                                                $("td[IdEstate='" + IdEstate + "'][dtDate='" + dtDate + "'][dtPart='" + 1 + "']").addClass("dtSel closeSel");
                                            }
                                        }
                                        function RNT_openSelection(elm) {
                                            var dtDate = parseInt($(elm).attr("dtDate"));
                                            var dtPart = parseInt($(elm).attr("dtPart"));
                                            var IdEstate = parseInt($(elm).attr("IdEstate"));
                                            var IdRes = parseInt($(elm).attr("IdRes"));
                                            var dtStart = $("#<%=HF_dtStart.ClientID %>").val();
                                            var dtEnd = $("#<%=HF_dtEnd.ClientID %>").val();
                                            var _url = "rnt_reservation_form.aspx?dtStart=" + dtStart + "&dtEnd=" + dtEnd + "&dtDate=" + dtDate + "&dtPart=" + dtPart + "&IdEstate=" + IdEstate + "&IdRes=" + IdRes;
                                            //alert(_url);
                                            OpenShadowbox(_url, 800, 0);
                                        }
                                    </script>
                                    <script type="text/javascript">
									    <%=ltr_script.Text %>
                                    </script>
                                    <asp:Literal ID="ltr_script" Visible="false" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Literal ID="ltr_tooltip_cont" runat="server"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
     var _JSCal_Range_;
    function setCal_<%= Unique %>() {
        _JSCal_Range_= new JSCal.Range({ dtFormat: "d MM yy", startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endTrigger: "#endCalTrigger_<%= Unique %>", changeMonth: true, changeYear: true });
    }
       function ReloadItems(id) {
            Shadowbox.close();
            __doPostBack('lnk_new', id);
        }
    </script>
</asp:Content>
