<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="pdf.aspx.cs" Inherits="RentalInRome.reservationarea.pdf" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Download Pdf</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_id" runat="server" Value="" Visible="false" />
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <div id="contatti">
        <div class="sx">
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <%=CurrentSource.getSysLangValue("lblYourReservation")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:UC_sx ID="UC_sx1" runat="server" />
                </div>
            </div>
        </div>
        <div class="dx reservation_details_dx">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Literal ID="ltr_error" runat="server" Visible="false"></asp:Literal>
                    <h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                        <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
                    </h3>
                    <div class="nulla">
                    </div>
                    <span class="tit_sez">Fatture/Voucher</span>
                    <div class="nulla">
                    </div>
                    <div id="pnl_request_cont" class="box_client_booking" runat="server" style="width: 545px;">
                        <asp:HyperLink ID="HL_voucher" runat="server" CssClass="btnDownload inattivo" Target="_blank" Enabled="false">
                            <span style="width: 415px;">
                                Download Voucher
                            </span>
                        </asp:HyperLink>
                        <div class="nulla">
                        </div>
                        <asp:ListView runat="server" ID="LV_invoice" DataSourceID="LDS_invoice" OnItemDataBound="LV_invoice_ItemDataBound">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lbl_uid" Text='<%# Eval("uid") %>' Visible="false"></asp:Label>
                                <asp:HyperLink ID="HL_pdf" runat="server" CssClass="btnDownload inattivo" Target="_blank" Enabled="false" Style="margin-top: 10px;">
                                    <span style="width: 415px;">
                                        Download Invoice #<%# Eval("code")%>
                                    </span>
                                </asp:HyperLink>
                                <div class="nulla">
                                </div>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="nulla">
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                        <asp:LinqDataSource ID="LDS_invoice" runat="server" ContextTypeName="RentalInRome.data.magaInvoice_DataContext" TableName="INV_TBL_INVOICE" Where="rnt_pid_reservation == @rnt_pid_reservation" OrderBy="inv_dtInvoice">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="HF_id" Name="rnt_pid_reservation" PropertyName="Value" Type="Int32" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                        <%=ltr_error.Text %>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
