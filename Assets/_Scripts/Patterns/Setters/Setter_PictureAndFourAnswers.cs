using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Setter_PictureAndFourAnswers : BaseSetter
{
    public override void SetQuestion(BaseQuestion info, Action<int, int, bool, List<SequenceOfClick>> Answered)
    {
        base.SetQuestion(info, Answered);
        Set();

        float t = Mathf.InverseLerp(9, 6, 3);
    }

    protected override void Set()
    {
        //Set the question here
        List<ButtonProperties> buttonProperties = new List<ButtonProperties>();
        for (int i = 0; i < info.Options.Count; i++)
        {
            Action<int, int, bool, List<SequenceOfClick>> actionOnClick = info.Options[i].IsCorrect ? CorrectlyAnswered : WronglyAnswered;

            ButtonProperties button = new ButtonProperties(info.Options[i].Sprite, info.Options[i].SecondarySprites, info.Options[i].text, info.Options[i].ID, actionOnClick, info.Options[i].IsCorrect, info.Options[i].SequenceInfo, info.Options[i].TextColor);
            buttonProperties.Add(button);
        }

        QuestionUIInfo pictureAndFourAnswers = new QuestionUIInfo(info.Question, info.SecondaryQuestion, info.QuestionSprite[0], info.QuestionData_Float, info.QuestionData_Int, buttonProperties);
        UIManager.Instance.SetUI(info.Pattern, pictureAndFourAnswers);
    }
	
	
}
