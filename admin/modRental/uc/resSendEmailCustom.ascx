<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="resSendEmailCustom.ascx.cs" Inherits="ModRental.admin.modRental.ucResDett.resSendEmailCustom" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Src="/admin/uc/UC_mailbody.ascx" TagName="UC_mailbody" TagPrefix="uc1" %>
<style type="text/css">
    .btnOrange {
        background-color: #e55300;
        border-radius: 4px;
        color: #fff;
        float: right;
        font-size: 12px;
        font-weight: bold;
        margin: 20px;
        padding: 8px;
        text-decoration: none;
    }

    .btnclose {
        background-image: url("/admin/images/filtr_piumeno.gif");
        background-position: 0 -23px;
        background-repeat: no-repeat;
        color: #fff;
        cursor: pointer;
        font-size: 10px;
        height: 12px;
        line-height: 12px;
        padding-left: 15px;
        text-decoration: none;
        display: none;
    }

    .btnOpen {
        background-image: url("/admin/images/filtr_piumeno.gif");
        background-position: 0 1px;
        background-repeat: no-repeat;
        color: #fff;
        cursor: pointer;
        font-size: 10px;
        height: 12px;
        line-height: 12px;
        padding-left: 15px;
        text-decoration: none;
    }

    .from {
        text-align: left;
        width: 95%;
        float: left;
    }

    .to {
        text-align: right;
        width: 95%;
        float: right;
    }

    .fromC {
        text-align: left;
        width: 4%;
        float: left;
    }

    .toC {
        text-align: right;
        width: 4%;
        float: right;
    }
</style>
<asp:HiddenField ID="HF_id" Value="0" runat="server" />
<div class="nulla">
</div>
<h2 class="titBoxBooking"><%= contUtils.getLabel("lblSendEmailToCustomer") %></h2>

<asp:PlaceHolder ID="PH_SendNew" runat="server">
    <div class="emailToCustomerHead requestEmailCustomerHead">
        <div class="senderCont" style="display: none">
            <label><%= contUtils.getLabel("lblSender") %></label>
            <asp:TextBox ID="txt_Sender" runat="server"></asp:TextBox>
        </div>
        <div class="objectCont">
            <label style="margin-right: 10px; padding: 10px"><%= contUtils.getLabel("lblObject") %></label>
            <asp:TextBox ID="txt_mail_subject" runat="server" Width="300px"></asp:TextBox>
        </div>
    </div>
    <div class="emailToCustomerBody">
        <telerik:RadEditor runat="server" ID="txt_mailBody" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileLimited.xml">
            <CssFiles>
                <telerik:EditorCssFile Value="/css/styleEditor.css" />
            </CssFiles>
        </telerik:RadEditor>
    </div>
    <asp:LinkButton ID="lnkSendMail" runat="server" CssClass="btnOrange" OnClick="lnkSendMail_Click"><%= contUtils.getLabel("lblSendEmail") %></asp:LinkButton>
</asp:PlaceHolder>

<asp:PlaceHolder ID="PH_history" runat="server" Visible="false">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HF_pid_user" Value="0" runat="server" />
            <asp:HiddenField ID="HF_usr_mail" Value="0" runat="server" />
            <asp:HiddenField ID="HiddenField1" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:Literal ID="ltr_new_messages" runat="server"></asp:Literal>
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>communication with customer</h1>
                        <div class="bottom_agg">
                        </div>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server">
                            <ItemTemplate>
                                <tr id="tr_normal" runat="server" style="cursor: pointer;">
                                    <td>
                                        <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Style="display: none;" />
                                        <asp:LinkButton ID="lnk_select" runat="server" Style="display: none;" CommandName="select" Visible="false"></asp:LinkButton>
                                        <div class="<%# Eval("IsReply").objToInt32()==1 ? "to" :"from" %>">
                                            <span>
                                                <strong><%# Eval("subject") %></strong>
                                            </span>
                                            <span>
                                                <%# "" + ((DateTime)Eval("date_received")).formatITA_Long(false,true)%>
                                            </span>
                                        </div>
                                        <div class="<%# Eval("IsReply").objToInt32()== 1 ? "fromC" :"toC" %>">
                                            <a href="#" id='lnk_open_<%# Eval("id") %>' onclick="showMessage(<%# Eval("id") %>);" class="btnOpen"></a>
                                            <a href="#" id='lnk_close_<%# Eval("id")%>' class="btnclose" style="display: none" onclick="hideMessage(<%# Eval("id") %>);"></a>
                                        </div>

                                    </td>
                                </tr>
                                <tr id="trSelected_<%# Eval("id") %>" style="cursor: pointer; display: none">
                                    <td>
                                        <%# Eval("body_html_text") %> 
                                    </td>

                                </tr>
                            </ItemTemplate>

                            <EmptyDataTemplate>
                                <table id="Table1" runat="server" style="">
                                    <tr>
                                        <td>No data was returned.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div class="table_fascia">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 650px;">
                                        <%-- <tr style="text-align: left">
                                            <th>Stato
                                            </th>
                                            <th>Nome Mittente
                                            </th>
                                            <th>Email Mittente
                                            </th>
                                            <th>Oggetto
                                            </th>
                                            <th id="Th3" runat="server" style="width: 150px;">Data
                                            </th>
                                            <th id="Th2" runat="server" style="width: 60px;">Ora
                                            </th>
                                            <th id="Th1" runat="server"></th>
                                        </tr>--%>
                                        <tr id="itemPlaceholder" runat="server" />
                                    </table>
                                </div>
                            </LayoutTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:PlaceHolder ID="PH_body" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:PlaceHolder>

