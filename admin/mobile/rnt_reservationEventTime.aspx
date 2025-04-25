<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminMobile.Master" AutoEventWireup="true" CodeBehind="rnt_reservationEventTime.aspx.cs" Inherits="RentalInRome.admin.mobile.rnt_reservationEventTime" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <asp:HiddenField ID="HF_id" runat="server" Value="0" />
    <asp:HiddenField ID="HF_code" runat="server" />
    <asp:HiddenField ID="HF_inOut" runat="server" Value="out" />
    <div class="sezione">
        <span>
            Cambio Orario <%=HF_inOut.Value == "out" ? "Check-Out" : "Check-In"%><br />
            Pren. # <%=HF_code.Value %><br />
            <asp:Literal ID="ltr_date" runat="server"></asp:Literal>
        </span>
    </div>
    <div class="contenuto" id="pnl_main" runat="server">
        <div class="button_segnala">
            <table style="margin-left: 50px;">
                <tr>
                    <td>
                        in Data
                    </td>
                    <td>
                        <asp:DropDownList ID="drp_date" runat="server" Style="margin-top: 2px; font-size: 11px;">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        alle Ore
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="drp_time_h" Style="width: 55px; margin-top: 2px; font-size: 11px;">
                        </asp:DropDownList>
                        :<asp:DropDownList runat="server" ID="drp_time_m" Style="width: 55px; margin-top: 2px; margin-left: 5px; font-size: 11px;">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Attenzione!
                        <br/> 
                        L'orario e la data inserita corrisponde 
                        <br />
                        a quella in cui il cliente esce 
                        <br />
                        dall'appartamento fisicamente, 
                        <br />
                        e non in quale si effettua l'accoglienza.
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="border-top: 1px dotted #ECECEC;">
                        Note Deposito
                    <br/>
                        <asp:TextBox ID="txt_pr_depositNotes" runat="server" TextMode="MultiLine" Width="200" Height="50"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="button_segnala" id="pnlOk" runat="server" visible="false">
            Orario è stato salvato correttamente.
        </div>
        <div class="button_segnala">
            <asp:LinkButton ID="lnk_showMode1" runat="server" OnClick="lnk_save_Click" CommandArgument="ritardoCl"><span>Salva</span></asp:LinkButton>
        </div>
        <div class="button_segnala">
            <asp:HyperLink ID="HL_back" runat="server"><span>&lt;&lt;&nbsp;Torna</span></asp:HyperLink>
        </div>
        <div class="nulla">
        </div>
    </div>
</asp:Content>
