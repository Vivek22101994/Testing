<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_contactStaff.ascx.cs" Inherits="RentalInRome.areariservataagenzie.uc.UC_contactStaff" %>
<p>
    <%=CurrentSource.getSysLangValue("lblModContRent")%>
</p>
<div class="salvataggio">
    <div class="bottom_salva">
        <a href="#" onclick="return open_utilForm_sendMailToStaff()"><span><%=CurrentSource.getSysLangValue("lblCliccaScriveiMess")%></span></a>
    </div>
</div>
