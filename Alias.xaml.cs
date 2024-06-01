/* 
* FILE          : Alias.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2024-05-30
* DESCRIPTION   :
*	This file holds the code behind to allow
*	the user to add/edit link aliases of resources.
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
    /// Interaction logic for Alias.xaml
    /// </summary>
    public partial class Alias : Window
    {
        /* Data members. */
        private static string aliasName = "";  /* The resource's the alias. */
        private static string rawResource = "";       /* The resource to give an alias to. */


        /* Properties. */
        public static string AliasName
        { 
            get { return aliasName; }
            set { aliasName = value; }
        }

        public static string RawResource
        {
            get { return rawResource; }
            set { rawResource = value; }
        }


        /* 
        * METHOD        : askForAlias
        * DESCRIPTION   :
        *   Used by other windows when it wants to manage the alias and 
        *   opens the alias window with the specified resource.
        * PARAMETERS    :
        *   string resource : the resource to give an alias to
        * RETURNS       :
        *   string : the resource plus the alias
        */
        public static string askForAlias(string resource)
        {
            /* Opening dialog for alias. */
            Alias alias_menu = new Alias(resource);
            alias_menu.ShowDialog();

            /* Combinging the alias and the resource. */
            string resource_with_alias = aliasName + "|" + rawResource;

            return resource_with_alias;
        }


        public Alias(string raw_resource)
        {
            InitializeComponent();

            aliasName = "";

            /* Saving and loading the raw resource. */
            rawResource = raw_resource;
            RawResourceTextBlock.Text = raw_resource;
        }


        private void AliasTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Need to add a guard for entering the delimiter '|'
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            aliasName = AliasTextBox.Text;
            Close();
        }
    }
}
