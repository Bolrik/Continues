using Abilities;
using GameManagement;
using Interaction;
using Levels;
using Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UnitControlls
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image fill;
        public Image Fill { get { return fill; } }

        [SerializeField] private float shakeValueDefault = .5f;
        public float ShakeValueDefault { get { return shakeValueDefault; } }


        HealthBarState State { get; set; } = HealthBarState.Full;
        float StateValue { get; set; }
        float ShakeValue { get; set; }

        public void SetState(HealthBarState state)
        {
            this.State = state;
        }

        public void Shake()
        {
            this.ShakeValue = this.ShakeValueDefault;
        }

        private void LateUpdate()
        {
            float stateValue = 
                this.State == HealthBarState.Empty ? 0 :
                this.State == HealthBarState.Full ? 1 : 0;

            if (this.StateValue != stateValue)
            {
                this.StateValue = Mathf.Lerp(this.StateValue, stateValue, Time.deltaTime * 5f);
                this.Fill.fillAmount = this.StateValue;
            }

            if (this.ShakeValue > 0)
            {
                this.ShakeValue -= Time.deltaTime;
                this.transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(this.ShakeValue * 20 * 8, 20) - 10);
            }
            else
            {
                this.transform.localEulerAngles = Vector3.zero;
            }
        }

    }

    [System.Serializable]
    public class HealthBarContainer
    {

        [SerializeField] private Transform healthBarContainerTransform;
        public Transform HealthBarContainerTransform { get { return healthBarContainerTransform; } }


        [SerializeField] private HealthBar[] healthBars;
        public HealthBar[] HealthBars { get { return healthBars; } }

        public int Health { get; private set; } = 3;
        public float InvincibleTime { get; private set; }

        public bool Damage()
        {
            if (this.InvincibleTime > 0)
                return false;

#warning Play Damage Sound

            this.InvincibleTime = .7f;
            // Screen Shake

            this.Health--;
            int healthBarIndex = this.Health;

            HealthBar healthBar = this.HealthBars[healthBarIndex];
            healthBar.Shake();
            healthBar.SetState(HealthBarState.Empty);

            return true;
        }

        public void Tick(float deltaTime)
        {
            this.InvincibleTime = Mathf.Clamp(this.InvincibleTime - deltaTime, 0, this.InvincibleTime);
        }

        public void Show(LevelData levelData)
        {
            if (levelData == null)
                return;

            this.HealthBarContainerTransform.gameObject.SetActive(levelData.HealthHUDActive);
        }
    }

    public enum HealthBarState
    {
        Full,
        Empty
    }
}
