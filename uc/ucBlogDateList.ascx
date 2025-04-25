<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBlogDateList.ascx.cs" Inherits="RentalInRome.uc.ucBlogDateList" %>
<div class="box_blog" id="articoliPeriodi">
    <span class="titBoxSx">
       <%= contUtils.getLabel("lblArticoliPerPeriodi")%></span>
    <asp:Literal ID="ltrYearList" runat="server"></asp:Literal>
    <asp:Literal ID="ltrMonthList" runat="server"></asp:Literal>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
        <ul class="listanno">
            <li><a href="#">2010</a></li>
            <li><a href="#">2011</a></li>
        </ul>
        <div class="nulla">
        </div>
        <ul class="listmese">
            <li><a href="#">Gennaio 2011</a></li>
            <li><a href="#">Febbraio 2011</a></li>
            <li><a href="#">Marzo 2011</a></li>
            <li><a href="#">Aprile 2011</a></li>
            <li><a href="#">Maggio 2011</a></li>
        </ul>
    </asp:PlaceHolder>
    <div class="nulla">
    </div>
</div>
