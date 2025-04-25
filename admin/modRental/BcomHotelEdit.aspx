<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="BcomHotelEdit.aspx.cs" Inherits="ModRental.admin.modRental.BcomHotelEdit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../modContent/UCgetImg.ascx" TagName="UCgetImg" TagPrefix="getImg" %>
<%@ Register Src="~/admin/modContent/UCgetFile.ascx" TagName="UCgetFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Gestione BcomHotel</h1>
                <div class="bottom_agg">
                    <asp:LinkButton ID="lnkNew" runat="server" OnClick="lnkNew_Click"><span>+ Nuovo</span></asp:LinkButton>
                    <a href="/admin/modRental/export_bcom_properies.aspx?data=bcom" target="_blank"><span>Scarica Elenco in Excel</span></a>
                </div>
            </div>
            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCmodRental" TableName="dbRntBcomHotelTBLs" OrderBy="hotelId" EntityTypeName="">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("hotelId")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("title")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("hotelId") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" CommandName="del" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;" OnClientClick="return confirm('sei sicuro?')">Elimina</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("hotelId")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("title")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("hotelId") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" CommandName="del" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;" OnClientClick="return confirm('sei sicuro?')">Elimina</asp:LinkButton>
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
                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                <tr style="text-align: left">
                                    <th style="width: 100px">HotelId </th>
                                    <th style="width: 150px">Nome Interno</th>
                                    <th style="width: 50px">Attivo? </th>
                                    <th></th>
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
                </asp:ListView>
            </div>
            <telerik:RadWindow runat="server" ID="rwdDett" Modal="true" AutoSize="true" ShowContentDuringLoad="false" MinWidth="450" OnClientClose="rwdDett_OnClientClose" VisibleStatusbar="false">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
                        <h1 class="titolo_main">Scheda </h1>
                        <!-- INIZIO MAIN LINE -->
                        <div class="mainline">
                            <div class="mainbox">
                                <div class="top">
                                    <div class="sx">
                                    </div>
                                    <div class="dx">
                                    </div>
                                </div>
                                <div class="center">
                                    <div class="boxmodulo">
                                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                            
                                            <tr>
                                                <td class="td_title">
                                                    HotelId:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_hotelId" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Nome Interno:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_title" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Attivo?
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_isActive" runat="server">
                                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">username:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_username" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">password:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_password" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">rateIdStandard:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_rateIdStandard" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">rateIdStandard2:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_rateIdStandard2" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">rateIdStandard3:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_rateIdStandard3" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">rateIdStandard4:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_rateIdStandard4" Width="230px" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="td_title">rateIdNotRefund:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_rateIdNotRefund" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">rateIdSpecial:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_rateIdSpecial" Width="230px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="boxmodulo" id="pnl_update" runat="server" style="border: 1px dotted; margin-top: 10px;">
                                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                            <tr>
                                                <th colspan="2">Scarica le pren da Booking.com</th>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Dal
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="rdp_updateDate" runat="server">
                                                        <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:LinkButton ID="lnk_update" runat="server" CssClass="inlinebtn" OnClick="lnk_update_Click">Scarica</asp:LinkButton>
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
                            <div class="nulla">
                            </div>
                           <div class="salvataggio">
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
                                <div class="bottom_salva">
                                    <a onclick="return rwdDettClose();">
                                        <span>Chiudi</span></a>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                        </div>
                    </telerik:RadAjaxPanel>
                </ContentTemplate>
            </telerik:RadWindow>
        </telerik:RadAjaxPanel>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function rwdDettClose() {
                $find("<%= pnlFascia.ClientID%>").ajaxRequest('rwdDett_Closing');
                //RadAjaxManager_ajaxRequest('rwdDett_Closing');
                return false;
            }
            function rwdDett_OnClientClose(sender, eventArgs) {
                return rwdDettClose();
            } 
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
