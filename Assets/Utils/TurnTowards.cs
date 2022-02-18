using UnityEngine;

namespace Utils
{
    public class TurnTowards : MonoBehaviour
    {
        [Header("Settings - Target")]
        [SerializeField] private Transform turnThis;
        public Transform TurnThis { get { return turnThis; } }

        [SerializeField] private Transform toThis;
        public Transform ToThis { get { return toThis; } }

        [Header("Settings - Speed")]
        [SerializeField] private float speed = 1f;
        public float Speed { get { return speed; } private set { this.speed = value; } }

        [SerializeField] private float activeDistance = 3f;
        public float ActiveDistance { get { return activeDistance; } }



        private void Update()
        {
            Vector3 delta = this.TurnThis.position - this.ToThis.position;

            if (delta.magnitude > this.ActiveDistance)
                return;

            this.TurnThis.LookAt(this.ToThis);
        }
    }
}
