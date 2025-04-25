<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="EstateCommentsDett.aspx.cs" Inherits="ModRental.admin.modRental.EstateCommentsDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <title><%=ltrTitle.Text%></title>
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
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton>
            </div>
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
                                <td class="td_title">Approvato:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isActive" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Mostra sulla Homepage ?:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isVisibleHome" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td class="td_title">Anonimo:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isAnonymous" runat="server">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Provenienza:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_type" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Appartamento:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_estate" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Voto
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td></td>
                                            <td>carente
                                            </td>
                                            <td>sufficiente
                                            </td>
                                            <td>buono
                                            </td>
                                            <td>ottimo
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Staff
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteStaff_4" runat="server" GroupName="voteStaff" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteStaff_6" runat="server" GroupName="voteStaff" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteStaff_8" runat="server" GroupName="voteStaff" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteStaff_10" runat="server" GroupName="voteStaff" /></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Service
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteService_4" runat="server" GroupName="voteService" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteService_6" runat="server" GroupName="voteService" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteService_8" runat="server" GroupName="voteService" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteService_10" runat="server" GroupName="voteService" /></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Cleaning
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteCleaning_4" runat="server" GroupName="voteCleaning" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteCleaning_6" runat="server" GroupName="voteCleaning" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteCleaning_8" runat="server" GroupName="voteCleaning" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteCleaning_10" runat="server" GroupName="voteCleaning" /></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Comfort
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteComfort_4" runat="server" GroupName="voteComfort" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteComfort_6" runat="server" GroupName="voteComfort" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteComfort_8" runat="server" GroupName="voteComfort" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteComfort_10" runat="server" GroupName="voteComfort" /></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>QualityPrice
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteQualityPrice_4" runat="server" GroupName="voteQualityPrice" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteQualityPrice_6" runat="server" GroupName="voteQualityPrice" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteQualityPrice_8" runat="server" GroupName="voteQualityPrice" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_voteQualityPrice_10" runat="server" GroupName="voteQualityPrice" /></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Position
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_votePosition_4" runat="server" GroupName="votePosition" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_votePosition_6" runat="server" GroupName="votePosition" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_votePosition_8" runat="server" GroupName="votePosition" /></label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:RadioButton ID="rbtn_votePosition_10" runat="server" GroupName="votePosition" /></label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Ospiti:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_pers" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Nome Cliente:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_cl_name_full" Width="230px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Nazione Cliente:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_cl_country" Width="230px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Data Commento:
                                </td>
                                <td>
                                    <telerik:RadDateTimePicker ID="rdtp_dtComment" runat="server">
                                        <DateInput DateFormat="dd/MM/yyyy HH:mm">
                                        </DateInput>
                                    </telerik:RadDateTimePicker>
                                </td>
                            </tr>
                            <tr id="Tr1" runat="server" visible="false">
                                <td class="td_title">Titolo:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_subject" Width="230px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title" valign="top">Commento positivo:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_body" Width="500px" TextMode="MultiLine" Height="250px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title" valign="top">Commento negativo:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_bodyNegative" Width="500px" TextMode="MultiLine" Height="250px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <strong>Attenzione!</strong><br />
                                    Controllare bene il contenuto del commento.<br />
                                    Non approvare se contiene link esterni, nomi dei siti estranei, etc.<br />
                                    Potrebbe causare perdita del PageRank dei motori di ricerca
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
