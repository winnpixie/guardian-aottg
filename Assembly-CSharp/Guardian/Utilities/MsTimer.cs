namespace Guardian.Utilities
{
    class MsTimer
    {
        private long _marker;

        public MsTimer()
        {
            Reset();
        }

        public void Reset()
        {
            _marker = GetNow();
        }

        public bool HasPassed(long ms)
        {
            return GetElapsed() >= ms;
        }

        public long GetElapsed()
        {
            return GetNow() - _marker;
        }

        public static long GetNow()
        {
            return GameHelper.CurrentTimeMillis();
        }
    }
}
