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
            Title = "Open Resources";
            /* Need to load resources into a control. */
        }

        private void OpenAllOption_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenOption_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenCmdOption_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
