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
        [SerializeField] private Sprite levelPreview;
        public Sprite LevelPreview { get { return levelPreview; } }

        [SerializeField] private int chapter;
        public int Chapter { get { return chapter; } }

        [SerializeField] private int level;
        public int Level { get { return level; } }

        [SerializeField] private int buildIndex;
        public int BuildIndex { get { return buildIndex; } }

        public string GetLevelString()
        {
            return $"{this.Chapter:D2} - {this.Level:D3}";
        }
    }
}
