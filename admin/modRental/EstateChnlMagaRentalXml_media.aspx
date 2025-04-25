<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlMagaRentalXml_media.aspx.cs" Inherits="ModRental.admin.modRental.EstateChnlMagaRentalXml_media" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dragging .common {
            display: none !important;
        }

        ul.photos li.smallimage .img {
            border: 2px solid #FF0000 !important;
        }

        .tabsTop.tabsChannelsTop table td a[title="Images"], #tabsHomeaway.tabsTop table td a[title="Images"] {
            background: #848484;
            border-color: #606060;
            color: #FFF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        #tabsHomeaway.tabsTop table td a, #tabsHomeaway.tabsTop table td a:visited {
            display: block;
            float: left;
            margin: 10px 0 0 10px;
            padding: 7px;
            width: auto;
        }

        div.salvataggio {
            float: left;
        }

            div.salvataggio.btnCopia {
                float: right;
                width: auto;
            }

        .homeAwayLogo.RUlogo {
            background: #fff none repeat scroll 0 0;
            padding: 10px;
        }
    </style>
    <telerik:RadAjaxPanel ID="pnlDett" runat="server" OnAjaxRequest="pnlDett_AjaxRequest">
        <asp:HiddenField ID="HF_type" Value="MagaRentalXml" runat="server" />
        <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
        <asp:HiddenField ID="HF_order" runat="server" />
        <%# rntUtils.getAgent_logoForDetailsPage(ChnlMagaRentalXmlProps.IdAdMedia) %>
        <h1 class="titolo_main">Multimedia della struttura su MagaRentalXml:
            <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
        </h1>
        <div id="fascia1">
            <div class="tabsTop tabsChannelsTop tabsHomeAwayTop" id="tabsHomeaway">
                <table class="details_tabs" cellpadding="0" cellspacing="0" border="0" style="display: table;">
                    <tr>
                        <td style="border-bottom: 0; width: 65%;">
                            <a href='/admin/modRental/EstateChnlMagaRentalXml_main.aspx?id=<%# IdEstate %>' class="scheda" title="Unit"><%# contUtils.getLabel("Details")%></a>
                            <a href='/admin/modRental/EstateChnlMagaRentalXml_media.aspx?id=<%# IdEstate %>' class="media" title="Images"><%# contUtils.getLabel("Images")%></a>
                            <a href='/admin/rnt_estate_details.aspx?id=<%#IdEstate %>' class="scheda" title="Torna alla Struttura">Back to property</a>
                        </td>

                        <td style="border-bottom: 0; width: 35%;" align="right">
                            <a style="float: right;" href='/admin/modRental/EstateChnlMagaRentalXml_main.aspx?id=<%# IdEstate %>&updatefrommagarental=true' class="scheda" title="Copia da MagaRental">Copy from MagaRental</a>

                        </td>

                    </tr>
                </table>
            </div>
        </div>
        <div class="nulla">
        </div>

        <div class="mainline mainChannels mainMagaRentalXmlLettings mainMediaHL">
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
                                            <asp:LinkButton ID="lnkEdit" runat="server"><span>Edit</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkAdd" CommandName="additem" runat="server" class="select"><span>Select</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDel" CommandName="deleteitem" runat="server" class="remove"><span>Remove</span></asp:LinkButton>
                                        </div>
                                        <%# Eval("type")+""== CurrType+"" ? "<span class='selected'>√</span>" : ""%>
                                        <%# Eval("code") + "" == "" ? "<span class='new'>new</span>" : ""%>
                                    </li>
                                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">No photos added yet</span>
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Actual photos
                                                <a href="#" onclick="enableSortable();

                                                                return false;"
                                                    id="hl_enableSortable">Order</a>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkSelectAll" OnClick="lnkSelectAll_Click" runat="server">Select all</asp:LinkButton>
                                            <asp:LinkButton ID="lnkRemoveAll" OnClick="lnkRemoveAll_Click" runat="server">Remove all</asp:LinkButton>
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
                    $find('<%#pnlDett.ClientID %>').ajaxRequest('');
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
                        $("#hl_enableSortable").html("Save");
                        $("#estateMediaSortable").addClass("dragging");
                    }
                    else {
                        $("#estateMediaSortable").sortable("disable");
                        $("#hl_enableSortable").removeClass("enabled");
                        $("#hl_enableSortable").html("Riordina");
                        $("#estateMediaSortable").removeClass("dragging");
                        saveSelection();
                        $find('<%#pnlDett.ClientID %>').ajaxRequest('sortable');
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
                        $("#<%#HF_order.ClientID %>").val(list);
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
                    $find('<%#pnlDett.ClientID %>').ajaxRequest('');
                    $(".invalid").html("");
                }
                function dragDropUpload_validationFailed(sender, args) {
                    $(".invalid").html("Estensione Non valida, seleziona un file con est. jpeg,jpg,gif,png");
                }
            </script>
        </telerik:RadCodeBlock>
    </telerik:RadAjaxPanel>
</asp:Content>
