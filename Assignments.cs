/* 
* FILE          : Assignments.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-18
* DESCRIPTION   :
*	Holds the definiton of a class that is to be
*	used as values in the data grid.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AssignmentManager.CodeFiles
{
    internal class Assignment
    {
        /* Assignment table columns. */
        public int AssignmentNumber { get; set; }
        public string ClassName { get; set; }
        public string AssignmentName { get; set; }
        public double AssignmentWeight { get; set; }
        public DateTime DueDate { get; set; }
        public string AssignmentStatus { get; set; }
        public string LocalResources { get; set; }
        public string OnlineResources { get; set; }
        public string GitFolder { get; set; }



    }
}
