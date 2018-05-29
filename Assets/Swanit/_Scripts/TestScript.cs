using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestScript : MonoBehaviour
{

    public QuestionHolder qHolder;
    public QuestionHolder testHold;

    private int index1, index2;

    [ContextMenu("Log Patterns")]
    public void sdjhs()
    {
        int index = 0;
        for (int i = 0; i < qHolder.Questions.Count; i++)
        {
            if (qHolder.Questions[i].QuestionType == QuestionType.NoBase)
            {
                Debug.Log(qHolder.Questions[i].Pattern.ToString() + "     @" + i);
            }
            //    Debug.Log(qHolder.Questions[i].Question);
        }
    }

    [ContextMenu("OrderInPatternGroup")]
    public void asjkak()
    {
        qHolder.Questions = qHolder.Questions.OrderBy(t => t.Pattern).ToList();
    }

    [ContextMenu("SwapElements")]
    public void swapElem()
    {

        #if UNITY_EDITOR
        EditorUtility.SetDirty(testHold);
        #endif

        index1 = testHold.Questions.FindIndex(f => f.sortOrder == 1004);
        index2 = testHold.Questions.FindIndex(f => f.sortOrder == 1005);

        BaseQuestion bq = testHold.Questions[index2];
        testHold.Questions[index2] = testHold.Questions[index1];
        testHold.Questions[index1] = bq;

        for (int i = 0; i < testHold.Questions.Count; i++)
            testHold.Questions[i].sortOrder = -1;
    }
     
    //    [ContextMenu("MoveElement")]
    //    public void asdklasl()
    //    {
    //        BaseQuestion q = testHold.Questions.Find(f => f.sortOrder == -200);
    //        q.sortOrder = -1;
    //        testHold.Questions.Remove(q);
    //        insertAt = insertAt - 1;
    //        testHold.Questions.Insert(insertAt, q);
    //
    //        remove_ind();
    //        Add_ind();
    //    }

    [ContextMenu("Add Index")]
    public void Add_ind()
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(testHold);
        #endif

        for (int i = 0; i < testHold.Questions.Count; i++)
        {
            testHold.Questions[i].Question += "~~~" + (i + 1).ToString();
        }
    }

    [ContextMenu("Remove Index")]
    public void remove_ind()
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(testHold);
        #endif

        for (int i = 0; i < testHold.Questions.Count; i++)
        {
            testHold.Questions[i].Question = testHold.Questions[i].Question.Split(new char[]{ '~', '~', '~' }, System.StringSplitOptions.None)[0];
        }
    }

    public int min, max;

    [ContextMenu("Generate Random Between")]
    public void lskdjens()
    {
//        int x = System.Environment.TickCount;
//        Debug.Log(x);
//        x = (x > 0) ? x : -x;
//        Debug.Log(x);
//        x = x % max;
//        Debug.Log(x);
//        x = (x + min > max) ? ((2 * max) - (x + min)) : x + min;

        int x = Random.Range(min, max);
        Debug.Log("Random no =" + x);
    }


    [ContextMenu("SetOrderDefault")]
    public void kajdl()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(qHolder);
#endif

        for (int i = 0; i < qHolder.Questions.Count; i++)
        {
            Debug.Log(qHolder.Questions[i].Pattern);
            qHolder.Questions[i].sortOrder = -1;
        }
    }
        
}