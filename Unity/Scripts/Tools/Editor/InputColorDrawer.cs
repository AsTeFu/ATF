using UnityEngine;
using UnityEditor;

namespace ATF {
    namespace Colors {
        [CustomPropertyDrawer(typeof(InputColor))]
        public class InputColorDrawer : PropertyDrawer {

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                GetPropertyHeight(property, label);
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}