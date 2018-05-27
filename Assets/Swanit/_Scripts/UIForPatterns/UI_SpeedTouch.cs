using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpeedTouch : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);
        UIManager.Instance.ShowSecondaryQuestion();

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
        }
       
    }

    public override void Reset()
    {
    }
}
