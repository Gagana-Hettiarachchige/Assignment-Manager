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
using System.Windows.Automation.Peers;

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

        const int NOTIFY_WIDTH = 665;


        /* Shortcuts. */
        public static RoutedCommand DueDateEditShortcut = new RoutedCommand();
        public static RoutedCommand LocalEditShortcut = new RoutedCommand();
        public static RoutedCommand OnlineEditShortcut = new RoutedCommand();
        public static RoutedCommand SelectNewShortcut = new RoutedCommand();
        public static RoutedCommand CycleUpShortcut = new RoutedCommand();
        public static RoutedCommand CycleDownShortcut = new RoutedCommand();
        public static RoutedCommand InsertUpdateShortcut = new RoutedCommand();
        public static RoutedCommand ClearDeleteShortCut = new RoutedCommand();

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


            /* Creating shortcuts. */
            DueDateEditShortcut.InputGestures.Add(new KeyGesture(Key.Q, ModifierKeys.Alt));
            LocalEditShortcut.InputGestures.Add(new KeyGesture(Key.W, ModifierKeys.Alt));
            OnlineEditShortcut.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Alt));
            SelectNewShortcut.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Alt));
            CycleUpShortcut.InputGestures.Add(new KeyGesture(Key.D1, ModifierKeys.Alt));
            CycleDownShortcut.InputGestures.Add(new KeyGesture(Key.D2, ModifierKeys.Alt));
            InsertUpdateShortcut.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Alt | ModifierKeys.Shift));
            ClearDeleteShortCut.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Alt | ModifierKeys.Shift));

            /* Setting time. */
            ClockTextBlock.Text = DateTime.Now.ToString("F");

            /* Making general info text block blank. */
            GeneralInfoTextBlock.Text = "";

            NotificationRectangle.Width = 0;


            /* Creating clock. */
            headerClock = new DispatcherTimer();

            /* Making it update every second. */
            headerClock.Interval = TimeSpan.FromTicks(1);
            headerClock.Tick += new EventHandler(HeaderClock_Tick);
            
            /* Starting clock. */
            headerClock.Start();
        }



        /* Event handlers. */


        /* Header row. */



        /* 
        * METHOD        : HeaderClock_Tick
        * DESCRIPTION   :
        *   Updates the header clock to the current time and also animates a rectangle.
        * PARAMETERS    :
        *   object sender   : the sender
        *   EventArgs       : the event arguments
        * RETURNS       :
        *   void
        */
        private void HeaderClock_Tick(object sender, EventArgs e)
        {
            /* Updating time. */
            ClockTextBlock.Text = DateTime.Now.ToString("F");

            /* Making notifaction rectangle shrink. */
            if (ConfigurationManager.AppSettings["animationsEnabled"] == "true")
            {
                if (NotificationRectangle.Width > 0)
                {
                    --NotificationRectangle.Width;
                }
            }
        }



        /* 
        * METHOD        : SettingsButton_Click
        * DESCRIPTION   :
        *   Raised when the settings button is clicked and
        *   gives options to change
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            /* Prompting to disable animations. */
            MessageBoxResult result = MessageBox.Show("Disable animations?", "Settings", 
                                      MessageBoxButton.YesNo, 
                                      MessageBoxImage.Question);


            if (result == MessageBoxResult.Yes)
            {
                ConfigurationManager.AppSettings["animationsEnabled"] = "false";
            }

            else
            {
                ConfigurationManager.AppSettings["animationsEnabled"] = "true";
            }

            //Should make an actual window for options.
        }


        /* 
        * METHOD        : HelpButton_Click
        * DESCRIPTION   :
        *   Raised when the help button is clicked and
        *   opens a message box with information
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ALT + Q = Edit Due Date\r\n" +
                            "ALT + W = Edit Local Resources\r\n" +
                            "ALT + E = Edit Online Resources\r\n" +
                            "ALT + A = Select New Assignment Option\r\n" +
                            "ALT + 1 = Cycle selected assignments up\r\n" +
                            "ALT + 2 = Cycle selected assignments down\r\n" +
                            "ALT + SHIFT + S = Save/Update current assignment.\r\n" +
                            "ALT + SHIFT + D = Delete/Clear current assignment.\r\n\r\n" +
                            "SHIFT + LEFT MOUSE CLICK = Multi-select resources", 
                            "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        /* Insert row. */


        /* 
        * METHOD        : ClassTextBox_TextChanged
        * DESCRIPTION   :
        *   
        * PARAMETERS    :
        *   type identifier : parameter description
        * RETURNS       :
        *   type : description
        */
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


        private void ClearUI()
        {
            /* Updating UI with selected assignment details. */
            ClassTextBox.Text = "";
            AssignmentTextBox.Text = "";
            WeightTextBox.Text = "";
            ViewModel.SelectedAssignment.DueDate = new DateTime(DateTime.Now.Year,
                                                       DateTime.Now.Month, DateTime.Now.Day,
                                                       23, 00, 00);
            DueDateButton.Content = ViewModel.SelectedAssignment.DueDate.ToString();
            StatusComboBox.SelectedIndex = 0;
            ViewModel.SelectedAssignment.LocalResources = "";
            ViewModel.SelectedAssignment.OnlineResources = "";
            ViewModel.SelectedLocalResources.Clear();
            ViewModel.SelectedOnlineResources.Clear();
            LocalResourcesButton.Content = ViewModel.SelectedLocalResources.Count;
            OnlineResourcesButton.Content = ViewModel.SelectedOnlineResources.Count;
        }




        private void UpdateUI()
        {
            /* Updating UI with selected assignment details. */
            ClassTextBox.Text = ViewModel.SelectedAssignment.ClassName;
            AssignmentTextBox.Text = ViewModel.SelectedAssignment.AssignmentName;
            WeightTextBox.Text = ViewModel.SelectedAssignment.AssignmentWeight.ToString();
            DueDateButton.Content = ViewModel.SelectedAssignment.DueDate.ToString("yyyy-MM-dd hh:mm:ss tt");
            StatusComboBox.SelectedItem = ViewModel.SelectedAssignment.AssignmentStatus;

            /* Updating resource lists. */
            string[] local_resources = ViewModel.SelectedAssignment.LocalResources.Split('\n');
            string[] online_resources = ViewModel.SelectedAssignment.OnlineResources.Split('\n');

            ViewModel.SelectedLocalResources.Clear();
            ViewModel.SelectedOnlineResources.Clear();


            foreach (string resource in local_resources)
            {
                if (resource != "")
                {
                    ViewModel.SelectedLocalResources.Add(resource);
                }
            }

            foreach (string resource in online_resources)
            {
                if (resource != "")
                {
                    ViewModel.SelectedOnlineResources.Add(resource);
                }
            }

            LocalResourcesButton.Content = ViewModel.SelectedLocalResources.Count;
            OnlineResourcesButton.Content = ViewModel.SelectedOnlineResources.Count;
        }


        private void AssignmentSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AssignmentSelectComboBox.SelectedItem != null)
            {
                if (AssignmentSelectComboBox.SelectedItem.ToString() == "New")
                {
                    /* Clearing UI if new is selected */
                    ClearUI();

                    /* Updating UI buttons to show available actions. */
                    InsertUpdateButton.Content = "Insert";
                    ClearDeleteButton.Content = "Clear";
                    InsertUpdateButton.ToolTip = "Insert current assignment...";
                    ClearDeleteButton.ToolTip = "Clear current assignment details...";

                    /* Updating grid highlight. */
                    TableGrid.SelectedItem = null;

                    NotificationRectangle.Width = NOTIFY_WIDTH;
                    NotificationRectangle.Fill = Brushes.SlateGray;
                }

                else
                {
                    /* Setting view model's number to the selected assignment number. */
                    int new_selection = int.Parse(AssignmentSelectComboBox.SelectedItem.ToString());

                    Database.Connect();

                    /* Updating the selected assignment details with the selection. */
                    Database.SelectNewAssignment(new_selection);

                    /* Updating UI. */
                    UpdateUI();

                    Database.Disconnect();


                    /* Updating UI buttons to show available actions. */
                    InsertUpdateButton.Content = "Update";
                    ClearDeleteButton.Content = "Delete";
                    InsertUpdateButton.ToolTip = "Update current assignment...";
                    ClearDeleteButton.ToolTip = "Delete current assignment...";

                    /* Updating grid highlight. */
                    foreach (Assignment row in TableGrid.Items)
                    {
                        if (row.AssignmentNumber == ViewModel.SelectedAssignment.AssignmentNumber)
                        {
                            TableGrid.SelectedItem = row;
                        }
                    }

                    NotificationRectangle.Width = NOTIFY_WIDTH;
                    NotificationRectangle.Fill = Brushes.CornflowerBlue;
                }

                if (ConfigurationManager.AppSettings["animationsEnabled"] == "false")
                {
                    NotificationRectangle.Width = 0;
                }
            }            
        }


        /* 
        * METHOD        : InsertUpdateButton_Click
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
        private void InsertUpdateButton_Click(object sender, RoutedEventArgs e)
        {

            /* Checking if action is insert. */
            if (InsertUpdateButton.Content.ToString() == "Insert")
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

                NotificationRectangle.Width = NOTIFY_WIDTH;
                NotificationRectangle.Fill = Brushes.Green;
            }
            
            /* Checking if action is alter. */
            else if (InsertUpdateButton.Content.ToString() == "Update")
            {
                Database.Connect();

                /* Updating selected assignment. */
                Database.UpdateSelectedAssignment();

                /* Updating UI with newest addition. */
                Database.UpdateAssignments();

                Database.Disconnect();


                NotificationRectangle.Width = NOTIFY_WIDTH;
                NotificationRectangle.Fill = Brushes.SeaGreen;
            }


            if (ConfigurationManager.AppSettings["animationsEnabled"] == "false")
            {
                NotificationRectangle.Width = 0;
            }
        }


        private void ClearDeleteButton_Click(object sender, RoutedEventArgs e)
        {

            /* Checking if action is clear. */
            if (ClearDeleteButton.Content.ToString() == "Clear")
            {
                ClearUI();
                NotificationRectangle.Width = NOTIFY_WIDTH;
                NotificationRectangle.Fill = Brushes.SlateGray;
            }

            /* Checking if action is delete. */
            else if (ClearDeleteButton.Content.ToString() == "Delete")
            {

                /* Setting view model's number to the selected assignment number. */
                int new_selection = int.Parse(AssignmentSelectComboBox.SelectedItem.ToString());


                Database.Connect();

                /* Deleting selected assignment. */
                Database.DeleteSelectedAssignment();
                
                /* Updating display of database assignments. */
                Database.UpdateAssignments();

                Database.Disconnect();


                /* Deleting the assignment from the combo box. */
                int selected_assignment_index = AssignmentSelectComboBox.SelectedIndex;
                AssignmentSelectComboBox.Items.Remove(AssignmentSelectComboBox.SelectedItem);
                AssignmentSelectComboBox.SelectedIndex = selected_assignment_index - 1;

                NotificationRectangle.Width = NOTIFY_WIDTH;
                NotificationRectangle.Fill = Brushes.Red;
            }


            if (ConfigurationManager.AppSettings["animationsEnabled"] == "false")
            {
                NotificationRectangle.Width = 0;
            }
        }




        /* Table row. */


        private void AssignmentNumber_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock assignment_number_text = (TextBlock)e.OriginalSource;
            int assignment_number = int.Parse(assignment_number_text.Text);

            AssignmentSelectComboBox.SelectedItem = assignment_number;
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
            
            /* Getting number of days and hours left. */
            int days_due = (due_date - DateTime.Now).Days;
            int hours_due = (due_date - DateTime.Now).Hours;

            /* Checking how many days are left. */
            if (days_due <= 0)
            {
                GeneralInfoTextBlock.Background = Brushes.OrangeRed;
            }

            else if (days_due <= 1)
            {
                GeneralInfoTextBlock.Background = Brushes.Yellow;
                
            }

            else if (days_due <= 2)
            {
                GeneralInfoTextBlock.Background = Brushes.Orange;
            }


            /* Displaying info about how many days left. */
            GeneralInfoTextBlock.Text = $"Assignment is due in {days_due} days and {hours_due} hours.";
        }

        private void DueDate_MouseLeave(object sender, MouseEventArgs e)
        {
            /* Resetting info text block. */
            GeneralInfoTextBlock.Background = Brushes.White;
            GeneralInfoTextBlock.Text = "";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /* Stopping clock. */
            headerClock.Stop();
        }





        /* Shortcuts. */
        private void DueDateEditShortcut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DueDateButton_Click(sender, e);
        }

        private void LocalEditShortcut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LocalResourcesButton_Click(sender, e);
        }

        private void OnlineEditShortcut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnlineResourcesButton_Click(sender, e);
        }

        private void SelectNewShortcut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /* Switching selected assignment to new. */
            AssignmentSelectComboBox.SelectedIndex = 0;
        }


        private void CycleUpShortcut_Executed(object sender, ExecutedRoutedEventArgs e)
        {   
            if (AssignmentSelectComboBox.SelectedIndex > 0)
            {
                /* Cycling up. */
                AssignmentSelectComboBox.SelectedIndex -= 1;
            }
            else
            {
                /* Wrapping around. */
                AssignmentSelectComboBox.SelectedIndex = AssignmentSelectComboBox.Items.Count - 1;
            }
        }

        private void CycleDownShortcut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AssignmentSelectComboBox.SelectedIndex < AssignmentSelectComboBox.Items.Count - 1)
            {
                /* Cycling down. */
                AssignmentSelectComboBox.SelectedIndex += 1;
            }

            else
            {
                /* Wrapping around. */
                AssignmentSelectComboBox.SelectedIndex = 0;
            }
        }

        private void InsertUpdateShortcut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InsertUpdateButton_Click(sender, e);
        }

        private void ClearDeleteShortCut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ClearDeleteButton_Click(sender, e);
        }

        
    }
}
