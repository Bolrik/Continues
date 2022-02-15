using UnityEngine;

namespace Utils
{
    public class MoveTowards : MonoBehaviour
    {
        [SerializeField] private Transform moveThis;
        public Transform MoveThis { get { return moveThis; } }

        [SerializeField] private Transform toHere;
        public Transform ToHere { get { return toHere; } }

        [SerializeField] private float speed = 1f;
        public float Speed { get { return speed; } }


        private void Update()
        {
            this.MoveThis.position = Vector3.MoveTowards(this.MoveThis.position, this.ToHere.position, this.Speed * Time.deltaTime);


        }
    }
}
