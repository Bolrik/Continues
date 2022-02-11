using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Level/LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private string display;
        public string Display { get { return display; } }

        [SerializeField] private Sprite levelPreview;
        public Sprite LevelPreview { get { return levelPreview; } }

        [SerializeField] private int chapter;
        public int Chapter { get { return chapter; } }

        [SerializeField] private int level;
        public int Level { get { return level; } }

        [SerializeField] private int buildIndex;
        public int BuildIndex { get { return buildIndex; } }

        [SerializeField] private bool healthHUDActive;
        public bool HealthHUDActive { get { return healthHUDActive; } }


        public string GetLevelString()
        {
            if (string.IsNullOrWhiteSpace(this.Display))
                return $"{this.Chapter:D2} - {this.Level:D3}";
            else
                return this.Display;
        }
    }
}
