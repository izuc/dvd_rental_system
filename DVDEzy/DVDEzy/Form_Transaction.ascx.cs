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
using System.Collections.Generic;

namespace DVDEzy {
    public partial class Form_Transaction : System.Web.UI.UserControl {
        // The lstRentals dictionary is used to store the added DVDCopy objects on the form. 
        private Dictionary<string, DVDCopy> lstRentals = new Dictionary<string, DVDCopy>();
        // The lstPurchases dictionary is used to store the added DVDCopy objects on the form.
        private Dictionary<string, DVDCopy> lstPurchases = new Dictionary<string, DVDCopy>();

        // Used to add the dictionaries into the ViewState
        protected void Page_PreRender(object sender, EventArgs e) {
            ViewState.Add("lstRentals", this.lstRentals);
            ViewState.Add("lstPurchases", this.lstPurchases);
        }

        // The method is used to initialise the dictionaries with the ViewState data.
        private void initCopyData() {
            if (ViewState["lstRentals"] != null) {
                this.lstRentals = (Dictionary<string, DVDCopy>)ViewState["lstRentals"];
            }
            if (ViewState["lstPurchases"] != null) {
                this.lstPurchases = (Dictionary<string, DVDCopy>)ViewState["lstPurchases"];
            }
        }

        // The page load event is triggered when the page loads.
        protected void Page_Load(object sender, EventArgs e) {
            this.initCopyData(); // Initialises the dictionaries to store the DVDCopies.
            if (!IsPostBack) { // Only does the following if the form isn't posted.
                // Adds the datasource of the customers dropdown list to the Customer List
                this.customers.DataSource = Customer.List();
                this.customers.DataTextField = "fullName"; // The display field is their full name
                this.customers.DataValueField = "customerID"; // The value field is the customer id.
                this.customers.DataBind(); // Binds it to the objects.
                this.customers.Items.Insert(0, new ListItem(String.Empty, String.Empty)); // Inserts a initial blank option.
                this.showCreditCards(); // Invokes the showCreditCards method
                this.loadDVDS(); // Invokes the loadDVDs method
                this.loadListBoxes(); // Loads the listBoxes
                // Sets the remove buttons intially to disabled.
                this.btnRemoveRental.Enabled = false; 
                this.btnRemovePurchase.Enabled = false;
                this.setCardHolder();
            }
        }

