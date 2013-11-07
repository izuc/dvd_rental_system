using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DVDEzy.BusinessLayer;

namespace DVDEzy {
    public partial class Form_Member : System.Web.UI.UserControl {

        /// <summary>
        /// The page load event first disables the validation on the card number & card holder
        /// which is reactivated when adding a card (in order to avoid conflict with the validation controls).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            this.rfvCardNumber.Enabled = false;
            this.revCardNumber.Enabled = false;
            this.rfvCardHolder.Enabled = false;
        }

        /// <summary>
        /// The ShowRecord method recieves a Customer object and shows it to the form.
        /// </summary>
        /// <param name="customer"></param>
        public void ShowRecord(Customer customer) {
            if (customer != null) {
                if (!Page.IsPostBack) { // Ensures that its not reloaded again when posted.
                    this.customerID.Value = customer.customerID.ToString();
                    this.firstName.Text = customer.firstName;
                    this.lastName.Text = customer.lastName;
                    this.gender.SelectedIndex = Convert.ToInt32(customer.gender);
                    this.streetAddress.Text = customer.streetAddress;
                    this.billingAddress.Text = customer.billingAddress;
                    this.postcode.Text = customer.postcode.ToString();
                    this.RefreshCards(); // Shows the creditcards in the datagridview.
                }
            }
        }

        /// <summary>
        /// The event is triggered when the save button is pressed. It will either fetch the customer (in view mode) or create a new one (add mode).
        /// It will set the properties of the object to the contents of the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e) {
            Customer customer = ((this.customerID.Value != String.Empty) ? 
                                 Customer.Fetch(Convert.ToInt32(this.customerID.Value)) : new Customer());
            
