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
    public class ChapterUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int chapter;
        public int Chapter { get { return chapter; } }

        public void Select()
        {
            MainMenuManager.Instance.LoadChapter(this.Chapter);
        }
    }
}