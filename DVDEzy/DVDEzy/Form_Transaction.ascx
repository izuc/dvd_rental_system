<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Form_Transaction.ascx.cs" Inherits="DVDEzy.Form_Transaction" %>
<script type="text/javascript">

    function disableRemoveRentalButton() {
        document.getElementById("<%= btnRemoveRental.ClientID %>").disabled = false;
    }

    function disableRemovePurchaseButton() {
        document.getElementById("<%= btnRemovePurchase.ClientID %>").disabled = false;
    }

    // Finds the index position for the selected item in the received listbox.
    function findSelectedIndex(listbox) {
        for (var i = 0; i < listbox.options.length; i++){
            if (listbox.options[i].selected){
                return i;
            }
        }
        return -1;
    }

    var selectedRental = -1; // Keeps track of the currently selected item in the listbox.
    // Used to DeSelect the Rentals in the ListBox.
    function deselectRentals() {
        var s = document.getElementById("<%= rentals.ClientID %>");
        for (var i = 0; i < s.options.length; i++) {
            // The following won't work in IE :( I've tried many different
            // things to dynamically add a click event but it just will not fire.
            s.options[i].onclick = function () {
                var selectedIndex = findSelectedIndex(s);
                if (selectedIndex > -1) {
                    if (selectedRental == selectedIndex) {
                        this.selected = false;
                        selectedRental = -1;
                        document.getElementById("<%= btnRemoveRental.ClientID %>").disabled = true;
                    } else {
                        selectedRental = selectedIndex;
                        document.getElementById("<%= btnRemoveRental.ClientID %>").disabled = false;
                    }
                }
            }
        }
    }

    var selectedPurchase = -1;  // Keeps track of the currently selected item in the listbox.
    // Used to DeSelect the Purchases in the ListBox.
    function deselectPurchases() {
        var s = document.getElementById("<%= purchases.ClientID %>");
        for (var i = 0; i < s.options.length; i++) {
            s.options[i].onclick = function() {
                var selectedIndex = findSelectedIndex(s);
                if (selectedIndex > -1) {
                    if (selectedPurchase == selectedIndex) {
                        this.selected = false;
                        selectedPurchase = -1;
                        document.getElementById("<%= btnRemovePurchase.ClientID %>").disabled = true;
                    } else {
                        selectedPurchase = selectedIndex;
                        document.getElementById("<%= btnRemovePurchase.ClientID %>").disabled = false;
                    }
                }
            }
        }
    }
</script>

