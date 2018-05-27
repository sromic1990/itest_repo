using System;
using System.Collections.Generic;
using IdiotTest.Scripts.GameScripts;
using UnityEngine;

[Serializable]
public struct QuestionUIInfo
{
	public string Question;
	public List<string> SecondaryQuestion;
	public BaseSpriteHolder Q_Image;
	public List<float> QuestionData_Float;
	public List<int> QuestionData_Int;
	public List<ButtonProperties> ButtonAnswer;

	public QuestionUIInfo (string Question, List<string> SecondaryQuestion, BaseSpriteHolder Q_Image, List<float> QuestionData_Float, List<int> QuestionData_Int, List<ButtonProperties> ButtonAnswer)
	{
		this.Question = Question;
		this.SecondaryQuestion = SecondaryQuestion;
		this.Q_Image = Q_Image;
		if (this.Q_Image == null) {
			this.Q_Image = new BaseSpriteHolder ();
			this.Q_Image.Sprite = SpriteID.DummySprite;
		} else {
			this.Q_Image.Sprite = Q_Image.Sprite;
		}
		this.QuestionData_Float = QuestionData_Float;
		this.QuestionData_Int = QuestionData_Int;
		this.ButtonAnswer = ButtonAnswer;
	}
}