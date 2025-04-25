<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_special_offer_details.aspx.cs" Inherits="RentalInRome.admin.rnt_special_offer_details" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_unique" Value="" runat="server" />
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:Literal ID="ltr_pageTitle" runat="server" Visible="false"></asp:Literal>
            <h1 class="titolo_main"><%=ltr_pageTitle.Text%></h1>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>">
                        <span>Torna nel elenco</span>
                    </a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <!-- BOX 1 -->
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Dati Offerta</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        Data Creazione:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_dtCreation" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Attivo?:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_is_active" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="td_title">
                                                            Zona:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drp_zone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_zone_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title">
                                                            Struttura:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drp_estate" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Classe:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_class_type" runat="server">
                                            <asp:ListItem Value="" Text="- - -"></asp:ListItem>
                                            <asp:ListItem Value="bestPrice" Text="bestPrice"></asp:ListItem>
                                            <asp:ListItem Value="" Text="- - -"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Sconto:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_pr_discount" Width="50px" />&nbsp;%
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_pr_discount" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txt_pr_discount" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Inizio Offerta:
                                    </td>
                                    <td>
                                        <input id="txt_dtStart" type="text" readonly="readonly" style="width: 90px" />
                                        <a class="ico_cal" id="trigger_dtStart"></a>
                                        <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Fine Offerta:
                                    </td>
                                    <td>
                                        <input id="txt_dtEnd" type="text" readonly="readonly" style="width: 90px" />
                                        <a class="ico_cal" id="trigger_dtEnd"></a>
                                        <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Inizio Pubblicazione:
                                    </td>
                                    <td>
                                        <input id="txt_dtPublicStart" type="text" readonly="readonly" style="width: 90px" />
                                        <a class="ico_cal" id="trigger_dtPublicStart"></a>
                                        <asp:HiddenField ID="HF_dtPublicStart" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Fine Pubblicazione:
                                    </td>
                                    <td>
                                        <input id="txt_dtPublicEnd" type="text" readonly="readonly" style="width: 90px" />
                                        <a class="ico_cal" id="trigger_dtPublicEnd"></a>
                                        <asp:HiddenField ID="HF_dtPublicEnd" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Note Interne: <br/>
                                        <asp:TextBox runat="server" ID="txt_inner_notes" Width="350px" Height="150px" TextMode="MultiLine" />
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
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Visualizzazione</span>
                        <div class="boxmodulo">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td colspan="2">
                                                <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
                                                <asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
                                                    <WhereParameters>
                                                        <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                                    </WhereParameters>
                                                </asp:LinqDataSource>
                                                <asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_lang" CommandName="change_lang" CssClass="tab_item" runat="server">
                                                            <span>
                                                                <%# Eval("title") %></span>
                                                            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <span>No data was returned.</span>
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <div class="menu2">
                                                            <a id="itemPlaceholder" runat="server" />
                                                        </div>
                                                    </LayoutTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Titolo:
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_title" Width="300px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                Sommario:<br />
                                                <asp:TextBox runat="server" ID="txt_summary" Width="400px" TextMode="MultiLine" Height="115px" />
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
                <div class="mainbox" runat="server" visible="false">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Immagine Grande</span>
                        <div class="boxmodulo">
                            <uc1:UC_get_image ID="UC_get_img_banner" runat="server" ShowCrop="true" ImgMaxWidth="405" ImgCropAspectRatio="true" ImgCropHeight="387" ImgCropWidth="405" ImgCropMaxHeight="387" ImgCropMaxWidth="405" />
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
           
            <asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
    var _JSCal_Range;
    var _JSCal_RangePublic;
    function setCal_Range() {
        _JSCal_Range = new JSCal.Range({ dtFormat: "d MM yy", startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart", startTrigger: "#trigger_dtStart", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd", endTrigger: "#trigger_dtEnd", changeMonth: true, changeYear: true });
        _JSCal_RangePublic = new JSCal.Range({ dtFormat: "d MM yy", startCont: "#<%= HF_dtPublicStart.ClientID %>", startView: "#txt_dtPublicStart", startTrigger: "#trigger_dtPublicStart", endCont: "#<%= HF_dtPublicEnd.ClientID %>", endView: "#txt_dtPublicEnd", endTrigger: "#trigger_dtPublicEnd", changeMonth: true, changeYear: true });
    }

    </script>
</asp:Content>
