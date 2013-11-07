<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Form_DVD.ascx.cs" Inherits="DVDEzy.Form_DVD" %>
<script type="text/javascript">
function validateBarcode(enabled) {
    var rfvBarcode = document.getElementById('<%=rfvBarcode.ClientID%>');
    var revBarcode = document.getElementById('<%=revBarcode.ClientID%>');
    if (rfvBarcode && revBarcode) {
        ValidatorEnable(rfvBarcode, enabled);
        ValidatorEnable(revBarcode, enabled);
        ValidatorUpdateDisplay(rfvBarcode);
        ValidatorUpdateDisplay(revBarcode);
    }
    Page_ClientValidate();
    return Page_IsValid;
}
</script>
<div style="margin-left: 50px; margin-top: 10px;">
    <asp:hiddenfield id="dvdID" runat="server" />
    <table class="frmData" style="width: 700px">
        <tr>
            <td colspan="2" class="form_message form_row_shade1">
                <asp:Label ID="lblMessage" runat="server" Text="" Visible="False" Font-Bold="True" Font-Italic="True" Width="100%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_label form_row_shade1">Title:</td>
            <td class="form_field form_row_shade1"><asp:TextBox ID="title" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="title" ErrorMessage="The title is required. *" CssClass="error"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="form_label form_row_shade2">Director:</td>
            <td class="form_field form_row_shade2">
                <asp:TextBox ID="director" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDirector" display="Dynamic" runat="server" ControlToValidate="director" ErrorMessage="The director is required. *" CssClass="error"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revDirector" display="Dynamic" runat="server" ErrorMessage="Must be only letters." ControlToValidate="director" ValidationExpression="[a-zA-Z ]+" CssClass="error"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="form_label form_row_shade1">Year:</td>
            <td class="form_field form_row_shade1"><asp:TextBox ID="yearReleased" runat="server" MaxLength="4"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvYearReleased" display="Dynamic" runat="server" ControlToValidate="yearReleased" ErrorMessage="The year is required. *" CssClass="error"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revYearReleased" display="Dynamic" runat="server" ErrorMessage="Must be a year." ControlToValidate="yearReleased" ValidationExpression="[0-9]{4}" CssClass="error" ></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td class="form_label form_row_shade2">Sale Price:</td>
            <td class="form_field form_row_shade2">
                <asp:TextBox ID="salePrice" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSalePrice" display="Dynamic" runat="server" ControlToValidate="salePrice" ErrorMessage="The sale price is required. *" CssClass="error"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revSalePrice" display="Dynamic" runat="server" ErrorMessage="Must be numeric." ControlToValidate="salePrice" ValidationExpression="^\d+(\.\d+)?$" CssClass="error"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="form_label form_row_shade1">Rental Price:</td>
            <td class="form_field form_row_shade1">
                <asp:TextBox ID="rentalPrice" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvRentalPrice" display="Dynamic" runat="server" ControlToValidate="rentalPrice" ErrorMessage="The rental price is required. *" CssClass="error"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revRentalPrice" display="Dynamic" runat="server" ErrorMessage="Must be numeric." ControlToValidate="rentalPrice" ValidationExpression="^\d+(\.\d+)?$" CssClass="error"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="form_submit form_row_shade2"><asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" OnClientClick="return validateBarcode(false);" /></td>
        </tr>
    </table>
    <br />
    <% if (this.dvdID.Value != String.Empty) { %>
        <asp:RequiredFieldValidator ID="rfvBarcode" display="Dynamic" runat="server" ControlToValidate="txtBarcode" ErrorMessage="Must have a Barcode *" CssClass="error"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="revBarcode" display="Dynamic" runat="server" ErrorMessage="Barcode must be 5 digits." ControlToValidate="txtBarcode" ValidationExpression="[0-9]{5}" CssClass="error"></asp:RegularExpressionValidator>
        <table class="mGrid" style="border-bottom: 0;" width="700px">
            <tr>
                <th width="40%">Barcode</th>
                <th width="30%">Available</th>
                <th width="30%"></th>
            </tr>
            <tr>
                <td style="border-bottom: 0;"><asp:TextBox ID="txtBarcode" runat="server" MaxLength="5"></asp:TextBox></td>
                <td style="border-bottom: 0;"><asp:CheckBox ID="chkAvailability" runat="server" Checked="true" /></td>
                <td style="border-bottom: 0;"><asp:Button ID="btnInsert" runat="server" Text="Add Copy" onclick="btnInsert_Click" OnClientClick="return validateBarcode(true);" /></td>
            </tr>
        </table>
        <asp:GridView ID="gridCopies" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="5"
            CssClass="mGrid" PagerStyle-CssClass="pgr" GridLines="None" AlternatingRowStyle-CssClass="alt" 
            Width="700px" OnRowCommand="gridCopies_RowCommand" OnPageIndexChanging="gridCopies_PageIndexChanging" DataKeyNames="copyID" ShowHeader="False"
            OnRowEditing="gridCopies_RowEditing"          
            OnRowUpdating="gridCopies_RowUpdating">
            <Columns>
                <asp:BoundField DataField="barcode" ItemStyle-Width="40%"></asp:BoundField>
                <asp:CheckBoxField DataField="isAvailable" ItemStyle-Width="30%"></asp:CheckBoxField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button Text="Edit" runat="server" CommandName="Edit" CausesValidation="false" OnClientClick="return validateBarcode(false);"></asp:Button>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button Text="Update" runat="server" CommandName="Update" CausesValidation="false" ></asp:Button>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
	                    <asp:Button Text="Delete" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="cmdDelete" CausesValidation="false" OnClientClick="return validateBarcode(false);"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No copies have been added.
            </EmptyDataTemplate>
        </asp:GridView>
     <% } %>
</div>