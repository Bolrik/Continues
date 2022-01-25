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
    public class Link : MonoBehaviour, IInteractable
    {
        [SerializeField] private AudioClip onUseSound;
        public AudioClip OnUseSound { get { return onUseSound; } }

        [SerializeField] private Sprite icon;
        public Sprite Icon { get { return icon; } }

        [SerializeField] private string value;
        public string Value { get { return value; } }



        public void Activate()
        {
            System.Diagnostics.Process.Start(this.Value);
        }
    }
}
