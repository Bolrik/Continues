using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        AudioClip OnUseSound { get; }
        Sprite Icon { get; }

        bool Activate();
        bool CanActivate();
    }
}