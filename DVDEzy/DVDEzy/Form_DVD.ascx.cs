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
    public partial class Form_DVD : System.Web.UI.UserControl {
		/// <summary>
        /// The page load event first disables the validation on the barcode input textbox
        /// which is reactivated when adding a DVDCopy (in order to avoid conflict with the main form's validation controls).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
			// Disables the input textboxes used for adding a DVDCopy. It
			// allows the main form to be submitted within conflict. The validation
			// is reactivated when needed.
            this.rfvBarcode.Enabled = false;
            this.revBarcode.Enabled = false;
        }

		/// <summary>
        /// The method ShowRecord is used to display the object data into the form.
        /// </summary>
        /// <param name="dvd">The DVD object</param>
        public void ShowRecord(DVD dvd) {
            if (dvd != null) {
                if (!Page.IsPostBack) { // Only does the following when the page isn't POSTED.
                    this.dvdID.Value = dvd.dvdID.ToString(); // Sets the DVD ID Label with the DVD object's ID.
                    this.title.Text = dvd.title; // Sets the title textbox with the DVD object's title.
                    this.director.Text = dvd.director; // Sets the director textbox with the DVD object's director.
                    this.yearReleased.Text = dvd.yearReleased.ToString(); // Sets the year released textbox with the DVD object's year.
                    this.salePrice.Text = dvd.salePrice.ToString(); // Sets the sale price textbox with the DVD object's sale price.
                    this.rentalPrice.Text = dvd.rentalPrice.ToString(); // Sets the sale price textbox with the DVD object's rental price.
                    this.gridCopies.DataSource = dvd.copies; // Sets the datasource of the DVDCopy DataGridView to the object's DVDCopy list.
                    this.gridCopies.DataBind(); // Binds the objects to the DataGridView.
                }
            }
        }

		/// <summary>
        /// The method checks to see whether the barcode is valid.
        /// </summary>
        /// <param name="cardNumber">The barcode</param>
        /// <returns>Whether it is valid</returns>
        private Boolean validateBarcode(String barcode) {
            return (barcode.All(Char.IsDigit) && barcode.Length == 5);
        }

		/// <summary>
        /// The event is triggered when the insert button is clicked. It will instantiate a new DVDCopy and add
        /// it to the list corresponding to the DVD object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e) {
			// Checks whether the barcode is not empty, and has a valid barcode.
            if (this.txtBarcode.Text != String.Empty && this.validateBarcode(this.txtBarcode.Text)) {
				// Fetches the current DVD object.
                DVD dvd = DVD.Fetch(Convert.ToInt32(this.dvdID.Value));
                if (dvd != null) { // If the DVD object exists.
					// It will instantiate a new DVDCopy object based on the insert record fields. It will
					// then add the object into the copies list contained inside the DVD object.
                    //dvd.copies.Add(new DVDCopy(dvd, this.txtBarcode.Text, this.chkAvailability.Checked));
                    if (dvd.AddCopy(this.txtBarcode.Text, ((this.chkAvailability.Checked) ? DVDCopy.Status.AVAILABLE : DVDCopy.Status.UNAVAILABLE))) {
                        // Shows a friendly message letting them know it has been added.
                        this.lblMessage.Text = "A new copy has been added.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Green;
                        this.lblMessage.Visible = true;
                        // Reloads the DVDCopy DataGridView to reflect the changes.
                        RefreshCopies();
                    } else {
                        // Shows a message stating the barcode is invalid.
                        this.lblMessage.Text = "Barcode already exists.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Red;
                        this.lblMessage.Visible = true;
                    }
                }
            }
			// Sets the input barcode textbox to empty.
            this.txtBarcode.Text = String.Empty;
			// Sets the input availability checkbox back to true.
            this.chkAvailability.Checked = true;
        }

		/// <summary>
        /// The event is triggered when the save button is pressed. It will either fetch the DVD (in view mode) or create a new one (add mode).
        /// It will set the properties of the object to the contents of the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e) {
			// Fetches or creates a new DVD object.
            DVD dvd = ((this.dvdID.Value != String.Empty) ?
                                 DVD.Fetch(Convert.ToInt32(this.dvdID.Value)) : new DVD());
			// Sets the properties to the contents of the form.
            dvd.title = this.title.Text;
            dvd.director = this.director.Text;
            dvd.yearReleased = Convert.ToInt32(this.yearReleased.Text.ToString());
            dvd.salePrice = Convert.ToDouble(this.salePrice.Text.ToString());
            dvd.rentalPrice = Convert.ToDouble(this.rentalPrice.Text.ToString());
            dvd.Save(); // Saves the object. It will ensure it has been added to the list.
            this.dvdID.Value = dvd.dvdID.ToString();
			// Shows a friendly message stating it has been saved.
            this.lblMessage.Text = "The record has been saved.";
            this.lblMessage.ForeColor = System.Drawing.Color.Green;
            this.lblMessage.Visible = true;
			// Refreshes the DVDCopy DataGridView.
            this.RefreshCopies();
            this.chkAvailability.Checked = true;
        }

		/// <summary>
        /// The row command event is triggered when actions are done associated with the copies DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridCopies_RowCommand(object sender, GridViewCommandEventArgs e) {
            // If the action is a delete
			if (e.CommandName == "cmdDelete") {
				// It will then get the index position of the element.
                int key = Convert.ToInt32(gridCopies.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value);
				// It will fetch the associated DVD object.
                if (DVDCopy.RemoveCopy(key)) {
                    // Displays a awesome message letting the user know.
                    this.lblMessage.Text = "A copy has been removed.";
                    this.lblMessage.ForeColor = System.Drawing.Color.Green;
                    this.lblMessage.Visible = true;
                    this.RefreshCopies(); // Reloads the copies.
                } else {
                    this.lblMessage.Text = "Cannot Delete. Used elsewhere.";
                    this.lblMessage.ForeColor = System.Drawing.Color.Red;
                    this.lblMessage.Visible = true;
                }
            }
        }

		/// <summary>
        /// The RefreshCards method initialises the datasource of the DataGridView to point to the copies list contained
        /// in the viewed DVD object. It binds it to the object list.
        /// </summary>
        private void RefreshCopies() {
			// Fetches the DVD object
            DVD dvd = DVD.Fetch(Convert.ToInt32(this.dvdID.Value));
            if (dvd != null) {
				// Sets the source & binds it to the list.
                this.gridCopies.DataSource = dvd.copies;
                this.gridCopies.DataBind();
            }
        }

		/// <summary>
        /// The event is fired when the row goes into edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridCopies_RowEditing(object sender, GridViewEditEventArgs e) {
            this.gridCopies.EditIndex = e.NewEditIndex;  // Sets the Edit index.
            this.RefreshCopies(); // Reloads the DataGridView to ensure it is bound.
			// Gets the row to be edited.
            GridViewRow row = this.gridCopies.Rows[this.gridCopies.EditIndex];
			// Gets the TextBox from the row (which is the Barcode and sets the MaxLength to 5.
            ((TextBox)(row.Cells[0].Controls[0])).MaxLength = 5; 
			// Disables the Insert button.
            this.btnInsert.Enabled = false;
        }

		/// <summary>
        /// The event is fired when the update button is pressed. It will check to see if the values are valid before
        /// making changes to the object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridCopies_RowUpdating(object sender, GridViewUpdateEventArgs e) {
			// Gets the row.
            GridViewRow row = gridCopies.Rows[e.RowIndex];
			// Gets the barcode textbox from the row.
            TextBox txtBarcode = (TextBox)(row.Cells[0].Controls[0]);
			// Checks to see whether the barcode entered is valid.
            if (this.validateBarcode(txtBarcode.Text)) {
				// Then get the relating DVDCopy from the DVD based on the Row index.
                DVDCopy copy = DVDCopy.FetchCopy(Convert.ToInt32(gridCopies.DataKeys[e.RowIndex].Value));
				// Sets the barcode property of the DVDCopy to the value in the textbox.
                copy.barcode = ((TextBox)(row.Cells[0].Controls[0])).Text;
				// Sets the isAvailable property of the DVDCopy to the value from the checkbox.
                copy.status = (((CheckBox)(row.Cells[1].Controls[0])).Checked) ? DVDCopy.Status.AVAILABLE : DVDCopy.Status.UNAVAILABLE;
                if (DVDCopy.UpdateCopy(copy)) {
                    // Shows a friendly message stating the copy has been updated.
                    this.lblMessage.Text = "A copy has been updated.";
                    this.lblMessage.ForeColor = System.Drawing.Color.Green;
                    this.lblMessage.Visible = true;
                    this.gridCopies.EditIndex = -1; // Ends the EditMode.
                    this.RefreshCopies(); // Refreshes the DVDCopy DataGridView to reflect the changes.
                    this.btnInsert.Enabled = true; // Enables the Insert button again.
                } else {
                    // Shows a message stating the barcode is invalid.
                    this.lblMessage.Text = "Barcode already exists.";
                    this.lblMessage.ForeColor = System.Drawing.Color.Red;
                    this.lblMessage.Visible = true;
                }
            } else {
				// Shows a message stating the barcode is invalid.
                this.lblMessage.Text = "A barcode must be 5 digits.";
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                this.lblMessage.Visible = true;
				e.Cancel = true; // Keeps it in Edit mode.
            }
        }

        protected void gridCopies_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            gridCopies.PageIndex = e.NewPageIndex;
            this.RefreshCopies();
        }

    }
}