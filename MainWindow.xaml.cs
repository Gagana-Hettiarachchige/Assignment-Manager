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

        /* All assignments. */
        ObservableCollection<Assignment> loadedAssignments = new ObservableCollection<Assignment>();
        int assignmentNumber = 0;   // Testing substitution for assignment number.

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


            /* Adding options to status combo box. */
            StatusComboBox.Items.Add("Incomplete");
            StatusComboBox.Items.Add("In Progress");
            StatusComboBox.Items.Add("Complete");
            StatusComboBox.SelectedIndex = 0; /* Initializes selected assignment's status. */


            /* Setting and getting defaults. */
            ClassTextBox.Text = ViewModel.SelectedAssignment.ClassName;
            AssignmentTextBox.Text = ViewModel.SelectedAssignment.AssignmentName;
            WeightTextBox.Text = ViewModel.SelectedAssignment.AssignmentWeight.ToString();
            DueDateButton.Content = ViewModel.SelectedAssignment.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt");
            LocalResourcesButton.Content = ViewModel.SelectedLocalResources.Count;
            OnlineResourcesButton.Content = ViewModel.SelectedOnlineResources.Count;



            /* Setting time. */
            ClockTextBlock.Text = DateTime.Now.ToString("F");

            /* Creating clock. */
            headerClock = new DispatcherTimer();

            /* Making it update every second. */
            headerClock.Interval = TimeSpan.FromSeconds(1);
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


        /* Modify row. */


        private void WeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                /* Checking if weight is a double. */
                lastValidWeight = double.Parse(WeightTextBox.Text);
            }

            catch
            {
                if (WeightTextBox.Text != "")
                {
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


        /* 
        * METHOD        : InsertButton_Click
        * DESCRIPTION   :
        *   Raised when the insert button is clicked and
        *   saves the new assignment in the database with
        *   the details entered.
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            ++assignmentNumber; // Testing.


            /* Holds all local resource paths. */
            string local_items = "";

            /* Holds all online resource paths. */
            string online_items = "";


            /* Looping through the local resources list. */
            foreach(string resource in ViewModel.SelectedLocalResources)
            {
                /* Adding all local resources to single string. */
                local_items += resource;
                local_items+= '\n';
            }

            /* Looping through the online resources list. */
            foreach (string resource in ViewModel.SelectedOnlineResources)
            {
                /* Adding all online resources to single string. */
                online_items += resource;
                online_items += '\n';
            }


            try
            {
                // Need to use viewmodel when items are changed changed so that
                // values for the new_assignment are correct.


                /* Creating new assignment. */ 
                Assignment new_assignment = new Assignment()
                {

                    //Need to add validation to stop blank values for mandatory.
                    AssignmentNumber = assignmentNumber,
                    ClassName = ClassTextBox.Text,
                    AssignmentName = AssignmentTextBox.Text,
                    AssignmentWeight = double.Parse(WeightTextBox.Text),
                    DueDate = DateTime.Parse(DueDateButton.Content.ToString()),
                    AssignmentStatus = StatusComboBox.Text,
                    LocalResources = local_items,
                    OnlineResources = online_items,
                };

                loadedAssignments.Add(new_assignment); //Temporary until database used.

                TableGrid.ItemsSource = loadedAssignments;
            }
            
            catch
            {
                MessageBox.Show(this, "Weight cannot be blank.", "Weight Input Error", 
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }





        /* Table row. */



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
            TextBlock text_block = (TextBlock)e.OriginalSource;
            
            /* Getting each resource as a string. */
            string[] resources = text_block.Text.Split('\n');
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
        * METHOD        : Resources_MouseEnter
        * DESCRIPTION   :
        *   Raised when either the local or online resources are hovered over and
        *   change the mouse to a hand icon.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void Resources_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock text_block = (TextBlock)e.OriginalSource;

            if (text_block.Text != "")
            {
                Cursor = Cursors.Hand;
            }
            
        }


        /* 
        * METHOD        : Resources_MouseLeave
        * DESCRIPTION   :
        *   Raised when either the local or online resources are hovered off and
        *   change the mouse back to an arrow.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void Resources_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock text_block = (TextBlock)e.OriginalSource;

            if (text_block.Text != "")
            {
                Cursor = Cursors.Arrow;
            }
            
        }
    }
}
