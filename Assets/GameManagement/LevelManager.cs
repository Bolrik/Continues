using Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitControlls;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class LevelManager
    {
        #region Singleton Pattern
        public static LevelManager Instance { get; private set; }
        static LevelManager()
        {
            new LevelManager();
        }

        private LevelManager()
        {
            Instance = this;
        }
        #endregion


        public Action PreLoadScene { get; set; }
        private Player Player { get; set; }

        public LevelData CurrentLevel { get; private set; }
        Dictionary<int, float> LevelTimes { get; set; } = 
            new Dictionary<int, float>();


        #region Levels

        public void Next()
        {
            this.Start(this.GetNextLevel());
        }

        public void Restart()
        {
            this.Start(this.CurrentLevel);
        }

        public void SetDone()
        {
            GameSettings.Instance.ShowCursor = true;
            this.Player?.GameOver(true);
        }

        public void SetActivePlayer(Player player)
        {
            this.Player = player;
        }


        LevelData GetLevel()
        {
            return this.CurrentLevel;
        }

        LevelData GetNextLevel()
        {
            return LevelDataStore.Instance.GetNext(this.CurrentLevel);
        }

        public int GetIndex()
        {
            return this.CurrentLevel.BuildIndex;
        }


        public void Start(LevelData levelData)
        {
            this.PreLoadScene?.Invoke();

            this.CurrentLevel = levelData;
            this.Player = null;
            int index = 0;

            if (this.CurrentLevel != null)
                index = this.CurrentLevel.BuildIndex;

            SceneManager.LoadScene(index);

            if (index > 0)
                GameSettings.Instance.ShowCursor = false;
        }
        #endregion


        //public void Do()
        //{
        //    LevelData levelData = LevelDataStore.Instance.GetByBuildIndex(this.GetIndex());
        //}

        #region Level Time
        public float UpdateLevelTime(LevelData levelData, float time)
        {
            if (levelData == null) return time;

            int index = levelData.BuildIndex;
            Debug.Log($"Update Level Time for {index} : {time}");

            //if (!this.LevelTimes.ContainsKey(index))
            //{
            //    // Load stored Time first
            //    if (!this.TryLoadLevelTime(levelData))
            //    {
            //        this.LevelTimes[index] = time;
            //    }
            //}

            //float current = this.LevelTimes[index];

            float current = this.GetLevelTime(levelData);

            if (time < current)
            {
                Debug.Log($"Save Level Time for {index} : {time}");

                this.LevelTimes[index] = time;
                PlayerPrefs.SetFloat(PlayerPrefKeys.GetLevelTimeKey(levelData), time);
            }

            return this.LevelTimes[index];
        }

        public float UpdateLevelTime(float time)
        {
            return this.UpdateLevelTime(this.CurrentLevel, time);
        }

        // Try to get the users Best Time for the current level
        public float GetLevelTime()
        {
            return this.GetLevelTime(this.CurrentLevel);
        }
        public float GetLevelTime(LevelData levelData)
        {
            if (levelData == null)
                return 99 * 60;

            int index = levelData.BuildIndex;

            if (!this.LevelTimes.ContainsKey(index))
            {
                Debug.Log("Load PlayerPrefs Time");
                // Load saved PlayerPref Time first
                if (!this.TryLoadLevelTime(levelData))
                {
                    Debug.Log("Failed - Load PlayerPrefs Time");
                    this.LevelTimes[index] = 99 * 60;
                }
            }

            return this.LevelTimes[index];
        }


        private bool TryLoadLevelTime(LevelData levelData)
        {
            int index = levelData.BuildIndex;
            string key = PlayerPrefKeys.GetLevelTimeKey(levelData);
            Debug.Log($"Loading PlayerPrefs Level Time for {index}");

            if (PlayerPrefs.HasKey(key))
            {
                this.LevelTimes[index] = PlayerPrefs.GetFloat(key);
                return true;
            }

            Debug.Log($"Failed - Loading PlayerPrefs Level Time for {index}");
            return false;
        }

        public void ClearLevelTimes()
        {
            this.LevelTimes.Clear();
        }
        #endregion
    }
}