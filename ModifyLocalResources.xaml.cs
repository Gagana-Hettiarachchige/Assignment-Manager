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

            foreach (string resource_with_alias in ViewModel.SelectedLocalResources)
            {
                /* Splitting resource into alias and resource. */
                try
                {
                    /* Adding alias and resource in their respective lists. */
                    AliasList.Items.Add(Alias.GetAlias(resource_with_alias));
                    LocalResourcesList.Items.Add(Alias.GetResource(resource_with_alias));
                }

                catch 
                {
                    /* Notifying user with error. */
                    MessageBox.Show("One or more of the resources in this assignment may have been formatted incorrectly.", 
                                    "Resource Formatting Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
            files_to_add.Filter = "All files |*.*|" + 
                                  "Solutions (*.sln)|*.sln|" +
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
                                  "Text Files (*.txt)|*.txt";

            /* Asking user to pick resources. */
            Nullable<bool> choice = files_to_add.ShowDialog();

            if (choice == true)
            {
                int count = 0;

                /* Adding resources to the lists. */
                while (count < files_to_add.FileNames.Length)
                {
                    /* Asking user if they want an alias for this specific resource. */
                    string new_resource = Alias.AskForAlias(files_to_add.FileNames[count]);

                    /* Adding the resource to the list and the selected local resources list. */
                    LocalResourcesList.Items.Add(Alias.GetResource(new_resource));
                    ViewModel.SelectedLocalResources.Add(new_resource);

                    /* Adding the alias to the seperate list. */
                    AliasList.Items.Add(Alias.GetAlias(new_resource));


                    //LocalResourcesList.Items.Add(files_to_add.FileNames[count]);
                    //ViewModel.SelectedLocalResources.Add(files_to_add.FileNames[count]);
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
                List<string> selected_aliases = new List<string>();

                int index = LocalResourcesList.SelectedIndex;

                /* Getting all the selected aliases and resources. */
                foreach (string resource in LocalResourcesList.SelectedItems)
                {
                    selected_resources.Add(resource);

                    /* Using selected index to correlate with the alias list. */
                    selected_aliases.Add(AliasList.Items[index].ToString());
                    ++index;
                }

                /* Removing every selected resource from the list. */
                foreach (string resource in selected_resources)
                {
                    LocalResourcesList.Items.Remove(resource);
                }

                /* Removing every selected alias from the list. */
                foreach (string alias in selected_aliases)
                {
                    AliasList.Items.Remove(alias);
                }

                /* Clearing and updating ViewModel list. */
                ViewModel.SelectedLocalResources.Clear();


                //This is assuming that the resource list is the one that is clicked
                //Needs to later allow selecting the resource alias combo using the alias list as well

                /* Alias index. */
                int count = 0;

                /* Updating the selected local resources list in the view model. */
                foreach (string resource in LocalResourcesList.Items)
                {
                    /* Getting corresponding alias */ 
                    string alias = AliasList.Items[count].ToString();

                    /* Combining with delimiter and resource before adding to local resources list.*/
                    ViewModel.SelectedLocalResources.Add(Alias.CombineAliasResource(alias, resource));

                    ++count;
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



        //Testing.
        private void LocalResourcesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //AliasList.SelectedIndex = LocalResourcesList.SelectedIndex;
            //MessageBox.Show(LocalResourcesList.SelectedIndex.ToString());
        }
    }
}