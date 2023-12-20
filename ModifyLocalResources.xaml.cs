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
        public ModifyLocalResources()
        {
            InitializeComponent();

            foreach (string resource in ViewModel.SelectedLocalResources)
            {
                LocalResourcesList.Items.Add(resource);
            }
        }

        private void AddFileOption_Click(object sender, RoutedEventArgs e)
        {
            /* Setting up dialog for adding files to be able to open. */
            OpenFileDialog files_to_add = new OpenFileDialog();
            files_to_add.Multiselect = true;
            files_to_add.Filter = "Solutions (*.sln)|*.sln|" +
                                  "HTML (*.html)|*.html|" +
                                  "HTM (*.htm)|*.htm|" +
                                  "PHP (*.php)|*.php|" +
                                  "ASP (*.asp)|*.asp|" +
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
        }

        private void RemoveOption_Click(object sender, RoutedEventArgs e)
        {
            /* Removing selected resource from the lists. */
            if (LocalResourcesList.SelectedItem != null)
            {
                /* Removing item from current list. */
                LocalResourcesList.Items.Remove(LocalResourcesList.SelectedItem);

                /* Clearing and updating ViewModel list. */
                ViewModel.SelectedLocalResources.Clear();
                foreach (string resource in LocalResourcesList.Items)
                {
                    ViewModel.SelectedLocalResources.Add(resource);
                }
            }
            

        }
    }
}
