using System.Collections;
using System.Collections.Generic;
using Sourav.Utilities.Scripts.Enums;
using UnityEngine;
using Sourav.Utilities.Scripts.Algorithms.Shuffle;

public class SpeedTouchSetter : MonoBehaviour
{
    public bool IsSpeedTouchSet = false;

    private List<SpeedTouch> SpeedTouchList;
    private int SpeedTouchParameters;
    private List<SpeedTouchAnswerInfo> AnswerInfos;
    private AnswerID CurrentColorID;

    #region Mono Methods

    private void Awake()
    {
        SpeedTouchParameters = EnumUtil.GetValues<SpeedTouch>().Count;

        Reset();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the speed touch question parameters. Call it before displaying speed touch questions.
    /// </summary>
    /// <param name="numberOfQuestions">Number of questions.</param>
    public void SetSpeedTouch(int numberOfQuestions)
    {
        SpeedTouchList.Clear();

        int previousQuestion = -1;

        for (int i = 0; i < numberOfQuestions; i++)
        {
            //Setting Speed touch data
            int question = (int)Random.Range(0, SpeedTouchParameters);

            //checking if the current question is same as the previous question
            if (previousQuestion != -1)
            {
                if (question == previousQuestion)
                {
                    question = (question == SpeedTouchParameters - 1) ? 0 : previousQuestion + 1;
                }   
            }

            SpeedTouchList.Add((SpeedTouch)question);
            previousQuestion = question;

        }

        GameManager.Instance.IsSequentialQuestionSet = true;
        GameManager.Instance.CurrentSequentialQuestionIndex = 0;
    }

    /// <summary>
    /// Gets the current Speed touch question.
    /// </summary>
    /// <returns>The UIQ uestion.</returns>
    /// <param name="questionIndex">Question index.</param>
    /// <param name="answerButtons">Answer buttons.</param>
    public BaseQuestion GetUIQuestion(int questionIndex, int answerButtons = 4)
    {
        SpeedTouch questionType = SpeedTouchList[questionIndex];

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
        bq.Pattern = QPattern.SpeedTouch;
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
            SpeedTouchAnswerInfo answerInfo = GetNewAnswerButton();
            SetAnswer(ref answerInfo, (ColorEnum)colorIndex[i], (ColorEnum)randomColorIndex);
            AnswerInfos.Add(answerInfo);
        }

        //Assignment to answer buttons
        SetAnswerOption(ref bq, AnswerInfos, (ColorEnum)randomColorIndex, questionType);

        return bq;
    }

    #endregion

    #region Setter Methods

    /// <summary>
    /// Sets answerInfo according to supplied color. If color is the random color, this sets the answerID to the supplied answerInfo AnswerID
    /// </summary>
    /// <param name="answerInfo">Answer info.</param>
    /// <param name="color">Color.</param>
    /// <param name="randomColor">Random color.</param>
    private void SetAnswer(ref SpeedTouchAnswerInfo answerInfo, ColorEnum color, ColorEnum randomColor)
    {
        switch (color)
        {
            case ColorEnum.Red:
                answerInfo.SpriteID = SpriteID.Balloon_Red;
                answerInfo.AnswerID = AnswerID.Balloon_Red;
                break;

            case ColorEnum.Blue:
                answerInfo.SpriteID = SpriteID.Balloon_Blue;
                answerInfo.AnswerID = AnswerID.Balloon_Blue;
                break;

            case ColorEnum.Yellow:
                answerInfo.SpriteID = SpriteID.Balloon_Yellow;
                answerInfo.AnswerID = AnswerID.Balloon_Yellow;
                break;

            case ColorEnum.Green:
                answerInfo.SpriteID = SpriteID.Balloon_Green;
                answerInfo.AnswerID = AnswerID.Balloon_Green;
                break;
        }

        if (color == randomColor)
        {
            CurrentColorID = answerInfo.AnswerID;
        }
    }

