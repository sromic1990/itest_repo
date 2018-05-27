using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TouchBallWrongCount : UI_Base
{
    public Text countNo;
    public List<AnswerButtonHolder> mButtonHolder;
    public List<RectTransform> ButtonRects;
    private List<Vector2> defaultPos;



    void OnEnable()
    {
        GameManager.Instance.Clicked += ButtonClicked;
    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        defaultPos = new List<Vector2>();
        //     QuestionDisplay.text = info.Question;

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            defaultPos.Add(ButtonRects[i].anchoredPosition);
        }
    }

    private void ButtonClicked(int count, int a)
    {
        Debug.Log(count);
        AnswerID id = (AnswerID)a;

        if (id == AnswerID.Ball_Green)
        {
            count = (count <= 8) ? count : count - 1;
            countNo.text = "Count : " + count.ToString();   
        }
    }

    public override void Reset()
    {
        countNo.text = "";

        for (int i = 0; i < ButtonRects.Count; i++)
        {
            ButtonRects[i].anchoredPosition = defaultPos[i];
            ButtonRects[i].localScale = Vector3.one;
        }
    }

    void OnDisable()
    {
        GameManager.Instance.Clicked -= ButtonClicked;
    }

}
