<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/DVDEzy.Master" Inherits="DVDEzy.Default" Title="DVDEzy"%>
<%@ Import Namespace="DVDEzy"%>
<%@ Import Namespace="System.Collections.Generic"%>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title">Welcome David!</div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                You are now logged-into the store administration panel.
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>
