<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateSequence.aspx.cs" Inherits="ModRental.admin.modRental.EstateSequence" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .sortable .ui-sortable-helper {
            background-color: #9A1212;
            color:#FFFFFF;
        }
        .sortable .ui-state-highlight {
           height:25px;
        }
        .sortable li {
            background-color:#F0F0F0;
            border-bottom:1px solid #CDCDCD;
            font-size:11px;
            cursor:move;
            height:25px;
        }
        .sortable li table {
             margin:5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManagerProxy1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnCont" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <div id="fascia1" style="min-height: 350px;">
        <div class="pannello_fascia1" id="pnCont" runat="server">
            <div style="clear: both">
                <h1>Cambia ordine delle strutture</h1>
                <div class="bottom_agg">
                    <asp:LinkButton ID="lnk_salva" runat="server" OnClientClick="return saveSelection();" OnClick="lnk_salva_Click" Style="float: left; display: inline-block; margin-left: 10px;"><span>Salva ordine</span></asp:LinkButton>
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" Style="float: left; display: inline-block; margin-left: 10px;"><span>Ripristina ordine</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div style="clear: both;">
                <asp:HiddenField ID="HF_order" runat="server" />
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TB_ESTATE" Where="is_active == 1 && is_deleted != 1 && pid_city == @pid_city" OrderBy="sequence">
                    <WhereParameters>
                        <asp:QueryStringParameter QueryStringField="pid_city" DefaultValue="1" Name="pid_city" Type="Int32" />
                    </WhereParameters>
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
                    <ItemTemplate>
                        <li class="" refid="<%# Eval("id") %>">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td width="50px">
                                        <%# Eval("id") %>
                                    </td>
                                    <td width="250px">
                                        <%# Eval("code")%>
                                    </td>
                                    <td width="250px">
                                        <%# CurrentSource.loc_cityTitle(Eval("pid_city").objToInt32(), 1, " -! non abbinato !-")%>
                                        -
                                        <%# CurrentSource.locZone_title(Eval("pid_zone").objToInt32(), 1, " -! non abbinato !-")%>
                                    </td>
                                    <td width="100px">
                                        <span style="cursor: move;">trascina</span>
                                    </td>
                                </tr>
                            </table>
                        </li>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <ul id="sortable" class="sortable" style="list-style: none outside none; padding: 0;">
                            <li id="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                </asp:ListView>
                <div class="nulla">
                </div>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript">
            function setSortable() {
                $("#sortable").sortable({ placeholder: "ui-state-highlight" });
                $("#sortable").disableSelection();
            }
            $(function () { setSortable(); });
            function saveSelection() {
                try {
                    var list = "";
                    var sep = "";
                    $("#sortable li").each(function () { list += "" + sep + $(this).attr("refid"); sep = "|"; });
                    $("#<%=HF_order.ClientID %>").val(list);
                    return true;
                }
                catch (ex) { alert(ex); return false; }
            }
        </script>

    </telerik:RadCodeBlock>
</asp:Content>
