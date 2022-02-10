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


        public void Show(float levelTime, bool isVictory)
        {
            this.gameObject.SetActive(true);

            this.LevelInfo.text = $"Level {LevelManager.Instance.GetIndex():D3}";
            this.Time.text = isVictory ? $"{Assistance.FloatToTimeString(levelTime)}" : "xx:xx:xxx";
            this.Best.text = $"{Assistance.FloatToTimeString(LevelManager.Instance.GetLevelTime())}";
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void Next()
        {
            LevelManager.Instance.Next();
        }

        public void Retry()
        {
            LevelManager.Instance.Restart();
        }

        public void MainMenu()
        {
            LevelManager.Instance.Start(null);
        }
    }
}