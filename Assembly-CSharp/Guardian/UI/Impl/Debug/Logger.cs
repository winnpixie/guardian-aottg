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
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            Entries.Add(new Entry(message));

            if (Entries.Count > GuardianClient.Properties.MaxLogEntries.Value)
            {
                Entries.RemoveAt(0);
            }

            ScrollPosition = GameHelper.ScrollBottom;
        }

        public void Info(string message)
        {
            Log("I ".AsColor("AAAAAA") + message);
        }

        public void Warn(string message)
        {
            Log("W ".AsColor("FFCC00") + message);
        }

        public void Error(string message)
        {
            Log("E ".AsColor("FF0000") + message);
        }

        public void Debug(string message)
        {
            Log("D ".AsColor("00FFFF") + message);
        }

        public class Entry
        {
            public string Text;
            public string Timestamp;

            public Entry(string text)
            {
                Text = text;
                Timestamp = DateTime.Now.ToString("HH:mm:ss");
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
