using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Setter_CatchMonkey : BaseSetter
{
	public override void SetQuestion (BaseQuestion info, Action<int, int, bool, List<SequenceOfClick>> Answered)
	{
		base.SetQuestion (info, Answered);
		Set ();
	}

	protected override void Set ()
	{
		List<ButtonProperties> buttons = new List<ButtonProperties> ();
		buttons.Add (new ButtonProperties (info.Options [0].Sprite, info.Options[0].SecondarySprites, info.Options [0].text, info.Options [0].ID, CorrectlyAnswered, info.Options [0].IsCorrect, info.Options[0].SequenceInfo));
		QuestionUIInfo catchMonkey = new QuestionUIInfo (info.Question, info.SecondaryQuestion, info.QuestionSprite [0], info.QuestionData_Float, info.QuestionData_Int, buttons);
		UIManager.Instance.SetUI (info.Pattern, catchMonkey);
	}
}
