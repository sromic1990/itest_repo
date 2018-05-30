using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PictureAndFourAnswers : UI_Base
{
    public Text Question;
    public Image Q_Image;
    public List<AnswerButtonHolder> ButtonAnswer;

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);
//        EProz.INSTANCE.cancelDelayCall = false;
        this.Question.text = info.Question;
        this.Q_Image.sprite = GetCorrectSpriteByID.Instance.GetSpriteFromID(info.Q_Image.Sprite);

        for (int i = 0; i < ButtonAnswer.Count; i++)
        {
            ButtonAnswer[i].SetAnswerButtonProperties(info.ButtonAnswer[i], true);
        }
    }

    public override void Reset()
    {
    }
}

