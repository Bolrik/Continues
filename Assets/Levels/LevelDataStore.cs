using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Levels
{
    public class LevelDataStore : Singleton<LevelDataStore>
    {
        [SerializeField] private LevelData[] levels;
        public static LevelData[] Levels { get { return Instance.levels; } }
    }
}
