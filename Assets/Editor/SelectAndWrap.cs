using UnityEngine;
using UnityEditor;

public class SelectAndWrap : Editor 
{
    //% = cmd or ctrl, # = shift, & = alt
    [MenuItem("ProjectUtility/Utilities/Wrap UI Object %w")]
    public static void SelectWrapUIObject()
    {
        GameObject originalGameObject = Selection.activeGameObject;
        if(originalGameObject != null && originalGameObject.GetComponent<RectTransform>() != null)
        {
            GameObject wrapper = new GameObject(originalGameObject.name);
            wrapper.AddComponent<RectTransform>();
            wrapper.GetComponent<RectTransform>().parent = originalGameObject.GetComponent<RectTransform>();
            wrapper.GetComponent<RectTransform>().parent = originalGameObject.GetComponent<RectTransform>().parent;
            originalGameObject.GetComponent<RectTransform>().parent = wrapper.GetComponent<RectTransform>();
        }
    }
}
