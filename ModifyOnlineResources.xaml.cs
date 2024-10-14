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
            foreach (string resource_with_alias in ViewModel.SelectedOnlineResources)
            {
                /* Splitting resource into alias and resource. */
                try
                {
                    /* Adding alias and resource in their respective lists. */
                    AliasList.Items.Add(Alias.GetAlias(resource_with_alias));
                    OnlineResourcesList.Items.Add(Alias.GetResource(resource_with_alias));
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
                        /* Asking user if they want an alias for this specific resource. */
                        string new_resource = Alias.AskForAlias(OnlineResourceTextBox.Text);

                        /* Adding entered resource to the lists. */
                        ViewModel.SelectedOnlineResources.Add(new_resource);
                        AliasList.Items.Add(Alias.GetAlias(new_resource));
                        OnlineResourcesList.Items.Add(Alias.GetResource(new_resource));
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
        *   slected resources from the list box and view model.
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
                /* Removing selected resources and aliases from UI. */
                List<string> selected_resources = new List<string>();
                List<string> selected_aliases = new List<string>();

                int index = OnlineResourcesList.SelectedIndex;

                /* Getting all the selected aliases and resources. */
                foreach (string resource in OnlineResourcesList.SelectedItems)
                {
                    selected_resources.Add(resource);

                    /* Using selected index to correlate with the alias list. */
                    selected_aliases.Add(AliasList.Items[index].ToString());
                    ++index;
                }

                /* Removing every selected resource from the list. */
                foreach (string resource in selected_resources)
                {
                    OnlineResourcesList.Items.Remove(resource);
                }

                /* Removing every selected alias from the list. */
                foreach (string alias in selected_aliases)
                {
                    AliasList.Items.Remove(alias);
                }

                /* Clearing and adding the remaining resources to the view model's list. */
                ViewModel.SelectedOnlineResources.Clear();

                /* Alias index. */
                int count = 0;

                foreach (string resource in OnlineResourcesList.Items)
                {
                    /* Getting corresponding alias */
                    string alias = AliasList.Items[count].ToString();

                    /* Combining with delimiter and resource before adding to local resources list.*/
                    ViewModel.SelectedOnlineResources.Add(Alias.CombineAliasResource(alias, resource));

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


        private void AliasList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}