using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVDEzy.BusinessLayer;

namespace DVDEzy.DataLayer {
    /// <summary>
    /// The DataTransactionItem class is used as a layer between the TransactionItem and the DataSQL class. It contains the SQL statements
    /// (delcared as constants) and the methods which are used for interacting with the database.
    /// Author: Lance Baker (c3128034).
    /// </summary>
    public class DataTransactionItem {
        private const string SQL_LIST = "SELECT * FROM TransactionItem WHERE transaction_id = @TransactionID AND type = @Type";
        private const string SQL_DELETE = "DELETE FROM TransactionItem WHERE transaction_id = @TransactionID";
        private const string SQL_INSERT = "INSERT INTO TransactionItem(transaction_id, copy_id, type) VALUES (@TransactionID, @CopyID, @Type)";
        private const string SQL_UPDATE = "UPDATE DVDCopy SET DVDCopy.status = @Status FROM DVDCopy INNER JOIN TransactionItem ON TransactionItem.copy_id = DVDCopy.copy_id WHERE TransactionItem.transaction_id = @TransactionID";
        private const string SQL_FETCH = "SELECT * FROM TransactionItem WHERE transaction_id = @TransactionID AND copy_id = @CopyID";
        private const string SQL_COUNT = "SELECT COUNT(*) FROM TransactionItem WHERE transaction_id = @TransactionID AND type = @Type";

        /// <summary>
        /// The Add method is used to insert the received object into the corresponding database table. 
        /// It coverts the object attributes into a dictionary, which is then passed
        /// into the DataSQL helper method along with the insert sql statement.
        /// </summary>
        /// <param name="copy">The transaction item object.</param>
        public static void Add(TransactionItem item) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TransactionID", item.transactionID);
            parameters.Add("@CopyID", item.copyID);
            parameters.Add("@Type", (int)item.type);
            DataSQL<TransactionItem>.NonQuery(SQL_INSERT, parameters);
        }

        /// <summary>
        /// The Fetch method will find the record in the database, and return a newly instantiated object containing
        /// the record data. It uses the generic DataSQL Fetch method to dynamically instantiate the object using Reflection,
        /// which will return the instance.
        /// </summary>
        /// <param name="transactionID">The Transaction primary key.</param>
        /// /// <param name="copyID">The DVD Copy primary key.</param>
        /// <returns>The instance object.</returns>
        public static TransactionItem Fetch(int transactionID, int copyID) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TransactionID", transactionID);
            parameters.Add("@CopyID", copyID);
            return DataSQL<TransactionItem>.Fetch(SQL_FETCH, parameters);
        }

        /// <summary>
        /// The Clear method is used to delete all record from the corresponding database table. It receives
        /// the primary key of the transaction, and will use the generic helper method of the DataSQL class to remove all the child records.
        /// </summary>
        /// <param name="id">The transaction primary key.</param>
        public static void Clear(int id) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TransactionID", id);
            DataSQL<TransactionItem>.NonQuery(SQL_DELETE, parameters);
        }

        /// <summary>
        /// The List method is used to fetch all objects within the corresponding database table. It uses
        /// the generic List method of the DataSQL class to fetch the instances. It uses Reflection to dynamically
        /// instantiate the object based on the generic type. It doesn't however return a List of TransactionItem,
        /// instead it will return the DVDCopy objects relating to the Item.
        /// </summary>
        /// <param name="transactionID">The Transaction primary key.</param>
        /// <param name="TransactionType">The Transaction Type.</param> 
        /// <returns>Returns a List of the DVD Copies.</returns>
        public static List<DVDCopy> List(int transactionID, TransactionItem.TransactionType type) {
            // Instantiates a empty list of DVDCopy.
            List<DVDCopy> list = new List<DVDCopy>();
            // Prepares and adds the paramaters to the dictionary.
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TransactionID", transactionID);
            parameters.Add("@Type", (int)type);
            // Executes the query through the helper, then iterates the resulting TransactionItems.
            foreach (TransactionItem item in DataSQL<TransactionItem>.List(SQL_LIST, parameters)) {
                // Adds each item's relating DVDCopy instance to the DVD Copy list.
                list.Add(item.copy);
            }
            return list; // Returns the List.
        }

        /// <summary>
        /// The UpdateCopies method is used to update the status of all the DVDCopies that
        /// were added to a transaction. It uses the generic helper method of the DataSQL class
        /// to execute the query.
        /// </summary>
        /// <param name="transactionID">The transactionID of the.</param>
        /// <returns>A boolean indicating whether the record was updated.</returns>
        public static int UpdateCopies(int transactionID, DVDCopy.Status status) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Status", (int)status);
            parameters.Add("@TransactionID", transactionID);
            return DataSQL<DVDCopy>.NonQuery(SQL_UPDATE, parameters);
        }

        /// <summary>
        /// The Count method is used to count the amount of transaction items that have been
        /// added to a specific transaction based on the transaction type (either being rental or sale).
        /// </summary>
        /// <param name="transactionID">The transaction's primary key.</param>
        /// <param name="TransactionType">The transaction type.</param>
        /// <returns>The count of items relating to a transaction & type.</returns>
        public static int Count(int transactionID, TransactionItem.TransactionType type) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TransactionID", transactionID);
            parameters.Add("@Type", (int)type);
            return DataSQL<Transaction>.ScalarQuery(SQL_COUNT, parameters);
        }
    }
}