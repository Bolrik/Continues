using Signals;
using System;
using System.Linq;
using UnityEngine;

namespace Interaction
{
    public class Door : MonoBehaviour, ISignalReceiver
    {
        [Header("References")]
        [SerializeField] private Transform gateTransform;
        public Transform GateTransform { get { return this.gateTransform; } }

        [SerializeField] private MeshRenderer colorPanel;
        public MeshRenderer ColorPanel { get { return colorPanel; } }


        [Header("Settings")]
        [SerializeField] private SignalChannel signalChannel;
        public SignalChannel SignalChannel { get { return this.signalChannel; } }

        [SerializeField] private bool isInverse;
        public bool IsInverse { get { return this.isInverse; } private set { this.isInverse = value; } }

        [SerializeField] private DoorStateLocation[] doorStateLocations;
        public DoorStateLocation[] DoorStateLocations { get { return this.doorStateLocations; } }

        [SerializeField] private float timeToOpenDefault;
        public float TimeToOpenDefault { get { return this.timeToOpenDefault; } }

        [SerializeField] private float reactionTimeDefault;
        public float ReactionTimeDefault { get { return reactionTimeDefault; } }



        float TimeToOpen { get; set; }
        float ReactionTime { get; set; }
        DoorState DoorState { get; set; }

        Vector3 GateOpenPosition { get; set; }
        Vector3 GateClosePosition { get; set; }

        private void Awake()
        {
            this.UpdateDoor();
        }

        private void Start()
        {
            this.GateOpenPosition = this.DoorStateLocations
                .FirstOrDefault(doorStateLocation => doorStateLocation.State == DoorState.Open)
                .LocalPosition;

            this.GateClosePosition = this.DoorStateLocations
                .FirstOrDefault(doorStateLocation => doorStateLocation.State == DoorState.Closed)
                .LocalPosition;
        }

        private void Update()
        {
            this.UpdateDoor();
            this.UpdateDoorPosition();
            this.UpdateColors();
        }

        private void UpdateColors()
        {
            if (this.ReactionTime > 0 && this.ReactionTime % .2f > .1f)
            {
                this.ColorPanel.material.color = Color.white;
            }
            else
                this.ColorPanel.material.color = this.SignalChannel.GetColor();
        }

        private void UpdateDoor()
        {
            bool signal = SignalManager.Instance.GetSignal(this.SignalChannel);

            var signalState = this.ReadSignalState(signal, this.IsInverse);
            this.SetSoorState(signalState.ToDoorState());
        }

        private void UpdateDoorPosition()
        {
            float deltaTime = Time.deltaTime;

            this.ReactionTime = Mathf.Clamp(this.ReactionTime - deltaTime, 0, this.ReactionTimeDefault);

            if (this.ReactionTime <= 0)
            {
                switch (this.DoorState)
                {
                    case DoorState.Open:
                        break;
                    case DoorState.Closed:
                        deltaTime *= -1;
                        break;
                    default:
                        throw new Exception();
                }

                this.TimeToOpen = Mathf.Clamp(this.TimeToOpen + deltaTime, 0, this.TimeToOpenDefault);
            }

            float percent = this.TimeToOpen / (this.TimeToOpenDefault == 0 ? 1 : this.TimeToOpenDefault);
            this.GateTransform.localPosition = Vector3.Slerp(this.GateClosePosition, this.GateOpenPosition, percent);
        }


        private void SetSoorState(DoorState doorState)
        {
            if (this.DoorState == doorState)
                return;
            
            this.ReactionTime = this.ReactionTimeDefault;
            this.DoorState = doorState;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = this.SignalChannel.GetColor();
            Gizmos.DrawSphere(this.transform.position, .35f);
        }
    }

    [System.Serializable]
    public class DoorStateLocation
    {
        [SerializeField] private DoorState state;
        public DoorState State { get { return this.state; } }

        [SerializeField] private Vector3 localPosition;
        public Vector3 LocalPosition { get { return this.localPosition; } }
    }

    public enum DoorState
    {
        Open,
        Closed
    }
}