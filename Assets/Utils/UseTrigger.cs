using Interaction;
using UnitControlls;
using UnityEngine;

namespace Utils
{
    public class UseTrigger : MonoBehaviour, ITrigger, IInteractable
    {
        [SerializeField] private Sprite icon;
        public Sprite Icon { get { return icon; } }

        [SerializeField] private AudioClip onUseSound;
        public AudioClip OnUseSound { get { return onUseSound; } }

        [SerializeField] private bool interactable = true;
        public bool Interactable { get { return interactable; } }

        System.Action OnTrigger { get; set; }

        //void ITrigger.Subscribe(System.Action action)
        public void Subscribe(System.Action action)
        {
            this.OnTrigger += action;
        }

        // void ITrigger.Unsubscribe(System.Action action)
        public void Unsubscribe(System.Action action)
        {
            this.OnTrigger -= action;
        }

        public bool Activate()
        {
            if (!this.CanActivate())
            {
                return false;
            }

            this.Trigger();
            return true;
        }

        protected bool Trigger()
        {
            if (!this.CanTrigger())
                return false;

            this.OnTrigger?.Invoke();
            this.OnTriggered();

            return true;
        }

        public bool CanActivate()
        {
            return this.Interactable && this.OnCanActivate() && this.CanTrigger();
        }

        private bool CanTrigger()
        {
            return this.OnCanTrigger();
        }

        protected virtual void OnTriggered() { }
        protected virtual bool OnCanActivate() => true;
        protected virtual bool OnCanTrigger() => true;
    }
}