<asp:hiddenfield id="transactionID" runat="server" />
<table class="frmData" style="width: 100%">
    <tr>
        <td colspan="2" class="form_message form_row_shade1">
            <asp:Label ID="lblMessage" runat="server" Text="" Visible="False" Font-Bold="True" Font-Italic="True" Width="100%"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="form_label form_row_shade1">Customer:</td>
        <td class="form_field form_row_shade1">
            <asp:DropDownList ID="customers" runat="server" OnSelectedIndexChanged="customers_SelectedIndexChange" AutoPostBack="True" Width="200"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvCustomers" runat="server" ControlToValidate="customers" ErrorMessage="Customer is Required. *" CssClass="error"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="form_label form_row_shade2">DVD:</td>
        <td class="form_field form_row_shade2">
            <table cellspacing="0" cellpadding="0">
                <tr>
                    <td valign="top"><asp:DropDownList ID="dvds" runat="server" OnSelectedIndexChanged="dvds_SelectedIndexChange" AutoPostBack="True" Width="200"></asp:DropDownList></td>
                    <td valign="top">
                        <asp:PlaceHolder ID="dvdCopies" runat="server" Visible="False">
                            <table cellspacing="0" cellpadding="0" style="padding-left: 5px;">
                                <tr>
                                    <td valign="top"><asp:DropDownList ID="copies" runat="server" Width="130"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td valign="top" style="padding: 5px;">
                                        <asp:Button ID="btnRental" runat="server" Text="Rental" CausesValidation="False" onclick="btnRental_Click" Width="60px" />
                                        <asp:Button ID="btnSale" runat="server" Text="Sale" CausesValidation="False" onclick="btnSale_Click" Width="60px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:PlaceHolder>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="form_label form_row_shade1"></td>
        <td class="form_field form_row_shade1">
            <table>
                <tr>
                    <th style="font-size: small">Rentals</th>
                    <th style="font-size: small">Purchases</th>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="rentals" onchange="disableRemoveRentalButton();" runat="server" Width="250"></asp:ListBox> <br />
                        <asp:Button ID="btnRemoveRental" runat="server" Text="Remove" CausesValidation="False" onclick="btnRemoveRental_Click" />
                    </td>
                    <td>
                        <asp:ListBox ID="purchases" onchange="disableRemovePurchaseButton();" runat="server" Width="250"></asp:ListBox> <br />
                        <asp:Button ID="btnRemovePurchase" runat="server" Text="Remove" CausesValidation="False" onclick="btnRemovePurchase_Click" />
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                // Adds the click events to each item in the Listboxes.
                deselectRentals();
                deselectPurchases();
            </script>
        </td>
    </tr>
    <asp:PlaceHolder ID="dueDate" runat="server" Visible="False">
    <tr>
        <td class="form_label form_row_shade2">Due Date:</td>
        <td class="form_field form_row_shade2">
            <asp:Label ID="lblDueDate" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td class="form_label form_row_shade1">Discount Rate:</td>
        <td class="form_field form_row_shade1">
            <asp:Label ID="lblDiscount" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="form_label form_row_shade1">Total Cost:</td>
        <td class="form_field form_row_shade1">
            <asp:Label ID="lblTotalCost" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="form_label form_row_shade2">Payment Type:</td>
        <td class="form_field form_row_shade2">
            <table>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="paymentType" runat="server" OnSelectedIndexChanged="paymentType_SelectedIndexChange" AutoPostBack="true">
                            <asp:ListItem>Cash</asp:ListItem>
                            <asp:ListItem>Visa</asp:ListItem>
                            <asp:ListItem>Mastercard</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="vertical-align: top;"><asp:RequiredFieldValidator ID="rfvPaymentType" runat="server" ControlToValidate="paymentType" ErrorMessage="Payment Type is Required. *" CssClass="error"></asp:RequiredFieldValidator></td>
                </tr>
            </table>
        </td>
    </tr>
    <asp:PlaceHolder ID="optionCreditCard" runat="server" Visible="False">
    <tr>
        <td class="form_label form_row_shade1">Credit Card:</td>
        <td class="form_field form_row_shade1">
            <asp:PlaceHolder ID="viewCreditCards" runat="server" Visible="True">
                <asp:RequiredFieldValidator ID="rfvCreditCards" runat="server" ControlToValidate="creditCards" ErrorMessage="Credit Card is Required. *" CssClass="error"></asp:RequiredFieldValidator>
                <table width="100%">
                    <tr>
                        <th width="30%" style="text-align: left; font-size: small">Card Holder</th>
                        <th width="30%" style="text-align: left; font-size: small">Credit Card</th>
                        <th width="40%"></th>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="cardHolder" runat="server" Enabled="False" ReadOnly="True" Width="150"></asp:TextBox></td>
                        <td><asp:DropDownList ID="creditCards" runat="server" OnSelectedIndexChanged="creditCard_SelectedIndexChange" AutoPostBack="true" Width="200"></asp:DropDownList></td>
                        <td><asp:Button ID="btnAddCard" runat="server" Text="Add Card" onclick="btnAddCard_Click" CausesValidation="False" Width="100" /></td>
                    </tr>
                </table>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="addCreditCard" runat="server" Visible="False">
                <table width="100%">
                    <tr>
                        <td><asp:RequiredFieldValidator ID="rfvCardHolder" runat="server" ControlToValidate="txtCardHolder" ErrorMessage="Card Holder is Required. *" CssClass="error"></asp:RequiredFieldValidator></td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvCardNumber" runat="server" display="Dynamic"  ControlToValidate="txtCreditCard" ErrorMessage="Card Number is Required. *" CssClass="error"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revCardNumber" display="Dynamic" runat="server" ErrorMessage="Credit Card Must have 16 Digits." ControlToValidate="txtCreditCard" ValidationExpression="[0-9]{16}" CssClass="error"></asp:RegularExpressionValidator>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <th width="30%" style="text-align: left; font-size: small">Card Holder</th>
                        <th width="30%" style="text-align: left; font-size: small">Credit Card</th>
                        <th width="40%"></th>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="txtCardHolder" runat="server" Width="150"></asp:TextBox></td>
                        <td><asp:TextBox ID="txtCreditCard" runat="server" Width="200" MaxLength="16"></asp:TextBox></td>
                        <td><asp:Button ID="btnSaveCard" runat="server" Text="Save Card" onclick="btnSaveCard_Click" Width="100" /><asp:Button ID="btnCancelCard" runat="server" Text="Cancel" onclick="btnCancelCard_Click" CausesValidation="False" Width="100" /></td>
                    </tr>
                </table>
            </asp:PlaceHolder>
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td colspan="2" class="form_submit form_row_shade2"><asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" /></td>
    </tr>
</table>