using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;

public class UI_TouchOneMan : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;


    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        // QuestionDisplay.text = info.Question;
        //     float orderConst = 0.7f;

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            mButtonHolder[i].gameObject.SetActive(false);
        }

        int index = 0;

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[index++].gameObject.SetActive(true);
        }
    }

    public override void Reset()
    {
        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].gameObject.SetActive(false);
        }
    }
}
