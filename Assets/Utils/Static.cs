using UnityEngine;

namespace Utils
{
    public class Static<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            Instance = this as T;
        }

        private void OnApplicationQuit()
        {
            Instance = null;
            GameObject.Destroy(this.gameObject);
        }
    }
}
