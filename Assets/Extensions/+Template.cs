using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine.InputSystem
{
    public static partial class Extension
    {
        public static bool ButtonPressed(this InputAction inputAction) => inputAction.triggered;
    }
}
