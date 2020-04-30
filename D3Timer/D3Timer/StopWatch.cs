using System;

namespace D3Timer
{
    public class StopWatch
    {
        private DateTime startTime;
        private DateTime currentTime;
        private bool running = false;
        public void Start()
        {
            this.startTime = DateTime.Now;
            this.running = true;
        }

        public void Stop()
        {
            this.running = false;
        }

        public string GetElapsedTimeString()
        {
            return running ? FormatTimeString(GetElapsedTime()) : "00:00.000";
        }

        private TimeSpan GetElapsedTime()
        {
            this.currentTime = DateTime.Now;
            return currentTime - startTime;
        }

        private String FormatTimeString(TimeSpan interval)
        {
            return String.Format("{0}:{1}.{2}", 
                interval.Minutes.ToString("00"), 
                interval.Seconds.ToString("00"), 
                interval.Milliseconds.ToString("000"));
        }
    }
}
