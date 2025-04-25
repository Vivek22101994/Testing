<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="BcomExport_Excel.aspx.cs" Inherits="RentalInRome.admin.modRental.BcomExport_Excel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pannello_fascia1">
        <div style="clear: both">
            <h1>Export Booking.com Properties </h1>
            <div class="nulla">
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="filt" class="filt">
                        <div class="t">
                            <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                            <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
                        </div>
                        <div class="c">
                            <a class="filto_bottone" style="display: none;" id="lnk_showfilt" onclick="showFilter();">FILTRA</a>
                            <a class="filto_bottone2" id="lnk_hidefilt" onclick="hideFilter();">FILTRA</a>
                            <div class="filtro_cont">
                                <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Nome:</label>
                                                        <asp:TextBox ID="txt_code" CssClass="inp aptComplete" runat="server"></asp:TextBox>
                                                    </td>

                                                    <td>
                                                        <label>
                                                            Zona</label>
                                                        <asp:DropDownList runat="server" ID="drp_zone" Width="180px" CssClass="inp" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <!-- Table seconda riga -->
                                        </td>
                                        <td valign="bottom">
                                            <asp:LinkButton runat="server" ID="lnk_filter" OnClientClick="resetEstate();" OnClick="lnk_filter_Click" CssClass="ricercaris"> <span> Filtra Risultati </span> </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="b">
                            <img src="images/filt_b1.gif" width="6" height="6" style="float: left" />
                            <img src="images/filt_b2.gif" width="6" height="6" style="float: right" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="salvataggio saveAgencyDetails">
            <div class="bottom_salva">
                <asp:Literal ID="lt_exportError" runat="server" Visible="false"> </asp:Literal>
            </div>
            <div class="bottom_salva" style="float: right">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" OnClientClick="return validate()"><span> Export to Excel </span></asp:LinkButton>
            </div>
            <div class="nulla">
            </div>
        </div>
        <asp:HiddenField ID="HF_Estate" runat="server" />
        <asp:Literal runat="server" ID="ltr_items" Visible="false"></asp:Literal>
        <div style="clear: both">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                <ContentTemplate>

                    <asp:Button ID="btn_page_update" runat="server" Text="Button" Style="display: none;" />
                    <div id="divList" runat="server">
                        <div class="page">
                            <asp:Label ID="lbl_record_count_top" runat="server" CssClass="total" Text=""></asp:Label>
                            <div class="nulla">
                            </div>
                        </div>
                        <asp:ListView ID="LV" runat="server" OnDataBound="LV_DataBound">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("id") %></span>
                                    </td>
                                    <td>
                                        <span style="margin-left: 20px">
                                            <%# Eval("code")%></span>
                                    </td>
                                    <td>
                                        <span style="margin-left: 20px">
                                            <%# Eval("bcomName")%></span>
                                    </td>

                                    <td>
                                        <span class="">
                                            <%# AdminUtilities.getZoneTitle(Eval("pid_zone").objToInt32())%></span>
                                    </td>
                                    <td>
                                        <a target="_blank" href="/admin/rnt_estate_interns.aspx?id=<%# Eval("id")%>">Interns Detail </a>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Text='<%# Eval("id") %>' runat="server" Visible="false"></asp:Label>
                                        <asp:CheckBox ID="chkExport" CssClass="chkexport" runat="server" />
                                    </td>

                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# Eval("id") %></span>
                                    </td>
                                    <td>
                                        <span style="margin-left: 20px">
                                            <%# Eval("code")%></span>
                                    </td>
                                    <td>
                                        <span style="margin-left: 20px">
                                            <%# Eval("bcomName")%></span>
                                    </td>
                                    <td>
                                        <span class="">
                                            <%# AdminUtilities.getZoneTitle(Eval("pid_zone").objToInt32())%></span>
                                    </td>
                                    <td>
                                        <a target="_blank" href="/admin/rnt_estate_interns.aspx?id=<%# Eval("id")%>">Interns Detail </a>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_id" Text='<%# Eval("id") %>' runat="server" Visible="false"></asp:Label>
                                        <asp:CheckBox ID="chkExport" CssClass="chkexport" runat="server" />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table id="Table1" runat="server" style="">
                                    <tr>
                                        <td>No data was returned.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <InsertItemTemplate>
                            </InsertItemTemplate>
                            <LayoutTemplate>
                                <div class="table_fascia">
                                    <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                                        <tr id="Tr1" runat="server" style="">
                                            <th>ID
                                            </th>
                                            <th id="Th1" runat="server" style="width: 350px">
                                                <%-- <asp:LinkButton runat="server" CssClass="lnk_view" Text="Nome &#9660;" ID="lnk_code" OnClick="lnk_code_Click"></asp:LinkButton>--%>
                                                    Property Name 
                                            </th>
                                            <th id="Th3" runat="server" style="width: 350px">
                                                <%-- <asp:LinkButton runat="server" CssClass="lnk_view" Text="Nome &#9660;" ID="lnk_code" OnClick="lnk_code_Click"></asp:LinkButton>--%>
                                                    Booking.com Name 
                                            </th>
                                            <th id="Th2" runat="server" style="width: 300px">Zone
                                            </th>
                                            <th style="width: 200px"></th>
                                            <th style="width: 200px">Export to Excel 
                                                  <input id="SelectAll" class="chkexportall" type="checkbox" />
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>

                            </LayoutTemplate>
                        </asp:ListView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_page_update" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <script type="text/javascript">
        var items = [
			<%=ltr_items.Text %>
        ];
        function setAutocomplete() {
            $(".aptComplete").autocomplete({
                source: items,
                select: function (event, ui) {
                    $("#<%= HF_Estate.ClientID %>").val(ui.item.idEstate);
                }
            });
            }
            function bindCheck() {
                $('#SelectAll').change(function () {
                    $("[id$='chkExport']").attr('checked', this.checked);
                });
            }

            function resetEstate() {
                console.log($("#<%= txt_code.Text%>").val());
                if ($("#<%= txt_code.Text%>").val() == "")
                    $("#<%= HF_Estate.ClientID %>").val("");
            }

            function validate() {
                console.log($("[type='checkbox']:checked").length);

                if ($("[type='checkbox']:checked").length > 0) {
                    return true;
                }
                else {
                    alert('Please  first choose the property to export ');
                    return false;
                }

            }
    </script>
</asp:Content>
