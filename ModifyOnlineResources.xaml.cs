/* 
* FILE          : ModifyOnlineResources.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-20
* DESCRIPTION   :
*	This file is the code behind for the menu to
*	modify the online resources.
*/

using System;
using System.Collections.Generic;
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
    /// Interaction logic for ModifyOnlineResources.xaml
    /// </summary>
    public partial class ModifyOnlineResources : Window
    {
        public ModifyOnlineResources()
        {
            InitializeComponent();

            /* Adding the selected assignment's resources to the list. */
            foreach (string resource in ViewModel.SelectedOnlineResources)
            {
                OnlineResourcesList.Items.Add(resource);
            }
        }

        private void OnlineResourceAdd_Click(object sender, RoutedEventArgs e)
        {
            /* Checking if resource to add is not empty. */
            if (OnlineResourceTextBox.Text != "")
            {
                /* Adding entered resource to the lists. */
                OnlineResourcesList.Items.Add(OnlineResourceTextBox.Text);
                ViewModel.SelectedOnlineResources.Add(OnlineResourceTextBox.Text);
            }
        }

        private void OnlineResourceRemove_Click(object sender, RoutedEventArgs e)
        {

            /* Checking if resource to remove is not empty. */
            if (OnlineResourcesList.SelectedItem != null)
            {
                /* Removing resource from UI. */
                OnlineResourcesList.Items.Remove(OnlineResourcesList.SelectedItem);

                /* Clearing and adding the remaining resources to the view model's list. */
                ViewModel.SelectedOnlineResources.Clear();
                foreach (string resource in OnlineResourcesList.Items)
                {
                    OnlineResourcesList.Items.Add(resource);
                }
            }
            
        }
    }
}
