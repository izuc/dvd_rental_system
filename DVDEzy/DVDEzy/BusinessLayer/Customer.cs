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
    /// The Customer class stores data relating a customer. It contains 
    /// their name, address, and credit cards.
    /// Author: Lance Baker (c3128034)
    /// </summary>
    public class Customer {
        private const string SPACE = " ";
        public enum Gender { MALE, FEMALE } // A enumerator for the gender.
        public int customerID { get; private set; } // A unqiue identifier which represents the object.
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string streetAddress { get; set; }
        public string billingAddress { get; set; }
        public Gender gender { get; set; }
        public int postcode { get; set; }

        public Customer() {
        }

        public Customer(string firstName, string lastName, string streetAddress, int postcode, Gender gender)
            : this(firstName, lastName, streetAddress, String.Empty, postcode, gender) {
        }

        public Customer(string firstName, string lastName, string streetAddress, string billingAddress, int postcode, Gender gender)
            : this(0, firstName, lastName, streetAddress, billingAddress, postcode, gender) {

        }

        public Customer(int customerID, string firstName, string lastName, string streetAddress, string billingAddress, int postcode, int gender)
            : this(customerID, firstName, lastName, streetAddress, billingAddress, postcode, (Gender) gender) {
        }

        public Customer(int customerID, string firstName, string lastName, string streetAddress, string billingAddress, int postcode, Gender gender) {
            this.customerID = customerID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.streetAddress = streetAddress;
            this.billingAddress = billingAddress;
            this.gender = gender;
            this.postcode = postcode;
        }

        /// <summary>
        /// The creditCards property will fetch a list of all the credit cards relating to
        /// the customer.
        /// </summary>
        public List<CreditCard> creditCards {
            get {
                return DataCreditCard.List(this.customerID);
            }
        }

        /// <summary>
        /// The AddCard method receives the cardName and cardNumber; 
        /// instantiates a new CreditCard, and adds it into the list.
        /// </summary>
        /// <param name="cardName">The card holder</param>
        /// <param name="cardNumber">The card number</param>
        public void AddCard(string cardName, string cardNumber) {
            DataCreditCard.Add(new CreditCard(this.customerID, cardName, cardNumber));
        }

        /// <summary>
        /// The fullName property concatenates the first & last name together (separated with a space).
        /// </summary>
        public string fullName {
            get {
                return this.firstName + SPACE + this.lastName;
            }
        }

        /// <summary>
        /// The Save method adds the Customer (this) object to the internal data structure.
        /// </summary>
        public void Save() {
            if (this.customerID > 0) {
                DataCustomer.Update(this);
            } else {
                this.customerID = DataCustomer.Add(this);
            }
        }

        /// <summary>
        /// The toString will just show the Customer's full name.
        /// </summary>
        /// <returns>The full name of the customer</returns>
        public override string ToString() {
            return this.fullName;
        }

        /// <summary>
        /// The static Fetch method interacts with the DataLayer 
        /// in order to retrieve a object which corresponds to the ID key received.
        /// </summary>
        /// <param name="id">The ID key of the object</param>
        /// <returns>The object found matching the key.</returns>
        public static Customer Fetch(int id) {
            return DataCustomer.Fetch(id);
        }

        /// <summary>
        /// The static Add method interacts with the DataLayer in order to
        /// add a received object to the internal structure.
        /// </summary>
        /// <param name="transaction">The Customer object that you want to add</param>
        public static void Add(Customer customer) {
            DataCustomer.Add(customer);
        }

        /// <summary>
        /// The static Remove method interacts with the DataLayer in order to
        /// remove a object corresponding to the received ID key.
        /// </summary>
        /// <param name="id">The ID key corresponding to the object that you want removed</param>
        public static Boolean Remove(int id) {
            return DataCustomer.Remove(id);
        }

        /// <summary>
        /// The List method gets the list of objects from the Data Layer.
        /// </summary>
        /// <returns>List of Customer objects</returns>
        public static List<Customer> List() {
            return DataCustomer.List();
        }
    }
}
