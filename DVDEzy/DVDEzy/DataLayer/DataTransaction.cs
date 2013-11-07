using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVDEzy.BusinessLayer;

namespace DVDEzy.DataLayer {
    /// <summary>
    /// The DataTransaction class is used as a layer between the Transaction and the DataSQL class. It contains the SQL statements
    /// (delcared as constants) and the methods which are used for interacting with the database.
    /// Author: Lance Baker (c3128034).
    /// </summary>
    public class DataTransaction {
        private const string SQL_LIST = "SELECT * FROM TransactionCart Order By transaction_date DESC";
        private const string SQL_LIST_CUSTOMER = "SELECT * FROM TransactionCart WHERE customer_id = @CustomerID Order By transaction_date DESC";
        private const string SQL_FETCH = "SELECT * FROM TransactionCart WHERE transaction_id = @TransactionID";
        private const string SQL_INSERT = "INSERT INTO TransactionCart (customer_id, payment_type, card_id, transaction_date) OUTPUT INSERTED.transaction_id VALUES (@CustomerID, @PaymentType, @CardID, @TransactionDate)";
        private const string SQL_DELETE = "DELETE FROM TransactionCart WHERE transaction_id = @TransactionID";
        private const string SQL_UPDATE = "UPDATE TransactionCart SET customer_id = @CustomerID, payment_type = @PaymentType, card_id = @CardID, transaction_date = @TransactionDate WHERE transaction_id = @TransactionID";

        /// <summary>
        /// The AddParameters method is used to convert the instance data of the received object
        /// into a dictionary (with the key corresponding to the data name used in the query).
        /// </summary>
        /// <param name="customer">The object to be converted.</param>
        /// <returns>The instance data as a dictionary.</returns>
        private static Dictionary<string, object> AddParameters(Transaction transaction) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CustomerID", transaction.customer.customerID);
            parameters.Add("@PaymentType", (int)transaction.paymentType);
            parameters.Add("@CardID", ((transaction.card != null) ? (int?)transaction.card.cardID : null));
            parameters.Add("@TransactionDate", transaction.transactionDate);
            return parameters;
        }

        /// <summary>
        /// The Add method is used to insert the received object into the corresponding database table. It uses the
        /// AddParameters method to covert the object attributes into a dictionary, which is then passed
        /// into the DataSQL helper method along with the insert sql statement.
        /// </summary>
        /// <param name="transaction">The transaction object.</param>
        /// <returns>The inserted record's primary key.</returns>
        public static int Add(Transaction transaction) {
            return DataSQL<Transaction>.ScalarQuery(SQL_INSERT, AddParameters(transaction));
        }

        /// <summary>
        /// The Update method is used to update the record within the database table to the instance
        /// data of the received object. The AddParameters method is used to covert the instance data
        /// into a passable dictionary of data, which is then passed to the generic helper method of
        /// the DataSQL class along with the statement. The ID is also added at the end of the dictionary,
        /// since it is the last variable (within the where clause) of the update statement.
        /// </summary>
        /// <param name="customer">The customer object.</param>
        /// <returns>A int indicating the rows affected.</returns>
        public static int Update(Transaction transaction) {
            Dictionary<string, object> parameters = AddParameters(transaction);
            parameters.Add("@TransactionID", transaction.transactionID);
            return DataSQL<Transaction>.NonQuery(SQL_UPDATE, parameters);
        }

        /// <summary>
        /// The Fetch method will find the record in the database, and return a newly instantiated object containing
        /// the record data. It uses the generic DataSQL Fetch method to dynamically instantiate the object using Reflection,
        /// which will return the instance.
        /// </summary>
        /// <param name="id">The primary key of the object desired to find.</param>
        /// <returns>The instance object.</returns>
        public static Transaction Fetch(int id) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TransactionID", id);
            return DataSQL<Transaction>.Fetch(SQL_FETCH, parameters);
        }

        /// <summary>
        /// The Remove method is used to delete a record from the corresponding database table. It receives
        /// the primary key, and will use the generic helper method of the DataSQL class to remove the record.
        /// </summary>
        /// <param name="id">The primary key.</param>
        /// <returns>A boolean indicating whether the record was removed.</returns>
        public static Boolean Remove(int id) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TransactionID", id);
            return (DataSQL<Transaction>.NonQuery(SQL_DELETE, parameters) > 0);
        }

        /// <summary>
        /// The List method is used to fetch all objects within the corresponding database table. It uses
        /// the generic List method of the DataSQL class to fetch the instances. It uses Reflection to dynamically
        /// instantiate the object based on the generic type. 
        /// </summary>
        /// <returns>Returns a List of the objects.</returns>
        public static List<Transaction> List() {
            return DataSQL<Transaction>.List(SQL_LIST, null);
        }

        /// <summary>
        /// The List method is used to fetch all objects within the corresponding database table. It uses
        /// the generic List method of the DataSQL class to fetch the instances. It uses Reflection to dynamically
        /// instantiate the object based on the generic type. 
        /// </summary>
        /// <param name="id">The Customer primary key.</param>
        /// <returns>Returns a List of the transactions relating to a Customer.</returns>
        public static List<Transaction> List(int customerID) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CustomerID", customerID);
            return DataSQL<Transaction>.List(SQL_LIST_CUSTOMER, parameters);
        }
    }
}