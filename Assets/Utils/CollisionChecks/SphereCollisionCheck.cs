using System.Linq;
using UnityEngine;

namespace Utils
{
    public class SphereCollisionCheck : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform[] ignore;
        public Transform[] Ignore { get { return ignore; } private set { ignore = value; } }

        [SerializeField] private float radius;
        public float Radius { get { return radius; } }

        [SerializeField] private Vector3 offset;
        public Vector3 Offset { get { return offset; } }



        public bool Check()
        {
            float scaleX = this.transform.lossyScale.x / this.transform.localScale.x;
            float scaleY = this.transform.lossyScale.y / this.transform.localScale.y;
            float scaleZ = this.transform.lossyScale.z / this.transform.localScale.z;

            float scale = Mathf.Min(scaleX, scaleY, scaleZ);

            Collider[] colliders = Physics.OverlapSphere(this.transform.position +
                new Vector3(this.Offset.x * scaleX, this.Offset.y * scaleY, this.Offset.z * scaleZ), this.Radius * scale);

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];
                if (collider == null || this.Ignore.Any(ignore => collider.transform.IsChildOf(ignore)))
                    continue;

                return true;
            }

            return false;
        }

        public bool CheckNonAloc(int maxColliders)
        {
            float scaleX = this.transform.lossyScale.x / this.transform.localScale.x;
            float scaleY = this.transform.lossyScale.y / this.transform.localScale.y;
            float scaleZ = this.transform.lossyScale.z / this.transform.localScale.z;

            float scale = Mathf.Min(scaleX, scaleY, scaleZ);

            Collider[] colliders = new Collider[maxColliders];
            Physics.OverlapSphereNonAlloc(this.transform.position +
                new Vector3(this.Offset.x * scaleX, this.Offset.y * scaleY, this.Offset.z * scaleZ), this.Radius * scale, colliders);

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];
                if (collider == null || this.Ignore.Any(ignore => collider.transform.IsChildOf(ignore)))
                    continue;

                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            float scaleX = this.transform.lossyScale.x / this.transform.localScale.x;
            float scaleY = this.transform.lossyScale.y / this.transform.localScale.y;
            float scaleZ = this.transform.lossyScale.z / this.transform.localScale.z;

            float scale = Mathf.Min(scaleX, scaleY, scaleZ);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position +
                new Vector3(this.Offset.x * scaleX, this.Offset.y * scaleY, this.Offset.z * scaleZ), this.Radius * scale);
        }
    }
}
