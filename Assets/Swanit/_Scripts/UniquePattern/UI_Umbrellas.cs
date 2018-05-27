using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Umbrellas : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;

    public override void Reset()
    {
    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        //QuestionDisplay.text = info.Question;

        for (int i = 0; i < mButtonHolder.Count; i++)
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
    }
}
