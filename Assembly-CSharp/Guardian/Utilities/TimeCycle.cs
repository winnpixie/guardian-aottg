using UnityEngine;

namespace Guardian.Utilities
{
    internal class TimeCycle : MonoBehaviour
    {
        private float timeOfDay = 0f;
        private Color previousLight = FengColor.Day;
        private Color previousAmbient = FengColor.AmbientDay;
        private Color nextLight = FengColor.Dawn;
        private Color nextAmbient = FengColor.AmbientDawn;

        private IN_GAME_MAIN_CAMERA cachedCamera;
        private Light cachedLight;

        void OnLevelWasLoaded(int level)
        {
            timeOfDay = 0f;

            UpdateCycle(IN_GAME_MAIN_CAMERA.Lighting);

            cachedCamera = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();
            if (cachedCamera == null)
            {
                return;
            }

            GameObject mainLight = GameObject.Find("mainLight");
            if (mainLight == null)
            {
                return;
            }

            cachedLight = mainLight.GetComponent<Light>();
        }

        void Update()
        {
            if (!GuardianClient.Properties.TimeCycle.Value)
            {
                return;
            }

            if (cachedCamera == null || cachedLight == null)
            {
                return;
            }

            float cycleLength = (float)GuardianClient.Properties.TimeCycleLength.Value;
            timeOfDay += Time.deltaTime;
            if (timeOfDay > cycleLength)
            {
                timeOfDay = 0f;

                DayLight nextCycle = IN_GAME_MAIN_CAMERA.Lighting switch
                {
                    DayLight.Day => DayLight.Dawn,
                    DayLight.Dawn => DayLight.Night,
                    DayLight.Night => DayLight.Day,
                    _ => DayLight.Day
                };
                cachedCamera.SetLighting(nextCycle);

                UpdateCycle(nextCycle);
            }

            cachedLight.color = Color.Lerp(previousLight, nextLight, timeOfDay / cycleLength);
            RenderSettings.ambientLight = Color.Lerp(previousAmbient, nextAmbient, timeOfDay / cycleLength);
        }

        private void UpdateCycle(DayLight cycle)
        {
            switch (cycle)
            {
                case DayLight.Day:
                    previousLight = FengColor.Night;
                    nextLight = FengColor.Day;

                    previousAmbient = FengColor.AmbientNight;
                    nextAmbient = FengColor.AmbientDay;
                    break;
                case DayLight.Dawn:
                    previousLight = FengColor.Day;
                    nextLight = FengColor.Dawn;

                    previousAmbient = FengColor.AmbientDay;
                    nextAmbient = FengColor.AmbientDawn;
                    break;
                case DayLight.Night:
                    previousLight = FengColor.Dawn;
                    nextLight = FengColor.Night;

                    previousAmbient = FengColor.AmbientDawn;
                    nextAmbient = FengColor.AmbientNight;
                    break;
            }
        }
    }
}
