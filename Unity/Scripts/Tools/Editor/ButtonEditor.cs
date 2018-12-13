#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace ATF {
    namespace ButtonsEditor {
        [CustomEditor(typeof(UnityEngine.Object), true)]
        public class ButtonEditor : Editor {

            public override void OnInspectorGUI() {
                base.OnInspectorGUI();
                
                foreach (var method in target.GetType().GetMethods()) {

                    var methodAtr = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));
                    
                    if (methodAtr != null) {
                        bool canDown = (EditorApplication.isPlaying && methodAtr.enabled == Enabled.EnabledPlayMode) 
                                        || (!EditorApplication.isPlaying && methodAtr.enabled == Enabled.DisablePlayMode) 
                                        || (methodAtr.enabled == Enabled.AlwaysEnabled);

                        if (GUILayout.Button(new GUIContent(method.Name, methodAtr.tooltip))) {
                            if (canDown) {
                                foreach (var target in targets) {
                                    method.Invoke(target, null);
                                }
                            } else {
                                Debug.Log("Access error!");
                            }
                        }
                    }
                }
            }
        }
    }
}
#endif