<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_inv_invoice_data.ascx.cs" Inherits="RentalInRome.admin.uc.UC_inv_invoice_data" %>
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_IdClient" runat="server" />
<asp:HiddenField ID="HF_IdInvoice" runat="server" />
<%--<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia Dati Cliente</asp:LinkButton>
        <h3>
            Dati Cliente</h3>
        <div class="price_div">
            <asp:Literal ID="ltr_clientDetailsHTML" runat="server"></asp:Literal>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Cambia Dati Cliente</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            <table class="selPeriod" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="3">
                        # Fattura<br />
                        <asp:TextBox ID="txt_invoice_number" runat="server" Style="width: 340px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        Indirizzo<br />
                        <asp:TextBox ID="txt_cl_loc_address" runat="server" Style="width: 340px;"></asp:TextBox>
                    </td>
                </tr>              
            </table>
            <div class="btnric" style="float: left; margin: 50px;">
                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click">Salva</asp:LinkButton>
            </div>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <div class="nulla">
    </div>
    <script type="text/javascript">
        var invoice_date;        
        function setCal() {
            cal_dtCreation_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtCreation_from.ClientID %>", View: "#txt_dtCreation_from", Cleaner: "#del_dtCreation_from", changeMonth: true, changeYear: true });
            cal_dtCreation_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtCreation_to.ClientID %>", View: "#txt_dtCreation_to", Cleaner: "#del_dtCreation_to", changeMonth: true, changeYear: true });

            cal_inv_dtInvoice_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_inv_dtInvoice_from.ClientID %>", View: "#txt_inv_dtInvoice_from", Cleaner: "#del_inv_dtInvoice_from", changeMonth: true, changeYear: true });
            cal_inv_dtInvoice_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_inv_dtInvoice_to.ClientID %>", View: "#txt_inv_dtInvoice_to", Cleaner: "#del_inv_dtInvoice_to", changeMonth: true, changeYear: true });
        }
    </script>
</div>--%>