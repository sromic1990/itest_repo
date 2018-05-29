using System.Collections;
using System.Collections.Generic;
using Sourav.Utilities.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpeedTouch : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;

    public override void SetUI(QuestionUIInfo info)
    {
        Debug.Log("SetUI_SpeedTouch");

        base.SetUI(info);
        UIManager.Instance.ShowSecondaryQuestion();

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            mButtonHolder[i].gameObject.Show();
        }
       
    }

    public override void Reset()
    {
    }
}
