/* 
* FILE          : Database.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-21
* DESCRIPTION   :
*	This file holds the code to interact with the database.
*/

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AssignmentManager.CodeFiles
{
    internal static class Database
    {
        /* Code behind's credentials to access database. */
        private const string AM_USERNAME = "AssignmentManager";
        private const string AM_PASSWORD = "manage";
        private const string DATABASE = "AM";

        private static MySqlConnection connection = new MySqlConnection();

        /* All assignments in database. */
        public static ObservableCollection<Assignment> databaseAssignments = new ObservableCollection<Assignment>();


       
        /* 
        * METHOD        : Connect 
        * DESCRIPTION   :
        *   Connects to the assignment manager database.
        * PARAMETERS    :
        *   void
        * RETURNS       :
        *   bool : whether it was able to connect or not.
        */
        public static bool Connect()
        {
            /* Logging into the assignment manager database using the code behind's credentials. */
            string login = "server=127.0.0.1;" +
                           "uid=" + AM_USERNAME + ";" +
                           "pwd=" + AM_PASSWORD + ";" +
                           "database=" + DATABASE + ";";

            try
            {
                /* Trying to save connection. */
                connection = new MySqlConnection(login);

                /* Opening connection. */
                connection.Open();
                
                return true;
            }

            catch(Exception error) 
            {

                /* Closing if it was opened during error. */
                Disconnect();


                MessageBox.Show(error.Message +"\n\n(If this is the first time launching this program, " +
                                "please make sure to have run the .sql file in the localhost connection in " +
                                "your MySQL workbench).", 
                                "Database Connection Failed", 
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            
        }


        /* 
        * METHOD        : Disconnect 
        * DESCRIPTION   :
        *   Disconnects from the assignment manager database 
        *   if the connection is open.
        * PARAMETERS    :
        *   void
        * RETURNS       :
        *   void
        */
        public static void Disconnect()
        {
            /* Closing only if already opened. */
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }


        /* 
        * METHOD        : InsertAssignment
        * DESCRIPTION   :
        *   Inserts the selected assignment into the database.
        * PARAMETERS    :
        *   void
        * RETURNS       :
        *   bool : whether or not the insert worked.
        */
        public static bool InsertAssignment()
        {
            try
            {
                Connect();

                /* Creating insert command. */
                MySqlCommand insert = new MySqlCommand
                    ("INSERT INTO Assignments(ClassName, AssignmentName, AssignmentWeight, " +
                    "DueDate, AssignmentStatus, LocalResources, OnlineResources) " +
                    "VALUES" +
                    $"('{ViewModel.SelectedAssignment.ClassName}', " +
                    $"'{ViewModel.SelectedAssignment.AssignmentName}', " +
                    $"{ViewModel.SelectedAssignment.AssignmentWeight}, " +
                    $"'{ViewModel.SelectedAssignment.DueDate.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                    $"'{ViewModel.SelectedAssignment.AssignmentStatus}', " +
                    $"'{ViewModel.SelectedAssignment.LocalResources}', " +
                    $"'{ViewModel.SelectedAssignment.OnlineResources}');", 
                    connection);

                
                /* Creating editor with insert command. */
                MySqlDataAdapter editor = new MySqlDataAdapter(insert);
                DataSet database = new DataSet(DATABASE);
                
                /* Editing the database. */
                editor.Fill(database);


                Disconnect();

                return true;
            }
            catch (Exception error)
            {
                /* Displaying error. */
                MessageBox.Show(error.Message, "Database Insert Failed", 
                                MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            
        }
    }
}
