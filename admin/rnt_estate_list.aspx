<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_list.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_list" %>

<%@ Register Src="uc/UC_rnt_estate_list.ascx" TagName="UC_rnt_estate_list" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_loader_list.ascx" TagName="UC_loader_list" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
		function showFilter() {
			$("#lnk_showfilt").css("display","none");
			$("#lnk_hidefilt").css("display","");
			$("#tbl_filter").css("display","");
			$("#<%=HF_is_filtered.ClientID %>").val("true");
		}
		function hideFilter() {
			$("#lnk_showfilt").css("display","");
			$("#lnk_hidefilt").css("display","none");
			$("#tbl_filter").css("display","none");
			$("#<%=HF_is_filtered.ClientID %>").val("false");
		}
		function showNew() {
		    $("#lnk_showNew").css("display","none");
		    $("#lnk_hideNew").css("display","");
		    $("#tbl_new").css("display","");
		}
		function hideNew() {
		    $("#lnk_showNew").css("display","");
		    $("#lnk_hideNew").css("display","none");
		    $("#tbl_new").css("display","none");
		}
        function checkNewSubmit() {
            var _error = "";
             var _value = "";
		    _value = $("#<%= txt_new_code.ClientID%>").val();
		    if (_value == "" || _value == "0" )
		        _error += "\n--Inserire il Nome della struttura.";
		    _value = $("#<%= drp_new_owner.ClientID%>").val();
		    if (_value == "" || _value == "0" )
		        _error += "\n--Selezionare il Proprietario della struttura.";
		    _value = $("#<%= drp_new_city.ClientID%>").val();
		    if (_value == "" || _value == "0" )
		        _error += "\n--Selezionare la città della struttura.";
		    _value = $("#<%= drp_new_zone.ClientID%>").val();
		    if (_value == "" || _value == "0" )
		        _error += "\n--Selezionare la zona della struttura.";
            if(_error!=""){
                alert("Per creare una nuova struttura mancano seguenti dati obbligatori..."+_error);
                return false;
            }
            return true;
		}
		function Filter(exportExcel) {
		    var _filter = "";
		    var _value = "";
		    _value = $("#<%= drp_category.ClientID%>").val();
		    if (_value != "")
		        _filter += "&cat=" + _value;
		    _value = $("#<%= drp_is_active.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_active=" + _value;
            _value = $("#<%= drp_homeAway.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_HomeAway=" + _value;
		    _value = $("#<%= drp_is_exclusive.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_exclusive=" + _value;
		    _value = $("#<%= drp_is_srs.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_srs=" + _value;
		    _value = $("#<%= drp_is_ecopulizie.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_ecopulizie=" + _value;
		    _value = $("#<%= drp_is_online_booking.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_online_booking=" + _value;
		    _value = $("#<%= drp_pr_has_overnight_tax.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&overnight_tax=" + _value; 
		    _value = $("#<%= drp_isContratto.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&isContratto=" + _value;
		    _value = $("#<%= txt_code.ClientID%>").val();
		    if (_value != "")
		        _filter += "&tith=" + _value;
		    _value = $("#<%= txt_address.ClientID%>").val();
		    if (_value != "")
		        _filter += "&addr=" + _value;
		    
		    _value = $("#<%= drp_city.ClientID%>").val();
		    if (_value != "0")
		        _filter += "&idc=" + _value;
		    _value = $("#<%= drp_zone.ClientID%>").val();
		    if (_value != "0")
		        _filter += "&idz=" + _value;
		    _value = $("#<%= drp_owner.ClientID%>").val();
		    if (_value != "0")
		        _filter += "&ido=" + _value;
		    _value = $("#<%= drp_bcomStatus.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&bcom=" + _value;
		    if (exportExcel) {
		        _filter += "&exportExcel=" + exportExcel;
		        _filter += "&exportExcelDisc=" + $("#<%= txt_exportExcelDisc.ClientID%>").val();
            }
		    
            window.location = "?flt=true" + _filter;
		}
		function filterEnterPress(e) {
			var evt = e ? e : window.event;
			if (evt.keyCode == 13) {
				Filter();
				return false;
			}
		}
        var items = [
			<%=ltr_items.Text %>
		];
        var itemsAddr = [
			<%=ltr_itemsAddr.Text %>
        ];

		
		function setAutocomplete(){
			$( ".aptComplete" ).autocomplete({
				source: items,
			    select: function( event, ui ) {
                    window.location = "rnt_estate_details.aspx?id="+ui.item.idEstate;
			    }
			});
			$(".addrComplete").autocomplete({
			    source: itemsAddr,
			    select: function (event, ui) {
			        window.location = "rnt_estate_details.aspx?id=" + ui.item.idEstate;
			    }
			});
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal ID="ltr_items" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_itemsAddr" runat="server" Visible="false"></asp:Literal>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:HiddenField ID="HF_is_filtered" runat="server" />
    <asp:HiddenField ID="HF_url_filter" runat="server" />
    <div id="fascia1">
        <div class="pannello_fascia1">
            <div style="clear: both">
                <h1>
                    Elenco Appartamenti</h1>
                <div class="nulla">
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="filt">
                            <div class="t">
                                <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
                            </div>
                            <div class="c">
                                <a class="filto_bottone" id="lnk_showNew" onclick="showNew();">Crea nuovo</a>
                                <a class="filto_bottone2" style="display: none;" id="lnk_hideNew" onclick="hideNew();">Nascondi</a>
                                <div class="filtro_cont">
                                    <table border="0" cellpadding="0" cellspacing="0" id="tbl_new" style="display: none;">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <label>
                                                                        Nome:</label>
                                                                    <asp:TextBox ID="txt_new_code" runat="server" CssClass="inp"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <label>
                                                                        Proprietario</label>
                                                                    <asp:DropDownList runat="server" ID="drp_new_owner" Width="120px" CssClass="inp" />
                                                                </td>
                                                                <td>
                                                                    <label>
                                                                        Città</label>
                                                                    <asp:DropDownList runat="server" ID="drp_new_city" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="drp_new_city_SelectedIndexChanged" CssClass="inp" />
                                                                </td>
                                                                <td>
                                                                    <label>
                                                                        Zona</label>
                                                                    <asp:DropDownList runat="server" ID="drp_new_zone" Width="180px" CssClass="inp" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <asp:LinkButton ID="lnk_createNew" CssClass="ricercaris" runat="server" OnClick="lnk_createNew_Click" OnClientClick="return checkNewSubmit();"><span>Crea Nuovo</span></asp:LinkButton>
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
                <div class="nulla">
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="filt">
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
                                                            <asp:TextBox ID="txt_code" runat="server" CssClass="inp aptComplete" onkeypress="return filterEnterPress(event)"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Indirizzo:</label>
                                                            <asp:TextBox ID="txt_address" runat="server" CssClass="inp addrComplete" onkeypress="return filterEnterPress(event)"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Tipo</label>
                                                            <asp:DropDownList runat="server" ID="drp_category" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="Appartamento" Value="apt"></asp:ListItem>
                                                                <asp:ListItem Text="Villa" Value="villa"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Proprietario</label>
                                                            <asp:DropDownList runat="server" ID="drp_owner" Width="120px" CssClass="inp" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Città</label>
                                                            <asp:DropDownList runat="server" ID="drp_city" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="drp_city_SelectedIndexChanged" CssClass="inp" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Zona</label>
                                                            <asp:DropDownList runat="server" ID="drp_zone" Width="180px" CssClass="inp" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Attivo</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_active" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Esclusiva</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_exclusive" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                SRS</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_srs" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Ecopulizie</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_ecopulizie" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Online B.</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_online_booking" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Tassa di s.</label>
                                                            <asp:DropDownList runat="server" ID="drp_pr_has_overnight_tax" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Contratto</label>
                                                            <asp:DropDownList runat="server" ID="drp_isContratto" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                HA Attivo</label>
                                                            <asp:DropDownList runat="server" ID="drp_homeAway" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>Booking.com</label>
                                                            <asp:DropDownList runat="server" ID="drp_bcomStatus" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="Pubblicato" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="Attivo" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Disattivato" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <a href="javascript: Filter()" class="ricercaris"><span>Filtra Risultati</span></a>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <a class="filto_bottone" >Esporta prezzi in excel</a>
                                <div class="filtro_cont">
                                    <table border="0" cellpadding="0" cellspacing="0" id="Table1">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <label>Sconto:</label>
                                                            <asp:TextBox ID="txt_exportExcelDisc" runat="server" CssClass="inp" Width="30"></asp:TextBox>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <a href="javascript: Filter('pricelist')" class="ricercaris"><span>Esporta</span></a>
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
            <div style="clear: both">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btn_page_update" runat="server" Text="Button" Style="display: none;" />
                        <uc1:UC_rnt_estate_list ID="UC_rnt_estate_list1" runat="server" Visible="false" />
                        <uc2:UC_loader_list ID="UC_loader_list1" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btn_page_update" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="pannello_fascia1">
            <asp:Literal ID="ltr_langTemp" runat="server"></asp:Literal>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
    <script type="text/javascript">
        var contentUpdater = "<%= btn_page_update.ClientID %>";
        function ReloadContent() {
            __doPostBack(contentUpdater, "load_content");
        }
        ReloadContent();
    </script>
</asp:Content>
