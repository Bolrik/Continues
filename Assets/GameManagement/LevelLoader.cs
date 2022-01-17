using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class LevelLoader 
    {
        #region Singleton Pattern
        public static LevelLoader Instance { get; private set; }
        static LevelLoader()
        {
            new LevelLoader();
        }

        private LevelLoader()
        {
            Instance = this;
        }
        #endregion


        public Action PreLoadScene { get; set; }

        public void Start(GameScene gameScene)
        {
            this.Start((int)gameScene);
        }

        public void Start(int index)
        {
            this.PreLoadScene?.Invoke();
            SceneManager.LoadScene(index);
        }

        public void Restart()
        {
            Scene scene = SceneManager.GetActiveScene();
            this.Start(scene.buildIndex);
        }

        public void Next()
        {
            Scene scene = SceneManager.GetActiveScene();
            this.Start(scene.buildIndex + 1);
        }
    }

    public enum GameScene
    {
        GameEntry,
        Level001,
        Level002,
        Level003,
        Level004,
        Level005,
        Level006,
        Level007,
        Level008,
        Debug
    }
}