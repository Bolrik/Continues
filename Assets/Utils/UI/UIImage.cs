using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Utils.UI
{
    class UIImage : Image
    {
        [SerializeField] bool invertMask;
        public bool InvertMask
        {
            get { return invertMask; }
            set
            {
                if (this.invertMask != value)
                {
                    invertMask = value;
                    this.RecalculateMasking();
                }
            }
        }


        public override Material materialForRendering
        {
            get
            {
                if (!this.InvertMask)
                    return base.materialForRendering;

                Material toReturn = new Material(base.materialForRendering);
                toReturn.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return toReturn;
            }
        }
    }
}