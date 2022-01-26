using Interaction;
using Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Interaction
{
    public class Crate : MonoBehaviour, IGrabObject
    {
        [Header("References")]
        [SerializeField] private Rigidbody rigidbody;
        public Rigidbody Rigidbody { get { return rigidbody; } }

        [SerializeField] private Transform grabPoint;
        public Transform GrabPoint { get { return grabPoint; } }

        [SerializeField] private AudioSource audioSource;
        public AudioSource AudioSource { get { return audioSource; } }

        [SerializeField] private AudioClip[] collisionClips;
        public AudioClip[] CollisionClips { get { return collisionClips; } }

        float StayLastPlay { get; set; }
        float LastPlay { get; set; }

        private void OnCollisionEnter(Collision collision)
        {
            if (this.TryPlayCollision(this.LastPlay, collision, 2.8f))
                this.LastPlay = -.1f;
        }

        private void OnCollisionStay(Collision collision)
        {
            if (this.TryPlayCollision(this.StayLastPlay, collision, 3.3f))
                this.StayLastPlay = -.25f;
        }

        private void LateUpdate()
        {
            this.StayLastPlay = Mathf.Clamp(this.StayLastPlay + Time.deltaTime, -1, 0);
            this.LastPlay = Mathf.Clamp(this.LastPlay + Time.deltaTime, -1, 0);
        }

        private bool TryPlayCollision(float cooldown, Collision collision, float minVelocity)
        {
            if (cooldown < 0)
                return false;

            float magnitude = collision.relativeVelocity.magnitude;

            if (magnitude < minVelocity)
                return false;

            float volume = (magnitude * magnitude) / 10f;
            float pitch = Mathf.Clamp(magnitude / 5f, .85f, 1.2f);

            this.AudioSource.transform.position = collision.contacts[0].point;
            this.AudioSource.volume = volume;
            this.AudioSource.pitch = pitch;
            this.AudioSource.PlayOneShot(this.CollisionClips[UnityEngine.Random.Range(0, this.CollisionClips.Length)]);

            return true;
        }
    }
}
