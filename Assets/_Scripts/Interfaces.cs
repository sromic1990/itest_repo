using System;
using System.Collections.Generic;

public interface ISetter
{
	void SetQuestion (BaseQuestion info, Action<int, int, bool, List<SequenceOfClick>> Answered);
}

public interface IEvaluator
{
	void ButtonClicked (int buttonID, int ClickCounter, bool isCorrect, List<SequenceOfClick> SequnceOfClicks);
}

public interface IUISetter
{
	void SetUI (QuestionUIInfo info);
}