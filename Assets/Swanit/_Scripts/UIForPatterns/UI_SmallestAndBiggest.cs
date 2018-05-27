using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SmallestAndBiggest : UI_Base
{
    public Text QuestionDisplay;
    public RectTransform answers;
    public float shrink;
    [Space(20)]
    public List<AnswerButtonHolder> mAnswerHolder;
    private List<Vector2> defaultSize = new List<Vector2>();
    private List<Vector2> defaultPos = new List<Vector2>();

    private int TotalBallNo;

    private void Awake()
    {
        
    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        //    QuestionDisplay.text = info.Question;
        TotalBallNo = info.QuestionData_Int[0];
        setPositions();

        Debug.Log(TotalBallNo);

        for (int i = 0; i < TotalBallNo; i++)
        {
            mAnswerHolder[i].gameObject.SetActive(true);
            mAnswerHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            SetSize(mAnswerHolder[i], info, i);
        }

    }

    private void setPositions()
    {
        float w = answers.rect.width - shrink;
        //      Debug.Log("Width = " + w);

        for (int i = 0; i < TotalBallNo; i++)
        {
            Vector2 pos = new Vector2(w * i / (TotalBallNo - 1) + shrink * 0.5f, -155.0f);
//            Debug.Log("Xposition = " + pos.x);
            mAnswerHolder[i].GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    private void SetSize(AnswerButtonHolder mBtnHolder, QuestionUIInfo info, int index)
    {
        RectTransform rt = mBtnHolder.GetComponent<RectTransform>();
        defaultSize.Add(rt.sizeDelta);
        rt.sizeDelta *= info.QuestionData_Float[index];
    }

    public override void Reset()
    {
        for (int i = 0; i < TotalBallNo; i++)
        {
            mAnswerHolder[i].GetComponent<RectTransform>().sizeDelta = defaultSize[i];
        }
    }

}
