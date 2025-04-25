<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateCommentsList.aspx.cs" Inherits="ModRental.admin.modRental.EstateCommentsList" %>

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
                url = "EstateCommentsDett.aspx?id=" + id;
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
                <h1>Gestione Commenti per appartamenti</h1>
                <div class="bottom_agg">
                    <a href="#" onclick="return setUrl('dett', '0')" title="Crea Nuovo">
                        <span>+ Nuovo</span>
                    </a>
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
                            <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                <tr>
                                    <td>
                                        <label>Data commento:</label>
                                        <table class="inp">
                                            <tr>
                                                <td>
                                                    <label>da:</label>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="rdp_flt_dtFrom" runat="server" Width="100px" CssClass="inp">
                                                        <DateInput DateFormat="dd/MM/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>a:</label>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="rdp_flt_dtTo" runat="server" Width="100px" CssClass="inp">
                                                        <DateInput DateFormat="dd/MM/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="nulla">
                                        </div>
                                    </td>
                                    <td>
                                        <label>Città:</label>
                                        <asp:DropDownList runat="server" ID="drp_flt_pidCity" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="drp_flt_pidCity_SelectedIndexChanged" CssClass="inp" Style="margin-bottom: 10px;" />
                                        <div class="nulla">
                                        </div>
                                        <label>Proprietario:</label>
                                        <asp:DropDownList runat="server" ID="drp_flt_pidOwner" Width="200px" CssClass="inp" Style="margin-bottom: 10px;" />
                                        <div class="nulla">
                                        </div>
                                        <label>Nome Appartamento:</label>
                                        <asp:TextBox ID="txt_flt_code" runat="server" Width="200px" CssClass="inp"></asp:TextBox>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <label>
                                            Zona:
                                            <br />
                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="select_pidZone" Style="color: #E01E15; text-decoration: none;">seleziona tutti</asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton3" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="deselect_pidZone" Style="color: #E01E15; text-decoration: none;">deseleziona tutti</asp:LinkButton>
                                        </label>
                                        <div class="nulla">
                                        </div>
                                        <div style="max-height: 150px; min-width: 200px; overflow-y: auto;">
                                            <asp:CheckBoxList ID="chkList_flt_pidZone" runat="server" CssClass="inp" TextAlign="Right" RepeatColumns="6" Style="margin: 0 5px 5px 0;">
                                            </asp:CheckBoxList>
                                            <div class="nulla">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:LinkButton ID="lnk_filter" runat="server" CssClass="ricercaris" OnClick="lnk_filter_Click"><span>Filtra</span></asp:LinkButton>
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
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCmodRental" TableName="dbRntEstateCommentsTBLs" EntityTypeName="" OrderBy="dtComment desc">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand" OnPagePropertiesChanged="LV_PagePropertiesChanged">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# CurrentSource.rntEstate_code(Eval("pidEstate").objToInt32(), "-tutti-")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime)Eval("dtComment")).formatITA(true) %></span>
                            </td>
                            <td>
                                <span>
                                    <%# getType(""+Eval("type"))  %></span>
                            </td>
                            <td>
                                <span>
                                    <%# getPers(""+Eval("pers"))  %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cl_name_full") + "&nbsp;(" + Eval("cl_country") + ")"%></span>
                            </td>
                            <td>
                                <%# "" + Eval("isActive") == "1" ? "<span style=\"color:#0f0\">SI</span>" : "<span style=\"color:#f00\">NO</span>"%>
                            </td>

                            <td>
                                <%# "" + Eval("isVisibleHomePage") == "1" ? "<span style=\"color:#0f0\">SI</span>" : "<span style=\"color:#f00\">NO</span>"%>
                            </td>
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
                                    <%# CurrentSource.rntEstate_code(Eval("pidEstate").objToInt32(), "-tutti-")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime)Eval("dtComment")).formatITA(true) %></span>
                            </td>
                            <td>
                                <span>
                                    <%# getType(""+Eval("type"))  %></span>
                            </td>
                            <td>
                                <span>
                                    <%# getPers(""+Eval("pers"))  %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cl_name_full") + "&nbsp;(" + Eval("cl_country") + ")"%></span>
                            </td>
                            <td>
                                <%# "" + Eval("isActive") == "1" ? "<span style=\"color:#0f0\">SI</span>" : "<span style=\"color:#f00\">NO</span>"%>
                            </td>
                            <td>
                                <%# "" + Eval("isVisibleHomePage") == "1" ? "<span style=\"color:#0f0\">SI</span>" : "<span style=\"color:#f00\">NO</span>"%>
                            </td>
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
                                <td>No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="table_fascia">
                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                <tr style="text-align: left">
                                    <th id="Th5" runat="server" style="width: 300px">Appartamento
                                    </th>
                                    <th id="Th2" runat="server" style="width: 120px">Data
                                    </th>
                                    <th id="Th7" runat="server" style="width: 60px">Provenienza
                                    </th>
                                    <th id="Th8" runat="server" style="width: 60px">Ospiti
                                    </th>
                                    <th id="Th1" runat="server" style="width: 200px">Cliente
                                    </th>
                                    <th id="Th4" runat="server" style="width: 100px">Approvato?
                                    </th>
                                      <th id="Th3" runat="server" style="width: 100px">visible homepage?
                                    </th>
                                    <th></th>
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
