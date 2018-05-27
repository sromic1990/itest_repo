using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlayButton : UI_Base, IPointerClickHandler
{
    public Text QuestionDisplay;
    public QPattern e_Touch;

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        //  QuestionDisplay.text = info.Question;
    }

    public void OnPointerClick(PointerEventData ped)
    {
        string CorrectObjName = getItemName(e_Touch);

        if (ped.pointerEnter.name.Equals(CorrectObjName))
        {
            Debug.Log("Correct ANswer");
            GameManager.Instance.AnsweredCorrectly();
            Debug.Log("Correct ANswer Done");
        }
        else
        {
            Debug.Log("Wrong ANswer");
            GameManager.Instance.AnsweredWrongly();
            Debug.Log("Wrong ANswer Done");
        }
    }

    private string getItemName(QPattern q)
    {
        string ans = "";


        switch (q)
        {
            case QPattern.PlayButton:
                ans = "Answer";
                break;
            case QPattern.LadyHands:
                ans = "RightThumb";
                break;
            case QPattern.Hifi:
                ans = "HiFi_hand";
                break;
            default :
                ans = "None";
                break;
        }
        Debug.Log("AND = " + ans);

        return ans;
    }

    public override void Reset()
    {
    }
}
