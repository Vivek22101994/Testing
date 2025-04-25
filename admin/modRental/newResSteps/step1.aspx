<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="step1.aspx.cs" Inherits="ModRental.admin.modRental.newResSteps.step1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h1 class="titolo_main">Nuova prenotazione</h1>
            <asp:Panel ID="pnlContent" runat="server" Width="50%">
                <div class="mainline">
                    <div class="mainbox">
                        <div class="top">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                        <div class="center">
                            <span class="titoloboxmodulo" style="margin-bottom: 0;">Struttura</span>
                            <div class="boxmodulo">
                                <%= CurrentSource.rntEstate_code(currTBL.pidEstate.objToInt32(), "- - -") %>
                            </div>
                            <span class="titoloboxmodulo" style="margin-bottom: 0;">Periodo e Ospiti</span>
                            <div class="boxmodulo">
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td class="td_title">
                                            Check In:
                                        </td>
                                        <td>
                                            <%= currTBL.dtStart.formatCustom("#dd# #MM# #yy#", 1, "- - -") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Check Out:
                                        </td>
                                        <td>
                                            <%= currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", 1, "- - -") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Adulti:
                                        </td>
                                        <td>
                                            <%= currTBL.numPers_adult %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Bambini over 3:
                                        </td>
                                        <td>
                                            <%= currTBL.numPers_childOver %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Neonati (gratis):
                                        </td>
                                        <td>
                                            <%= currTBL.numPers_childMin %>
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
                            <span class="titoloboxmodulo" style="margin-bottom: 0;">Dati anagrafici</span>
                            <div class="boxmodulo">
                                <script type="text/javascript">
                                    function checkEmail(sender, args) {
                                        args.IsValid = FORM_validateEmail($('#<%=txt_email.ClientID %>').val());
                                    }
                                </script>
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td class="td_title">
                                            E-mail:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_email" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_email" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ClientValidationFunction="checkEmail" ID="ValidateCheckBoxList1" runat="server" ErrorMessage="<br/>//e-mail non valido" ValidationGroup="dati" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Nome completo:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_honorific" runat="server" Style="margin-right: 1%; width: 15%;">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txt_name_full" runat="server" Style="margin: 5px 0 0; width: 78%;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_name_full" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Lingua:
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="drp_lang" OnDataBound="drp_lang_DataBound" DataSourceID="LDS_lang" DataTextField="title" DataValueField="id">
                                            </asp:DropDownList>
                                            <asp:LinqDataSource ID="LDS_lang" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="title" TableName="CONT_TBL_LANGs" Where="is_active == 1">
                                            </asp:LinqDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Cellulare:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_phone_mobile" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Paese:
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                                <WhereParameters>
                                                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                                </WhereParameters>
                                            </asp:LinqDataSource>
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
                <div class="nulla">
                </div>
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_saveManual" runat="server" ValidationGroup="dati" OnClick="lnk_saveManual_Click"><span>Prenotazione con i prezzi modificabili</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_save" runat="server" ValidationGroup="dati" OnClick="lnk_save_Click"><span>Prenotazione con i prezzi da tariffa</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <a href="#" onclick="CloseRadWindow('');return false;"><span>Annulla / Chiudi</span></a>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
