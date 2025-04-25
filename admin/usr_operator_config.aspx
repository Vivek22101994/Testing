<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_operator_config.aspx.cs" Inherits="RentalInRome.admin.usr_operator_config" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var _editors = ['<%=txt_mailing_header.ClientID %>', '<%=txt_mailing_signature.ClientID %>'];
        function removeTinyEditor() {
            removeTinyEditors(_editors);
        }
        function setTinyEditor(IsReadOnly) {
            setTinyEditors(_editors, IsReadOnly);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <h1 class="titolo_main">
                Configurazione Mailing</h1>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_modify" runat="server" OnClick="lnk_modify_Click" OnClientClick="removeTinyEditor();"><span>Modifica</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_template" runat="server" OnClick="lnk_template_Click" OnClientClick="removeTinyEditor();"><span>Incolla Template</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage+"?id="+HF_id.Value %>"><span>Torna nella scheda</span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <table cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="2">
                                        Intestazione:<br />
                                        <asp:TextBox runat="server" ID="txt_mailing_header" Width="400px" TextMode="MultiLine" Height="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Firma:<br />
                                        <asp:TextBox runat="server" ID="txt_mailing_signature" Width="400px" TextMode="MultiLine" Height="250px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <h1 class="titolo_main">
                Configurazione SMTP</h1>
            <div class="mainline">
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                            <table cellpadding="3" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="salvataggio">
                                            <div class="bottom_salva">
                                                <asp:LinkButton ID="lnk_modifySMTP" runat="server" OnClick="lnk_modifySMTP_Click"><span>Modifica</span></asp:LinkButton>
                                            </div>
                                            <div class="bottom_salva">
                                                <asp:LinkButton ID="lnk_salvaSMTP" runat="server" OnClick="lnk_salvaSMTP_Click"><span>Salva Modifiche</span></asp:LinkButton>
                                            </div>
                                            <div class="bottom_salva">
                                                <asp:LinkButton ID="lnk_annullaSMTP" runat="server" OnClick="lnk_annullaSMTP_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                                            </div>
                                            <div class="nulla">
                                            </div>
                                        </div>
                                        <div class="nulla">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        E-mail:
                                        <br/>
                                        <asp:TextBox runat="server" ID="txt_email" Width="300px" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="3" cellspacing="0" id="pnl_smtpView" runat="server">
                                <tr>
                                    <td class="td_title">
                                        Pwd:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPwd" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="3" cellspacing="0" id="pnl_smtpEdit" runat="server">
                                <tr>
                                    <td class="td_title">
                                        Password:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_pwd" Width="200px" TextMode="Password" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Conferma:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_pwdConf" Width="200px" TextMode="Password" />
                                    </td>
                                </tr>
                            </table>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Literal ID="ltr_header_template" runat="server">
    </asp:Literal>
    <asp:Literal ID="ltr_footer_template" runat="server">
    <br />
    <span style="font-family: tahoma,sans-serif;">
        <a style="text-decoration: none;" href="http://www.rentalinrome.com/" target="_blank">
            <img src="http://www.rentalinrome.com/images/css/logo.gif" border="0" alt="" width="120" /></a><br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="text-decoration: none;" href="http://www.twitter.com/rentalinrome" target="_blank"><img src="http://twitter-badges.s3.amazonaws.com/t_logo-a.png" border="0" alt="Follow Rental in Rome on Twitter" width="30" height="30" /></a>&nbsp;<a style="text-decoration: none;" href="http://www.facebook.com/rentalinrome" target="_blank"><img src="http://buttonshut.com/links/facebook.png" border="0" alt="Like Us on Facebook" width="30" height="30" /></a>
    </span>
    <br />
    Maurizio Lecce (Administrator)<br />
    <a href="http://www.rentalinrome.com" target="_blank">www.rentalinrome.com</a>
    <br />
    <a href="mailto:maurizio.lecce@rentalinrome.com" target="_blank">maurizio.lecce@rentalinrome.com</a><br />
    Mobile: +39 3296140257<br />
    Tel: +39 06 9905199/+39 06 9905513<br />
    Fax: +39 06 23328717<br />
    Rental In Rome S.r.l. Via Appia Nuova, 677 -00179-Roma<br />
    P.iva: 07824541002<br />
    </asp:Literal>
</asp:Content>
