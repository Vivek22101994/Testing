<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="agentMonthlyReportList.aspx.cs" Inherits="ModRental.admin.modRental.agentMonthlyReportList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function showFilter() {
            document.getElementById("lnk_showfilt").style.display = "none";
            document.getElementById("lnk_hidefilt").style.display = "";
            document.getElementById("tbl_filter").style.display = "";
        }
        function hideFilter() {
            document.getElementById("lnk_showfilt").style.display = "";
            document.getElementById("lnk_hidefilt").style.display = "none";
            document.getElementById("tbl_filter").style.display = "none";
        }
    </script>
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "agentDett.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(700);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                rwdUrl.maximize();
            }
            return false;
        }
        function rwdUrl_OnClientClose(sender, eventArgs) {
            $find('<%= pnlFascia.ClientID %>').ajaxRequest('rwdUrl_Closing');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="0" runat="server" Visible="false" />
            <asp:HiddenField ID="HF_title" Value="" runat="server" Visible="false" />
            <div style="clear: both">
                <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                    <h1>Report mensile Agenzia:
                        <%=HF_title.Value %></h1>
                </telerik:RadCodeBlock>
                <div class="bottom_agg">
                    <a href="agentList.aspx" title="Torna nella lista">
                        <span>Torna nella lista</span>
                    </a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCmodRental" TableName="dbRntAgentMonthlyReportTBLs" Where="pidAgent == @pidAgent" OrderBy="reportYear desc, reportMonth desc" EntityTypeName="">
                 <WhereParameters>
                    <asp:ControlParameter Type="Int64" ControlID="HfId" Name="pidAgent" />
                 </WhereParameters>
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("code") %></span>
                            </td>
                            <td>
                                <span>
                                    <%#  (new DateTime(Eval("reportYear").objToInt32(), Eval("reportMonth").objToInt32(), 1)).formatCustom("#MM# #yy#", 1, "--/--")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("countReservationsTotal")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashReservationsTotal").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashCommissionsTotal").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashDiscountNotPayed").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashToPayBack").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("code") %></span>
                            </td>
                            <td>
                                <span>
                                    <%#  (new DateTime(Eval("reportYear").objToInt32(), Eval("reportMonth").objToInt32(), 1)).formatCustom("#MM# #yy#", 1, "--/--")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("countReservationsTotal")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashReservationsTotal").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashCommissionsTotal").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashDiscountNotPayed").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashToPayBack").objToDecimal().ToString("N2")%></span>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" style="">
                            <tr>
                                <td>
                                    No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="table_fascia">
                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                <tr style="text-align: left">
                                    <th style="width: 50px">Codice </th>
                                    <th style="width: 120px">Mese</th>
                                    <th style="width: 40px">Num. Pren. </th>
                                    <th style="width: 100px">Somma Totale </th>
                                    <th style="width: 100px">Comm. Totale </th>
                                    <th style="width: 100px">Comm. scalate </th>
                                    <th style="width: 150px;">Comm. da ripagare </th>
                                    <th></th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                        <div class="page">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;">
                                <Fields>
                                    <asp:NumericPagerField ButtonCount="20" />
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </telerik:RadAjaxPanel>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function rwdDettClose() {
                $find("<%= pnlFascia.ClientID%>").ajaxRequest('rwdDett_Closing');
                //RadAjaxManager_ajaxRequest('rwdDett_Closing');
                return false;
            }
            function rwdDett_OnClientClose(sender, eventArgs) {
                return rwdDettClose();
            } 
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
