/* 
* FILE          : Database.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-21
* DESCRIPTION   :
*	This file holds the code to interact with the database.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentManager.CodeFiles
{
    internal static class Database
    {
        /* All assignments in database. */
        public static ObservableCollection<Assignment> databaseAssignments = new ObservableCollection<Assignment>();

    }
}
