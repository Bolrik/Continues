using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using Utils.UI;

namespace Assets.Editors.UI
{
    [CustomEditor(typeof(UIImage))]
    class UIImageEditor : ImageEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            this.serializedObject.Update();

            SerializedProperty invertMaskProperty = this.serializedObject.FindProperty("invertMask");
            if (invertMaskProperty != null)
            {
                EditorGUILayout.PropertyField(invertMaskProperty, new GUIContent("Invert Mask"));
            }
            this.serializedObject.ApplyModifiedProperties();

            // uIImage.InvertMask = EditorGUILayout.Toggle("Invert Mask", uIImage.InvertMask);

            // target.ApplyModifiedProperties();
        }
    }
}
