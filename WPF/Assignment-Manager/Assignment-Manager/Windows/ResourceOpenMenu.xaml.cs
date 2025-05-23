﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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

namespace Assignment_Manager.Windows
{
    /// <summary>
    /// Interaction logic for ResourceOpenMenu.xaml
    /// </summary>
    public partial class ResourceOpenMenu : Window
    {
        //Next thing to do is to correlate between alias and resources in all menus



        /* 
        * METHOD        : ResourceMenuOpen
        * DESCRIPTION   :
        *   Constructor that creates a new 
        *   resource opening dialog.
        * PARAMETERS    :
        *   List<string> resources : the resources to open
        */
        public ResourceOpenMenu(ObservableCollection<string> resources)
        {
            InitializeComponent();

            /* Splitting resource into alias and resource. */
            try
            {
                /* Adding alias and resource in their respective lists. */
                foreach (string resource_with_alias in resources)
                {
                    AliasList.Items.Add(Alias.GetAlias(resource_with_alias));
                    ResourcesList.Items.Add(Alias.GetResource(resource_with_alias));
                }
            }

            catch
            {
                /* Notifying user with error. */
                MessageBox.Show("One or more of the resources in this assignment may have been formatted incorrectly.",
                                "Resource Formatting Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /* 
        * METHOD        : OpenAllOption_Click
        * DESCRIPTION   :
        *   Opens all resources.
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void OpenAllOption_Click(object sender, RoutedEventArgs e)
        {

            /* Used for error displaying. */
            string resource_to_open = "";

            try
            {
                /* Looping through each resource. */
                foreach (string resource in ResourcesList.Items)
                {
                    /* Opening all of them. */
                    resource_to_open = resource;
                    Process.Start('"' + resource + '"');
                }
            }

            catch (Exception error)
            {
                MessageBox.Show(error.Message + '\n' + resource_to_open,
                                "Resource Open Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /* 
        * METHOD        : OpenOption_Click
        * DESCRIPTION   :
        *   Opens selected resources.
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void OpenOption_Click(object sender, RoutedEventArgs e)
        {

            /* Used for error displaying. */
            string resource_to_open = "";

            try
            {
                /* Making sure something is selected. */
                if (ResourcesList.SelectedItems != null)
                {
                    /* Opening the selected resources. */
                    foreach (string resource in ResourcesList.SelectedItems)
                    {
                        resource_to_open = resource;
                        Process.Start('"' + resource + '"');
                    }
                }
            }

            catch (Exception error)
            {
                MessageBox.Show(error.Message + '\n' + resource_to_open,
                                "Resource Open Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /* 
        * METHOD        : OpenCmdOption_Click
        * DESCRIPTION   :
        *   Opens selected resources in CMD with a command.
        * PARAMETERS    :
        *   object sender     : the sender
        *   RoutedEventArgs e : the routed event arguments
        * RETURNS       :
        *   void
        */
        private void OpenCmdOption_Click(object sender, RoutedEventArgs e)
        {

            /* Checking if something is selected. */
            if (ResourcesList.SelectedItems != null)
            {
                try
                {
                    /* Opening CMD for each selected item. */
                    foreach (string resource in ResourcesList.SelectedItems)
                    {

                        //string resource = ResourcesList.SelectedItem.ToString();
                        Uri online_resource;
                        ProcessStartInfo info = new ProcessStartInfo();

                        Uri.TryCreate(resource, UriKind.Absolute, out online_resource);

                        /* Checking if an online resource. */
                        if (online_resource.Scheme == Uri.UriSchemeHttp ||
                            online_resource.Scheme == Uri.UriSchemeHttps)
                        {
                            /* Pinging the domain if online website. */
                            info.WorkingDirectory = @"C:\";
                            info.FileName = "CMD.exe";
                            info.Arguments = "/K ping " + online_resource.Host;
                            Process.Start(info);
                        }

                        else
                        {
                            FileAttributes resource_attributes = File.GetAttributes(resource);

                            /* Checking if it was a directory or File. */
                            if (resource_attributes == FileAttributes.Directory)
                            {
                                /* Running git status on directory. */
                                info.WorkingDirectory = resource;
                                info.FileName = "CMD.exe";
                                info.Arguments = "/K git status";
                                Process.Start(info);
                            }
                            else
                            {
                                string directory = System.IO.Path.GetDirectoryName(resource);

                                /* Running git log on file. */
                                info.WorkingDirectory = directory;
                                info.FileName = "CMD.exe";
                                info.Arguments = "/K git log --name-only " + resource;
                                Process.Start(info);
                            }
                        }
                    }
                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Resource CMD Open Failed",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        /* 
        * METHOD        : ResourcesList_MouseDoubleClick
        * DESCRIPTION   :
        *   Opens selected resources on double click.
        * PARAMETERS    :
        *   object sender          : the sender
        *   MouseButtonEventArgs e : the mouse event arguments
        * RETURNS       :
        *   void
        */
        private void ResourcesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /* Used for error displaying. */
            string resource_to_open = "";

            try
            {
                /* Making sure something is selected. */
                if (ResourcesList.SelectedItems != null)
                {
                    /* Opening the selected resources. */
                    foreach (string resource in ResourcesList.SelectedItems)
                    {
                        resource_to_open = resource;
                        Process.Start('"' + resource + '"');
                    }
                }
            }

            catch (Exception error)
            {
                MessageBox.Show(error.Message + '\n' + resource_to_open,
                                "Resource Open Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /* 
        * METHOD        : AliasList_SelectionChanged
        * DESCRIPTION   :
        *   Correlates the selection highlight between the alias and resource list
        *   when the alias list is clicked on.
        * PARAMETERS    :
        *   object sender               : the sender
        *   SelectionChangedEventArgs e : the items that were added to and removed form selection
        * RETURNS       :
        *   void
        */
        private void AliasList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //    /* Checking which items were selected. */
            //    foreach (string item in e.AddedItems)
            //    {
            //        /* Getting index to correlate with resource list. */
            //        int count = 0;

            //        /* Looping through resources to find the item selected. */
            //        foreach (string resource in AliasList.Items)
            //        {
            //            if (item == resource)
            //            {
            //                /* Adding the corresponding resources to the selected list. */
            //                ResourcesList.SelectedItems.Add(ResourcesList.Items[count]);
            //            }

            //            ++count;
            //        }
            //    }

            //    /* Checking which items were unselected. */
            //    foreach (string item in e.RemovedItems)
            //    {
            //        /* Getting index to correlate with resource list. */
            //        int count = 0;

            //        /* Looping through resources to find the item selected. */
            //        foreach (string resource in AliasList.Items)
            //        {
            //            if (item == resource)
            //            {
            //                /* Removing the corresponding resources from the selected list. */
            //                ResourcesList.SelectedItems.Remove(ResourcesList.Items[count]);
            //            }

            //            ++count;
            //        }
            //    }

            //Temporary fix.
            ResourcesList.SelectedIndex = AliasList.SelectedIndex;

            //ResourcesList.SelectedIndex = AliasList.SelectedItems.IndexOf(this);


        }


        /* 
        * METHOD        : ResourcesList_SelectionChanged
        * DESCRIPTION   :
        *   Correlates the selection highlight between the alias and resources list
        *   when the resources list is clicked on.
        * PARAMETERS    :
        *   object sender               : the sender
        *   SelectionChangedEventArgs e : the items that were added to and removed form selection
        * RETURNS       :
        *   void
        */
        private void ResourcesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Note: there is an issue if duplicates exist in either list and needs to be fixed
            //(either fix here or Not allow duplicate resources)


            ///* Checking which items were selected. */
            //foreach (string item in e.AddedItems)
            //{
            //    /* Getting index to correlate with alias list. */
            //    int count = 0;

            //    /* Looping through resources to find the item selected. */
            //    foreach (string resource in ResourcesList.Items)
            //    {
            //        if (item == resource)
            //        {
            //            /* Adding the corresponding aliases to the selected list. */
            //            AliasList.SelectedItems.Add(AliasList.Items[count]);
            //        }

            //        ++count;
            //    }
            //}

            ///* Checking which items were unselected. */
            //foreach (string item in e.RemovedItems)
            //{
            //    /* Getting index to correlate with alias list. */
            //    int count = 0;

            //    /* Looping through resources to find the item selected. */
            //    foreach (string resource in ResourcesList.Items)
            //    {
            //        if (item == resource)
            //        {
            //            /* Removing the corresponding aliases from the selected list. */
            //            AliasList.SelectedItems.Remove(AliasList.Items[count]);
            //        }

            //        ++count;
            //    }
            //}


        }
    }
}
