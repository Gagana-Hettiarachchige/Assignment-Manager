﻿/* 
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
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Assignment_Manager.Classes
{
    internal static class Database
    {
        /* Code behind's credentials to access database. */
        private const string AM_USERNAME = "AssignmentManager";
        private const string AM_PASSWORD = "manage";
        private const string DATABASE = "AM";

        private static MySqlConnection connection = new MySqlConnection();

        /* All assignments in database. */
        private static ObservableCollection<Assignment> databaseAssignments = new ObservableCollection<Assignment>();



        /* Properties. */
        public static ObservableCollection<Assignment> DatabaseAssignments
        {
            get { return databaseAssignments; }
            set { databaseAssignments = value; }
        }

       
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
                                "please make sure to run the .sql file in the localhost connection in " +
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
            if (connection.State == ConnectionState.Open)
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
                /* Making database friendly versions of the paths. */
                string database_friendly_local_path =
                       ViewModel.SelectedAssignment.LocalResources.Replace(@"\", @"\\");
                string database_friendly_online_path = 
                       ViewModel.SelectedAssignment.OnlineResources.Replace(@"\", @"\\");

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
                    $"'{database_friendly_local_path}', " +
                    $"'{database_friendly_online_path}');", 
                    connection);

                
                /* Creating editor with insert command. */
                MySqlDataAdapter editor = new MySqlDataAdapter(insert);
                DataSet database = new DataSet(DATABASE);
                
                /* Adding the assignment to the database. */
                editor.Fill(database);

                return true;
            }

            catch (Exception error)
            {
                Disconnect();

                /* Displaying error. */
                MessageBox.Show(error.Message, "Database Insert Failed", 
                                MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }


        /* 
        * METHOD        : UpdateAssignments
        * DESCRIPTION   :
        *   Updates the UI with the current assignments in database.
        * PARAMETERS    :
        *   void
        * RETURNS       :
        *   bool : whether or not the update worked.
        */
        public static bool UpdateAssignments()
        {
            try
            {
                /* Creating read query. */
                MySqlCommand read = new MySqlCommand("SELECT * FROM Assignments ORDER BY " + 
                                                     "DueDate, AssignmentWeight DESC, ClassName;", 
                                                      connection);


                /* Starting read. */
                MySqlDataReader reader = read.ExecuteReader();


                /* Clearing old list of database assignments. */
                databaseAssignments.Clear();


                /* Reading all assignments from assignments database. */
                while (reader.Read()) 
                {

                    /* Getting assignment from database. */
                    Assignment assignment = new Assignment()
                    {
                        AssignmentNumber = int.Parse(reader[0].ToString()),
                        ClassName = reader[1].ToString(),
                        AssignmentName = reader[2].ToString(),
                        AssignmentWeight = double.Parse(reader[3].ToString()),
                        DueDate = DateTime.Parse(reader[4].ToString()),
                        AssignmentStatus = reader[5].ToString(),
                        LocalResources = reader[6].ToString(),
                        OnlineResources = reader[7].ToString()
                    };

                    /* Updating list of assignments in database. */
                    databaseAssignments.Add(assignment);
                }

                /* Stopping read. */
                reader.Close();

                return true;
            }

            catch (Exception error) 
            {
                Disconnect();

                /* Displaying error. */
                MessageBox.Show(error.Message, "Database Select Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }


        /* 
        * METHOD        : DeleteSelectedAssignment
        * DESCRIPTION   :
        *   Deletes the selected assignment from the database
        * PARAMETERS    :
        *   void
        * RETURNS       :
        *   bool : whether or not the delete worked.
        */
        public static bool DeleteSelectedAssignment()
        {
            try
            {
                /* Creating delete command. */
                MySqlCommand delete = new MySqlCommand($"DELETE FROM Assignments " +
                                                       $"WHERE AssignmentNumber = " +
                                                       $"{ViewModel.SelectedAssignment.AssignmentNumber};",
                                                       connection);

                /* Creating editor with delete command. */
                MySqlDataAdapter editor = new MySqlDataAdapter(delete);
                DataSet database = new DataSet(DATABASE);


                /* Deleting the selected assignment from the database. */
                editor.Fill(database);


                return true;
            }

            catch (Exception error) 
            {
                Disconnect();

                /* Displaying error. */
                MessageBox.Show(error.Message, "Database Delete Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }


        /* 
        * METHOD        : UpdateSelectedAssignment
        * DESCRIPTION   :
        *   Updates the selected assignment in the database
        * PARAMETERS    :
        *   void
        * RETURNS       :
        *   bool : whether or not the update worked.
        */
        public static bool UpdateSelectedAssignment()
        {
            try
            {
                /* Making database friendly versions of the paths. */
                string database_friendly_local_path =
                       ViewModel.SelectedAssignment.LocalResources.Replace(@"\", @"\\");
                string database_friendly_online_path =
                       ViewModel.SelectedAssignment.OnlineResources.Replace(@"\", @"\\");



                /* Creating update command. */
                MySqlCommand update = 
                    new MySqlCommand($"UPDATE Assignments " +
                                     $"SET " +
                                     $"AssignmentNumber = {ViewModel.SelectedAssignment.AssignmentNumber}, " +
                                     $"ClassName = '{ViewModel.SelectedAssignment.ClassName}', " +
                                     $"AssignmentName = '{ViewModel.SelectedAssignment.AssignmentName}', " +
                                     $"AssignmentWeight = {ViewModel.SelectedAssignment.AssignmentWeight}, " +
                                     $"DueDate = '{ViewModel.SelectedAssignment.DueDate.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                                     $"AssignmentStatus = '{ViewModel.SelectedAssignment.AssignmentStatus}', " +
                                     $"LocalResources = '{database_friendly_local_path}', " +
                                     $"OnlineResources = '{database_friendly_online_path}' " + 
                                     $"WHERE AssignmentNumber = " +
                                     $"{ViewModel.SelectedAssignment.AssignmentNumber};",
                                     connection);


                /* Creating editor with update command. */
                MySqlDataAdapter editor = new MySqlDataAdapter(update);
                DataSet database = new DataSet(DATABASE);


                /* Updating the selected assignment in the database. */
                editor.Fill(database);



                return true;
            }
            catch (Exception error) 
            {
                Disconnect();

                /* Displaying error. */
                MessageBox.Show(error.Message, "Database Update Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }



        /* 
        * METHOD        : SelectNewAssignment
        * DESCRIPTION   :
        *   Selects the new assignment from the database
        * PARAMETERS    :
        *   int new_assignment_number : the number of the new assignment
        * RETURNS       :
        *   bool : whether or not the select worked.
        */
        public static bool SelectNewAssignment(int new_assignment_number)
        {
            try
            {
                /* Creating read query to get the specific assignment. */
                MySqlCommand read = new MySqlCommand($"SELECT * FROM Assignments " +
                                                     $"WHERE AssignmentNumber = {new_assignment_number};", 
                                                     connection);

                /* Reading. */
                MySqlDataReader reader = read.ExecuteReader();
                reader.Read();


                /* Updating the selected assignment. */
                ViewModel.SelectedAssignment.AssignmentNumber = int.Parse(reader[0].ToString());
                ViewModel.SelectedAssignment.ClassName = reader[1].ToString();
                ViewModel.SelectedAssignment.AssignmentName = reader[2].ToString();
                ViewModel.SelectedAssignment.AssignmentWeight = double.Parse(reader[3].ToString());
                ViewModel.SelectedAssignment.DueDate = DateTime.Parse(reader[4].ToString());
                ViewModel.SelectedAssignment.AssignmentStatus = reader[5].ToString();
                ViewModel.SelectedAssignment.LocalResources = reader[6].ToString();
                ViewModel.SelectedAssignment.OnlineResources = reader[7].ToString();


                reader.Close();

                return true;
            }

            catch (Exception error) 
            {
                Disconnect();

                /* Displaying error. */
                MessageBox.Show(error.Message, "Database Select Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }



    }
}
