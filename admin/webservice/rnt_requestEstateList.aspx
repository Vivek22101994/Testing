<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rnt_requestEstateList.aspx.cs" Inherits="RentalInRome.admin.webservice.rnt_requestEstateList" %>


    <form id="form1" runat="server" visible="false">
        <asp:Literal ID="ltrLayoutTemplate" runat="server" Visible="false">
            <table class="pref_admin" cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width: 20px;">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        Struttura
                    </td>
                    <td>
                        Zona
                    </td>
                    <td>
                        Prezzo nel periodo
                    </td>
                    <td>
                        Comm.
                    </td>
                    <td>
                        Esclusiva?
                    </td>
                    <td style="width: 11px;">
                    </td>
                </tr>
                #itemPlaceHolder#
            </table>
        </asp:Literal>
        <asp:Literal ID="ltrEmptyDataTemplate" runat="server" Visible="false">
            <div class="listEmpty error">Non ci sono appartamenti preferiti dal cliente</div>
        </asp:Literal>
        <asp:Literal ID="ltrItemTemplate" runat="server" Visible="false">
            <tr>
                <td style="width: 20px;">
                    #ltr_avv#
                </td>
                <td>
                    #HL_book#
                    
                </td>
                <td>
                    <a href="#HL_owner#" class="apt" target="_blank">Prop</a>
                </td>
                <td>
                    <a href="#page_path#" class="apt" target="_blank">#title#</a>
                </td>
                <td>
                    #zoneTitle#
                </td>
                <td>
                    #price#
                </td>
                <td>
                    #pr_percentage#
                </td>
                <td>
                    #is_exclusive#
                </td>
                <td style="width: 11px;">
                    <a href="#" onclick="if(confirm('sta per eliminare la preferenza?')){RNT_fillList('list', '', '#id#')} return false;">
                        <img alt="" src="../images/ico/ico_del.gif" class="ico" alt="" />
                    </a>
                </td>
            </tr>
        </asp:Literal>
    </form>

