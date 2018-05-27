using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GetCorrectSpriteByID))]
public class GetCorrectSpriteByIDEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //var gcsbi = target as GetCorrectSpriteByID;
        //List<SpriteIDHolder> list = gcsbi.SpriteHolders;

        //for (int i = 0; i < list.Count; i++)
        //{
        //    //Debug.Log(temp);
        //    SpriteIDHolder test = list[i];
        //    test.ID = test.SpriteID.ToString();
        //}
    }
}
