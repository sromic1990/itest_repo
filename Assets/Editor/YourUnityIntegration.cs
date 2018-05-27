using UnityEngine;
using UnityEditor;

static class YourUnityIntegration {

	[MenuItem("Assets/Create/CreateScriptableObject")]
	public static void CreateYourScriptableObject() {
		Object obj = Selection.activeObject;
		if (obj != null)
			ScriptableObjectUtility.CreateAsset (obj);
	}

}