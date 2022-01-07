using UnityEngine;

namespace Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField] private PreviewSprite previewSprite;
        public PreviewSprite PreviewSprite { get { return previewSprite; } }

    }

    [System.Serializable]
    public struct PreviewSprite
    {
        [SerializeField] private Sprite sprite;
        public Sprite Sprite { get { return sprite; } }

        [SerializeField] private Color color;
        public Color Color { get { return color; } }
    }
}