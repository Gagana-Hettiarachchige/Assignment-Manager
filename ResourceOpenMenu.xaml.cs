/* 
* FILE          : ResourceOpenMenu.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-19
* DESCRIPTION   :
*   This file holds the code behind for the
*	menu that allows the user to open
*	certain or all resources.
*/

using AssignmentManager.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.IO;

namespace AssignmentManager.CodeFiles
{
    /// <summary>
    /// Interaction logic for ResourceOpenMenu.xaml
    /// </summary>
    
    /*
    * NAME	  : ResourceOpenMenu
    * PURPOSE : The resource open menu allows for the user to
    *           select which resources passed they want to open.
    */
    public partial class ResourceOpenMenu : Window
    {


        /* 
        * METHOD        : ResourceMenuOpen
        * DESCRIPTION   :
        *   Constructor that creates a new 
        *   resource opening dialog.
        * PARAMETERS    :
        *   List<string> resources : the resources to open
        */
        public ResourceOpenMenu(List<string> resources)
        {
            InitializeComponent();

            /* Putting the resources into the listbox. */
            ResourcesList.ItemsSource = resources;
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
    }
}
