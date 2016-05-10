using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Timers;

namespace TimeManager
{
    internal class TimeManager
    {
        private const long INTERVAL = 5000;
        private const long DAY_LIMIT = 7200000;
        private const long TIME_SPENT = 0;

        private const string LOCAL_MACHINE_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\TimeManager\Common";
        private const string INTERVAL_KEY = "Interval";
        private const string DAY_LIMIT_KEY = "DayLimit";
        private const string TIME_SPENT_KEY = "TimeSpent";
        private const string CURRENT_DATE_KEY = "CurrentDate";

        private long timeSpent;

        private readonly long interval;
        private readonly long timeLimit;
        private readonly Timer _timer;
        private readonly DateTime currentDate;

        public TimeManager()
        {
            interval = GetKeyValue(INTERVAL_KEY) ?? INTERVAL;
            timeLimit = GetKeyValue(DAY_LIMIT_KEY) ?? DAY_LIMIT;
            currentDate = GetDateTimeKeyValue(CURRENT_DATE_KEY) ?? DateTime.Now;

            CheckTimeLimit();

            _timer = new Timer(interval) { AutoReset = true };
            _timer.Elapsed += _timer_Elapsed;
        }

        public void Start() { _timer.Start(); }

        public void Stop() { _timer.Stop(); }

        private bool IsOverTime()
        {
            return currentDate == DateTime.Now.Date && timeSpent >= timeLimit;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timeSpent += interval;

            SetKeyValue(CURRENT_DATE_KEY, DateTime.Now.Date.ToShortDateString());

            CheckTimeLimit();
            
        }

        private void CheckTimeLimit()
        {
            if (IsOverTime())
            {
                SetKeyValue(TIME_SPENT_KEY, timeLimit.ToString());
                ShutDownPc();
            }
            else
            {
                SetKeyValue(TIME_SPENT_KEY, timeSpent.ToString());
            }
        }

        private void ShutDownPc()
        {
            Process.Start("shutdown", "/s /t 0");
        }

        private void SetKeyValue(string keyName, string value)
        {
            Registry.SetValue(LOCAL_MACHINE_KEY, keyName, value);
        }

        private long? GetKeyValue(string keyName)
        {
            return (long?)Registry.GetValue(LOCAL_MACHINE_KEY, keyName, null);
        }

        private DateTime? GetDateTimeKeyValue(string keyName)
        {
            var dateString = (string)Registry.GetValue(LOCAL_MACHINE_KEY, keyName, null);

            DateTime? date = null;
            if (!String.IsNullOrEmpty(dateString))
            {
                date = DateTime.Parse(dateString);
            }

            return date;
        }

        //private void CreateRegistryKey()
        //{

        //}

        //private long GetInterval()
        //{
        //    return 0;
        //}

        //private long GetTimeLimit()
        //{
        //    return 0;
        //}

        //private long GetTimeSpent()
        //{
        //    return 0;
        //}

        //private void UpdateTimeSpent(long value)
        //{

        //}
    }
}
