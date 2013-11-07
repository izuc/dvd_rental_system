

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
using DVDEzy.DataLayer;

namespace DVDEzy.BusinessLayer {
    /// <summary>
    /// The CreditCard class is used by the Customer. 
    /// It is just a simple class which stores the card's holder name & card number.
    /// Author: Lance Baker (c3128034)
    /// </summary>
    public class CreditCard {
        public int cardID { get; set; }
        public int customerID { get; set; }
        public string cardName { get; set; }
        public string cardNumber { get; set; }

        public CreditCard(int customerID, string cardName, string cardNumber)
            : this(0, customerID, cardName, cardNumber) {
        }

        public CreditCard(int cardID, int customerID, string cardName, string cardNumber) {
            this.cardID = cardID;
            this.customerID = customerID;
            this.cardName = cardName;
            this.cardNumber = cardNumber;
        }

        /// <summary>
        /// The UpdateCard method is a static method, which is used to interact with the data layer.
        /// It will update the record relating to the card object received.
        /// </summary>
        /// <param name="card">The credit card object</param>
        public static void UpdateCard(CreditCard card) {
            DataCreditCard.Update(card);
        }

        /// <summary>
        /// The FetchCard method is a static method, which is used to interact with the data layer.
        /// It will return the card instance based on the received ID.
        /// </summary>
        /// <param name="id">The Card primary key.</param>
        /// <returns>The CreditCard instance</returns>
        public static CreditCard FetchCard(int id) {
            return DataCreditCard.Fetch(id);
        }

        /// <summary>
        /// The RemoveCard method is a static method, which is used to interact with the data layer.
        /// It will remove a card relating to the received ID.
        /// </summary>
        /// <param name="id">The Card primary key.</param>
        /// <returns>A boolean indicating success.</returns>
        public static Boolean RemoveCard(int id) {
            return DataCreditCard.Remove(id);
        }
    }
}