    /// <summary>
    /// Main method to set the question property and answer button property for each answer button
    /// </summary>
    /// <param name="question">Question.</param>
    /// <param name="answer">Answer.</param>
    /// <param name="randomColor">Random color.</param>
    /// <param name="questionType">Question type.</param>
    private void SetAnswerOption(ref BaseQuestion question, List<SpeedTouchAnswerInfo> answer, ColorEnum randomColor, SpeedTouch questionType)
    {
        ColorEnum color = GetColorBasedOnAnswerOrSpriteID(IDType.AnswerID, CurrentColorID);
        switch (questionType)
        {
            case SpeedTouch.OneX:
                question.Question = "Touch the " + color.ToString() + " balloon.";

                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].IsCorrect = (CurrentColorID == answer[i].AnswerID) ? true : false;
                    question.Options[i].ID = answer[i].AnswerID;
                    question.Options[i].Sprite = answer[i].SpriteID;
                }
                break;

            case SpeedTouch.ExceptX:
                question.Question = "Touch any balloon except the " + color.ToString() + " balloon.";
                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].IsCorrect = (CurrentColorID == answer[i].AnswerID) ? false : true;
                    question.Options[i].ID = answer[i].AnswerID;
                    question.Options[i].Sprite = answer[i].SpriteID;
                }
                break;

            case SpeedTouch.AnyX:
                question.Question = "Touch any " + color.ToString() + " balloon.";
                int secondColorPos = -1;
                int firstColorPos = -1;

                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].ID = answer[i].AnswerID;
                    question.Options[i].Sprite = answer[i].SpriteID;
                    Debug.Log("Answer Image = " + question.Options[i].Sprite.ToString());

                    if (answer[i].AnswerID == CurrentColorID)
                    {
                        firstColorPos = i;
                    }

                    question.Options[i].IsCorrect = (CurrentColorID == answer[i].AnswerID) ? true : false;
                }

                secondColorPos = Random.Range(0, question.Options.Count);
                while (secondColorPos == firstColorPos)
                {
                    secondColorPos = Random.Range(0, question.Options.Count);
                }

                question.Options[secondColorPos].ID = CurrentColorID;
                question.Options[secondColorPos].Sprite = GetSpriteIDBasedOnAnswerID(CurrentColorID);
                question.Options[secondColorPos].IsCorrect = true;

                break;

            case SpeedTouch.LeftX:
                question.Question = "Touch the balloon on the left of the " + color.ToString() + " balloon.";
                int randomLeftx = Random.Range(0, 10);

                randomLeftx = (randomLeftx % 2 == 0) ? 1 : 3;
                Debug.Log("randomLeftx = " + randomLeftx);
                CheckAndRearrangeAnswers(answer, randomLeftx);

                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].ID = answer[i].AnswerID;
                    question.Options[i].Sprite = answer[i].SpriteID;
                    question.Options[i].IsCorrect = (i + 1 == randomLeftx) ? true : false;
                }
                break;

            case SpeedTouch.RightX:
                question.Question = "Touch the balloon on the right of the " + color.ToString() + " balloon.";
                int randomRightX = Random.Range(0, 2);

                randomRightX = (randomRightX == 1) ? 0 : 2;
                CheckAndRearrangeAnswers(answer, randomRightX);

                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].ID = answer[i].AnswerID;
                    question.Options[i].Sprite = answer[i].SpriteID;
                    question.Options[i].IsCorrect = (i - 1 == randomRightX) ? true : false;
                }
                break;

            case SpeedTouch.AboveX:
                question.Question = "Touch the balloon above the " + color.ToString() + " balloon.";
                int randomAboveX = Random.Range(0, 2);

                randomAboveX = (randomAboveX == 1) ? 2 : 3;
                CheckAndRearrangeAnswers(answer, randomAboveX);

                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].ID = answer[i].AnswerID;
                    question.Options[i].Sprite = answer[i].SpriteID;
                    question.Options[i].IsCorrect = (i + 2 == randomAboveX) ? true : false;
                    Debug.Log("Answer Image = " + question.Options[i].Sprite.ToString());
                }
                break;

            case SpeedTouch.BelowX:
                question.Question = "Touch the balloon below the " + color.ToString() + " balloon.";
                int randomBelowX = Random.Range(0, 2);

                randomBelowX = (randomBelowX == 1) ? 0 : 1;
                CheckAndRearrangeAnswers(answer, randomBelowX);

                for (int i = 0; i < question.Options.Count; i++)
                {
                    question.Options[i].ID = answer[i].AnswerID;
                    question.Options[i].Sprite = answer[i].SpriteID;
                    question.Options[i].IsCorrect = (i - 2 == randomBelowX) ? true : false;
                    Debug.Log("Answer Image = " + question.Options[i].Sprite.ToString());
                }
                break;
        }

    }

    private void CheckAndRearrangeAnswers(List<SpeedTouchAnswerInfo> answer, int randomPos)
    {
        //Search what is the position of the currentColorId and set it at the correct position
        int index = -1;
        for (int i = 0; i < answer.Count; i++)
        {
            if (CurrentColorID == answer[i].AnswerID && i != randomPos)
            {
                index = i;
            }
        }
        if (index != -1)
        {
            //Debug.Log("index = "+index);
            //return;
            SpeedTouchAnswerInfo answerInfo = answer[index];
            answer[index] = answer[randomPos];
            answer[randomPos] = answerInfo;
        }
    }

    #endregion

    #region Reset Method

    /// <summary>
    /// Resets SpeedTouchList, AnswerInfos, GameManager IsSequentialQuestionSet and Sets GameManager CurrentSequentialQuestionIndex to -1
    /// </summary>
    private void Reset()
    {
        SpeedTouchList = new List<SpeedTouch>();
        AnswerInfos = new List<SpeedTouchAnswerInfo>();
        GameManager.Instance.IsSequentialQuestionSet = false;
        GameManager.Instance.CurrentSequentialQuestionIndex = -1;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets the sprite ID based on answer ID.
    /// </summary>
    /// <returns>The sprite IDB ased on answer identifier.</returns>
    /// <param name="answerID">Answer identifier.</param>
    private SpriteID GetSpriteIDBasedOnAnswerID(AnswerID answerID)
    {
        switch (answerID)
        {
            case AnswerID.Balloon_Blue:
                return SpriteID.Balloon_Blue;

            case AnswerID.Balloon_Green:
                return SpriteID.Balloon_Green;

            case AnswerID.Balloon_Red:
                return SpriteID.Balloon_Red;

            default:
                return SpriteID.Balloon_Yellow;
        }
    }

    /// <summary>
    /// Gets the answer ID based on sprite ID.
    /// </summary>
    /// <returns>The answer IDB ased on sprite identifier.</returns>
    /// <param name="spriteID">Sprite identifier.</param>
    private AnswerID GetAnswerIDBasedOnSpriteID(SpriteID spriteID)
    {
        switch (spriteID)
        {
            case SpriteID.Balloon_Blue:
                return AnswerID.Balloon_Blue;

            case SpriteID.Balloon_Green:
                return AnswerID.Balloon_Green;

            case SpriteID.Balloon_Red:
                return AnswerID.Balloon_Red;

            default:
                return AnswerID.Balloon_Yellow;
        }
    }

    /// <summary>
    /// Gets the color based on answer or sprite ID.
    /// </summary>
    /// <returns>The color based on answer or sprite identifier.</returns>
    /// <param name="iD">I d.</param>
    /// <param name="answerID">Answer identifier.</param>
    /// <param name="spriteID">Sprite identifier.</param>
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

    /// <summary>
    /// Creates an instance of SpeedTouchInfo
    /// </summary>
    /// <returns>The new answer button.</returns>
    private SpeedTouchAnswerInfo GetNewAnswerButton()
    {
        return new SpeedTouchAnswerInfo(SpriteID.DummySprite, false, AnswerID.None);
    }

    #endregion
}

public struct SpeedTouchAnswerInfo
{
    public SpriteID SpriteID;
    public bool IsCorrect;
    public AnswerID AnswerID;

    public SpeedTouchAnswerInfo(SpriteID SpriteID, bool IsCorrect, AnswerID AnswerID)
    {
        this.SpriteID = SpriteID;
        this.IsCorrect = IsCorrect;
        this.AnswerID = AnswerID;
    }
}
