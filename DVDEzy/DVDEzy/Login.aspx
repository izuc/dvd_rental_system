<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" MasterPageFile="~/DVDEzy.Master" Inherits="DVDEzy.Login" Title="DVDEzy - Login Page" %>
<%@ Import Namespace="DVDEzy"%>
<%@ Import Namespace="System.Collections.Generic"%>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="login_area">
        <div id="login_header"></div>
        <div id="login_repeat">
            <div id="login_box">
                <div id="error_message">
                    <asp:Label ID="label_message" runat="server" Text=""></asp:Label>
                </div>
                <div id="login_username">
                    <div id="login_username_label"></div>
                    <div id="username_field"><asp:TextBox ID="username" Width="200px" runat="server"></asp:TextBox></div>
                </div>
                <div id="login_password">
                    <div id="login_password_label"></div>
                    <div id="login_password_field"><asp:TextBox ID="password" TextMode="password" Width="200px" runat="server"></asp:TextBox></div>
                </div>
                <div id="login_submit"><asp:Button ID="btnLogin" runat="server" Text="Login" onclick="btnLogin_Click" /></div>
            </div>
        </div>
        <div id="login_footer"></div>
    </div>
</asp:Content>
