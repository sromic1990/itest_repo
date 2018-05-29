using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "QuestionHolder", menuName = "QuestionHolder", order = 1)]
public class QuestionHolder : ScriptableObject
{
    public List<BaseQuestion> Questions;
    public List<BaseQuestion> UniqueQuestions;
}

[Serializable]
public class BaseSpriteHolder
{
    //Any text that will written if the sprite also has a text field.
    public string text;
    //The sprite ID of the sprite
    public SpriteID Sprite;
    //Any list of spriteIDs which may be exchanged with the primary spriteID
    public List<SpriteID> SecondarySprites;
    //Color of the Text if the sprite has a text field.
    public Color TextColor;
    
}

[Serializable]
public class AnswerSpriteHolder : BaseSpriteHolder
{
    //If Answer is correct or not.
    public bool IsCorrect;
    //ID of the answer.
    public AnswerID ID;
    //Sequence info of the option, if any.
    public List<SequenceOfClick> SequenceInfo;
}

[Serializable]
public class BaseQuestion
{
    //Actual Question Text
    public string Question;
    //Any secondary question text that must be shown
    public List<string> SecondaryQuestion;
    //Whether to ignore click count for answer for this question.
    public bool IgnoreClickCount;
    //Should time be ignored for input for this question.
    public bool IgnoreClickTime;
    //Does the question has sequence to click.
    public bool HasSequenceAnswer;
    //Count the sequence but ignore sequence number
    public bool CountSequenceIgnoreValue;
    //Does the question has any dependency on the ReturnValue_Bool
    public bool HasBoolValue;
    //Does Question has any timer value or not. Which means at the end of this timer, question will be auto complete, whether it is answered or not.
    public bool HasTimerValue;
    //Whether the question has any sequential questions or not?
    public bool HasSequentialQuestions;
    //How many times will this question repeat itself, ONLY for sequential questions.
    public int RepeatValue;
    //The pattern of the question.
    public QPattern Pattern;
    //Whether the question has base or no base. [REDUNDANT]
    public QuestionType QuestionType;
    //Time to answer the question.
    public float TimeForQuestion;
    //Whether the question has any intro, if yes, all details of the intros here.
    public List<IntroForQuestion> IntrosForQuestion;
    //Any int or float data that the question might possess.
    public List<float> QuestionData_Float;
    public List<int> QuestionData_Int;
    [HideInInspector]
    //Useful when UI sends control back to GameManager and stashes a true or false value according to which question may either be false or true.
	public bool ReturnValue_Bool;
    //Delay till which input will remain suspended after showing question
    public float DelayTimeAfterQuestionShow;
    //Any sprites that are there for the question
    public List<BaseSpriteHolder> QuestionSprite;
    //Answer properties
    public List<AnswerSpriteHolder> Options;
    //How much we must wait before showing the result
    public int WaitTimeAfterAnswer = 0;
    //Whether the question can be answered multiple times
    public bool AllowMultipleAnswers;
    public int sortOrder = -1;
    [Space(5)]
    [Header("Only for IF ELSE")]
    [Space(5)]
    //Data for IfElse Questions
    public IfElsePattern IfElseData;


    [Space(5)]
    [Header("Only for OBJECTS IN PREVIOUS")]
    [Space(5)]
    //Data for ObjectsInPrevious Questions
    public ObjectsInPreviousPattern ObjectsInPreviousData;

    /// <summary>
    /// Constructor
    /// </summary>
    public BaseQuestion()
    {
        SecondaryQuestion = new List<string>();
        QuestionData_Int = new List<int>();
        QuestionData_Float = new List<float>();
        QuestionSprite = new List<BaseSpriteHolder>();
        Options = new List<AnswerSpriteHolder>();
    }

    /// <summary>
    /// Upon given a sequence number, this method returns the sequence data.
    /// </summary>
    /// <returns>The current sequence.</returns>
    /// <param name="sequenceNumber">Sequence number.</param>
    public SequenceOfClick GetCurrentSequence(int sequenceNumber)
    {
        SequenceOfClick sequenceOfClick = null;
        bool sequenceFound = false;

        //Debug.Log("Options = "+Options.Count);

        for (int i = 0; i < Options.Count; i++)
        {
            for (int j = 0; j < Options[i].SequenceInfo.Count; j++)
            {
                if (Options[i].SequenceInfo[j].SequenceNumber == sequenceNumber)
                {
                    Debug.Log("Sequence Number = " + Options[i].SequenceInfo[j].RequiredClicks);
                    sequenceOfClick = Options[i].SequenceInfo[j];
                    sequenceFound = true;
                    break;
                }
            }

            if (sequenceFound)
                break;
        }

        //Debug.Log("sequenceFound = "+sequenceFound);

        return sequenceOfClick;
    }

    /// <summary>
    /// At anytime, this method will get the last sequence number from any answer's given sequence.
    /// </summary>
    /// <returns>The maximum sequence.</returns>
    public int GetMaximumSequence()
    {
        int maxSeq = 0;
        for (int i = 0; i < Options.Count; i++)
        {
            if (Options[i].SequenceInfo == null)
                break;
            for (int j = 0; j < Options[i].SequenceInfo.Count; j++)
            {
                if (maxSeq < Options[i].SequenceInfo[j].SequenceNumber)
                {
                    maxSeq = Options[i].SequenceInfo[j].SequenceNumber;
                }
            }
            
        }
        return maxSeq;
    }

    /// <summary>
    /// This method gets the maximum float value from any float data that we have in the question.
    /// </summary>
    /// <returns>The maximum float data.</returns>
    public float GetMaximumFloatData()
    {
        float maxFloat = 0.0f;

        for (int i = 0; i < QuestionData_Float.Count; i++)
        {
            maxFloat = (maxFloat < QuestionData_Float[i]) ? QuestionData_Float[i] : maxFloat;
        }

        return maxFloat;
    }

    /// <summary>
    /// This method gets the maximum int value from any int data that we have in the question.
    /// </summary>
    /// <returns>The maximum int data.</returns>
    public int GetMaximumIntData()
    {
        int maxInt = 0;

        for (int i = 0; i < QuestionData_Int.Count; i++)
        {
            maxInt = (maxInt < QuestionData_Int[i]) ? QuestionData_Int[i] : maxInt;
        }

        return maxInt;
    }
}

[Serializable]
public class SequenceOfClick
{
    //What is the sequence of the option? Starts from 1.
    public int SequenceNumber;
    //If Number of clicks in sequence matters for answering.
    public int RequiredClicks;
    //If the option needs to be clicked on a certain time.
    public int ClickOnTime;
}

[Serializable]
public class IntroForQuestion
{
    //What will be the intro text to the question
    public string IntroText;
    //If the intro has timer value
    public bool HasTimer;
    //Timer value for the intro
    public float TimerValue;
    //Whether to hide ok button or not
    public bool HideOkButton;
}

[Serializable]
public class IfElsePattern
{
    //Whether the IfElse Question is of 'First' or 'Last' type
    public IfElseAppear QuestionAppear;
    //Which is the correct option if first is shown according to QuestionAppearOrder
    public OptionOrder IfFirst;
    //Which is the correct option if second is shown according to QuestionAppearOrder
    public OptionOrder IfSecond;
}

[Serializable]
public struct ObjectsInPreviousPattern
{
    //Whether the object to touch was "IN" or "NOT IN" the previous section.
    public ObjectsInPrevious Mode;
}