using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameManagement
{
    public class GameSettings
    {
        #region Singleton Pattern
        public static GameSettings Instance { get; private set; }
        static GameSettings()
        {
            new GameSettings();
        }

        private GameSettings()
        {
            Instance = this;
        }
        #endregion

        private float mouseSensitivity = 1f;
        public float MouseSensitivity
        {
            get { return mouseSensitivity; }
            set { mouseSensitivity = value; }
        }

        private bool showCursor;
        public bool ShowCursor
        {
            get { return showCursor; }
            set
            {
                showCursor = value;

                Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = true;
            }
        }


        Dictionary<int, float> LevelTimes { get; set; } = new Dictionary<int, float>();

        public float UpdateLevelTime(int levelIndex, float time)
        {
            Debug.Log($"Update Level Time for {levelIndex} : {time}");

            if (!this.LevelTimes.ContainsKey(levelIndex))
            {
                // Load saved PlayerPref Time first
                if (!this.TryLoadSave(levelIndex))
                {
                    this.LevelTimes[levelIndex] = time;
                }
            }

            float current = this.LevelTimes[levelIndex];

            if (time < current)
            {
                Debug.Log($"Save Level Time for {levelIndex} : {time}");

                this.LevelTimes[levelIndex] = time;
                PlayerPrefs.SetFloat(GameSettingsKeys.GetLevelTimeKey(levelIndex), time);
            }

            return this.LevelTimes[levelIndex];
        }

        public float UpdateLevelTime(float time)
        {
            return this.UpdateLevelTime(LevelLoader.Instance.GetIndex(), time);
        }

        public float GetLevelTime()
        {
            return this.GetLevelTime(LevelLoader.Instance.GetIndex());
        }
        public float GetLevelTime(int levelIndex)
        {
            if (!this.LevelTimes.ContainsKey(levelIndex))
            {
                Debug.Log("Load PlayerPrefs Time");
                // Load saved PlayerPref Time first
                if (!this.TryLoadSave(levelIndex))
                {
                    Debug.Log("Failed - Load PlayerPrefs Time");
                    this.LevelTimes[levelIndex] = 99*60;
                }
            }

            return this.LevelTimes[levelIndex];
        }


        private bool TryLoadSave(int levelIndex)
        {
            string key = GameSettingsKeys.GetLevelTimeKey(levelIndex);
            Debug.Log($"Loading PlayerPrefs Level Time for {levelIndex}");

            if (PlayerPrefs.HasKey(key))
            {
                this.LevelTimes[levelIndex] = PlayerPrefs.GetFloat(key);
                return true;
            }

            Debug.Log($"Failed - Loading PlayerPrefs Level Time for {levelIndex}");
            return false;
        }
    }

    public static class GameSettingsKeys
    {
        public static string MouseSensitivity { get; } = "MOUSE_SENSITIVITY";


        public static string GetLevelTimeKey(int index)
        {
            // Todo...
            return $"LEVEL_{index:D8}_TIME";
        }
    }
}