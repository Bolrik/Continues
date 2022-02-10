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

    public enum HealthBarState
    {
        Full,
        Empty
    }
}
