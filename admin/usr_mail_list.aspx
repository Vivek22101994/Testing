<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_mail_list.aspx.cs" Inherits="RentalInRome.admin.usr_mail_list" %>

<%@ Register Src="uc/UC_mailbody.ascx" TagName="UC_mailbody" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_pid_user" Value="0" runat="server" />
            <asp:HiddenField ID="HF_usr_mail" Value="0" runat="server" />
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:Literal ID="ltr_new_messages" runat="server"></asp:Literal>
            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaMail_DataContext" TableName="MAIL_TBL_MESSAGE" OrderBy="date_received desc" Where="pid_user == @pid_user">
                <WhereParameters>
                    <asp:ControlParameter ControlID="HF_pid_user" Name="pid_user" PropertyName="Value" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Elenco Mail di <%=HF_usr_mail.Value%></h1>
                        <div class="bottom_agg">
                            <asp:DropDownList ID="drpUpdateMessages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpUpdateMessages_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="- Scarica ultime e-mail da GMail"></asp:ListItem>
                                <asp:ListItem Value="100" Text="ultime 100"></asp:ListItem>
                                <asp:ListItem Value="200" Text="ultime 200"></asp:ListItem>
                                <asp:ListItem Value="300" Text="ultime 300"></asp:ListItem>
                                <asp:ListItem Value="400" Text="ultime 400"></asp:ListItem>
                                <asp:ListItem Value="500" Text="ultime 500"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemDataBound="LV_ItemDataBound" OnSelectedIndexChanging="LV_SelectedIndexChanging" OnItemCommand="LV_ItemCommand">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')" id="tr_normal" runat="server" style="cursor: pointer;">
                                    <td>
                                        <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Style="display: none;" />
                                        <asp:LinkButton ID="lnk_select" runat="server" Style="display: none;" CommandName="select"></asp:LinkButton>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>Nuovo</strong>": ""%>
                                            <%# ("" + Eval("is_new") == "0" && "" + Eval("pid_request") == "0" && "" + Eval("pid_request_state") == "0") ? "<strong>Letto</strong>" : ""%>
                                            <%# ("" + Eval("is_new") == "0" && "" + Eval("pid_request") != "0" && "" + Eval("pid_request_state") == "0") ? "<span style='color:#333366'>Richiesta</span>" : ""%>
                                            <%# ("" + Eval("is_new") == "0" && "" + Eval("pid_request") != "0" && "" + Eval("pid_request_state") != "0") ? "<span style='color:#333366'>Nota Cliente</span>" : ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# Eval("from_name")%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# Eval("from_email")%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# Eval("subject") %>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# "" + ((DateTime)Eval("date_received")).formatITA_Long(true, true)%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# ((DateTime)Eval("date_received")).formatTime(":", true)%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')" id="tr_normal" runat="server" style="cursor: pointer;">
                                    <td>
                                        <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Style="display: none;" />
                                        <asp:LinkButton ID="lnk_select" runat="server" Style="display: none;" CommandName="select"></asp:LinkButton>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>Nuovo</strong>": ""%>
                                            <%# ("" + Eval("is_new") == "0" && "" + Eval("pid_request") == "0" && "" + Eval("pid_request_state") == "0") ? "<strong>Letto</strong>" : ""%>
                                            <%# ("" + Eval("is_new") == "0" && "" + Eval("pid_request") != "0" && "" + Eval("pid_request_state") == "0") ? "<span style='color:#333366'>Richiesta</span>" : ""%>
                                            <%# ("" + Eval("is_new") == "0" && "" + Eval("pid_request") != "0" && "" + Eval("pid_request_state") != "0") ? "<span style='color:#333366'>Nota Cliente</span>" : ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# Eval("from_name")%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# Eval("from_email")%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# Eval("subject") %>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# "" + ((DateTime)Eval("date_received")).formatITA_Long(true, true)%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# ((DateTime)Eval("date_received")).formatTime(":", true)%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
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
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 1200px;">
                                        <tr style="text-align: left">
                                            <th>
                                                Stato
                                            </th>
                                            <th>
                                                Nome Mittente
                                            </th>
                                            <th>
                                                Email Mittente
                                            </th>
                                            <th>
                                                Oggetto
                                            </th>
                                            <th id="Th3" runat="server" style="width: 150px;">
                                                Data
                                            </th>
                                            <th id="Th2" runat="server" style="width: 60px;">
                                                Ora
                                            </th>
                                            <th id="Th1" runat="server">
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server" />
                                    </table>
                                </div>
                                <div class="page">
                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;">
                                        <Fields>
                                            <asp:NumericPagerField ButtonCount="20" />
                                        </Fields>
                                    </asp:DataPager>
                                    <asp:Label ID="lbl_record_count" runat="server" CssClass="total" Text=""></asp:Label>
                                </div>
                            </LayoutTemplate>
                            <SelectedItemTemplate>
                                <tr class="current" id="tr_selected" runat="server" style="cursor: pointer;">
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:LinkButton ID="lnk_close" runat="server" Style="display: none;" CommandName="close"></asp:LinkButton>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>Nuovo</strong>": ""%>
                                            <%# ("" + Eval("is_new") == "0" && "" + Eval("pid_request") == "0" && "" + Eval("pid_request_state") == "0") ? "<strong>Letto</strong>" : ""%>
                                            <%# ("" + Eval("is_new") == "0" && "" + Eval("pid_request") != "0" && "" + Eval("pid_request_state") == "0") ? "<span style='color:#333366'>Richiesta</span>" : ""%>
                                            <%# ("" + Eval("is_new") == "0" && "" + Eval("pid_request") != "0" && "" + Eval("pid_request_state") != "0") ? "<span style='color:#333366'>Nota Cliente</span>" : ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# Eval("from_name")%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# Eval("from_email")%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# Eval("subject") %>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# "" + ((DateTime)Eval("date_received")).formatITA_Long(true, true)%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# (""+Eval("is_new")=="1")?"<strong>": ""%>
                                            <%# ((DateTime)Eval("date_received")).formatTime(":", true)%>
                                            <%# (""+Eval("is_new")=="1")?"</strong>": ""%>
                                        </span>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="current">
                                    <td colspan="6">
                                        <uc1:UC_mailbody ID="UC_mailbody" runat="server" />
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
            <asp:PlaceHolder ID="PH_body" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
