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
        public static bool SendSignal(this ISignalSender signalSender, SignalChannel signalChannel)
        {
            return SignalManager.Instance.Toggle(signalChannel);
        }
    }
}
