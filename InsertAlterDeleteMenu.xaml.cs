/* 
* FILE          : InsertAlterDelete.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-22
* DESCRIPTION   :
*	This file holds the code behind of the insert,
*	alter, delete menu and allows the user to 
*	insert assignments, alter selected assignments, and
*	delete assignments in the database.
*/

using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AssignmentManager.CodeFiles
{
    /// <summary>
    /// Interaction logic for InsertAlterDeleteMenu.xaml
    /// </summary>
    public partial class InsertAlterDeleteMenu : Window
    {
        public InsertAlterDeleteMenu()
        {
            InitializeComponent();

            /* Getting comparison. */
            ViewModel.GetCompare();
            TableGrid.ItemsSource = ViewModel.SelectedAssignmentCompare;
        }

        private void InsertOption_Click(object sender, RoutedEventArgs e)
        {

            Database.Connect();

            /* Inserting into the database. */
            if(Database.InsertAssignment())
            {
                Database.UpdateAssignments();
                
                
                Database.Disconnect();

                /* Closing the window if it worked. */
                Close();
            }

            Database.Disconnect();

            
        }

        private void AlterOption_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteOption_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteAllOption_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
