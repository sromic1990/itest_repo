using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;
using Sourav.Utilities.Extensions;

public class UI_ObjectsInPrevious : UI_Base
{
    public Text QuestionDisplay;
    public List<Image> QuestionImages;
    public List<AnswerButtonHolder> mButtonHolder;
    public RectTransform QimageParent;
    public RectTransform AnswerParent;
    public float displayTime;
// = 5.0f;


    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        //     QuestionDisplay.text = info.Question;

        BaseQuestion question = GameManager.Instance.GetCurrentQuestion();


        displayTime = info.QuestionData_Float[0];

        for (int i = 0; i < question.QuestionSprite.Count; i++)
            QuestionImages[i].sprite = GetCorrectSpriteByID.Instance.GetSpriteFromID(question.QuestionSprite[i].Sprite);

        QimageParent.gameObject.SetActive(true);


        EProz.INSTANCE.WaitAndCall(displayTime, () =>
            {
                QimageParent.gameObject.SetActive(false);
                //QuestionDisplay.text = info.SecondaryQuestion [0];

                UIManager.Instance.ShowSecondaryQuestion();
                AnswerParent.gameObject.SetActive(true);
                //    UIManager.Instance.ShowSecondaryQuestion();

                for (int i = 0; i < mButtonHolder.Count; i++)
                {
                    mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
                }

            });
    }

    public override void Reset()
    {
        AnswerParent.gameObject.Hide();
        QimageParent.gameObject.Show();
    }
}
