using System;

namespace ExitGames.Client.DemoParticle
{
    public class TimeKeeper
    {
        private int lastExecutionTime = Environment.TickCount;
        private bool shouldExecute;

        public int Interval
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        public bool ShouldExecute
        {
            get
            {
                return IsEnabled && (shouldExecute || Environment.TickCount - lastExecutionTime > Interval);
            }
            set
            {
                shouldExecute = value;
            }
        }

        public TimeKeeper(int interval)
        {
            IsEnabled = true;
            Interval = interval;
        }

        public void Reset()
        {
            shouldExecute = false;
            lastExecutionTime = Environment.TickCount;
        }
    }
}
