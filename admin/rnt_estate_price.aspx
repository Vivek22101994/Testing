<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_price.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_price" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #main .riTextBox {
            padding: 1px 2px;
        }

        .linebottom td {
            border-bottom: 1px dotted;
            padding: 5px;
        }

        .linebottom strong {
            display: inline-block;
            margin-top: 3px;
        }
    </style>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var rwdUrl = null;
            function rwdUrl_OnClientClose(sender, eventArgs) {
            }
            function openSeasonGroupEdit(id) {
                if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
                url = "SeasonGroupEdit.aspx?id=" + id + "&callback=onSeasonGroupCreated";
                var width = 700;
                var height = $(window).height();
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(width);
                rwdUrl.set_minHeight(height);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                //rwdUrl.maximize();
                return false;
            }
            function onSeasonGroupCreated(id) {
                $find('<%= pnl_pidSeasonGroup.ClientID %>').ajaxRequest('' + id);
                if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
                rwdUrl.close();
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="" runat="server" />
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_VIEW_ESTATE_PRICEs" OrderBy="dtStart desc, priority desc" Where="pid_estate == @pid_estate &amp;&amp; is_active == @is_active">
                <WhereParameters>
                    <asp:ControlParameter ControlID="HF_IdEstate" Name="pid_estate" PropertyName="Value" Type="Int32" />
                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
            <h1 class="titolo_main">Prezzi della struttura:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal></h1>
            <div id="fascia1">
                <div style="clear: both; margin: 3px 0 5px 30px;">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Panel ID="pnlContent" runat="server" Width="100%">
                <!-- INIZIO MAIN LINE -->
                <div class="mainline">
                    <!-- BOX 1 -->
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                            </div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                            </div>
                        </div>
                        <div class="center">
                            <span class="titoloboxmodulo" style="margin-bottom: 5px;">Stagionalità della struttura</span>
                            <telerik:RadAjaxPanel ID="pnl_pidSeasonGroup" runat="server" CssClass="boxmodulo" OnAjaxRequest="pnl_pidSeasonGroup_AjaxRequest">
                                <asp:DropDownList ID="drp_pidSeasonGroup" runat="server">
                                </asp:DropDownList>
                                <a href="#" onclick="openSeasonGroupEdit(-1);  return false;" class="inlinebtn">+ Nuova</a>
                            </telerik:RadAjaxPanel>
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td colspan="2">Max persone per Prezzo Base:
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txt_pr_basePersons" runat="server" Style="width: 80px"></asp:TextBox>&nbsp;pax
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator20" ControlToValidate="txt_pr_basePersons" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator20" ControlToValidate="txt_pr_basePersons" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="border: 1px dotted; padding: 5px;">
                                            <a id="lnk_pr_basePersons" class="inlinebtn" onclick="toggle_pr_basePersons()" style="cursor: pointer;">Vedi Note</a>
                                            <br />
                                            <div id="div_pr_basePersons" style="display: none;">
                                                <script type="text/javascript">
                                                    function toggle_pr_basePersons() {
                                                        var tmp = $("#div_pr_basePersons");
                                                        if (tmp.css("display") == "none") {
                                                            tmp.css("display", "block");
                                                            $("#lnk_pr_basePersons").html("Nascondi Note");
                                                        }
                                                        else {
                                                            tmp.css("display", "none");
                                                            $("#lnk_pr_basePersons").html("Vedi Note");
                                                        }
                                                    }
                                                </script>
                                                Prezzo base indica il prezzo minimo che viene accettato.<br />
                                                Max persone per Prezzo Base indica il numero persone ospitate senza Letto aggiuntivo.<br />
                                                Es:<br />
                                                Max persone per Prezzo Base: <strong>4</strong><br />
                                                Prezzo base: <strong>150</strong>&euro;<br />
                                                Letto aggiuntivo: <strong>30</strong>&euro;<br />
                                                <br />
                                                1 pax = 150&euro;,<br />
                                                2 pax = 150&euro;,<br />
                                                3 pax = 150&euro;,<br />
                                                4 pax = 150&euro;,<br />
                                                5 pax = 180&euro;,<br />
                                                6 pax = 210&euro;,<br />
                                                7 pax = 240&euro;<br />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" class="td_title">
                                            <span class="titoloboxmodulo">Bassa Stagione</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Prezzo (2pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_1" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ntxt_price_1" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td class="td_title">Letto aggiuntivo (+1pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_optional_1" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ntxt_price_optional_1" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" class="td_title">
                                            <span class="titoloboxmodulo">Media Stagione </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Prezzo (2pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_4" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator21" ControlToValidate="ntxt_price_4" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td class="td_title">Letto aggiuntivo (+1pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_optional_4" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator22" ControlToValidate="ntxt_price_optional_4" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <span class="titoloboxmodulo">Alta Stagione</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Prezzo (2pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_2" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="ntxt_price_2" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td class="td_title">Letto aggiuntivo (+1pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_optional_2" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="ntxt_price_optional_2" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <span class="titoloboxmodulo">Altissima Stagione ( +20%)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Prezzo (2pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_3" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="ntxt_price_3" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td class="td_title">Letto aggiuntivo (+1pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_optional_3" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="ntxt_price_optional_3" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="5" class="td_title">
                                            <span class="titoloboxmodulo">LastMinute</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Ore prima del checkin:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_lm_inhours" runat="server" Style="width: 80px"></asp:TextBox>&nbsp;ore
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" ControlToValidate="txt_lm_inhours" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" ControlToValidate="txt_lm_inhours" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td class="td_title">Sconto:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_lm_discount" runat="server" Style="width: 80px"></asp:TextBox>&nbsp;%
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ControlToValidate="txt_lm_discount" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" ControlToValidate="txt_lm_discount" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">Min notti:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_lm_nights_min" runat="server" Style="width: 80px"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" ControlToValidate="txt_lm_nights_min" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" ControlToValidate="txt_lm_nights_min" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td class="td_title">Max notti:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_lm_nights_max" runat="server" Style="width: 80px"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator13" ControlToValidate="txt_lm_nights_max" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" ControlToValidate="txt_lm_nights_max" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" class="td_title">
                                            <span class="titoloboxmodulo">Sconto a periodi lunghi</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            <strong>Sistema:</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_pr_dcSUsed" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_pr_dcSUsed_SelectedIndexChanged">
                                                <asp:ListItem Value="1">Standard</asp:ListItem>
                                                <asp:ListItem Value="2">Nuovo Pers.</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr id="pnl_dcSUsed_1" runat="server">
                                        <td colspan="5">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="td_title">Settimanale (7gg):
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_pr_discount7days" runat="server" Style="width: 80px"></asp:TextBox>&nbsp;%
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="txt_pr_discount7days" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" ControlToValidate="txt_pr_discount7days" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="width: 30px;"></td>
                                                    <td class="td_title" runat="server" visible="false">Mensile (30gg):
                                                        <br />
                                                        *non utilizzato
                                                    </td>
                                                    <td runat="server" visible="false">
                                                        <asp:TextBox ID="txt_pr_discount30days" runat="server" Style="width: 80px"></asp:TextBox>&nbsp;%
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="txt_pr_discount30days" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" ControlToValidate="txt_pr_discount30days" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <strong>Attenzione!</strong><br />
                                                        Lo sconto inserito si applica per Ogni 7 notti della prenotazione.<br />
                                                        Es:<br />
                                                        Settimanale (7gg): <strong>5</strong> %<br />
                                                        <br />
                                                        in un pren. di 6 notti il prezzo si calcola senza sconto,<br />
                                                        7 notti - (7 notti scontato di 5%)<br />
                                                        8 notti - (7 notti scontato di 5%) + 1 notte senza sconto<br />
                                                        13 notti - (7 notti scontato di 5%) + 6 notti senza sconto<br />
                                                        14 notti - (14 notti scontato di 5%)<br />
                                                        15 notti - (14 notti scontato di 5%) + 1 notte senza sconto<br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="pnl_dcSUsed_2" runat="server" visible="false">
                                        <td colspan="5">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 80px;"></td>
                                                    <td style="width: 100px;"></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="border-bottom: thin dotted;">
                                                        <strong>Periodo 1:</strong>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:TextBox ID="txt_pr_dcS2_1_inDays" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;notti
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" ControlToValidate="txt_pr_dcS2_1_inDays" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator14" ControlToValidate="txt_pr_dcS2_1_inDays" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:TextBox ID="txt_pr_dcS2_1_percent" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;%
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator15" ControlToValidate="txt_pr_dcS2_1_percent" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator15" ControlToValidate="txt_pr_dcS2_1_percent" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="border-bottom: thin dotted;">
                                                        <strong>Periodo 2:</strong>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:TextBox ID="txt_pr_dcS2_2_inDays" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;notti
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator16" ControlToValidate="txt_pr_dcS2_2_inDays" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator16" ControlToValidate="txt_pr_dcS2_2_inDays" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:TextBox ID="txt_pr_dcS2_2_percent" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;%
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator17" ControlToValidate="txt_pr_dcS2_2_percent" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator17" ControlToValidate="txt_pr_dcS2_2_percent" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="border-bottom: thin dotted;">
                                                        <strong>Periodo 3:</strong>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:TextBox ID="txt_pr_dcS2_3_inDays" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;notti
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator18" ControlToValidate="txt_pr_dcS2_3_inDays" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator18" ControlToValidate="txt_pr_dcS2_3_inDays" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:TextBox ID="txt_pr_dcS2_3_percent" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;%
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator19" ControlToValidate="txt_pr_dcS2_3_percent" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator19" ControlToValidate="txt_pr_dcS2_3_percent" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr id="Tr2" runat="server" visible="false">
                                                    <td style="border-bottom: thin dotted;">
                                                        <strong>Periodo 4:</strong>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:TextBox ID="txt_pr_dcS2_4_inDays" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;notti
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator44" ControlToValidate="txt_pr_dcS2_4_inDays" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator44" ControlToValidate="txt_pr_dcS2_4_inDays" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:TextBox ID="txt_pr_dcS2_4_percent" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;%
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator45" ControlToValidate="txt_pr_dcS2_4_percent" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator45" ControlToValidate="txt_pr_dcS2_4_percent" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr id="Tr3" runat="server" visible="false">
                                                    <td style="border-bottom: thin dotted;">
                                                        <strong>Periodo 5:</strong>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:TextBox ID="txt_pr_dcS2_5_inDays" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;notti
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator54" ControlToValidate="txt_pr_dcS2_5_inDays" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator54" ControlToValidate="txt_pr_dcS2_5_inDays" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:TextBox ID="txt_pr_dcS2_5_percent" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;%
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator55" ControlToValidate="txt_pr_dcS2_5_percent" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator55" ControlToValidate="txt_pr_dcS2_5_percent" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr id="Tr4" runat="server" visible="false">
                                                    <td style="border-bottom: thin dotted;">
                                                        <strong>Periodo 6:</strong>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:TextBox ID="txt_pr_dcS2_6_inDays" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;notti
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator64" ControlToValidate="txt_pr_dcS2_6_inDays" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator64" ControlToValidate="txt_pr_dcS2_6_inDays" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:TextBox ID="txt_pr_dcS2_6_percent" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;%
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator65" ControlToValidate="txt_pr_dcS2_6_percent" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator65" ControlToValidate="txt_pr_dcS2_6_percent" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr id="Tr5" runat="server" visible="false">
                                                    <td style="border-bottom: thin dotted;">
                                                        <strong>Periodo 7:</strong>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:TextBox ID="txt_pr_dcS2_7_inDays" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;notti
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator74" ControlToValidate="txt_pr_dcS2_7_inDays" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator74" ControlToValidate="txt_pr_dcS2_7_inDays" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:TextBox ID="txt_pr_dcS2_7_percent" runat="server" Style="width: 30px"></asp:TextBox>&nbsp;%
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator75" ControlToValidate="txt_pr_dcS2_7_percent" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator75" ControlToValidate="txt_pr_dcS2_7_percent" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <strong>Attenzione!</strong><br />
                                                        Gli sconti inseriti si applicano per il periodo intero della prenotazione.<br />
                                                        Es:<br />
                                                        P.1 - Dopo <strong>6</strong> notti Sconto <strong>5</strong> %<br />
                                                        P.2 - Dopo <strong>9</strong> notti Sconto <strong>10</strong> %<br />
                                                        P.3 - Dopo <strong>14</strong> notti Sconto <strong>15</strong> %<br />
                                                        <br />
                                                        in un pren. di 6 notti il prezzo si calcola senza sconto,<br />
                                                        7 notti - 5% di sconto<br />
                                                        9 notti - 5% di sconto<br />
                                                        10 notti - 10% di sconto<br />
                                                        14 notti - 10% di sconto<br />
                                                        15 notti e più - 15% di sconto 
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <div class="salvataggio">
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click" CausesValidation="true" TabIndex="28" ValidationGroup="price"><span>Salva Modifiche</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla</span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <span class="titoloboxmodulo" id="lbl_changeSaved" runat="server" visible="false">Le modifiche sono state salvate...</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="bottom">
                            <div style="float: left;">
                                <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                            </div>
                            <div style="float: right;">
                                <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                            </div>
                        </div>
                    </div>
                    <div class="salvataggio">
                        <div class="nulla">
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <script type="text/javascript">
                
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
        <script type="text/javascript">
            var _JSCal_Range;
            function setCal(dtStartInt, dtEndInt) {
                _JSCal_Range = new JSCal.Range({ startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart", startDateInt: dtStartInt, endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd", endDateInt: dtEndInt, beforeShowDay: CheckDisabledDate });
            }
        </script>
        <table border="0" cellspacing="3" cellpadding="0">
            <tr>
                <td class="td_title">Stagione:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="drp_period" DataTextField="title" DataValueField="id" />
                </td>
            </tr>
            <tr>
                <td class="td_title">Data di inizio:
                </td>
                <td>
                    <input id="txt_dtStart" type="text" readonly="readonly" style="width: 150px" />
                    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td class="td_title">Data di fine:
                </td>
                <td>
                    <input id="txt_dtEnd" type="text" readonly="readonly" style="width: 150px" />
                    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td class="td_title">Prezzo (2pax):
                </td>
                <td>&euro;&nbsp;<input type="text" runat="server" id="txt_price" style="width: 120px" />
                    <asp:RequiredFieldValidator runat="server" ID="RFV_1" ControlToValidate="txt_price" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="REV_txt_price" ControlToValidate="txt_price" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="td_title">Letto aggiuntivo (+1pax):
                </td>
                <td>&euro;&nbsp;<input type="text" runat="server" id="txt_price_optional" style="width: 120px" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txt_price_optional" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txt_price_optional" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
