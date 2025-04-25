<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_mail_new.aspx.cs" Inherits="RentalInRome.admin.usr_mail_new" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainline">
        <div class="mainbox">
            <div class="top">
                <div style="float: left;">
                    <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                <div style="float: right;">
                    <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
            </div>
            <div class="center">
                <span class="titoloboxmodulo">Nuova mail</span>
                <div class="boxmodulo">
                    <table>
                        <tr>
                            <td>
                                To:<br/>
                                <asp:TextBox ID="txt_to" runat="server" Width="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Subject:<br />
                                <asp:TextBox ID="txt_subject" runat="server" Width="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Body:<br />
                                <asp:TextBox ID="txt_body" runat="server" TextMode="MultiLine" Width="500" Height="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnk_send" runat="server" onclick="lnk_send_Click">Send</asp:LinkButton>
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
</asp:Content>
