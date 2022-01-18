using Interaction;
using Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Interaction
{
    public class Button : MonoBehaviour, ISignalSender, IInteractable
    {
        [Header("References")]
        [SerializeField] private MeshRenderer colorPanel;
        public MeshRenderer ColorPanel { get { return colorPanel; } }

        [SerializeField] private MeshRenderer onOffBulb;
        public MeshRenderer OnOffBulb { get { return onOffBulb; } }

        [SerializeField] private Transform visualButtonTransform;
        public Transform VisualButtonTransform { get { return visualButtonTransform; } }

        [Header("Settings")]
        [SerializeField] private SignalChannel signalChannel;
        public SignalChannel SignalChannel { get { return signalChannel; } }

        Vector3 InitialVisualButtonTransform_LocalPosition { get; set; }
        bool IsActive { get; set; }

        private void Awake()
        {
            this.InitialVisualButtonTransform_LocalPosition = this.VisualButtonTransform.localPosition;
        }

        private void Update()
        {
            this.IsActive = SignalManager.Instance.GetSignal(this.SignalChannel);

            this.UpdateColors();
            this.UpdateTransforms();
        }

        private void UpdateColors()
        {
            this.ColorPanel.material.color = this.SignalChannel.GetColor();
            this.OnOffBulb.material.color = this.IsActive ? Color.green : Color.red;
        }

        private void UpdateTransforms()
        {
            Vector3 localPos = this.InitialVisualButtonTransform_LocalPosition;
            
            if (this.IsActive)
                localPos.y -= .1f;

            this.VisualButtonTransform.localPosition = localPos;
        }


        public void Activate()
        {
            this.SendSignal(this.SignalChannel);
        }
    }
}
