<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="SeasonDatesEdit.aspx.cs" Inherits="ModRental.admin.modRental.SeasonDatesEdit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HF_SeasonGroup" Value="0" runat="server" />
        <asp:HiddenField ID="HF_currDtInt" Value="0" runat="server" />
        <h1 class="titolo_main">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>
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
                        <table>
                            <tr>
                                <td>
                                    <span class="titoloboxmodulo" style="margin-bottom: 5px;">Stagione precedente</span>
                                </td>
                                <td rowspan="3" style="width: 20px;">
                                </td>
                                <td>
                                    <span class="titoloboxmodulo" style="margin-bottom: 5px;">Stagione nella data selezionata</span>
                                </td>
                                <td rowspan="3" style="width: 20px;">
                                </td>
                                <td>
                                    <span class="titoloboxmodulo" style="margin-bottom: 5px;">Stagione successiva</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: middle;">
                                    <asp:Literal ID="ltr_beforeTitle" runat="server"></asp:Literal>
                                </td>
                                <td style="vertical-align: middle;">
                                    <asp:Literal ID="ltr_thisTitle" runat="server"></asp:Literal>
                                </td>
                                <td style="vertical-align: middle;">
                                    <asp:Literal ID="ltr_afterTitle" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: middle;">
                                    <asp:Literal ID="ltr_beforeDates" runat="server"></asp:Literal>
                                </td>
                                <td style="vertical-align: middle;">
                                    <asp:Literal ID="ltr_thisDates" runat="server"></asp:Literal>
                                </td>
                                <td style="vertical-align: middle;">
                                    <asp:Literal ID="ltr_afterDates" runat="server"></asp:Literal>
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
                    <span class="titoloboxmodulo">Aggiorna periodo</span>
                    <div class="boxmodulo">
                        <table>
                            <tr>
                                <td>
                                    dal:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_dtStart" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    al:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_dtEnd" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Stagione
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_pidPeriod" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <strong>Nota bene,</strong><br />
                                    Tutti i periodi nel range di date che selezioni
                                    <br />
                                    vengono sovrascritte con una nuova stagione
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:LinkButton ID="lnk_fileNew" runat="server" CssClass="inlinebtn" OnClick="lnk_saveMain_Click">Salva Stagione</asp:LinkButton>
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
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
