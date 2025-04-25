<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlAtraveo_media.aspx.cs" Inherits="ModRental.admin.modRental.EstateChnlAtraveo_media" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucEstateChnlAtraveoTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dragging .common {
            display: none !important;
        }
        ul.photos li.smallimage .img {
            border: 2px solid #FF0000 !important;
        }
        .tabsTop.tabsChannelsTop table td a[title="Images"], #tabsHomeaway.tabsTop table td a[title="Images"]  {
	        background:#848484;
	        border-color:#606060;
	        color:#FFF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server" OnAjaxRequest="pnlDett_AjaxRequest">
        <asp:HiddenField ID="HF_type" Value="chnlAtraveo" runat="server" />
        <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
        <asp:HiddenField ID="HF_order" runat="server" />
        <img src="/images/css/atraveo-integrated-magarental.png" class="homeAwayLogo" alt="Integrated with Atraveo" />
        <h1 class="titolo_main">
            Multimedia della struttura su Atraveo:
            <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
        </h1>
        <div id="fascia1" style="margin-bottom: 10px;">
            <div class="tabsTop tabsChannelsTop tabsHomeAwayTop" id="tabsHomeaway">
                <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
            </div>
        </div>
        <div class="nulla">
        </div>
        
        <div class="mainline mainChannels mainAtraveoLettings mainMediaHL">
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <div class="boxmodulo">
                        <table border="0" cellspacing="3" cellpadding="0" style="width: 100%;">
                            <asp:ListView ID="LV_gallery" runat="server" OnItemCommand="LV_gallery_ItemCommand">
                                <ItemTemplate>
                                    <li id='photoitem_<%# Eval("id") %>' imgbig="v" code="<%# Eval("code") %>" <%# Eval("type")+""== CurrType+""?" refid='"+Eval("id")+"'":" class='common'" %>>
                                        <img alt="" src="<%# (""+Eval("img_thumb")).StartsWith("http")?""+Eval("img_thumb"):"/"+Eval("img_thumb") +"?resize=true&w=300&h=300"%>" class="img" />
                                        <img src="images/aptphotos-current.png" alt="" class="first" />
                                        <div class="hover-state">
                                            <asp:LinkButton ID="lnkEdit" runat="server"><span>Modifica</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkAdd" CommandName="additem" runat="server" class="select"><span>Seleziona</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDel" CommandName="deleteitem" runat="server" class="remove"><span>Rimuovi</span></asp:LinkButton>
                                        </div>
                                        <%# Eval("type")+""== CurrType+"" ? "<span class='selected'>√</span>" : ""%>
                                        <%# Eval("code") + "" == "" ? "<span class='new'>new</span>" : ""%>
                                    </li>
                                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Non hai ancora aggiunto nessuna foto</span>
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Foto attuali
                                                <a href="#" onclick="enableSortable();

                                                                return false;"
                                                    id="hl_enableSortable">Riordina</a>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkSelectAll" OnClick="lnkSelectAll_Click" runat="server">Seleziona tutte</asp:LinkButton>
                                            <asp:LinkButton ID="lnkRemoveAll" OnClick="lnkRemoveAll_Click" runat="server">Rimuovi tutte</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ul id="estateMediaSortable" class="photos">
                                                <li id="itemPlaceholder" runat="server" />
                                            </ul>
                                            <div class="nulla">
                                            </div>
                                        </td>
                                    </tr>
                                </LayoutTemplate>
                            </asp:ListView>
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
        </div>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function closeDett() {
                    Shadowbox.close();
                    $find('<%=pnlDett.ClientID %>').ajaxRequest('');
                }
                function setSortable() {
                    $("#estateMediaSortable").sortable({ placeholder: "ui-state-highlight", scrollSensitivity: 40, scrollSpeed: 60, disabled: true });
                    $("#estateMediaSortable").disableSelection();
                }
                function enableSortable() {
                    var action = $("#hl_enableSortable").hasClass("enabled") ? "disable" : "enable";
                    if (action == "enable") {
                        $("#estateMediaSortable").sortable("enable");
                        $("#hl_enableSortable").addClass("enabled");
                        $("#hl_enableSortable").html("Salva");
                        $("#estateMediaSortable").addClass("dragging");
                    }
                    else {
                        $("#estateMediaSortable").sortable("disable");
                        $("#hl_enableSortable").removeClass("enabled");
                        $("#hl_enableSortable").html("Riordina");
                        $("#estateMediaSortable").removeClass("dragging");
                        saveSelection();
                        $find('<%=pnlDett.ClientID %>').ajaxRequest('sortable');
                    }
                }
                $(function () {
                    setSortable();
                });
                function saveSelection() {
                    try {
                        var list = "";
                        var sep = "";
                        $("#estateMediaSortable li").each(function () {
                            list += "" + sep + $(this).attr("refid");
                            sep = "|";
                        });
                        $("#<%=HF_order.ClientID %>").val(list);
                        return true;
                    }
                    catch (ex) {
                        alert(ex);
                        return false;
                    }
                }
            </script>
            <script type="text/javascript">
                function dragDropUpload_fileUploaded() {
                    $find('<%=pnlDett.ClientID %>').ajaxRequest('');
                    $(".invalid").html("");
                }
                function dragDropUpload_validationFailed(sender, args) {
                    $(".invalid").html("Estensione Non valida, seleziona un file con est. jpeg,jpg,gif,png");
                }
            </script>
        </telerik:RadCodeBlock>
    </telerik:RadAjaxPanel>
</asp:Content>
