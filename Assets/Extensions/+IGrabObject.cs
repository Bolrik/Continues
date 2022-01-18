using Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaction
{
    public static partial class Extension
    {
        public static void IsGrab(this IGrabObject grabObject, bool value)
        {
            // grabObject.Rigidbody.isKinematic = value;
            if (grabObject == null)
                return;

            grabObject.Rigidbody.useGravity = !value;
        }
    }
}
