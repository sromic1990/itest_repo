using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BurstBaloon : UI_Base
{

    public float delayTime = 1.5f;
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolders;

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        // QuestionDisplay.text = info.Question;
        StartCoroutine(DisplayBalloons(info));
    }

    IEnumerator DisplayBalloons(QuestionUIInfo info)
    {
        for (int i = 0; i < mButtonHolders.Count; i++)
        {
            mButtonHolders[i].gameObject.SetActive(true);
            mButtonHolders[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            yield return new WaitForSeconds(delayTime);
        }
    }

    public override void Reset()
    {
        for (int i = 0; i < mButtonHolders.Count; i++)
            mButtonHolders[i].gameObject.SetActive(false);
    }
}
