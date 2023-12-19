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

namespace AssignmentManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private void SchoolButton_Click(object sender, RoutedEventArgs e)
        {
            /* Opens tab to school's webpage. */
            System.Diagnostics.Process.Start("https://conestoga.desire2learn.com/d2l/home");
        }


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


        private void LocalResourcesRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            /* Removing selected resource from the combo box. */
            LocalResourcesComboBox.Items.Remove(LocalResourcesComboBox.SelectedItem);
        }

        private void OnlineResourcesAddButton_Click(object sender, RoutedEventArgs e)
        {
            /* Adding selected resource to the combo box. */
            if (OnlineResourcesTextBox.Text != "")
            {
                OnlineResourcesComboBox.Items.Add(OnlineResourcesTextBox.Text);
                OnlineResourcesComboBox.SelectedIndex = OnlineResourcesComboBox.Items.IndexOf(OnlineResourcesTextBox.Text);
            }
            
        }

        private void OnlineResourcesRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            /* Removing selected resource from the combo box. */
            OnlineResourcesComboBox.Items.Remove(OnlineResourcesComboBox.SelectedItem);
        }

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

        private void GitFolderRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            /* Removing the folder. */
            SelectedGitFolderComboBox.Items.Clear();
        }

        
    }
}
