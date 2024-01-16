/* 
* FILE          : SettingsMenu.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2024-01-15
* DESCRIPTION   :
*	This file holds the logic for updating the UI and settings.
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

namespace AssignmentManager.CodeFiles
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : Window
    {
        public SettingsMenu()
        {
            InitializeComponent();

            /* Adding the settings categories. */
            CategoriesListBox.Items.Add("Visual");
            CategoriesListBox.Items.Add("Notifications");
            CategoriesListBox.Items.Add("Key Binding");
            CategoriesListBox.Items.Add("Database");

            /* Setting default selected to first. */
            CategoriesListBox.SelectedIndex = 0;

            /* Getting settings. */
            /* Loading in the visual settings as is. */
            string current_setting = ConfigurationManager.AppSettings["animationsEnabled"];
            if (current_setting == "true")
            {
                AnimationToggleButton.Content = "True";
            }
            else if (current_setting == "false") 
            {
                AnimationToggleButton.Content = "False";
            }
        }

        private void CategoriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesListBox.SelectedItem != null) 
            {

                /* Checking if visual was selected. */
                if (CategoriesListBox.SelectedItem.ToString() == "Visual")
                {
                    /* Making the visual settings visible. */
                    VisualSettings.Visibility = Visibility.Visible;
                }
                else
                {
                    VisualSettings.Visibility = Visibility.Hidden;
                }
            }
        }

        private void AnimationToggleButton_Click(object sender, RoutedEventArgs e)
        {
            /* Switching the button. */
            if (AnimationToggleButton.Content.ToString() == "True")
            {
                AnimationToggleButton.Content = "False";
            }

            else if (AnimationToggleButton.Content.ToString() == "False")
            {
                AnimationToggleButton.Content = "True";
            }

            /* Updating settings. */
            Configuration settings = ConfigurationManager.OpenExeConfiguration
                                     (ConfigurationUserLevel.None);
            settings.AppSettings.Settings["animationsEnabled"].Value = AnimationToggleButton.Content.ToString().ToLower();
            settings.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
