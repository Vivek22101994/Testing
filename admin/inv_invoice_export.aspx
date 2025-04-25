<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="inv_invoice_export.aspx.cs" Inherits="RentalInRome.admin.inv_invoice_export" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_type" Value="exp1" runat="server" />
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_fileName" Value="" runat="server" />
            <asp:HiddenField ID="HF_mail_1" Value="jacopo.calabro@rentalinrome.com " runat="server" Visible="false" />
            <asp:HiddenField ID="HF_mail_2" Value="iamotti@smistudio.it" runat="server" Visible="false" />
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>
                            Gestione Esportazione Fatture</h1>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" onitemcommand="LV_ItemCommand">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("Date")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("Name")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("InvoiceIDsCount")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("MailSentCount")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("ID") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="seleziona" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("Date")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("Name")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("InvoiceIDsCount")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("MailSentCount")%></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("ID") %>' />
                                        <asp:ImageButton ID="ibtn_select" AlternateText="sch." runat="server" Height="9px" CommandName="seleziona" ImageUrl="~/images/ico/ico_scheda.gif" ToolTip="Scheda" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 3px;" />
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
                                    <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                                        <tr id="Tr1" runat="server" style="text-align: left">
                                            <th style="width: 120px;">
                                                Data Esportazione
                                            </th>
                                            <th style="width: 150px;">
                                                Nome 
                                            </th>
                                            <th style="width: 100px;">
                                                Num. Fatture
                                            </th>
                                            <th style="width: 80px;">
                                                Inviato
                                            </th>
                                            <th id="Th3" runat="server">
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                            </LayoutTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <SelectedItemTemplate>
                                <tr class="current">
                                    <td>
                                        <span>
                                            <%# Eval("Date")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("Name")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("InvoiceIDsCount")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("MailSentCount")%></span>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </SelectedItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Panel ID="pnlContent" runat="server" Width="100%" Visible="false">
                <h1 class="titolo_main">
                    Scheda Esportazione
                </h1>
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click"><span>Invia Mail</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Chiudi</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_createPdf" runat="server" OnClick="lnk_createPdf_Click"><span>Crea il file</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva" id="pnlGetPdf" runat="server">
                        <a target="_blank" href="/log/invoice_export/files/<%= HF_fileName.Value %>"><span>Link al file</span></a>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
                <div class="nulla">
                </div>
                <div class="mainline">
                    <!-- BOX 1 -->
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                        </div>
                        <div class="center">
                            <div class="boxmodulo">
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td class="td_title">
                                            Nome:
                                        </td>
                                        <td>
                                            <asp:Literal ID="ltr_name" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Data:
                                        </td>
                                        <td>
                                            <asp:Literal ID="ltr_date" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Num. Fatture:
                                        </td>
                                        <td>
                                            <asp:Literal ID="ltr_count" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Mail Inviato
                                        </td>
                                        <td>
                                            <asp:Literal ID="ltr_countMail" runat="server"></asp:Literal> volte
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
