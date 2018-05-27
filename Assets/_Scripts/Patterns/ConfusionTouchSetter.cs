using System;
using System.Collections.Generic;
using Sourav.Utilities.Scripts.Enums;
using UnityEngine;
using Random = UnityEngine.Random;
using Sourav.Utilities.Scripts.Algorithms.Shuffle;
using Sourav.Utilities.Scripts;

public class ConfusionTouchSetter : MonoBehaviour
{
    public List<ColorInfo> Colors;

    private List<ConfusionTouch> ConfusionTouchList;
    private int ConfusionTouchParameters;
    private List<ConfusionTouchAnswerInfo> AnswerInfos;
    private AnswerID CurrentColorID;

    private CircularRandomAccessList<ColorEnum> CircularColorsList;
    private List<ColorEnum> ColorsList;

    #region Mono Methods

    void Awake()
    {
        ConfusionTouchParameters = EnumUtil.GetValues<ConfusionTouch>().Count;
        ColorsList = new List<ColorEnum> { ColorEnum.Red, ColorEnum.Blue, ColorEnum.Yellow, ColorEnum.Green, ColorEnum.Red, ColorEnum.Blue, ColorEnum.Yellow, ColorEnum.Green, ColorEnum.Red, ColorEnum.Blue, ColorEnum.Yellow, ColorEnum.Green };
        Reset();
    }

    #endregion

    #region Public Methods

    public void SetConfusionTouch(int numberOfQuestions)
    {
        ConfusionTouchList.Clear();

        int previousQuestion = -1;

        for (int i = 0; i < numberOfQuestions; i++)
        {
            //Setting Speed touch data
            int question = (int)Random.Range(0, ConfusionTouchParameters);

            //checking if the current question is same as the previous question
            if (previousQuestion != -1)
            {
                if (question == previousQuestion)
                {
                    question = (question == ConfusionTouchParameters - 1) ? 0 : previousQuestion + 1;
                }
            }

            ConfusionTouchList.Add((ConfusionTouch)question);
            previousQuestion = question;

        }

        GameManager.Instance.IsSequentialQuestionSet = true;
        GameManager.Instance.CurrentSequentialQuestionIndex = 0;
    }

    public BaseQuestion GetUIQuestion(int questionIndex, int answerButtons = 4)
    {
        ConfusionTouch questionType = ConfusionTouchList[questionIndex];

        AnswerInfos.Clear();

        CurrentColorID = AnswerID.None;

        int colors = EnumUtil.GetValues<ColorEnum>().Count;

        //Initializing a Question
        BaseQuestion bq = new BaseQuestion();
        bq.IgnoreClickCount = true;
        bq.IgnoreClickTime = true;
        bq.HasSequenceAnswer = false;
        bq.HasBoolValue = false;
        bq.HasTimerValue = true;
        bq.HasSequentialQuestions = true;
        bq.Pattern = QPattern.ConfusionTouch;
        bq.QuestionType = QuestionType.Base;
        bq.TimeForQuestion = 0;
        bq.Options = new List<AnswerSpriteHolder>();
        for (int i = 0; i < answerButtons; i++)
        {
            AnswerSpriteHolder answer = new AnswerSpriteHolder();
            bq.Options.Add(answer);
        }
        //

        //Getting a random color for the question
        int randomColorIndex = (int)Random.Range(0, colors);

        //Shuffle
        FisherYatesShuffle shuffle = new FisherYatesShuffle(colors);
        shuffle.ShuffleList();
        List<int> colorIndex = shuffle.ShuffledList;

        //Setting up button info for later assignment and setting up current answerID Color for the current question
        for (int i = 0; i < answerButtons; i++)
        {
            ConfusionTouchAnswerInfo answerInfo = GetNewAnswerButton();
            AnswerInfos.Add(answerInfo);
        }

        SetAnswerForQuestion();
        SetAnswerID((ColorEnum)randomColorIndex);

        //Assignment to answer buttons
        SetAnswerOption(ref bq, (ColorEnum)randomColorIndex, questionType);

        return bq;
    }

