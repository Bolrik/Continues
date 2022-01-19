using System;
using UnitControlls;
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
        private Player Player { get; set; }


        public void Start(GameScene gameScene)
        {
            this.Start((int)gameScene);
        }

        public void Start(int index)
        {
            this.PreLoadScene?.Invoke();
            this.Player = null;

            SceneManager.LoadScene(index);

            if (index > 0)
                GameSettings.Instance.ShowCursor = false;
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

        public void SetDone()
        {
            GameSettings.Instance.ShowCursor = true;
            this.Player?.GameOver();
        }

        public void SetActivePlayer(Player player)
        {
            this.Player = player;
        }

        public int GetIndex()
        {
            Scene scene = SceneManager.GetActiveScene();
            return scene.buildIndex;
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