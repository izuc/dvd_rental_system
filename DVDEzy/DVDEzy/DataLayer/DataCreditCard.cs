using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVDEzy.BusinessLayer;

namespace DVDEzy.DataLayer {
    /// <summary>
    /// The DataCreditCard class is used as a layer between the CreditCard and the DataSQL class. It contains the SQL statements
    /// (delcared as constants) and the methods which are used for interacting with the database.
    /// Author: Lance Baker (c3128034).
    /// </summary>
    public class DataCreditCard {
        private const string SQL_LIST = "SELECT * FROM CreditCard WHERE customer_id = @CustomerID";
        private const string SQL_FETCH = "SELECT * FROM CreditCard WHERE card_id = @CardID";
        private const string SQL_DELETE = "DELETE FROM CreditCard WHERE card_id = @CardID";
        private const string SQL_INSERT = "INSERT INTO CreditCard(customer_id, card_holder, card_number) OUTPUT INSERTED.card_id VALUES (@CustomerID, @CardHolder, @CardNumber)";
        private const string SQL_UPDATE = "UPDATE CreditCard SET card_holder = @CardHolder, card_number = @CardNumber WHERE card_id = @CardID";


        /// <summary>
        /// The Add method is used to insert the received object into the corresponding database table. 
        /// It coverts the object attributes into a dictionary, which is then passed
        /// into the DataSQL helper method along with the insert sql statement.
        /// </summary>
        /// <param name="card">The credit card object.</param>
        /// <returns>The rows affected.</returns>
        public static int Add(CreditCard card) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CustomerID", card.customerID);
            parameters.Add("@CardHolder", card.cardName);
            parameters.Add("@CardNumber", card.cardNumber);
            return DataSQL<CreditCard>.NonQuery(SQL_INSERT, parameters);
        }

        /// <summary>
        /// The Update method is used to update the record within the database table to the instance
        /// data of the received object. It then coverts the instance data
        /// into a passable dictionary of data, which is then passed to the generic helper method of
        /// the DataSQL class along with the statement. The ID is also added at the end of the dictionary,
        /// since it is the last variable (within the where clause) of the update statement.
        /// </summary>
        /// <param name="card">The credit card object.</param>
        /// <returns>A int indicating the rows affected.</returns>
        public static int Update(CreditCard card) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CardHolder", card.cardName);
            parameters.Add("@CardNumber", card.cardNumber);
            parameters.Add("@CardID", card.cardID);
            return DataSQL<CreditCard>.NonQuery(SQL_UPDATE, parameters);
        }

        /// <summary>
        /// The Fetch method will find the record in the database, and return a newly instantiated object containing
        /// the record data. It uses the generic DataSQL Fetch method to dynamically instantiate the object using Reflection,
        /// which will return the instance.
        /// </summary>
        /// <param name="id">The primary key of the object desired to find.</param>
        /// <returns>The instance object.</returns>
        public static CreditCard Fetch(int id) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CardID", id);
            return DataSQL<CreditCard>.Fetch(SQL_FETCH, parameters);
        }

        /// <summary>
        /// The Remove method is used to delete a record from the corresponding database table. It receives
        /// the primary key, and will use the generic helper method of the DataSQL class to remove the record.
        /// </summary>
        /// <param name="id">The primary key.</param>
        /// <returns>A boolean indicating whether the record was removed.</returns>
        public static Boolean Remove(int id) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CardID", id);
            return (DataSQL<CreditCard>.NonQuery(SQL_DELETE, parameters) > 0);
        }

        /// <summary>
        /// The List method is used to fetch all objects within the corresponding database table. It uses
        /// the generic List method of the DataSQL class to fetch the instances. It uses Reflection to dynamically
        /// instantiate the object based on the generic type. 
        /// </summary>
        /// <param name="id">The Customer primary key.</param>
        /// <returns>Returns a List of the Credit Cards.</returns>
        public static List<CreditCard> List(int customerID) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CustomerID", customerID);
            return DataSQL<CreditCard>.List(SQL_LIST, parameters);
        }        
    }
}