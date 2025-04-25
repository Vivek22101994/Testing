<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="BlockedIpList.aspx.cs" Inherits="ModAppServerCommon.BlockedIpList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
            <div id="fascia1">
                <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
                    <div style="clear: both">
                        <h1>
                            Elenco IP bloccati</h1>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="filt">
                        <div class="t">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                        <div class="c">
                            <div class="filtro_cont">
                                <table border="0" cellpadding="0" cellspacing="0" id="tbl_new">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <label>Aggiungi Nuovo Ip:</label>
                                                        <asp:TextBox ID="txtAddNewIp" runat="server" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="bottom">
                                                        <a href="#" onclick="addNewIp(); return false;" class="inlinebtn">Aggiungi </a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                            <script type="text/javascript">
                                                function FORM_validateIp(str) {
                                                    return str.match(/^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/);
                                                }
                                                function addNewIp() {
                                                    if (!FORM_validateIp($("#<%= txtAddNewIp.ClientID %>").val())) {
                                                        radalert("Ip non valido.");
                                                        return;
                                                    }
                                                    var _url = "BlockedIpAddNew.aspx";
                                                    _url += "?ip=" + $("#<%= txtAddNewIp.ClientID %>").val();
                                                    //radalert(_url);return;
                                                    $.ajax({
                                                        type: "GET",
                                                        url: _url,
                                                        dataType: "html",
                                                        success: function (html) {
                                                            if (html == "exist") {
                                                                radalert("Ip esistente.");
                                                            } 
                                                            else if (html == "notvalid") {
                                                                radalert("Ip non valido.");
                                                            }
                                                            else if (html != "") {
                                                                radalert("Ip '" + html + "' è stato aggiunto correttamente");
                                                                $("#<%= txtAddNewIp.ClientID %>").val("")
                                                                $find('<%= pnlFascia.ClientID %>').ajaxRequest('rwdUrl_Closing');
                                                            }
                                                        }
                                                    });
                                                }
                                            </script>
                                            </telerik:RadCodeBlock>
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
                    <div class="nulla">
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" OnSelectedIndexChanging="LV_SelectedIndexChanging" OnItemCommand="LV_ItemCommand">
                            <ItemTemplate>
                                <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                    <td>
                                        <span>
                                            <%# Eval("ip")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_uid" Visible="false" runat="server" Text='<%#Eval("uid") %>'></asp:Label>
                                        <asp:LinkButton ID="lnk_edit" runat="server" CommandName="select" Style="margin-top: 2px; margin-right: 10px;">modifica</asp:LinkButton>
                                        <asp:LinkButton ID="lnk_delete" runat="server" CommandName="elimina" Style="margin-top: 2px; margin-right: 10px;" OnClientClick="return confirm('Sta per eliminare il Record?')">elimina</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                    <td>
                                        <span>
                                            <%# Eval("ip")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_uid" Visible="false" runat="server" Text='<%#Eval("uid") %>'></asp:Label>
                                        <asp:LinkButton ID="lnk_edit" runat="server" CommandName="select" Style="margin-top: 2px; margin-right: 10px;">modifica</asp:LinkButton>
                                        <asp:LinkButton ID="lnk_delete" runat="server" CommandName="elimina" Style="margin-top: 2px; margin-right: 10px;" OnClientClick="return confirm('Sta per eliminare il Record?')">elimina</asp:LinkButton>
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
                            <InsertItemTemplate>
                            </InsertItemTemplate>
                            <LayoutTemplate>
                                <div class="table_fascia">
                                    <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0" cellpadding="0" style="">
                                        <tr id="Tr1" runat="server" style="">
                                            <th id="Th1" runat="server" width="300px">
                                                Ip
                                            </th>
                                            <th id="Th5" runat="server">
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                            </LayoutTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <SelectedItemTemplate>
                                <tr class="current">
                                    <td>
                                        <span class="selez">
                                            <asp:TextBox ID="txt_ip" runat="server" Text='<%# Eval("ip")%>' Style="font-size: 11px; width: 250px; height: 12px;"></asp:TextBox>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_uid" Visible="false" runat="server" Text='<%# Eval("uid") %>'></asp:Label>
                                        <asp:LinkButton ID="lnk_save" runat="server" CommandName="salva" Style="margin-top: 2px; margin-right: 10px;">salva</asp:LinkButton>
                                    </td>
                                </tr>
                            </SelectedItemTemplate>
                        </asp:ListView>
                    </div>
                </telerik:RadAjaxPanel>
            </div>
</asp:Content>
