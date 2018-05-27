using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;

public class UI_Giraffe : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        //   QuestionDisplay.text = info.Question;

        int data1 = info.QuestionData_Int[0];
        int data2 = info.QuestionData_Int[1];

        GameManager.Instance.GetCurrentQuestion().ReturnValue_Bool = true;

        EProz.INSTANCE.WaitAndCall(data1, () =>
            {
                mButtonHolder[0].gameObject.SetActive(true);
                mButtonHolder[0].SetAnswerButtonProperties(info.ButtonAnswer[0]);

            });

        EProz.INSTANCE.WaitAndCall(data2, () =>
            {
                mButtonHolder[1].gameObject.SetActive(true);
                mButtonHolder[1].SetAnswerButtonProperties(info.ButtonAnswer[1]);

                EProz.INSTANCE.WaitAndCall(2.0f, () =>
                    {
                        //  GameManager.Instance.CurrentQuestion.ReturnValue_Bool = false;
                        if (!GameManager.Instance.Answered)
                        {
                            GameManager.Instance.AnsweredWrongly();
                        }
                    });
            });

    }

    public override void Reset()
    {
        for (int i = 0; i < mButtonHolder.Count; i++)
            mButtonHolder[i].gameObject.SetActive(false);
    }
}
