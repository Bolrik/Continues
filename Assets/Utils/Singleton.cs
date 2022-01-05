using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class Singleton<T> : Static<T>
        where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (Instance != null)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            base.Awake();
        }
    }
}
