<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add_Transaction.aspx.cs" MasterPageFile="~/DVDEzy.Master"  Inherits="DVDEzy.Add_Transaction" Title="Add Transaction" %>
<%@ Register Src="Form_Transaction.ascx" TagName="form" TagPrefix="transactionForm" %>
<%@ Import Namespace="DVDEzy.BusinessLayer"%>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title">Add Transaction</div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                <transactionForm:form ID="transactionForm" runat="server" />
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>
