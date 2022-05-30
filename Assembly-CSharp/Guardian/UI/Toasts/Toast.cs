using Guardian.Utilities;
using System;

namespace Guardian.UI.Toasts
{
    class Toast
    {
        public string Title;
        public string Message;
        public float TimeToLive;
        public long Time;
        public string Timestamp;

        public Toast(string title, string message, float timeToLive)
        {
            this.Title = title;
            this.Message = message;
            this.TimeToLive = timeToLive;
            this.Time = GameHelper.CurrentTimeMillis();

            DateTime date = GameHelper.Epoch.AddMilliseconds(this.Time).ToLocalTime();
            this.Timestamp = date.ToString("HH:mm:ss");
        }
    }
}
