<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBodyFooter.ascx.cs" Inherits="RentalInRome.mobile.uc.ucBodyFooter" %>
<footer id="footerMobile" class="footer" data-stretch="true">
    <span class="datiSoc">© Rental in Rome 2003 - <%= DateTime.Now.Year %></span>
    <menu class="menuFooter">
        <a data-role="button" data-rel="modalview" href="#modalview-privacy">Privacy</a>
        <a href="<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>?force=desktop"><%= contUtils.getLabel("lblDesktopVersion") %></a>
    </menu>
    <div class="nulla">
    </div>
</footer>
