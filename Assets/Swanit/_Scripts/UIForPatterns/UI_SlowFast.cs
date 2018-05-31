using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sourav.Utilities.Extensions;

public class UI_SlowFast : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;
    private List<float> monkeySpeed;

    private bool isUISet = false;

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        monkeySpeed = new List<float>();

//        EProz.INSTANCE.cancelDelayCall = false;

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].gameObject.Show();
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            monkeySpeed.Add(info.QuestionData_Float[i]);
            Debug.Log(monkeySpeed[i]);
        }
       
        SetSpeedMonkey();
        isUISet = true;
    }

    private void SetSpeedMonkey()
    {

        Debug.Log("Setting Monkey Speed");

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            RectTransform r = mButtonHolder[i].GetComponent<RectTransform>();
            float xPos = Random.Range(300, 500);
            float variation = Random.Range(200, 300);
            int operation = (System.Environment.TickCount % 2 == 0) ? 1 : -1;
            Debug.Log("OP = " + operation + "     variation = " + variation);
            float x = xPos + variation * operation;
            r.anchoredPosition = new Vector2(x, r.anchoredPosition.y);
            setAnimation(mButtonHolder[i], i);
        }
    }

    private void setAnimation(AnswerButtonHolder btn, int d)
    {
        RectTransform mRecttrans = btn.GetComponent<RectTransform>();
        Debug.Log("Setting Animation ==");
        mRecttrans.DOAnchorPosX(2550.0f, monkeySpeed[d]).SetEase(Ease.Linear);
    }

    public override void Reset()
    {
        if (isUISet)
        {
            Debug.LogError("RESET");

            for (int i = 0; i < mButtonHolder.Count; i++)
            {
                //   Debug.Log("Stopping Tween");
                RectTransform rt = mButtonHolder[i].GetComponent<RectTransform>();
                rt.DOPause();
                rt.gameObject.Hide();
                rt.anchoredPosition = new Vector2(200.0f, rt.anchoredPosition.y);
            }
            isUISet = false;
        }

    }

}