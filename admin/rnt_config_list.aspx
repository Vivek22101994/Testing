<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_config_list.aspx.cs" Inherits="RentalInRome.admin.rnt_config_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_VIEW_CONFIGs" OrderBy="title" Where="pid_lang == @pid_lang">
                <WhereParameters>
                    <asp:Parameter DefaultValue="1" Name="pid_lang" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>
                            Gestione accessori
                        </h1>
                        <div class="bottom_agg">
                            <asp:LinkButton ID="lnk_nuovo" runat="server" OnClick="lnk_nuovo_Click" OnClientClick="removeTinyEditor();"><span>+ Nuovo</span></asp:LinkButton>
                        </div>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnSelectedIndexChanging="LV_SelectedIndexChanging" onpagepropertieschanging="LV_PagePropertiesChanging">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:LinkButton ID="lnk_select" runat="server" CommandName="select" Style="margin-top: 5px;">scheda</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:LinkButton ID="lnk_select" runat="server" CommandName="select" Style="margin-top: 5px;">scheda</asp:LinkButton>
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
                                            <th id="Th1" runat="server" style="width: 200px">
                                                Accessorio
                                            </th>
                                            <th id="Th3" runat="server">
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                                <div class="page">
                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="30" style="border-right: medium none;">
                                        <Fields>
                                            <asp:NumericPagerField />
                                        </Fields>
                                    </asp:DataPager>
                                </div>
                            </LayoutTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <SelectedItemTemplate>
                                <tr class="current">
                                    <td>
                                        <asp:Label ID="name_datiLabel" runat="server" Text='<%# Eval("title") %>' />
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
                    Scheda
                </h1>
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
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td colspan="2">
                                            <asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
                                                <WhereParameters>
                                                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                                </WhereParameters>
                                            </asp:LinqDataSource>
                                            <asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound" OnClientClick="removeTinyEditor();">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnk_lang" CommandName="change_lang" CssClass="tab_item" runat="server">
                                                        <span>
                                                            <%# Eval("title") %></span>
                                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                    </asp:LinkButton></ItemTemplate><EmptyDataTemplate>
                                                    <span>No data was returned.</span></EmptyDataTemplate><LayoutTemplate>
                                                    <div class="menu2">
                                                        <a id="itemPlaceholder" runat="server" />
                                                    </div>
                                                </LayoutTemplate>
                                            </asp:ListView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Nome in lingua: </td><td>
                                            <asp:TextBox runat="server" ID="txt_title" Width="230px" />
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
                <div class="nulla">
                </div>
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton></div><div class="bottom_salva">
                        <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton></div><div class="nulla">
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
