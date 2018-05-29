using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;

public class UI_Giraffe : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;

    private int callResetOnQuestion = 40;
    private int data1 = 40;
    private int data2 = 40;
    private bool showData1;
    private bool showData2;
    private bool checkData;
    private QuestionUIInfo info;

    public void OnDisable()
    {
        GameManager.Instance.TimeTicker += TimerTick;
    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);
        this.info = info;
        GameManager.Instance.TimeTicker += TimerTick;
        showData1 = true;
        showData2 = true;
        checkData = true;

        data1 = info.QuestionData_Int[0];
        data2 = info.QuestionData_Int[1];

        callResetOnQuestion = data2 + 2;

        GameManager.Instance.GetCurrentQuestion().ReturnValue_Bool = false;
    }

    private void TimerTick(int timer)
    {
        if(timer > data1 && showData1)
        {
            mButtonHolder[0].gameObject.SetActive(true);
            mButtonHolder[0].SetAnswerButtonProperties(info.ButtonAnswer[0]);
            showData1 = false;
        }
        if(timer > data2 && showData2)
        {
            mButtonHolder[1].gameObject.SetActive(true);
            mButtonHolder[1].SetAnswerButtonProperties(info.ButtonAnswer[1]);
            showData2 = false;
            GameManager.Instance.GetCurrentQuestion().ReturnValue_Bool = true;
        }
        if(timer > callResetOnQuestion && checkData)
        {
            GameManager.Instance.GetCurrentQuestion().ReturnValue_Bool = false;
            if (!GameManager.Instance.Answered)
            {
                GameManager.Instance.AnsweredWrongly();
            }
        }

    }

    public override void Reset()
    {
        showData1 = false;
        showData2 = false;
        checkData = false;
        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].gameObject.SetActive(false);
        }
    }
}
