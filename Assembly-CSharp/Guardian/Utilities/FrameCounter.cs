namespace Guardian.Utilities
{
    class FrameCounter
    {
        public int FrameCount;

        private readonly MsTimer Timer = new MsTimer();

        private int CurrentFrameCount;

        public void UpdateCounter()
        {
            CurrentFrameCount++;

            if (!Timer.HasPassed(1000)) return;

            FrameCount = CurrentFrameCount;
            CurrentFrameCount = 0;
            Timer.Update();
        }
    }
}
