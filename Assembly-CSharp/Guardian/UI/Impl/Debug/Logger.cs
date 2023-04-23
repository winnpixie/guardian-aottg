using Guardian.Utilities;
using System;
using UnityEngine;

namespace Guardian.UI.Impl.Debug
{
    class Logger
    {
        public SynchronizedList<Entry> Entries = new SynchronizedList<Entry>();
        public Vector2 ScrollPosition = GameHelper.ScrollBottom;

        private void Log(string message)
        {
            message = GameHelper.DangerousTagsPattern.Replace(message, string.Empty);
            if (message.Length < 1) return;

            Entries.Add(new Entry(message));

            if (Entries.Count > GuardianClient.Properties.MaxLogLines.Value)
            {
                Entries.RemoveAt(0);
            }

            ScrollPosition = GameHelper.ScrollBottom;
        }

        public void Info(string message)
        {
            Log("* ".AsColor("AAAAAA") + message);
        }

        public void Warn(string message)
        {
            Log("* ".AsColor("FFCC00") + message);
        }

        public void Error(string message)
        {
            Log("* ".AsColor("FF0000") + message);
        }

        public void Debug(string message)
        {
            Log("* ".AsColor("00FFFF") + message);
        }

        public class Entry
        {
            public string Text;
            public long Time;
            public string Timestamp;

            public Entry(string text)
            {
                Text = text;
                Time = GameHelper.CurrentTimeMillis();

                DateTime date = GameHelper.Epoch.AddMilliseconds(Time).ToLocalTime();
                Timestamp = date.ToString("HH:mm:ss");
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
