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
        /* Assignment table column values. */
        private int assignmentNumber = 0;
        private string className = "";
        private string assignmentName = "";
        private double assignmentWeight = 0.0;
        private DateTime dueDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 00, 00);
        private string assignmentStatus = "";
        private string localResources = "";
        private string onlineResources = "";
        private string gitFolder = "";

        /* Properties for assignment assignment values. */
        public int AssignmentNumber 
        { 
            get { return assignmentNumber; } 
            set { assignmentNumber = value; } 
        }
        public string ClassName 
        {
            get { return className; }
            set { className = value; }
        }
        public string AssignmentName 
        {
            get { return assignmentName; }
            set { assignmentName = value; }
        }
        public double AssignmentWeight 
        {
            get { return assignmentWeight; }
            set { assignmentWeight = value; }
        }
        public DateTime DueDate 
        {
            get { return dueDate; }
            set { dueDate = value; }
        }
        public string AssignmentStatus 
        {
            get { return assignmentStatus; }
            set { assignmentStatus = value; }
        }
        public string LocalResources 
        {
            get { return localResources; }
            set { localResources = value; }
        }
        public string OnlineResources 
        {
            get { return onlineResources; }
            set { onlineResources = value; }
        }
        public string GitFolder 
        {
            get { return gitFolder; }
            set { gitFolder = value; }
        }



    }
}