            customer.firstName = this.firstName.Text;
            customer.lastName = this.lastName.Text;
            customer.gender = (Customer.Gender)Enum.ToObject(typeof(Customer.Gender), this.gender.SelectedIndex);
            customer.streetAddress = this.streetAddress.Text;
            customer.billingAddress = this.billingAddress.Text;
            customer.postcode = Convert.ToInt32(this.postcode.Text.ToString());
            customer.Save(); // The save method adds the object to the internal data structure.
            this.customerID.Value = customer.customerID.ToString();
            this.lblMessage.Text = "The record has been saved.";
            this.lblMessage.ForeColor = System.Drawing.Color.Green;
            this.lblMessage.Visible = true;
            this.RefreshCards();
        }

        /// <summary>
        /// The row command event is triggered when actions are done associated with the creditcards DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridCards_RowCommand(object sender, GridViewCommandEventArgs e) {
            // If the action is a delete.
            if (e.CommandName == "cmdDelete") {
                // It will then get the index position of the element.
                int key = Convert.ToInt32(gridCards.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value);
                // It will fetch the associated Customer object.
                Customer customer = Customer.Fetch(Convert.ToInt32(this.customerID.Value));
                if (customer != null) {
                    // It will then remove the card from the list.
                    if (CreditCard.RemoveCard(key)) {
                        // Displays a awesome message letting the user know.
                        this.lblMessage.Text = "A card has been deleted.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Green;
                        this.lblMessage.Visible = true;
                        RefreshCards(); // Reloads the cards.
                    } else {
                        this.lblMessage.Text = "Cannot Delete. Used elsewhere.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Red;
                        this.lblMessage.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// The RefreshCards method initialises the datasource of the DataGridView to point to the creditcards corresponding
        /// to the customer. It binds it to the object list.
        /// </summary>
        private void RefreshCards() {
            // Fetches the Customer object.
            Customer customer = Customer.Fetch(Convert.ToInt32(this.customerID.Value));
            if (customer != null) {
                // Sets the source & binds it to the list.
                this.gridCards.DataSource = customer.creditCards;
                this.gridCards.DataBind();
                // Sets the card holder name to the customer fullname on the textbox.
                this.txtCardHolder.Text = customer.fullName;
                // Sets the card number textbox to empty string.
                this.txtCardNumber.Text = String.Empty;
            }
            // Enables the validation.
            this.rfvCardNumber.Enabled = true;
            this.rfvCardHolder.Enabled = true;
            // Enables the insert button.
            this.btnInsert.Enabled = true;
        }

        /// <summary>
        /// The method checks to see whether the card number is valid.
        /// </summary>
        /// <param name="cardNumber">The card number</param>
        /// <returns>Whether it is valid</returns>
        private Boolean validateCardNumber(String cardNumber) {
            return (cardNumber.All(Char.IsDigit) && cardNumber.Length == 16);
        }

        /// <summary>
        /// The event is triggered when the insert button is clicked. It will instantiate a new CreditCard and add
        /// it to the list corresponding to the customer object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e) {
            // Only proceeds if the cardholder and cardnumber is not empty.
            if (this.txtCardHolder.Text != String.Empty && this.txtCardNumber.Text != String.Empty) {
                // Checks if the card number is valid.
                if (this.validateCardNumber(this.txtCardNumber.Text.Trim())) {
                    // Fetches the customer.
                    Customer customer = Customer.Fetch(Convert.ToInt32(this.customerID.Value));
                    if (customer != null) {
                        // Adds a new CreditCard to the Customer.
                        customer.AddCard(this.txtCardHolder.Text, this.txtCardNumber.Text);
                        // Shows a friendly message.
                        this.lblMessage.Text = "A new card has been added.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Green;
                        this.lblMessage.Visible = true;
                        RefreshCards(); // Refreshes the creditcard DataGridView.
                    }
                }
            }
        }

        /// <summary>
        /// The event is fired when the row goes into edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridCards_RowEditing(object sender, GridViewEditEventArgs e) {
            this.gridCards.EditIndex = e.NewEditIndex; // Sets the Edit index.
            this.RefreshCards(); // Reloads the DataGridView to ensure it is bound.
            // Gets the row to be edited.
            GridViewRow row = this.gridCards.Rows[this.gridCards.EditIndex];
            // Gets the TextBox from the row (which is the CardNumber and sets the MaxLength to 16.
            ((TextBox)(row.Cells[1].Controls[0])).MaxLength = 16;
            // Disables the Insert button.
            this.btnInsert.Enabled = false;
        }

        /// <summary>
        /// The event is fired when the update button is pressed. It will check to see if the values are valid before
        /// making changes to the object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridCards_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            // Gets the row.
            GridViewRow row = this.gridCards.Rows[e.RowIndex];
            // Checks to ensure the textboxes have a value.
            if ((((TextBox)(row.Cells[0].Controls[0])).Text != String.Empty) && (((TextBox)(row.Cells[1].Controls[0])).Text != String.Empty)) {
                // Checks if the CreditCard is actually valid.
                if (this.validateCardNumber(((TextBox)(row.Cells[1].Controls[0])).Text)) {
                    // Gets the card object being edited.
                    CreditCard card = CreditCard.FetchCard(Convert.ToInt32(gridCards.DataKeys[e.RowIndex].Value));
                    // Sets the properties to the data from the textfields.
                    card.cardName = ((TextBox)(row.Cells[0].Controls[0])).Text;
                    card.cardNumber = ((TextBox)(row.Cells[1].Controls[0])).Text;
                    // Updates the card.
                    CreditCard.UpdateCard(card);
                    // Shows a message indicating success.
                    this.lblMessage.Text = "A credit card has been updated.";
                    this.lblMessage.ForeColor = System.Drawing.Color.Green;
                    this.lblMessage.Visible = true;
                    // Gets it out of EditMode.
                    this.gridCards.EditIndex = -1;
                    // Refreshes the cards.
                    this.RefreshCards();
                // Otherwise, shows an error message stating that it must be 16 digits.
                } else {
                    e.Cancel = true;
                    this.lblMessage.Text = "Error: A card must consist of 16 digits.";
                    this.lblMessage.ForeColor = System.Drawing.Color.Red;
                    this.lblMessage.Visible = true;
                }
            // Otherwise, an error message stating that they must be filled in is shown.
            } else {
                this.lblMessage.Text = "Error: Must have a card holder & card number.";
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                this.lblMessage.Visible = true;
                e.Cancel = true; // Keeps it in Edit mode.
            }
        }

        protected void gridCards_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            gridCards.PageIndex = e.NewPageIndex;
            this.RefreshCards();
        }
    }
}