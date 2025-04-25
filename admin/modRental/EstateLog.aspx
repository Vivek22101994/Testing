<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateLog.aspx.cs" Inherits="ModRental.admin.modRental.EstateLog" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var rwdUrl = null;
            function rwdDettOpen(id) {
                if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
                url = "EstateLogDett.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(900);
                rwdUrl.set_minHeight(500);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                //rwdUrl.maximize();
                return false;
            }
            function rwdUrl_OnClientClose(sender, eventArgs) {
                $find('<%= pnlFascia.ClientID %>').ajaxRequest('rwdUrl_Closing');
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxPanel ID="pnlFascia" runat="server" OnAjaxRequest="pnlFascia_AjaxRequest">
        <asp:HiddenField ID="HF_folder" runat="server" Value="log/mailerrors" />
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>
                            Log degli Apt
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
                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                                                            <table class="inp">
                                                                <tr>
                                                                    <td style="width: 180px;">
                                                                        <label>Zona:</label>
                                                                        <asp:ListBox ID="lbx_flt_zone" runat="server" SelectionMode="Multiple" Width="150px" Rows="11" CssClass="inp" AutoPostBack="true" OnSelectedIndexChanged="lbx_flt_zone_SelectedIndexChanged"></asp:ListBox>
                                                                    </td>
                                                                    <td style="width: 250px;">
                                                                        <label>Struttura:</label>
                                                                        <asp:ListBox ID="lbx_flt_estate" runat="server" SelectionMode="Multiple" Width="220px" Rows="11" CssClass="inp"></asp:ListBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </telerik:RadAjaxPanel>
                                                    </td>
                                                    <td>
                                                        <label>Utente</label>
                                                        <asp:DropDownList runat="server" ID="drp_account" CssClass="inp" Width="120px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>Registrato da:</label>
                                                        <telerik:RadDatePicker ID="rdp_flt_createdDateFrom" runat="server" Width="100px" CssClass="inp">
                                                            <DateInput DateFormat="dd/MM/yyyy">
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                        <label>Registrato a:</label>
                                                        <telerik:RadDatePicker ID="rdp_flt_createdDateTo" runat="server" Width="100px" CssClass="inp">
                                                            <DateInput DateFormat="dd/MM/yyyy">
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                        <label>Campo</label>
                                                        <asp:TextBox ID="txt_flt_changeField" runat="server" CssClass="inp" Width="120px"></asp:TextBox>
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
                            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCmodRental" TableName="dbRntEstateLOGs" OrderBy="logDate desc" EntityTypeName="">
                            </asp:LinqDataSource>
                            <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnSelectedIndexChanging="LV_SelectedIndexChanging" ViewStateMode="Disabled">
                                <ItemTemplate>
                                    <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                        <td>
                                            <span>
                                                <%# Eval("logDate")%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("estateCode")%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("userName")%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("changeField")%></span>
                                        </td>
                                        <td>
                                            <a href="#" onclick="rwdDettOpen('<%# Eval("uid")%>'); return false;">apri</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                        <td>
                                            <span>
                                                <%# Eval("logDate")%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("estateCode")%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("userName")%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("changeField")%></span>
                                        </td>
                                        <td>
                                            <a href="#" onclick="rwdDettOpen('<%# Eval("uid")%>'); return false;">apri</a>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <table id="Table1" runat="server" style="">
                                        <tr>
                                            <td>
                                                nessuna modifica effettuata con criteri di ricerca
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <div class="table_fascia">
                                        <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0" cellpadding="0" style="">
                                            <tr id="Tr1" runat="server" style="">
                                                <th id="Th4" runat="server" width="120px">Data </th>
                                                <th id="Th2" runat="server" style="min-width: 200px;">Struttura </th>
                                                <th id="Th6" runat="server" style="min-width: 200px;">Utente</th>
                                                <th id="Th5" runat="server" style="min-width: 200px;">Campo </th>
                                                <th id="Th3" runat="server"></th>
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
