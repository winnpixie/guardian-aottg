namespace Guardian.Utilities
{
    class FrameCounter
    {
        public int FrameCount;

        private int CurrentFrameCount;
        private long LastPollTime;

        public void UpdateCounter()
        {
            CurrentFrameCount++;

            if (GameHelper.CurrentTimeMillis() - LastPollTime < 1000) return;

            FrameCount = CurrentFrameCount;
            CurrentFrameCount = 0;
            LastPollTime = GameHelper.CurrentTimeMillis();
        }
    }
}
