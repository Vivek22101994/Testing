<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_residence_details.aspx.cs" Inherits="RentalInRome.admin.rnt_residence_details" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register Src="~/admin/uc/UC_rnt_residence_navlinks.ascx" TagName="UC_rnt_residence_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
		var _editors = ['<%=txt_description.ClientID %>'];
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
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <h1 class="titolo_main">Scheda residenza</h1>
            <div id="fascia1">
                <div style="clear: both; margin: 3px 0 5px 30px;">
                    <uc1:UC_rnt_residence_navlinks ID="UC_rnt_residence_navlinks1" runat="server" />
                </div>
            </div>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva e torna nella lista</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>">
                        <span>Torna nel elenco</span></a>
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
                        <span class="titoloboxmodulo">Dati Identificativi</span>
                        <div class="boxmodulo">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr style="height: 0px;">
                                            <td style="width: 60px;">
                                            </td>
                                            <td style="width: 130px;">
                                            </td>
                                            <td style="width: 20px;">
                                            </td>
                                            <td style="width: 70px;">
                                            </td>
                                            <td style="width: 200px;">
                                            </td>
                                        </tr>
                                        <tr id="pnl_tempLang" runat="server">
                                            <td colspan="2">
                                                Importa le lingue dalla risorsa:
                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList runat="server" ID="drp_tempLang" />
                                                <asp:LinkButton ID="lnk_save_tempLang" runat="server" OnClick="lnk_save_tempLang_Click">Importa</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Nome Struttura:
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txt_code" runat="server" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Proprietario:
                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList runat="server" ID="drp_owner" Width="200px" DataSourceID="LDS_owners" DataTextField="name_full" DataValueField="id" />
                                                <asp:LinqDataSource ID="LDS_owners" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" OrderBy="name_full" TableName="USR_TBL_OWNER" Where="is_active == @is_active">
                                                    <WhereParameters>
                                                        <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                                    </WhereParameters>
                                                </asp:LinqDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Citta:
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="drp_city" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="drp_city_SelectedIndexChanged" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                Zona:
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="drp_zone" Width="180px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                CAP:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_zip_code" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                Citofono:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_loc_inner_bell" runat="server" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Indirizzo:
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txt_address" runat="server" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Tel 1:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_phone_1" runat="server" Width="120px"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                Tel 2:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_phone_2" Width="140px" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top">
                                                Note Interne:
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox runat="server" ID="txt_inner_notes" TextMode="MultiLine" Height="60px" Width="420px" />
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
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">MQ - Locali - Persone</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td style="width: 100px;">
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                    <td style="width: 20px;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 50px;">
                                    </td>
                                    <td style="width: 60px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        MQ interni:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_mq_inner" Width="70px" />
                                        <asp:RegularExpressionValidator ID="REV_txt_mq_inner" runat="server" ControlToValidate="txt_mq_inner" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        MQ Terrazzo:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_mq_terrace" Width="70px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txt_mq_terrace" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        MQ esterni:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_mq_outer" Width="70px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt_mq_outer" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        Vani:
                                    </td>
                                    <td style="width: 60px;">
                                        <asp:TextBox runat="server" ID="txt_num_rooms_bed" Width="30px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_num_rooms_bed" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
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
                        <span class="titoloboxmodulo">Opzioni</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td class="td_title">
                                        Attivo ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_is_active" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Google maps ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_is_google_maps" />
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
                <div class="nulla">
                </div>
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
                        <div class="boxmodulo">
                            <asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
                                <WhereParameters>
                                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                </WhereParameters>
                            </asp:LinqDataSource>
                            <asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_lang" CommandName="change_lang" OnClientClick="removeTinyEditor()" CssClass="tab_item" runat="server">
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
                            <table cellpadding="0" cellspacing="10">
                                <tr>
                                    <td style="border-right: medium ridge;">
                                        <table cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:LinkButton ID="lnk_copyLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_copyLang_Click">copia</asp:LinkButton>
                                                    &nbsp; &nbsp; &nbsp;
                                                    <asp:LinkButton Visible="false" ID="lnk_pasteLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_pasteLang_Click">incolla</asp:LinkButton>
                                                    <asp:HiddenField ID="HF_copyLang" Value="0" runat="server" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span class="titoloboxmodulo">Visualizzazione</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Nome in Lingua:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_title" MaxLength="100" Width="300px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Caratteristica:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_sub_title" MaxLength="200" Width="300px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    Sommario nel elenco:<br />
                                                    <asp:TextBox runat="server" ID="txt_summary" Width="400px" TextMode="MultiLine" Height="115px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <br />
                                                    Descrizione:<br />
                                                    <asp:TextBox runat="server" ID="txt_description" Width="400px" TextMode="MultiLine" Height="250px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="border-left: medium ridge;">
                                        <table cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td colspan="2">
                                                    <span class="titoloboxmodulo">Motori Ricerca</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    Meta Title:<br />
                                                    <span class="error_text" id='count_txt_meta_title' style="display: none">
                                                        <span class="char"></span>
                                                        &nbsp;caratteri e&nbsp;
                                                        <span class="word"></span>
                                                        &nbsp;parole </span>
                                                    <br />
                                                    <asp:TextBox runat="server" ID="txt_meta_title" Width="350px" MaxLength="100" onkeyup="CountWords(this,'count_txt_meta_title')" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    Meta KeyWords:<br />
                                                    <span class="error_text" id='count_txt_meta_keywords' style="display: none">
                                                        <span class="char"></span>
                                                        &nbsp;caratteri e&nbsp;
                                                        <span class="word"></span>
                                                        &nbsp;parole </span>
                                                    <br />
                                                    <asp:RegularExpressionValidator ID="txtConclusionValidator1" ControlToValidate="txt_meta_keywords" Display="Dynamic" Text="// 500 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,500}$" ValidationGroup="dati" runat="server" />
                                                    <asp:TextBox runat="server" ID="txt_meta_keywords" TextMode="MultiLine" MaxLength="500" Height="100px" Width="350px" onkeyup="CountWords(this,'count_txt_meta_keywords')" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    Meta Description:<br />
                                                    <span class="error_text" id='count_txt_meta_description' style="display: none">
                                                        <span class="char"></span>
                                                        &nbsp;caratteri e&nbsp;
                                                        <span class="word"></span>
                                                        &nbsp;parole </span>
                                                    <br />
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" ControlToValidate="txt_meta_description" Display="Dynamic" Text="// 500 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,500}$" ValidationGroup="dati" runat="server" />
                                                    <asp:TextBox runat="server" ID="txt_meta_description" TextMode="MultiLine" MaxLength="500" Height="150px" Width="350px" onkeyup="CountWords(this,'count_txt_meta_description')" />
                                                </td>
                                            </tr>
                                        </table>
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
            <asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
