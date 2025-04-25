<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master"
    AutoEventWireup="true" CodeBehind="EstateExtraPriceType.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateExtraPriceType" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>
        <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <asp:HiddenField ID="hfd_price" Value="0" runat="server" />
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
            <div class="mainbox" id="div_price" runat="server">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Tipo Prezzo:</span>
                    <div class="boxmodulo">
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
                                        Titolo sub :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SubTitle" runat="server"></asp:TextBox>
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
                    <asp:ListView ID="LVPrice" runat="server" OnItemCommand="LvPrice_ItemCommand">
                        <ItemTemplate>
                            <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                <%--<td align="center">
                                    <asp:Label runat="server" ID="lbl_group"></asp:Label>
                                </td>--%>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_id" Text='<%#Eval("pidPriceType") %>' Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="lbl_min_pax" Text='<%#Eval("title") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_max_pax" Text='<%#Eval("subTitle") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_hour" Text='<%#Eval("description") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditPrice" Text="Scheda"
                                        title="Apri Scheda" Style="text-decoration: none; border: 0 none; margin: 5px;
                                        color: #000000;"></asp:LinkButton>
                                </td>
                                <td align="center">
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeletePrice" Text="Elimina"
                                        OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');"
                                        Style="text-decoration: none; border: 0 none; margin: 5px; color: #000000;"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                <%--<td align="center">
                                    <asp:Label runat="server" ID="lbl_group"></asp:Label>
                                </td>--%>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_id" Text='<%#Eval("pidPriceType") %>' Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="lbl_min_pax" Text='<%#Eval("title") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_max_pax" Text='<%#Eval("subTitle") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_hour" Text='<%#Eval("description") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditPrice" title="Apri Scheda"
                                        Style="text-decoration: none; border: 0 none; margin: 5px; color: #000000;">Scheda</asp:LinkButton>
                                </td>
                                <td align="center">
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeletePrice" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');"
                                        Style="text-decoration: none; border: 0 none; margin: 5px; color: #000000;">Elimina</asp:LinkButton>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <div style="margin-top: 30px;">
                                <table border="0" cellpadding="0" cellspacing="0" style="">
                                    <tr style="">
                                        <%-- <th style="width: 70px;border-bottom: 1px solid #DFDFDF;" align="left">
                                        Gruppo
                                    </th>--%>
                                        <th style="width: 130px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Titalo
                                        </th>
                                        <th style="width: 130px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Titalo Sub
                                        </th>
                                        <th style="width: 130px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Descrizione
                                        </th>
                                        <th style="border-bottom: 1px solid #DFDFDF;">
                                        </th>
                                        <th style="border-bottom: 1px solid #DFDFDF;">
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </div>
                        </LayoutTemplate>
                    </asp:ListView>
                    <asp:HiddenField ID="hfd_cat" runat="server" />
                    <div class="bottom">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
