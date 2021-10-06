namespace Guardian.Utilities
{
    class FrameCounter
    {
        public int Frames;

        private int _frames;
        private long _lastUpdate;

        public void UpdateCounter()
        {
            _frames++;

            if (GameHelper.CurrentTimeMillis() - _lastUpdate >= 1000)
            {
                Frames = _frames;
                _frames = 0;

                _lastUpdate = GameHelper.CurrentTimeMillis();
            }
        }
    }
}
