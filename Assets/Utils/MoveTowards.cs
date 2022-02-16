using UnityEngine;

namespace Utils
{
    public class MoveTowards : MonoBehaviour
    {
        [Header("Settings - Target")]
        [SerializeField] private Transform moveThis;
        public Transform MoveThis { get { return moveThis; } }

        [SerializeField] private Transform toHere;
        public Transform ToHere { get { return toHere; } }

        [Header("Settings - Delay")]
        [SerializeField] private float delay = 0f;
        public float Delay { get { return delay; } }

        [SerializeField] private float acceleration = 0f;
        public float Acceleration { get { return acceleration; } }

        [Header("Settings - Speed")]
        [SerializeField] private float speed = 1f;
        public float Speed { get { return speed; } private set { this.speed = value; } }

        [SerializeField] private float speedMax = 1f;
        public float SpeedMax { get { return speedMax; } }


        private float DelayTime { get; set; }


        private void Update()
        {
            this.DelayTime = Mathf.Clamp(this.DelayTime + Time.deltaTime, 0, this.Delay);

            if (this.DelayTime < this.Delay)
                return;

            this.Speed = Mathf.Clamp(this.Speed + this.Acceleration * Time.deltaTime, 0, this.SpeedMax);
            this.MoveThis.position = Vector3.MoveTowards(this.MoveThis.position, this.ToHere.position, this.Speed * Time.deltaTime);
        }
    }
}
