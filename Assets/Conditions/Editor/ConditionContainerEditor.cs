using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Conditions.Editors
{
    [CustomEditor(typeof(ConditionContainer))]
    public class ConditionContainerEditor : Editor
    {
        int ConditionTypeIndex { get; set; }
        string[] ConditionTypes { get; set; }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            this.ConditionTypes = this.ConditionTypes ?? Enum.GetNames(typeof(ConditionType));

            this.ConditionTypeIndex = EditorGUILayout.Popup(this.ConditionTypeIndex, this.ConditionTypes);

            if (GUILayout.Button("Create Condition"))
            {
                if (!(this.target is ConditionContainer conditionContainer))
                    return;

                ConditionType conditionType = (ConditionType)this.ConditionTypeIndex;
                switch (conditionType)
                {
                    case ConditionType.PlayerAbilityCondition:
                        conditionContainer.Add(new PlayerAbilityCondition());
                        break;
                    case ConditionType.PlayerLevelCondition:
                        conditionContainer.Add(new PlayerAbilityCondition());
                        break;
                    default:
                        return;
                }

            }
        }
    }

    public enum ConditionType
    {
        PlayerAbilityCondition,
        PlayerLevelCondition
    }
}
