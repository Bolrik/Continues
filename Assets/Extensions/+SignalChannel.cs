using Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Signals
{
    public static partial class Extension
    {
        public static Color GetColor(this SignalChannel signalChannel)
        {
            switch (signalChannel)
            {
                case SignalChannel.Red:
                    return Color.red;
                case SignalChannel.Green:
                    return Color.green;
                case SignalChannel.Blue:
                    return Color.blue;
                case SignalChannel.Yellow:
                    return Color.yellow;
                case SignalChannel.Magenta:
                    return Color.magenta;
                case SignalChannel.Cyan:
                    return Color.cyan;
                case SignalChannel.Gray:
                    return Color.gray;
            }

            throw new Exception();
        }
    }
}
