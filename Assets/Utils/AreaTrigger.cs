using Interaction;
using UnitControlls;
using UnityEngine;

namespace Utils
{
    public class AreaTrigger : UseTrigger
    {
        [Header("Settings - References")]
        [SerializeField] private MeshRenderer areaRenderer;
        public MeshRenderer AreaRenderer { get { return areaRenderer; } }



        [Header("Settings - Details")]
        [SerializeField] private bool isVisible = true;
        public bool IsVisible { get { return isVisible; } }

        [SerializeField] private bool persistent = true;
        public bool Persistent { get { return persistent; } }

        [Header("Debug")]
        [SerializeField] private int triggerCount;
        public int TriggerCount { get { return triggerCount; } private set { this.triggerCount = value; } }



        private void Update()
        {
            this.AreaRenderer.enabled = this.IsVisible && this.OnCanTrigger();
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();

            if (player == null)
                return;

            if (this.Trigger() && this.IsVisible)
                player.Play(this.OnUseSound);
        }

        protected override void OnTriggered()
        {
            this.TriggerCount++;
        }

        protected override bool OnCanTrigger()
        {
            if (this.Persistent)
                return base.OnCanActivate();

            return this.TriggerCount <= 0;
        }

        protected override bool OnCanActivate()
        {
            return this.IsVisible;
        }
    }
}
