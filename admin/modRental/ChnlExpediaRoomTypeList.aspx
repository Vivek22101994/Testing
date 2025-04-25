<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="ChnlExpediaRoomTypeList.aspx.cs" Inherits="ModRental.admin.modRental.ChnlExpediaRoomTypeList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../modContent/UCgetImg.ascx" TagName="UCgetImg" TagPrefix="getImg" %>
<%@ Register Src="~/admin/modContent/UCgetFile.ascx" TagName="UCgetFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
            <div style="clear: both">
                <h1>Gestione ChnlExpediaRoomType</h1>
                <div class="bottom_agg">
                    <asp:LinkButton ID="lnkGetPropertys" runat="server" OnClick="lnkGetPropertys_Click"><span>Scarica Elenco</span></asp:LinkButton>
                </div>
            </div>
            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCchnlExpedia" TableName="dbRntChnlExpediaRoomTypeTBLs" OrderBy="HotelId, name" EntityTypeName="">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("HotelId")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# HotelName(Eval("HotelId").objToInt32())%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("name")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("status") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <%# (IdEstate > 0 && PropertyKeyIsFree(Eval("id")+""))?"<a href='EstateChnlExpedia_main.aspx?id="+IdEstate+"&set="+ Eval("id")+"' style='text-decoration: none; border: 0 none; margin: 5px;'>Seleziona</a>":"" %>
                                <%# IdEstate == 0 ?( PropertyKeyIsFree(Eval("id")+"")?"<span style='color: #f00;'>Non abbinato</span>":"<a href='EstateChnlExpedia_main.aspx?id="+PropertyKeyEstateId(Eval("id")+"")+"' style='color: #0f0;'>Abbinato</a>"):"" %>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("HotelId")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# HotelName(Eval("HotelId").objToInt32())%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("name")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("status") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <%# (IdEstate > 0 && PropertyKeyIsFree(Eval("id")+""))?"<a href='EstateChnlExpedia_main.aspx?id="+IdEstate+"&set="+ Eval("id")+"' style='text-decoration: none; border: 0 none; margin: 5px;'>Seleziona</a>":"" %>
                                <%# IdEstate == 0 ?( PropertyKeyIsFree(Eval("id")+"")?"<span style='color: #f00;'>Non abbinato</span>":"<a href='EstateChnlExpedia_main.aspx?id="+PropertyKeyEstateId(Eval("id")+"")+"' style='color: #0f0;'>Abbinato</a>"):"" %>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" style="">
                            <tr>
                                <td>No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="table_fascia">
                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                <tr style="text-align: left">
                                    <th style="width: 100px">HotelId</th>
                                    <th style="width: 250px">HotelName</th>
                                    <th style="width: 100px">RoomTypeId </th>
                                    <th style="width: 250px">RoomTypeName</th>
                                    <th style="width: 50px">Attivo? </th>
                                    <th></th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                        <div class="page">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;">
                                <Fields>
                                    <asp:NumericPagerField />
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
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
