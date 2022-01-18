using Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abilities
{
    public static partial class Extension
    {
        public static Ability SwapAbility(this IAbilityReceiver abilityReceiver, Ability swapTo)
        {
            Ability toReturn = abilityReceiver.Ability;
            abilityReceiver.SetAbility(swapTo);

            return toReturn;
        }
    }
}
