<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_comment.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_comment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
		var _editors = ['<%=txt_body.ClientID %>'];
		function removeTinyEditor() {
		    return;
		    removeTinyEditors(_editors);
		}
		function setTinyEditor() {
		    return;
		    setTinyEditors(_editors, false);
        }
        var _dtCommentCal;
        function setCalendar(dtInt) {
            _dtCommentCal = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtComment.ClientID %>", View: "#txt_dtComment", DateInt: dtInt, changeMonth: true, changeYear: true });
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/jquery/js/ui-timepicker-custom.js"></script>
    <link href="/jquery/css/ui-timepicker-custom.css" rel="stylesheet" type="text/css" rel="stylesheet" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:LinqDataSource ID="LDS_page_blocks" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TBL_ESTATE_COMMENTs" OrderBy="dtComment desc">
            </asp:LinqDataSource>
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Gestione Commenti </h1>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS_page_blocks" OnSelectedIndexChanging="LV_SelectedIndexChanging">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span><%# Eval("vote") %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtComment")).formatITA(true) %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# getType(""+Eval("type"))  %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# getPers(""+Eval("pers"))  %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("cl_name_full") + "&nbsp;(" + Eval("cl_country") + ")"%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(), "-tutti-")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ("" + Eval("is_active") == "1") ? "SI" : "NO"%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="removeTinyEditor()" />
                                        <asp:ImageButton ID="ibtn_delete" AlternateText="sch." runat="server" Height="9px" CommandArgument='<%# Eval("id") %>' ImageUrl="~/images/ico/ico_del.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="return confirm('Sta per eliminare il Commento?');removeTinyEditor();" OnClick="DeleteRecord" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span><%# Eval("vote") %></span>
                                    </td>
                                    <td>
                                        <span><%# ((DateTime)Eval("dtComment")).formatITA(true) %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# getType(""+Eval("type"))  %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# getPers(""+Eval("pers"))  %></span>
                                    </td>
                                    <td>    
                                        <span><%# Eval("cl_name_full") + "&nbsp;(" + Eval("cl_country") + ")"%></span>
                                    </td>
                                    <td>
                                        <span><%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(),"-tutti-")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ("" + Eval("is_active") == "1") ? "SI" : "NO"%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="select" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="removeTinyEditor()" />
                                        <asp:ImageButton ID="ibtn_delete" AlternateText="sch." runat="server" Height="9px" CommandArgument='<%# Eval("id") %>' ImageUrl="~/images/ico/ico_del.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" OnClientClick="return confirm('Sta per eliminare il Commento?');removeTinyEditor();" OnClick="DeleteRecord" />
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
                                            <th id="Th6" runat="server" style="width: 30px">
                                                Voto
                                            </th>
                                            <th id="Th2" runat="server" style="width: 120px">
                                                Data
                                            </th>
                                            <th id="Th7" runat="server" style="width: 60px">
                                                Provenienza
                                            </th>
                                            <th id="Th8" runat="server" style="width: 60px">
                                                Ospiti
                                            </th>
                                            <th id="Th1" runat="server" style="width: 200px">
                                                Cliente
                                            </th>
                                            <th id="Th5" runat="server" style="width: 300px">
                                                Appartamento
                                            </th>
                                            <th id="Th4" runat="server" style="width: 100px">
                                                Approvato
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
                                        <span><%# Eval("vote") %></span>
                                    </td>
                                     <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtComment")).formatITA(true) %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# getType(""+Eval("type"))  %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# getPers(""+Eval("pers"))  %></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("cl_name_full") + "&nbsp;(" + Eval("cl_country") + ")"%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(), "-tutti-")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ("" + Eval("is_active") == "1") ? "SI" : "NO"%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
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
                <!-- INIZIO MAIN LINE -->
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
                                            Approvato:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_is_active" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Provenienza:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_type" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Appartamento:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_estate" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Voto:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_vote" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Ospiti:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_pers" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Nome Cliente:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_cl_name_full" Width="230px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Nazione Cliente:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_cl_country" Width="230px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Data Commento:
                                        </td>
                                        <td>
                                            <input id="txt_dtComment" type="text" readonly="readonly" />
                                            &nbsp;&nbsp;
                                            <asp:DropDownList ID="drp_dtComment_hour" runat="server">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="drp_dtComment_minute" runat="server">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="HF_dtComment" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                    <tr runat="server" visible="false">
                                        <td class="td_title">
                                            Titolo:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txt_subject" Width="230px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title" valign="top">
                                            Commento:
                                        </td>
                                        <td>
                                            <div runat="server" visible="false">
                                                <%=txt_body.Text%>
                                            </div>
                                            <asp:TextBox runat="server" ID="txt_body" Width="500px" TextMode="MultiLine" Height="250px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <strong>Attenzione!</strong><br />
                                            Controllare bene il contenuto del commento.<br/>
                                            Non approvare se contiene link esterni, nomi dei siti estranei, etc.<br />
                                            Potrebbe causare perdita del PageRank dei motori di ricerca
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
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_save" runat="server" ValidationGroup="dati" OnClick="lnk_save_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_cancel" runat="server" OnClick="lnk_cancel_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
