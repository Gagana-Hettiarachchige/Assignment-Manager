/* 
* FILE          : MainWindow.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-17
* DESCRIPTION   :
*	Holds the code behind of the main window.
*/

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using AssignmentManager.CodeFiles;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace AssignmentManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    

    /*
    * NAME	  : MainWindow
    * PURPOSE : The main window allows for the user to interact
    *           with the assignments by inserting, altering, deleting,
    *           and display assignments and information about them.
    */
    public partial class MainWindow : Window
    {
        double lastValidWeight = 10.01;
        DispatcherTimer headerClock;


        /* Constructors. */

        /* 
        * METHOD        : MainWindow
        * DESCRIPTION   :
        *   Constructor that creates the main window
        * PARAMETERS    :
        *   void
        */
        public MainWindow()
        {
            InitializeComponent();

            /* Making window fullscreen. */
            this.WindowState = WindowState.Maximized;


            /* Checking database connection. */
            if(Database.Connect() == false)
            {
                /* Closing program if database failed to connect. */
                Close();
            }
            else
            {
                Database.UpdateAssignments();
            }

            Database.Disconnect();

            /* Adding options to status combo box. */
            StatusComboBox.Items.Add("Incomplete");
            StatusComboBox.Items.Add("In Progress");
            StatusComboBox.Items.Add("Complete");
            StatusComboBox.SelectedIndex = 0; /* Initializes selected assignment's status. */


            /* Setting and getting defaults. */
            ClassTextBox.Text = ViewModel.SelectedAssignment.ClassName;
            AssignmentTextBox.Text = ViewModel.SelectedAssignment.AssignmentName;
            WeightTextBox.Text = ViewModel.SelectedAssignment.AssignmentWeight.ToString();

            /* Initializing due date. */
            ViewModel.SelectedAssignment.DueDate = new DateTime(DateTime.Now.Year, 
                                                       DateTime.Now.Month, DateTime.Now.Day, 
                                                       23, 00, 00);
            DueDateButton.Content = ViewModel.SelectedAssignment.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt");
            
            LocalResourcesButton.Content = ViewModel.SelectedLocalResources.Count;
            OnlineResourcesButton.Content = ViewModel.SelectedOnlineResources.Count;


            /* Assigning data source to table. */
            TableGrid.ItemsSource = Database.DatabaseAssignments;


            /* Setting up assignment select combobox. */
            AssignmentSelectComboBox.Items.Add("New");
            AssignmentSelectComboBox.SelectedIndex = 0;

            /* Getting every existing assignment number. */
            foreach (Assignment assignment in Database.DatabaseAssignments)
            {
                AssignmentSelectComboBox.Items.Add(assignment.AssignmentNumber);
            }


            /* Setting time. */
            ClockTextBlock.Text = DateTime.Now.ToString("F");

            /* Making general info text block blank. */
            GeneralInfoTextBlock.Text = "";


            /* Creating clock. */
            headerClock = new DispatcherTimer();

            /* Making it update every second. */
            headerClock.Interval = TimeSpan.FromSeconds(0.1);
            headerClock.Tick += new EventHandler(HeaderClock_Tick);
            
            /* Starting clock. */
            headerClock.Start();
        }



        /* Event handlers. */


        /* Header row. */

        private void HeaderClock_Tick(object sender, EventArgs e)
        {
            /* Updating time. */
            ClockTextBlock.Text = DateTime.Now.ToString("F");
        }


        /* 
        * METHOD        : SchoolButton_Click
        * DESCRIPTION   :
        *   Raised when the school button is clicked and
        *   opens a new tab to the school's homepage.
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void SchoolButton_Click(object sender, RoutedEventArgs e)
        {
            /* Opens tab to school's webpage. */
            Process.Start("https://conestoga.desire2learn.com/d2l/home");
        }




        /* Insert row. */
        private void ClassTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ClassTextBox.Text == "")
            {
                /* Making the class name N/A if left blank. */
                ViewModel.SelectedAssignment.ClassName = "N/A";
            }

            else
            {
                /* Making the class name the textbox value. */
                ViewModel.SelectedAssignment.ClassName = ClassTextBox.Text;
            }
        }

        private void AssignmentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AssignmentTextBox.Text == "")
            {
                /* Making the name N/A if left blank. */
                ViewModel.SelectedAssignment.AssignmentName = "N/A";
            }

            else
            {
                /* Making the name the textbox value. */
                ViewModel.SelectedAssignment.AssignmentName = AssignmentTextBox.Text;
            }
        }

        private void WeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                /* Checking if weight is a double. */
                lastValidWeight = double.Parse(WeightTextBox.Text);

                /* Making the value the selected value. */
                ViewModel.SelectedAssignment.AssignmentWeight = double.Parse(WeightTextBox.Text);
            }

            catch
            {
                if (WeightTextBox.Text == "")
                {
                    /* Setting weight to 0 if blank. */
                    ViewModel.SelectedAssignment.AssignmentWeight = 0;
                }
                else
                {
                    /* Displaying error. */
                    WeightTextBox.Text = lastValidWeight.ToString();
                    MessageBox.Show(this, "Only numbers and decimals are allowed in weight.",
                                        "Weight Input Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


        private void DueDateButton_Click(object sender, RoutedEventArgs e)
        {
            DueDatePicker due_date_picker = new DueDatePicker();

            /* Prompting user to pick the due date. */
            due_date_picker.ShowDialog();

            /* Updating the UI with new date if changed. */
            DueDateButton.Content = ViewModel.SelectedAssignment.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt");
        }


        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /* Updating the selected assignment's assignment status. */
            ViewModel.SelectedAssignment.AssignmentStatus = StatusComboBox.SelectedItem.ToString();
        }


        private void LocalResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyLocalResources modify_local_resources = new ModifyLocalResources();

            /* Opening modify local resources menu. */
            modify_local_resources.ShowDialog();

            /* Updating UI with new count of items if changed. */
            LocalResourcesButton.Content = ViewModel.SelectedLocalResources.Count;
        }


        private void OnlineResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyOnlineResources modify_online_resources = new ModifyOnlineResources();

            /* Opening modify online resources menu. */
            modify_online_resources.ShowDialog();

            /* Updating UI with new count of items if changed. */
            OnlineResourcesButton.Content = ViewModel.SelectedOnlineResources.Count;
        }






        /* Modify row. */



        private void AssignmentSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (AssignmentSelectComboBox.SelectedItem != null)
            {
                if (AssignmentSelectComboBox.SelectedItem.ToString() == "New")
                {
                    /* Setting selected assignment's number to 0 if new is selected. */
                    ViewModel.SelectedAssignment.AssignmentNumber = 0;

                    /* Updating UI buttons to show available actions. */
                    InsertAlterButton.Content = "Insert";
                    ClearDeleteButton.Content = "Clear";
                }

                else
                {
                    /* Setting view model's number to the selected assignment number. */
                    int new_selection = int.Parse(AssignmentSelectComboBox.SelectedItem.ToString());


                    ViewModel.SelectedAssignment.AssignmentNumber = new_selection;


                    //Need to add method call to update the selected assignment details with the selection.


                    /* Updating UI buttons to show available actions. */
                    InsertAlterButton.Content = "Alter";
                    ClearDeleteButton.Content = "Delete";
                }
            }            
        }


        /* 
        * METHOD        : InsertAlterButton_Click
        * DESCRIPTION   :
        *   Raised when the inser/alter button is clicked and
        *   saves the assignment in the database with
        *   the details entered in the row.
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void InsertAlterButton_Click(object sender, RoutedEventArgs e)
        {

            /* Checking if action is insert. */
            if (InsertAlterButton.Content.ToString() == "Insert")
            {
                Database.Connect();

                /* Inserting selected assignment. */
                Database.InsertAssignment();

                /* Updating UI with newest addition. */
                Database.UpdateAssignments();


                Database.Disconnect();


                /* Updating assignment select. */
                AssignmentSelectComboBox.Items.Clear();
                AssignmentSelectComboBox.Items.Add("New");

                int count = 0;
                foreach (Assignment assignment in Database.DatabaseAssignments)
                {
                    AssignmentSelectComboBox.Items.Add(assignment.AssignmentNumber);
                    ++count;
                }

                AssignmentSelectComboBox.SelectedIndex = count;
            }
            
            /* Checking if action is alter. */
            else if (InsertAlterButton.Content.ToString() == "Alter")
            {

            }    
        }


        private void ClearDeleteButton_Click(object sender, RoutedEventArgs e)
        {

            /* Checking if action is clear. */
            if (ClearDeleteButton.Content.ToString() == "Clear")
            {

            }

            /* Checking if action is delete. */
            else if (ClearDeleteButton.Content.ToString() == "Delete")
            {

                /* Setting view model's number to the selected assignment number. */
                int new_selection = int.Parse(AssignmentSelectComboBox.SelectedItem.ToString());


                Database.Connect();

                /* Deleting selected assignment. */
                Database.DeleteSelectedAssignment(new_selection);
                
                /* Updating display of database assignments. */
                Database.UpdateAssignments();

                Database.Disconnect();


                /* Deleting the assignment from the combo box. */
                int selected_assignment_index = AssignmentSelectComboBox.SelectedIndex;
                AssignmentSelectComboBox.Items.Remove(AssignmentSelectComboBox.SelectedItem);
                AssignmentSelectComboBox.SelectedIndex = selected_assignment_index - 1;

            }
        }




        /* Table row. */


        private void AssignmentNumber_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock assignment_number = (TextBlock)e.OriginalSource;

            MessageBox.Show("Selected Assignment: " + assignment_number.Text);
        }

        /* 
        * METHOD        : Resources_MouseLeftButtonUp
        * DESCRIPTION   :
        *   Raised when either the local or online resources are clicked in
        *   the grid and opens a menu to allow for opening the resources of that
        *   assignment.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void Resources_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /* Getting the resources. */
            TextBlock resources_text = (TextBlock)e.OriginalSource;
            
            /* Getting each resource as a string. */
            string[] resources = resources_text.Text.Split('\n');
            List<string> list_resources = new List<string>();

            int count = 0;

            /* Iterating through each item and opening them. */
            while (resources[count] != "") 
            {
                //Process.Start('"' + resources[count] + '"');
                list_resources.Add(resources[count]);
                ++count;
            }


            if (count >= 1)
            {
                /* Creating new resource menu with current resources. */
                ResourceOpenMenu resource_menu = new ResourceOpenMenu(list_resources);
                resource_menu.ShowDialog();
            }

            
        }


        /* 
        * METHOD        : ClickableText_MouseEnter
        * DESCRIPTION   :
        *   Raised when clickable text is hovered over and
        *   changes the mouse to a hand icon.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void ClickableText_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock text_block = (TextBlock)e.OriginalSource;

            if (text_block.Text != "")
            {
                Cursor = Cursors.Hand;
            }
            
        }


        /* 
        * METHOD        : ClickableText_MouseLeave
        * DESCRIPTION   :
        *   Raised when a clickable text section is hovered off and
        *   changes the mouse back to an arrow.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void ClickableText_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock text_block = (TextBlock)e.OriginalSource;

            if (text_block.Text != "")
            {
                Cursor = Cursors.Arrow;
            }
            
        }

        private void DueDate_MouseEnter(object sender, MouseEventArgs e)
        {
            /* Getting the hovered over assignment's due date. */
            TextBlock due_date_text = (TextBlock)e.OriginalSource;
            DateTime due_date = DateTime.Parse(due_date_text.Text);
            
            /* Getting number of days left. */
            int days_due = (due_date - DateTime.Now).Days;

            /* Checking how many days are left. */
            if (days_due == 2)
            {
                /* Displaying info about how many days left. */
                GeneralInfoTextBlock.Background = Brushes.Yellow;
                GeneralInfoTextBlock.Text = "Assignment is due day after tommorrow.";
            }

            else if (days_due == 1)
            {
                GeneralInfoTextBlock.Background = Brushes.Orange;
                GeneralInfoTextBlock.Text = "Assignment is due tommorrow.";
            }

            else if (days_due == 0)
            {
                GeneralInfoTextBlock.Background = Brushes.OrangeRed;
                GeneralInfoTextBlock.Text = "Assignment is due today.";
            }

            else
            {
                GeneralInfoTextBlock.Text = $"Assignment is due in {days_due} days.";
            }
            
            
        }

        private void DueDate_MouseLeave(object sender, MouseEventArgs e)
        {
            /* Resetting info text block. */
            GeneralInfoTextBlock.Background = Brushes.White;
            GeneralInfoTextBlock.Text = "";
        }

        
    }
}
