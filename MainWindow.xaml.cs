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
            StatusComboBox.SelectedIndex = 0;
        }



        /* Event handlers. */

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


        /* 
        * METHOD        : LocalResourcesAddButton_Click
        * DESCRIPTION   :
        *   Raised when the ocal resources add file is clicked and
        *   creates an open file dialog to select a resource and
        *   saves it to the local resources combo box
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void LocalResourcesAddButton_Click(object sender, RoutedEventArgs e)
        {
            /* Setting up dialog for adding files to be able to open. */
            OpenFileDialog files_to_add = new OpenFileDialog();
            files_to_add.Multiselect = true;
            files_to_add.Filter = "Solutions (*.sln)|*.sln|" +
                                 "HTML (*.html)|*.html|" +
                                 "PHP (*.php)|*.php|" +
                                 "ASP (*.asp)|*.asp|" +
                                 "All files |*.*";

            /* Asking user to pick resources. */
            Nullable<bool> choice = files_to_add.ShowDialog();

            if (choice == true)
            {
                int count = 0;

                /* Adding resources to the combo box. */
                while (count < files_to_add.FileNames.Length)
                {
                    LocalResourcesComboBox.Items.Add(files_to_add.FileNames[count]);
                    ++count;
                }

                LocalResourcesComboBox.SelectedIndex = count - 1;
            }
            
        }


        /* 
        * METHOD        : LocalResourcesAddFolderButton_Click
        * DESCRIPTION   :
        *   Raised when the local resouces add folder button is clicked and
        *   creates and open folder dialog so select a folder and
        *   adds it to the local resource combo box.
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   type : description
        */
        private void LocalResourcesAddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog resource_folder = new CommonOpenFileDialog();
            resource_folder.IsFolderPicker = true;

            /* Asking user for folder with resources. */
            if (resource_folder.ShowDialog() == CommonFileDialogResult.Ok)
            {
                /* Adding folder to the combo box. */
                LocalResourcesComboBox.Items.Add(resource_folder.FileName);
                LocalResourcesComboBox.SelectedIndex = LocalResourcesComboBox.Items.Count - 1;
            }
        }


        /* 
        * METHOD        : LocalResourcesRemoveButton_Click
        * DESCRIPTION   :
        *   Raised when the local resources remove button is clicked and
        *   removes the selected resource in the local resouces combo box.
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void LocalResourcesRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            /* Removing selected resource from the combo box. */
            LocalResourcesComboBox.Items.Remove(LocalResourcesComboBox.SelectedItem);
        }


        /* 
        * METHOD        : OnlineResourcesAddButton_Click
        * DESCRIPTION   :
        *   Raised when the online resources add button is clicked and 
        *   adds the online resource entered in the online resource texbox to
        *   the online resouces combo box.
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void OnlineResourcesAddButton_Click(object sender, RoutedEventArgs e)
        {
            /* Adding selected resource to the combo box. */
            if (OnlineResourcesTextBox.Text != "")
            {
                OnlineResourcesComboBox.Items.Add(OnlineResourcesTextBox.Text);
                OnlineResourcesComboBox.SelectedIndex = OnlineResourcesComboBox.Items.IndexOf(OnlineResourcesTextBox.Text);
            }
            
        }


        /* 
        * METHOD        : OnlineResourcesRemoveButton_Click
        * DESCRIPTION   :
        *   Raised when the remove online resources button is clicked and
        *   removes the select online resource from the combo box.
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void OnlineResourcesRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            /* Removing selected resource from the combo box. */
            OnlineResourcesComboBox.Items.Remove(OnlineResourcesComboBox.SelectedItem);
        }


        /* 
        * METHOD        : GitFolderAddButton_Click
        * DESCRIPTION   :
        *   Raised when the add git folder button is clicked and
        *   creates a dialog to select a single folder and
        *   adds it to the git folder combo box. 
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void GitFolderAddButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (SelectedGitFolderComboBox.Items.Count < 1)
            {
                CommonOpenFileDialog git_folder = new CommonOpenFileDialog();
                git_folder.IsFolderPicker = true;

                /* Asking user for folder to run open cmd in. */
                if (git_folder.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    /* Adding folder to the combo box. */
                    SelectedGitFolderComboBox.Items.Add(git_folder.FileName);
                    SelectedGitFolderComboBox.SelectedIndex = 0;
                }

            }
        }


        /* 
        * METHOD        : GitFolderRemoveButton_Click
        * DESCRIPTION   :
        *   Raised when the remove git folder button is clicked and
        *   removes the selected folder from the git folder combo box.
        * PARAMETERS    :
        *   object sender   : the sender
        *   RouterEventArgs : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void GitFolderRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            /* Removing the folder. */
            SelectedGitFolderComboBox.Items.Clear();
        }


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


            int count = 0;

            /* Getting all local resources. */
            while (count < LocalResourcesComboBox.Items.Count) 
            {
                /* Adding all local resources to single string. */
                local_items += LocalResourcesComboBox.Items[count];
                local_items += '\n';
                ++count;
            }

            count = 0;

            /* Getting all online resources. */
            while (count < OnlineResourcesComboBox.Items.Count)
            {
                /* Adding all online resources to single string. */
                online_items += OnlineResourcesComboBox.Items[count];
                online_items += '\n';
                ++count;
            }


            /* Creating new assignment. */
            Assignment new_assignment = new Assignment()
            {
                AssignmentNumber = assignmentNumber,
                ClassName = ClassTextBox.Text,
                AssignmentName = AssignmentTextBox.Text,
                AssignmentWeight = double.Parse(WeightTextBox.Text),
                DueDate = DateTime.Parse(DueDateTextBox.Text),
                AssignmentStatus = StatusComboBox.Text,
                LocalResources = local_items,
                OnlineResources = online_items,
                GitFolder = SelectedGitFolderComboBox.Text
            };

            loadedAssignments.Add(new_assignment);

            TableGrid.ItemsSource = loadedAssignments;
            
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
