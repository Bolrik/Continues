using UnityEngine;

namespace Abilities
{
    public class AbilityBeacon : MonoBehaviour, IAbilityReceiver
    {
        [Header("References")]
        [SerializeField] private Transform previewTransform;
        public Transform PreviewTransform { get { return previewTransform; } }

        [SerializeField] private SpriteRenderer previewSpriteRenderer;
        public SpriteRenderer PreviewSpriteRenderer { get { return previewSpriteRenderer; } }

        [Header("Details")]
        [SerializeField] private Ability stored;
        public Ability Stored { get { return stored; } private set { stored = value; } }

        public OnAbilityChanged AbilityChanged { get; set; }


        private void OnTriggerEnter(Collider other)
        {
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
            this.AnimatePreview();
            this.DisplayPreview();
        }

        private void AnimatePreview()
        {
            if (this.Stored == null)
                return;

            this.PreviewTransform.localPosition = Vector3.up * Mathf.Sin(Time.time) * .2f;
            this.PreviewTransform.localEulerAngles += Vector3.up * 180 * Time.deltaTime;
        }

        private void DisplayPreview()
        {
            if (this.Stored == null)
            {
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