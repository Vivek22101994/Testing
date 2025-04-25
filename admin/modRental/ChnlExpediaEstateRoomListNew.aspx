<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="ChnlExpediaEstateRoomListNew.aspx.cs" Inherits="MagaRentalCE.admin.modRental.ChnlExpediaEstateRoomListNew" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
            <div style="clear: both">
                <h1>Expedia Estate </h1>
            </div>

            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCchnlExpedia" TableName="dbRntChnlExpediaEstateTBLs" OrderBy="HotelId" EntityTypeName="">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# GetMagaRentalProeprtyname(Eval("id").objToInt32()) %>
                                </span>
                            </td>
                            <td>
                                <span><%#  Eval("RoomTypeId") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# GetExpediaPropertyName(Eval("RoomTypeId")+"") %>
                                </span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_RoomTypeId" Visible="false" runat="server" Text='<%# Eval("RoomTypeId") %>' />
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />

                                <asp:LinkButton ID="lnk_assign" runat="server" CommandName="edit" Visible='<%# string.IsNullOrWhiteSpace(Eval("RoomTypeId").ToString()) ? false :true %>'> Assign Room</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">

                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# GetMagaRentalProeprtyname(Eval("id").objToInt32()) %>
                                </span>
                            </td>
                            <td>
                                <span><%#  Eval("RoomTypeId") %>  </span>
                            </td>
                            <td>
                                <span>
                                    <%# GetExpediaPropertyName(Eval("RoomTypeId")+"") %>
                                </span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_RoomTypeId" Visible="false" runat="server" Text='<%# Eval("RoomTypeId") %>' />
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnk_assign" runat="server" CommandName="edit" Visible='<%# string.IsNullOrWhiteSpace(Eval("RoomTypeId").ToString()) ? false :true %>'> Assign Room</asp:LinkButton>
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
                                    <th style="width: 50px">MagaRental ID </th>
                                    <th style="width: 100px">MagaRental Name</th>
                                    <th style="width: 100px">Expedia Room Type ID</th>
                                    <th style="width: 250px">Expedia Name</th>
                                    <th style="width: 100px">Assign </th>
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

            <telerik:RadWindow runat="server" ID="rwdDett" Modal="true" AutoSize="true" ShowContentDuringLoad="false" MinWidth="600" OnClientClose="rwdDett_OnClientClose" VisibleStatusbar="false">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
                        <h1 class="titolo_main">Associate other Property with
                            <asp:Literal ID="ltr_est_title" runat="server"></asp:Literal>
                        </h1>
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
                                        <%--<asp:RadioButtonList ID="Rdb_OtherRoom" runat="server" RepeatColumns="2"></asp:RadioButtonList>--%>
                                        <asp:CheckBoxList ID="chk_OtherRooms" runat="server" RepeatColumns="2"></asp:CheckBoxList>
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
                                    <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton>
                                </div>
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                                </div>
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

