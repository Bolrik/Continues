using GameManagement;
using Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Levels
{
    public class ResetLevelDataUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Image button;
        public Image Button { get { return button; } }

        Color InitialColor { get; set; }

        float Confirm { get; set; }

        protected void Awake()
        {
            this.InitialColor = this.Button.color;
        }

        public void Click()
        {
            if (this.Confirm < 0)
            {
                Debug.Log("Reset");
                // MainMenuManager.Instance.ResetLevelData();
                return;
            }

            this.Confirm = -4;
        }

        private void LateUpdate()
        {
            this.Confirm = Mathf.Clamp(this.Confirm + Time.deltaTime, -4, 0);

            if (this.Confirm < 0)
            {
                int ms = Mathf.Abs((int)(this.Confirm * 1000));

                Debug.Log(ms);

                if (ms % 500 < 250)
                    this.Button.color = Color.red;
                else
                    this.Button.color = this.InitialColor;
            }
            else
                this.Button.color = this.InitialColor;
        }
    }
}