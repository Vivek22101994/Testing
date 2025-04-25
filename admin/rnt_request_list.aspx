<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true"
    CodeBehind="rnt_request_list.aspx.cs" Inherits="RentalInRome.admin.rnt_request_list" %>

<%@ Register Src="uc/UC_loader_list.ascx" TagName="UC_loader_list" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function refreshDates() {
            window.location = "rnt_request_details.aspx?id=<%=HF_id.Value %>";
        }
        function RNT_openSelection(IdRes) {
            var _url = "rnt_reservation_form.aspx?IdRes=" + IdRes;
            //alert(_url);
            OpenShadowbox(_url, 800, 0);
        }
    </script>
    <script type="text/javascript">

        function showFilter() {
            $get("lnk_showfilt").style.display = "none";
            $get("lnk_hidefilt").style.display = "";
            $get("tbl_filter").style.display = "";
            $get("<%=HF_is_filtered.ClientID %>").value = "true";
        }
        function hideFilter() {
            $get("lnk_showfilt").style.display = "";
            $get("lnk_hidefilt").style.display = "none";
            $get("tbl_filter").style.display = "none";
            $get("<%=HF_is_filtered.ClientID %>").value = "false";
        }
        function Filter() {

            var _filter = "";
            var _value = "";
            _value = $("#<%= drp_is_deleted.ClientID%>").lenght != 0 ? $("#<%= drp_is_deleted.ClientID%>").val() : "0";
            if (_value != "")
                _filter += "&is_del=" + _value;
            _value = $("#<%= drp_flt_pid_creator.ClientID%>").val();
            if (_value != "")
                _filter += "&creator=" + _value;
            _value = $("#<%= drp_flt_related.ClientID%>").val();
            if (_value != "")
                _filter += "&rel=" + _value;
            _value = $("#<%= drp_lang.ClientID%>").val();
            if (_value != "")
                _filter += "&lang=" + _value;
            _value = $("#<%= drp_city.ClientID%>").val();
            if (_value != "")
                _filter += "&city=" + _value;
            _value = $("#<%= drp_country.ClientID%>").val();
            if (_value != "")
                _filter += "&country=" + _value;
            _value = $("#<%= drp_account.ClientID%>").val();
            if (_value != "")
                _filter += "&account=" + _value;
            _value = $("#<%= txt_name_full.ClientID%>").val();
            if (_value != "")
                _filter += "&name_full=" + _value;
            _value = $("#<%= txt_email.ClientID%>").val();
            if (_value != "")
                _filter += "&email=" + _value;
            _value = $("#<%= txt_request_ip.ClientID%>").val();
            if (_value != "")
                _filter += "&ip=" + _value;
            _value = $("#<%= txt_IdAdMedia.ClientID%>").val();
            if (_value != "")
                _filter += "&IdAdMedia=" + _value;
            _value = $("#<%= txt_IdLink.ClientID%>").val();
            if (_value != "")
                _filter += "&IdLink=" + _value;


            _value = $("#<%= HF_date_start_from.ClientID%>").val();
            if (_value != "")
                _filter += "&dtsf=" + _value;
            _value = $("#<%= HF_date_start_to.ClientID%>").val();
            if (_value != "")
                _filter += "&dtst=" + _value;

            _value = $("#<%= HF_date_end_from.ClientID%>").val();
            if (_value != "")
                _filter += "&dtef=" + _value;
            _value = $("#<%= HF_date_end_to.ClientID%>").val();
            if (_value != "")
                _filter += "&dtet=" + _value;

            _value = $("#<%= HF_date_contract_from.ClientID%>").val();
            if (_value != "")
                _filter += "&dtcf=" + _value;
            _value = $("#<%= HF_date_contract_to.ClientID%>").val();
            if (_value != "")
                _filter += "&dtct=" + _value;
            _value = $("#<%= drp_conv.ClientID%>").val();
            if (_value != "-1")
                _filter += "&conv=" + _value;

            window.location = "?flt=true" + _filter;


            return;
            _value = $("#<%= drp_contract_state.ClientID%>").val();
            if (_value != "0")
                _filter += "&ids=" + _value;
            _value = $("#<%= txt_code.ClientID%>").val();
            if (_value != "")
                _filter += "&tith=" + _value;
            _value = $("#<%= drp_hotel.ClientID%>").val();
            if (_value != "0")
                _filter += "&idh=" + _value;
        }
        function swapColumn(col) {
            if ($("#state_" + col).val() == "0") {
                $("#toggler_" + col).addClass("opened");
                $("#toggler_" + col).removeClass("closed");
                $("#state_" + col).val("1");
                $(".cont_" + col).css("display", "");
            }
            else {
                $("#toggler_" + col).removeClass("opened");
                $("#toggler_" + col).addClass("closed");
                $("#state_" + col).val("0");
                $(".cont_" + col).css("display", "none");
            }
        }
    </script>
    <style type="text/css">
        #column_visualizer {
            clear: both;
            margin-bottom: 8px;
            margin-top: 15px;
            display: block;
        }

            #column_visualizer a.chk {
                width: 20px;
                height: 20px;
                text-indent: 200px;
                cursor: pointer;
                color: #fff;
                display: inline-block;
                margin: 0;
                text-decoration: none;
            }

            #column_visualizer a.opened {
                background-image: url('../images/ico/chk_ok.png');
            }

            #column_visualizer a.closed {
                background-image: url('../images/ico/chk_no.png');
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal ID="Literal1" runat="server" Visible="false">
		Riepilogo colonne:<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
    </asp:Literal>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:HiddenField ID="HF_is_filtered" runat="server" />
            <asp:HiddenField ID="HF_url_filter" runat="server" />
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Elenco richieste
                        </h1>
                        <div class="bottom_agg">
                            <a href="rnt_request_new.aspx"><span>+ Nuova richiesta</span></a>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="filt">
                            <div class="t">
                                <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
                            </div>
                            <div class="c">
                                <a class="filto_bottone" style="display: none;" id="lnk_showfilt" onclick="showFilter();">FILTRA</a>
                                <a class="filto_bottone2" id="lnk_hidefilt" onclick="hideFilter();">FILTRA</a>
                                <div class="filtro_cont">
                                    <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <asp:PlaceHolder ID="PH_admin" runat="server">
                                                            <td>
                                                                <label>
                                                                    Eliminati:</label>
                                                                <asp:DropDownList runat="server" ID="drp_is_deleted" Width="70px" CssClass="inp">
                                                                    <asp:ListItem Value="">-tutti-</asp:ListItem>
                                                                    <asp:ListItem Value="1">SI</asp:ListItem>
                                                                    <asp:ListItem Value="0">NO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </asp:PlaceHolder>
                                                        <asp:PlaceHolder ID="PH_flt_hidden" runat="server" Visible="false">
                                                            <td>
                                                                <label>
                                                                    Stato</label>
                                                                <asp:DropDownList runat="server" ID="drp_contract_state" Width="100px" CssClass="inp">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    Codice:</label>
                                                                <asp:TextBox ID="txt_code" runat="server" CssClass="inp" Width="50px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    Struttura</label>
                                                                <asp:DropDownList runat="server" ID="drp_hotel" Width="200px" CssClass="inp" />
                                                            </td>
                                                        </asp:PlaceHolder>
                                                        <td>
                                                            <label>
                                                                Nome Cognome:</label>
                                                            <asp:TextBox ID="txt_name_full" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Email:</label>
                                                            <asp:TextBox ID="txt_email" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                IP del cliente :</label>
                                                            <asp:TextBox ID="txt_request_ip" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Data check-in:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="cal_date_start_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_date_start_from" style="cursor: pointer;"
                                                                            alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="cal_date_start_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_date_start_to" style="cursor: pointer;"
                                                                            alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_date_start_from" runat="server" />
                                                            <asp:HiddenField ID="HF_date_start_to" runat="server" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Data check-out:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="cal_date_end_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_date_end_from" style="cursor: pointer;"
                                                                            alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="cal_date_end_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_date_end_to" style="cursor: pointer;"
                                                                            alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_date_end_from" runat="server" />
                                                            <asp:HiddenField ID="HF_date_end_to" runat="server" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Data Creazione:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="cal_date_contract_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_date_contract_from" style="cursor: pointer;"
                                                                            alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="cal_date_contract_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_date_contract_to" style="cursor: pointer;"
                                                                            alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_date_contract_from" runat="server" />
                                                            <asp:HiddenField ID="HF_date_contract_to" runat="server" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Lingua</label>
                                                            <asp:DropDownList runat="server" ID="drp_lang" Width="100px" CssClass="inp">
                                                            </asp:DropDownList>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Location</label>
                                                            <asp:DropDownList runat="server" ID="drp_country" CssClass="inp" Width="120px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td id="pnl_flt_account" runat="server">
                                                            <label>
                                                                Account</label>
                                                            <asp:DropDownList runat="server" ID="drp_account" CssClass="inp" Width="120px">
                                                            </asp:DropDownList>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Correlazione</label>
                                                            <asp:DropDownList runat="server" ID="drp_flt_related" CssClass="inp" Width="120px">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="solo Primarie e Uniche" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="solo Secondarie" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Da</label>
                                                            <asp:DropDownList runat="server" ID="drp_flt_pid_creator" Width="100px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="solo dal Sito" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="solo da info" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Città</label>
                                                            <asp:DropDownList runat="server" ID="drp_city" Width="120px" CssClass="inp" />
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Adwords:</label>
                                                            <asp:DropDownList runat="server" ID="drp_conv" Width="70px" CssClass="inp">
                                                                <asp:ListItem Value="-1">Tutti</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <a href="javascript: Filter()" class="ricercaris"><span>Filtra Risultati</span></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Visualizza le email inviate negli</label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drp_mailing_days" runat="server" CssClass="inp" AutoPostBack="true"
                                                                OnSelectedIndexChanged="drp_mailing_days_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                solo Account disponibili
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chk_only_availables" runat="server" CssClass="inp" OnCheckedChanged="chk_only_availables_CheckedChanged"
                                                                AutoPostBack="true" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                IdAdMedia:
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_IdAdMedia" runat="server" CssClass="inp" Width="120px"></asp:TextBox>
                                                            <td>
                                                                <label>
                                                                    IdLink:
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_IdLink" runat="server" CssClass="inp" Width="120px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <table id="Table1" border="0" cellpadding="0" cellspacing="0" class="inp" runat="server" visible="false">
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                                num. mail inviate in
                                                                            </label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="inp" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="drp_mailing_days_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                                <label>
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <label>
                                                                </label>
                                                            </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="b">
                                <img src="images/filt_b1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_b2.gif" width="6" height="6" style="float: right" />
                            </div>
                        </div>
                    </div>
                    <div style="clear: both">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="HF_lds_filter" runat="server" Visible="false" />
                                <asp:HiddenField ID="HF_id_editAdmin" runat="server" Visible="false" />
                                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext"
                                    TableName="RNT_TBL_REQUEST" OrderBy="request_date_created desc">
                                </asp:LinqDataSource>
                                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" Visible="false" OnDataBound="LV_DataBound"
                                    OnItemDataBound="LV_ItemDataBound" OnItemCommand="LV_ItemCommand">
                                    <ItemTemplate>
                                        <tr class="" onmouseover="$(this).addClass('current');" onmouseout="$(this).removeClass('current');" <%# ("" + Eval("IdAdMedia") == "Ha" || "" + Eval("IdAdMedia") == "HA") ? " bgcolor='#bde4f7'" : ""%>>
                                            <td>
                                                <%# ("" + Eval("pid_creator") != "1") ? "<img src='images/i.png' alt='I' title='" + AdminUtilities.usr_adminName(Eval("pid_creator").objToInt32(), "") + "' />" : ""%>
                                                <%# ("" + Eval("pid_city") == "1") ? "<img src='images/r.png' alt='R' />" : ""%>
                                                <%# ("" + Eval("pid_city") == "2") ? "<img src='images/f.png' alt='F' />" : ""%>
                                                <%# ("" + Eval("pid_city") == "3") ? "<img src='images/v.png' alt='V' />" : ""%>
                                                <%# ("" + Eval("pid_city") == "4") ? "<img src='images/k.png' alt='K' />" : ""%>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("id") %></span>
                                            </td>
                                            <td>
                                                <%# ("" + Eval("state_pid") == "1" || "" + Eval("state_pid") == "2") ? "<span>Lavorazione</span>" : ""%>
                                                <%# ("" + Eval("state_pid") == "5") ? "<span style='color:#00ff00;cursor:pointer;' onclick='RNT_openSelection(" + Eval("pid_reservation") + ")'>E-mail pren.</span>" : ""%>
                                                <%# ("" + Eval("state_pid") == "6") ? "<span style='color:#ff0000;'>Cancellato</span>" : ""%>
                                            </td>
                                            <td style="text-align: center;">
                                                <span>
                                                    <%# ("" + Eval("mail_out") == "1") ? "<span style='color:#00ff00;'>SI</span>" : "<span style='color:#ff0000;'>NO</span>"%>
                                                </span>
                                            </td>
                                            <td style="text-align: center;">
                                                <span>
                                                    <%# ("" + Eval("mail_in") == "1") ? "<span style='color:#00ff00;'>SI</span>" : "<span style='color:#ff0000;'>NO</span>"%>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("request_date_created")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#(!string.IsNullOrEmpty(Convert.ToString(Eval("request_date_start"))))?((DateTime)Eval("request_date_start")).formatITA(true):""%></span>
                                                <%--<%# ((DateTime)Eval("request_date_start")).formatITA(true)%></span>--%>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#(!string.IsNullOrEmpty(Convert.ToString(Eval("request_date_end"))))?((DateTime)Eval("request_date_end")).formatITA(true):""%></span>
                                                <%--<%# ((DateTime)Eval("request_date_end")).formatITA(true)%></span>--%>
                                            </td>
                                            <%--  <td>
                                                <span>
                                                    <%# ((DateTime)Eval("request_date_start")).formatITA(true)%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime)Eval("request_date_end")).formatITA(true)%></span>
                                            </td>--%>
                                            <td class="cont_ip">
                                                <span>
                                                    <%# Eval("request_ip")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("email") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("phone") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("request_country")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# contUtils.getLang_title(Eval("pid_lang").objToInt32())%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_related_request" runat="server" Text='' />
                                            </td>
                                            <td>
                                                <asp:PlaceHolder ID="PH_view" runat="server"><span>
                                                    <%# AdminUtilities.usr_adminName(Eval("pid_operator").objToInt32(), "-! non assegnato !-")%>
                                                    <asp:ImageButton ID="ibtn_edit" CommandName="edit_operator" AlternateText="edit"
                                                        runat="server" ImageUrl="~/images/ico/ico_modif.gif" ToolTip="Cambia Account"
                                                        Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px; margin-left: 5px;" />
                                                </span>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none"> </asp:LinkButton>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="PH_edit" runat="server">
                                                    <asp:DropDownList runat="server" ID="drp_admin" Style="margin-top: 2px; margin-right: 5px; font-size: 11px;">
                                                    </asp:DropDownList>
                                                    <asp:ImageButton ID="ibtn_save" AlternateText="save" runat="server" CommandName="save_operator"
                                                        ImageUrl="~/images/ico/ico_save.gif" ToolTip="Salva Account" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px;" />
                                                    <asp:ImageButton ID="ibtn_cancel" AlternateText="cancel" runat="server" CommandName="cancel_operator"
                                                        ImageUrl="~/images/ico/ico_annulla.gif" ToolTip="Annulla" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px;" />
                                                </asp:PlaceHolder>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# AdminUtilities.usr_adminName(Eval("pid_creator").objToInt32(), "System")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("IdAdMedia")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("IdLink")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_pid_related_request" Visible="false" runat="server" Text='<%# Eval("pid_related_request") %>' />
                                                <asp:Label ID="lbl_pid_operator" Visible="false" runat="server" Text='<%# Eval("pid_operator") %>' />
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <a href="rnt_request_details.aspx?id=<%# Eval("id") %>" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr onmouseover="$(this).addClass('current');" onmouseout="$(this).removeClass('current');" <%# ("" + Eval("IdAdMedia") == "Ha" || "" + Eval("IdAdMedia") == "HA") ? " bgcolor='#bde4f7'" : ""%>>
                                            <td>
                                                <%# ("" + Eval("pid_creator") != "1") ? "<img src='images/i.png' alt='I' title='" + AdminUtilities.usr_adminName(Eval("pid_creator").objToInt32(), "") + "' />" : ""%>
                                                <%# ("" + Eval("pid_city") == "1") ? "<img src='images/r.png' alt='R' />" : ""%>
                                                <%# ("" + Eval("pid_city") == "2") ? "<img src='images/f.png' alt='F' />" : ""%>
                                                <%# ("" + Eval("pid_city") == "3") ? "<img src='images/v.png' alt='V' />" : ""%>
                                                <%# ("" + Eval("pid_city") == "4") ? "<img src='images/k.png' alt='K' />" : ""%>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("id") %></span>
                                            </td>
                                            <td>
                                                <%# ("" + Eval("state_pid") == "1" || "" + Eval("state_pid") == "2") ? "<span>Lavorazione</span>" : ""%>
                                                <%# ("" + Eval("state_pid") == "5") ? "<span style='color:#00ff00;cursor:pointer;' onclick='RNT_openSelection(" + Eval("pid_reservation") + ")'>E-mail pren.</span>" : ""%>
                                                <%# ("" + Eval("state_pid") == "6") ? "<span style='color:#ff0000;'>Cancellato</span>" : ""%>
                                            </td>
                                            <td style="text-align: center;">
                                                <span>
                                                    <%# ("" + Eval("mail_out") == "1") ? "<span style='color:#00ff00;'>SI</span>" : "<span style='color:#ff0000;'>NO</span>"%>
                                                </span>
                                            </td>
                                            <td style="text-align: center;">
                                                <span>
                                                    <%# ("" + Eval("mail_in") == "1") ? "<span style='color:#00ff00;'>SI</span>" : "<span style='color:#ff0000;'>NO</span>"%>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("request_date_created")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#(!string.IsNullOrEmpty(Convert.ToString(Eval("request_date_start"))))?((DateTime)Eval("request_date_start")).formatITA(true):""%></span>
                                                <%--<%# ((DateTime)Eval("request_date_start")).formatITA(true)%></span>--%>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#(!string.IsNullOrEmpty(Convert.ToString(Eval("request_date_end"))))?((DateTime)Eval("request_date_end")).formatITA(true):""%></span>
                                                <%--<%# ((DateTime)Eval("request_date_end")).formatITA(true)%></span>--%>
                                            </td>
                                            <td class="cont_ip">
                                                <span>
                                                    <%# Eval("request_ip")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("email") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("phone") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("request_country")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# contUtils.getLang_title(Eval("pid_lang").objToInt32())%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_related_request" runat="server" Text='' />
                                            </td>
                                            <td>
                                                <asp:PlaceHolder ID="PH_view" runat="server"><span>
                                                    <%# AdminUtilities.usr_adminName(Eval("pid_operator").objToInt32(), "-! non assegnato !-")%>
                                                    <asp:ImageButton ID="ibtn_edit" CommandName="edit_operator" AlternateText="edit"
                                                        runat="server" ImageUrl="~/images/ico/ico_modif.gif" ToolTip="Cambia Account"
                                                        Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px; margin-left: 5px;" />
                                                </span>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none"> </asp:LinkButton>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="PH_edit" runat="server">
                                                    <asp:DropDownList runat="server" ID="drp_admin" Style="margin-top: 2px; margin-right: 5px; font-size: 11px;">
                                                    </asp:DropDownList>
                                                    <asp:ImageButton ID="ibtn_save" AlternateText="save" runat="server" CommandName="save_operator"
                                                        ImageUrl="~/images/ico/ico_save.gif" ToolTip="Salva Account" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px;" />
                                                    <asp:ImageButton ID="ibtn_cancel" AlternateText="cancel" runat="server" CommandName="cancel_operator"
                                                        ImageUrl="~/images/ico/ico_annulla.gif" ToolTip="Annulla" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px;" />
                                                </asp:PlaceHolder>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# AdminUtilities.usr_adminName(Eval("pid_creator").objToInt32(), "System")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("IdAdMedia")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("IdLink")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_pid_related_request" Visible="false" runat="server" Text='<%# Eval("pid_related_request") %>' />
                                                <asp:Label ID="lbl_pid_operator" Visible="false" runat="server" Text='<%# Eval("pid_operator") %>' />
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <a href="rnt_request_details.aspx?id=<%# Eval("id") %>" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>
                                        </tr>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table id="Table1" runat="server" style="">
                                            <tr>
                                                <td>No data was returned.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div id="column_visualizer" style="display: none;">
                                            visualizza nel elenco:<br />
                                            IP del cliente
                                            <a id="toggler_ip" href="javascript:swapColumn('ip')" class="chk opened">chk</a>
                                            <input type="hidden" value="1" id="state_ip">
                                            <script type="text/javascript">
                                                $("#state_ip").val("1");
                                            </script>
                                        </div>
                                        <div class="page">
                                            <asp:DataPager ID="DataPager2" runat="server" PageSize="50" style="border-right: medium none;"
                                                QueryStringField="pg">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="20" />
                                                </Fields>
                                            </asp:DataPager>
                                            <asp:Label ID="lbl_record_count_top" runat="server" CssClass="total" Text=""></asp:Label>
                                            <div class="nulla">
                                            </div>
                                        </div>
                                        <div class="table_fascia">
                                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                                <tr>
                                                    <th>Da
                                                    </th>
                                                    <th style="text-align: center;">ID
                                                    </th>
                                                    <th style="text-align: center;">Stato
                                                    </th>
                                                    <th style="text-align: center; width: 30px;">M.Out
                                                    </th>
                                                    <th style="text-align: center; width: 30px;">M.In
                                                    </th>
                                                    <th>Data Ora richiesta
                                                    </th>
                                                    <th>Data Check-In
                                                    </th>
                                                    <th>Data Check-Out
                                                    </th>
                                                    <th class="cont_ip">IP del cliente
                                                    </th>
                                                    <th>E-mail
                                                    </th>
                                                    <th>Nome Cognome
                                                    </th>
                                                    <th>Telefono
                                                    </th>
                                                    <th>Nazione (Location)
                                                    </th>
                                                    <th>Lingua
                                                    </th>
                                                    <th>Correlazione
                                                    </th>
                                                    <th>Account
                                                    </th>
                                                    <th>Creato da
                                                    </th>
                                                    <th>IdAdMedia
                                                    </th>
                                                    <th>IdLink
                                                    </th>
                                                    <th></th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server" />
                                                <tr>
                                                    <th></th>
                                                    <th></th>
                                                    <th></th>
                                                    <th></th>
                                                    <th></th>
                                                    <th></th>
                                                    <th class="cont_ip"></th>
                                                    <th></th>
                                                    <th></th>
                                                    <th></th>
                                                    <th></th>
                                                    <th></th>
                                                    <th></th>
                                                    <th>
                                                        <asp:LinkButton ID="lnk_updateAllOperators" OnClick="lnk_updateAllOperators_Click"
                                                            runat="server">Aggiorna gli Account</asp:LinkButton>
                                                    </th>
                                                    <th></th>
                                            </table>
                                        </div>
                                        <div class="page">
                                            <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;"
                                                QueryStringField="pg">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="20" />
                                                </Fields>
                                            </asp:DataPager>
                                            <asp:Label ID="lbl_record_count" runat="server" CssClass="total" Text=""></asp:Label>
                                        </div>
                                    </LayoutTemplate>
                                </asp:ListView>
                                <asp:Button ID="btn_page_update" runat="server" Text="Button" Style="display: none;" />
                                <uc2:UC_loader_list ID="UC_loader_list1" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btn_page_update" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var contentUpdater = "<%= btn_page_update.ClientID %>";
        function ReloadContent() {
            __doPostBack(contentUpdater, "load_content");
        }
        ReloadContent();
        var cal_dtStart_from;
        var cal_dtStart_to;

        var cal_dtEnd_from;
        var cal_dtEnd_to;

        var cal_dtCreation_from;
        var cal_dtCreation_to;
        function setCal() {
            cal_dtStart_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_date_start_from.ClientID %>", View: "#cal_date_start_from", Cleaner: "#del_date_start_from", changeMonth: true, changeYear: true });
            cal_dtStart_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_date_start_to.ClientID %>", View: "#cal_date_start_to", Cleaner: "#del_date_start_to", changeMonth: true, changeYear: true });

            cal_dtEnd_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_date_end_from.ClientID %>", View: "#cal_date_end_from", Cleaner: "#del_date_end_from", changeMonth: true, changeYear: true });
            cal_dtEnd_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_date_end_to.ClientID %>", View: "#cal_date_end_to", Cleaner: "#del_date_end_to", changeMonth: true, changeYear: true });

            cal_dtCreation_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_date_contract_from.ClientID %>", View: "#cal_date_contract_from", Cleaner: "#del_date_contract_from", changeMonth: true, changeYear: true });
            cal_dtCreation_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_date_contract_to.ClientID %>", View: "#cal_date_contract_to", Cleaner: "#del_date_contract_to", changeMonth: true, changeYear: true });
        }
    </script>
</asp:Content>
