using System;
using System.Linq;
using UnityEngine;
using Utils;

namespace Levels
{
    public class LevelDataStore : Singleton<LevelDataStore>
    {
        [SerializeField] private LevelData[] levels;
        public LevelData[] Levels { get { return this.levels; } private set { this.levels = value; } }


        private void Start()
        {
            for (int i = 0; i < this.Levels.Length; i++)
            {
                LevelData level = this.Levels[i];
                if (level.Level == 0 &&
                    level.Chapter > 0) // Ignore Debug Chapter??
                    Debug.LogWarning($"Level: '{level}' on Chapter {level.Chapter} has Level Value of 0!");
            }

            this.Levels = this.Levels.OrderBy(level => level.Chapter).ThenBy(level => level.Level).ToArray();
        }

        //public LevelData GetByBuildIndex(int buildIndex)
        //{
        //    return this.Levels.FirstOrDefault(levelData => levelData.BuildIndex == buildIndex);
        //}

        public LevelData Get(Predicate<LevelData> condition)
        {
            for (int idx = 0; idx < this.Levels.Length; idx++)
            {
                LevelData level = this.Levels[idx];
                if (condition(level))
                    return level;
            }

            return null;
        }

        public LevelData Get(int chapter, int level)
        {
            return this.Levels.FirstOrDefault(levelData => levelData.Chapter == chapter && levelData.Level == level);
        }

        public LevelData GetNext(LevelData current)
        {
            // Try get the next level of this chapter
            LevelData toReturn = this.Get(current.Chapter, current.Level + 1);

            if (toReturn != null) return toReturn;

            // Try get Next Chapter, Level 1
            toReturn = this.Get(current.Chapter + 1, 1);
            if (toReturn != null) return toReturn;

            return null;
        }

        public LevelData[] GetChapter(int chapter)
        {
            return this.Levels.Where(levelData => levelData.Chapter == chapter).ToArray();
        }
    }
}