        /// <summary>
        /// The following method is used to count the amount of copies that a specific DVD has available.
        /// </summary>
        /// <param name="dvd">The DVD object</param>
        /// <returns>Count of copies</returns>
        private int copiesAvailable(DVD dvd) {
            int count = 0;
            foreach (DVDCopy copy in dvd.copies) {
                if ((copy.isAvailable) && (!this.copyExists(copy))) {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// The loadDVDs method will load the DVD objects from the list into the dropdown box.
        /// It will only add the DVD if there are copies for that available.
        /// </summary>
        private void loadDVDS() {
            this.dvdCopies.Visible = false;
            this.dvds.Items.Clear();
            List<DVD> list = DVD.List();
            foreach (DVD dvd in list) {
                int copies = this.copiesAvailable(dvd);
                if (copies > 0) {
                    this.dvds.Items.Add(new ListItem(dvd.title, dvd.dvdID.ToString()));
                }
            }
            this.dvds.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            this.dvds.SelectedIndex = 0;
        }

        /// <summary>
        /// The copyExists method checks whether a copy already exists within the dictionaries on the form.
        /// </summary>
        /// <param name="copy">The DVDCopy</param>
        /// <returns>A boolean indicating whether it exists</returns>
        private Boolean copyExists(DVDCopy copy) {
            return (this.lstRentals.ContainsKey(copy.barcode) || this.lstPurchases.ContainsKey(copy.barcode));
        }

        /// <summary>
        /// The loadCopies method is used to show another dropdown box containing the DVDCopy objects that belong to the selected DVD.
        /// It will check whether a DVD is selected, and if so; it will add available copies into the dropdown box.
        /// </summary>
        private void loadCopies() {
            // Clears the copies dropdown list.
            this.copies.Items.Clear();
            // If a DVD is selected
            if (this.dvds.SelectedValue != String.Empty) {
                this.dvdCopies.Visible = true; // shows the dropdown box
                // Fetches the DVD object relating to the selected item
                DVD dvd = DVD.Fetch(Convert.ToInt32(this.dvds.SelectedValue.ToString()));
                // Grabs the DVD copies belonging to that object
                List<DVDCopy> list = dvd.copies;
                foreach (DVDCopy copy in list) { // Iterates for each copy
                    // If the copy is available & doesn't already exist on the form
                    if (copy.isAvailable && (!this.copyExists(copy))) {
                        // Adds the copy to the dropdown box.
                        this.copies.Items.Add(new ListItem(copy.barcode, copy.copyID.ToString()));
                    }
                }
                // If the copies dropdown list has no items it will
                // reload the DVDs, which will hide the copies.
                if (this.copies.Items.Count == 0) {
                    this.loadDVDS();
                }
            } else {
                // Otherwise, if no DVD is selected it will hide the copies.
                this.dvdCopies.Visible = false;
            }
        }

        /// <summary>
        /// The loadCopyData method will load the DVDCopy objects, that are stored in the Transaction object, into the 
        /// dictionaries that are on the form. It double checks that the copy doesn't already exist before adding (incase it 
        /// accidently got triggered again).
        /// </summary>
        private void loadCopyData() {
            // The transaction ID is stored in a label.
            if (this.transactionID.Value != String.Empty) {
                // Fetches the transaction object.
                Transaction transaction = Transaction.Fetch(Convert.ToInt32(this.transactionID.Value));
                // Iterates the rentals
                foreach (DVDCopy copy in transaction.rentals) {
                    if (!this.lstRentals.ContainsKey(copy.barcode)) {
                        this.lstRentals.Add(copy.barcode, copy); // Adds the copy to the dictionary
                    }
                }
                // Iterates the purchases
                foreach (DVDCopy copy in transaction.purchases) {
                    if (!this.lstPurchases.ContainsKey(copy.barcode)) {
                        this.lstPurchases.Add(copy.barcode, copy); // Adds the copy to the dictionary
                    }
                }
            }
        }

        /// <summary>
        /// The returnDate method calculates and returns the DataTime relating to the allowable loan length.
        /// Based on the Transaction Date (when the record was created) it will add days to it based on the 
        /// amount of rentals. The reason why it cannot be calculated within the transaction object, is
        /// because the rental copy objects are stored on the form in the view state, and are only saved to the object
        /// once the save button has been pressed.
        /// </summary>
        /// <returns>The calculated due date</returns>
        private DateTime returnDate() {
            DateTime transactionDate;
            if (this.transactionID.Value != String.Empty) {
                // Fetches the transaction object
                Transaction transaction = Transaction.Fetch(Convert.ToInt32(this.transactionID.Value));
                // Gets the transaction date
                transactionDate = transaction.transactionDate;
            } else {
                // If the transaction object doesn't exist (being a new record) then it will create a new date.
                transactionDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            }
            // returns the calculated date.
            return ((this.lstRentals.Count > 14) ? transactionDate.AddDays(14) :
                    (this.lstRentals.Count > 5) ? transactionDate.AddDays(7) :
                     transactionDate.AddDays(2));
        }

        /// <summary>
        /// The loadListBoxes method will clear both the rentals list box & the purchases list box, and just simply add all the items
        /// which are in the DVDCopy dictionaries. It will then invoke the calcTotalCost method, and show/hide the due date depending if there are rentals.
        /// It will also set the due date label to the calculated date.
        /// </summary>
        private void loadListBoxes() {
            this.rentals.Items.Clear(); // Clears the list box.
            // Adds the stored items from the rental dictionary
            foreach (KeyValuePair<String, DVDCopy> entry in this.lstRentals) {
                this.rentals.Items.Add(new ListItem(entry.Value.ToString(), String.Empty));
            }
            this.purchases.Items.Clear(); // Clears the list box
            // Adds the stored items from the purchases dictionary
            foreach (KeyValuePair<String, DVDCopy> entry in this.lstPurchases) {
                this.purchases.Items.Add(new ListItem(entry.Value.ToString(), String.Empty));
            }
            this.calcTotalCost(); // Calculates & displays the total cost.
            this.dueDate.Visible = (this.lstRentals.Count > 0); // Shows or hides the due date.
            // Sets the due date label to the calculated due date (if there are rentals).
            this.lblDueDate.Text = ((this.lstRentals.Count > 0) ? this.returnDate().ToShortDateString() : String.Empty);
        }

        /// <summary>
        /// The showCreditCards method will load the CreditCard objects that belong to a selected Customer into
        /// the credit card dropdown box. It binds the datasource of the dropdown box to the credit card list.
        /// </summary>
        private void showCreditCards() {
            if (this.customers.SelectedIndex > 0) { // If a Customer is selected.
                // It will fetch the Customer object, get the credit card list, and assign it to the DataSource of the dropdown.
                this.creditCards.DataSource = Customer.Fetch(Convert.ToInt32(this.customers.SelectedValue.ToString())).creditCards;
                this.creditCards.DataTextField = "cardNumber"; // The cardNumber is shown for the text.
                this.creditCards.DataBind(); // Binds to the objects.
                this.setCardHolder(); // Sets the cardholder textbox to the card selected.
                this.btnAddCard.Enabled = true; // Enables the add card button.
            } else {
                this.creditCards.Items.Clear(); // Ensures the list is cleared.
                this.cardHolder.Text = String.Empty; // Sets the cardholder textbox to a empty string.
                this.btnAddCard.Enabled = false; // Disables the add card button.
            }
        }

        private int findCardIndex(Transaction transaction) {
            if (transaction.card != null) {
                List<CreditCard> cards = transaction.customer.creditCards;
                for (int i = 0; i < cards.Count; i++) {
                    if (transaction.card.cardNumber.Equals(cards[i].cardNumber)) {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// The ShowRecord method recieves the Transaction object, and sets the fields to the corresponding object data.
        /// </summary>
        /// <param name="transaction">The transaction object that will be shown</param>
        public void ShowRecord(Transaction transaction) {
            if (transaction != null) {
                if (!IsPostBack) {
                    // Sets the transaction id label to the id from the object.
                    this.transactionID.Value = transaction.transactionID.ToString();
                    // Sets the selected customer to correspond to the customer that the transaction was made by.
                    this.customers.SelectedValue = transaction.customer.customerID.ToString();
                    // Sets the payment type.
                    this.paymentType.SelectedIndex = Convert.ToInt32(transaction.paymentType);
                    // Shows the credit cards relating to the customer.
                    this.showCreditCards();
                    // Loads the transaction rentals & purchases into the dictionaries stored on the form.
                    this.loadCopyData();
                    // Loads the list boxes with the rentals & purchases.
                    this.loadListBoxes();
                    // Ensures the credit card option is visible if a credit card is selected as a payment type.
                    this.checkPaymentType();
                    // Sets the selected index of the creditcards downdown list to the one that was used by the transaction.
                    this.creditCards.SelectedIndex = this.findCardIndex(transaction);
                    // Sets the cardholder to display the card name of the selected credit card.
                    this.setCardHolder();
                }
            }
        }

        /// <summary>
        /// The saveCopies method adds the copies that are stored in either the rentals dictionary, or the purchases dictionary
        /// into the transaction object.
        /// </summary>
        /// <param name="transaction">The transaction object</param>
        /// <param name="isRental">A boolean indicating which dictionary list to iterate (true for rentals, false for purchases)</param>
        private void saveCopies(Transaction transaction, Boolean isRental) {
            // Iterates for each copy stored in the dictionary (either being the rentals or purchases).
            foreach (KeyValuePair<String, DVDCopy> entry in ((isRental) ? this.lstRentals : this.lstPurchases)) {
                DVDCopy copy = entry.Value; // Gets the copy object from the KeyValuePair
                // If the copy doesn't already exist in the transaction
                if (!transaction.copyExists(copy)) {
                    transaction.AddItem(copy, (isRental) ? TransactionItem.TransactionType.RENTAL : TransactionItem.TransactionType.SALE);
                    // Sets the availability of the copy to being false, so it can't be rented again.
                    copy.status = DVDCopy.Status.UNAVAILABLE;
                    DVDCopy.UpdateCopy(copy);
                }
            }
        }

        /// <summary>
        /// The discountRate method returns the discount rate that will be applied to the transaction (which is
        /// based on the amount of rentals or purchases that will be made). It fetches the values from the Transaction
        /// class.
        /// </summary>
        /// <returns>The discount rate that will be applied.</returns>
        private double discountRate() {
            return ((this.lstRentals.Count >= 15 || this.lstPurchases.Count >= 15) ?
                    Transaction.DISCOUNT_RATE[3] : (this.lstRentals.Count >= 8 || this.lstPurchases.Count >= 8) ?
                    Transaction.DISCOUNT_RATE[2] : (this.lstRentals.Count >= 5 || this.lstPurchases.Count >= 5) ?
                    Transaction.DISCOUNT_RATE[1] : Transaction.DISCOUNT_RATE[0]);
        }

        /// <summary>
        /// The calcTotalCost method is used to calculate the total cost of all the rentals & purchases. It displays
        /// the result into the corresponding labels, including updating the discount label to show the rate which will be discounted.
        /// </summary>
        private void calcTotalCost() {
            double total = 0;
            // Iterates for each rental stored in the form's rental dictionary.
            foreach (KeyValuePair<String, DVDCopy> entry in this.lstRentals) {
                total += entry.Value.dvd.rentalPrice; // Adds the rental price to the total.
            }
            // Iterates for each purchase stored in the form's purchase dictionary.
            foreach (KeyValuePair<String, DVDCopy> entry in this.lstPurchases) {
                total += entry.Value.dvd.salePrice; // Adds the purchase price to the total.
            }
            // Calculates and formats the discount rate, and shows it in the label.
            this.lblDiscount.Text = String.Format("{0}%", (this.discountRate() * 100));
            // Displays the total cost, minus the discount.
            this.lblTotalCost.Text = String.Format("${0: #,#.00}", (total - (total * this.discountRate())).ToString());
        }

        /// <summary>
        /// The event is triggered when the remove rental button is pressed. It will remove a rental from the dictionary, corresponding
        /// to the selected index. It will however have to first find the barcode of the selected item inorder to remove the damn thing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveRental_Click(object sender, EventArgs e) {
            if (this.rentals.SelectedIndex > -1) { // Only does it if the rental item is selected in the listbox.
                // Removes the rental from the rental dictionary.
                this.lstRentals.Remove(this.lstRentals.ElementAt(this.rentals.SelectedIndex).Key);
                this.loadListBoxes(); // Reloads the listbox to reflect the change.
                this.loadDVDS(); // Reloads the DVDs since one title may be available again (since it will now have a additional copy).
            }
        }

        /// <summary>
        /// The event is triggered when the remove purchase button is pressed. It will remove a purchase from the dictionary, corresponding
        /// to the selected index. It will however have to first find the barcode of the selected item inorder to remove the damn thing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemovePurchase_Click(object sender, EventArgs e) {
            if (this.purchases.SelectedIndex > -1) { // Only does it if the purchase item is selected in the listbox.
                // Removes the purchase from the rental dictionary.
                this.lstPurchases.Remove(this.lstPurchases.ElementAt(this.purchases.SelectedIndex).Key);
                this.loadListBoxes(); // Reloads the listbox to reflect the change.
                this.loadDVDS(); // Reloads the DVDs since one title may be available again (since it will now have a additional copy).
            }
        }

        /// <summary>
        /// The clearCopies method will clear the copies contained within a transaction object. It will
        /// reset the availability of the copies back to true, before clearing the lists.
        /// </summary>
        /// <param name="transaction">The transaction object</param>
        private void clearCopies(Transaction transaction) {
            if (transaction.transactionID > 0) {
                TransactionItem.UpdateCopyStatus(transaction.transactionID, DVDCopy.Status.AVAILABLE);
                TransactionItem.Clear(transaction.transactionID);
            }
        }
        
        /// <summary>
        /// The event is triggered when the save button is pressed. 
        /// It will either made the changes to a existing transaction object, or it will create a new one altogether.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e) {
            // Will only save if the rentals count or purchases count is greater than 0.
            if ((this.lstRentals.Count > 0) || this.lstPurchases.Count > 0) {
                // It will either fetch a the transaction object corresponding to the transaction id label or it will instantiate a new object.
                Transaction transaction = ((this.transactionID.Value != String.Empty) ?
                                     Transaction.Fetch(Convert.ToInt32(this.transactionID.Value)) : new Transaction());
                // Sets customer to the one selected on the form.
                transaction.customer = Customer.Fetch(Convert.ToInt32(this.customers.SelectedValue.ToString()));
                // Sets the payment type to the one selected on the form.
                transaction.paymentType = (Transaction.PaymentType)Enum.ToObject(typeof(Transaction.PaymentType), this.paymentType.SelectedIndex);
                // Sets the transaction card to the one selected on the form, or if not selected it will be set to null.
                transaction.card = ((this.paymentType.SelectedIndex > 0) && (this.creditCards.SelectedIndex > -1)) ?
                    transaction.customer.creditCards[this.creditCards.SelectedIndex] : null;
                // Clears the copies on the transaction.
                this.clearCopies(transaction);
                // Invokes the save operation on the object to ensure it is added to the internal data structure.
                if (transaction.Save()) {
                    // Sets the transaction id label to the transaction object's id.
                    this.transactionID.Value = transaction.transactionID.ToString();
                    // Saves the copies for both the rentals & purchases.
                    this.saveCopies(transaction, true);
                    this.saveCopies(transaction, false);
                    // A friendly message indicating it has been saved.
                    this.lblMessage.Text = "The record has been saved.";
                    this.lblMessage.ForeColor = System.Drawing.Color.Green;
                    this.lblMessage.Visible = true;
                    // Sets the creditcards option visibility to true if the credit card payment type is selected.
                    this.checkPaymentType();
                    // Reloads the list boxes.
                    this.loadListBoxes();
                    // Reloads the DVDs.
                    this.loadDVDS();
                } else {
                    this.lblMessage.Text = "Error: Something wrong. Didn't Save.";
                    this.lblMessage.ForeColor = System.Drawing.Color.Red;
                    this.lblMessage.Visible = true;
                }
            } else {
                // A error message is shown if there aren't any rentals or purchases added.
                this.lblMessage.Text = "Error: Must have some Rentals & Sales.";
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                this.lblMessage.Visible = true;
            }
        }

        /// <summary>
        /// The addCopy method is used to add a DVDCopy object that was selected in the DVDCopy dropdown box to
        /// either the rentals or purchases dictionary (depending on the received boolean). It will then reload
        /// the copies dropdown box to reflect the changes. 
        /// </summary>
        /// <param name="isRental"></param>
        private void addCopy(Boolean isRental) {
            // If a copy is selected.
            if (this.copies.SelectedValue != String.Empty) {
                // It will fetch the DVD that the copy belongs to.
                DVD dvd = DVD.Fetch(Convert.ToInt32(this.dvds.SelectedValue.ToString()));
                // It will then fetch the copy from the DVD (by passing in the barcode).
                DVDCopy copy = DVDCopy.FetchCopy(Convert.ToInt32(this.copies.SelectedItem.Value));
                // If the copy doesn't already exist in both dictionaries
                if (!(this.lstRentals.ContainsKey(this.copies.SelectedItem.Text) ||
                    this.lstPurchases.ContainsKey(this.copies.SelectedItem.Text))) {
                    // Then it will add it to either the rentals or purchases
                    if (isRental) {
                        this.lstRentals.Add(copy.barcode, copy);
                    } else {
                        this.lstPurchases.Add(copy.barcode, copy);
                    }
                }
                // Reloads the copies.
                this.loadCopies();
            }
            // Reshows the creditcard payment option.
            this.checkPaymentType();
            // Loads the list boxes (with the new copy included).
            this.loadListBoxes();
        }

        /// <summary>
        /// The event is triggered when the rental button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRental_Click(object sender, EventArgs e) {
            this.addCopy(true); // Invokes addCopy passing in true to indicate that it is to be added to the rentals.
        }

        /// <summary>
        /// The event is triggered when the sale button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSale_Click(object sender, EventArgs e) {
            this.addCopy(false); // Invokes addCopy passing in false to indicate that it is to be added to the purchases.
        }

        /// <summary>
        /// The event is triggered when the add card button is clicked. 
        /// It will show a form to add a new credit card & hide the one to select a credit card.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddCard_Click(object sender, EventArgs e) {
            this.viewCreditCards.Visible = false; // Hides selecting card.
            this.addCreditCard.Visible = true; // To show the form to add a card.
            Customer customer = Customer.Fetch(Convert.ToInt32(this.customers.SelectedValue.ToString())); // Gets the customer.
            this.txtCardHolder.Text = customer.fullName; // Sets the card holder textbox to their fullname.
        }

        /// <summary>
        /// The event is triggered when the save card button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveCard_Click(object sender, EventArgs e) {
            // Checks whether it is filled in.
            if ((this.txtCardHolder.Text != String.Empty) && (this.txtCreditCard.Text != String.Empty)) {
                // A check to validate the received card number. It ensures the card number is digits, and has a length of 16 characters.
                if (this.txtCreditCard.Text.All(Char.IsDigit) && this.txtCreditCard.Text.Length == 16) {
                    // Fetches the Customer object.
                    Customer customer = Customer.Fetch(Convert.ToInt32(this.customers.SelectedValue.ToString()));
                    // Adds the card to the customer.
                    customer.AddCard(this.txtCardHolder.Text, this.txtCreditCard.Text);
                    // Sets the textboxes to being empty.
                    this.txtCardHolder.Text = String.Empty;
                    this.txtCreditCard.Text = String.Empty;
                    // Hides the add card form
                    this.addCreditCard.Visible = false;
                    // Shows the view cards again.
                    this.viewCreditCards.Visible = true;
                    // Reloads the creditcards into the dropdown box.
                    this.showCreditCards();
                    // Sets the selected index to the new card.
                    this.creditCards.SelectedIndex = (customer.creditCards.Count - 1);
                    // Sets the cardholder text to the card name.
                    this.cardHolder.Text = (customer.creditCards[(customer.creditCards.Count - 1)].cardName);
                // The card is invalid.
                } else {
                    this.lblMessage.Text = "Error: A card must consist of 16 digits.";
                    this.lblMessage.ForeColor = System.Drawing.Color.Red;
                    this.lblMessage.Visible = true;
                }
            // Otherwise shows an error stating it must be entered.
            } else {
                this.lblMessage.Text = "Error: Must have a card holder & card number.";
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                this.lblMessage.Visible = true;
            }
        }

