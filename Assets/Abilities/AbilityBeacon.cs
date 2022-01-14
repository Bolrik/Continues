using Conditions;
using UnityEngine;

namespace Abilities
{
    public class AbilityBeacon : MonoBehaviour, IAbilityReceiver
    {
        [Header("References")]
        [SerializeField] private Transform previewTransform;
        public Transform PreviewTransform { get { return previewTransform; } }

        [SerializeField] private Transform beaconBeam;
        public Transform BeaconBeam { get { return beaconBeam; } }

        [SerializeField] private SpriteRenderer previewSpriteRenderer;
        public SpriteRenderer PreviewSpriteRenderer { get { return previewSpriteRenderer; } }

        [SerializeField] private MeshRenderer isPersistentRenderer;
        public MeshRenderer IsPersistentRenderer { get { return isPersistentRenderer; } }

        [Header("Details")]
        [SerializeField] private Ability stored;
        public Ability Stored { get { return stored; } private set { stored = value; } }

        [SerializeField] private bool isPersistent = false;
        public bool IsPersistent { get { return isPersistent; } }

        [SerializeField] private ConditionContainer isActiveConditionContainer;
        public ConditionContainer IsActiveConditionContainer { get { return isActiveConditionContainer; } }



        public OnAbilityChanged AbilityChanged { get; set; }
        public bool IsActive { get; private set; }

        private void Start()
        {
            this.IsPersistentRenderer.material.color = this.IsPersistent ? Color.green : this.IsPersistentRenderer.material.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!this.IsActive)
                return;

#warning GGF umstellen auf TriggerStay????

            if (other.GetComponentInParent<IAbilityReceiver>() is IAbilityReceiver abilityReceiver)
            {
                this.SetAbility(abilityReceiver.SwapAbility(this.Stored));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
        }

        private void Update()
        {
            this.IsActive = this.IsActiveConditionContainer.Check();

            this.AnimatePreview();
            this.DisplayPreview();
        }

        private void AnimatePreview()
        {
            if (this.Stored == null || !this.IsActive)
                return;

            this.PreviewTransform.localPosition = Vector3.up * Mathf.Sin(Time.time) * .2f;
            this.PreviewTransform.localEulerAngles += Vector3.up * 180 * Time.deltaTime;
        }

        private void DisplayPreview()
        {
            if (this.Stored == null || !this.IsActive)
            {
                this.BeaconBeam.gameObject.SetActive(this.IsActive);
                this.PreviewTransform.gameObject.SetActive(false);
                this.PreviewSpriteRenderer.sprite = null;
            }
            else
            {
                this.PreviewTransform.gameObject.SetActive(true);
                this.PreviewSpriteRenderer.sprite = this.Stored.PreviewSprite.Sprite;
                this.PreviewSpriteRenderer.color = this.Stored.PreviewSprite.Color;
            }
        }


        public void SetAbility(Ability ability)
        {
            if (this.IsPersistent)
                return;

            this.Stored = ability;
            this.AbilityChanged?.Invoke(this.Stored);
        }

        // Use Extension Method
        Ability IAbilityReceiver.SwapAbility(Ability swapTo)
        {
            return this.SwapAbility(swapTo);
        }
    }
}