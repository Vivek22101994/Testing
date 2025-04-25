<%@ Page Language="C#" %>
<%

    string _auth = Request.QueryString["auth"];
    string _qs = string.IsNullOrEmpty(_auth) ? "" : "?auth=" + _auth;
    Response.StatusCode = 301;
    Response.AddHeader("Location", "https://rentalinrome.com/reservationarea/login.aspx" + _qs);
    Response.End();
    return;
     %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="content-type" content="text/html; charset=utf-8" />
<title>Redirect</title>
</head>
<body>
<form id="form1" runat="server">
</form>
</body>
</html>
