using Interaction;
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
        public static DoorState ToDoorState(this SignalState signalState)
        {
            switch (signalState)
            {
                case SignalState.Active:
                    return DoorState.Open;
                case SignalState.Inactive:
                    return DoorState.Closed;
                default:
                    throw new Exception();
            }
        }
    }
}
