using Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conditions
{
    public static partial class Extension
    {
        public static bool Check(this ConditionContainer conditionContainer)
        {
            if (conditionContainer == null)
                return true;

            return conditionContainer.CheckConditions();
        }
    }
}
