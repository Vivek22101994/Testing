<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_request_new_from_mail.aspx.cs" Inherits="RentalInRome.admin.rnt_request_new_from_mail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var _editors = ['<%=txt_description.ClientID %>'];
        function removeTinyEditor() {
            removeTinyEditors(_editors);
            return false;
        }
        function setTinyEditor(IsReadOnly) {
            setTinyEditors(_editors, IsReadOnly);
        }
    </script>
    <script type="text/javascript">
        var items = [];


        function setAutocomplete() {
            $(".aptComplete").autocomplete({
                source: items
            });
        }
    </script>
    <link href="../jquery/css/ui-lightness/jquery-ui-1.8.7.custom.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .ui-autocomplete
        {
            max-height: 200px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            padding-right: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_unique" Value="" runat="server" Visible="false" />
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_pid_request" Value="0" runat="server" Visible="false" />
            <asp:Literal ID="ltr_body_html_text" runat="server" Visible="false"></asp:Literal>
            <asp:Literal ID="ltr_body_plain_text" runat="server" Visible="false"></asp:Literal>
            <h1 class="titolo_main">
                Nuova Richiesta mail</h1>
            <div class="mainline">
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Dati Cliente</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td class="td_title">
                                        Email:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_email" runat="server" Style="width: 300px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Nome / Cognome:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_name_full" runat="server" Style="width: 300px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Telefono:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_phone" runat="server" Style="width: 300px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Location:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_country" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="id">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == 1">
                                        </asp:LinqDataSource>
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
                            </table>
                        </div>
                        <span class="titoloboxmodulo">Dati Richiesta</span>
                        <div class="boxmodulo">
                            <table>
                                <tr>
                                    <td class="td_title">City:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_pidCity">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Assegna ad un account:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_admin">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Data check-in:
                                    </td>
                                    <td>
                                        <input type="text" id="cal_date_start" style="width: 120px" />
                                        <img src="../images/ico/ico_del.gif" id="del_date_start" style="cursor: pointer; display: none;" alt="X" title="Pulisci" />
                                        <asp:HiddenField ID="HF_date_start" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Data check-out:
                                    </td>
                                    <td>
                                        <input type="text" id="cal_date_end" style="width: 120px" />
                                        <img src="../images/ico/ico_del.gif" id="del_date_end" style="cursor: pointer; display: none;" alt="X" title="Pulisci" />
                                        <asp:HiddenField ID="HF_date_end" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Date Flessibili?
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_date_is_flexible" runat="server">
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        num. Adulti:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_adult_num" runat="server">
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        num. Bambini:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_child_num" runat="server">
                                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
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
                        <span class="titoloboxmodulo">Note (Richiesta Speciale)</span>
                        <div class="boxmodulo">
                            <asp:TextBox runat="server" ID="txt_description" Width="400px" TextMode="MultiLine" Height="250px" />
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
                        <span class="titoloboxmodulo">Dati Cliente</span>
                        <div class="boxmodulo">
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
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_create" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_create_Click"><span>Salva Richiesta</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>">
                        <span>Torna nel elenco delle Mail</span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
