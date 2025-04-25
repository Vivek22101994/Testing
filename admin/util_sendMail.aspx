<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="util_sendMail.aspx.cs" Inherits="RentalInRome.admin.util_sendMail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server" CssClass="mainline">
        <div class="mainbox">
            <div class="top">
                <div class="sx">
                </div>
                <div class="dx">
                </div>
            </div>
            <div class="center">
                <span class="titoloboxmodulo">Nuova mail</span>
                <div class="boxmodulo">
                    <table>
                        <tr>
                            <td>
                                E-mail Mittente:<br />
                                <asp:TextBox ID="txt_fromMail" runat="server" Width="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nome Mittente:<br />
                                <asp:TextBox ID="txt_fromName" runat="server" Width="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Destinatari:<br />
                                <div class="filt">
                                    <div class="t">
                                        <div class="sx">
                                        </div>
                                        <div class="dx">
                                        </div>
                                    </div>
                                    <div class="c">
                                        <div class="filtro_cont">
                                            <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                                <tr>
                                                    <td>
                                                        <telerik:RadAjaxPanel ID="rapFilter" runat="server">
                                                            <label>Lingua:
                                                                <br />
                                                                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="select_pidLang" Style="color: #E01E15; text-decoration: none;">seleziona tutti</asp:LinkButton>
                                                                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton3" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="deselect_pidLang" Style="color: #E01E15; text-decoration: none;">deseleziona tutti</asp:LinkButton>
                                                            </label>
                                                            <div class="nulla">
                                                            </div>
                                                            <div style="max-height: 150px; min-width: 200px; overflow-y: auto;">
                                                                <asp:CheckBoxList ID="chkList_flt_pidLang" runat="server" CssClass="inp" TextAlign="Right" RepeatColumns="4" Style="margin: 0 5px 5px 0;">
                                                                </asp:CheckBoxList>
                                                                <div class="nulla">
                                                                </div>
                                                            </div>
                                                        </telerik:RadAjaxPanel>
                                                    </td>
                                                    <td valign="bottom">
                                                        <asp:LinkButton ID="lnk_filter" runat="server" CssClass="ricercaris" OnClick="lnk_filter_Click"><span>Filtra Destinatari</span></asp:LinkButton>
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
                                <br />
                                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="select_agency" Style="color: #E01E15; text-decoration: none;">seleziona tutti</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton4" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="deselect_agency" Style="color: #E01E15; text-decoration: none;">deseleziona tutti</asp:LinkButton>
                                <br />
                                <asp:CheckBoxList ID="chkList_agency" runat="server" RepeatColumns="5">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Oggetto:<br />
                                <asp:TextBox ID="txt_subject" runat="server" Width="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Body:<br />
                                <telerik:RadEditor runat="server" ID="re_body" SkinID="DefaultSetOfTools" Height="600" Width="800" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                    <CssFiles>
                                        <telerik:EditorCssFile Value="" />
                                    </CssFiles>
                                </telerik:RadEditor>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="salvataggio">
                                    <div class="bottom_salva">
                                        <asp:LinkButton ID="lnk_send" runat="server" OnClick="lnk_send_Click"><span>Send</span></asp:LinkButton>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                </div>
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
    </telerik:RadAjaxPanel>
    <asp:Literal ID="ltr_template_1" runat="server" Visible="false">
    <a href="http://www.rentalinrome.com/"><img src="http://www.rentalinrome.com/images/css/logo.gif" alt="" /></a>
    <br />
    <br />
    <br />
    
    <br />
    Best Regards,<br />
    The Rental in Rome Staff
    <br />
    <br />
    <strong class="titCon">Contact Us by Phone:</strong><strong></strong><br />
    +39 06 9905533 <br />
    +39 06 9905513<br />
    +39 06 9905199<br />
    +39 06 99329392
    <br /><br />
    <strong>Fax: </strong>
    +39 06 23328717
    <br /><br />
    <strong>E-mail:</strong>
    <a class="mailHome" href="mailto:agencies@rentalinrome.com">agencies@rentalinrome.com</a>
    </asp:Literal>
</asp:Content>
