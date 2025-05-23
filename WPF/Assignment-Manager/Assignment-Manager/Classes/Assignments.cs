﻿/* 
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

namespace Assignment_Manager.Classes
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
            }
        }


    }
}
