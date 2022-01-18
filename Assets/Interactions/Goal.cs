using GameManagement;
using Signals;
using System;
using System.Linq;
using UnitControlls;
using UnityEngine;

namespace Interaction
{
    public class Goal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();
            Debug.Log(other);
            if (player == null)
                return;

            LevelLoader.Instance.Next();
        }
    }
}