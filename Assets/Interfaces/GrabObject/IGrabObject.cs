using UnityEngine;

namespace Interaction
{
    public interface IGrabObject
    {
        Rigidbody Rigidbody { get; }
        Transform GrabPoint { get; }
    }
}