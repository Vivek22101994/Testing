<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateExtraOwnerList.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateExtraOwnerList" %>
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
                url = "EstateExtraOwnerDett.aspx?id=" + id;
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
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Gestione Agenzie </h1>
                <div class="bottom_agg">
                    <a href="#" onclick="return setUrl('dett', '0')" title="Crea Nuovo">
                        <span>+ Nuovo</span>
                    </a>
                </div>
               <%-- <div class="bottom_agg" style="margin-right: 20px;">
                    <asp:LinkButton ID="lnkSendMail" runat="server" OnClick="lnkSendMail_Click" OnClientClick="return confirm('Stai per inviare le mail di benvenuto a tutte Attive\npotrebbe essere gia inviato.\nVuoi procedere?');"><span>Invia le mail di benvenuto a tutte Attive</span></asp:LinkButton>
                </div>
                <div class="bottom_agg" style="margin-right: 20px;">
                    <asp:LinkButton ID="lnkCreateAuth" runat="server" OnClick="lnkCreateAuth_Click"><span>Genera le password</span></asp:LinkButton>
                </div>--%>
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
                        <a class="filto_bottone" style="display: none;" id="lnk_showfilt" onclick="showFilter();">FILTRA</a>
                        <a class="filto_bottone2" id="lnk_hidefilt" onclick="hideFilter();">FILTRA</a>
                        <div class="filtro_cont">
                            <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                <tr>
                                    <td>
                                        <telerik:RadAjaxPanel ID="rapFilter" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <label>Codice:</label>
                                                        <asp:TextBox ID="txt_flt_code" runat="server" Width="50" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Nome Agenzia:</label>
                                                        <asp:TextBox ID="txt_flt_nameCompany" runat="server" Width="150" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Nome Contatto:</label>
                                                        <asp:TextBox ID="txt_flt_nameFull" runat="server" Width="150" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>E-mail:</label>
                                                        <asp:TextBox ID="txt_flt_contactEmail" runat="server" Width="150" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td id="pnl_flt_pidReferer" runat="server" visible="false">
                                                        <label>Account (Produttore)</label>
                                                        <asp:DropDownList runat="server" ID="drp_flt_pidReferer" CssClass="inp" Width="120px">
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
                                                        <label>Stato:</label>
                                                        <asp:DropDownList ID="drp_flt_isActive" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">Attivi</asp:ListItem>
                                                            <asp:ListItem Value="0">NON Attivi</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>Terms:</label>
                                                        <asp:DropDownList ID="drp_flt_hasAcceptedContract" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                            <asp:ListItem Value="0">NO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>Pren.:</label>
                                                        <asp:DropDownList ID="drp_flt_hasPren" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="">Solo con pren. in questo mese</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadAjaxPanel>
                                    </td>
                                    <td valign="bottom">
                                        <asp:LinkButton ID="lnk_filter" runat="server" CssClass="ricercaris" OnClick="lnk_filter_Click"><span>Filtra Risultati</span></asp:LinkButton>
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
            </div>
            <div style="clear: both">
                <asp:Literal ID="ltrLDSfiltter" runat="server" Visible="false"></asp:Literal>
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCmodRental" TableName="dbRntExtraOwnerTBLs" OrderBy="createdDate desc" EntityTypeName="">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand" OnPagePropertiesChanged="LV_PagePropertiesChanged">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("code") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("nameCompany")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# rntUtils.getDiscountType_code(Eval("pidDiscountType").objToInt32(), "- - -")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("payDiscountNotPayed") + "|" + Eval("payFullPayment") == "0|0" ? "Commissione" : ""%>
                                    <%# Eval("payDiscountNotPayed") + "|" + Eval("payFullPayment") == "1|0" ? "Commissione - Sconto" : ""%>
                                    <%# Eval("payDiscountNotPayed") + "|" + Eval("payFullPayment") == "0|1" ? "Saldo" : ""%>
                                    <%# Eval("payDiscountNotPayed") + "|" + Eval("payFullPayment") == "1|1" ? "Saldo - Sconto" : ""%>
                                </span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("nameFull")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("contactEmail")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("contactPhone")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# AdminUtilities.usr_adminName(Eval("pidReferer").objToInt32(), "- - -")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("createdDate")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("hasAcceptedContract") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                          <%--  <td>
                                <a href="agentMonthlyReportList.aspx?id=<%# Eval("id") %>" title="Apri Report mensile" style="text-decoration: none; border: 0 none; margin: 5px;">Report mensile </a>
                            </td>--%>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("code") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("nameCompany")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# rntUtils.getDiscountType_code(Eval("pidDiscountType").objToInt32(), "- - -")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("payDiscountNotPayed") + "|" + Eval("payFullPayment") == "0|0" ? "Commissione" : ""%>
                                    <%# Eval("payDiscountNotPayed") + "|" + Eval("payFullPayment") == "1|0" ? "Commissione - Sconto" : ""%>
                                    <%# Eval("payDiscountNotPayed") + "|" + Eval("payFullPayment") == "0|1" ? "Saldo" : ""%>
                                    <%# Eval("payDiscountNotPayed") + "|" + Eval("payFullPayment") == "1|1" ? "Saldo - Sconto" : ""%>
                                </span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("nameFull")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("contactEmail")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("contactPhone")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# AdminUtilities.usr_adminName(Eval("pidReferer").objToInt32(), "- - -")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("createdDate")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("hasAcceptedContract") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                           <%-- <td>
                                <a href="agentMonthlyReportList.aspx?id=<%# Eval("id") %>" title="Apri Report mensile" style="text-decoration: none; border: 0 none; margin: 5px;">Report mensile </a>
                            </td>--%>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
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
                                    <th style="width: 150px">Nome Agenzia </th>
                                    <th style="width: 150px">Tipo Commissioni </th>
                                    <th style="width: 150px">Tipo Pagamento </th>
                                    <th style="width: 150px">Nome Contatto </th>
                                    <th style="width: 150px">E-mail </th>
                                    <th style="width: 150px">Telefono </th>
                                    <th style="width: 150px;">Account (Produttore) </th>
                                    <th style="width: 130px">Registrato in data </th>
                                    <th style="width: 40px;">Attivo? </th>
                                    <th style="width: 40px;">Terms? </th>
                                    <th></th>
                                    <th></th>
                                   <%-- <th></th>--%>
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

