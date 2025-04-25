<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_apt_in_rome_bottom.ascx.cs" Inherits="RentalInRome.uc.UC_apt_in_rome_bottom" %>
<asp:HiddenField runat="server" ID="HF_currZone" Value="0" Visible="false" />
<asp:HiddenField runat="server" ID="HF_currType" Value="bottom" Visible="false" />


<asp:ListView ID="LV_bottom" runat="server" ondatabound="LV_bottom_DataBound">
    <ItemTemplate>
        <a class="zona <%# "" + Eval("Id")=="9"?"zona_new":""%>" href="<%# Eval("Path") %>">
            <%# "" + Eval("Id")=="9" && 1==2 ?"<span class=\"ico_new\"></span>":""%>
            <span class="contimg">
                <img src="/<%# Eval("Img") %>" alt="<%# Eval("Title") %>" />
            </span>
            <span>
                <%# Eval("Title") %></span>
        </a>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div id="aptInRome">
            <h3>
                <asp:Literal ID="ltr_title" runat="server"></asp:Literal>
            </h3>
            <div class="aptInRome">
                <a id="itemPlaceholder" runat="server" />
            </div>
        </div>
    </LayoutTemplate>
</asp:ListView>
<asp:ListView ID="LV_sxZone" runat="server" OnDataBound="LV_sxZone_DataBound">
    <ItemTemplate>
        <a class="zonalista <%# "" + Eval("Id")=="9"?"zona_new":""%>" href="<%# Eval("Path") %>">
            <%# "" + Eval("Id")=="9" && 1==2 ?"<span class=\"ico_new\"></span>":""%>
            <span class="contimg">
                <img src="/<%# Eval("Img") %>" alt="<%# Eval("Title") %>" />
            </span>
            <span class="nomeZona">
                <%# Eval("Title") %></span>
            <span class="numApts">
                <strong>
                    <%# Eval("Count") %>
                </strong> 
            </span>
        </a>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div class="lista_zone_pgzona">
            <div>
                <span class="tit">
                    <asp:Literal ID="ltr_title" runat="server"></asp:Literal>
                </span>
                <a id="itemPlaceholder" runat="server" />
                <div class="nulla">
                </div>
            </div>
        </div>
    </LayoutTemplate>
</asp:ListView>
<asp:ListView ID="LV_zoneHome" runat="server">
    <ItemTemplate>
        <a class="zonaHome <%# "" + Eval("Id")=="9"?"zona_new":""%>" href="<%# Eval("Path") %>">
            <%# "" + Eval("Id")=="9" && 1==2 ?"<span class=\"ico_new\"></span>":""%>
            <span class="contimg">
                <img src="/images/css/zone_thumb.jpg" alt="<%# Eval("Title") %>" id="_zoneList_img_<%# Eval("Id") %>" />
            </span>
            <span class="nomeZonaHome">
                <%# Eval("Title") %></span>
            <span class="numAptsHome">
                <strong>
                    <%# Eval("Count") %>
                </strong>
            </span>
        </a>
        <script type="text/javascript">
            GLOBAL_imgLoader.push(new Array("_zoneList_img_<%# Eval("Id") %>","/<%# Eval("Img") %>"));
        </script>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div id="zoneHome">
            <a id="itemPlaceholder" runat="server" />
        </div>
    </LayoutTemplate>
</asp:ListView>
