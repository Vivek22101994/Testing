<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="ChnlRentalsUnitedAcceptedPaymentForms.aspx.cs" Inherits="ModRental.admin.modRental.ChnlRentalsUnitedAcceptedPaymentForms" %>

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
                <h1>RentalsUnited Accepted Payment Forms</h1>
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
            <div style="clear: both">
                <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("paymentFormType")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cardCode")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cardType")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_paymentFormType" Visible="false" runat="server" Text='<%# Eval("paymentFormType") %>' />
                                <asp:Label ID="lbl_cardCode" Visible="false" runat="server" Text='<%# Eval("cardCode") %>' />
                                <asp:Label ID="lbl_cardType" Visible="false" runat="server" Text='<%# Eval("cardType") %>' />
                                <asp:Label ID="lbl_isActive" Visible="false" runat="server" Text='<%# Eval("isActive") %>' />
                                <asp:CheckBox ID="chk" runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("paymentFormType")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cardCode")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cardType")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_paymentFormType" Visible="false" runat="server" Text='<%# Eval("paymentFormType") %>' />
                                <asp:Label ID="lbl_cardCode" Visible="false" runat="server" Text='<%# Eval("cardCode") %>' />
                                <asp:Label ID="lbl_cardType" Visible="false" runat="server" Text='<%# Eval("cardType") %>' />
                                <asp:Label ID="lbl_isActive" Visible="false" runat="server" Text='<%# Eval("isActive") %>' />
                                <asp:CheckBox ID="chk" runat="server" />
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
                                    <th style="width: 100px">paymentFormType</th>
                                    <th style="width: 100px">cardCode</th>
                                    <th style="width: 100px">cardType</th>
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
