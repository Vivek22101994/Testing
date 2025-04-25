<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_point.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_point" %>

<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titolo_main">Punti d'interesse della struttura:
    <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal></h1>
    <div id="fascia1">
        <div style="clear: both; margin: 3px 0 5px 30px;">
            <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
        </div>
    </div>
    <div class="nulla">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="" runat="server" />
            <asp:HiddenField ID="HF_pid_city" Value="1" runat="server" />
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <!-- INIZIO MAIN LINE -->
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
                            <table border="0" cellspacing="3" cellpadding="0">
                                <tr>
                                    <td>
                                        <asp:LinqDataSource ID="LDS" runat="server" OrderBy="title" ContextTypeName="RentalInRome.data.magaLocation_DataContext" TableName="LOC_VIEW_POINT" Where="pid_city == @pid_city &amp;&amp; is_acitve == @is_acitve &amp;&amp; pid_lang == @pid_lang">
                                            <WhereParameters>
                                                <asp:ControlParameter ControlID="HF_pid_city" Name="pid_city" PropertyName="Value" Type="Int32" />
                                                <asp:Parameter DefaultValue="1" Name="is_acitve" Type="Int32" />
                                                <asp:Parameter DefaultValue="1" Name="pid_lang" Type="Int32" />
                                            </WhereParameters>
                                        </asp:LinqDataSource>
                                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemDataBound="LV_ItemDataBound" GroupItemCount="2">
                                            <ItemTemplate>
                                                <td style="width: 450px; padding-right: 10px; border-right: thin solid;">
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' />
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                    <div style="float: right; width: 90px;">
                                                        <asp:TextBox ID="txt" Style="width: 60px; text-align: right;" runat="server"></asp:TextBox>&nbsp;m
                                                    </div>
                                                </td>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <td style="width: 450px;">
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' />
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                    <div style="float: right; width: 90px;">
                                                        <asp:TextBox ID="txt" Style="width: 60px; text-align: right;" runat="server"></asp:TextBox>&nbsp;m
                                                    </div>
                                                </td>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <span class="nessun">No data was returned.</span>
                                            </EmptyDataTemplate>
                                            <InsertItemTemplate>
                                            </InsertItemTemplate>
                                            <LayoutTemplate>
                                                <table>
                                                    <tr id="groupPlaceholder" runat="server" />
                                                </table>
                                            </LayoutTemplate>
                                            <GroupTemplate>
                                                <tr>
                                                    <td id="itemPlaceholder" runat="server" />
                                                </tr>
                                            </GroupTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="salvataggio">
                                            <div class="bottom_salva">
                                                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click" CausesValidation="true" TabIndex="28" ValidationGroup="price"><span>Salva Modifiche</span></asp:LinkButton>
                                            </div>
                                            <div class="bottom_salva">
                                                <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla</span></asp:LinkButton>
                                            </div>
                                        </div>
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
                <div class="salvataggio">
                    <div class="nulla">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