        /// <summary>
        /// A button which cancels the add card form. It reshows the view cards section.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelCard_Click(object sender, EventArgs e) {
            this.txtCardHolder.Text = String.Empty;
            this.txtCreditCard.Text = String.Empty;
            this.addCreditCard.Visible = false;
            this.viewCreditCards.Visible = true;
        }

        /// <summary>
        /// The event when a customer is selected. It will show the creditcards relating to the customer, and also
        /// set the selected index to -1. It then checks whether the credit card payment type is selected; showing & hiding the section as appropriate.
        /// It then revalidates the page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void customers_SelectedIndexChange(object sender, EventArgs e) {
            this.showCreditCards();
            this.creditCards.SelectedIndex = -1;
            this.checkPaymentType();
            Page.Validate(); // Causes the validation on the form to be checked.
        }

        /// <summary>
        /// The event when a DVD is selected. It will load the corresponding copies. It then checks whether the credit card payment type is selected; 
        /// showing & hiding the section as appropriate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dvds_SelectedIndexChange(object sender, EventArgs e) {
            this.loadCopies();
            this.checkPaymentType();
        }

        /// <summary>
        /// The event when a payment type is selected. It checks whether the credit card payment type is selected; 
        /// showing & hiding the section as appropriate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void paymentType_SelectedIndexChange(object sender, EventArgs e) {
            this.checkPaymentType();
        }

        /// <summary>
        /// The setCardHolder method sets the readonly cardHolder textbox to contain the selected creditcard's cardholder name.
        /// </summary>
        private void setCardHolder() {
            if (this.customers.SelectedIndex > -1 && this.creditCards.SelectedIndex > -1) {
                this.cardHolder.Text = Customer.Fetch(Convert.ToInt32(this.customers.SelectedValue.ToString())).creditCards[this.creditCards.SelectedIndex].cardName;
            } else {
                this.cardHolder.Text = String.Empty;
            }
        }

        /// <summary>
        /// The event when a credit card is selected. It will set the card holder name. And also check the payment type (showing/ hiding the creditcard options).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void creditCard_SelectedIndexChange(object sender, EventArgs e) {
            this.setCardHolder();
            this.checkPaymentType();
        }

        /// <summary>
        /// The checkPaymentType shows or hides the creditcard options depending on the payment type.
        /// </summary>
        private void checkPaymentType() {
            this.optionCreditCard.Visible = (this.paymentType.SelectedIndex > 0);
        }
    }
}