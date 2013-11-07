using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVDEzy.BusinessLayer;

namespace DVDEzy.DataLayer {
    /// <summary>
    /// The DataCustomer class is used as a layer between the Customer and the DataSQL class. It contains the SQL statements
    /// (delcared as constants) and the methods which are used for interacting with the database.
    /// Author: Lance Baker (c3128034).
    /// </summary>
    public class DataCustomer {
        private const string SQL_LIST = "SELECT * FROM Customer Order By first_name, last_name ASC";
        private const string SQL_FETCH = "SELECT * FROM Customer WHERE customer_id = @CustomerID";
        private const string SQL_INSERT = "INSERT INTO Customer (first_name, last_name, street_address, billing_address, postcode, gender) OUTPUT INSERTED.customer_id VALUES (@FirstName, @LastName, @StreetAddress, @BillingAddress, @Postcode, @Gender)";
        private const string SQL_DELETE = "DELETE FROM Customer WHERE customer_id = @CustomerID";
        private const string SQL_UPDATE = "UPDATE Customer SET first_name = @FirstName, last_name = @LastName, street_address = @StreetAddress, billing_address = @BillingAddress, postcode = @Postcode, gender = @Gender WHERE customer_id = @CustomerID";

        /// <summary>
        /// The AddParameters method is used to convert the instance data of the received object
        /// into a dictionary (with the key corresponding to the data name used in the query).
        /// </summary>
        /// <param name="customer">The object to be converted.</param>
        /// <returns>The instance data as a dictionary.</returns>
        private static Dictionary<string, object> AddParameters(Customer customer) {
            // Instantiates a new dictionary used for the parameters.
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            // Adds the instance data to the dictionary.
            parameters.Add("@FirstName", customer.firstName);
            parameters.Add("@LastName", customer.lastName);
            parameters.Add("@StreetAddress", customer.streetAddress);
            parameters.Add("@BillingAddress", customer.billingAddress);
            parameters.Add("@Postcode", customer.postcode);
            parameters.Add("@Gender", (int)customer.gender);
            return parameters; // Returns the dictionary.
        }

        /// <summary>
        /// The Add method is used to insert the received object into the corresponding database table. It uses the
        /// AddParameters method to covert the object attributes into a dictionary, which is then passed
        /// into the DataSQL helper method along with the insert sql statement.
        /// </summary>
        /// <param name="customer">The customer object.</param>
        /// <returns>The inserted record's primary key.</returns>
        public static int Add(Customer customer) {
            // Converts the instance data to a dictionary, and passes both the sql statement & the dictionary
            // as arguments to the generic ScalarQuery helper method. It will return a int being the newly inserted
            // primary key.
            return DataSQL<Customer>.ScalarQuery(SQL_INSERT, AddParameters(customer));
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
        public static int Update(Customer customer) {
            // Converts the instance data to a dictionary which matches the variables in the sql statement.
            Dictionary<string, object> parameters = AddParameters(customer);
            // Adds the primary key to the end, which is for the where clause.
            parameters.Add("@CustomerID", customer.customerID);
            // Passes both the SQL statement, and the parameters as arguments to the generic NonQuery
            // helper method. It will return a int of the rows affected by the change.
            return DataSQL<Customer>.NonQuery(SQL_UPDATE, parameters);
        }

        /// <summary>
        /// The Fetch method will find the record in the database, and return a newly instantiated object containing
        /// the record data. It uses the generic DataSQL Fetch method to dynamically instantiate the object using Reflection,
        /// which will return the instance.
        /// </summary>
        /// <param name="id">The primary key of the object desired to find.</param>
        /// <returns>The instance object.</returns>
        public static Customer Fetch(int id) {
            // Instantiates a new dictionary to add the parameters.
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            // Adds the primary key as a parameter.
            parameters.Add("@CustomerID", id);
            // Invokes the generic Fetch method passing the sql statement, and the paramaters as arguments.
            // Returns the object instance.
            return DataSQL<Customer>.Fetch(SQL_FETCH, parameters);
        }

        /// <summary>
        /// The Remove method is used to delete a record from the corresponding database table. It receives
        /// the primary key, and will use the generic helper method of the DataSQL class to remove the record.
        /// </summary>
        /// <param name="id">The primary key.</param>
        /// <returns>A boolean indicating whether the record was removed.</returns>
        public static Boolean Remove(int id) {
            // Instantiates a new dictionary to add the parameters.
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            // Adds the primary key as a parameter.
            parameters.Add("@CustomerID", id);
            // Invokes the generic NonQuery method passing the sql statement, and the paramaters as arguments.
            // Returns a boolean indicating success.
            return (DataSQL<Customer>.NonQuery(SQL_DELETE, parameters) > 0);
        }

        /// <summary>
        /// The List method is used to fetch all objects within the corresponding database table. It uses
        /// the generic List method of the DataSQL class to fetch the instances. It uses Reflection to dynamically
        /// instantiate the object based on the generic type. 
        /// </summary>
        /// <returns>Returns a List of the objects.</returns>
        public static List<Customer> List() {
            return DataSQL<Customer>.List(SQL_LIST, null);
        }
    }
}