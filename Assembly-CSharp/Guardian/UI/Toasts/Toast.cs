using Guardian.Utilities;

namespace Guardian.UI.Toasts
{
    class Toast
    {
        public string Title;
        public string Message;
        public float TimeToLive;
        public long Now;

        public Toast(string title, string message, float timeToLive)
        {
            this.Title = title;
            this.Message = message;
            this.TimeToLive = timeToLive;
            this.Now = GameHelper.CurrentTimeMillis();
        }
    }
}
