<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_visa.ascx.cs" Inherits="RentalInRome.uc.UC_rnt_reservation_visa" %>
<asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_IdReservation" runat="server" Value="0" />
<asp:HiddenField ID="HF_visa_persons" runat="server" Value="0" />
<h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
    <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
</h3>
<div class="nulla">
</div>
<span class="tit_sez">
    <%=CurrentSource.getSysLangValue("pdf_CustomerData")%></span>
<div class="nulla">
</div>
<div id="pnl_request_sent" class="box_client_booking" runat="server" visible="false" style="width: 545px;">
    <%=CurrentSource.getSysLangValue("reqRequestSent")%>
</div>
<div id="pnl_request_cont" class="box_client_booking" runat="server" style="width: 545px;">
    <asp:ListView ID="LV" runat="server" OnItemDataBound="LV_ItemDataBound">
        <ItemTemplate>
            <div class="line" style="border: none;">
                <div id="Div1" class="left">
                    <label class="desc">
                        <%=CurrentSource.getSysLangValue("reqFullName")%>*
                    </label>
                    <div>
                        <asp:TextBox ID="txt_name_full" runat="server" Style="margin-right: 10px; width: 310px;"></asp:TextBox>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="line" style="border: none;">
                <div id="drp_doc_type_cont" class="left">
                    <label class="desc">
                        <%=CurrentSource.getSysLangValue("pdf_Documento_d_identita")%>*
                    </label>
                    <div>
                        <asp:DropDownList ID="drp_doc_type" runat="server" Style="margin-right: 10px; width: 310px;">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="line" style="border: none;">
                <div id="txt_doc_num_cont" class="left">
                    <label class="desc">
                        <%=CurrentSource.getSysLangValue("pdf_Num_Documento")%>*
                    </label>
                    <div>
                        <asp:TextBox ID="txt_doc_num" runat="server" Style="margin-right: 10px; width: 310px;"></asp:TextBox>
                    </div>
                </div>
                <div id="txt_doc_issue_place_cont" class="left" runat="server" visible="false">
                    <label class="desc">
                        <%=CurrentSource.getSysLangValue("pdf_RilasciatoDal")%>*
                    </label>
                    <div>
                        <asp:TextBox ID="txt_doc_issue_place" runat="server" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div id="txt_doc_expiry_date_cont" class="left" runat="server" visible="false">
                    <label class="desc">
                        <%=CurrentSource.getSysLangValue("pdf_Data_di_scadenza")%>*
                    </label>
                    <div>
                        <asp:TextBox ID="txt_doc_expiry_date" runat="server" Style="width: 142px"></asp:TextBox>
                        <asp:HiddenField ID="HF_doc_expiry_date" runat="server" />
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="line">
            </div>
            <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
        </ItemTemplate>
        <EmptyDataTemplate>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <div id="itemPlaceholder" runat="server" />
        </LayoutTemplate>
    </asp:ListView>
    <asp:LinkButton ID="lnk_saveClientData" CssClass="btn bonifico" runat="server" OnClick="lnk_saveClientData_Click"><span>Submit</span></asp:LinkButton>
</div>
<div class="nulla">
</div>
