using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Signals
{
    public static partial class Extension
    {
        public static SignalState ReadSignalState(this ISignalBase signalBase, float signalValue, bool isInverse)
        {
            bool isActive = signalValue > 0;

            return (isActive == isInverse) ? SignalState.Inactive : SignalState.Active;
        }
    }
}
