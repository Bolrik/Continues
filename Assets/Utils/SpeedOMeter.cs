using UnityEngine;

namespace Utils
{
    public class SpeedOMeter : MonoBehaviour
    {
        [SerializeField] private Transform target;
        public Transform Target { get { return target; } }

        [SerializeField] private bool includeY;
        public bool IncludeY { get { return includeY; } }

        [SerializeField] private float sampleRate = .33f;
        public float SampleRate { get { return sampleRate; } }


        Vector3 Previous { get; set; }
        float Delta { get; set; }

        private void Update()
        {
            if (this.target == null) return;

            this.Delta += Time.deltaTime;

            if (this.Delta < this.SampleRate)
                return;

            Vector3 current = this.Target.position;
            Vector3 delta = this.Previous - current;

            if (!this.IncludeY)
                delta.y = 0;

            Debug.Log(delta.magnitude / this.Delta);

            this.Previous = current;
            this.Delta = 0;
        }
    }
}
