using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSetter : MonoBehaviour, ISetter
{
	protected BaseQuestion info;
	protected Action<int, int, bool, List<SequenceOfClick>> CorrectlyAnswered, WronglyAnswered;

	protected abstract void Set ();

	public virtual void SetQuestion (BaseQuestion info, Action<int, int, bool, List<SequenceOfClick>> Answered)
	{
		this.info = info;
		this.CorrectlyAnswered = Answered;
		this.WronglyAnswered = Answered;
	}
}
