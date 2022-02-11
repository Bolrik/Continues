using UnityEngine;

namespace UnitControlls
{
    [CreateAssetMenu(fileName = "New Player Config", menuName = "Player/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float interactionRange = 2.75f;
        public float InteractionRange { get { return interactionRange; } }

        [SerializeField] private float interactionBuffer = .15f;
        public float InteractionBuffer { get { return interactionBuffer; } }


    }
}
