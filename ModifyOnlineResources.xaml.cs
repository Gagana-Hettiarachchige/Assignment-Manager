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


        /* 
        * METHOD        : ModifyOnlineResources
        * DESCRIPTION   :
        *   Constructor for this screen.
        * PARAMETERS    :
        *   void
        */
        public ModifyOnlineResources()
        {
            InitializeComponent();

            /* Adding the selected assignment's resources to the list. */
            foreach (string resource in ViewModel.SelectedOnlineResources)
            {
                OnlineResourcesList.Items.Add(resource);
            }
        }


        /* 
        * METHOD        : OnlineResourceAdd_Click
        * DESCRIPTION   :
        *   Raised when the add button is clicked and adds the
        *   resource(s) to the list box and view model.
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event args
        * RETURNS       :
        *   void
        */
        private void OnlineResourceAdd_Click(object sender, RoutedEventArgs e)
        {
            /* Checking if resource to add is not empty. */
            if (OnlineResourceTextBox.Text != "")
            {

                /* Checking if the resource is an actual online resource. */
                try
                {
                    Uri online_resource;
                    Uri.TryCreate(OnlineResourceTextBox.Text, UriKind.Absolute, out online_resource);

                    if (online_resource.Scheme == Uri.UriSchemeHttp ||
                       online_resource.Scheme == Uri.UriSchemeHttps)
                    {
                        /* Adding entered resource to the lists. */
                        OnlineResourcesList.Items.Add(OnlineResourceTextBox.Text);
                        ViewModel.SelectedOnlineResources.Add(OnlineResourceTextBox.Text);
                    }

                    else
                    {
                        throw new Exception();
                    }
                }

                catch
                {
                    /* Notifying user with error. */
                    MessageBox.Show("Online resources must be URLs.", "Online Resource Add Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }

                
            }
        }


        /* 
        * METHOD        : OnlineResourceRemove_Click
        * DESCRIPTION   :
        *   Raised when the remove button is clicked and removes the
        *   slected assingments from the list box and view model.
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event args
        * RETURNS       :
        *   void
        */
        private void OnlineResourceRemove_Click(object sender, RoutedEventArgs e)
        {

            /* Checking if resources to remove are not empty. */
            if (OnlineResourcesList.SelectedItems != null)
            {
                /* Removing selected resources from UI. */
                List<string> selected_resources = new List<string>();

                foreach (string resource in OnlineResourcesList.SelectedItems)
                {
                    selected_resources.Add(resource);
                }

                foreach (string resource in selected_resources)
                {
                    OnlineResourcesList.Items.Remove(resource);
                }

                /* Clearing and adding the remaining resources to the view model's list. */
                ViewModel.SelectedOnlineResources.Clear();
                foreach (string resource in OnlineResourcesList.Items)
                {
                    ViewModel.SelectedOnlineResources.Add(resource);
                }
            }
            
        }


        /* 
        * METHOD        : WindowClosing
        * DESCRIPTION   :
        *   Raised when the window is closing and saves the selected assignments
        *   into the view model as a string.
        * PARAMETERS    :
        *   object sender                           : the sender
        *   System.ComponentModel.CancelEventArgs e : the cancel event args
        * RETURNS       :
        *   void
        */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /* Clearing out view model's selected online resources. */
            ViewModel.SelectedAssignment.OnlineResources = "";

            /* Looping through the online resources list. */
            foreach (string resource in ViewModel.SelectedOnlineResources)
            {
                /* Adding all online resources to single string. */
                ViewModel.SelectedAssignment.OnlineResources += (resource + '\n');
            }
        }


        /* 
        * METHOD        : OnlineResourceClear_Click
        * DESCRIPTION   :
        *   Clears the online resource text box.
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event args
        * RETURNS       :
        *   void
        */
        private void OnlineResourceClear_Click(object sender, RoutedEventArgs e)
        {
            /* Clearing the input text box. */
            OnlineResourceTextBox.Text = "";
        }
    }
}
