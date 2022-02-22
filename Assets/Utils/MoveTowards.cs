using UnityEngine;

namespace Utils
{
    public class MoveTowards : MonoBehaviour
    {
        [Header("Settings - Target")]
        [SerializeField] private Transform moveThis;
        public Transform MoveThis { get { return moveThis; } }

        //[SerializeField] private Transform toHere;
        //public Transform ToHere { get { return toHere; } }
        [SerializeField] private Transform origin;
        public Transform Origin { get { return origin; } }

        [SerializeField] private Transform destination;
        public Transform Destination { get { return destination; } }

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
        float SpeedDefault { get; set; }

        private float DelayTime { get; set; }

        [Header("Settings - Misc")]
        [SerializeField] private MoveTowardsState state = MoveTowardsState.ToDestination;
        public MoveTowardsState State { get { return state; } private set { state = value; } }

        private MoveTowardsState PreviousState { get; set; }


        [SerializeField] private bool autoToggle;
        public bool AutoToggle { get { return autoToggle; } }

        [Header("Settings - Conditions")]
        [SerializeField] private AreaTrigger[] triggers;
        public AreaTrigger[] Triggers { get { return triggers; } }



        private void Start()
        {
            this.SpeedDefault = this.Speed;
            this.PreviousState = this.State;

            if (this.Triggers != null)
            {
                for (int idx = 0; idx < this.Triggers.Length; idx++)
                {
                    this.Triggers[idx].Subscribe(this.ToggleState);
                }
            }
        }

        private void Update()
        {
            if (this.State == MoveTowardsState.Wait)
                return;

            this.DelayTime = Mathf.Clamp(this.DelayTime + Time.deltaTime, 0, this.Delay);

            if (this.DelayTime < this.Delay)
                return;

            this.Speed = Mathf.Clamp(this.Speed + this.Acceleration * Time.deltaTime, 0, this.SpeedMax);

            Transform currentDestination = this.MoveThis;

            switch (this.State)
            {
                case MoveTowardsState.ToDestination:
                    currentDestination = this.Destination;
                    break;
                case MoveTowardsState.ToOrigin:
                    currentDestination = this.Origin;
                    break;
            }

            this.MoveThis.position = Vector3.MoveTowards(this.MoveThis.position, currentDestination.position, this.Speed * Time.deltaTime);

            if ((this.MoveThis.position - currentDestination.position).sqrMagnitude <= 0)
            {
                if (this.AutoToggle)
                    this.ToggleState();
                else
                    this.State = MoveTowardsState.Wait;
            }
        }

        private void ToggleState()
        {
            switch (this.PreviousState)
            {
                case MoveTowardsState.Wait:
                    this.State = MoveTowardsState.ToDestination;
                    break;
                case MoveTowardsState.ToDestination:
                    this.PreviousState = MoveTowardsState.ToOrigin;
                    this.State = MoveTowardsState.ToOrigin;
                    break;
                case MoveTowardsState.ToOrigin:
                    this.PreviousState = MoveTowardsState.ToDestination;
                    this.State = MoveTowardsState.ToDestination;
                    break;
            }

            this.DelayTime = 0;
            this.Speed = this.SpeedDefault;
        }
    }

    public enum MoveTowardsState
    {
        Wait,
        ToDestination,
        ToOrigin
    }

    public interface ITrigger
    {
        void Subscribe(System.Action action);
        void Unsubscribe(System.Action action);
    }
}
