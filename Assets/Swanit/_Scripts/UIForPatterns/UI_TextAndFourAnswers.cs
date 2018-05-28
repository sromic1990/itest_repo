using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextAndFourAnswers : UI_Base
{
    public Text QuestionDisplay;
    public Text textDisplay;
    public Image QImage;
    public List<AnswerButtonHolder> mButtonHolder;

    public override void Reset()
    {
    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        // QuestionDisplay.text = info.Question;
        textDisplay.text = info.Q_Image.text;
        QImage.sprite = GetCorrectSpriteByID.Instance.GetSpriteFromID(info.Q_Image.Sprite);

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i], true);
        }
    }
}
