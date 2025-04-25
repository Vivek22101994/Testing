<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="RentalInRome.affiliatesarea.Contacts" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <%-- <div class="sx" style="width:262px;">
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:UC_sx ID="UC_sx1" runat="server" />
                </div>
            </div>
        </div>--%>
    <h3 style="margin-bottom: 20px; margin-left: 8px;" class="underlined">Contacts</h3>
    <asp:Literal ID="ltr_nameFull" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_nameCompany" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_email" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_phone" runat="server" Visible="false"></asp:Literal>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="pnl_request_sent" class="box_client_booking" runat="server" visible="false" style="width: 545px;">
                <%=CurrentSource.getSysLangValue("lblYourRequestRequest")%>
            </div>
            <div id="pnl_request_cont" class="box_client_booking" runat="server" style="width: 545px;">
                <div class="">
                    <div class="left check_list">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("reqRequest")%>
                        </label>
                        <div>
                            <span id="txt_note_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;"></span>
                            <textarea id="txt_note" runat="server" rows="10" cols="50" tabindex="27" style="height: 150px; width: 540px; font-size: 12px;"></textarea>
                        </div>
                    </div>
                    <asp:LinkButton ID="lnk_send" CssClass="btn" runat="server" OnClick="lnk_send_Click" OnClientClick="return RNT_validateRequestForm()"><span><%=CurrentSource.getSysLangValue("lblSubmit")%></span></asp:LinkButton>
                    <div class="nulla">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function RNT_validateRequestForm() {
            var _validate = true;
            $("#txt_note_check").hide();
            $("#<%= txt_note.ClientID%>").removeClass(FORM_errorClass);

            if ($.trim($("#<%= txt_note.ClientID%>").val()) == "") {
                $("#<%= txt_note.ClientID%>").addClass(FORM_errorClass);
                $("#txt_note_check").html('This field is required. Please enter a value.');
                $("#txt_note_check").css("display", "block");
                _validate = false;
            }
            if ($("#<%= txt_note.ClientID%>").val().toLowerCase().indexOf("<") >= 0 || $("#<%= txt_note.ClientID%>").val().toLowerCase().indexOf(">") >= 0) {
                $("#<%= txt_note.ClientID%>").addClass(FORM_errorClass);
                $("#txt_note_check").html('These characters are not allowed: < >')
                $("#txt_note_check").css("display", "block");
                _validate = false;
            }
            return _validate;
        }
    </script>
</asp:Content>
