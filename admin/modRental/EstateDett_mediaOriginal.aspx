<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateDett_mediaOriginal.aspx.cs" Inherits="ModRental.admin.modRental.EstateDett_mediaOriginal" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dragging .common {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HF_main_folder" Value="originalphotos" runat="server" />
    <telerik:RadAjaxPanel ID="pnlDett" runat="server" OnAjaxRequest="pnlDett_AjaxRequest">
        <asp:HiddenField ID="HF_type" Value="romeapartmentsphoto" runat="server" />
        <asp:HiddenField ID="HF_mediaCommonType" Value="" runat="server" />
        <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
        <asp:HiddenField ID="HF_order" runat="server" />
        <h1 class="titolo_main">Foto originali della struttura:
            <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
        </h1>
        <div id="fascia1">
            <div style="clear: both; margin: 3px 0 5px 30px;">
                <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
            </div>
        </div>
        <div class="nulla">
        </div>
        <asp:ListView ID="LV_otherType" runat="server">
            <ItemTemplate>
                <asp:HyperLink ID="HL" runat="server" CssClass="gotopage inlinebtn"></asp:HyperLink>
                <asp:Label ID="lblType" Visible="false" runat="server" Text='<%# Eval("type") %>' />
            </ItemTemplate>
            <EmptyDataTemplate>
            </EmptyDataTemplate>
        </asp:ListView>
        <div class="nulla">
        </div>
        <div class="mainline">
            <!-- BOX 1 -->
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <div class="boxmodulo">
                        <table border="0" cellspacing="3" cellpadding="0" id="pnlUpload" runat="server">
                            <tr>
                                <td>

                                    <div id="pnlFolderEdit" runat="server">
                                        <span class="labelSx">Seleziona la cartella </span>
                                        <asp:DropDownList ID="drp_folderList" runat="server"></asp:DropDownList>
                                        <asp:LinkButton ID="lnk_rate_save" runat="server" CssClass="inlinebtn" OnClick="lnk_saveFolder_Click">Salva</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="inlinebtn" OnClick="lnk_cancelFolder_Click">Annulla</asp:LinkButton>
                                    </div>
                                    <div id="pnlFolderView" runat="server">
                                        <span class="labelSx">Cartella per caricare immagini </span>
                                        <asp:TextBox ID="txt_mediaFolderOriginalPhotos" ReadOnly="true" runat="server" Style="float: left; margin-right: 0;" Width="553" MaxLength="500"></asp:TextBox>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="inlinebtn" OnClick="lnk_changeFolder_Click">Cambia cartella</asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellspacing="3" cellpadding="0" style="width: 100%;">
                            <asp:ListView ID="LV_gallery" runat="server" OnItemCommand="LV_gallery_ItemCommand">
                                <ItemTemplate>
                                    <li id='photoitem_<%# Eval("id") %>' imgbig="v" code="<%# Eval("code") %>" <%# Eval("type")+""== CurrType+""?" refid='"+Eval("id")+"'":" class='common'" %>>
                                        <img alt="" src="<%# (""+Eval("img_thumb")).StartsWith("http")?""+Eval("img_thumb"):"/"+Eval("img_thumb") %>?resize=true&w=300&h=300" class="img" />
                                        <img src="images/aptphotos-current.png" alt="" class="first" />
                                        <div class="hover-state">
                                            <asp:LinkButton ID="lnkDel" CommandName="deleteitem" runat="server" class="remove"><span>Rimuovi</span></asp:LinkButton>
                                        </div>
                                        <span class='selected'>√</span>
                                    </li>
                                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                    <asp:Label ID="lbl_path" Visible="false" runat="server" Text='<%# Eval("img_banner") %>' />
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Non hai ancora selezionato nessuna foto</span>
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Foto selezionate
                                                <asp:LinkButton ID="lnkRemoveAll" OnClick="lnkRemoveAll_Click" runat="server">Rimuovi tutte</asp:LinkButton>
                                                <a href="#" onclick="enableSortable();

                                                                return false;"
                                                    id="hl_enableSortable">Riordina</a>
                                            </span>
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
                            <asp:ListView ID="LvFolders" runat="server" OnItemCommand="LvFolders_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo"><%# Eval("code") %>
                                                <asp:LinkButton ID="lnkSelectAll" CommandName="SelectAll" runat="server">Seleziona tutte</asp:LinkButton>
                                            </span>
                                        </td>
                                    </tr>
                                    <asp:ListView ID="LvGallery" runat="server" OnItemCommand="LV_gallery_ItemCommand">
                                        <ItemTemplate>
                                            <li id='photoitem_<%# Eval("id") %>' imgbig="v" code="<%# Eval("code") %>" <%# Eval("type")+""== CurrType+""?" refid='"+Eval("id")+"'":" class='common'" %>>
                                                <img alt="" src="<%# (""+Eval("img_thumb")).StartsWith("http")?""+Eval("img_thumb"):"/"+Eval("img_thumb") %>?resize=true&w=300&h=300" class="img" />
                                                <img src="images/aptphotos-current.png" alt="" class="first" />
                                                <div class="hover-state">
                                                    <asp:LinkButton ID="lnkAdd" CommandName="additem" runat="server" class="select"><span>Seleziona</span></asp:LinkButton>
                                                </div>
                                            </li>
                                            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                            <asp:Label ID="lbl_path" Visible="false" runat="server" Text='<%# Eval("img_banner") %>' />
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td>
                                                    <span class="titoloboxmodulo">Non ci sono foto da selezionare</span>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <tr>
                                                <td>
                                                    <ul class="photos">
                                                        <li id="itemPlaceholder" runat="server" />
                                                    </ul>
                                                    <div class="nulla">
                                                    </div>
                                                </td>
                                            </tr>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                    <asp:Label ID="lbl_type" Visible="false" runat="server" Text='<%# Eval("type") %>' />
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Non hai ancora caricato nessuna foto</span>
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
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
        </telerik:RadCodeBlock>
    </telerik:RadAjaxPanel>
</asp:Content>
