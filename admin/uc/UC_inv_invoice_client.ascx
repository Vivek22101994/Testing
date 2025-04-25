<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_inv_invoice_client.ascx.cs" Inherits="RentalInRome.admin.uc.UC_inv_invoice_client" %>
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_IdClient" runat="server" />
<asp:HiddenField ID="HF_IdInvoice" runat="server" />
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia Dati Cliente</asp:LinkButton>
        <h3>Dati Cliente</h3>
        <div class="price_div">
            <asp:Literal ID="ltr_clientDetailsHTML" runat="server"></asp:Literal>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>Cambia Dati Cliente</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            <table class="selPeriod" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="3">Denominazione<br />
                        <asp:TextBox ID="txt_cl_name_full" runat="server" Style="width: 340px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">Indirizzo<br />
                        <asp:TextBox ID="txt_cl_loc_address" runat="server" Style="width: 340px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>CAP<br />
                        <asp:TextBox ID="txt_cl_loc_zip_code" runat="server"></asp:TextBox>
                    </td>
                    <td>Città<br />
                        <asp:TextBox ID="txt_cl_loc_city" runat="server"></asp:TextBox>
                    </td>
                    <td>Provincia/Stato<br />
                        <asp:TextBox ID="txt_cl_loc_state" runat="server" Style="width: 120px;"></asp:TextBox>
                    </td>
                    <td>Provincia<br />
                        <asp:TextBox ID="txt_cl_loc_province" runat="server" Style="width: 120px;" MaxLength="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>

                    <td>Codice fiscale<br />
                        <asp:TextBox ID="txt_cl_doc_cf_num" runat="server" Style="width: 340px;"></asp:TextBox>
                    </td>
                    
                    <td>Codice Destinatario<br />
                        <asp:TextBox ID="txt_codice_destinatario" runat="server" Style="width: 120px;" MaxLength="7"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                     <td>Vat<br />
                        <asp:TextBox ID="txt_cl_doc_vat_num" runat="server" Style="width: 340px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">Nazione/Location<br />
                        <asp:DropDownList runat="server" ID="drp_cl_loc_country" CssClass="field select large" Style="width: 350px;" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                            <WhereParameters>
                                <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                            </WhereParameters>
                        </asp:LinqDataSource>
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
</div>
