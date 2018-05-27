using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_Ballons : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;
    int num;

    private int time;

    private QuestionUIInfo mInfo;

    void OnEnable()
    {
        GameManager.Instance.TimeTicker += GetTime;
        //GameManager.Instance.ResetAllData += Reset;
    }

    private void GetTime(int t)
    {
        if (mInfo.QuestionData_Int.Contains(t))
        {
            for (int i = 0; i < mInfo.QuestionData_Int.Count; i++)
            {
                if (t == mInfo.QuestionData_Int[i])
                {
                    mButtonHolder[i].gameObject.SetActive(true);
                    mButtonHolder[i].SetAnswerButtonProperties(mInfo.ButtonAnswer[i]);
                }
            }
        }

    }

    void OnDisable()
    {
        GameManager.Instance.TimeTicker -= GetTime;
        //GameManager.Instance.ResetAllData -= Reset;
    }

    public override void SetUI(QuestionUIInfo info)
    {
        Debug.Log("------------------>This Set UI");

        base.SetUI(info);
        //QuestionDisplay.text = info.SecondaryQuestion[0];
        UIManager.Instance.ShowSecondaryQuestion();

        QPattern pattern = GameManager.Instance.GetCurrentQuestion().Pattern;

        mInfo = info;
    }

    public override void Reset()
    {
        StopAllCoroutines();

        for (int i = 0; i < num; i++)
            mButtonHolder[i].gameObject.SetActive(false);

        //  base.Reset();
    }


    private int getCount(QPattern qp)
    {
        if (qp == QPattern.NextFiveBalloons)
        {
            return 9;
        }
        else if (qp == QPattern.NextThreeBalloons)
        {
            return 7;
        }
        else if (qp == QPattern.TouchExceptYellowBall)
        {
            return 8;
        }
        else
        {
            return mButtonHolder.Count;
        }
    }
}