    #endregion

    #region Private Methods

    private ConfusionTouchAnswerInfo GetNewAnswerButton()
    {
        return new ConfusionTouchAnswerInfo(SpriteID.DummySprite, false, AnswerID.None, ColorEnum.Blue, ColorEnum.Blue, ColorEnum.Blue);
    }

    private void SetAnswerForQuestion()
    {
        if (AnswerInfos.Count <= 0)
            return;

        int randomAccessPoint = Random.Range(0, ColorsList.Count);

        CircularColorsList = new CircularRandomAccessList<ColorEnum>(randomAccessPoint, ColorsList);

        for (int i = 0; i < AnswerInfos.Count; i++)
        {
            AnswerInfos[i].BalloonColor = CircularColorsList.GetNextIndex();
            AnswerInfos[i].TextInBalloon = CircularColorsList.GetNextIndex();
            AnswerInfos[i].TextColor = CircularColorsList.GetNextIndex();

            AnswerInfos[i].SpriteID = SetSpriteID(AnswerInfos[i]);
        }
    }

    #region Helper Methods for SetAnswerForQuestion

    private SpriteID SetSpriteID(ConfusionTouchAnswerInfo answerInfo)
    {
        switch (answerInfo.BalloonColor)
        {
            case ColorEnum.Blue:
                return SpriteID.Balloon_Blue;

            case ColorEnum.Green:
                return SpriteID.Balloon_Green;

            case ColorEnum.Red:
                return SpriteID.Balloon_Red;

            default: //Yellow
                return SpriteID.Balloon_Yellow;
        }
    }

    #endregion

    private void SetAnswerID(ColorEnum randomColor)
    {
        for (int i = 0; i < AnswerInfos.Count; i++)
        {
            ColorEnum color = ColorEnum.Blue;
            switch (AnswerInfos[i].SpriteID)
            {
                case SpriteID.Balloon_Red:
                    AnswerInfos[i].AnswerID = AnswerID.Balloon_Red;
                    color = ColorEnum.Red;
                    break;

                case SpriteID.Balloon_Blue:
                    AnswerInfos[i].AnswerID = AnswerID.Balloon_Blue;
                    color = ColorEnum.Blue;
                    break;

                case SpriteID.Balloon_Green:
                    AnswerInfos[i].AnswerID = AnswerID.Balloon_Green;
                    color = ColorEnum.Green;
                    break;

                case SpriteID.Balloon_Yellow:
                    AnswerInfos[i].AnswerID = AnswerID.Balloon_Yellow;
                    color = ColorEnum.Yellow;
                    break;
            }
            if (color == randomColor)
            {
                CurrentColorID = AnswerInfos[i].AnswerID;
            }
        }
        
    }

