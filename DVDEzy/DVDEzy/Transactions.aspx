<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" MasterPageFile="~/DVDEzy.Master" Inherits="DVDEzy.Transactions" Title="Transactions"%>
<%@ Import Namespace="DVDEzy.BusinessLayer"%>
<%@ Import Namespace="System.Collections.Generic"%>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title">Transactions</div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                <asp:Label ID="lblMessage" runat="server" Text="" Visible="False" Font-Bold="True" Font-Italic="True" Width="100%"></asp:Label> <br />
                <asp:GridView ID="gridTransactions" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                    CssClass="mGrid" PagerStyle-CssClass="pgr" GridLines="None" OnPageIndexChanging="gridTransactions_PageIndexChanging" OnRowCommand="gridTransactions_RowCommand" DataKeyNames="transactionID"
                    AlternatingRowStyle-CssClass="alt" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="customer" HeaderText="Member">
                            <HeaderStyle Width="20%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="transactionDate" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}">
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="quantityRented" HeaderText="Qty. Rented">
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="quantityPurchased" HeaderText="Qty. Purchased">
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="totalCost" HeaderText="Total Cost" DataFormatString="${0}">
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
	                            <asp:Button Text="View" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="cmdView"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
	                            <asp:Button Text="Delete" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="cmdDelete"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="alt" />
                    <EmptyDataTemplate>
                        There are no Transaction records.
                    </EmptyDataTemplate>
                </asp:GridView>
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>