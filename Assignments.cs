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
using System.Windows.Diagnostics;

namespace AssignmentManager.CodeFiles
{
    internal class Assignment
    {
        /* Assignment table column default values. */
        private int assignmentNumber = 0;
        private string className = "ABCD";
        private string assignmentName = "A-01";
        private double assignmentWeight = 10.01;
        private DateTime dueDate = new DateTime();
        private string assignmentStatus = "";
        private string localResources = "";
        private string onlineResources = "";

        /* Names of the rows for display purposes. */
        public static readonly List<string> ColumnNames = new List<string>()
        {
            "Number",
            "Class",
            "Name",
            "Weight",
            "Due Date",
            "Status",
            "Local Resources",
            "Online Resources"
        };

        /* Dictionary to iterate through values using the name. */
        public readonly Dictionary<string, string> ColumnValues = new Dictionary<string, string>()
        {
            { ColumnNames[0], "0"},
            { ColumnNames[1], "ABCD"},
            { ColumnNames[2], "A-01" },
            { ColumnNames[3], "10.01" },
            { ColumnNames[4], "yyyy-MM-dd hh:mm:ss tt"},
            { ColumnNames[5], "Incomplete" },
            { ColumnNames[6], "" },
            { ColumnNames[7], "" },
        };


        /* Properties for assignment assignment values. */

        public int AssignmentNumber 
        { 
            get 
            { 
                return assignmentNumber; 
            } 
            set
            { 
                assignmentNumber = value; 
                ColumnValues[ColumnNames[0]] = value.ToString(); 
            } 
        }

        public string ClassName 
        {
            get 
            { 
                return className; 
            }
            set 
            { 
                className = value; 
                ColumnValues[ColumnNames[1]] = value.ToString(); 
            }
        }

        public string AssignmentName 
        {
            get 
            { 
                return assignmentName; 
            }
            set 
            { 
                assignmentName = value; 
                ColumnValues[ColumnNames[2]] = value.ToString(); 
            }
        }

        public double AssignmentWeight 
        {
            get 
            { 
                return assignmentWeight; 
            }
            set 
            { 
                assignmentWeight = value; 
                ColumnValues[ColumnNames[3]] = value.ToString(); 
            }
        }

        public DateTime DueDate 
        {
            get 
            { 
                return dueDate; 
            }
            set 
            { 
                dueDate = value; 
                ColumnValues[ColumnNames[4]] = value.ToString(); 
            }
        }

        public string AssignmentStatus 
        {
            get 
            { 
                return assignmentStatus; 
            }
            set 
            { 
                assignmentStatus = value; 
                ColumnValues[ColumnNames[5]] = value.ToString(); 
            }
        }

        public string LocalResources 
        {
            get 
            { 
                return localResources; 
            }
            set
            { 
                localResources = value; 
                ColumnValues[ColumnNames[6]] = value.ToString(); 
            }
        }

        public string OnlineResources 
        {
            get 
            { 
                return onlineResources; 
            }
            set 
            { 
                onlineResources = value; 
                ColumnValues[ColumnNames[7]] = value.ToString(); 
            }
        }


    }
}
