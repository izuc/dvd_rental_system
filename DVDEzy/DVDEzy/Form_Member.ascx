<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Form_Member.ascx.cs" Inherits="DVDEzy.Form_Member" %>
<script type="text/javascript">
function validateAddCard(enabled) {
    var rfvCardNumber = document.getElementById('<%=rfvCardNumber.ClientID%>');
    var revCardNumber = document.getElementById('<%=revCardNumber.ClientID%>');
    var rfvCardHolder = document.getElementById('<%=rfvCardHolder.ClientID%>');
    if (rfvCardNumber && revCardNumber && rfvCardHolder) {
        ValidatorEnable(rfvCardNumber, enabled);
        ValidatorEnable(rfvCardHolder, enabled);
        ValidatorEnable(revCardNumber, enabled);
        ValidatorUpdateDisplay(rfvCardNumber);
        ValidatorUpdateDisplay(rfvCardHolder);
        ValidatorUpdateDisplay(revCardNumber);
    }
    Page_ClientValidate();
    return Page_IsValid;
}
</script>

<div style="margin-left: 50px; margin-top: 10px;">
    <asp:hiddenfield id="customerID" runat="server" />
    <table class="frmData" style="width: 700px">
        <tr>
            <td colspan="2" class="form_message form_row_shade1">
                <asp:Label ID="lblMessage" runat="server" Text="" Visible="False" Font-Bold="True" Font-Italic="True" Width="100%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_label form_row_shade1">First name:</td>
            <td class="form_field form_row_shade1">
                <asp:TextBox ID="firstName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" display="Dynamic" runat="server" ControlToValidate="firstName" ErrorMessage="The first name is required. *" CssClass="error"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revFirstName" display="Dynamic" runat="server" ErrorMessage="Must be only letters." ControlToValidate="firstName" ValidationExpression="[a-zA-Z]+" CssClass="error"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="form_label form_row_shade2">Last name:</td>
            <td class="form_field form_row_shade2">
                <asp:TextBox ID="lastName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvLastName" display="Dynamic" runat="server" ControlToValidate="lastName" ErrorMessage="The last name is required. *" CssClass="error"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revLastName" display="Dynamic" runat="server" ErrorMessage="Must be only letters." ControlToValidate="lastName" ValidationExpression="[a-zA-Z]+" CssClass="error"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="form_label form_row_shade1">Gender:</td>
            <td class="form_field form_row_shade1">
                <table>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="gender" runat="server">
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="vertical-align: top;"><asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="gender" ErrorMessage="Gender is required. *" CssClass="error"></asp:RequiredFieldValidator></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="form_label form_row_shade2">Street address:</td>
            <td class="form_field form_row_shade2"><asp:TextBox ID="streetAddress" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvStreetAddress" runat="server" ControlToValidate="streetAddress" ErrorMessage="The address is required. *" CssClass="error"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="form_label form_row_shade1">Billing address:</td>
            <td class="form_field form_row_shade1"><asp:TextBox ID="billingAddress" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="form_label form_row_shade2">Postcode:</td>
            <td class="form_field form_row_shade2"><asp:TextBox ID="postcode" runat="server" MaxLength="4"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPostcode" display="Dynamic" runat="server" ControlToValidate="postcode" ErrorMessage="The postcode is required. *" CssClass="error"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revPostcode" display="Dynamic" runat="server" ErrorMessage="Postcode must be 4 numbers" ControlToValidate="postcode" ValidationExpression="[0-9]{4}" CssClass="error"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td colspan="2" class="form_submit form_row_shade1"><asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" OnClientClick="return validateAddCard(false);" /></td>
        </tr>
    </table>
    <br />
    <% if (this.customerID.Value != String.Empty) { %>
        <table width="700px">
            <tr>
                <td width="30%"><asp:RequiredFieldValidator ID="rfvCardHolder" runat="server" ControlToValidate="txtCardHolder" ErrorMessage="Must have a Card Holder *" CssClass="error"></asp:RequiredFieldValidator>&nbsp;</td>
                <td width="40%">
                    <asp:RequiredFieldValidator ID="rfvCardNumber" display="Dynamic" runat="server" ControlToValidate="txtCardNumber" ErrorMessage="Must have a Credit Card *" CssClass="error"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revCardNumber" display="Dynamic" runat="server" ErrorMessage="Credit Card Must have 16 Digits." ControlToValidate="txtCardNumber" ValidationExpression="[0-9]{16}" CssClass="error"></asp:RegularExpressionValidator>&nbsp;
                </td>
                <td width="30%"></td>
            </tr>
        </table>
        <table class="mGrid" style="border-bottom: 0;" width="700px">
            <tr>
                <th width="30%">Card Holder</th>
                <th width="40%">Card Number</th>
                <th width="30%"></th>
            </tr>
            <tr>
                <td style="border-bottom: 0;"><asp:TextBox ID="txtCardHolder" runat="server"></asp:TextBox></td>
                <td style="border-bottom: 0;"><asp:TextBox ID="txtCardNumber" runat="server" MaxLength="16"></asp:TextBox></td>
                <td style="border-bottom: 0;"><asp:Button ID="btnInsert" runat="server" Text="Add Card" onclick="btnInsert_Click" OnClientClick="return validateAddCard(true);" /></td>
            </tr>
        </table>
        <asp:GridView ID="gridCards" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="5"
            CssClass="mGrid" PagerStyle-CssClass="pgr" GridLines="None" AlternatingRowStyle-CssClass="alt"
            Width="700px" OnRowCommand="gridCards_RowCommand" OnPageIndexChanging="gridCards_PageIndexChanging" DataKeyNames="cardID" ShowHeader="False"
            OnRowEditing="gridCards_RowEditing"          
            OnRowUpdating="gridCards_RowUpdating">
            <Columns>
                <asp:BoundField DataField="cardName" ItemStyle-Width="30%"></asp:BoundField>
                <asp:BoundField DataField="cardNumber" ItemStyle-Width="40%"></asp:BoundField>
                <asp:TemplateField>
	                <ItemTemplate>
		                <asp:Button Text="Edit" runat="server" CommandName="Edit" CausesValidation="false" OnClientClick="return validateAddCard(false);"></asp:Button>
	                </ItemTemplate>
	                <EditItemTemplate>
		                <asp:Button Text="Update" runat="server" CommandName="Update" CausesValidation="false" ></asp:Button>
	                </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
	                <ItemTemplate>
		                <asp:Button Text="Delete" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="cmdDelete" CausesValidation="false" OnClientClick="return validateAddCard(false);"></asp:Button>
	                </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No credit cards are added.
            </EmptyDataTemplate>
        </asp:GridView>
    <% } %>
</div>

