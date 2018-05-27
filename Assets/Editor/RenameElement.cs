using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class RenameElement : Editor 
{
    //% = cmd or ctrl, # = shift, & = alt
    [MenuItem("ProjectUtility/Utilities/Set Button Text as Holder name %r")]
    public static void Rename_ButtonText()
    {
        GameObject current = Selection.activeGameObject;
        string name = current.name;
        string formedName = string.Empty;
        bool formNameNow = false;
        //parsing the name to get the useful info out
        for (int i = 0; i < name.Length; i++)
        {
            formedName += name[i];

            if(formedName.Equals("Text_") || formedName.Equals("Button_"))
            {
                formNameNow = true;
            }

            if(formNameNow)
            {
                formedName = string.Empty;
                formNameNow = false;
            }
        }

        Text text = current.GetComponent<RectTransform>().GetComponentInChildren<Text>(true);
        if(text != null)
        {
            text.text = formedName;
        }
    }

    //% = cmd or ctrl, # = shift, & = alt
    [MenuItem("ProjectUtility/Utilities/Rename as Parent Name %&r")]
    public static void SameNameAsParent()
    {
        GameObject current = Selection.activeGameObject;
        current.name = current.transform.parent.name;
    }
}
