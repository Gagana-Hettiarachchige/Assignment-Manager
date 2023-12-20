/* 
* FILE          : ResourceOpenMenu.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-19
* DESCRIPTION   :
*	This file holds the code behind for the
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
    public partial class ResourceOpenMenu : Window
    {
        public ResourceOpenMenu(List<string> resources)
        {
            InitializeComponent();
        }
    }
}
