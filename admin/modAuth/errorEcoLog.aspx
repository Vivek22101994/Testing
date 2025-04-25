<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="errorEcoLog.aspx.cs" Inherits="ModAuth.admin.modAuth.errorEcoLog" %>

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
                url = "errorEcoLogDett.aspx?id=" + id;
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
                            ErrorEcoLog
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
                                                            Ids:</label>
                                                        <asp:TextBox ID="txt_IP" runat="server" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="bottom">
                                            <asp:LinkButton ID="lnk_flt" CssClass="ricercaris" runat="server" OnClick="lnk_flt_Click" ><span>Carica log</span></asp:LinkButton>
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
                            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModAuth.DCmodAuth" TableName="dbAuthErrorEcoLOGs" OrderBy="logDateTime desc" EntityTypeName="">
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
                                        <a href="errorEcoLog.aspx?resend=<%# Eval("uid")%>" style="margin: 5px;">reinvia</a>
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
                                        <a href="errorEcoLog.aspx?resend=<%# Eval("uid")%>" style="margin: 5px;">reinvia</a>
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
                                                Ids
                                            </th>
                                            <th id="Th5" runat="server" width="">
                                            </th>
                                            <th id="Th3" runat="server" width="35px"></th>
                                            <th id="Th6" runat="server" width="85px"></th>
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
