namespace Guardian.Utilities
{
    class MsTimer
    {
        private long _marker;

        public MsTimer()
        {
            Update();
        }

        public void Update()
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
