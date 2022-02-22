using UnitControlls;
using UnityEngine;

namespace Utils
{
    public class AreaTrigger : MonoBehaviour, ITrigger
    {
        System.Action OnTrigger { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();

            if (player == null)
                return;

            this.OnTrigger?.Invoke();
            Debug.Log("Trigger");
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
    }
}
