<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inv_invoice_formNew.aspx.cs" Inherits="RentalInRome.admin.inv_invoice_formNew" %>

<%@ Register Src="~/admin/uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        @import url(/admin/css/adminStyle.css);
        @import url(/jquery/css/ui-core.css);
        @import url(/jquery/css/ui-datepicker.css);
        html, body
        {
            background-color: #FFF;
        }
    </style>
    <script src="../js/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <script src="../js/tiny_mce/init.js" type="text/javascript"></script>
    <script src="../jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../jquery/js/ui--core.min.js" type="text/javascript"></script>
    <script src="../jquery/js/ui-effects.min.js" type="text/javascript"></script>
    <script src="../jquery/js/ui-datepicker.min.js" type="text/javascript"></script>
    <script src="../jquery/datepicker-lang/jquery.ui.datepicker-it.js" type="text/javascript"></script>
    <style type="text/css">
        a.del {
            color: #FF0000;
            display: block;
            font-size: 13px;
            font-weight: bold;
            margin: 4px 0;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10">
        <ProgressTemplate>
            <uc1:UC_loader ID="UC_loader1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel_main" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" runat="server" />
            <asp:HiddenField ID="HF_pr_total" runat="server" Value="0" />
            <asp:HiddenField ID="HF_pr_tf" runat="server" Value="0" />
            <asp:HiddenField ID="HF_pr_tax" runat="server" Value="0" />
            <asp:HiddenField ID="HF_is_complete" runat="server" Value="0" />
            <div id="main">
                <span class="titlight">Emmissione della Nuova fattura manuale</span><div class="mainline">
                    <div class="prices">
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <h3>Dati Cliente / Fatturazione</h3>
                            <div class="price_div">
                                <table class="selPeriod" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="3">
                                            Denominazione<br />
                                            <asp:TextBox ID="txt_cl_name_full" runat="server" Style="width: 340px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            Indirizzo<br />
                                            <asp:TextBox ID="txt_cl_loc_address" runat="server" Style="width: 340px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            CAP<br />
                                            <asp:TextBox ID="txt_cl_loc_zip_code" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            Città<br />
                                            <asp:TextBox ID="txt_cl_loc_city" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            Provincia/Stato<br />
                                            <asp:TextBox ID="txt_cl_loc_state" runat="server" Style="width: 120px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">Provincia<br />
                                            <asp:TextBox ID="txt_loc_province" runat="server" Style="width: 120px;" MaxLength="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            Nazione/Location<br />
                                            <asp:DropDownList runat="server" ID="drp_cl_loc_country" CssClass="field select large" Style="width: 350px;" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                                <WhereParameters>
                                                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                                </WhereParameters>
                                            </asp:LinqDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            Codice fiscale<br />
                                            <asp:TextBox ID="txt_cl_doc_cf_num" runat="server" Style="width: 340px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            Codice Destinatario <br />
                                            <asp:TextBox ID="txt_codice_destinatario" runat="server" Style="width: 340px;" MaxLength="7"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            Partita iva<br />
                                            <asp:TextBox ID="txt_cl_doc_vat_num" runat="server" Style="width: 340px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <h3>Oggetti della fattura </h3>
                            <asp:LinkButton ID="lnk_add_new" runat="server" CssClass="changeapt topright" OnClick="lnk_add_new_Click"><span>Aggiungi</span></asp:LinkButton>
                            <div class="price_div">
                                <table border="0" cellpadding="0" cellspacing="0" style="">
                                    <tr style="text-align: left">
                                        <th style="width: 20px"># </th>
                                        <th style="width: 100px">Item </th>
                                        <th style="width: 400px">Description </th>
                                        <th style="width: 50px; text-align: center;">Quantity </th>
                                        <th style="width: 80px; text-align: center;">
                                            Iva Applicata
                                        </th>
                                        <th style="width: 120px; text-align: center;">Prezzo Totale Ivato </th>
                                        <th></th>
                                    </tr>
                                    <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <span>
                                                        <%# Eval("sequence") %></span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_code" runat="server" Text='<%# Eval("code") %>' Width="90"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_description" runat="server" Text='<%# Eval("description") %>' Width="390"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:TextBox ID="txt_quantity" runat="server" Text='<%# Eval("quantity") %>' Width="30"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:DropDownList ID="drp_cashTaxID" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:TextBox ID="txt_price_total" runat="server" Text='<%# Eval("price_total") %>' Width="100"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                    <asp:Label ID="lbl_sequence" Visible="false" runat="server" Text='<%# Eval("sequence") %>' />
                                                    <asp:Label ID="lbl_cashTaxID" Visible="false" runat="server" Text='<%# Eval("price_tax_id") %>' />
                                                    <asp:LinkButton ID="lnk_del" runat="server" CssClass="del" CommandName="elimina" OnClientClick="return confirm('sta per eliminare oggetto della fattura?')">X</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td colspan="8">
                                                    Non sono presenti Oggetti della fattura!
                                                    <asp:LinkButton ID="lnk_add_new" runat="server" OnClick="lnk_add_new_Click">Aggiungi</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </table>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
                            </div>
                            <h3>Importi</h3>
                            <div class="price_div">
                                <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 110px;">
                                            Imponibile
                                        </td>
                                        <td align="right">
                                            <%=HF_pr_tf.Value + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 110px;">
                                            Imposta 
                                        </td>
                                        <td align="right">
                                            <%=HF_pr_tax.Value + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 110px;">
                                            Importo Fattura
                                        </td>
                                        <td align="right">
                                            <%=HF_pr_total.Value + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <div class="price_div">
                                <div class="btnric" style="float: left; margin: 5px 20px; color: #F00;" id="pnl_error" runat="server" visible="false">
                                    <h2>Ci sono errori da correggere</h2>
                                    <asp:Literal ID="ltr_error" runat="server"></asp:Literal>
                                </div>
                                <div id="Div1" class="btnric" style="float: left; margin: 5px 20px;" runat="server">
                                    <asp:LinkButton ID="lnk_calculate" runat="server" OnClick="lnk_calculate_Click"><span>Calcola Totali</span></asp:LinkButton>
                                </div>
                                <div class="btnric" style="float: left; margin: 5px 20px;" id="pnl_btnSave" runat="server" visible="false">
                                    <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click"><span>Salva</span></asp:LinkButton>
                                </div>
                                <div id="Div2" class="btnric" style="float: left; margin: 5px 20px;" runat="server">
                                    <a href="javascript:parent.Shadowbox.close();">
                                        <span>Annulla / Chiudi</span></a>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
