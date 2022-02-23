using Interaction;
using UnitControlls;
using UnityEngine;

namespace Utils
{
    public class AreaTrigger : MonoBehaviour, ITrigger, IInteractable
    {
        [SerializeField] private Sprite icon;
        public Sprite Icon { get { return icon; } }

        [SerializeField] private AudioClip onUseSound;
        public AudioClip OnUseSound { get { return onUseSound; } }

        System.Action OnTrigger { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();

            if (player == null)
                return;

            this.Trigger();
            player.Play(this.OnUseSound);
        }

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

        public void Activate()
        {
            this.Trigger();
        }

        private void Trigger()
        {
            this.OnTrigger?.Invoke();
        }
    }
}
