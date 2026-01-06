namespace Guardian.Utilities
{
    public class FrameCounter
    {
        public int FrameCount;

        private readonly MsTimer _timer = new MsTimer();

        private int _frameCount;

        public void UpdateCounter()
        {
            _frameCount++;

            if (!_timer.HasPassed(1000)) return;

            FrameCount = _frameCount;
            _frameCount = 0;
            _timer.Update();
        }
    }
}