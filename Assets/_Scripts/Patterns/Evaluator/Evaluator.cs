﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluator : MonoBehaviour, IEvaluator
{
    public void ButtonClicked(int buttonID, int ClickCounter, bool isCorrect, List<SequenceOfClick> SequenceOfClicks)
    {
        //Debug.Log("Clicked");

        Debug.Log("ClickCounter = " + ClickCounter);

        bool matchedSequence = false;
        BaseQuestion question = GameManager.Instance.GetCurrentQuestion();


        if (question.IgnoreClickCount && ClickCounter > 1)
            return;

        if (!isCorrect)
        {
            Debug.Log("!isCorrect");
            GameManager.Instance.AnsweredWrongly();
        }
        else
        {
            Debug.Log("IsCorrect");
            if (!question.HasSequenceAnswer && !question.HasBoolValue)
            {
                GameManager.Instance.AnsweredCorrectly();
            }
            else if (!question.HasSequenceAnswer && question.HasBoolValue)
            {
                if (question.ReturnValue_Bool)
                {
                    GameManager.Instance.AnsweredCorrectly();
                }
                else
                {
                    Debug.Log("!GameManager.Instance.CurrentQuestion.HasSequenceAnswer && !GameManager.Instance.CurrentQuestion.HasBoolValue");
                    GameManager.Instance.AnsweredWrongly();
                }
            }
            else if (question.HasSequenceAnswer)
            {
                Debug.Log("question.HasSequenceAnswer");

                byte properClicked = 0;
                byte properTimed = 0;

                if (GameManager.Instance.SequenceOfClick == null)
                {
                    Debug.LogError("Something wrong. Question has Sequence but No sequence data found from options.");
                }
                else
                {
                    if (question.CountSequenceIgnoreValue)
                    {
                        Debug.Log("CountSequenceIgnoreValue");
                        for (int i = 0; i < SequenceOfClicks.Count; i++)
                        {
                            if (!GameManager.Instance.SequenceCount.Contains(SequenceOfClicks[i].SequenceNumber))
                            {
                                Debug.Log("<color=cyan>!GameManager.Instance.SequenceCount.Contains(SequenceOfClicks[i].SequenceNumber)</color>");
                                GameManager.Instance.SequenceCount.Add(SequenceOfClicks[i].SequenceNumber);
                                if (GameManager.Instance.SequenceCount.Count >= GameManager.Instance.MaxSequence)
                                {
                                    Debug.Log("GameManager.Instance.SequenceCount.Count >= GameManager.Instance.MaxSequence");
                                    properClicked = 1;
                                    properTimed = 1;
                                    break;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                Debug.Log("Something wrong. More than 1 click on the same sequence number when sequence is to be ignored");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < SequenceOfClicks.Count; i++)
                        {
                            if (GameManager.Instance.SequenceOfClick.SequenceNumber == SequenceOfClicks[i].SequenceNumber)
                            {
                                if (question.IgnoreClickCount)
                                {
                                    Debug.Log("question.IgnoreClickCount");
                                    //ignore click count
                                    properClicked = 1;
                                }
                                else
                                {
                                    if (GameManager.Instance.SequenceOfClick.RequiredClicks == ClickCounter)
                                    {
                                        Debug.Log("==");
                                        properClicked = 1;
                                    }
                                    else if (GameManager.Instance.SequenceOfClick.RequiredClicks < ClickCounter)
                                    {
                                        Debug.Log("<");
                                        properClicked = 0;
                                    }
                                    else if (GameManager.Instance.SequenceOfClick.RequiredClicks > ClickCounter)
                                    {
                                        Debug.Log("RequiredClicks = "+GameManager.Instance.SequenceOfClick.RequiredClicks+ " and ClickCounter = "+ClickCounter);
                                        Debug.Log(">");
                                        properClicked = 0;
                                        //Keep clicking.
                                        return;
                                    }
                                }
                                
                                if (question.IgnoreClickTime)
                                {
                                    properTimed = 1;
                                }
                                else
                                {
                                    if (GameManager.Instance.SequenceOfClick.ClickOnTime >= GameManager.Instance.timerInt)
                                    {
                                        properTimed = 1;
                                    }
                                    else if (GameManager.Instance.SequenceOfClick.ClickOnTime < GameManager.Instance.timerInt)
                                    {
                                        properTimed = 0;
                                    }
                                }
                            }
                        }
                    }

                    if (properTimed + properClicked == (byte)2)
                    {
                        
                        if (question.CountSequenceIgnoreValue)
                        {
                            GameManager.Instance.AnsweredCorrectly();
                        }
                        else
                        {
                            if (GameManager.Instance.SequenceOfClick.SequenceNumber == GameManager.Instance.MaxSequence)
                            {
                                GameManager.Instance.AnsweredCorrectly();
                            }
                            else
                            {
                                GameManager.Instance.IncreaseSequenceNumber();
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("properTimed = " + properTimed);
                        Debug.Log("properClicked = " + properClicked);

                        Debug.Log("Sequence Wrong");
                        GameManager.Instance.AnsweredWrongly(); 
                    }
                }
            }
        }
    }
}
