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
        public static float SendSignal(this ISignalSender signalSender, SignalChannel signalChannel, float value)
        {
            SignalManager.Instance.SetSignal(signalChannel, value);
            Debug.Log($"Sending on Channel: {signalChannel} >> {value}");

            return value;
        }
    }
}
