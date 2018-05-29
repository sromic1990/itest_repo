using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TrueFalse : UI_Base//MonoBehaviour
{
    public Text QuestionDisplay;
    public RectTransform monkey;
    public List<AnswerButtonHolder> mButtonHolder;

    void Start()
    {
        InvokeRepeating("FlipMonkey", 0.5f, 0.5f);
    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);
        QPattern p = GameManager.Instance.GetCurrentQuestion().Pattern;
        //     QuestionDisplay.text = info.Question;
        if(monkey != null)
        {
            if (p != QPattern.TrueFalse)
            {
                monkey.gameObject.SetActive(false);
            }
            else
            {
                monkey.gameObject.SetActive(true);
            }
        }


        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
        }
    }

    private void FlipMonkey()
    {
        if (monkey != null)
        {
            Vector3 locScale = monkey.localScale;
            monkey.localScale = new Vector3(-locScale.x, 1, 1);
        }
    }

    public override void Reset()
    {
        
    }
}