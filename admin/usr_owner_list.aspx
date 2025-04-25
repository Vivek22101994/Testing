<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_owner_list.aspx.cs" Inherits="RentalInRome.admin.usr_owner_list" %>

<%@ Register Src="uc/UC_loader_list.ascx" TagName="UC_loader_list" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

		function showFilter() {
			$get("lnk_showfilt").style.display = "none";
			$get("lnk_hidefilt").style.display = "";
			$get("tbl_filter").style.display = "";
			$get("<%=HF_is_filtered.ClientID %>").value = "true";
		}
		function hideFilter() {
			$get("lnk_showfilt").style.display = "";
			$get("lnk_hidefilt").style.display = "none";
			$get("tbl_filter").style.display = "none";
			$get("<%=HF_is_filtered.ClientID %>").value = "false";
		}
		function Filter() {
			
			var _filter = "";
			var _value = ""; 
			_value = $get("<%= drp_country.ClientID%>").value;
			if (_value != "")
				_filter += "&country=" + _value;
			_value = $get("<%= txt_name_full.ClientID%>").value; 
			if (_value != "")
				_filter += "&name_full=" + _value;
			_value = $get("<%= txt_email.ClientID%>").value;
			if (_value != "")
				_filter += "&email=" + _value;

			_value = $get("<%= HF_date_created_from.ClientID%>").value;
			if (_value != "")
				_filter += "&dtcf=" + _value;
			_value = $get("<%= HF_date_created_to.ClientID%>").value;
			if (_value != "")
				_filter += "&dtct=" + _value;
				
			window.location = "?flt=true" + _filter;


			return;
			_value = $get("<%= drp_is_deleted.ClientID%>") != null ? $get("<%= drp_is_deleted.ClientID%>").value : "0";
			if (_value != "")
				_filter += "&is_del=" + _value;
			_value = $get("<%= drp_account.ClientID%>").value;
			if (_value != "")
				_filter += "&account=" + _value;
			_value = $get("<%= drp_lang.ClientID%>").value;
			if (_value != "")
				_filter += "&lang=" + _value;
		}
		function swapColumn(col) {
			if ($("#state_" + col).val() == "0") {
				$("#toggler_" + col).addClass("opened");
				$("#toggler_" + col).removeClass("closed");
				$("#state_" + col).val("1");
				$(".cont_" + col).css("display", "");
			}
			else {
				$("#toggler_" + col).removeClass("opened");
				$("#toggler_" + col).addClass("closed");
				$("#state_" + col).val("0");
				$(".cont_" + col).css("display", "none");
			}
		}
    </script>

    <style type="text/css">
		#column_visualizer{
			clear: both;
			margin-bottom: 8px;
			margin-top: 15px;
			display: block;
		}
		#column_visualizer a.chk{
			width:20px;
			height:20px;
			text-indent:200px;
			cursor:pointer;
			color: #fff;
			display: inline-block;
			margin: 0;
			text-decoration: none;
		}
		#column_visualizer a.opened{
			 background-image:url('../images/ico/chk_ok.png');
		}
		#column_visualizer a.closed{
			 background-image:url('../images/ico/chk_no.png');
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal ID="Literal1" runat="server" Visible="false">
		Riepilogo colonne:<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
    </asp:Literal>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:HiddenField ID="HF_is_filtered" runat="server" />
            <asp:HiddenField ID="HF_url_filter" runat="server" />
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Elenco Proprietari</h1>
                        <div class="bottom_agg">
                            <a href="usr_owner_details.aspx"><span>+ Nuovo</span></a>
                        </div>
                        <div class="nulla">
                        </div>
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
                                                        <asp:PlaceHolder ID="PH_admin" runat="server">
                                                            <td>
                                                                <label>
                                                                    Eliminati:</label>
                                                                <asp:DropDownList runat="server" ID="drp_is_deleted" Width="70px" CssClass="inp">
                                                                    <asp:ListItem Value="">-tutti-</asp:ListItem>
                                                                    <asp:ListItem Value="1">SI</asp:ListItem>
                                                                    <asp:ListItem Value="0">NO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </asp:PlaceHolder>
                                                        <td>
                                                            <label>
                                                                Nome Cognome:</label>
                                                            <asp:TextBox ID="txt_name_full" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Email:</label>
                                                            <asp:TextBox ID="txt_email" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Data Creazione:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="cal_date_created_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_date_created_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="cal_date_created_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_date_created_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_date_created_from" runat="server" />
                                                            <asp:HiddenField ID="HF_date_created_to" runat="server" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Location</label>
                                                            <asp:DropDownList runat="server" ID="drp_country" CssClass="inp" Width="120px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td runat="server" visible="false">
                                                            <label>
                                                                Account</label>
                                                            <asp:DropDownList runat="server" ID="drp_account" CssClass="inp" Width="120px">
                                                            </asp:DropDownList>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                        </td>
                                                        <td runat="server" visible="false">
                                                            <label>
                                                                Lingua</label>
                                                            <asp:DropDownList runat="server" ID="drp_lang" Width="100px" CssClass="inp">
                                                            </asp:DropDownList>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <a href="javascript: Filter()" class="ricercaris">
                                                    <span>Filtra Risultati</span></a>
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
                    </div>
                    <div style="clear: both">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="HF_lds_filter" runat="server" Visible="false" />
                                <asp:HiddenField ID="HF_id_editAdmin" runat="server" Visible="false" />
                                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_TBL_OWNER" OrderBy="name_full">
                                </asp:LinqDataSource>
                                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" Visible="false" OnDataBound="LV_DataBound" OnItemCommand="LV_ItemCommand">
                                    <ItemTemplate>
                                        <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                            <td>
                                                <span>
                                                    <%# Eval("id") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("contact_email")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("contact_phone")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("contact_phone_mobile")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# contUtils.getLang_title(Eval("pid_lang").objToInt32())%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("date_created")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_pid_operator" Visible="false" runat="server" Text='<%# Eval("pid_operator") %>' />
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <a href="usr_owner_details.aspx?id=<%# Eval("id") %>" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
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
                                                <span>
                                                    <%# Eval("name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("contact_email")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("contact_phone")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("contact_phone_mobile")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# contUtils.getLang_title(Eval("pid_lang").objToInt32())%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("date_created")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_pid_operator" Visible="false" runat="server" Text='<%# Eval("pid_operator") %>' />
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <a href="usr_owner_details.aspx?id=<%# Eval("id") %>" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>
                                        </tr>
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
                                        <div class="page">
                                            <asp:DataPager ID="DataPager2" runat="server" PageSize="50" style="border-right: medium none;" QueryStringField="pg">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="20" />
                                                </Fields>
                                            </asp:DataPager>
                                            <asp:Label ID="lbl_record_count_top" runat="server" CssClass="total" Text=""></asp:Label>
                                            <div class="nulla">
                                            </div>
                                        </div>
                                        <div class="table_fascia">
                                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                                <tr>
                                                    <th style="text-align: center;">
                                                        ID
                                                    </th>
                                                    <th>
                                                        Nome Cognome
                                                    </th>
                                                    <th>
                                                        E-mail
                                                    </th>
                                                    <th>
                                                        Telefono
                                                    </th>
                                                    <th>
                                                        Cellulare
                                                    </th>
                                                    <th>
                                                        Lingua
                                                    </th>
                                                    <th>
                                                        Data Ora Creazione
                                                    </th>
                                                    <th>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server" />
                                            </table>
                                        </div>
                                        <div class="page">
                                            <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;" QueryStringField="pg">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="20" />
                                                </Fields>
                                            </asp:DataPager>
                                            <asp:Label ID="lbl_record_count" runat="server" CssClass="total" Text=""></asp:Label>
                                        </div>
                                    </LayoutTemplate>
                                </asp:ListView>
                                <asp:Button ID="btn_page_update" runat="server" Text="Button" Style="display: none;" />
                                <uc2:UC_loader_list ID="UC_loader_list1" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btn_page_update" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
		var contentUpdater = "<%= btn_page_update.ClientID %>";
		function ReloadContent() {
			__doPostBack(contentUpdater, "load_content");
		}
		ReloadContent();
		var cal_date_created_from;
		var cal_date_created_to;
		function setCal() {
			cal_date_created_from = new JSCal_single('cal_date_created_from', '<%= HF_date_created_from.ClientID %>', 'cal_date_created_from', null, '', 'del_date_created_from');
			cal_date_created_to = new JSCal_single('cal_date_created_to', '<%= HF_date_created_to.ClientID %>', 'cal_date_created_to', null, '', 'del_date_created_to');
		}
    </script>

</asp:Content>
