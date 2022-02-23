using System.Linq;
using UnityEngine;

namespace Utils
{
    public class RayCheck : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform[] ignore;
        public Transform[] Ignore { get { return ignore; } private set { ignore = value; } }

        [SerializeField] private float length;
        public float Length { get { return length; } }

        [SerializeField] private Vector3 direction;
        public Vector3 Direction { get { return direction; } }

        [SerializeField] private bool includeTrigger;
        public bool IncludeTrigger { get { return includeTrigger; } private set { includeTrigger = value; } }



        public bool Check()
        {
            return this.Check(-1, out _);
        }

        public bool Check(LayerMask layerMask)
        {
            return this.Check(layerMask, out _);
        }

        public bool Check(LayerMask layerMask, out RaycastHit[] hits)
        {
            hits = Physics.RaycastAll(new Ray(this.transform.position, this.Direction), this.Length,
                layerMask,
                this.IncludeTrigger ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (this.Ignore.Any(ignore => hit.transform.IsChildOf(ignore)))
                    continue;

                return true;
            }

            return false;
        }
    }
}
