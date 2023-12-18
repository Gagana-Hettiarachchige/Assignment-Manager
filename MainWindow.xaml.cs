/* 
* FILE          : MainWindow.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-17
* DESCRIPTION   :
*	Holds the code behind of the main window.
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void SchoolBtn_Click(object sender, RoutedEventArgs e)
        {
            /* Opens tab to school's webpage. */
            System.Diagnostics.Process.Start("https://conestoga.desire2learn.com/d2l/home");
        }
    }
}
