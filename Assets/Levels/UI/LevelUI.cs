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


        public void SetData(LevelData levelData)
        {
            this.Preview.sprite = levelData.LevelPreview;
            this.Info.text = levelData.GetLevelString();
            this.Time.text = "00:00:001";
        }
    }
}