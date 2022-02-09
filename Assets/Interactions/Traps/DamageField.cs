using Interaction;
using Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitControlls;
using UnityEngine;

namespace Interaction
{
    public class DamageField : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();

            if (player == null)
                return;

            player.Damage();
        }
    }
}
