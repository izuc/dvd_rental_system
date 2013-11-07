using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;

namespace DVDEzy.DataLayer {
    /// <summary>
    /// The DataSQL is a generic class which contains methods used for connecting and interacting with the database.
    /// The List and Fetch methods use generics in order to facilitate retrieving objects; enabling an instantiated object (using Reflection)
    /// to be returned based on the contents of the table record(s). It allows for a nice data layer for interacting with the database.
    /// Author: Lance Baker (c3128034)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataSQL<T> {
        // The connection string is based on the connection key declared in the webconfig.
        private static SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("connection"));

        /// <summary>
        /// The Connect method receives the SQL statement and also the parameters (as a dictionary) that will be added to
        /// the query. It will open the database connection, create the sql command using the statement, and then (if any)
        /// will add the parameters (received as a dictionary) into the query. It will then return the SqlCommand.
        /// </summary>
        /// <param name="statement">The SQL Query as a string.</param>
        /// <param name="parameters">The parameters that will be added into the query.</param>
        /// <returns>The SqlCommand object ready to be invoked.</returns>
        private static SqlCommand Connect(string statement, Dictionary<string, object> parameters) {
            // If the connection is closed it will open it.
            if (connection.State == ConnectionState.Closed) {
                connection.Open();
            }
            // Creates a new SqlCommand based on the statement received.
            SqlCommand command = new SqlCommand(statement);
            // Adds the connection to the command.
            command.Connection = connection;
            // If the parameters are not null.
            if (parameters != null) {
                // It will then iterate foreach parameter
                foreach (KeyValuePair<string, object> entry in parameters) {
                    // Then add the parameter into the query. It will replace any null values with the DBNull equivalent.
                    command.Parameters.AddWithValue(entry.Key, (entry.Value != null) ? entry.Value : DBNull.Value);
                }
            }
            // Returns the SqlCommand.
            return command;
        }

        /// <summary>
        /// The GetValues method receives a SqlDataReader as a parameter, and
        /// it creates a object[] array with the size of the field count. It will then
        /// convert the values in the reader to the object values. It then iterates throughout
        /// the values replacing the DBNull references with a null. It will then return a
        /// object[] array.
        /// </summary>
        /// <param name="reader">The SqlDataReader that was received when executing a query.</param>
        /// <returns>A object[] array representation of the data.</returns>
        private static object[] GetValues(SqlDataReader reader) {
            // Creates a new object[] array with the size of the reader fields.
            object[] values = new Object[reader.FieldCount];
            reader.GetValues(values); // Adds the contents of the reader to the object array.
            // Iterates throughout the object array.
            for (int i = 0; i < values.Length; i++) {
                // If a element's type is DBNull
                if (values[i].GetType() == typeof(DBNull)) {
                    values[i] = null; // Replaces it will a standard null.
                }
            }
            return values; // Returns the object array.
        }

        /// <summary>
        /// The Fetch method receives a query statement (string) and also the parameters that will
        /// be added to the query (dictionary). It will then connect, and initiate the query; grabbing
        /// the values from the SqlDataReader as an object[] array, which will then create an instance 
        /// of that generic type based on the contents of the array. It will then return that instance.
        /// </summary>
        /// <param name="statement">The SQL statement.</param>
        /// <param name="parameters">The Parameters to be added.</param>
        /// <returns>A instance of the Generic type based on the result from the query.</returns>
        public static T Fetch(string statement, Dictionary<string, object> parameters) {
            try {
                // Connects and grabs a SqlCommand based on the statement & parameters.
                SqlCommand command = Connect(statement, parameters);
                // Executes the SQL Query, which will then return the results as a SqlDataReader.
                SqlDataReader reader = command.ExecuteReader();
                // If the reader has a record.
                if (reader.Read()) {
                    // It will then convert the data to a object[] array.
                    object[] values = GetValues(reader);
                    reader.Close(); // Close reader.
                    connection.Close(); // Close connection.
                    // It will then using Reflection, instantiate a instance of the generic type. It
                    // passes the object array in; which will then find a constructor with the parameters
                    // of those types, and then create a instance which will be returned.
                    return (T)Activator.CreateInstance(typeof(T), values);
                } else {
                    reader.Close(); // Close reader.
                    connection.Close(); // Close connection.
                }
            }
            catch (SqlException ex) {} // Catches any problems.
            return default(T); // Returns the "default" null value of the generic type.
        }

        /// <summary>
        /// The List method receives a query statement (string) and also the parameters that will
        /// be added to the query (dictionary). It will then connect, and initiate the query; iterate for each
        /// record gabbing the values from the SqlDataReader and instantiate a new instance (using Reflection) for
        /// that generic type, which is then added to the generic list. The list is then returned.
        /// </summary>
        /// <param name="statement">The SQL statement.</param>
        /// <param name="parameters">The Parameters to be added.</param>
        /// <returns>A List of instances based on the result from the query.</returns>
        public static List<T> List(string statement, Dictionary<string, object> parameters) {
            List<T> data = new List<T>();
            try {
                // Connects and grabs a SqlCommand based on the statement & parameters.
                SqlCommand command = Connect(statement, parameters);
                // Executes the SQL Query, which will then return the results as a SqlDataReader.
                SqlDataReader reader = command.ExecuteReader();
                // Iterates for each record.
                while (reader.Read()) {
                    // It will then convert the data to a object[] array.
                    object[] values = GetValues(reader);
                    // It will then using Reflection, instantiate a instance of the generic type. It
                    // passes the object array in; which will then find a constructor with the parameters
                    // of those types, and then create a instance which will then be added to the list.
                    data.Add((T)Activator.CreateInstance(typeof(T), values));
                }
                reader.Close(); // Close reader.
                connection.Close(); // Close connection.
            }
            catch (SqlException ex) { } // Catches any problems.
            return data; // Returns the Generic list.
        }

        /// <summary>
        /// The NonQuery method is used for statements that have no output. For example: UPDATE, DELETE, and 
        /// INSERT can be performed using this method. It receives the statement query as a string, and the parameters
        /// that will be added to the query as a dictionary. It then executes the non query and returns the rows affected.
        /// </summary>
        /// <param name="statement">The SQL statement.</param>
        /// <param name="parameters">The Parameters to be added.</param>
        /// <returns>Rows affected.</returns>
        public static int NonQuery(string statement, Dictionary<string, object> parameters) {
            try {
                // Connects and grabs a SqlCommand based on the statement & parameters.
                SqlCommand command = Connect(statement, parameters);
                // Executes the query, and returns the rows affected.
                return command.ExecuteNonQuery();
            }
            catch (SqlException ex) {
                return 0; // Returns 0 if an error occurred.
            }
        }

        /// <summary>
        /// The ScalarQuery method is used for manipulation statements that also output some data. It is primarily
        /// used for an INSERT statement which adds the record and also returns the auto incremented id, but is also
        /// used with COUNT statements and etc.
        /// </summary>
        /// <param name="statement">The SQL statement.</param>
        /// <param name="parameters">The Parameters to be added.</param>
        /// <returns>The outputted value from the query.</returns>
        public static int ScalarQuery(string statement, Dictionary<string, object> parameters) {
            try {
                // Connects and grabs a SqlCommand based on the statement & parameters.
                SqlCommand command = Connect(statement, parameters);
                // Executes the query, and returns the value outputted in the statement.
                return (int)command.ExecuteScalar();
            }
            catch (SqlException ex) {
                return 0; // Returns 0 if an error occurred.
            }
        }
    }
}