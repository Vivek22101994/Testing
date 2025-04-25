<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="pdf_template.aspx.cs" Inherits="RentalInRome.admin.pdf_template" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var _editors = ['<%=txt_body.ClientID %>'];
		function removeTinyEditor() {
			removeTinyEditors(_editors);
		}
		function setTinyEditor() {
			setTinyEditors(_editors, false);
		}
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaPdf_DataContext" TableName="PDF_TBL_TEMPLATEs" OrderBy="code">
            </asp:LinqDataSource>
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Elenco MailTemplate</h1>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnSelectedIndexChanging="LV_SelectedIndexChanging">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("code") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="removeTinyEditor();" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("code") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="removeTinyEditor();" />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table id="Table1" runat="server" style="">
                                    <tr>
                                        <td>
                                            No data was returned.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div class="table_fascia">
                                    <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                                        <tr id="Tr1" runat="server" style="text-align: left">
                                            <th id="Th2" runat="server" style="width: 40px">
                                                ID
                                            </th>
                                            <th id="Th1" runat="server" style="width: 200px">
                                                Nome
                                            </th>
                                            <th id="Th5" runat="server" style="width: 300px">
                                                Descrizione
                                            </th>
                                            <th id="Th3" runat="server">
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                                <div class="page">
                                    <asp:DataPager ID="DataPager1" runat="server" style="border-right: medium none;">
                                        <Fields>
                                            <asp:NumericPagerField />
                                        </Fields>
                                    </asp:DataPager>
                                </div>
                            </LayoutTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <SelectedItemTemplate>
                                <tr class="current">
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("code") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </SelectedItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Panel ID="pnlContent" runat="server" Width="100%" Visible="false">
                <h1 class="titolo_main">Scheda </h1>
                <div class="nulla">
                </div>
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
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
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td class="td_title">
                                            Nome:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_code" Width="230px" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr runat="server" visible="false">
                                        <td class="td_title">
                                            Descrizione:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_inner_notes" Width="230px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Descrizione:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_subject" Width="230px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Contenuto Pdf:
                                            <br />
                                            <asp:TextBox runat="server" ID="txt_body" Width="500px" TextMode="MultiLine" Height="250px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Variabili ammessi:
                                            <br />
                                            <asp:TextBox runat="server" ID="txt_replace_notes" Width="500px" TextMode="MultiLine" Height="250px" />
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
                            <span class="titoloboxmodulo">Prove visualizzazione</span>
                            <div class="boxmodulo">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr class="alternate">
                                        <td class="td_title">
                                            Margine LEFT
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_margin_left" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="alternate">
                                        <td class="td_title">
                                            Margine RIGHT
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_margin_right" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="alternate">
                                        <td class="td_title">
                                            Margine TOP
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_margin_top" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="alternate">
                                        <td class="td_title">
                                            Margine BOTTOM
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_margin_bottom" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:LinkButton ID="lnk_create" runat="server" OnClick="lnk_create_Click">crea</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:HyperLink ID="HL_open_pdf" runat="server" Target="_blank" NavigateUrl="/pdf/prova.pdf">apri</asp:HyperLink>
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
