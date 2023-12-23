/* 
* FILE          : DueDatePicker.xaml.cs
* PROJECT       : Assignment Manager
* PROGRAMMER    : Gagana Hettiarachchige
* FIRST VERSION : 2023-12-20
* DESCRIPTION   :
*	Holds the code behind of the due date picker screen.
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
    /// Interaction logic for DueDatePicker.xaml
    /// </summary>
    public partial class DueDatePicker : Window
    {
        private string dueDay = "";
        private string dueHour = "";



        /* Constructor. */
        public DueDatePicker()
        {
            InitializeComponent();

            /* Defaulting to selected date. */
            DueDayPicker.SelectedDate = ViewModel.SelectedAssignment.DueDate;
            HourValue.Text = ViewModel.SelectedAssignment.DueDate.ToString("hh");
            MinuteValue.Text = ViewModel.SelectedAssignment.DueDate.ToString("mm");
            AmPmValue.Text = ViewModel.SelectedAssignment.DueDate.ToString("tt");
        }

        

        /* Methods. */

        /* 
        * METHOD        : ChangeTime
        * DESCRIPTION   :
        *   Increments/decrements the time value by the
        *   time change (negative or positibe) and keeps
        *   the value inbetween a min and max.
        * PARAMETERS    :
        *   string time_value : the time value
        *   int time_change   : the amount to change by (+/-)
        *   int value_min     : the minimum the value can be
        *   int value_max     : the maximum the value can be
        * RETURNS       :
        *   string : the updated time value
        */
        private string ChangeTime(string time_value, int time_change, 
                                  int value_min, int value_max)
        {
            int hour = int.Parse(time_value);

            /* Incrementing/decrementing the hour. */
            hour += time_change;

            /* Wrapping time around. */
            if (hour > value_max)
            {
                hour = value_min;
            }

            else if (hour < value_min)
            {
                hour = value_max;
            }


            string hour_string;

            /* Adding a leading 0 if single digit number. */
            if (hour < 10)
            {
                hour_string = "0" + hour.ToString();
            }
            else
            {
                hour_string = hour.ToString();
            }

            return hour_string;
        }


        /* 
        * METHOD        : 
        * DESCRIPTION   :
        *   Switches the AM/PM value.
        * PARAMETERS    :
        *   string current_value : current AM/PM
        * RETURNS       :
        *   string : the switched value
        */
        private string SwitchAmPm(string current_value)
        {
            /* Checking value and using opposite. */
            if (current_value == "AM")
            {
                return "PM";
            }

            else if (current_value == "PM")
            {
                return "AM";
            }

            else
            {
                return "?";
            }
        }



        /* Event handlers. */

        private void DueDayPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime due_date_time = (DateTime)DueDayPicker.SelectedDate;
            dueDay = due_date_time.ToString("yyyy-MM-dd");
        }

        private void HourIncrementButton_Click(object sender, RoutedEventArgs e)
        {
            /* Incrementing and saving the hour. */
            HourValue.Text = ChangeTime(HourValue.Text, 1, 1, 12);
        }

        private void HourDecrementButton_Click(object sender, RoutedEventArgs e)
        {
            /* Decrementing and saving the hour. */
            HourValue.Text = ChangeTime(HourValue.Text, -1, 1, 12);
        }

        private void MinuteIncrementButton_Click(object sender, RoutedEventArgs e)
        {
            /* Incrementing and saving the minute. */
            MinuteValue.Text = ChangeTime(MinuteValue.Text, 1, 0, 59);
        }

        private void MinuteDecrementButton_Click(object sender, RoutedEventArgs e)
        {
            /* Decrementing and saving the minute. */
            MinuteValue.Text = ChangeTime(MinuteValue.Text, -1, 0, 59);
        }

        private void AmPmValueButton_Click(object sender, RoutedEventArgs e)
        {
            /* Switching AM to PM and PM to AM. */
            AmPmValue.Text = SwitchAmPm(AmPmValue.Text);
        }

        private void SaveDueDateButton_Click(object sender, RoutedEventArgs e)
        {
            /* Getting the selected due day */
            DateTime day = (DateTime)DueDayPicker.SelectedDate;

            /* Creating full date and time. */
            dueDay = day.ToString("yyyy-MM-dd");
            dueHour = HourValue.Text + ':' + MinuteValue.Text + ":00 " + AmPmValue.Text;

            /* Saving in view model. */
            ViewModel.SelectedAssignment.DueDate = DateTime.Parse(dueDay + ' ' + dueHour);

            /* Closing the window. */
            Close();
        }
    }
}
