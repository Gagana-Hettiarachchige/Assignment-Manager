/* 
* FILE          : ViewModel.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-20
* DESCRIPTION   :
*	This file acts as a view model for the
*	selected assignment, edited assignment, and collections
*	of items.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Assignment_Manager.Classes
{
    internal static class ViewModel
    {
        /* The assignment currently selected. */
        private static Assignment selectedAssignment = new Assignment();

        /* The list of local resources of the currentlly selected assignment. */
        private static ObservableCollection<string> selectedLocalResources = 
                   new ObservableCollection<string>();

        /* The list of online resources of the currentlly selected assignment. */
        private static ObservableCollection<string> selectedOnlineResources = 
                   new ObservableCollection<string>();


        /* The two selected assignments to compare. */
        private static ObservableCollection<AssignmentCompare> selectedAssignmentCompare = 
                   new ObservableCollection<AssignmentCompare>();


        /* Properties. */

        public static Assignment SelectedAssignment 
        {
            get { return selectedAssignment; } 
            set { selectedAssignment = value; } 
        }

        public static ObservableCollection<string> SelectedLocalResources
        { 
            get { return selectedLocalResources; }
            set { selectedLocalResources = value; }
        }

        public static ObservableCollection<string> SelectedOnlineResources
        {
            get { return selectedOnlineResources; }
            set { selectedOnlineResources = value; }
        }

        public static ObservableCollection<AssignmentCompare> SelectedAssignmentCompare
        {
            get { return selectedAssignmentCompare; }
            set { selectedAssignmentCompare = value; }
        }


    }
}
