<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="ReservationOccupancyReport.aspx.cs" Inherits="MagaRentalCE.admin.modRental.ReservationOccupancyReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_loader.ascx" TagName="UC_loader_list" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .reportList {
            width: auto;
        }

            .reportList table {
                width: 100%;
                padding: 5px;
                margin-top: 10px;
                margin-right: 20px;
                border-collapse: collapse;
            }

        .alternate {
            background-color: #e8e8e8;
        }

        .reportList table tr {
            height: 30px;
        }

        .content table td {
            border: 1px solid #d5d5d5;
        }

        .tbleft table td {
            border-top: 1px solid #d5d5d5;
            border-bottom: 1px solid #d5d5d5;
            border-left: 1px solid #d5d5d5;
        }

        .reportList .head_row {
            background-color: #808080;
            color: #fff;
            font-weight: bold;
            text-align: center;
        }

            .reportList .head_row td {
                border: 1px solid #fff !important;
                text-align: center!important;
            }

        .col {
            width: 50px;
            text-align: center;
        }

        .tbleft {
            float: left;
            width: 19.5%;
        }


        div.horiz-container {
            /*border: 1px solid silver;*/
            overflow-x: auto;
            overflow-y: hidden;
            white-space: nowrap;
            width: 80%;
        }

            div.horiz-container div.content {
                float: left;
                display: inline;
                width: auto;
            }

        .head_row tr td {
            margin-right: 10px;
            margin-left: 10px;
        }

        .colTitle {
            margin-right: 10px;
            margin-left: 10px;
        }

        .colHeader {
            float: right;
        }

        .colTp {
            float: left;
        }

        .colTitle.infoBtn.contTp {
            display: inline-block !important;
            float: none !important;
            margin: 2px 0 0 5px !important;
        }

        .colHeader.colTitle {
            display: inline-block;
            float: none;
            font-size: 12px;
            margin: 0 5px;
            position: relative;
            top: -2px;
        }

        .tbleft table td .aptName {
            display: block;
            font-size: 12px;
            line-height: 12px;
            max-height: 24px;
        }

        .filt table tr td {
            padding: 5px 0 0 5px;
        }
    </style>
    <script type="text/javascript">
        function showFilter() {
            $("#lnk_showfilt").css("display", "none");
            $("#lnk_hidefilt").css("display", "");
            $("#tbl_filter").css("display", "");
            $("#div_columns").css("display", "");
            $("#<%=HF_is_filtered.ClientID %>").val("true");
        }
        function hideFilter() {
            $("#lnk_showfilt").css("display", "");
            $("#lnk_hidefilt").css("display", "none");
            $("#tbl_filter").css("display", "none");
            $("#div_columns").css("display", "none");
            $("#<%=HF_is_filtered.ClientID %>").val("false");
        }
        function showColumnFilter() {
            $("#lnk_showcolumnfilt").css("display", "none");
            $("#lnk_hidecolumnfilt").css("display", "");
            $("#div_columns").css("display", "");
            // $("#<%=HF_is_filtered.ClientID %>").val("true");
        }
        function hideColumnFilter() {
            $("#lnk_showcolumnfilt").css("display", "");
            $("#lnk_hidecolumnfilt").css("display", "none");
            $("#div_columns").css("display", "none");
            // $("#<%=HF_is_filtered.ClientID %>").val("false");
        }

        function filterEnterPress(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                Filter();
                return false;
            }
        }
        var items = [
			<%=ltr_items.Text %>
        ];


        function setAutocomplete() {
            $(".aptComplete").autocomplete({
                source: items,
                select: function (event, ui) {
                    //window.location = "modRental/EstateDettMain.aspx?id=" + ui.item.idEstate;
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal ID="ltr_items" runat="server" Visible="false"></asp:Literal>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:HiddenField ID="HF_is_filtered" runat="server" />
    <asp:HiddenField ID="HF_url_filter" runat="server" />
    <div id="fascia1">
        <div class="pannello_fascia1">
            <div style="clear: both">
                <h1><%=contUtils.getLabel("lblOcuupacyReportTitle") %></h1>
                <div class="nulla">
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="filt">
                            <div class="t">
                                <asp:LinkButton ID="lnkFakeUpdate" runat="server" Style="display: none;"></asp:LinkButton>
                                <img src="/admin/images/filt_t1.gif" width="6" height="6" style="float: left" />
                                <img src="/admin/images/filt_t2.gif" width="6" height="6" style="float: right" />
                            </div>
                            <div class="c">
                                <a class="filto_bottone" style="display: none;" id="lnk_showfilt" onclick="showFilter();"><%= contUtils.getLabel("lblFiltraIRisultati")%></a>
                                <a class="filto_bottone2" id="lnk_hidefilt" onclick="hideFilter();"><%= contUtils.getLabel("lblFiltraIRisultati")%></a>

                                <div class="filtro_cont">
                                    <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <%= contUtils.getLabel("lblCity")%></label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="drp_flt_city" runat="server" Width="300px" EmptyMessage='-- All --' CheckBoxes="true" OnClientDropDownClosed="drp_city_ItemChecked" EnableCheckAllItemsCheckBox="true">
                                                            </telerik:RadComboBox>
                                                        </td>

                                                        <td>
                                                            <label><%= contUtils.getLabel("lblZone")%></label>
                                                        </td>

                                                        <td>
                                                            <telerik:RadComboBox ID="drp_flt_Zone" runat="server" Width="300px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" OnClientDropDownClosed="drp_zone_ItemChecked">
                                                            </telerik:RadComboBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label><%= contUtils.getLabel("reqApartments")%>:</label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="drp_estate" runat="server" Width="300px" EmptyMessage='-- All --' CheckBoxes="true" EnableCheckAllItemsCheckBox="true" OnClientDropDownClosed="drp_estate_ItemChecked">
                                                            </telerik:RadComboBox>
                                                        </td>

                                                        <td>
                                                            <label>
                                                                <%= contUtils.getLabel("lblMonth")%>:
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="drp_months" EmptyMessage='-- All --' runat="server" CheckBoxes="true" Width="300px" EnableCheckAllItemsCheckBox="true">
                                                                <Items>
                                                                    <%--<telerik:RadComboBoxItem Text="-- All -- " Value="0" />--%>
                                                                    <telerik:RadComboBoxItem Text="January" Value="1" />
                                                                    <telerik:RadComboBoxItem Text="February" Value="2" />
                                                                    <telerik:RadComboBoxItem Text="March" Value="3" />
                                                                    <telerik:RadComboBoxItem Text="April" Value="4" />
                                                                    <telerik:RadComboBoxItem Text="May" Value="5" />
                                                                    <telerik:RadComboBoxItem Text="June" Value="6" />
                                                                    <telerik:RadComboBoxItem Text="July" Value="7" />
                                                                    <telerik:RadComboBoxItem Text="August" Value="8" />
                                                                    <telerik:RadComboBoxItem Text="September" Value="9" />
                                                                    <telerik:RadComboBoxItem Text="October" Value="10" />
                                                                    <telerik:RadComboBoxItem Text="November" Value="11" />
                                                                    <telerik:RadComboBoxItem Text="December" Value="12" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <%= contUtils.getLabel("lblOwner")%>
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="drp_flt_Owner" runat="server" Width="300px" EmptyMessage='-- All --' CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <%= contUtils.getLabel("lblYear")%>
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="drp_flt_year" EmptyMessage='-- Select Year --' runat="server" CheckBoxes="true" Width="300px" EnableCheckAllItemsCheckBox="true">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <%= contUtils.getLabel("lblChannles")%>
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="drp_flt_agency" CssClass="inp" Width="120px">
                                                            </asp:DropDownList>
                                                        </td>

                                                        <td>
                                                            <label>
                                                                <%= contUtils.getLabel("lblReportColumns")%>
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="drp_columns" EmptyMessage='-- Select Columns --' runat="server" CheckBoxes="true" Width="300px" EnableCheckAllItemsCheckBox="true"></telerik:RadComboBox>

                                                        </td>
                                                    </tr>

                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="middle">
                                                <a onclick="RNT_fillList();" href="#" class="ricercaris"><span><%= contUtils.getLabel("lblFiltraIRisultati") %></span></a>
                                            </td>
                                            <td valign="middle">
                                                <asp:LinkButton ID="lnk_export_excel" runat="server" CssClass="ricercaris" OnClick="lnk_export_excel_Click"><span><%= contUtils.getLabel("btnExcelExport") %></span></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="b">
                                <img src="/admin/images/filt_b1.gif" width="6" height="6" style="float: left" />
                                <img src="/admin/images/filt_b2.gif" width="6" height="6" style="float: right" />
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnk_export_excel" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div style="clear: both">
            </div>
        </div>

    </div>
    <div class="nulla">
    </div>
    <div class="">
        <asp:Literal ID="ltr_langTemp" runat="server"></asp:Literal>
        <div id="resList_body" class="reportList"></div>
    </div>
    <div class="nulla">
    </div>

    <script type="text/javascript">

        function getSelectedValues(radCombo) {
            var items = radCombo.get_checkedItems();
            var _arrSep = "";
            _arrStr = "";
            for (var i = 0; i < items.length; i++) {
                _arrStr += _arrSep + "" + items[i].get_value();
                _arrSep = "|";
            }
            return _arrStr;
        }


        function RNT_fillList() {

            SITE_showLoader();
            $("#resList_body").html($("#loaderBody").html());
            var combo = $find("<%= drp_months.ClientID %>");
            var _monthListStr = getSelectedValues(combo);
            console.log('months :' + _monthListStr);

            combo = $find("<%= drp_flt_city.ClientID %>");
            var _selectedCity = getSelectedValues(combo);

            combo = $find("<%= drp_estate.ClientID %>");
            var _selectedEstate = getSelectedValues(combo);

            combo = $find("<%= drp_flt_Zone.ClientID %>");
            var _selectedZone = getSelectedValues(combo);

            combo = $find("<%= drp_flt_year.ClientID %>");
            var _selectedYear = getSelectedValues(combo);

            combo = $find("<%= drp_flt_Owner.ClientID %>");
            var _selectedOwner = getSelectedValues(combo);

            combo = document.getElementById("<%=drp_flt_agency.ClientID%>");
            var _selectedAgency = combo.value;

            combo = $find("<%=drp_columns.ClientID%>");
            var _selectedColumn = getSelectedValues(combo);
            //alert(_selectedAgency);

            var _url = "/admin/webservice/rnt_reservationOccupancy.aspx";
            var jsondata = {};
            jsondata.currCity = _selectedCity;
            jsondata.currZone = _selectedZone;
            jsondata.currEstate = _selectedEstate;
            jsondata.months = _monthListStr;
            jsondata.ownerID = _selectedOwner;
            jsondata.agencyID = _selectedAgency;
            jsondata.year = _selectedYear;
            jsondata.lang = "<%=App.LangID%>";
            jsondata.columns = _selectedColumn;
            var _xml = $.ajax({
                type: "POST",
                url: _url,
                data:jsondata,
                dataType: "html",
                success: function (html) {
                    SITE_hideLoader();
                    $("#resList_body").html(html);
                    setToolTip();
                }
            });
        }

    </script>

    <telerik:RadScriptBlock runat="server" ID="RadBlock1">
        <script type="text/javascript">
            function drp_city_ItemChecked() {
                __doPostBack("<%= lnkFakeUpdate.ClientID %>", "drp_city_ItemChecked");
                return false;
            }

            function drp_zone_ItemChecked() {
                __doPostBack("<%= lnkFakeUpdate.ClientID %>", "drp_zone_ItemChecked");
                return false;
            }
            function drp_estate_ItemChecked() {
                __doPostBack("<%= lnkFakeUpdate.ClientID %>", "drp_estate_ItemChecked");
                return false;
            }
        </script>
    </telerik:RadScriptBlock>
</asp:Content>

