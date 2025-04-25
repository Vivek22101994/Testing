<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master"
    AutoEventWireup="true" CodeBehind="EstateExtrasDett.aspx.cs" Inherits="ModRental.admin.modRental.EstateExtrasDett" %>

<%@ Register Src="../modContent/UCgetImg.ascx" TagName="UCgetImg" TagPrefix="getImg" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <title>
        <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var minDate;
        $(document).ready(function () {
            dates();

            function dates() {


                $('#<%= txt_dtStart.ClientID %>').datepicker({ dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true,
                    onSelect: function () {
                        var date = $(this).datepicker('getDate');
                        var joindate = new Date(date);

                        var dd = joindate.getDate() + 2;
                        var mm = joindate.getMonth() + 1;
                        var y = joindate.getFullYear();
                        var joinFormattedDate = dd + '/' + mm + '/' + y;
                        minDate = new Date(new Date($('#<%= txt_dtStart.ClientID %>').datepicker('getDate')).getFullYear(), new Date($('#<%= txt_dtStart.ClientID %>').datepicker('getDate')).getMonth(), new Date($('#<%= txt_dtStart.ClientID %>').datepicker('getDate')).getDate() + 1);
                        $('#<%=txt_dtEnd.ClientID%>').datepicker("option", "minDate", minDate);
                        $('#<%=txt_dtEnd.ClientID%>').datepicker('setDate', joinFormattedDate);


                    }

                });


                $('#<%=txt_dtEnd.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true });

                $('div.ui-datepicker').css({ fontSize: '12px' });
            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(dates);
        });

        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <h1 class="titolo_main">
                <%= ltrTitle.Text%>
            </h1>
        </telerik:RadCodeBlock>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');"><span>Chiudi</span></a>
            </div>
            <div class="nulla">
            </div>
        </div>
        <div class="nulla">
        </div>
        <div class="mainline">
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Dati essenziali</span>
                    <div class="boxmodulo">
                        <table cellpadding="3" cellspacing="0">
                            <tr>
                                <td class="td_title">
                                    City:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_city" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                             <tr>
                                <td class="td_title">
                                    Proprietario
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_owner" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    MacroCategory:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_macroCategory" runat="server" OnSelectedIndexChanged="drp_macroCategory_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Category:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_category" runat="server" OnSelectedIndexChanged="drp_Category_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    SubCategory:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_subCategory" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Visibile nella scheda?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isImportant" runat="server">
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Nei filtri?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isInFilters" runat="server">
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <%--<tr>
                                <td class="td_title">
                                    Tariffazione:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_priceType" runat="server">
                                        <asp:ListItem Value="">Gratis</asp:ListItem>
                                        <asp:ListItem Value="forfait">forfait</asp:ListItem>
                                        <asp:ListItem Value="persona">persona</asp:ListItem>
                                        <asp:ListItem Value="notte">notte</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Prezzo:
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="txt_priceAmount" runat="server" Width="50">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="td_title">
                                    Obbligatorio?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isRequired" runat="server">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Pagato in anticipo?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isInstantPayment" runat="server">
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    In fattura?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isInInvoice" runat="server">
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Nei periodi limitati?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_hasPeriodLimits" runat="server" OnSelectedIndexChanged="drp_hasPeriodLimits_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    In Res Area?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isInResArea" runat="server">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Immagine anteprima</span>
                    <div class="boxmodulo">
                        <getImg:UCgetImg ID="imgPreview" runat="server" />
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainbox" id="div1" runat="server">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Estates:</span>
                    <div class="boxmodulo">
                        <table cellpadding="3" cellspacing="0">
                            <tr>
                                <td class="td_title">
                                    per tutti gli appartamenti ?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_all_estate" runat="server" OnSelectedIndexChanged="drp_all_estate_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <asp:CheckBoxList ID="chk_estates" runat="server" Visible="false" RepeatColumns="10">
                                </asp:CheckBoxList>
                            </tr>
                        </table>
                    </div>
                    <div class="bottom">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
            <div class="mainbox" id="div_period" runat="server" visible="false">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Periodo:</span>
                    <div class="boxmodulo">
                        <table cellpadding="3" cellspacing="0">
                            <tr>
                                <td class="td_title">
                                    Data di inizio:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_dtStart" Style="width: 120px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    data di fine:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_dtEnd" Style="width: 120px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    giorni di chiusura:
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="chkClosingDays" runat="server">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                        <div class="salvataggio">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnkSaveService" runat="server" OnClick="lnkSaveService_Click"><span>Save Services</span></asp:LinkButton></div>
                        </div>
                    </div>
                    <asp:ListView ID="LVService" runat="server" OnItemDataBound="LvService_ItemDataBound"
                        OnItemCommand="LvService_ItemCommand">
                        <ItemTemplate>
                            <tr style="">
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_dtStart"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                    <asp:Label runat="server" ID="lbl_dtEnd"> </asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblClosingDays"></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteService" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');">Elimina</asp:LinkButton>
                                </td>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr>
                                    <th style="width: 100px" align="left">
                                        Data di inizio
                                    </th>
                                    <th style="width: 100px" align="left">
                                        data di fine
                                    </th>
                                    <th style="width: 100px" align="left">
                                        giorni di chiusura
                                    </th>
                                    <th>
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                    </asp:ListView>
                    <div class="bottom">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td colspan="2">
                                    <asp:HiddenField ID="HfLang" Value="1" runat="server" />
                                    <asp:ListView ID="LvLangs" runat="server" OnItemCommand="LvLangs_ItemCommand">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLang" CommandName="change_lang" runat="server" CssClass='<%# HfLang.Value == "" + Eval("id") ? "tab_item_current" : "tab_item"%>'>
                                                <span>
                                                    <%# Eval("title") %></span>
                                            </asp:LinkButton>
                                            <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <div class="menu2">
                                                <a id="itemPlaceholder" runat="server" />
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Nome in lingua:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_title" Width="230px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Sommario:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_Sommario" Width="499px" TextMode="MultiLine"
                                        Columns="100" Rows="10" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                    Descrizione:<br />
                                    <telerik:RadEditor runat="server" StripFormattingOnPaste="AllExceptNewLines" ID="txt_description"
                                        SkinID="DefaultSetOfTools" Height="400" Width="500" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                        <CssFiles>
                                            <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                        </CssFiles>
                                    </telerik:RadEditor>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
