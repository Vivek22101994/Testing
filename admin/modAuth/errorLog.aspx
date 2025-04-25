<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="errorLog.aspx.cs" Inherits="ModAuth.admin.modAuth.errorLog" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var rwdUrl = null;
            function rwdDettOpen(id) {
                if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
                url = "errorLogDett.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(900);
                rwdUrl.set_minHeight(500);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                //rwdUrl.maximize();
                return false;
            }
        </script>
        <script type="text/javascript">
            function checkIpForSpam(ip) {
                var _url = "http://www.stopforumspam.com/api";
                _url += "?ip=" + ip;
                $.ajax({
                    type: "GET",
                    url: "/common/proxy.aspx?u=" + encodeURIComponent(_url),
                    dataType: "html",
                    success: function (html) {
                        if (html == "") return;
                        if (html.indexOf("<appears>yes</appears>") > -1) {
                            if (confirm(ip + "\n" + html + "\n\n" + "Ip è segnalato come spammer, vuoi bloccarlo?")) addNewIp(ip)
                        }
                        else
                            alert(ip + "\n" + html + "\n\n" + "Ip Non risulta come spammer");
                    }
                });
            }
        </script>
        <script type="text/javascript">
            var timeArray = {};
            var occurs = {};
            function checkBlockedIpList() {
                var _url = "/admin/modAppServerCommon/BlockedIpGetList.aspx";
                $.ajax({
                    type: "GET",
                    url: _url,
                    dataType: "html",
                    success: function (html) {
                        if (html == "") return;
                        var selector = "";
                        var sep = "";
                        var tmpList = html.split("|");
                        for (var i = 0; i < tmpList.length; i++) {
                            selector += sep + "[ip='" + tmpList[i] + "']";
                            sep = ",";
                        }
                        if (selector != "")
                            $("" + selector).addClass("exist");
                    }
                });
                timeArray = [];
                occurs = {};
                $('tr[ip]').each(function (i, selected) {
                    timeArray[i] = $(selected).attr('ip');
                    if (occurs[timeArray[i]] != null) { occurs[timeArray[i]]++; }
                    else { occurs[timeArray[i]] = 1; }
                });
                for (var i = 0; i < timeArray.length; i++) {
                    $("[ip='" + timeArray[i] + "']").attr("occurs", "" + occurs[timeArray[i]]);
                    if (occurs[timeArray[i]] >= 50)
                        $("[ip='" + timeArray[i] + "']").addClass("alert50");
                    else if (occurs[timeArray[i]] >= 40)
                        $("[ip='" + timeArray[i] + "']").addClass("alert40");
                    else if (occurs[timeArray[i]] >= 30)
                        $("[ip='" + timeArray[i] + "']").addClass("alert30");
                    else if (occurs[timeArray[i]] >= 20)
                        $("[ip='" + timeArray[i] + "']").addClass("alert20");
                    else if (occurs[timeArray[i]] >= 10)
                        $("[ip='" + timeArray[i] + "']").addClass("alert10");
                }

            }
            function checkIpForSpam_all() {
                var _url = "http://www.stopforumspam.com/api";
                checked = [];
                for (var i = 0; i < timeArray.length; i++) {
                    if ($.inArray(timeArray[i], checked) == -1) {
                        var ip = timeArray[i]
                        checked.push(ip);
                        if ($("[ip='" + ip + "']").hasClass('exist')) continue;

                        $.ajax({
                            ajaxI: ip,
                            type: "GET",
                            url: "/common/proxy.aspx?u=" + encodeURIComponent(_url + "?ip=" + ip),
                            dataType: "html",
                            success: function (html) {
                                var tmpIp = this.ajaxI;
                                if (html == "") return;
                                if (html.indexOf("<appears>yes</appears>") > -1) {
                                    addNewIp(tmpIp, true);
                                    //$("[ip='" + tmpIp + "']").addClass("exist"); 
                                }
                                else
                                    $("[ip='" + tmpIp + "']").addClass("nospam");
                            }
                        });
                    }
                }

            }
        </script>
        <script type="text/javascript">
            function addNewIp(ip, noalert) {
                if (!noalert && !confirm("Stai per bloccare '" + ip + "'"))return;
                var _url = "/admin/modAppServerCommon/BlockedIpAddNew.aspx";
                _url += "?ip=" + ip;
                $.ajax({
                    type: "GET",
                    url: _url,
                    dataType: "html",
                    success: function (html) {
                        if (noalert) {
                            $("[ip='" + ip + "']").addClass("exist");
                            return;
                        }
                        if (html == "exist") {
                            radalert("Ip esistente.");
                        }
                        else if (html == "notvalid") {
                            radalert("Ip non valido.");
                        }
                        else if (html != "") {
                            radalert("Ip '" + html + "' è stato aggiunto correttamente");
                            $find('<%= pnlFascia.ClientID %>').ajaxRequest('rwdUrl_Closing');
                        }
                    }
                });

            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .ipadded{ display:none !important; color:Green;}
        .ipnospam{ display:none !important; color:Green;}
        .exist .ipadded{ display:block !important;}
        .nospam .ipnospam{ display:block !important;}
        .exist .addip,.nospam .addip{ display:none !important;}
        .alert10 .ip{ color: #FE6634 !important;}
        .alert20 .ip{ color: Red !important;}
        .alert30 .ip{ color: Red !important;}
        .alert40 .ip{ color: Red !important;}
        .alert50 .ip{ color: Red !important;}
        .exist .ip{ color: Green !important;}
    </style>
    <telerik:RadAjaxPanel ID="pnlFascia" runat="server" OnAjaxRequest="pnlFascia_AjaxRequest">
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>
                            ErrorLog
                        </h1>
                    </div>
                    <div class="nulla">
				    </div>
                    <div class="filt">
                        <div class="t">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                        <div class="c">
                            <div class="filtro_cont">
                                <table border="0" cellpadding="0" cellspacing="0" id="tbl_new">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Da:</label>
                                                        <telerik:RadDateTimePicker ID="rdtp_DateFrom" runat="server" CssClass="inp">
                                                            <DateInput DateFormat="dd/MM/yyyy HH:mm">
                                                            </DateInput>
                                                        </telerik:RadDateTimePicker>
                                                    </td>
                                                    <td>
                                                        <label>A:</label>
                                                        <telerik:RadDateTimePicker ID="rdtp_DateTo" runat="server" CssClass="inp">
                                                            <DateInput DateFormat="dd/MM/yyyy HH:mm">
                                                            </DateInput>
                                                        </telerik:RadDateTimePicker>
                                                    </td>
                                                    <td>
                                                        <label>
                                                            Tipo:</label>
                                                        <asp:TextBox ID="txt_Url" runat="server" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>
                                                            Ip:</label>
                                                        <asp:TextBox ID="txt_IP" runat="server" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="bottom">
                                            <asp:LinkButton ID="lnk_flt" CssClass="ricercaris" runat="server" OnClick="lnk_flt_Click" ><span>Carica log</span></asp:LinkButton>
                                        </td>
                                        <td valign="bottom">
                                            <a href="#" onclick="checkIpForSpam_all(); return false;" class="ricercaris"><span>controlla spam tutti</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="b">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                    </div>
                    <div class="nulla">
				    </div>
                    <div style="clear: both">
                        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                            <asp:Literal ID="ltrLDSfiltter" runat="server" Visible="false"></asp:Literal>
                            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModAuth.DCmodAuth" TableName="dbAuthErrorLOGs" OrderBy="logDateTime desc" EntityTypeName="">
                            </asp:LinqDataSource>
                            <asp:ListView ID="LV" DataSourceID="LDS" runat="server" OnSelectedIndexChanging="LV_SelectedIndexChanging" ViewStateMode="Disabled">
                            <ItemTemplate>
                                <tr ip="<%# Eval("logIp")%>" class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                    <td>
                                        <span>
                                            <%# Eval("logDateTime")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("logUrl")%></span>
                                    </td>
                                    <td>
                                        <span class="ip">
                                            <%# Eval("logIp")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("errorContent").ToString().getLines(0).cutString(100)%></span>
                                    </td>
                                    <td>
                                        <a href="#" onclick="rwdDettOpen('<%# Eval("uid")%>'); return false;" style="margin: 5px;">apri</a>
                                    </td>
                                    <td>
                                        <a class="" href="#" onclick="checkIpForSpam('<%# Eval("logIp")%>'); return false;" style="margin: 5px;">controlla spam</a>
                                    </td>
                                    <td>
                                        <a class="addip" href="#" onclick="addNewIp('<%# Eval("logIp")%>'); return false;" style="margin: 5px;">blocca ip</a>
                                        <span class="ipadded">ip bloccato</span>
                                        <span class="ipnospam">non spam</span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr ip="<%# Eval("logIp")%>" class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                    <td>
                                        <span>
                                            <%# Eval("logDateTime")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("logUrl")%></span>
                                    </td>
                                    <td>
                                        <span class="ip">
                                            <%# Eval("logIp")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("errorContent").ToString().getLines(0).cutString(100)%></span>
                                    </td>
                                    <td>
                                        <a href="#" onclick="rwdDettOpen('<%# Eval("uid")%>'); return false;" style="margin: 5px;">apri</a>
                                    </td>
                                    <td>
                                        <a class="" href="#" onclick="checkIpForSpam('<%# Eval("logIp")%>'); return false;" style="margin: 5px;">controlla spam</a>
                                    </td>
                                    <td>
                                        <a class="addip" href="#" onclick="addNewIp('<%# Eval("logIp")%>'); return false;" style="margin: 5px;">blocca ip</a>
                                        <span class="ipadded">ip bloccato</span>
                                        <span class="ipnospam">non spam</span>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table id="Table1" runat="server" style="">
                                    <tr>
                                        <td>
                                            Non ci sono errori nel log.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div class="table_fascia">
                                    <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0" cellpadding="0"
                                        style="">
                                        <tr id="Tr1" runat="server" style="">
                                            <th id="Th4" runat="server" width="120px">
                                                Data
                                            </th>
                                            <th id="Th1" runat="server" width="150px">
                                                Tipo
                                            </th>
                                            <th id="Th2" runat="server" width="100px">
                                                IP
                                            </th>
                                            <th id="Th5" runat="server" width="">
                                            </th>
                                            <th id="Th3" runat="server" width="35px"></th>
                                            <th id="Th6" runat="server" width="85px"></th>
                                            <th id="Th7" runat="server" width="70px"></th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                            </LayoutTemplate>
                        </asp:ListView>
                        </telerik:RadCodeBlock>
                    </div>
                </div>
            </div>
        </telerik:RadAjaxPanel>
</asp:Content>
