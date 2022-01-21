using GameManagement;
using Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class LevelUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image preview;
        public Image Preview { get { return preview; } }

        [SerializeField] private Text info;
        public Text Info { get { return info; } }

        [SerializeField] private Text time;
        public Text Time { get { return time; } }


        private LevelData LevelData { get; set; }


        public void SetData(LevelData levelData)
        {
            this.LevelData = levelData;

            this.Preview.sprite = this.LevelData.LevelPreview;
            this.Info.text = this.LevelData.GetLevelString();
            this.Time.text = Assistance.FloatToTimeString(LevelManager.Instance.GetLevelTime(this.LevelData));
            //this.Time.text = "00:00:001";
        }

        public void StartLevel()
        {
            LevelManager.Instance.Start(this.LevelData);
        }
    }
}