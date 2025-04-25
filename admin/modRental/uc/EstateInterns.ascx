<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EstateInterns.ascx.cs" Inherits="MagaRentalCE.admin.modRental.ucEstateDett.EstateInterns" %>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
        <asp:HiddenField ID="HF_CurrId" Value="" runat="server" />
        <asp:HiddenField ID="HF_Type" Value="" runat="server" />


        <asp:Label ID="lblInternType" runat="server" Visible="false"></asp:Label>

        <%-- <div id="divInternsMain" style="clear: both">--%>
        <asp:ListView ID="Lv" runat="server" OnItemCommand="LvItemCommand">
            <ItemTemplate>
                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                <div class="mainbox" style="margin: 10px; clear: both">
                    <h3 class="titoloboxmodulo"><%= lblInternType.Text%>
                        #<asp:Literal ID="ltrNumber" runat="server"></asp:Literal>
                    </h3>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpType" runat="server" Style="height: 25px;" Width="100px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <asp:ListView ID="LvFeatures" runat="server">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_featureid" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                    <tr>
                                        <td class="td_title"><%# Eval("title") %>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_featuresCount" runat="server" Width="50px">
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="chk_featuresSelected" runat="server" Text='' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                            <tr>
                                <td colspan="2" class="">
                                    <div class="boxBooking" style="width: 100%;">
                                        <ul class="tabs tabIntersLang">
                                            <asp:ListView ID="LvLangTab" runat="server">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                                    <li><a href="#"><%# Eval("title") %></a></li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ul>
                                        <div class="panes">
                                            <asp:ListView ID="LvLangPane" runat="server">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                                    <div class="paneCliente">
                                                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                                            <tr>
                                                                <td class="td_title">name:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txt_title" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">note:<br />
                                                                    <asp:TextBox runat="server" ID="txt_description" TextMode="MultiLine" Height="100" Width="400" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>

                                        <asp:LinkButton ID="lnkDel" runat="server" CssClass="inlinebtn" CommandName="del" OnClientClick="return confirm('Do You want to delete?')">Delete</asp:LinkButton>

                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ItemTemplate>
            <EmptyDataTemplate>
            </EmptyDataTemplate>
        </asp:ListView>

        <div style="clear: both; margin-left: 20px" runat="server" id="divSaveAll">
            <div class="salvataggio">
                <div class="bottom_salva">
                    <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" ID="lnk_cancle" OnClick="lnkCancel_Click"><span>  Annulla Modifiche </span> </asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnk_save_all" OnClick="lnk_save_all_Click"><span> Salva Modifiche </span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

        <div class="mainbox" style="margin: 10px; clear: both">
            <h3 class="titoloboxmodulo">Add</h3>
            <div class="boxmodulo">
                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                    <tr>
                        <td class="td_title">type:
                        </td>
                        <td>
                            <asp:DropDownList ID="drpSubType" runat="server" Style="height: 25px;" Width="100px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <asp:ListView ID="LvFeatures" runat="server">
                        <ItemTemplate>
                            <asp:Label ID="lbl_featureid" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                            <tr>
                                <td class="td_title"><%# Eval("title") %>:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_featuresCount" runat="server" Width="50px">
                                    </asp:DropDownList>
                                    <asp:CheckBox ID="chk_featuresSelected" runat="server" Text='' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                    <tr>
                        <td colspan="2">
                            <div class="boxBooking" style="width: 100%;">
                                <ul class="tabs tabIntersLang">
                                    <asp:ListView ID="LvLangTab" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                            <li><a href="#"><%# Eval("title") %></a></li>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </ul>
                                <div class="panes">
                                    <asp:ListView ID="LvLangPane" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                            <div class="paneCliente">
                                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                                    <tr>
                                                        <td class="td_title">name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txt_title" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">note:<br />
                                                            <asp:TextBox runat="server" ID="txt_description" TextMode="MultiLine" Height="100" Width="400" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>

                                <div class="nulla">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="inlinebtn" OnClick="lnkSave_Click">Save</asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="inlinebtn" OnClick="lnkCancel_Click">Cancel</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>


<script type="text/javascript">
    function setInternsLangTabs() {

        $("ul.tabs.tabIntersLang").tabs("div.panes > div");
    }

    $(document).ready(function () {
        setInternsLangTabs();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setInternsLangTabs);
    });
</script>
