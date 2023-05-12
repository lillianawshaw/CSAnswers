using System;
using System.Collections.Generic;
using System.Linq;

namespace Rextester
{
    /// <summary>
    /// The code below checks the return value from GetTimeWorked against several
    /// test cases, defined by the property TestCases. The test cases represent
    /// clock in/out times from a clock-in system.
    /// The values are strings in the format "HH:mm-HH:mm HH:mm-HH:mm". HH:mm represents
    /// the hour and minute of the day in 24-hour format. Times to the left of the dash "-"
    /// are clock-in times, times to the right are clock out times. There are always 4
    /// times - a clock-in followed by a clock-out, then a space (for a lunch break), then
    /// another clock-in/clock-out. We can assume that times on the left always precede the
    /// times on the right, and that all times occur within the same day.
    ///
    /// At the end of each question please save the entire file as the response to that
    /// question. You should send back a total of 3 files.
    ///
    /// 1) Please implement GetTimeWorked so that the test cases are satisfied.
    /// 2) It's now a requirement that each clock-in period cannot exceed 6 hours.
    ///    Please update the test cases, and update your implementation of GetTimeWorked.
    /// 3) It's now a requirement that the minimum period which can be taken for a lunch
    ///    break is 20 minutes. If the break is less than 20 minutes, time must be taken
    ///    out of either the morning or afternoon work period. If either period was more
    ///    than 6 hours, then you should use the excess time from that period to contribute
    ///    to the lunch break. For example, if I work 06:00-12:02 12:20-14:30, I would expect
    ///    that the 2 excess minutes from my morning period would be added to my lunch
    ///    break to make up the 20 minutes. The total working time would be 08:10.
    ///    Please update the test cases, and update your implementation of GetTimeWorked.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Runs test cases for the GetTimeWorked method.
        /// Please don't alter this method.
        /// </summary>
        public static void Main(string[] args)
        {
            
            foreach (var testCase in TestCases)
            {
                var times = testCase.Key;
                var expectedResult = testCase.Value;
                var testResult = String.Empty;

                try
                {
                    var timeWorked = GetTimeWorked(times);
                    testResult = timeWorked == expectedResult
                                 ? String.Format("OK    {0}", timeWorked)
                                 : String.Format("Expected {0} but got {1}", testCase.Value, timeWorked);
                }
                catch (Exception ex)
                {
                    testResult = ex.Message;
                }

                Console.WriteLine("{0}{1}", times.PadRight(27), testResult);
            }
        }

        /// <summary>
        /// Gets the test cases which check the results of GetTimeWorked.
        /// These are correct for the first question. They will need to be
        /// altered for the second and third questions. You may add to the
        /// test cases, but please make sure that the original 5 clock-in
        /// strings remain (the TimeSpan values will need to be updated for
        /// questions 2 and 3).
        /// </summary>
        public static IDictionary<String, TimeSpan> TestCases
        {
            get
            {
                return new Dictionary<String, TimeSpan>
                {
                    { "09:10-12:33 13:07-17:02", TimeSpan.Parse("07:18") },
                    { "08:52-12:13 12:45-17:02", TimeSpan.Parse("07:38") },
                    { "07:22-14:11 14:58 16:09", TimeSpan.Parse("08:00") },
                    { "06:10-12:43 13:00-15:00", TimeSpan.Parse("08:33") },
                    { "09:47-12:32 12:45-18:48", TimeSpan.Parse("08:48") }
                };
            }
        }

        //This I made to deal with a small use case, looking at the spec in order to incorporate would require
        //bloating the spec so please ignore it for now. However could be useful if there are rewrites in the future.


        ////Created this function to calculate the time span from an array of times after normalising the times to adjust for daylight savings
        ////Reason I did this was to increase modulatrity of the program, this code can be reuse in different contexts. 
        ////.ToUniversalTime() is important in this context because if the program was used by night shift workers on the days
        ////which clocks change this would impact accuracte logging and wage calculation. 
        //public static TimeSpan CalculateUniversalTime(TimeSpan[] Time)
        //{
        //    for (int i = 0; i < 2; i++)
        //    {
        //        Time[i] = Time[i].ToUniversalTime();
                
        //    }
        //    return Time[1] - Time[0];

        //}

        /// <summary>
        /// Given a string in the format "HH:mm-HH:mm HH:mm-HH:mm" works
        /// out how long an employee was clocked in for. ("09:10-12:33 13:07-17:02")
        /// </summary>
        public static TimeSpan GetTimeWorked(String times)
        {

                // Split the string at the first space and return the resulting array
            var Morning = times.Substring(0, 11);
            var Afternoon = times.Substring(12);

            // split the times into 4 objects for calculations
            var MorningIn = TimeSpan.Parse(Morning.Substring(0, 5));
            var MorningOut = TimeSpan.Parse(Morning.Substring(6));
            
            var AfternoonIn = TimeSpan.Parse(Afternoon.Substring(0, 5));
            var AfternoonOut = TimeSpan.Parse(Afternoon.Substring(6));
            //calculate each block time
            TimeSpan MorningTime = MorningOut - MorningIn;
            TimeSpan AfternoonTime = AfternoonOut - AfternoonIn;
            //return total time
            return MorningTime + AfternoonTime;
        }
    }
}