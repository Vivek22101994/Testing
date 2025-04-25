<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true"
    CodeBehind="util_ErrorLog.aspx.cs" Inherits="RentalInRome.admin.util_ErrorLog" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function checkSubmit() {
                var _error = "";
                var _value = "";
                _value = $("#<%= drp_logList.ClientID%>").val();
                if (_value == "" || _value == "0")
                    _error += "\n--Seleziona la data.";
                if (_error != "") {
                    alert("Per caricare il log mancano seguenti dati obbligatori..." + _error);
                    return false;
                }
                return true;
            }
        </script>
        <script type="text/javascript">
            var rwdUrl = null;
            function rwdDettOpen(id) {
                if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
                url = "util_ErrorLogDett.aspx?id=" + id + "&dt=" + $("#<%= drp_logList.ClientID%>").val();
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
    </telerik:RadCodeBlock>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <asp:HiddenField ID="HF_folder" runat="server" Value="log/errors" />
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
                            <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                            <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
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
                                                            Data</label>
                                                        <asp:DropDownList runat="server" ID="drp_logList" Width="120px" CssClass="inp" />
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
                                                    <td>
                                                        <label>
                                                            Contenuto:</label>
                                                        <asp:TextBox ID="txt_Value" runat="server" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="bottom">
                                            <asp:LinkButton ID="lnk_flt" CssClass="ricercaris" runat="server" OnClick="lnk_flt_Click"
                                                OnClientClick="return checkSubmit();"><span>Carica log</span></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="b">
                            <img src="images/filt_b1.gif" width="6" height="6" style="float: left" />
                            <img src="images/filt_b2.gif" width="6" height="6" style="float: right" />
                        </div>
                    </div>
                    <div class="nulla">
				    </div>
                    <div style="clear: both">
                        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                            <asp:ListView ID="LV" runat="server" OnSelectedIndexChanging="LV_SelectedIndexChanging" ViewStateMode="Disabled">
                            <ItemTemplate>
                                <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                    <td>
                                        <span>
                                            <%# Eval("Date").ToString().JSCal_stringToDateTime()%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("Url")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("IP")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("Value").ToString().getLines(0).cutString(100)%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Value" Visible="false" runat="server" Text='<%#Eval("Value") %>'></asp:Label>
                                        <a href="#" onclick="rwdDettOpen('<%# Eval("ID")%>'); return false;">apri</a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                    <td>
                                        <span>
                                            <%# Eval("Date").ToString().JSCal_stringToDateTime()%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("Url")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("IP")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("Value").ToString().getLines(0).cutString(100)%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Value" Visible="false" runat="server" Text='<%#Eval("Value") %>'></asp:Label>
                                        <a href="#" onclick="rwdDettOpen('<%# Eval("ID")%>'); return false;">apri</a>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table id="Table1" runat="server" style="">
                                    <tr>
                                        <td>
                                            Seleziona la data
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
                                            <th id="Th5" runat="server">
                                            </th>
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
