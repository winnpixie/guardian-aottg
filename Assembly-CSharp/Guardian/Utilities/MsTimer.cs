namespace Guardian.Utilities
{
    class MsTimer
    {
        private long Marker;

        public MsTimer()
        {
            Update();
        }

        public void Update()
        {
            Marker = GetNow();
        }

        public bool HasPassed(long ms)
        {
            return GetElapsed() >= ms;
        }

        public long GetElapsed()
        {
            return GetNow() - Marker;
        }

        public static long GetNow()
        {
            return GameHelper.CurrentTimeMillis();
        }
    }
}
