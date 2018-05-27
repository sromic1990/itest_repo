using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;

public class UI_IfElse : UI_Base
{
    public float DelayTime = 1.5f;
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].gameObject.SetActive(true);
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            mButtonHolder[i].gameObject.SetActive(false);
        }

        mButtonHolder[info.QuestionData_Int[0]].gameObject.SetActive(true);
        EProz.INSTANCE.WaitAndCall(DelayTime, () =>
            {
                mButtonHolder[info.QuestionData_Int[1]].gameObject.SetActive(true);
                EProz.INSTANCE.WaitAndCall(DelayTime, () =>
                    {
                        mButtonHolder[info.QuestionData_Int[2]].gameObject.SetActive(true);
                        GameManager.Instance.SetInput();
                    });
            });
    }


    public override void Reset()
    {
        for (int i = 0; i < mButtonHolder.Count; i++)
            mButtonHolder[i].gameObject.SetActive(false);
    }
     
}