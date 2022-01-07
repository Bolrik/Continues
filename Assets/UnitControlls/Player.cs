using Abilities;
using Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitControlls
{
    public class Player : MonoBehaviour, IAbilityReceiver
    {
        [SerializeField] private MovementBehaviour movement;
        public MovementBehaviour Movement { get { return movement; } }

        public Ability Stored { get; private set; }
        public OnAbilityChanged AbilityChanged { get; set; }

        public void SetAbility(Ability ability)
        {
            this.Stored = ability;
            this.AbilityChanged?.Invoke(this.Stored);
        }

        Ability IAbilityReceiver.SwapAbility(Ability swapTo)
        {
            return this.SwapAbility(swapTo);
        }
    }
}
