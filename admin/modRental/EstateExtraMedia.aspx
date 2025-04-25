<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateExtraMedia.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateExtraMedia" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HF_main_folder" Value="romeapartmentsphoto" runat="server" />
    <telerik:RadAjaxPanel ID="pnlDett" runat="server" OnAjaxRequest="pnlDett_AjaxRequest">
        <asp:HiddenField ID="HF_type" Value="romeapartmentsphoto" runat="server" />
        <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
        <asp:HiddenField ID="HF_order" runat="server" />
        <%--<h1 class="titolo_main">
            Multimedia della struttura:
            <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
        </h1>
        <div id="fascia1"> 
            <div style="clear: both; margin: 3px 0 5px 30px;">
                <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
            </div>
        </div>--%>
        <div class="nulla">
        </div>
        <h1 class="titolo_main">
            <asp:Literal runat="server" ID="ltrCurrType" Text="Gallery del Appartamento"></asp:Literal>
        </h1>
       <%-- <asp:HyperLink ID="HL_otherType" runat="server" CssClass="gotopage inlinebtn">Carica le foto delle Vicinanze/HomeAway</asp:HyperLink>--%>
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
                    <div id="Div1" class="boxmodulo" runat="server" visible="false">
                        Dimensioni Foto grande:<asp:TextBox ID="txt_imgBigW" runat="server" Width="30" Text="960"></asp:TextBox>x<asp:TextBox ID="txt_imgBigH" runat="server" Width="30" Text="540"></asp:TextBox>
                        <br />
                        Path Watermark grande:<asp:TextBox ID="txt_imgWatermarkBigPath" runat="server" Width="300" Text="images/css/img_watermark_big.png"></asp:TextBox>
                        <br />
                        Padding Watermark grande x:<asp:TextBox ID="txt_imgWatermarkBigPadX" runat="server" Width="20" Text="10"></asp:TextBox>y:<asp:TextBox ID="txt_imgWatermarkBigPadY" runat="server" Width="20" Text="10"></asp:TextBox>
                        <asp:DropDownList ID="drp_imgWatermarkBigFloat" runat="server">
                            <asp:ListItem Value="Right"></asp:ListItem>
                            <asp:ListItem Value="Left"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <br />
                        Dimensioni Foto piccola:<asp:TextBox ID="txt_imgSmallW" runat="server" Width="30" Text="330"></asp:TextBox>x<asp:TextBox ID="txt_imgSmallH" runat="server" Width="30" Text="186"></asp:TextBox>
                        <br />
                        Path Watermark piccola:<asp:TextBox ID="txt_imgWatermarkSmallPath" runat="server" Width="300" Text="images/css/img_watermark_small.png"></asp:TextBox>
                        <br />
                        Padding Watermark piccola x:<asp:TextBox ID="txt_imgWatermarkSmallPadX" runat="server" Width="20" Text="10"></asp:TextBox>y:<asp:TextBox ID="txt_imgWatermarkSmallPadY" runat="server" Width="20" Text="10"></asp:TextBox>
                        <asp:DropDownList ID="drp_imgWatermarkSmallFloat" runat="server">
                            <asp:ListItem Value="Right"></asp:ListItem>
                            <asp:ListItem Value="Left"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="boxmodulo">
                        <table border="0" cellspacing="3" cellpadding="0">
                            <tr>
                                <td>
                                    <span class="titoloboxmodulo">Cartella di multimedia </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_media_folder" ReadOnly="true" runat="server" Width="300" MaxLength="500"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="drag-drop-upload">
                                        <div class="browserError">
                                        </div>
                                        <telerik:RadAsyncUpload runat="server" ID="dragDropUpload" MultipleFileSelection="Automatic" InputSize="30" OnFileUploaded="dragDropUpload_FileUploaded" OnClientFilesUploaded="dragDropUpload_fileUploaded" AllowedFileExtensions="jpeg,jpg,gif,png" />
                                    </div>
                                    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                        <script type="text/javascript">
                                            function pageLoad() {
                                                if (!Telerik.Web.UI.RadAsyncUpload.Modules.FileApi.isAvailable()) {
                                                    $("#drag-drop-upload").addClass("nodrag");
                                                    $telerik.$("#drag-drop-upload .browserError").html("<strong>il tuo browser non supporta Drag and Drop. Ti preghiamo di aggiornare a FireFox 4.0+, Chrome, Safari 5.0+.</strong>");
                                                }
                                                else {
                                                    $telerik.$(document).bind({ "drop": function (e) {
                                                        e.preventDefault();
                                                    }
                                                    });
                                                    $telerik.$(document).bind({ "dragstart": function (e) {
                                                        e.preventDefault();
                                                        return false;
                                                    }
                                                    });

                                                    var dropZone1 = $telerik.$(document).find("#drag-drop-upload ul.ruInputs li:last-child .ruDropZone");
                                                    dropZone1.bind({ "dragenter": function (e) {
                                                        dragEnterHandler(e, dropZone1);
                                                    }
                                                    })
                                                    .bind({ "dragleave": function (e) {
                                                        dragLeaveHandler(e, dropZone1);
                                                    }
                                                    })
                                                    .bind({ "drop": function (e) {
                                                        dropHandler(e, dropZone1);
                                                    }
                                                    });

                                                }
                                            }

                                            function dropHandler(e, dropZone) {
                                                //dropZone[0].style.backgroundColor = "#357A2B";
                                            }

                                            function dragEnterHandler(e, dropZone) {
                                                var dt = e.originalEvent.dataTransfer;
                                                var isFile = (dt.types != null && (dt.types.indexOf ? dt.types.indexOf('Files') != -1 : dt.types.contains('application/x-moz-file')));
                                                if (isFile || $telerik.isSafari5 || $telerik.isIE10Mode || $telerik.isOpera) {
                                                }
                                                else {
                                                    e.preventDefault();
                                                    e.returnValue = false;
                                                    //alert("no");
                                                }
                                                //dropZone[0].style.backgroundColor = "#000000";
                                            }

                                            function dragLeaveHandler(e, dropZone) {
                                                //if (!$telerik.isMouseOverElement(dropZone[0], e.originalEvent))
                                                //dropZone[0].style.backgroundColor = "#357A2B";
                                            }

                                        </script>
                                    </telerik:RadScriptBlock>
                                </td>
                            </tr>
                            <asp:ListView ID="LV_gallery" runat="server">
                                <ItemTemplate>
                                    <li id="photoitem_<%# Eval("id") %>" imgbig="v" code="<%# Eval("code") %>" refid="<%# Eval("id") %>">
                                        <img alt="" src="/<%# Eval("img_thumb") %>" class="img" />
                                        <img src="images/aptphotos-current.png" alt="" class="first" />
                                        <a href="#" onclick="OpenShadowbox('EstateExtraMediaDett.aspx?id=<%# Eval("id") %>');

                                           return false;" class="hover-state"><span>Vedi</span> </a>
                                        <%# Eval("code") + "" == "" ? "<span class='new'>new</span>" : ""%>
                                    </li>
                                    <asp:Label ID="Label1" Visible="false" runat="server" Text='<%# Eval("id") %>' />
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

                                                                return false;" id="hl_enableSortable">Riordina</a>
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

