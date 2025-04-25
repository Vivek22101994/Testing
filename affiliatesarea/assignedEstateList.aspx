<%@ Page Title="" Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="assignedEstateList.aspx.cs" Inherits="RentalInRome.affiliatesarea.assignedEstateList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%>
        </telerik:RadCodeBlock>
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">    
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
     <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "estatePrice.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(800);
                rwdUrl.set_minHeight(700);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                //rwdUrl.maximize();
            }
            return false;
        }
        function rwdUrl_OnClientClose(sender, eventArgs) {
            $find('<%= pnlFascia.ClientID %>').ajaxRequest('rwdUrl_Closing');
        }
    </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
     <script language="c#" runat="server">
      public Dictionary<int, string> FillPriceChangeTypeDropdown()
      {
          Dictionary<int, string> dic = new Dictionary<int, string>();
          dic.Add(0, contUtils.getLabel("lblIncrement"));
          dic.Add(1, contUtils.getLabel("lblDiscount"));
          return dic;
      }
    </script>
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
        <telerik:RadCodeBlock ID="RadCodeBlock4" runat="server">
            <h3 style="margin-bottom: 20px; margin-left: 8px;" class="underlined"><%=CurrentSource.getSysLangValue("lblPrices")%></h3>
        </telerik:RadCodeBlock>
        <div class="nulla">
        </div>
        <div class="agentprice">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                <tr>
                    <td class="td_title" style="width: auto;">
                         <telerik:RadCodeBlock ID="RadCodeBlock5" runat="server">
                            <%=CurrentSource.getSysLangValue("lblRate")%>:
                        </telerik:RadCodeBlock>
                    </td>
                    <td>
                        <asp:DropDownList ID="drp_wl_changeIsDiscount_Agent" runat="server" Style="height: 25px;" Width="100px"
                            DataSource='<%# FillPriceChangeTypeDropdown() %>' DataTextField="Value" DataValueField="Key" >
                            <%--<asp:ListItem Value="0">aumento di</asp:ListItem>
                            <asp:ListItem Value="1">sconto di</asp:ListItem>--%>
                        </asp:DropDownList>
                        <telerik:RadNumericTextBox ID="ntxt_wl_changeAmount_Agent" runat="server" Width="50" MinValue="0" Style="text-align: right;">
                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                        </telerik:RadNumericTextBox>%
                    </td>
                </tr>
                <tr>
                    <td>
                         <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click" >
                             <span> <%=CurrentSource.getSysLangValue("lblSaveChanges")%></span>
                         </asp:LinkButton>
                    </td>
                    <td>
                         <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click">
                             <span><%=CurrentSource.getSysLangValue("lblCancelChanges")%></span>
                         </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="nulla">
        </div>
         <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
            <h3 style="margin-bottom: 20px; margin-left: 8px;" class="underlined"><%=CurrentSource.getSysLangValue("lblEstateList")%></h3>
        </telerik:RadCodeBlock>
        <div class="nulla">
        </div>
        <div class="estatelist">
            <div class="table_fascia tfagency">
                <asp:ListView ID="LV" runat="server">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <a href="/<%# Eval("page_path")%>" id="lnkEstate" class="esttitle" target="_blank"><span><%# Eval("title")%></span></a></td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" class="dettlink" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="<%# contUtils.getLabel("lblOpenDetail",App.LangID,"") %>" style="text-decoration: none; border: 0 none; margin: 5px;">
                                    <%# contUtils.getLabel("lblDetail") %>
                                </a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" style="">
                            <tr>
                                <td>No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" style="">
                            <tr id="itemPlaceholder" runat="server" />
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
