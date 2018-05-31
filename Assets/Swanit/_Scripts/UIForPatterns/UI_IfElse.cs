using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;

public class UI_IfElse : UI_Base
{
    public float DelayTime = 0.7f;
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;

    private bool isUISet = false;

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        if (isUISet)
            return;

        isUISet = true;

//        EProz.INSTANCE.cancelDelayCall = false;

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
        if (isUISet)
        {
            isUISet = false;

            for (int i = 0; i < mButtonHolder.Count; i++)
                mButtonHolder[i].gameObject.SetActive(false);  
        }

    }
     
}