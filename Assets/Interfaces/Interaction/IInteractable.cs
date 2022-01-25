using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        AudioClip OnUseSound { get; }
        Sprite Icon { get; }
        void Activate();
    }
}