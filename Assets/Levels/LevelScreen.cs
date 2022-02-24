using GameManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Levels
{
    public class LevelScreen : MonoBehaviour
    {
        static string[] LevelScreenTips { get; set; } = new string[]
        {
            // Helpfull
            "Immediately after taking damage, you can jump higher!",
            "Secrets make a loud 'bong' now and then",

            // *'Funny'*
            "(>'-')> <('-'<) ^('-')^ v('-')v (>'-')> (^-^)",
            "TV out of Order... Sorry!",
            "Watch out, Behind you!"
        };

        [SerializeField] private TMP_Text levelName;
        public TMP_Text LevelName { get { return levelName; } }

        [SerializeField] private TMP_Text tip;
        public TMP_Text Tip { get { return tip; } }

        [SerializeField] private TMP_Text time;
        public TMP_Text Time { get { return time; } }

        [SerializeField] private TMP_Text best;
        public TMP_Text Best { get { return best; } }

        [SerializeField] private SpriteRenderer secretRenderer;
        public SpriteRenderer SecretRenderer { get { return secretRenderer; } }




        [SerializeField, Header("Debug")] private float bestValue;
        private float BestValue { get { return bestValue; } set { bestValue = value; } }



        float TimeToTip { get; set; }

        private void Start()
        {
            this.NextTip();
            this.ReadLevelData();
        }

        private void Update()
        {
            this.TimeToTip += UnityEngine.Time.deltaTime;

            if (this.TimeToTip > 10)
                this.NextTip();
        }


        private void ReadLevelData()
        {
            LevelData levelData = LevelManager.Instance.GetLevel();

            if (levelData == null)
            {
                this.LevelName.text = $"Unknown";
                this.Time.text = $"99:00:000";
                this.Best.text = $"99:00:000";
                this.SecretRenderer.enabled = false;
                return;
            }

            this.BestValue = levelData.GetLevelTime();

            this.LevelName.text = $"{levelData.GetLevelString()}";
            this.Time.text = $"{Assistance.FloatToTimeString(levelData.GetLevelTime())}";
            this.Best.text = $"{Assistance.FloatToTimeString(levelData.DevTime)}";
            this.SecretRenderer.enabled = levelData.HasSecret;
        }

        private void NextTip()
        {
            this.Tip.text = LevelScreen.LevelScreenTips[UnityEngine.Random.Range(0, LevelScreen.LevelScreenTips.Length)];
            this.TimeToTip = 0;
        }
    }
}
