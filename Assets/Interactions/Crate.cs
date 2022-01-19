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
    public class Crate : MonoBehaviour, IGrabObject
    {
        [Header("References")]
        [SerializeField] private Rigidbody rigidbody;
        public Rigidbody Rigidbody { get { return rigidbody; } }

        [SerializeField] private Transform grabPoint;
        public Transform GrabPoint { get { return grabPoint; } }

    }
}
