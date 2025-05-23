﻿/* 
* FILE          : HomeWindow.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-17
* DESCRIPTION   :
*	Holds the code behind of the main window.
*/

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
using System.Windows.Shapes;
using System.Windows.Threading;
using Assignment_Manager.Classes;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Media.Animation;
using System.Windows.Automation.Peers;
using Windows.UI.Notifications;
using System.Xml;
using Windows.ApplicationModel;
using Assignment_Manager.Properties;
using MySqlX.XDevAPI.Relational;

namespace Assignment_Manager.Windows
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        const int NOTIFY_WIDTH = 665;
        const int DEFAULT_FLASH_COUNT = 0; // One quick flash.
        const int DEFAULT_FLASH_DURATION = 150;

        double lastValidWeight = 10.01;
        DispatcherTimer headerClock;
        DispatcherTimer flashClock = new DispatcherTimer();
        int flashesLeft = DEFAULT_FLASH_COUNT;
        int flashDuration = 150;
        Brush flashColour = Brushes.White;

        List<int> assignmentsNotified = new List<int>();


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
        * METHOD        : HomeWindow
        * DESCRIPTION   :
        *   Constructor that creates the home window
        * PARAMETERS    :
        *   void
        */
        public HomeWindow()
        {
            InitializeComponent();

            /* Making window fullscreen. */
            this.WindowState = WindowState.Maximized;


            /* Checking database connection. */
            if (Database.Connect() == false)
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
            SelectNewShortcut.InputGestures.Add(new KeyGesture(Key.D1, ModifierKeys.Alt));
            CycleUpShortcut.InputGestures.Add(new KeyGesture(Key.D2, ModifierKeys.Alt));
            CycleDownShortcut.InputGestures.Add(new KeyGesture(Key.D3, ModifierKeys.Alt));
            InsertUpdateShortcut.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Alt | ModifierKeys.Shift));
            ClearDeleteShortCut.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Alt | ModifierKeys.Shift));

            /* Setting time. */
            ClockTextBlock.Text = DateTime.Now.ToString("F");

            /* Making general info text block blank. */
            GeneralInfoTextBlock.Text = "";

            /* Creating clock for flashing ui. */
            flashClock.Interval = TimeSpan.FromMilliseconds(flashDuration);
            flashClock.Tick += new EventHandler(FlashClock_Tick);


            /* Creating clock. */
            headerClock = new DispatcherTimer();

            /* Making it update every other tick. */
            headerClock.Interval = TimeSpan.FromTicks(1);
            headerClock.Tick += new EventHandler(HeaderClock_Tick);

            /* Starting clock. */
            headerClock.Start();
        }


        /* Methods. */

        /* 
        * METHOD        : HighlightUI
        * DESCRIPTION   :
        *   Highlights the assignmment input section of the UI.
        * PARAMETERS    :
        *   Brush colour : the colour to highlight the UI with
        * RETURNS       :
        *   void
        */
        private void HighlightUI(Brush colour)
        {
            ClassTextBox.Background = colour;
            AssignmentTextBox.Background = colour;
            WeightTextBox.Background = colour;
            DueDateButton.Background = colour;
            StatusComboBox.Foreground = colour;
            LocalResourcesButton.Background = colour;
            OnlineResourcesButton.Background = colour;
        }


        /* 
        * METHOD        : UnhighlightUI
        * DESCRIPTION   :
        *   Sets the assignmment input section of the UI back to normal.
        * PARAMETERS    :
        *   void
        * RETURNS       :
        *   void
        */
        private void UnhighlightUI()
        {
            ClassTextBox.Background = Brushes.White;
            AssignmentTextBox.Background = Brushes.White;
            WeightTextBox.Background = Brushes.White;
            DueDateButton.Background = (Brush)this.FindResource("StrongAccent");
            StatusComboBox.Foreground = Brushes.Black;
            LocalResourcesButton.Background = (Brush)this.FindResource("StrongAccent");
            OnlineResourcesButton.Background = (Brush)this.FindResource("StrongAccent");
        }


        /* 
        * METHOD        : FlashUI
        * DESCRIPTION   :
        *   Flashes the input section of the UI.
        * PARAMETERS    :
        *   int flash_count : the amount of times to flash + 1
        *   Brush colour    : the colour to highlight the UI with
        * RETURNS       :
        *   void
        */
        private void FlashUI(int flash_count, Brush colour)
        {
            /* Resetting the clock if it has started. */
            if (flashClock.IsEnabled)
            {
                flashClock.Stop();
                UnhighlightUI();
            }

            /* Setting number of times to flash. */
            flashesLeft = flash_count;

            /* Setting colour to flash. */
            flashColour = colour;

            /* Starting flash. */
            HighlightUI(colour);
            flashClock.Start();
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


            //Can use this for something else later
            /* Making notifaction rectangle shrink. */
            if (ConfigurationManager.AppSettings["animationsEnabled"] == "true")
            {

            }

            /* Checking if any notifications need to be sent out. */
            foreach (Assignment assignment in Database.DatabaseAssignments)
            {
                int days_due = (assignment.DueDate - DateTime.Now).Days;
                int hours_due = (assignment.DueDate - DateTime.Now).Hours;

                /* Notifying if assignment is due in less than 3 days. */
                if (days_due < 3)
                {
                    bool shouldShow = true;

                    /* Checking if the current assignment has already been notified. */
                    foreach (int assignmnet_number in assignmentsNotified)
                    {
                        if (assignment.AssignmentNumber == assignmnet_number)
                        {
                            /* Not sending another notification if it has. */
                            shouldShow = false;
                            break;
                        }
                    }

                    if (shouldShow)
                    {
                        // Should put this into a function.
                        var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                        var stringElements = toastXml.GetElementsByTagName("text");


                        string title = assignment.ClassName + ": " + assignment.AssignmentName;

                        /* Changing notification message based on how close it is. */
                        if (hours_due < 0)
                        {
                            title += " is overdue.";
                        }

                        else if (days_due <= 0)
                        {
                            title += " is due today.";
                        }

                        else if (days_due <= 1)
                        {
                            title += " is due tomorrow.";
                        }

                        else if (days_due <= 3)
                        {
                            title += " is due the day after tomorrow.";
                        }

                        else
                        {
                            title += " is due in " + days_due + " days.";
                        }

                        /* Setting the title and the caption of the notification. */
                        stringElements[0].AppendChild(toastXml.CreateTextNode(title));
                        stringElements[1].AppendChild(toastXml.CreateTextNode
                            ("Assignment is due on " + assignment.DueDate.ToString("F")));


                        /* Creating notification. */
                        var toast = new ToastNotification(toastXml);
                        ToastNotificationManager.CreateToastNotifier("Assignment Manager").Show(toast);

                        /* Added the current assignment to the list of already notified assignments. */
                        assignmentsNotified.Add(assignment.AssignmentNumber);
                    }
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
            /* Opening settings menu. */
            SettingsMenu settings_menu = new SettingsMenu();
            settings_menu.ShowDialog();

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
                            "ALT + 1 = Select New Assignment Option\r\n" +
                            "ALT + 2 = Cycle selected assignments up\r\n" +
                            "ALT + 3 = Cycle selected assignments down\r\n" +
                            "ALT + SHIFT + S = Save/Update current assignment.\r\n" +
                            "ALT + SHIFT + D = Delete/Clear current assignment.\r\n\r\n" +
                            "SHIFT + LEFT MOUSE CLICK = Multi-select resources",
                            "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }





        /* Insert row. */

        /* 
        * METHOD        : FlashClock_Tick
        * DESCRIPTION   :
        *   Starts flashing the UI and stops by itself.
        * PARAMETERS    :
        *   object sender   : the sender
        *   EventArgs       : the event arguments
        * RETURNS       :
        *   void
        */
        private void FlashClock_Tick(object sender, EventArgs e)
        {
            if (flashesLeft > 0)
            {
                /* Flashing the colour if uneven flash count. */
                if ((flashesLeft % 2) != 0)
                {
                    HighlightUI(flashColour);
                }

                /* Switching to default if it is. */
                else
                {
                    UnhighlightUI();
                }

                /* Reducing flash count. */
                --flashesLeft;
            }

            else
            {
                /* Setting to defaults. */
                UnhighlightUI();

                /* Stopping flash. */
                flashClock.Stop();
            }
        }

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



                    /* Flashing UI. */
                    FlashUI(DEFAULT_FLASH_COUNT, Brushes.SlateGray);
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

                    FlashUI(DEFAULT_FLASH_COUNT, Brushes.CornflowerBlue);
                }


                /*** Need to remove the setting as well. ***/
                if (ConfigurationManager.AppSettings["animationsEnabled"] == "false")
                {

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

                FlashUI(2, Brushes.Green);
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

                FlashUI(2, Brushes.Green);
            }


            if (ConfigurationManager.AppSettings["animationsEnabled"] == "false")
            {

            }
        }


        private void ClearDeleteButton_Click(object sender, RoutedEventArgs e)
        {

            /* Checking if action is clear. */
            if (ClearDeleteButton.Content.ToString() == "Clear")
            {
                ClearUI();
                FlashUI(DEFAULT_FLASH_COUNT, Brushes.SlateGray);
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


                FlashUI(2, Brushes.Red);
            }


            if (ConfigurationManager.AppSettings["animationsEnabled"] == "false")
            {

            }
        }




        /* Table row. */


        /* 
        * METHOD        : LocalResources_MouseLeftButtonUp
        * DESCRIPTION   :
        *   Raised when the local resources are clicked in the grid and opens a 
        *   menu to allow for opening the resources of that assignment.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void LocalResources_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Need to
            // 1. Create a sub-window for prompting the user to add an alias when adding resources.
            // 2. Two different event handlers for local and online resources.
            // 3. Use the selected assignment from ViewModel to get the resources and parse them.
            // 4. Aliases should be stored with links as strings delimited by '|' and need to be accounted for when parsing.


            ///* Getting the resources. */
            //TextBlock resources_text = (TextBlock)e.OriginalSource;

            ///* Getting each resource as a string. */
            //string[] resources = resources_text.Text.Split('\n');
            //List<string> list_resources = new List<string>();

            //int count = 0;

            ///* Iterating through each resource. */
            //while (resources[count] != "") 
            //{
            //    list_resources.Add(resources[count]);
            //    ++count;
            //}

            /* Checking if at least 1 resource is in the selected assignment's resource cell. */
            if (ViewModel.SelectedLocalResources.Count >= 1)
            {
                ResourceOpenMenu resource_menu = new ResourceOpenMenu(ViewModel.SelectedLocalResources);
                resource_menu.ShowDialog();
            }
        }


        /* 
        * METHOD        : OnlineResources_MouseLeftButtonUp
        * DESCRIPTION   :
        *   Raised when the online resources are clicked in the grid and opens a 
        *   menu to allow for opening the resources of that assignment.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void OnlineResources_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /* Checking if at least 1 resource is in the selected assignment's resource cell. */
            if (ViewModel.SelectedOnlineResources.Count >= 1)
            {
                ResourceOpenMenu resource_menu = new ResourceOpenMenu(ViewModel.SelectedOnlineResources);
                resource_menu.ShowDialog();
            }
        }


        /* 
        * METHOD        : Resources_MouseEnter
        * DESCRIPTION   :
        *   Raised when resources are hovered over and
        *   changes the mouse icon.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void Resources_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock text_block = (TextBlock)e.OriginalSource;

            /* Only switches to hand if resources are there. */
            if (text_block.Text != "")
            {
                Cursor = Cursors.Hand;
            }

            /* Switches to pen (select) otherwise. */
            else
            {
                Cursor = Cursors.Pen;
            }
        }


        /* 
        * METHOD        : Resources_MouseLeave
        * DESCRIPTION   :
        *   Raised when a clickable text section is hovered off and
        *   changes the mouse back to an arrow.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void Resources_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }


        /* 
        * METHOD        : Cell_MouseEnter
        * DESCRIPTION   :
        *   Raised when a cell section is hovered on and
        *   changes the mouse to a pen.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void Cell_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Pen;
        }


        /* 
        * METHOD        : Cell_MouseLeave
        * DESCRIPTION   :
        *   Raised when a cell section is hovered off and
        *   changes the mouse back to an arrow.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void Cell_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }




        /* 
        * METHOD        : DueDate_MouseEnter
        * DESCRIPTION   :
        *   Raised when a due date cell section is hovered on and
        *   displays the number of days left.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
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
                GeneralInfoTextBlock.Background = Brushes.Orange;
            }

            else if (days_due <= 2)
            {
                GeneralInfoTextBlock.Background = Brushes.Yellow;
            }


            /* Displaying info about how many days left. */
            GeneralInfoTextBlock.Text = $"Assignment is due in {days_due} days and {hours_due} hours.";

            /* Changing cursor to pen. */
            Cursor = Cursors.Pen;
        }


        private void Class_MouseEnter(object sender, MouseEventArgs e)
        {
            /* Getting the class name hovered over. */
            TextBlock class_name_text = (TextBlock)e.OriginalSource;
            string class_name = class_name_text.Text;

            int count = 0;

            foreach (Assignment assignment in TableGrid.Items)
            {
                if (assignment.ClassName == class_name)
                {
                    //TableGrid.SelectedItems.Add(assignment); //Temporary solution.
                    //Need to make it possible to simply change the background of rows with matching class name.
                }

                ++count;
            }
        }

        private void Class_MouseLeave(object sender, MouseEventArgs e)
        {
            //TableGrid.SelectedItems.Clear();
        }


        /* 
        * METHOD        : DueDate_MouseLeave
        * DESCRIPTION   :
        *   Raised when a due date cell section is hovered off and
        *   resets the general text box.
        * PARAMETERS    :
        *   object sender        : the sender
        *   MouseButtonEventArgs : the mouse event args
        * RETURNS       :
        *   void
        */
        private void DueDate_MouseLeave(object sender, MouseEventArgs e)
        {
            /* Resetting info text block. */
            GeneralInfoTextBlock.Background = Brushes.White;
            GeneralInfoTextBlock.Text = "";

            /* Changing cursor back. */
            Cursor = Cursors.Arrow;
        }


        /* 
        * METHOD        : TableGrid_SelectionChanged
        * DESCRIPTION   :
        *   Raised when a new assignment is selected through the table grid
        *   and updates the selection in the combo box.
        * PARAMETERS    :
        *   object sender                         : the sender
        *   System.ComponentModel.CancelEventArgs : the cancel event args
        * RETURNS       :
        *   void
        */
        private void TableGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /* Getting the selected assignment. */
            Assignment selected_assignment = (Assignment)TableGrid.SelectedItem;

            if (selected_assignment != null)
            {
                AssignmentSelectComboBox.SelectedItem = selected_assignment.AssignmentNumber;
            }
        }


        /* 
        * METHOD        : Window_Closing
        * DESCRIPTION   :
        *   Raised when the window is closing and stops the clock.
        * PARAMETERS    :
        *   object sender                         : the sender
        *   System.ComponentModel.CancelEventArgs : the cancel event args
        * RETURNS       :
        *   void
        */
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

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }
    }
}
