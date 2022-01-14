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
        public static SignalState ReadSignalState(this ISignalBase signalBase, bool signal, bool isInverse)
        {
            return (signal == isInverse) ? SignalState.Inactive : SignalState.Active;
        }
    }
}
