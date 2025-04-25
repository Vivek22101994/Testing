<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateExtraSubCategoryList.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateExtraSubCategoryList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id, category) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

        var url;
        if (type == "dett") {

            if (category == "") {
                category = document.getElementById('<%= HfCatId.ClientID %>').value;
            }

            url = "EstateExtraSubCategoryDett.aspx?id=" + id + "&category=" + category;
            rwdUrl.set_autoSize(false);
            rwdUrl.set_visibleTitlebar(true);
            //rwdUrl.set_minWidth(700);
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
            <asp:HiddenField ID="HfCatId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>
                    <asp:Label ID="lbl_title" runat="server"></asp:Label>
                </h1>
                <telerik:RadCodeBlock ID="rcbMenu" runat="server">
                    <div id="header_admin" style="height: 30px;">
                        <div id="menu">
                            <div id="chromemenu" class="chromestyle" style="margin-left: 20px; margin-top: 0px; margin-bottom: 0px;">
                                <ul>
                                    <li><a href="EstateExtraMacroCategoryList.aspx?id=0">Macrocategoria</a> </li>
                                    <li><a href="EstateExtraCategoryList.aspx?id=0">Categoria</a> </li>
                                    <li><a href="EstateExtraSubCategoryList.aspx?id=0">SottoCategoria</a> </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </telerik:RadCodeBlock>
                <div class="bottom_agg">
                    <a href="#" onclick="return setUrl('dett', '0','0')" title="Crea nuovo"><span>+ Nuovo</span> </a>
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
                                                        <label>
                                                            Nome:</label>
                                                        <asp:TextBox ID="txt_flt_title" runat="server" Width="300" CssClass="inp"></asp:TextBox>
                                                    </td>

                                                    <td>
                                                        <label>
                                                            Attivo:</label>
                                                        <asp:DropDownList ID="drp_isActive" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                            <asp:ListItem Value="0">NO</asp:ListItem>
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
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCmodRental" TableName="dbRntExtrasSubCategoryTBs" OrderBy="code" EntityTypeName="">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("code")%></span>
                            </td>

                            <td>
                                <%# Eval("isActive") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>

                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>','<%# Eval("pidCategory") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
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
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("code")%></span>
                            </td>

                            <td>
                                <%# Eval("isActive") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>

                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>','<%# Eval("pidCategory") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
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
                                    <th style="width: 30px">ID </th>
                                    <th style="width: 300px">Nome </th>
                                    <th style="width: 100px">Active?
                                    </th>


                                    <th colspan="2"></th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                        <div class="page">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;">
                                <Fields>
                                    <asp:NumericPagerField />
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
</asp:Content>
