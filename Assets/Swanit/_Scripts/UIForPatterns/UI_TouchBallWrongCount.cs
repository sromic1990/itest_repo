using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_TouchBallWrongCount : UI_Base
{
    public Text countNo;
    public List<AnswerButtonHolder> mButtonHolder;
    public List<RectTransform> ButtonRects;
    private List<Vector2> currentPosList;
    private List<Vector2> defaultPos;
    private Transform parent;

    void OnEnable()
    {
        GameManager.Instance.Clicked += ButtonClicked;

    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        parent = ButtonRects[0].parent;

//        EProz.INSTANCE.cancelDelayCall = false;

        defaultPos = new List<Vector2>();
        currentPosList = new List<Vector2>();
        //     QuestionDisplay.text = info.Question;

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            defaultPos.Add(ButtonRects[i].anchoredPosition);
            currentPosList.Add(ButtonRects[i].anchoredPosition);
        }
    }

    private void ButtonClicked(int count, int a)
    {
        AnswerID id = (AnswerID)a;
        Debug.Log(id.ToString() + "     " + count);

        if (id == AnswerID.Ball_Green)
        {
            count = (count <= 8) ? count : count - 1;
            countNo.text = "Count : " + count.ToString();

            switch (count)
            {
                case 2:
                    DistractVariation(1, 1.1f, 0);
                    break;
                case 3:
                    DistractVariation(1, 1.0f, 0);
                    break;
                case 4:
                    DistractVariation(1, 1.1f, 3);
                    break;
                case 5:
                    DistractVariation(1, 1.0f, 3);
                    break;
                case 7:
                    DistractVariation(2, 1.0f, 0, 1);
                    break;
                case 9:
                    DistractVariation(2, 1.0f, 1, 3);
                    break;
                case 10:
                    DistractVariation(1, 1.1f, 3);
                    break;
                case 11:
                    DistractVariation(1, 1.0f, 3);
                    break;
            }
        }
        else if (id == AnswerID.Ball_Blue)
        {
            // Debug.Log(id.ToString() + "     " + count);
            switch (count)
            {
                case 2:
                    DistractVariation(1, 1.1f, 3);
                    break;
                case 3:
                    DistractVariation(1, 1.0f, 3);
                    break;
            }
        }
    }

    //Mode 1 Scale;
    //Mode 2 Swap;
   
    private void DistractVariation(int mode, float mul, int arrayIndex, int swapIndex = -1)
    {
        if (mode == 1)
        {
            ButtonRects[arrayIndex].localScale = Vector3.one * mul;
        }
        else if (mode == 2)
        {
            Vector2 swap1 = currentPosList[arrayIndex];
            Vector2 swap2 = currentPosList[swapIndex];

            ButtonRects[arrayIndex].DOAnchorPos(swap2, 0.3f);
            ButtonRects[swapIndex].DOAnchorPos(swap1, 0.3f);
            Invoke("ReassignPos", 0.5f);

        }
        
    }

    public void ReassignPos()
    {
        for (int i = 0; i < ButtonRects.Count; i++)
        {
            Debug.Log(parent.name);
            currentPosList[i] = parent.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
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
