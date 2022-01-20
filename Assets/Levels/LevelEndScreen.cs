using GameManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class LevelEndScreen : MonoBehaviour
    {
        [Header("References/General")]
        [SerializeField] private Text levelInfo;
        public Text LevelInfo { get { return levelInfo; } }


        [Header("References/Time")]
        [SerializeField] private Text time;
        public Text Time { get { return time; } }

        [SerializeField] private Text best;
        public Text Best { get { return best; } }


        public void Show(float levelTime)
        {
            this.gameObject.SetActive(true);

            this.LevelInfo.text = $"Level {LevelLoader.Instance.GetIndex():D3}";
            this.Time.text = $"{Assistance.FloatToTimeString(levelTime)}";
            this.Best.text = $"{Assistance.FloatToTimeString(GameSettings.Instance.GetLevelTime(LevelLoader.Instance.GetIndex()))}";
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void Next()
        {
            LevelLoader.Instance.Next();
        }

        public void Retry()
        {
            LevelLoader.Instance.Restart();
        }

        public void MainMenu()
        {
            LevelLoader.Instance.Start(GameScene.GameEntry);
        }
    }
}