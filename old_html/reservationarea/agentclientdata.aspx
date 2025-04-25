<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="agentclientdata.aspx.cs" Inherits="RentalInRome.reservationarea.agentclientdata" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Personal Data</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "agentClientDett.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(700);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                rwdUrl.maximize();
            }
            return false;
        }
        function rwdUrl_OnClientClose(sender, eventArgs) {
            $find('<%= pnlFascia.ClientID %>').ajaxRequest('rwdUrl_Closing');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <asp:HiddenField ID="HF_id" runat="server" Value="" Visible="false" />
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <asp:HiddenField ID="HF_idAgent" runat="server" Value="" Visible="false" />
    <div id="contatti">
        <div class="sx">
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <%=CurrentSource.getSysLangValue("lblYourReservation")%>
                    </telerik:RadCodeBlock>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:uc_sx id="UC_sx1" runat="server" />
                </div>
            </div>
        </div>
        <div class="dx reservation_details_dx">
            <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="reslist" OnAjaxRequest="pnlFascia_AjaxRequest">
                <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                    <h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                        <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
                    </h3>
                    <div class="nulla">
                    </div>
                    <span class="tit_sez">
                        <%=CurrentSource.getSysLangValue("pdf_CustomerData")%></span>
                    <div class="nulla">
                    </div>
                    <div id="pnl_edit" class="box_client_booking" runat="server" style="width: 545px;">
                        <div class="line" style="padding-bottom: 5px; border-bottom-color: #333366;">
                            <div class="left">
                                <label class="desc">
                                    Select your client </label>
                                <div>
                                    <asp:DropDownList ID="drp_client" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <asp:LinkButton ID="lnk_saveClientData" CssClass="btn" runat="server" OnClick="lnk_save_Click"><span>Submit</span></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" CssClass="btn" runat="server" OnClick="lnk_cancel_Click"><span>Cancel</span></asp:LinkButton>
                        <a href="#" onclick="return setUrl('dett', '0')" class="btn">
                            <span>
                                <%=CurrentSource.getSysLangValue("lblCreateNew")%></span>
                        </a>
                    </div>
                    <div id="pnl_view" class="box_client_booking" runat="server" visible="false" style="width: 545px;">
                        <div class="line" style="padding-bottom: 5px; border-bottom-color: #333366;">
                            <div class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqFullName")%>:
                                    <strong style="font-size: 14px; display: block;"><%= tblClient.nameFull%></strong>
                                </label>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border: none;">
                            <div id="txt_locBirth_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("pdf_Nato_a")%>*
                                </label>
                                <div>
                                    <%= tblClient.birthPlace%>
                                </div>
                            </div>
                            <div id="txt_dtBirth_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("pdf_Data_di_Nascita")%>
                                </label>
                                <div>
                                    <%= tblClient.birthDate.formatCustom("#dd#/#mm#/#yy#", CurrentLang.ID, "--/--/----")%>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                        </div>
                        <div class="line" style="border: none;">
                            <div id="drp_doc_type_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("pdf_Documento_d_identita")%>*
                                </label>
                                <div>
                                    <%= authUtils.getDocType_title(tblClient.docType, "")%>
                                </div>
                            </div>
                            <div id="txt_doc_num_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("pdf_Num_Documento")%>*
                                </label>
                                <div>
                                    <%= tblClient.docNum%>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border: none;">
                            <div id="Div4" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblDocIssueDate")%>* </label>
                                <div>
                                    <%= tblClient.docIssueDate.formatCustom("#dd#/#mm#/#yy#", CurrentLang.ID, "--/--/----")%>
                                </div>
                            </div>
                            <div id="txt_doc_issue_place_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("pdf_RilasciatoDal")%>*
                                </label>
                                <div>
                                    <%= tblClient.docIssuePlace%>
                                </div>
                            </div>
                            <div id="txt_doc_expiry_date_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("pdf_Data_di_scadenza")%>*
                                </label>
                                <div>
                                    <%= tblClient.docExpiryDate.formatCustom("#dd#/#mm#/#yy#", CurrentLang.ID, "--/--/----")%>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line">
                        </div>
                        <div class="line" style="border: none;">
                            <div id="txt_loc_address_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("pdf_IndirizzoResidenza")%>*
                                </label>
                                <div>
                                    <%= tblClient.locAddress%>
                                </div>
                            </div>
                            <div id="txt_loc_state_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblRegionState")%>*
                                </label>
                                <div>
                                    <%= tblClient.locState%>
                                </div>
                            </div>
                            <div id="txt_loc_zip_code_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblZipCode")%>*
                                </label>
                                <div>
                                    <%= tblClient.locZipCode%>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border: none;">
                            <div id="txt_loc_city_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblCity")%>*
                                </label>
                                <div>
                                    <%= tblClient.locCity%>
                                </div>
                            </div>
                            <div id="Div1" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqLocation")%>*
                                </label>
                                <div>
                                    <%= tblClient.locCountry%>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border: none;">
                            <div id="txt_contact_phone_mobile_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqPhoneNumber")%>*
                                </label>
                                <div>
                                    <%= tblClient.contactPhoneMobile%>
                                </div>
                            </div>
                            <div id="txt_contact_phone_trip_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblCellulare_Viaggio")%>
                                </label>
                                <div>
                                    <%= tblClient.contactPhoneTrip%>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border: none;">
                            <div id="txt_contact_phone_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblPhone")%>*
                                </label>
                                <div>
                                    <%= tblClient.contactPhone%>
                                </div>
                            </div>
                            <div id="txt_contact_fax_cont" class="left">
                                <label class="desc">
                                    Fax
                                </label>
                                <div>
                                    <%= tblClient.contactFax%>
                                </div>
                            </div>
                            <div id="txt_contact_email_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqEmail")%>
                                </label>
                                <div>
                                    <%= tblClient.contactEmail%>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border: none;">
                            <div id="Div3" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("formVatNumber")%></label>
                                <div>
                                    <%= tblClient.docVat%>
                                </div>
                            </div>
                            <div id="Div2" class="left">
                                <label class="desc">Codice Fiscale (Only for Italy)* </label>
                                <div>
                                    <%= tblClient.docCf%>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <asp:LinkButton ID="LinkButton1" CssClass="btn" runat="server" OnClick="lnk_change_Click"><span>Choose another client</span></asp:LinkButton>
                        <a href="#" onclick="return setUrl('dett', '<%= tblClient.id%>')" class="btn">
                            <span>Change Details of this client</span>
                        </a>
                    </div>
                    <div class="nulla">
                    </div>
                    </telerik:RadCodeBlock>
            </telerik:RadAjaxPanel>
        </div>
    </div>


</asp:Content>
