using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Conditions
{
    public class ConditionContainer : MonoBehaviour
    {
        IEnumerable<Condition> Conditions { get => this.PlayerAbilityConditions.Select(con => con as Condition); }

        [SerializeField] private List<PlayerAbilityCondition> playerAbilityConditions = new List<PlayerAbilityCondition>();
        public List<PlayerAbilityCondition> PlayerAbilityConditions { get { return playerAbilityConditions; } }




        public void Add(PlayerAbilityCondition condition)
        {
            this.PlayerAbilityConditions.Add(condition);
        }

        public bool CheckConditions()
        {
            return this.Conditions.All(condition => condition.Check());
        }
    }
}