using UnityEngine;

namespace Utils
{
    public class PersistentSingleton<T> : Singleton<T>
        where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
