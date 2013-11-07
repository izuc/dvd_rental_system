<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View_Transaction.aspx.cs" MasterPageFile="~/DVDEzy.Master"  Inherits="DVDEzy.View_Transaction" Title="View Transaction" %>
<%@ Register Src="Form_Transaction.ascx" TagName="form" TagPrefix="transactionForm" %>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title">View Transaction</div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                <transactionForm:form ID="transactionForm" runat="server" Visible="False" />
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>
