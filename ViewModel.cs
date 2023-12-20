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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentManager.CodeFiles
{
    internal static class ViewModel
    {
        /* The assignment currently selected. */
        private static Assignment selectedAssignment = new Assignment();

        /* Properties. */

        public static Assignment SelectedAssignment 
        {
            get { return selectedAssignment; } 
            set { selectedAssignment = value; } 
        }

    }
}
