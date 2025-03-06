/* 
* FILE          : AssignmentCompare.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-22
* DESCRIPTION   :
*	This file holds the class defintion for 
*	AssignmentCompare which is an object that holds
*	column names, the old assignment's values 
*	(the previous selected assignment values), and 
*	the new assignment's values 
*	(the selcted assignment's potential change or new
*	assignment to add to the database).
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Manager.Classes
{
    internal class AssignmentCompare
    {
        /* Data members. */
        private string columnName = ""; /* Each column name in an assignment. */
        private string oldValue = ""; /* Old value for each column. */
        private string newValue = ""; /* New value for each column. */

        /* Properties. */
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        public string OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }

        public string NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }


    }
}