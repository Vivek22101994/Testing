<%@ Page Title="" Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="wlPaymentConfig.aspx.cs" Inherits="RentalInRome.affiliatesarea.wlPaymentConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <h3 style="margin-bottom: 20px; margin-left: 8px;" class="underlined">Payment Gateway Settings For Your Website</h3>
            <div class="nulla">
            </div>
            <div class="agentprice">
                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                    <tr>
                        <td class="td_title" style="width: auto;">Paypal Email:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_email" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click">
                             <span> <%=CurrentSource.getSysLangValue("lblSaveChanges")%></span>
                            </asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click">
                             <span><%=CurrentSource.getSysLangValue("lblCancelChanges")%></span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="nulla">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
