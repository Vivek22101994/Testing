<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="DiscountTypeDett.aspx.cs" Inherits="ModRental.admin.modRental.DiscountTypeDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <h1 class="titolo_main">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');">
                    <span>Chiudi</span></a>
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
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">
                                    Codice:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_code" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Sconto Base:
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="ntxt_fase1_discount" runat="server" Width="50" MaxValue="50" MinValue="0" Type="Percent">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ntxt_fase1_discount" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                        <tr>
                                            <td colspan="3">
                                                <span class="titoloboxmodulo" style="margin: 10px 0 0;">Sconti Extra</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                
                                            </td>
                                            <th style="text-align: center;">
                                                A partire da
                                            </th>
                                            <th>
                                                Sconto
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                Step 2
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase2_start" runat="server" Width="80" MinValue="0" Type="Currency" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase2_discount" runat="server" Width="30" MaxValue="50" MinValue="0" Type="Percent">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Step 3
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase3_start" runat="server" Width="80" MinValue="0" Type="Currency" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase3_discount" runat="server" Width="30" MaxValue="50" MinValue="0" Type="Percent">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Step 4
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase4_start" runat="server" Width="80" MinValue="0" Type="Currency" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase4_discount" runat="server" Width="30" MaxValue="50" MinValue="0" Type="Percent">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Step 5
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase5_start" runat="server" Width="80" MinValue="0" Type="Currency" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase5_discount" runat="server" Width="30" MaxValue="50" MinValue="0" Type="Percent">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Step 6
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase6_start" runat="server" Width="80" MinValue="0" Type="Currency" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase6_discount" runat="server" Width="30" MaxValue="50" MinValue="0" Type="Percent">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Step 7
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase7_start" runat="server" Width="80" MinValue="0" Type="Currency" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase7_discount" runat="server" Width="30" MaxValue="50" MinValue="0" Type="Percent">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Step 8
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase8_start" runat="server" Width="80" MinValue="0" Type="Currency" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase8_discount" runat="server" Width="30" MaxValue="50" MinValue="0" Type="Percent">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Step 9
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase9_start" runat="server" Width="80" MinValue="0" Type="Currency" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase9_discount" runat="server" Width="30" MaxValue="50" MinValue="0" Type="Percent">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Step 10
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase10_start" runat="server" Width="80" MinValue="0" Type="Currency" Style="text-align: right;">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="ntxt_fase10_discount" runat="server" Width="30" MaxValue="50" MinValue="0" Type="Percent">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="center" id="pnl_faseEdit" runat="server" visible="false">
                    <span class="titoloboxmodulo">
                        <asp:Literal ID="ltr_faseEditTitle" runat="server"></asp:Literal>
                    </span>
                    <div class="boxmodulo">
                        <asp:HiddenField ID="HF_faseEdit" runat="server" />
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">
                                    Da:
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="ntxt_start" runat="server" Width="120" MinValue="0" Type="Currency">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ntxt_start" ErrorMessage="<br/>//obbligatorio" ValidationGroup="fase" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    A:
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="ntxt_end" runat="server" Width="120" MinValue="0" Type="Currency">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ntxt_end" ErrorMessage="<br/>//obbligatorio" ValidationGroup="fase" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Commissioni:
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="ntxt_discount" runat="server" Width="50" MaxValue="100" MinValue="0" Type="Percent">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ntxt_discount" ErrorMessage="<br/>//obbligatorio" ValidationGroup="fase" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:LinkButton ID="lnk_faseSave" runat="server" CssClass="inlinebtn" OnClick="lnk_faseSave_Click">Salva</asp:LinkButton>
                                    <asp:LinkButton ID="lnk_faseCancel" runat="server" CssClass="inlinebtn" OnClick="lnk_faseCancel_Click">Annulla</asp:LinkButton>
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
                    <span class="titoloboxmodulo">Descrizione</span>
                    <div class="boxmodulo">
                        <telerik:RadEditor runat="server" ID="re_description" SkinID="DefaultSetOfTools" Height="200" Width="600" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
                            <CssFiles>
                                <telerik:EditorCssFile Value="" />
                            </CssFiles>
                        </telerik:RadEditor>
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
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
