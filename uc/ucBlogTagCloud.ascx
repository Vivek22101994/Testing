<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBlogTagCloud.ascx.cs" Inherits="RentalInRome.uc.ucBlogTagCloud" %>
<div class="box_blog">
    <span class="titBoxSx">
        Tag Cloud</span>
    <span class="testo_blog tagcloud">
        <%= blogUtils.getTagCloud(App.LangID, "<a href='/#pagePath#' class='#cssClass#'>#title#</a>&nbsp &nbsp")%> 
    </span>
</div>
