<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="ChnlExpediaRoomAmenitiesList.aspx.cs" Inherits="MagaRentalCE.admin.modRental.ChnlExpediaRoomAmenitiesList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Expedia Amenities</h1>
                <div class="nulla"></div>
                <span class="dr_values">*Dropdown values Highlighed in red color are mandatory.</span>
                <div style="height: 20px;"></div>
                <div class="salvataggio saveAgencyDetails">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click"><span><%# contUtils.getLabel("lblSaveChanges")%></span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>

                </div>
            </div>
            <div class="nulla">
            </div>

            <div class="nulla">
            </div>
            <div style="clear: both">
                <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("name")%></span>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:Label ID="lbl_detail_code" Visible="false" runat="server" Text='<%# Eval("pidDetailCode") %>' />
                                <asp:Label ID="lbl_detail_code_required" Visible="false" runat="server" Text='<%# Eval("isDetailCodeRequired") %>' />
                            </td>
                            <td>
                                <asp:DropDownList ID="drp_detailCode" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="drp_value" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_name" Visible="false" runat="server" Text='<%# Eval("name") %>' />
                                <asp:Label ID="lbl_refId" Visible="false" runat="server" Text='<%# Eval("refId") %>' />
                                <asp:DropDownList ID="drp_refId" runat="server"></asp:DropDownList>
                            </td>

                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("name")%></span>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:Label ID="lbl_detail_code" Visible="false" runat="server" Text='<%# Eval("pidDetailCode") %>' />
                                <asp:Label ID="lbl_detail_code_required" Visible="false" runat="server" Text='<%# Eval("isDetailCodeRequired") %>' />
                            </td>
                            <td>
                                <asp:DropDownList ID="drp_detailCode" runat="server"></asp:DropDownList>
                            </td>
                             <td>
                                <asp:DropDownList ID="drp_value" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_name" Visible="false" runat="server" Text='<%# Eval("name") %>' />
                                <asp:Label ID="lbl_refId" Visible="false" runat="server" Text='<%# Eval("refId") %>' />
                                <asp:DropDownList ID="drp_refId" runat="server"></asp:DropDownList>
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
                                    <th style="width: 250px">Code</th>
                                    <th style="width: 250px">Dropdown Value</th>
                                    <th style="width: 250px">Value</th>
                                    <th></th>                                    
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
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
</asp:Content>
