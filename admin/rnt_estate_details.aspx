<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_details.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_details" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var _editors = [];
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
            <asp:HiddenField ID="HF_updater_newId" Value="0" runat="server" />
            <asp:HiddenField ID="HF_updater_args" Value="0" runat="server" />
            <h1 class="titolo_main">Scheda Struttura</h1>
            <div id="fascia1">
                <div style="clear: both; margin: 3px 0 5px 30px;">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>
            <div class="salvataggio hiddenbeforload" style="display: none;">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva e torna nella lista</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>"><span>Torna nella lista senza salvare</span> </a>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
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
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Dati Identificativi</span>
                        <div class="boxmodulo">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:LinkButton ID="btn_page_update_1" runat="server" Style="display: none;" OnClick="btn_page_update_Click"></asp:LinkButton>
                                    <script type="text/javascript">
                                        var contentUpdater_1 = "<%= btn_page_update_1.UniqueID %>";
                                        function ReloadContent_1(arg, id) {
                                            $("#<%= HF_updater_newId.ClientID %>").val("" + id);
                                            $("#<%= HF_updater_args.ClientID %>").val("" + arg);
                                            setTimeout("__doPostBack('" + contentUpdater_1 + "', '')", 0);
                                            setTimeout("Shadowbox.close()", 500);
                                            //alert(id+"/" + arg);
                                        }
                                    </script>
                                    <table>
                                        <tr style="height: 0px;">
                                            <td style="width: 60px;"></td>
                                            <td style="width: 130px;"></td>
                                            <td style="width: 20px;"></td>
                                            <td style="width: 70px;"></td>
                                            <td style="width: 200px;"></td>
                                        </tr>
                                        <tr>
                                            <td>Nome Struttura:
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txt_code" runat="server" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Proprietario:
                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList runat="server" ID="drp_owner" Width="300px" DataSourceID="LDS_owners" DataTextField="name_full" DataValueField="id" OnDataBound="drp_owner_DataBound" />
                                                <asp:LinqDataSource ID="LDS_owners" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" OrderBy="name_full" TableName="USR_TBL_OWNER" Where="is_active == @is_active">
                                                    <WhereParameters>
                                                        <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                                    </WhereParameters>
                                                </asp:LinqDataSource>
                                                <img onclick="OpenShadowbox('usr_owner_createform.aspx', 480, 305)" alt="agg." title="Aggiungi nuovo" src="../images/ico/agg.gif" style="cursor: pointer; height: 15px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Giorni consentiti all'anno:
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox runat="server" ID="txt_ext_ownerdaysinyear" Width="70px" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_ext_ownerdaysinyear" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator22" runat="server" ControlToValidate="txt_ext_ownerdaysinyear" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Residenza:
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList runat="server" ID="drp_residence" Width="200px" />
                                            </td>
                                            <td>Tipo:&nbsp;&nbsp;
                                                <asp:DropDownList runat="server" ID="drp_category">
                                                    <asp:ListItem Text="Appartamento" Value="apt"></asp:ListItem>
                                                    <asp:ListItem Text="Villa" Value="villa"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Citta:
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="drp_city" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="drp_city_SelectedIndexChanged" />
                                            </td>
                                            <td></td>
                                            <td>Zona:
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="drp_zone" Width="180px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>CAP:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_zip_code" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>Citofono:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_loc_inner_bell" runat="server" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Indirizzo:
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txt_address" runat="server" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tel 1:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_phone_1" runat="server" Width="120px"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td>Tel 2:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_phone_2" Width="140px" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top">Note Interne:
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox runat="server" ID="txt_inner_notes" TextMode="MultiLine" Height="400px" Width="420px" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                </div>
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">MQ - Locali - Persone</span>
                        <div class="boxmodulo">
                            <div id="ttp_bedDouble" style="display: none;">
                                <img src="/images/css/letto-matrimonale-standard.gif" alt="" />
                            </div>
                            <div id="ttp_bedDoubleD" style="display: none;">
                                <img src="/images/css/letto-matrimonale-divisibile.gif" alt="" />
                            </div>
                            <div id="ttp_bedSingle" style="display: none;">
                                <img src="/images/css/letto-singolo.gif" alt="" />
                            </div>
                            <div id="ttp_bedSofaDouble" style="display: none;">
                                <img src="/images/css/divano-letto.gif" alt="" />
                            </div>
                            <div id="ttp_bedSofaSingle" style="display: none;">
                                <img src="/images/css/poltrona-letto.gif" alt="" />
                            </div>
                            <div id="ttp_bedDouble2level" style="display: none;">
                                <img src="/images/css/letto-castello.gif" alt="" />
                            </div>
                            <style type="text/css">
                                .ico_info {
                                    color: #FE6634;
                                    cursor: pointer;
                                    display: inline-block;
                                    font-family: monospace;
                                    font-size: 14px;
                                    font-weight: bold;
                                    line-height: 14px;
                                    margin: 3px;
                                }
                            </style>
                            <table>
                                <tr>
                                    <td style="width: 100px;"></td>
                                    <td style="width: 100px;"></td>
                                    <td style="width: 20px;">&nbsp;
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td style="width: 60px;"></td>
                                </tr>
                                <tr>
                                    <td>Deposito cauzionale:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_pr_deposit" Width="70px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_pr_deposit" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txt_pr_deposit" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td></td>
                                    <td>Commissione:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_pr_percentage" Width="70px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_pr_percentage" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txt_pr_percentage" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>CreditCard per dep.:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_pr_depositWithCard" runat="server">
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Letti Matrimoniali:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_bed_double" Width="30px" /><a class="ico_info ico_tooltip_right" ttpc="ttp_bedDouble">?</a>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server" ControlToValidate="txt_num_bed_double" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>Letti Single:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_bed_single" Width="30px" /><a class="ico_info ico_tooltip_right" ttpc="ttp_bedSingle">?</a>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="txt_num_bed_single" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Matrimoniali Divisibili:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_bed_double_divisible" Width="30px" /><a class="ico_info ico_tooltip_right" ttpc="ttp_bedDoubleD">?</a>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator27" runat="server" ControlToValidate="txt_num_bed_double_divisible" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>Letto a Castello:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_bed_double_2level" Width="30px" /><a class="ico_info ico_tooltip_right" ttpc="ttp_bedDouble2level">?</a>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator28" runat="server" ControlToValidate="txt_num_bed_double_2level" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Poltrona Letti:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_sofa_single" Width="30px" /><a class="ico_info ico_tooltip_right" ttpc="ttp_bedSofaSingle">?</a>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server" ControlToValidate="txt_num_sofa_single" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>Divano Letti:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_sofa_double" Width="30px" /><a class="ico_info ico_tooltip_right" ttpc="ttp_bedSofaDouble">?</a>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator20" runat="server" ControlToValidate="txt_num_sofa_double" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>MQ interni:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_mq_inner" Width="70px" />
                                        <asp:RegularExpressionValidator ID="REV_txt_mq_inner" runat="server" ControlToValidate="txt_mq_inner" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td></td>
                                    <td>MQ Terrazzo:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_mq_terrace" Width="70px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txt_mq_terrace" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>MQ esterni:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_mq_outer" Width="70px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt_mq_outer" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td></td>
                                    <td>Camere da letto:
                                    </td>
                                    <td style="width: 60px;">
                                        <asp:TextBox runat="server" ID="txt_num_rooms_bed" Width="30px" ReadOnly="true" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_num_rooms_bed" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Salotto :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_room_living" Width="70px" ReadOnly="true" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator29" runat="server" ControlToValidate="txt_num_room_living" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td></td>
                                    <td>Numero di Cucina :
                                    </td>
                                    <td style="width: 60px;">
                                        <asp:TextBox runat="server" ID="txt_num_kitchen" Width="30px" ReadOnly="true" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator30" runat="server" ControlToValidate="txt_num_kitchen" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td># Posti Letto*:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_beds" Width="70px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_num_beds" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_num_beds" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td></td>
                                    <td>Bagni:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_bath_rooms" Width="30px" ReadOnly="true" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_bath_rooms" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td># Letti Agg.:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_beds_optional" Width="70px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="txt_num_beds_optional" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td></td>
                                    <td>Terrazze:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_terraces" Width="30px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt_num_terraces" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Children allowed?
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_children_allowed" runat="server" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td># Bambini(-3)
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_persons_child" Width="70px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_num_persons_child" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txt_num_persons_child" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td></td>
                                    <td>Piano:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_floor" Width="30px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_floor" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td># min. Persone
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_num_persons_min" Width="70px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_num_persons_min" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator21" runat="server" ControlToValidate="txt_num_persons_min" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Min. Notti:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_nights_min" Width="30px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txt_nights_min" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>Max. Notti:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_nights_max" Width="30px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ControlToValidate="txt_nights_max" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Min. Notti Altissima:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_nights_minVHSeason" Width="30px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator23" runat="server" ControlToValidate="txt_nights_minVHSeason" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>pr. Ricavo Pulizie:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_eco_pr_1" Width="30px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ControlToValidate="txt_eco_pr_1" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>pr. Agg Pulizie:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_eco_pr_2" Width="30px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ControlToValidate="txt_eco_pr_2" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Note Pulizie:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_eco_notes" Width="100px" />
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td><%= contUtils.getLabel("lbl_SqFeet")%>:
                                    </td>
                                    <td>
                                         <asp:TextBox runat="server" ID="txt_sqFeet" Width="88px" />
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator31" runat="server" ControlToValidate="txt_mq_inner" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                </div>
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Opzioni</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td class="td_title">Valutazione
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_importance_vote" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Attivo ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_is_active" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Online Booking ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_is_online_booking" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Google maps ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_is_google_maps" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">In Esclusiva ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_is_exclusive" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Tassa di soggiorno ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_pr_has_overnight_tax" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Cedolare Secca ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_isCedolareSecca" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Contratto ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_isContratto" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Long Terms
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_longTermRent" runat="server" onchange="$('#pnl_longTermPrMonthly').css('display',(this.value=='1'?'':'none'))">
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <script type="text/javascript">
                                            $(document).ready(function () {
                                                $('#pnl_longTermPrMonthly').css('display', ($('#<%=drp_longTermRent.ClientID %>').val() == '1' ? '' : 'none'))
                                            });
                                        </script>
                                    </td>
                                </tr>
                                <tr id="pnl_longTermPrMonthly">
                                    <td class="td_title">pr. mensile
                                    </td>
                                    <td style="width: 70px;">&euro;&nbsp;
                                        <telerik:RadNumericTextBox ID="txt_longTermPrMonthly" runat="server" Width="50" MinValue="0">
                                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Limitazione delle notti<br />
                            per prenotazioni future</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td class="td_title">Abilitato?
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_lpb_is" runat="server" onchange="$('#lpb_container').css('display',(this.value=='1'?'':'none'))">
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <script type="text/javascript">
                                            $(document).ready(function () {
                                                $('#lpb_container').css('display', ($('#<%=drp_lpb_is.ClientID %>').val() == '1' ? '' : 'none'))
                                            });
                                        </script>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="boxmodulo" id="lpb_container">
                            <table>
                                <tr>
                                    <td class="td_title">Dopo
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_lpb_afterdays" runat="server">
                                        </asp:DropDownList>
                                        &nbsp;gg
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">per Minimo di notti
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_lpb_nights_min" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Solo AltaStagione?
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_lpb_onlyhighseason" runat="server">
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Dati Pulizie</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td class="td_title">Gestore
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_is_ecopulizie" runat="server" onchange="$('#eco_container').css('display',(this.value!='1'?'':'none'))">
                                            <asp:ListItem Text="Altro (Esterno)" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Ecopulizie" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <script type="text/javascript">
                                            $(document).ready(function () {
                                                $('#eco_container').css('display', ($('#<%=drp_is_ecopulizie.ClientID %>').val() != '1' ? '' : 'none'))
                                            });
                                        </script>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="boxmodulo" id="eco_container">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <strong>Dati Pulizia Esterna</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Nome Gestore
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_eco_ext_name_full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">E-mail
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_eco_ext_email" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Telefono
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_eco_ext_phone" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Prezzo
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_eco_ext_price" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator24" runat="server" ControlToValidate="txt_eco_ext_price" ErrorMessage=" // solo numeri" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Incluso nel Prezzo?
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_eco_ext_clientPay" runat="server">
                                            <asp:ListItem Text="SI" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Pagamento ogni
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_eco_ext_payInDays" runat="server" Width="40"></asp:TextBox>&nbsp;giorni
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator26" runat="server" ControlToValidate="txt_eco_ext_payInDays" ErrorMessage=" // solo numeri" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Dati Accoglienza</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td class="td_title">Gestore
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_is_srs" runat="server" onchange="$('#srs_container').css('display',(this.value!='1'?'':'none'))">
                                            <asp:ListItem Text="Altro (Esterno)" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Srs" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <script type="text/javascript">
                                            $(document).ready(function () {
                                                $('#srs_container').css('display', ($('#<%=drp_is_srs.ClientID %>').val() != '1' ? '' : 'none'))
                                            });
                                        </script>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="boxmodulo" id="srs_container">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <strong>Dati Accoglienza Esterna</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Nome Gestore
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_srs_ext_name_full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">E-mail
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_srs_ext_email" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Telefono
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_srs_ext_phone" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Telefono 2
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_srs_ext_phone_2" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Telefono 3
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_srs_ext_phone_3" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Prezzo
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_srs_ext_price" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator25" runat="server" ControlToValidate="txt_srs_ext_price" ErrorMessage=" // solo numeri" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Incluso nel Prezzo?
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_srs_ext_clientPay" runat="server">
                                            <asp:ListItem Text="SI" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Punto d'Incontro con cliente
                                        <br />
                                        <asp:TextBox ID="txt_srs_ext_meetingPoint" runat="server" TextMode="MultiLine" Style="width: 225px; height: 40px;"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                </div>
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Channel Manager HA</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td>Advertiser ID:
                                    </td>
                                    <td>
                                        <%--<asp:TextBox runat="server" ID="txt_advertiserID"/>--%>
                                        <asp:DropDownList ID="drpHomeAwayAdvertiser" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Listing ID:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_listingId" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Property Type:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_Property" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">Attivo ?
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_homeAway" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            
            <div class="nulla">
            </div>
            <asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
                <br />
                tempo previ. Pulizie:<asp:TextBox runat="server" ID="txt_eco_time_preview" Width="30px" />
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
