using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using Object = UnityEngine.Object;

public class ToolsEditor {

    [MenuItem("Tools/Clear Console &R")]
	static void ClearConsole() {
        Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditorInternal.LogEntries");
        type.GetMethod("Clear").Invoke(null, null);
    }

}