    private void PrintListContent(List<ColorEnum> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log("At " + i + " is " + list[i].ToString());
        }
    }

    private void SetAnswerOption(ref BaseQuestion question, ColorEnum randomColor, ConfusionTouch questionType)
    {
        ColorEnum color = GetColorBasedOnAnswerOrSpriteID(IDType.AnswerID, CurrentColorID);
        switch (questionType)
        {
            case ConfusionTouch.ColorX:
                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].Sprite = AnswerInfos[i].SpriteID;
                    question.Options[i].ID = AnswerInfos[i].AnswerID;
                    question.Options[i].text = AnswerInfos[i].TextInBalloon.ToString();
                    question.Options[i].TextColor = GetColorBasedOnColorEnum(AnswerInfos[i].TextColor);
                    if (CurrentColorID == AnswerInfos[i].AnswerID)
                    {
                        question.Options[i].IsCorrect = true;
                        question.Question = "Touch the " + color.ToString() + " balloon.";
                    }
                    else
                    {
                        question.Options[i].IsCorrect = false;
                    }
                }
                break;
            
            case ConfusionTouch.WordColorX:
                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].Sprite = AnswerInfos[i].SpriteID;
                    question.Options[i].ID = AnswerInfos[i].AnswerID;
                    question.Options[i].text = AnswerInfos[i].TextInBalloon.ToString();
                    question.Options[i].TextColor = GetColorBasedOnColorEnum(AnswerInfos[i].TextColor);
                    if (CurrentColorID == AnswerInfos[i].AnswerID)
                    {
                        question.Options[i].IsCorrect = true;
                        question.Question = "Touch the " + AnswerInfos[i].TextColor.ToString() + " word.";
                    }
                    else
                    {
                        question.Options[i].IsCorrect = false;
                    }
                }
                break;
            
            case ConfusionTouch.WordX:
                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].Sprite = AnswerInfos[i].SpriteID;
                    question.Options[i].ID = AnswerInfos[i].AnswerID;
                    question.Options[i].text = AnswerInfos[i].TextInBalloon.ToString();
                    question.Options[i].TextColor = GetColorBasedOnColorEnum(AnswerInfos[i].TextColor);
                    if (CurrentColorID == AnswerInfos[i].AnswerID)
                    {
                        question.Options[i].IsCorrect = true;
                        question.Question = "Touch the word " + question.Options[i].text + ".";
                    }
                    else
                    {
                        question.Options[i].IsCorrect = false;
                    }
                }
                break;
        }

    }

    #region Helper Methods for SetAnswerOption

    private Color GetColorBasedOnColorEnum(ColorEnum color)
    {
        Color col = Color.blue;

        for (int i = 0; i < Colors.Count; i++)
        {
            if (color == Colors[i].color)
            {
                col = Colors[i].colorValue;
                break;
            }
        }
        return col;
    }

    private ColorEnum GetColorBasedOnAnswerOrSpriteID(IDType iD, AnswerID answerID = AnswerID.None, SpriteID spriteID = SpriteID.DummySprite)
    {
        switch (iD)
        {
            case IDType.AnswerID:

                switch (answerID) //AnswerID
                {
                    case AnswerID.Balloon_Blue:
                        return ColorEnum.Blue;

                    case AnswerID.Balloon_Red:
                        return ColorEnum.Red;

                    case AnswerID.Balloon_Green:
                        return ColorEnum.Green;

                    default:
                        return ColorEnum.Yellow;
                }

            default: //SpriteID

                switch (spriteID)
                {
                    case SpriteID.Balloon_Blue:
                        return ColorEnum.Blue;

                    case SpriteID.Balloon_Red:
                        return ColorEnum.Red;

                    case SpriteID.Balloon_Green:
                        return ColorEnum.Green;

                    default:
                        return ColorEnum.Yellow;
                }
        }
    }

    #endregion

    #endregion

    #region Reset Method

    private void Reset()
    {
        ConfusionTouchList = new List<ConfusionTouch>();
        AnswerInfos = new List<ConfusionTouchAnswerInfo>();
        GameManager.Instance.IsSequentialQuestionSet = false;
        GameManager.Instance.CurrentSequentialQuestionIndex = -1;
    }

    #endregion
}

[Serializable]
public struct ColorInfo
{
    public string colorString;
    public ColorEnum color;
    public Color colorValue;
}

[Serializable]
public class ConfusionTouchAnswerInfo
{
    public SpriteID SpriteID;
    public bool IsCorrect;
    public AnswerID AnswerID;
    public ColorEnum BalloonColor;
    public ColorEnum TextInBalloon;
    public ColorEnum TextColor;

    public ConfusionTouchAnswerInfo(SpriteID SpriteID, bool IsCorrect, AnswerID AnswerID, ColorEnum BalloonColor, ColorEnum TextInBalloon, ColorEnum TextColor)
    {
        this.SpriteID = SpriteID;
        this.IsCorrect = IsCorrect;
        this.AnswerID = AnswerID;
        this.BalloonColor = BalloonColor;
        this.TextInBalloon = TextInBalloon;
        this.TextColor = TextColor;
    }
}

[Serializable]
public class BalloonInfo
{
    public SpriteID Sprite;
    public ColorEnum Text;
    public ColorEnum TextColor;
}


