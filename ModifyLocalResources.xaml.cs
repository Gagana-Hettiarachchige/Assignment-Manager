/* 
* FILE          : ModifyLocalResources.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-20
* DESCRIPTION   :
*	This file holds the code behind to allow
*	the use to modify their local resources.
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace AssignmentManager.CodeFiles
{
    /// <summary>
    /// Interaction logic for ModifyLocalResources.xaml
    /// </summary>
    public partial class ModifyLocalResources : Window
    {



        /* 
        * METHOD        : ModifyLocalResources
        * DESCRIPTION   :
        *   Constructor for this screen.
        * PARAMETERS    :
        *   void
        */
        public ModifyLocalResources()
        {
            InitializeComponent();

            foreach (string resource in ViewModel.SelectedLocalResources)
            {
                LocalResourcesList.Items.Add(resource);
            }
        }


        /* 
        * METHOD        : AddFileOption_Click
        * DESCRIPTION   :
        *   Raised when the add file button is clicked and adds the
        *   resource to the list box and view model.
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event args
        * RETURNS       :
        *   void
        */
        private void AddFileOption_Click(object sender, RoutedEventArgs e)
        {
            /* Setting up dialog for adding files to be able to open. */
            OpenFileDialog files_to_add = new OpenFileDialog();
            files_to_add.Multiselect = true;
            files_to_add.Filter = "Solutions (*.sln)|*.sln|" +
                                  "C Files (*.c)|*.c|" +
                                  "C++ Files (*.cpp)|*.cpp|" +
                                  "C# Files (*.cs)|*.cs|" +
                                  "XAML Files (*.xaml)|*.xaml|" +
                                  "Config Files (*.confog)|*.config|" +
                                  "HTML Files (*.html)|*.html|" +
                                  "HTM Files (*.htm)|*.htm|" +
                                  "PHP Files (*.php)|*.php|" +
                                  "ASP Files (*.asp)|*.asp|" +
                                  "ASPX Files (*.aspx)|*.aspx|" +
                                  "CSS Files (*.css)|*.css|" +
                                  "SQL Files (*.sql)|*.sql|" +
                                  "Text Files (*.txt)|*.txt|" +
                                  "All files |*.*";

            /* Asking user to pick resources. */
            Nullable<bool> choice = files_to_add.ShowDialog();

            if (choice == true)
            {
                int count = 0;

                /* Adding resources to the lists. */
                while (count < files_to_add.FileNames.Length)
                {
                    LocalResourcesList.Items.Add(files_to_add.FileNames[count]);
                    ViewModel.SelectedLocalResources.Add(files_to_add.FileNames[count]);
                    ++count;
                }
            }
        }


        /* 
        * METHOD        : AddFolderOption_Click
        * DESCRIPTION   :
        *   Raised when the add folder button is clicked and adds the
        *   resource to the list box and view model.
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event args
        * RETURNS       :
        *   void
        */
        private void AddFolderOption_Click(object sender, RoutedEventArgs e)
        {
            /* Setting up dialog for selecting a folder. */
            CommonOpenFileDialog folder_to_add = new CommonOpenFileDialog();
            folder_to_add.IsFolderPicker = true;

            /* Asking user for folder. */
            if (folder_to_add.ShowDialog() == CommonFileDialogResult.Ok)
            {
                /* Adding folder to the lists. */
                LocalResourcesList.Items.Add(folder_to_add.FileName);
                ViewModel.SelectedLocalResources.Add(folder_to_add.FileName);
            }

            this.WindowState = WindowState.Maximized;
            this.WindowState = WindowState.Normal;
        }


        /* 
        * METHOD        : RemoveOption_Click
        * DESCRIPTION   :
        *   Raised when the remove button is clicked and removes the
        *   slected resources from the list box and view model.
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event args
        * RETURNS       :
        *   void
        */
        private void RemoveOption_Click(object sender, RoutedEventArgs e)
        {
            /* Checking if a resource is actually selected. */
            if (LocalResourcesList.SelectedItems != null)
            {
                /* Removing selected resources from UI. */
                List<string> selected_resources = new List<string>();

                foreach (string resource in LocalResourcesList.SelectedItems)
                {
                    selected_resources.Add(resource);
                }

                foreach (string resource in selected_resources)
                {
                    LocalResourcesList.Items.Remove(resource);
                }

                /* Clearing and updating ViewModel list. */
                ViewModel.SelectedLocalResources.Clear();
                foreach (string resource in LocalResourcesList.Items)
                {
                    ViewModel.SelectedLocalResources.Add(resource);
                }
            }
            

        }


        /* 
        * METHOD        : WindowClosing
        * DESCRIPTION   :
        *   Raised when the window is closing and saves the selected resources
        *   into the view model as a string.
        * PARAMETERS    :
        *   object sender                           : the sender
        *   System.ComponentModel.CancelEventArgs e : the cancel event args
        * RETURNS       :
        *   void
        */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /* Clearing out view model's selected local resources. */
            ViewModel.SelectedAssignment.LocalResources = "";

            /* Looping through the local resources list. */
            foreach (string resource in ViewModel.SelectedLocalResources)
            {
                /* Adding all local resources to single string. */
                ViewModel.SelectedAssignment.LocalResources += (resource + '\n');
            }
        }
    }
}
