<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="userReportDett.aspx.cs" Inherits="ModAuth.admin.modAuth.userReportDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <title>RentalInRome -
        <%=ltrTitle.Text %></title>
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
                                    Account (Produttore):
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_pidUser" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Tipo:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_repType" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Titolo:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_repTitle" Width="200px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_repTitle" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Data / Ora:
                                </td>
                                <td>
                                    <telerik:RadDateTimePicker ID="rdtp_repDateTime" runat="server">
                                        <DateInput DateFormat="dd/MM/yyyy HH:mm">
                                        </DateInput>
                                    </telerik:RadDateTimePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="rdtp_repDateTime" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Descrizione</span>
                    <div class="boxmodulo">
                        <telerik:RadEditor runat="server" ID="re_repDesc" SkinID="DefaultSetOfTools" Height="200" Width="600" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
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
