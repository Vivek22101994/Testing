<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="agentClientDett.aspx.cs" Inherits="RentalInRome.reservationarea.agentClientDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <title>RentalInRome -
        <%=ltrTitle.Text %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background-image: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <asp:HiddenField ID="HF_reloadDrp" Value="0" runat="server" />
        <asp:HiddenField ID="HF_idAgent" runat="server" Value="" Visible="false" />
        <h1 class="titolo_main" style="color: #FE6634;">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span><%=CurrentSource.getSysLangValue("lblSaveChanges")%></span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span><%=CurrentSource.getSysLangValue("lblCancelChanges")%></span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');">
                    <span>Close</span></a>
            </div>
            <div class="nulla">
            </div>
        </div>
        <div class="nulla">
        </div>
        <div class="mainline mainline2">
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <div class="mainbox">
                    <div class="center">
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("reqFullName")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_nameFull" Width="200px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_nameFull" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// Required"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo" style="margin-top: 20px; margin-bottom: 0;">
                            <%=CurrentSource.getSysLangValue("pdf_IndirizzoResidenza")%>
                        </span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td colspan="3">
                                        <%=CurrentSource.getSysLangValue("lblAddress")%>:<br />
                                        <asp:TextBox runat="server" ID="txt_locAddress" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%=CurrentSource.getSysLangValue("lblZipCode")%>:<br />
                                        <asp:TextBox runat="server" ID="txt_locZipCode" Width="100px" />
                                    </td>
                                    <td>
                                        <%=CurrentSource.getSysLangValue("lblCity")%>:<br />
                                        <asp:TextBox runat="server" ID="txt_locCity" Width="100px" />
                                    </td>
                                    <td>
                                        <%=CurrentSource.getSysLangValue("lblRegionState")%>:<br />
                                        <asp:TextBox runat="server" ID="txt_locState" Width="100px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%=CurrentSource.getSysLangValue("locProvince")%>:<br />
                                        <asp:TextBox runat="server" ID="txt_loc_province" Width="100px" MaxLength="10" />
                                    </td>
                                    <td colspan="2">
                                        <%=CurrentSource.getSysLangValue("reqLocation")%>:<br />
                                        <asp:DropDownList ID="drp_locCountry" runat="server" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="mainbox">
                    <div class="center">
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("pdf_Nato_a")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_birthPlace" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("pdf_Data_di_Nascita")%>:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="rdp_birthDate" runat="server" Width="100px" MinDate="1900-01-01">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("pdf_Documento_d_identita")%>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_docType" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("pdf_Num_Documento")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_docNum" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("pdf_RilasciatoDal")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_docIssuePlace" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("lblDocIssueDate")%>:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="rdp_docIssueDate" runat="server" Width="100px">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("pdf_Data_di_scadenza")%>:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="rdp_docExpiryDate" runat="server" Width="100px">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("lblIdDestinatario")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_id_codice" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("formVatNumber")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_docVat" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("lblCodiceFiscale")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_docCf" Width="200px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                </div>
                <div class="mainbox">

                    <div class="center">
                        <span class="titoloboxmodulo"><%=CurrentSource.getSysLangValue("lblContacts")%></span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("reqEmail")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contactEmail" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("lblPhone")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contactPhone" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("reqPhoneNumber")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contactPhoneMobile" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("lblCellulare_Viaggio")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contactPhoneTrip" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        <%=CurrentSource.getSysLangValue("pdf_Numero_di_fax")%>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contactFax" Width="200px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                </div>
            </telerik:RadCodeBlock>
            <div class="nulla">
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
