<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master"
    AutoEventWireup="true" CodeBehind="EstateExtrasList.aspx.cs" Inherits="ModRental.admin.modRental.EstateExtrasList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "EstateExtrasDett.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                //rwdUrl.set_minWidth(700);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                rwdUrl.maximize();
            }

            if (type == "price") {
                url = "EstateExtraPrice.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                //rwdUrl.set_minWidth(700);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                rwdUrl.maximize();
            }
            if (type == "priceType") {
                url = "EstateExtraPriceType.aspx?id=" + id;
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
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false"
        OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false"
        VisibleTitlebar="false">
    </telerik:RadWindow>
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>
                    Gestione Accessori per Strutture
                </h1>
                <div class="bottom_agg">
                    <a href="#" onclick="return setUrl('dett', '0')" title="Crea nuovo"><span>+ Nuovo</span>
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
                        <a class="filto_bottone" style="display: none;" id="lnk_showfilt" onclick="showFilter();">
                            FILTRA</a> <a class="filto_bottone2" id="lnk_hidefilt" onclick="hideFilter();">FILTRA</a>
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
                                                        <asp:TextBox ID="txt_flt_title" runat="server" Width="50" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>
                                                            Visibile nella scheda:</label>
                                                        <asp:DropDownList ID="drp_flt_isImportant" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                            <asp:ListItem Value="0">NO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>
                                                            Nei filtri:</label>
                                                        <asp:DropDownList ID="drp_flt_isInFilters" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                            <asp:ListItem Value="0">NO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <%-- <td>
                                                        <label>
                                                            Tariffazione:</label>
                                                        <asp:DropDownList ID="drp_flt_priceType" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="forfait">forfait</asp:ListItem>
                                                            <asp:ListItem Value="persona">persona</asp:ListItem>
                                                            <asp:ListItem Value="notte">notte</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>--%>
                                                    <td>
                                                        <label>
                                                            Obbligatorio:</label>
                                                        <asp:DropDownList ID="drp_flt_isRequired" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                            <asp:ListItem Value="0">NO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>
                                                            Pagato in anticipo:</label>
                                                        <asp:DropDownList ID="drp_flt_isInstantPayment" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                            <asp:ListItem Value="0">NO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>
                                                            In fattura:</label>
                                                        <asp:DropDownList ID="drp_flt_isInInvoice" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                            <asp:ListItem Value="0">NO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>
                                                            Nei periodi limitati:</label>
                                                        <asp:DropDownList ID="drp_flt_hasPeriodLimits" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                            <asp:ListItem Value="0">NO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                     <td>
                                                        <label>
                                                            Categoria:</label>
                                                        <asp:DropDownList ID="drp_flt_category" runat="server" CssClass="inp" OnSelectedIndexChanged="drp_flt_category_SelectedIndexChanged" AutoPostBack="true" style="width:200px;">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                     <td>
                                                        <label>
                                                            Sottocategoria:</label>
                                                        <asp:DropDownList ID="drp_flt_subcategory" runat="server" CssClass="inp" style="width:300px;">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
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
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCmodRental"
                    TableName="dbRntEstateExtrasVIEWs" Where="pidLang == 1" OrderBy="title" EntityTypeName="">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand"
                    OnItemDataBound="LV_ItemDataBound">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_extra" runat="server">
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("title")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_city" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_macroCategory" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_category" runat="server"></asp:Label>
                                <%--<asp:Label ID="lbl_pidCategory" runat="server" style="display:none" Text='<%# Eval("pidCategory") %>'></asp:Label>--%>
                            </td>
                            <td>
                                <asp:Label ID="lbl_subCategory" runat="server"></asp:Label>
                                <%--<asp:Label ID="lbl_pidSubCategory" runat="server" style="display:none" Text='<%# Eval("pidSubCategory") %>'></asp:Label>--%>
                            </td>
                            <%-- <td>
                                <span>
                                    <%# Eval("priceType") + "" != "" ? "" + Eval("priceType") + " &euro;" + Eval("priceAmount").objToDecimal().ToString("N2") + "" : "Gratis"%></span>
                            </td>--%>
                            <td>
                                <%# Eval("isImportant").objToInt32() > 0 ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("isInFilters").objToInt32() > 0 ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("isRequired") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("isInstantPayment") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("isInInvoice") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("hasPeriodLimits") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <asp:Label ID="lbl_resArea" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_price" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda"
                                    style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');"
                                    Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
                            </td>
                            <td>
                                <a href='EstateExtraMedia.aspx?id=<%# Eval("id") %>' class="media" title="Multimedia"
                                    style="text-decoration: none; border: 0 none; margin: 5px;">Media</a>
                            </td>
                            <td>
                                <a href="#" onclick="return setUrl('price', '<%# Eval("id") %>')" title="aggiungere prezzo"
                                    style="text-decoration: none; border: 0 none; margin: 5px;">Gestione Prezzi</a>
                            </td>
                            <td>
                                <a href="#" onclick="return setUrl('priceType', '<%# Eval("id") %>')" title="aggiungere prezzo"
                                    style="text-decoration: none; border: 0 none; margin: 5px;">Gestione categoria Prezzi</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_extra" runat="server">
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("title")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_city" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_macroCategory" runat="server"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:Label ID="lbl_category" runat="server"></asp:Label>
                                <%--<asp:Label ID="lbl_pidCategory" runat="server" style="display:none" Text='<%# Eval("pidCategory") %>'></asp:Label>--%>
                            </td>
                            <td>
                                <asp:Label ID="lbl_subCategory" runat="server"></asp:Label>
                                <%--<asp:Label ID="lbl_pidSubCategory" runat="server" style="display:none" Text='<%# Eval("pidSubCategory") %>'></asp:Label>--%>
                            </td>
                            <%-- <td>
                                <span>
                                    <%# Eval("priceType") + "" != "" ? "" + Eval("priceType") + " &euro;" + Eval("priceAmount").objToDecimal().ToString("N2") + "" : "Gratis"%></span>
                            </td>--%>
                            <td>
                                <%# Eval("isImportant").objToInt32() > 0 ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("isInFilters").objToInt32() > 0 ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("isRequired") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("isInstantPayment") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("isInInvoice") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <%# Eval("hasPeriodLimits") + "" == "1" ? "<span style='color: #00FF00'>Si</span>" : "<span style='color: #FF0000'>No</span>"%>
                            </td>
                            <td>
                                <asp:Label ID="lbl_resArea" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_price" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda"
                                    style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');"
                                    Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
                            </td>
                            <td>
                                <a href='EstateExtraMedia.aspx?id=<%# Eval("id") %>' class="media" title="Multimedia"
                                    style="text-decoration: none; border: 0 none; margin: 5px;">Media</a>
                            </td>
                            <td>
                                <a href="#" onclick="return setUrl('price', '<%# Eval("id") %>')" title="aggiungere prezzo"
                                    style="text-decoration: none; border: 0 none; margin: 5px;">Gestione Prezzi</a>
                            </td>
                            <td>
                                <a href="#" onclick="return setUrl('priceType', '<%# Eval("id") %>')" title="aggiungere prezzo"
                                    style="text-decoration: none; border: 0 none; margin: 5px;">Gestione categoria Prezzi</a>
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
                                    <th style="width: 30px">
                                        ID
                                    </th>
                                    <th style="width: 300px">
                                        Nome
                                    </th>
                                    <th style="width: 100px">
                                        City
                                    </th>
                                    <th style="width: 100px">
                                        MacroCategory
                                    </th>
                                    <th style="width: 100px">
                                        Category
                                    </th>
                                    <th style="width: 100px">
                                        SubCategory
                                    </th>
                                    <%-- <th style="width: 100px">
                                        Tariffazione
                                    </th>--%>
                                    <th style="width: 50px">
                                        Nella scheda?
                                    </th>
                                    <th style="width: 50px">
                                        Nei filtri?
                                    </th>
                                    <th style="width: 50px">
                                        Obbligatorio?
                                    </th>
                                    <th style="width: 50px">
                                        Pagato in anticipo?
                                    </th>
                                    <th style="width: 50px">
                                        In fattura?
                                    </th>
                                    <th style="width: 50px">
                                        Nei periodi limitati?
                                    </th>
                                    <th style="width: 50px">
                                        Is In Res Area?
                                    </th>
                                    <th style="width: 50px">
                                        Has Price?
                                    </th>
                                    <th colspan="12">
                                    </th>
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
