using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using DVDEzy.DataLayer;

namespace DVDEzy.BusinessLayer {
    /// <summary>
    /// The Transaction object represents a transaction that is made by the customer, with each
    /// transaction containing the Customer, the transaction date, the rentals, the purchases, and the payment type. The relating credit
    /// card (if used) is also stored. The total cost, discount rate, and due date; are all
    /// calculated properties. The transaction contains a list of DVDCopy for both the rentals and purchases made, 
    /// which both contain references to the copies stored in a DVD.
    /// Author: Lance Baker (c3128034)
    /// </summary>
    public class Transaction {
        public enum PaymentType { Cash, Visa, Mastercard }; // A enumerated type for the payment method.
        public static readonly double[] DISCOUNT_RATE = { 0, 0.05, 0.10, 0.15 }; // A static readonly array containing the discount rate percentages.
        public int transactionID { get; set; } // A unqiue identifier which represents the object.
        public Customer customer { get; set; } // The customer the transaction is for.
        public CreditCard card { get; set; } // The credit card used (which is a reference).
        public PaymentType paymentType { get; set; } // The payment type.
        public DateTime transactionDate { get; set; } // The transaction date.

        public Transaction() {
            this.transactionDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        }

        public Transaction(int transactionID, int customerID, int paymentType, int? cardID, DateTime transactionDate)
            : this(transactionID, Customer.Fetch(customerID), (PaymentType)paymentType, ((cardID != null) ? CreditCard.FetchCard(cardID.Value) : null), transactionDate) {   
        }

        public Transaction(Customer customer, PaymentType paymentType, CreditCard card)
            : this(0, customer, paymentType, card, Convert.ToDateTime(DateTime.Now.ToShortDateString())) {
        }

        public Transaction(int transactionID, Customer customer, PaymentType paymentType, CreditCard card, DateTime transactionDate) {
            this.transactionID = transactionID;
            this.customer = customer;
            this.paymentType = paymentType;
            this.card = card;
            this.transactionDate = transactionDate;
        }

        public List<DVDCopy> rentals {
            get {
                return DataTransactionItem.List(this.transactionID, TransactionItem.TransactionType.RENTAL);
            }
        }

        public List<DVDCopy> purchases {
            get {
                return DataTransactionItem.List(this.transactionID, TransactionItem.TransactionType.SALE);
            }
        }

        /// <summary>
        /// The quantityRented property returns the count of the rentals list.
        /// </summary>
        public int quantityRented {
            get { 
                return DataTransactionItem.Count(this.transactionID, TransactionItem.TransactionType.RENTAL);
            }
        }

        /// <summary>
        /// The quantityPurchased property returns the count of the purchases list.
        /// </summary>
        public int quantityPurchased {
            get { 
                return DataTransactionItem.Count(this.transactionID, TransactionItem.TransactionType.SALE); 
            }
        }

        /// <summary>
        /// The discountRate property is used to fetch a discount rate percentage (stored in the readonly array)
        /// based on the amount of rentals & purchases made.
        /// </summary>
        public double discountRate {
            get {
                int quantityRented = this.quantityRented;
                int quantityPurchased = this.quantityPurchased;
                return ((quantityRented >= 15 || quantityPurchased >= 15) ?
                    DISCOUNT_RATE[3] : (quantityRented >= 8 || quantityPurchased >= 8) ?
                    DISCOUNT_RATE[2] : (quantityRented >= 5 || quantityPurchased >= 5) ?
                    DISCOUNT_RATE[1] : DISCOUNT_RATE[0]);
            }
        }
      
        /// <summary>
        /// The totalCost property iterates throughout the DVDCopy objects stored in both lists; adding
        /// the rental price/ or the sale price to the total. It then returns the total minus the discount rate.
        /// </summary>
        public double totalCost {
            get {
                double total = 0;
                foreach (DVDCopy copy in this.rentals) {
                    total += copy.dvd.rentalPrice;
                }
                foreach (DVDCopy copy in this.purchases) {
                    total += copy.dvd.salePrice;
                }
                return (total - (total * this.discountRate));
            }
        }

        /// <summary>
        /// The dueDate property returns a DateTime object based on the transaction date (as the starting date) 
        /// with days added. The allowable loan time depends on the rental count; higher the count having
        /// an increased duration.
        /// </summary>
        public DateTime dueDate {
            get {
                int quantityRented = this.quantityRented;
                return ((quantityRented > 14) ? this.transactionDate.AddDays(14) :
                    (quantityRented > 5) ? this.transactionDate.AddDays(7) :
                     this.transactionDate.AddDays(2));
            }
        }

        /// <summary>
        /// The copyExists method receives a DVDCopy object and checks whether the copy exists
        /// in both of the lists. It matches the barcodes together and if it finds a match it will return true, otherwise false;
        /// </summary>
        /// <param name="copy">The DVDCopy that you want to check if it exists</param>
        /// <returns>A boolean indicating whether it was found</returns>
        public Boolean copyExists(DVDCopy copy) {
            return (DataTransactionItem.Fetch(this.transactionID, copy.copyID) != null);
        }


        public void AddItem(DVDCopy copy, TransactionItem.TransactionType type) {
            DataTransactionItem.Add(new TransactionItem(this.transactionID, copy.copyID, type));
        }

        /// <summary>
        /// The Save method first checks whether the current object exists already in the 
        /// internal data structure, if not it will add the object.
        /// </summary>
        public Boolean Save() {
            if (this.transactionID > 0) {
                return (DataTransaction.Update(this) > 0);
            } else {
                this.transactionID = DataTransaction.Add(this);
                return (this.transactionID > 0);
            }
        }

        /// <summary>
        /// The static Fetch method interacts with the DataLayer 
        /// in order to retrieve a object which corresponds to the ID key received.
        /// </summary>
        /// <param name="id">The ID key of the object</param>
        /// <returns>The object found matching the key.</returns>
        public static Transaction Fetch(int id) {
            return DataTransaction.Fetch(id);
        }

        /// <summary>
        /// The static Add method interacts with the DataLayer in order to
        /// add a received object to the internal structure.
        /// </summary>
        /// <param name="transaction">The transaction object that you want to add</param>
        public static void Add(Transaction transaction) {
            DataTransaction.Add(transaction);
        }

        /// <summary>
        /// The static Remove method interacts with the DataLayer in order to
        /// remove a object corresponding to the received ID key. It first however,
        /// will set the availability on the DVDCopy objects back to true; allowing
        /// for the copy to be re-rented/or purchased.
        /// </summary>
        /// <param name="id">The ID key corresponding to the object that you want removed</param>
        public static Boolean Remove(int id) {
            TransactionItem.UpdateCopyStatus(id, DVDCopy.Status.AVAILABLE);
            TransactionItem.Clear(id);
            return DataTransaction.Remove(id);
        }

        /// <summary>
        /// The List method gets the list of objects from the Data Layer.
        /// </summary>
        /// <returns>List of Transaction objects</returns>
        public static List<Transaction> List() {
            return DataTransaction.List();
        }

        /// <summary>
        /// The List method gets the list of objects from the Data Layer.
        /// </summary>
        /// <returns>List of Transaction objects</returns>
        public static List<Transaction> List(int customerID) {
            return DataTransaction.List(customerID);
        }
    }
}
