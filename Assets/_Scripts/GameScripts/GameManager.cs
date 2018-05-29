using System.Collections.Generic;
using UnityEngine;
using Scripts.Utilities;
using System;
using Sourav.Utilities.Scripts.Enums;
using IdiotTest.Scripts.GameScripts;
using Sourav.Utilities.Scripts.Algorithms.Shuffle;
using Random = UnityEngine.Random;
using Sourav.Utilities.Scripts;

public class GameManager : Singleton<GameManager>
{
    #region Fields

    #region Misc

    [Space(10)]
    public QuestionHolder Questions;
    public QuestionHolder MultiplayerQuestions;
    public List<SetterHolder> SettersAndEvaluators;
    public AnswerID LastSequenceAnswer;
    GameObject Setter;

    #endregion

    #region Data For Question Transition and Level

    [Space(10)]
    [Header("Data For Question Transition and Level")]
    public int LevelIndex;
    public int WaitTimeAfterQuestionAnswered;
    public int SkipTextAttempt;
    public int WarningTextAttempt;
    public int ResetAttempt;
    public int LivesEarnedPerLevel;
    public int TotalQuestions;
    public int MinTimeForQuestion;
    public int ScorePerQuestion;
    public int ScoreDuductPerQuestion_Regular;
    public int ScorePerQuestionMultiplayer;
    public int ScoreForLastQuestionMultiplayer;
    public int DecreaseScorePerAttempt;
    public int MultiplayerQuestionsPerRound;
    public int MultiplayerTotalTime;
    public int MultiplayerWaitTimeFor2ndPlayer;
    public int MultiplayerMaxWaitTimeForResult;
    public int MultiplayerQuestionMultiplier;
    public int MultiplayerLastQuestionMultiplier;
    public int RewardForReview;
    [Space(10)]
    #endregion

    #region Test Specific
    [Header("Test")]
    public TestQuestion TestingQuestion;

    #endregion

    #region Question specific fields

    [SerializeField]
    private BaseQuestion CurrentQuestion;
    [SerializeField]
    private List<BaseQuestion> SequentialQuestions;

    #endregion

    #region Evaluator

    public Evaluator Evaluator;

    #endregion

    #region Non-Timer Action Specific

    public Action ResetAllData;

    public Action PauseAction;
    public Action ResumeAction;

    public Action<int, int> Clicked;

    #endregion

    #region Max And Sequence Specific

    public int nextAnswerSequence = 1;
    public SequenceOfClick SequenceOfClick;
    public int MaxSequence = 0;
    public float MaxFloat = 0.0f;
    public int MaxInt = 0;
    public List<int> SequenceCount;

    #endregion

    #region Input Related

    [SerializeField]
    public bool CanProcessInput = false;

    #endregion

    #region Timer Specific

    //Question specific timer
    public int timerInt = 0;
    public Action<int> TimeTicker;

    //Multiplayer timer
    public int timerMultiplayer = 60;
    public Action<int> MultiplayerTimeTicker;

    #endregion

    #region Confusion And Speed Touch Specific

    public SpeedTouchSetter SpeedTouchSetter;
    public ConfusionTouchSetter ConfusionTouchSetter;

    #endregion

    #region Question Ordering Specific

    public List<int> QuestionIndex;
    public int CurrentQuestionIndex;

    #endregion

    #region Need to reset when a next question is generated

    public int CurrentSequentialQuestionIndex;
    public bool IsSequentialQuestionSet;
    public int CurrentIntroScreenIndex;
    private int answeredTimer;
    public bool Answered;
    private int questionTimer;
    private AnswerResult Result;

    #endregion

    #region Save after question is answered

    [SerializeField]
    private int currentQuestion;
    [SerializeField]
    private int levelProgress;

    #endregion

    #region Game Status Specific

    [SerializeField]
    private GameStatus _status;

    public GameStatus Status
    {
        get { return _status; }
        set
        {
            _status = value;
            if (_status == GameStatus.OutOfSession)
            {
                UIManager.Instance.OnGameOutOfSession();
            }
            else if (_status == GameStatus.InSession)
            {
                ScreenManager.Instance.ShowQuestionPanel();
                Resume();
            }
            else if (_status == GameStatus.IntroSession)
            {
                UIManager.Instance.HideAllQuestionPanels();
                Resume();
            }
            else
            {
                ScreenManager.Instance.HideQuestionPanel();
            }

        }
    }

    public bool ToStoreFromGameplayDueToLackOfFunds = false;
    public ApplicationResumeFrom ResumeFrom;

    #endregion

    #region Reset after every multiplayer game

    public string code = "";
    public int MultiplayerQuestionIndex;
    public int CurrentMultiplayerQuestion;
    public int TimeSinceLastQuestionAnswered = 60;
    public int LastQuestionAnsweredTime;
    public bool IsOpponentGameOver;
    public bool IsMyGameOver;
    public bool IsGameStarted;
    public int CurrentWaitingTimeForMultiplayer;
    public bool IsWaiting = false;
    private int waitTime;
    private bool _aloneInRoom;

    public bool AmIAloneInRoom
    {
        get { return _aloneInRoom; }
        set
        {
            _aloneInRoom = value;
            UIManager.Instance.RoomOccupancyChanged(_aloneInRoom);
        }
    }

    private int _currentBetAmount;

    public int CurrentBetAmount
    {
        get { return (int)(GameDataManager.Instance.BetAmount); }
        set
        {
            _currentBetAmount = value;
        }
    }

    private int _multiplayerScore;

    public int MultiplayerScore
    {
        get { return _multiplayerScore; }
        set
        {
            _multiplayerScore = value;
            if (_multiplayerScore < 0)
            {
                _multiplayerScore = 0;
            }
            MultiplayerManager.Instance.UpdateScore(_multiplayerScore);
            UIManager.Instance.Multiplayer_ScoreChanged(_multiplayerScore);
        }
    }

    private int _multiplayerOpponentScore;

    public int MultiplayerOpponentScore
    {
        get { return _multiplayerOpponentScore; }
        set
        {
            _multiplayerOpponentScore = value;
            if (_multiplayerOpponentScore < 0)
            {
                _multiplayerOpponentScore = 0;
            }
            UIManager.Instance.Multiplayer_OpponentScoreChanged(_multiplayerOpponentScore);
        }
    }

    private MultiplayerSecondPlayerJoinStatus _multiplayerSeondPlayerJoinStatus;

    public MultiplayerSecondPlayerJoinStatus SecondPlayerJoinStatus
    {
        get { return _multiplayerSeondPlayerJoinStatus; }
        set
        {
            _multiplayerSeondPlayerJoinStatus = value;
        }
    }

    public MultiplayerGameplayType CurrentMultiplayerGameplay;
    public bool ActivelyLookingForOpponent;

    #endregion

    #endregion

    #region Methods

    #region Mono Methods

    private void Start()
    {
        SequentialQuestions = new List<BaseQuestion>();
        SequenceCount = new List<int>();
        GameDataManager.Instance.CurrentGameMode = GameMode.SinglePlayer;
        ResumeFrom = ApplicationResumeFrom.None;
        //OptionsOriginal = new List<AnswerSpriteHolder>();

#if UNITY_ANDROID && !UNITY_EDITOR || UNITY_IOS && !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif

        InvokeRepeating("Tick", 0.1f, 1.0f);
        InvokeRepeating("MultiplayerTick", 0.1f, 1.0f);
    }

    private void OnValidate()
    {
        for (int i = 0; i < SettersAndEvaluators.Count; i++)
        {
            SettersAndEvaluators[i].SetterPattern = SettersAndEvaluators[i].Pattern.ToString();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            switch (ResumeFrom)
            {
                case ApplicationResumeFrom.ReviewApp:
                    if (!GameDataManager.Instance.IsRewardFromReviewReceived)
                    {
                        AddDeductCurrency(Currency.Life, AddDeductAction.Add, RewardForReview);
                        GameDataManager.Instance.IsRewardFromReviewReceived = true;
                    }
                    break;

                case ApplicationResumeFrom.ShareMultiplayer:
                    CreateRoomForChallenge();
                    break;

                case ApplicationResumeFrom.ShareRegular:
                    break;
            }

            ResumeFrom = ApplicationResumeFrom.None;
        }
    }

    #endregion

    #region Setter Specific

    public void GetCorrectSetterAndEvaluator(QPattern pattern)
    {
        for (int i = 0; i < SettersAndEvaluators.Count; i++)
        {
            if (SettersAndEvaluators[i].Pattern == pattern)
            {
                Setter = SettersAndEvaluators[i].Setter;
            }
        }
    }

    #endregion

    #region Timer Specific

    //Question Specific Timer
    public void ResetTimer()
    {
        if (IsSequentialQuestionSet)
        {
            if (CurrentSequentialQuestionIndex > 0)
            {
                if (CurrentQuestion.RepeatValue > CurrentSequentialQuestionIndex)
                {
                    return;
                }
            }
        }

        timerInt = 0;
        UIManager.Instance.ResetTimer();
    }

    private void Tick()
    {
        timerInt++;
        AnnounceTime(timerInt);
    }

    private void AnnounceTime(int second)
    {
        //Debug.Log("time = "+second);
        if (TimeTicker != null)
        {
            TimeTicker.Invoke(second);
        }

        if (Answered && !IsSequentialQuestionSet)
        {
            if (answeredTimer < second)
            {
                Answered = false;
                Debug.Log("answeredTimer > second");
                ShowAnswerResult();
            }
        }
        else
        {
            if (CurrentQuestion.HasTimerValue)
            {
                //if (questionTimer > 0 && second > CurrentQuestion.TimeForQuestion)
                //{
                //    if (questionTimer > second && !IsSequentialQuestionSet)
                //    {
                //        Debug.Log("questionTimer > second");
                //        ShowAnswerResult(); 
                //    }
                //}
                if (questionTimer > 0)
                {
                    if (second > questionTimer)
                    {
                        ShowAnswerResult();
                    }
                }
            }
        }

        //MULTIPLAYER
        if (second > CurrentWaitingTimeForMultiplayer)
        {
            if (ActivelyLookingForOpponent)
            {
                InitiateAIPlay();
                ActivelyLookingForOpponent = false;
            }
        }

        if (second > waitTime && IsWaiting)
        {
            IsWaiting = false;
            UIManager.Instance.ShowPopUp("Sorry! No response from the other player", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);
            GetToMultiplayerPlayMenu();
        }
    }


    //Multiplayer Timer
    public void ResetMultiplayerTimer()
    {
        timerMultiplayer = MultiplayerTotalTime;
    }

    private void MultiplayerTick()
    {
        //Debug.Log();
        timerMultiplayer--;
        if (timerMultiplayer < 0)
        {
            timerMultiplayer = 0;
        }
        AnnounceMultiplayerTime(timerMultiplayer);
    }

    private void AnnounceMultiplayerTime(int second)
    {
        if (MultiplayerTimeTicker != null)
        {
            MultiplayerTimeTicker.Invoke(second);
        }
        if (second <= 0 && !IsMyGameOver && IsGameStarted)
        {
            IsMyGameOver = true;
            Debug.Log("From AnnounceMultiplayerTime");
            CheckAndEvaluateMultiplayer();
        }
    }

    #endregion

    #region Question Variation Specific

    private void SetQuestionVariation()
    {
        Debug.Log("<color=red>-----------------SetQuestionVariation-------------------</color>");
        switch (CurrentQuestion.Pattern)
        {
            #region BASE Patterns

            case QPattern.CatchMonkey:
                break;

            case QPattern.ConfusionTouch:
                Debug.Log("<color=red>ConfusionTouch</color>");
                if (!IsSequentialQuestionSet)
                {
                    int repeatValue = CurrentQuestion.RepeatValue;
                    ConfusionTouchSetter.SetConfusionTouch(repeatValue); // 10 in other words, as this is the number of questions this question will have.
                    SequentialQuestions.Clear();
                    for (int i = 0; i < repeatValue; i++)
                    {
                        //Debug.Log("RepatValue = " + repeatValue);
                        SequentialQuestions.Add(ConfusionTouchSetter.GetUIQuestion(i));
                    }
                }
                break;

            case QPattern.Homophone:
                break;

            case QPattern.IfElse:

                int random = Random.Range(0, 2);

                switch (CurrentQuestion.IfElseData.QuestionAppear)
                {
                    case IfElseAppear.First:
                        DetermineCorrectAnswerForIfElse(random);
                        if (random == 0) // First is coming first, then second and then third
                        {
                            //Do Nothing.
                            CurrentQuestion.QuestionData_Int = new List<int>();
                            CurrentQuestion.QuestionData_Int.Add(0);
                            CurrentQuestion.QuestionData_Int.Add(1);
                            CurrentQuestion.QuestionData_Int.Add(2);
                        }
                        else if (random == 1)
                        {
                            //Swap 0 and 1
                            CurrentQuestion.QuestionData_Int = new List<int>();
                            CurrentQuestion.QuestionData_Int.Add(1);
                            CurrentQuestion.QuestionData_Int.Add(0);
                            CurrentQuestion.QuestionData_Int.Add(2);
                        }
                        break;

                    case IfElseAppear.Last:
                        DetermineCorrectAnswerForIfElse(random);
                        if (random == 0)
                        {
                            //Swap 2 and 0
                            CurrentQuestion.QuestionData_Int = new List<int>();
                            CurrentQuestion.QuestionData_Int.Add(2);
                            CurrentQuestion.QuestionData_Int.Add(1);
                            CurrentQuestion.QuestionData_Int.Add(0);
                        }
                        else if (random == 1)
                        {
                            //Swap 1 and 2
                            CurrentQuestion.QuestionData_Int = new List<int>();
                            CurrentQuestion.QuestionData_Int.Add(0);
                            CurrentQuestion.QuestionData_Int.Add(2);
                            CurrentQuestion.QuestionData_Int.Add(1);
                        }
                        break;
                }
                break;

            case QPattern.PictureAnd4Answers:
                break;

            case QPattern.QuestionAnd4Answers:
                break;

            case QPattern.SlowFast:
                break;

            case QPattern.SmallestAndBiggest:
                break;

            case QPattern.SpeedTouch:
                if (!IsSequentialQuestionSet)
                {
                    int repeatValue = CurrentQuestion.RepeatValue;
                    SpeedTouchSetter.SetSpeedTouch(repeatValue); // 10 in other words, as this is the number of questions this question will have.
                    SequentialQuestions.Clear();
                    for (int i = 0; i < repeatValue; i++)
                    {
                        SequentialQuestions.Add(SpeedTouchSetter.GetUIQuestion(i));
                    }
                }
                break;

            case QPattern.TestYourSight:
                break;

            case QPattern.TextAnd4Answers:
                break;

            case QPattern.TouchAppearOrder3:
                break;

            case QPattern.TouchAppearOrder5:
                break;

            case QPattern.TouchFiveMen:
                List<TypeOfMan> Men = EnumUtil.GetValues<TypeOfMan>();
                FisherYatesShuffle FYShuffle = new FisherYatesShuffle(Men.Count);
                List<int> MenIndex = FYShuffle.ShuffledList;

                string QuestionText = "Quick! Memorize order : " + Men[MenIndex[0]] + " , " + Men[MenIndex[1]] + " , " + Men[MenIndex[2]] + " , " + Men[MenIndex[3]] + " , " + Men[MenIndex[4]] + ".";

                for (int i = 0; i < CurrentQuestion.Options.Count; i++)
                {
                    SetAnswerButtonForTypeOfMen(Men[MenIndex[i]], i, Men[MenIndex[i]]);

                    CurrentQuestion.Options[i].SequenceInfo = new List<SequenceOfClick>();
                    SequenceOfClick sequence = new SequenceOfClick();
                    CurrentQuestion.Options[i].SequenceInfo.Add(sequence);
                    CurrentQuestion.Options[i].SequenceInfo[0].SequenceNumber = i + 1;
                }

                IntroForQuestion intro0 = new IntroForQuestion();
                intro0.IntroText = "Next question requires fast reaction time. Get ready and click OK,";

                IntroForQuestion intro1 = new IntroForQuestion();
                intro1.IntroText = QuestionText;
                intro1.HasTimer = true;
                intro1.TimerValue = Mathf.RoundToInt(CurrentQuestion.QuestionData_Float[0]);

                CurrentQuestion.IntrosForQuestion = new List<IntroForQuestion>() { intro0, intro1 };

                ShuffleAnswerOptions();
                break;

            case QPattern.TouchOneManQuickly:
                List<TypeOfMan> men = EnumUtil.GetValues<TypeOfMan>();
                FisherYatesShuffle fisherYatesShuffle = new FisherYatesShuffle(men.Count);
                List<int> menIndex = fisherYatesShuffle.ShuffledList;

                int randomCorrectAnswer = Random.Range(0, 5);
                TypeOfMan correctMan = men[menIndex[randomCorrectAnswer]];


                string introText = "5 men will appear on screen. Identify and touch " + correctMan.ToString() + " very quickly. Get ready and click play.";

                for (int i = 0; i < CurrentQuestion.Options.Count; i++)
                {
                    SetAnswerButtonForTypeOfMen(men[menIndex[i]], i, correctMan);
                }

                CurrentQuestion.IntrosForQuestion[0].IntroText = introText;
                break;

            case QPattern.TrueFalse:
                break;

            case QPattern.XYZSeconds:
                ShuffleAnswerOptions();
                break;

            case QPattern.MemorizeOrder:
                break;

            case QPattern.ObjectInPrevious:

                string Question = "Which picture was " + ((CurrentQuestion.ObjectsInPreviousData.Mode == ObjectsInPrevious.In) ? "on" : "not on") + " the previous screen?";
                CurrentQuestion.SecondaryQuestion = new List<string>() { Question };

                List<Balloons> balloons = EnumUtil.GetValues<Balloons>();
                FisherYatesShuffle BalloonShuffle = new FisherYatesShuffle(balloons.Count);
                BalloonShuffle.ShuffleList();
                List<int> balloonShuffle = BalloonShuffle.ShuffledList;

                List<Balls> balls = EnumUtil.GetValues<Balls>();
                FisherYatesShuffle BallShuffle = new FisherYatesShuffle(balls.Count);
                BallShuffle.ShuffleList();
                List<int> ballShuffle = BallShuffle.ShuffledList;

                List<Balls> shuffledBallsList = new List<Balls>();
                List<Balloons> shuffledBalloonsList = new List<Balloons>();
                //Assuming balls and balloons have the same count
                for (int i = 0; i < balloons.Count; i++)
                {
                    shuffledBallsList.Add(balls[ballShuffle[i]]);
                    shuffledBalloonsList.Add(balloons[balloonShuffle[i]]);
                }

                CurrentQuestion.QuestionSprite = new List<BaseSpriteHolder>();
                for (int i = 0; i < 4; i++)
                {
                    BaseSpriteHolder baseSpriteHolder = new BaseSpriteHolder();
                    CurrentQuestion.QuestionSprite.Add(baseSpriteHolder);
                }

                CurrentQuestion.Options = new List<AnswerSpriteHolder>();
                for (int i = 0; i < 2; i++)
                {
                    AnswerSpriteHolder answerSpriteHolder = new AnswerSpriteHolder();
                    CurrentQuestion.Options.Add(answerSpriteHolder);
                }
                AssignSpritesToQuestionSprite_ObjectsInPreviousQuestion(shuffledBalloonsList, shuffledBallsList);
                ShuffleQuestionSprite_ObjectsInPreviousQuestion();
                ShuffleAnswerOptions();

                break;

            #endregion

            #region UNIQUE Patterns

        //UNIQUE PATTERN
            case QPattern.Different_Umbrellas:
                break;

        //UNIQUE PATTERN
            case QPattern.Giraffe:
                break;

        //UNIQUE PATTERN
            case QPattern.Hifi:
                break;

        //UNIQUE PATTERN
            case QPattern.KeysLocks:
                break;

        //UNIQUE PATTERN
            case QPattern.KnockTheDoor:
                break;

        //UNIQUE PATTERN
            case QPattern.LadyHands:
                break;

        //UNIQUE PATTERN



        //UNIQUE PATTERN
            case QPattern.MemorizeOrderHard:
                break;

        //UNIQUE PATTERN
            case QPattern.NextFiveBalloons:
                break;

        //UNIQUE PATTERN
            case QPattern.NextThreeBalloons:
                break;

        //UNIQUE PATTERN
            case QPattern.OpenCloseDoor:
                break;

        //UNIQUE PATTERN
            case QPattern.PlayButton:
                break;

        //UNIQUE PATTERN
            case QPattern.ThirdBalloon:
                break;

        //UNIQUE PATTERN
            case QPattern.TouchBallsWrongCount:
                break;

        //UNIQUE PATTERN
            case QPattern.TouchExceptYellowBall:
                break;

        //UNIQUE PATTERN
            case QPattern.Umbrellas:
                break;

                #endregion

        }
    }

    #region Varaition Helpers

    private void AssignSpritesToQuestionSprite_ObjectsInPreviousQuestion(List<Balloons> balloons, List<Balls> balls)
    {
        //Setting question sprite
        for (int i = 0; i < 4; i++)
        {
            if (i < 2)
            {
                switch (balloons[i])
                {
                    case Balloons.Balloon_Blue:
                        CurrentQuestion.QuestionSprite[i].Sprite = SpriteID.Balloon_Blue;
                        break;

                    case Balloons.Balloon_Green:
                        CurrentQuestion.QuestionSprite[i].Sprite = SpriteID.Balloon_Green;
                        break;

                    case Balloons.Balloon_Red:
                        CurrentQuestion.QuestionSprite[i].Sprite = SpriteID.Balloon_Red;
                        break;

                    case Balloons.Balloon_Yellow:
                        CurrentQuestion.QuestionSprite[i].Sprite = SpriteID.Balloon_Yellow;
                        break;
                }
            }
            else
            {
                switch (balls[i])
                {
                    case Balls.Ball_Blue:
                        CurrentQuestion.QuestionSprite[i].Sprite = SpriteID.Ball_Blue;
                        break;

                    case Balls.Ball_Green:
                        CurrentQuestion.QuestionSprite[i].Sprite = SpriteID.Ball_Green;
                        break;

                    case Balls.Ball_Red:
                        CurrentQuestion.QuestionSprite[i].Sprite = SpriteID.Ball_Red;
                        break;

                    case Balls.Ball_Yellow:
                        CurrentQuestion.QuestionSprite[i].Sprite = SpriteID.Ball_Yellow;
                        break;
                }
            }
        }

        // 0 - Balloon, 1 - Ball

        //For outside
        int randomBetweenBallAndBalloon_For_NotIn = Random.Range(0, 2);
        //For inside
        int randomBetweenBallAndBalloon_For_In = Random.Range(0, 2);

        SpriteID InSprite = SpriteID.DummySprite;
        SpriteID NotInSprite = SpriteID.DummySprite;

        if (randomBetweenBallAndBalloon_For_In == 0)
        {
            switch (balloons[0])
            {
                case Balloons.Balloon_Blue:
                    InSprite = SpriteID.Balloon_Blue;
                    break;

                case Balloons.Balloon_Green:
                    InSprite = SpriteID.Balloon_Green;
                    break;

                case Balloons.Balloon_Red:
                    InSprite = SpriteID.Balloon_Red;
                    break;

                case Balloons.Balloon_Yellow:
                    InSprite = SpriteID.Balloon_Yellow;
                    break;
            }
        }
        else
        {
            switch (balls[2])
            {
                case Balls.Ball_Blue:
                    InSprite = SpriteID.Ball_Blue;
                    break;

                case Balls.Ball_Green:
                    InSprite = SpriteID.Ball_Green;
                    break;

                case Balls.Ball_Red:
                    InSprite = SpriteID.Ball_Red;
                    break;

                case Balls.Ball_Yellow:
                    InSprite = SpriteID.Ball_Yellow;
                    break;
            }
        }

        if (randomBetweenBallAndBalloon_For_NotIn == 0)
        {
            switch (balloons[3])
            {
                case Balloons.Balloon_Blue:
                    NotInSprite = SpriteID.Balloon_Blue;
                    break;

                case Balloons.Balloon_Green:
                    NotInSprite = SpriteID.Balloon_Green;
                    break;

                case Balloons.Balloon_Red:
                    NotInSprite = SpriteID.Balloon_Red;
                    break;

                case Balloons.Balloon_Yellow:
                    NotInSprite = SpriteID.Balloon_Yellow;
                    break;
            }
        }
        else
        {
            switch (balls[0])
            {
                case Balls.Ball_Blue:
                    NotInSprite = SpriteID.Ball_Blue;
                    break;

                case Balls.Ball_Green:
                    NotInSprite = SpriteID.Ball_Green;
                    break;

                case Balls.Ball_Red:
                    NotInSprite = SpriteID.Ball_Red;
                    break;

                case Balls.Ball_Yellow:
                    NotInSprite = SpriteID.Ball_Yellow;
                    break;
            }
        }

        CurrentQuestion.Options[0].Sprite = NotInSprite;
        CurrentQuestion.Options[0].IsCorrect = (CurrentQuestion.ObjectsInPreviousData.Mode == ObjectsInPrevious.NotIn) ? true : false;

        CurrentQuestion.Options[1].Sprite = InSprite;
        CurrentQuestion.Options[1].IsCorrect = (CurrentQuestion.ObjectsInPreviousData.Mode == ObjectsInPrevious.In) ? true : false;

    }

    private void ShuffleQuestionSprite_ObjectsInPreviousQuestion()
    {
        List<BaseSpriteHolder> questionSprites = CurrentQuestion.QuestionSprite;
        FisherYatesShuffle fisherYatesShuffle = new FisherYatesShuffle(questionSprites.Count);
        fisherYatesShuffle.ShuffleList();
        List<BaseSpriteHolder> newQuestionSprites = new List<BaseSpriteHolder>();
        List<int> randomShuffle = fisherYatesShuffle.ShuffledList;
        for (int i = 0; i < CurrentQuestion.QuestionSprite.Count; i++)
        {
            newQuestionSprites.Add(questionSprites[randomShuffle[i]]);
        }
        CurrentQuestion.QuestionSprite = newQuestionSprites;
    }

    private void DetermineCorrectAnswerForIfElse(int random)
    {
        CurrentQuestion.Options[0].IsCorrect = false;
        CurrentQuestion.Options[1].IsCorrect = false;
        CurrentQuestion.Options[2].IsCorrect = false;

        if (random == 0)
        {
            switch (CurrentQuestion.IfElseData.IfFirst)
            {
                case OptionOrder.First:
                    CurrentQuestion.Options[0].IsCorrect = true;
                    CurrentQuestion.Options[1].IsCorrect = false;
                    CurrentQuestion.Options[2].IsCorrect = false;
                    break;

                case OptionOrder.Second:
                    CurrentQuestion.Options[1].IsCorrect = true;
                    CurrentQuestion.Options[0].IsCorrect = false;
                    CurrentQuestion.Options[2].IsCorrect = false;
                    break;

                case OptionOrder.Third:
                    CurrentQuestion.Options[2].IsCorrect = true;
                    CurrentQuestion.Options[1].IsCorrect = false;
                    CurrentQuestion.Options[0].IsCorrect = false;
                    break;
            }
        }
        else if (random == 1)
        {
            switch (CurrentQuestion.IfElseData.IfSecond)
            {
                case OptionOrder.First:
                    CurrentQuestion.Options[0].IsCorrect = true;
                    CurrentQuestion.Options[1].IsCorrect = false;
                    CurrentQuestion.Options[2].IsCorrect = false;
                    break;

                case OptionOrder.Second:
                    CurrentQuestion.Options[1].IsCorrect = true;
                    CurrentQuestion.Options[0].IsCorrect = false;
                    CurrentQuestion.Options[2].IsCorrect = false;
                    break;

                case OptionOrder.Third:
                    CurrentQuestion.Options[2].IsCorrect = true;
                    CurrentQuestion.Options[1].IsCorrect = false;
                    CurrentQuestion.Options[0].IsCorrect = false;
                    break;
            }
        }
    }

    private void SetAnswerButtonForTypeOfMen(TypeOfMan Man, int OptionIndex, TypeOfMan CorrectAnswer)
    {
        if (Man == CorrectAnswer)
        {
            CurrentQuestion.Options[OptionIndex].IsCorrect = true;
        }
        else
        {
            CurrentQuestion.Options[OptionIndex].IsCorrect = false;
        }

        switch (Man)
        {
            case TypeOfMan.ConfusedMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Confused;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.ConfusedMan;
                break;

            case TypeOfMan.GreedyMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Greedy;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.GreedyMan;
                break;

            case TypeOfMan.HandyMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Handy;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.HandyMan;
                break;

            case TypeOfMan.HappyMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Happy;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.HappyMan;
                break;

            case TypeOfMan.LoserMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Loser;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.LoserMan;
                break;

            case TypeOfMan.RichMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Rich;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.RichMan;
                break;

            case TypeOfMan.SportsFan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_SportsFan;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.SportsFan;
                break;

            case TypeOfMan.SportsMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Sports;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.SportsMan;
                break;

            case TypeOfMan.StrongMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Strong;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.StrongMan;
                break;

            case TypeOfMan.WeakMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Weak;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.WeakMan;
                break;

            case TypeOfMan.WinnerMan:
                CurrentQuestion.Options[OptionIndex].Sprite = SpriteID.Man_Winner;
                CurrentQuestion.Options[OptionIndex].ID = AnswerID.WinnerMan;
                break;
        }
    }

    private void ShuffleAnswerOptions(bool ShuffleOther = false, ShuffleList List = ShuffleList.None)
    {
        FisherYatesShuffle fisherYatesShuffle = new FisherYatesShuffle(CurrentQuestion.Options.Count);
        fisherYatesShuffle.ShuffleList();
        List<int> ShuffledList = fisherYatesShuffle.ShuffledList;

        List<AnswerSpriteHolder> NewOptions = new List<AnswerSpriteHolder>();

        for (int i = 0; i < ShuffledList.Count; i++)
        {
            NewOptions.Add(CurrentQuestion.Options[ShuffledList[i]]);
        }

        CurrentQuestion.Options = NewOptions;

        if (ShuffleOther)
        {
            switch (List)
            {
                case ShuffleList.IntData:
                    List<int> NewIntData = new List<int>();
                    for (int i = 0; i < ShuffledList.Count; i++)
                    {
                        NewIntData.Add(CurrentQuestion.QuestionData_Int[ShuffledList[i]]);
                    }

                    CurrentQuestion.QuestionData_Int = NewIntData;
                    break;

                case ShuffleList.FloatData:
                    List<float> NewFloatData = new List<float>();
                    for (int i = 0; i < ShuffledList.Count; i++)
                    {
                        NewFloatData.Add(CurrentQuestion.QuestionData_Int[ShuffledList[i]]);
                    }

                    CurrentQuestion.QuestionData_Float = NewFloatData;
                    break;
            }
        }
    }

    #endregion

    #endregion

    #region GameFlow

    private void ResetQuestionSpecificData()
    {
        if (Status == GameStatus.InSession)
        {
            if (CurrentSequentialQuestionIndex < CurrentQuestion.RepeatValue && IsSequentialQuestionSet)
                return;
        }

        CurrentSequentialQuestionIndex = 0;
        IsSequentialQuestionSet = false;
        CurrentIntroScreenIndex = 0;
        Answered = false;
        answeredTimer = 0;
        questionTimer = 0;
        Result = AnswerResult.Wrong;
    }

    private void ResetInput()
    {
        CanProcessInput = false;
    }

    public void ResetAnswerButtons()
    {
        if (ResetAllData != null)
        {
            ResetAllData.Invoke();
        }

        //Debug.Log("Calling next Question from Reset");
        //NextQuestion(0);
    }

    public void SetInput()
    {
        CanProcessInput = true;
    }

    public void PlayGame(bool freshGame = true)
    {
        bool gameFinished = false;

        if (CheckForRequisiteLifeOrBananasToPlayGame())
        {
            if(freshGame)
            {
                ResetTimer();
                ResetQuestionSpecificData();
            }

            //Testing Zone
            if (TestingQuestion.Test)
            {
                if (TestingQuestion.QuestionNumber == 0)
                {
                    CurrentQuestion = GetQuestionBasedOnPattern(TestingQuestion.Question);
                }
                else
                {
                    if (TestingQuestion.FirstTime)
                    {
                        GameDataManager.Instance.CurrentQuestion = TestingQuestion.QuestionNumber;
                        CurrentQuestion = Questions.Questions[GameDataManager.Instance.CurrentQuestion];
                        GameDataManager.Instance.CurrentLevelProgress = TestingQuestion.QuestionNumber % 10;
                        TestingQuestion.FirstTime = false;
                        TestingQuestion.Test = false;
                    }
                }
            }
            //Testing zone
            else
            {
                switch (GameDataManager.Instance.CurrentGameMode)
                {
                    case GameMode.SinglePlayer:
                        CurrentQuestionIndex = GameDataManager.Instance.CurrentQuestion;
                        currentQuestion = CurrentQuestionIndex;
                        levelProgress = GameDataManager.Instance.CurrentLevelProgress;
                        //ExtractQuestionData();
                        if(currentQuestion >= Questions.Questions.Count)
                        {
                            //Show Passed certificate
                            //UIManager.Instance.HideAllQuestionPanels();
                            currentQuestion = 0;
                            GameDataManager.Instance.CurrentQuestion = 0;
                            GamePassed();
                            gameFinished = true;
                        }
                        else
                        {
                            CurrentQuestion = Questions.Questions[currentQuestion];
                        }
                        break;

                    case GameMode.Multiplayer:
                        if (MultiplayerQuestionIndex < MultiplayerQuestions.Questions.Count)
                        {
                            CurrentQuestion = MultiplayerQuestions.Questions[CurrentMultiplayerQuestion];
                            Debug.Log("<color=green>Mutiplayer Question Index Type = " + MultiplayerQuestions.Questions[MultiplayerQuestionIndex].Pattern.ToString() + "</color>");
                        }
                        break;
                }
            }

            if(!gameFinished)
            {
                SetQuestionVariation();
                IntroToNext(freshGame);
            }
            else
            {
                gameFinished = false;
            }

        }
        else
        {
            ToStoreFromGameplayDueToLackOfFunds = true;

            switch (GameDataManager.Instance.CurrentGameMode)
            {
                case GameMode.SinglePlayer:
                    OpenStoreForLives();
                    break;

                case GameMode.Multiplayer:
                    OpenStoreForBananas();
                    break;
            }
        }
    }

    private void GamePassed()
    {
        Status = GameStatus.OutOfSession;
        GameDataManager.Instance.ResetData(ResetType.Progress);
        ScreenManager.Instance.SetANewScreen(ScreensEnum.PassedCertificate);
    }

    private bool CheckForRequisiteLifeOrBananasToPlayGame()
    {
        switch (GameDataManager.Instance.CurrentGameMode)
        {
            case GameMode.Multiplayer:
                if (GameDataManager.Instance.TotalBananas == 0)
                {
                    return false;
                }
                break;

            case GameMode.SinglePlayer:
                if (GameDataManager.Instance.TotalLives == 0)
                {
                    return false;
                }
                break;
        }

        return true;
    }

    public void OpenStoreForBananas()
    {
        UIManager.Instance.ShowPopUp("Get More Bananas to Continue!", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, OpenStore, null);
    }

    public void OpenStoreForLives()
    {
        UIManager.Instance.ShowPopUp("Get More Lives to Continue!", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, OpenStore, null);
    }

    private void ExtractQuestionData()
    {
        BaseQuestion ActualQuestion = Questions.Questions[currentQuestion];

        CurrentQuestion = new BaseQuestion();
        CurrentQuestion.Question = ActualQuestion.Question;
        CurrentQuestion.SecondaryQuestion = new List<string>();
        for (int i = 0; i < ActualQuestion.SecondaryQuestion.Count; i++)
        {
            string secondaryQuestion = ActualQuestion.SecondaryQuestion[i];
            CurrentQuestion.SecondaryQuestion.Add(secondaryQuestion);
        }
        CurrentQuestion.IgnoreClickCount = ActualQuestion.IgnoreClickCount;
        CurrentQuestion.IgnoreClickTime = ActualQuestion.IgnoreClickTime;
        CurrentQuestion.HasSequenceAnswer = ActualQuestion.HasSequenceAnswer;
        CurrentQuestion.HasBoolValue = ActualQuestion.HasBoolValue;
        CurrentQuestion.HasTimerValue = ActualQuestion.HasTimerValue;
        CurrentQuestion.HasSequentialQuestions = ActualQuestion.HasSequentialQuestions;
        CurrentQuestion.RepeatValue = ActualQuestion.RepeatValue;
        CurrentQuestion.Pattern = ActualQuestion.Pattern;
        CurrentQuestion.QuestionType = ActualQuestion.QuestionType;
        CurrentQuestion.TimeForQuestion = ActualQuestion.TimeForQuestion;
        CurrentQuestion.IntrosForQuestion = new List<IntroForQuestion>();
        for (int i = 0; i < ActualQuestion.IntrosForQuestion.Count; i++)
        {
            IntroForQuestion intro = new IntroForQuestion();
            intro.IntroText = ActualQuestion.IntrosForQuestion[i].IntroText;
            intro.HasTimer = ActualQuestion.IntrosForQuestion[i].HasTimer;
            intro.HideOkButton = ActualQuestion.IntrosForQuestion[i].HideOkButton;
            intro.TimerValue = ActualQuestion.IntrosForQuestion[i].TimerValue;
            CurrentQuestion.IntrosForQuestion.Add(intro);
        }
        CurrentQuestion.QuestionData_Int = new List<int>();
        for (int i = 0; i < ActualQuestion.QuestionData_Int.Count; i++)
        {
            int data = ActualQuestion.QuestionData_Int[i];
            CurrentQuestion.QuestionData_Int.Add(data);
        }
        CurrentQuestion.QuestionData_Float = new List<float>();
        for (int i = 0; i < ActualQuestion.QuestionData_Int.Count; i++)
        {
            float data = ActualQuestion.QuestionData_Float[i];
            CurrentQuestion.QuestionData_Float.Add(data);
        }
        CurrentQuestion.ReturnValue_Bool = ActualQuestion.ReturnValue_Bool;
        CurrentQuestion.DelayTimeAfterQuestionShow = ActualQuestion.DelayTimeAfterQuestionShow;
        CurrentQuestion.QuestionSprite = new List<BaseSpriteHolder>();
        for (int i = 0; i < ActualQuestion.QuestionSprite.Count; i++)
        {
            BaseSpriteHolder questionSprite = new BaseSpriteHolder();
            questionSprite.text = ActualQuestion.QuestionSprite[i].text;
            questionSprite.Sprite = ActualQuestion.QuestionSprite[i].Sprite;
            SpriteID spriteID = new SpriteID();
            questionSprite.SecondarySprites = new List<SpriteID>();
            for (int j = 0; j < ActualQuestion.QuestionSprite[i].SecondarySprites.Count; j++)
            {
                spriteID = ActualQuestion.QuestionSprite[i].SecondarySprites[i];
                questionSprite.SecondarySprites.Add(spriteID);
            }
            questionSprite.TextColor = ActualQuestion.QuestionSprite[i].TextColor;
            CurrentQuestion.QuestionSprite.Add(questionSprite);
        }
        CurrentQuestion.Options = new List<AnswerSpriteHolder>();
        for (int i = 0; i < ActualQuestion.Options.Count; i++)
        {
            AnswerSpriteHolder answer = new AnswerSpriteHolder();
            answer.text = ActualQuestion.Options[i].text;
            answer.Sprite = ActualQuestion.Options[i].Sprite;
            answer.SecondarySprites = new List<SpriteID>();
            for (int j = 0; j < ActualQuestion.Options[i].SecondarySprites.Count; j++)
            {
                SpriteID spriteID = ActualQuestion.Options[i].SecondarySprites[j];
                answer.SecondarySprites.Add(spriteID);
            }
            answer.TextColor = ActualQuestion.Options[i].TextColor;
            answer.IsCorrect = ActualQuestion.Options[i].IsCorrect;
            answer.ID = ActualQuestion.Options[i].ID;

            answer.SequenceInfo = new List<SequenceOfClick>();
            for (int k = 0; k < ActualQuestion.Options[i].SequenceInfo.Count; k++)
            {
                SequenceOfClick sequence = new SequenceOfClick();
                sequence.SequenceNumber = ActualQuestion.Options[i].SequenceInfo[k].SequenceNumber;
                sequence.RequiredClicks = ActualQuestion.Options[i].SequenceInfo[k].RequiredClicks;
                sequence.ClickOnTime = ActualQuestion.Options[i].SequenceInfo[k].ClickOnTime;
                answer.SequenceInfo.Add(sequence);
            }
            CurrentQuestion.Options.Add(answer);
        }
        CurrentQuestion.WaitTimeAfterAnswer = ActualQuestion.WaitTimeAfterAnswer;
        CurrentQuestion.AllowMultipleAnswers = ActualQuestion.AllowMultipleAnswers;
        CurrentQuestion.sortOrder = ActualQuestion.sortOrder;

        CurrentQuestion.IfElseData = ActualQuestion.IfElseData;

        //CurrentQuestion.IfElseData.QuestionAppear = ActualQuestion.IfElseData.QuestionAppear;
        //CurrentQuestion.IfElseData.IfFirst = ActualQuestion.IfElseData.IfFirst;
        //CurrentQuestion.IfElseData.IfSecond = ActualQuestion.IfElseData.IfSecond;

        CurrentQuestion.ObjectsInPreviousData = ActualQuestion.ObjectsInPreviousData;
    }

    public void IntroToNext(bool freshGame = true)
    {
        Debug.Log("IntroToNext");

        if(freshGame)
        {
            ResetTimer();
        }

        if (CurrentQuestion.IntrosForQuestion.Count > CurrentIntroScreenIndex)
        {
            Status = GameStatus.IntroSession;
            ShowIntro();
        }
        else
        {
            ShowActualQuestion(freshGame);
        }
    }

    private void ShowIntro()
    {
        Debug.Log("ShowIntro");

        string questionIntro = CurrentQuestion.IntrosForQuestion[CurrentIntroScreenIndex].IntroText;
        int timer = Mathf.RoundToInt(CurrentQuestion.IntrosForQuestion[CurrentIntroScreenIndex].TimerValue);
        bool showOkButton = !CurrentQuestion.IntrosForQuestion[CurrentIntroScreenIndex].HideOkButton;
        bool hasTimerValue = CurrentQuestion.IntrosForQuestion[CurrentIntroScreenIndex].HasTimer;

        UIManager.Instance.ShowIntroScreen(questionIntro, timer, showOkButton, hasTimerValue);
        CurrentIntroScreenIndex++;
    }

    public void NextQuestion(int question = -1)
    {
        Debug.Log("Next Question");

        if (GameDataManager.Instance.CurrentGameMode == GameMode.Multiplayer)
        {
            if (MultiplayerQuestionIndex >= MultiplayerQuestionsPerRound && !IsMyGameOver)
            {
                IsMyGameOver = true;
                CheckAndEvaluateMultiplayer();

                return;
            }
            else if (MultiplayerQuestionIndex == MultiplayerQuestionsPerRound - 1)
            {
                UIManager.Instance.ShowPopUp("Answer this question to get a 2x Score", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);
            }
        }

        //Reseting data for Next Question
        Result = AnswerResult.Wrong;

        if (CurrentQuestion.HasSequentialQuestions)
        {
            //Debug.Log("CurrentQuestion.HasSequentialQuestions");

            if (CurrentSequentialQuestionIndex >= CurrentQuestion.RepeatValue - 1)
            {
                //Debug.Log("CurrentSequentialQuestionIndex = " + CurrentSequentialQuestionIndex + " >= CurrentQuestion.RepeatValue - 1");

                IsSequentialQuestionSet = false;
                IncreaseQuestionIndex();
            }
            else
            {
                //Debug.Log("else");
                CurrentSequentialQuestionIndex++;
                //Debug.Log("CurrentSequentialQuestionIndex = " + CurrentSequentialQuestionIndex);
            }
        }
        else
        {
            Status = GameStatus.OutOfSession;
            //Disable QuestionPanel

        }

        PlayGame();
    }

    public void PlayButtonClicked()
    {
        UIManager.Instance.HideIntro();
        ResetTimer();
        IntroToNext();
    }

    private void ShowActualQuestion(bool freshGame = true)
    {
        Debug.Log("ShowActualQuestion");

        Status = GameStatus.InSession;
        if(freshGame)
        {
            ResetAnswerButtons();
            GetCorrectSetterAndEvaluator(CurrentQuestion.Pattern);
            ResetInput();
            ResetTimer();
            ResetAnswerSequenceIndex();
            SetSequence();
            questionTimer = Mathf.RoundToInt(CurrentQuestion.TimeForQuestion);
        }

        SetMaxValuesForQuestion();
        if (IsSequentialQuestionSet)
        {
            Setter.GetComponent<ISetter>().SetQuestion(SequentialQuestions[CurrentSequentialQuestionIndex], Evaluator.ButtonClicked);
        }
        else
        {
            Setter.GetComponent<ISetter>().SetQuestion(CurrentQuestion, Evaluator.ButtonClicked);
        }
        if (Status == GameStatus.InSession)
        {
            UIManager.Instance.ShowGameplay(false);
        }
        else
        {
            UIManager.Instance.ShowGameplay();
        }
    }

    public BaseQuestion GetCurrentQuestion(bool forceGetCurrentQuestion = false)
    {
        if (forceGetCurrentQuestion)
        {
            return CurrentQuestion;
        }

        if (CurrentQuestion.HasSequentialQuestions)
        {
            if (CurrentSequentialQuestionIndex < CurrentQuestion.RepeatValue)
            {
                return SequentialQuestions[CurrentSequentialQuestionIndex];
            }
            else
            {
                return CurrentQuestion;
            }
        }
        else
        {
            return CurrentQuestion;
        }
    }

    private void LevelCompleted(bool comingFromSkippedQuestion = false)
    {
        //Debug.Log("<color=red><--------------------------------------->Level Completed<-----------------------------------------------------></color>");

        GameDataManager.Instance.CurrentLevel++;
        GameDataManager.Instance.RemainingLevels = GameDataManager.Instance.TotalLevels - GameDataManager.Instance.CurrentLevel;
        GameDataManager.Instance.LevelFallBackQuestion = GameDataManager.Instance.CurrentQuestion;
        GameDataManager.Instance.CurrentLevelProgress = 0;
        if (!comingFromSkippedQuestion)
        {
            GameDataManager.Instance.LivesEarned = LivesEarnedPerLevel;
            AddDeductCurrency(Currency.Life, AddDeductAction.Add, LivesEarnedPerLevel);
        }
        else
        {
            //TODO life earned calculation
        }
        //TODO clear this once level completed is implemented
        UIManager.Instance.LevelCompleted();
    }

    private void LevelClearedContinue()
    {
        GameDataManager.Instance.ResetData(ResetType.LevelSpecific);
    }

    #endregion

    #region Click Related

    public void OnClickedButton(int clicked, int ButtonId)
    {
        if (Clicked != null)
        {
            Clicked.Invoke(clicked, ButtonId);
        }
    }

    #endregion

    #region Test

    private BaseQuestion GetQuestionBasedOnPattern(QPattern qPattern)
    {
        BaseQuestion question = null;
        bool questionFound = false;

        for (int i = 0; i < Questions.Questions.Count; i++)
        {
            if (Questions.Questions[i].Pattern == qPattern)
            {
                question = Questions.Questions[i];
                questionFound = true;
                break;
            }
        }

        if (!questionFound)
        {
            for (int i = 0; i < Questions.UniqueQuestions.Count; i++)
            {
                if (Questions.UniqueQuestions[i].Pattern == qPattern)
                {
                    question = Questions.UniqueQuestions[i];
                    break;
                }
            }
        }

        return question;
    }

    #endregion

    #region Answer Specific

    public void AnsweredCorrectly()
    {
        if (!CurrentQuestion.HasSequentialQuestions)
        {
            if (!CurrentQuestion.AllowMultipleAnswers)
            {
                if (Answered)
                    return;
            }
        }

        CheckIfAnswered();

        ClickedButton();

        Debug.Log("Correct");
        UIManager.Instance.QuestionAnswered();
        UIManager.Instance.AnswerButtonAnimation();

        Result = AnswerResult.Correct;

        if (IsSequentialQuestionSet)
        {
            ShowAnswerResult();
        }
    }

    public void AnsweredWrongly()
    {
        if (!CurrentQuestion.HasSequentialQuestions)
        {
            if (!CurrentQuestion.AllowMultipleAnswers)
            {
                if (Answered)
                    return;
            }
        }

        CheckIfAnswered();

        ClickedButton();

        Debug.Log("Wrong");

        Result = AnswerResult.Wrong;
        UIManager.Instance.QuestionAnswered();
        UIManager.Instance.AnswerButtonAnimation();

        IsSequentialQuestionSet = false;
        SequentialQuestions.Clear();

        //ShowAnswerResult();
    }

    private void CheckIfAnswered()
    {
        if (!Answered)
        {
            Answered = true;
            answeredTimer = timerInt + CurrentQuestion.WaitTimeAfterAnswer;
        }
    }

    private void ClickedButton()
    {
        Debug.Log("ClickedButton");
        UIManager.Instance.AnswerButtonClicked();
    }

    private void ShowAnswerResult(bool comingFromSkippedQuestion = false)
    {
        //Replacing any shuffling in the options with the original options
        UIManager.Instance.ShowingAnswerResult();
        switch (Result)
        {
            case AnswerResult.Correct:
                switch (GameDataManager.Instance.CurrentGameMode)
                {
                    case GameMode.SinglePlayer:
                        if (GameDataManager.Instance.CurrentQuestion > TotalQuestions)
                        {
                            //TODO Show game finished panel
                        }
                        else
                        {
                            if (IsSequentialQuestionSet && CurrentSequentialQuestionIndex < CurrentQuestion.RepeatValue)
                            {
                                NextQuestion();
                            }
                            else
                            {
                                if (!comingFromSkippedQuestion)
                                {
                                    GameDataManager.Instance.WinStreak_Regular++;
                                    GameDataManager.Instance.QuestionsAnsweredCorrectly++;
                                }
                                else
                                {
                                    GameDataManager.Instance.QuestionPassed++;
                                }

                                GameDataManager.Instance.QuestionsLeft--;
                                if (GameDataManager.Instance.QuestionsLeft > 0)
                                {
                                    IncreaseQuestionIndex();
                                    GameDataManager.Instance.CurrentLevelProgress++;
                                    GameDataManager.Instance.Score += ScorePerQuestion;
                                    if (GameDataManager.Instance.CurrentLevelProgress > LevelIndex)
                                    {
                                        LevelCompleted(comingFromSkippedQuestion);
                                        LevelClearedContinue();
                                    }
                                    else
                                    {
                                        NextQuestion();
                                    }
                                }
                                else
                                {
                                    //TODO Show Passed Game over
                                    GameDataManager.Instance.CurrentQuestion = 0;
                                }
                            }
                        }
                        Debug.Log("Result shown = correct");
                        break;

                    case GameMode.Multiplayer:
                        if (IsSequentialQuestionSet && CurrentSequentialQuestionIndex < CurrentQuestion.RepeatValue)
                        {
                            NextQuestion();
                        }
                        else
                        {
                            CurrentMultiplayerQuestion++;
                            MultiplayerQuestionIndex++;
                            if (MultiplayerQuestionIndex < MultiplayerQuestionsPerRound)
                            {
                                SetTimeForMultiplayerQuestionAnswered();
                                CalculateScorePerQuestion(MultiplayerQuestionIndex);
                            }
                            NextQuestion();
                        }
                        break;
                }
                break;

            case AnswerResult.Wrong:
                switch (GameDataManager.Instance.CurrentGameMode)
                {
                    case GameMode.SinglePlayer:
                        PenaltyForWrongAnswer();
                        
                        IsSequentialQuestionSet = false;
                        SequentialQuestions.Clear();
                        ResetQuestionSpecificData();
                        Status = GameStatus.OutOfSession;

                        GameDataManager.Instance.WinStreak_Regular = 0;
                        
                        //if (GameDataManager.Instance.TotalLives <= 0)
                        //{
                        //    //TODO Show shop
                        //}
                        if (GameDataManager.Instance.QuestionAttempt > ResetAttempt)
                        {
                            GameDataManager.Instance.CurrentQuestion = GameDataManager.Instance.CurrentQuestion - GameDataManager.Instance.CurrentLevelProgress;
                            GameDataManager.Instance.LastFallBackQuestion = GameDataManager.Instance.CurrentQuestion;
                            GameDataManager.Instance.CurrentLevelProgress = 0;
                            GameDataManager.Instance.QuestionAttempt = 0;
                            UIManager.Instance.ShowFailedCertificate();
                        }
                        else if (GameDataManager.Instance.QuestionAttempt > WarningTextAttempt)
                        {
                            UIManager.Instance.ShowWarning();
                        }
                        else
                        {
                            UIManager.Instance.AnsweredWrong();
                        }
                        Debug.Log("Result shown = Wrong");
                        break;

                    case GameMode.Multiplayer:
                        IsSequentialQuestionSet = false;
                        SequentialQuestions.Clear();
                        ResetQuestionSpecificData();
                        CurrentMultiplayerQuestion++;
                        MultiplayerQuestionIndex++;
                        if (MultiplayerQuestionIndex < MultiplayerQuestionsPerRound)
                        {
                            SetTimeForMultiplayerQuestionAnswered();
                        }
                        NextQuestion();
                        break;
                }

                break;
        }
    }

    private void PenaltyForWrongAnswer(bool fromSkip = false)
    {
        if (!fromSkip)
        {
            GameDataManager.Instance.NumberOfAttemptsInThisLevel++;
            GameDataManager.Instance.OverallAttemptsTaken++;
            GameDataManager.Instance.QuestionAttempt++;
            GameDataManager.Instance.Score -= DecreaseScorePerAttempt;
        }
        AddDeductCurrency(Currency.Life, AddDeductAction.Deduct, ScoreDuductPerQuestion_Regular);
    }

    private static void IncreaseQuestionIndex()
    {
        GameDataManager.Instance.CurrentQuestion++;
        GameDataManager.Instance.LastFallBackQuestion = GameDataManager.Instance.CurrentQuestion;
    }

    public void SkippedQuestion()
    {
        if (GameDataManager.Instance.TotalLives <= 0)
        {
            OpenStoreForLives();
        }
        else
        {
            UIManager.Instance.HideQuestionFailed();
            ScreenManager.Instance.SetANewScreen(ScreensEnum.GamePlay);
            
            PenaltyForWrongAnswer(true);
            
            Result = AnswerResult.Correct;
            ShowAnswerResult(true);
        }

    }

    #endregion

    #region Pause - Resume

    public void Pause()
    {
        if (PauseAction != null)
        {
            PauseAction.Invoke();
        }

        Time.timeScale = 0;
    }

    public void Resume()
    {
        Debug.Log("Resume");

        Time.timeScale = 1;

        if (ResumeAction != null)
        {
            ResumeAction.Invoke();
        }
    }

    #endregion

    #region Sequence Specific

    public void ResetAnswerSequenceIndex()
    {
        nextAnswerSequence = 1;
    }

    public void IncreaseSequenceNumber()
    {
        ClickedButton();
        nextAnswerSequence++;
        SetSequence();
        Debug.Log("FROM INCREASE SEQUENCE NUMBER");
        UIManager.Instance.CheckForEffectsOnButtonClick();
    }

    private void SetSequence()
    {
        if (CurrentQuestion == null)
            return;

        if (CurrentQuestion.HasSequenceAnswer)
        {
            Debug.Log("nextSeq = " + nextAnswerSequence);
            SequenceOfClick = CurrentQuestion.GetCurrentSequence(nextAnswerSequence);
        }
    }

    private void SetMaxValuesForQuestion()
    {
        MaxSequence = CurrentQuestion.GetMaximumSequence();
        MaxInt = CurrentQuestion.GetMaximumIntData();
        MaxFloat = CurrentQuestion.GetMaximumFloatData();
        SequenceCount.Clear();
    }

    #endregion

    #region UI Elements Changed

    public void LivesChanged()
    {
        UIManager.Instance.LivesChanged();
    }

    public void BananasChanged()
    {
        UIManager.Instance.BananasChanged();
    }

    public void OverallAttemptsChanged()
    {
        UIManager.Instance.OverallAttemptsChanged();
    }

    public void GameModeChanged()
    {
        UIManager.Instance.GameModeChanged();
    }

    public void MusicStateChanged()
    {
        UIManager.Instance.MusicStateChanged();
    }

    public void SFXStateChanged()
    {
        UIManager.Instance.SFXStateChanged();
    }

    public void LivesEarnedChanged()
    {
        UIManager.Instance.LivesEarnedChanged();
    }

    public void LevelsChanged()
    {
        UIManager.Instance.TotalLevelsChanged();
    }

    public void ScoreChanged()
    {
        UIManager.Instance.ScoreChanged();
    }

    #endregion

    #region InApps and Add Life/Banana Rewards Related

    public void RemoveAds()
    {
        GameDataManager.Instance.IsRemoveAds = true;
    }

    public void AddDeductCurrency(Currency type, AddDeductAction action, int amount)
    {
        if (amount < 0)
        {
            GameDataManager.Instance.IsUnlimitedLives = true;
            return;
        }

        int currency = AddDeductValueOfCurrency(action, amount);
        switch (type)
        {
            case Currency.Banana:
                //if(action == AddDeductAction.Add)
                //{
                    //UIManager.Instance.ShowPopUp("Successfully added "+amount+" Bananas!", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);
                //}
                GameDataManager.Instance.TotalBananas += currency;
                break;

            case Currency.Life:
                if (GameDataManager.Instance.IsUnlimitedLives)
                    return;
                //if (action == AddDeductAction.Add)
                //{
                //    UIManager.Instance.ShowPopUp("Successfully added " + amount + " Life!", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);
                //}
                GameDataManager.Instance.TotalLives += currency;
                break;
        }
    }

    private int AddDeductValueOfCurrency(AddDeductAction action, int amount)
    {
        int currency = 0;
        switch (action)
        {
            case AddDeductAction.Add:
                currency += amount;
                break;

            default: //AddDeductAction.Deduct
                currency -= amount;
                break;
        }

        return currency;
    }

    public void OpenStore()
    {
        ScreenManager.Instance.SetANewScreen(ScreensEnum.Store);
    }

    public void ReviewApp()
    {
        ResumeFrom = ApplicationResumeFrom.ReviewApp;
    }

    public void ShareRegular()
    {
        ResumeFrom = ApplicationResumeFrom.ShareRegular;
        ShareManager.Instance.NativeShare(ShareType.FromGameWin);
    }

    #endregion

    #region Multiplayer Related

    public void PlayMultiplayer()
    {
        MultiplayerManager.Instance.Connect();
    }

    public void StartFindingMatch()
    {
        if (MultiplayerManager.Instance.GetBetAmount(GameDataManager.Instance.BetAmount) < GameDataManager.Instance.TotalBananas)
        {
            OpenStoreForBananas();
        }
        else
        {
            UIManager.Instance.SetMultiplayerUI(MultiplayerMode.FindingPlayer);
            ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerGamePanel);
            MultiplayerManager.Instance.JoinOnCreateRandomRoom();
            CurrentMultiplayerGameplay = MultiplayerGameplayType.Person;
            ActivelyLookingForOpponent = true;
        }

    }

    public void CancelFindingmatch(bool leaveRoom = true)
    {
        ActivelyLookingForOpponent = false;
        AmIAloneInRoom = false;
        UIManager.Instance.SetMultiplayerUI(MultiplayerMode.FindPlayer);
        if (leaveRoom)
        {
            MultiplayerManager.Instance.LeaveRoom();
            ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerGamePanel);
        }
        else
        {
            ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerMode);
        }
    }

    public void JoinGame()
    {
        Debug.Log("Join Game");
        code = UIManager.Instance.MultiplayerJoinGameInput.text;
        Debug.Log("Code = " + code);
        char[] array = code.ToCharArray();
        int bet = 0;
        int.TryParse(array[0].ToString(), out bet);
        Debug.Log("BetAmount = " + bet.ToString());
        int betRequired = MultiplayerManager.Instance.GetBetAmount((BetAmount)bet);
        Debug.Log("Bet Required = " + betRequired.ToString());
        if (betRequired > GameDataManager.Instance.TotalBananas)
        {
            OpenStoreForBananas();
        }
        else
        {
            UIManager.Instance.ShowPopUp("Trying to connect to Challenge game.", new List<string>(){ "Cancel" }, TypeOfPopUpButtons.Ok, TypeOfPopUp.ButtonedAndEvented, 0, ChallengeCancelled, null);
            MultiplayerManager.Instance.JoinNamedRoom(code);
        }
    }

    public void ChallengeCancelled()
    {
        MultiplayerManager.Instance.LeaveRoom();
        MultiplayerManager.Instance.Disconnect();
    }

    public void ChallengeRoomJoinFailed()
    {
        ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerMode);
    }

    public void CreatedRoom_Random()
    {
        AmIAloneInRoom = true;
        StartWaiting();
    }

    public void Joined_RandomRoom()
    {
        if (PhotonNetwork.room.PlayerCount > 1)
        {
            AmIAloneInRoom = false;
            CurrentMultiplayerGameplay = MultiplayerGameplayType.Person;
            ActivelyLookingForOpponent = false;
        }
    }

    public void CreatedRoom_Challenge()
    {
        Debug.Log("CreatedRoom_Challenge");
        AmIAloneInRoom = true;
        CurrentMultiplayerGameplay = MultiplayerGameplayType.Person;
        UIManager.Instance.MultiplayerUI.Mode = MultiplayerMode.ChallengeWaitingForPlayer;
    }

    public void SetMultiplayerIndex(int questionIndex)
    {
        CurrentMultiplayerQuestion = questionIndex;
    }

    public int GetRandomMultiplayerIndex()
    {
        List<int> questionStartIndices = new List<int>();
        for (int i = 0; i < (42 * 5); i = i + 5)
        {
            questionStartIndices.Add(i);
        }

        int rand = Random.Range(0, questionStartIndices.Count);
        Debug.Log("<color=red>Rand = " + rand + "</color>");
        Debug.Log("<color=red>INDEX = " + questionStartIndices[rand] + "</color>");
        return questionStartIndices[rand];
    }

    public void BetChanged()
    {
        MultiplayerManager.Instance.Bet = GameDataManager.Instance.BetAmount;
    }

    public void ResetAfterMultiplayerDisconnect()
    {
        code = string.Empty;
        MultiplayerQuestionIndex = 0;
        //CurrentMultiplayerQuestion = 0;
        LastQuestionAnsweredTime = 0;
        TimeSinceLastQuestionAnswered = MultiplayerTotalTime;
        //MultiplayerScore = 0;
        //MultiplayerOpponentScore = 0;
        IsOpponentGameOver = false;
        IsMyGameOver = false;
        IsGameStarted = false;
    }

    private void StartWaiting()
    {
        Resume();
        ResetTimer();
        CurrentWaitingTimeForMultiplayer = timerInt + MultiplayerWaitTimeFor2ndPlayer;
        Debug.Log("CurrentWaitingTimeForMultiplayer = " + CurrentWaitingTimeForMultiplayer);
    }

    public void PlayMultiplayerGame()
    {
        ResetAfterMultiplayerDisconnect();
        MultiplayerScore = 0;
        MultiplayerOpponentScore = 0;
        ResetMultiplayerTimer();
        CurrentBetAmount = MultiplayerManager.Instance.GetBetAmount(GameDataManager.Instance.BetAmount);
        AddDeductCurrency(Currency.Banana, AddDeductAction.Deduct, CurrentBetAmount);
        IsGameStarted = true;
        ScreenManager.Instance.SetANewScreen(ScreensEnum.GamePlay);
        PlayGame();
    }

    //AI - Person Score Update
    public void MultiplayerOpponentScoreUpdate(int score)
    {
        Debug.Log("Score recieved = " + score);
        MultiplayerOpponentScore += score;
    }

    private void CalculateScorePerQuestion(int questionIndex)
    {
        if (questionIndex < MultiplayerQuestionsPerRound - 1)
        {
            MultiplayerScore += ScorePerQuestionMultiplayer - (MultiplayerQuestionMultiplier * LastQuestionAnsweredTime);
        }
        else//Double
        {
            MultiplayerScore += ScoreForLastQuestionMultiplayer - (MultiplayerLastQuestionMultiplier * LastQuestionAnsweredTime);
        }
    }

    private void SetTimeForMultiplayerQuestionAnswered()
    {
        LastQuestionAnsweredTime = timerMultiplayer - TimeSinceLastQuestionAnswered;
        TimeSinceLastQuestionAnswered = timerMultiplayer;
    }

    //AI Game
    public void AIGameOver()
    {
        IsOpponentGameOver = true;
        if (IsMyGameOver)
        {
            EvaluateAndShowResult();
        }
    }

    private void InitiateAIPlay()
    {
        CurrentMultiplayerGameplay = MultiplayerGameplayType.AI;
        if (PhotonNetwork.room != null)
        {
            MultiplayerManager.Instance.LeaveRoom();
        }
        else
        {
            StartAIPlay();
        }
    }

    public void MultiplayerCodeShared()
    {
        Debug.Log("Bet = " + MultiplayerManager.Instance.GetBetAmount(GameDataManager.Instance.BetAmount).ToString());

        if (MultiplayerManager.Instance.GetBetAmount(GameDataManager.Instance.BetAmount) > GameDataManager.Instance.TotalBananas)
        {
            OpenStoreForBananas();
        }
        else
        {
            code = ((int)CurrentBetAmount).ToString();
            code += System.DateTime.Now.ToString("fff");
            code += RandomAlphaNumeric.GenerateRandomNumeric(3);
            ResumeFrom = ApplicationResumeFrom.ShareMultiplayer;
            Debug.Log("Code = " + code);
            ShareManager.Instance.NativeShare(ShareType.FromMultiplayer, code);
        }
    }

    private void CreateRoomForChallenge()
    {
        MultiplayerManager.Instance.CreateChallengeRoom(code);
    }

    public void StartAIPlay()
    {
        Debug.Log("<color=red>AI IS PLAYING</color>");
        AmIAloneInRoom = true;
        AI_Manager.Instance.StartAI(GameDataManager.Instance.BetAmount);
        PlayMultiplayerGame();
    }

    public void CheckAndEvaluateMultiplayer(bool isOpponentGameOver = false)
    {
        if (GameDataManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
            return;
        if (CurrentMultiplayerGameplay == MultiplayerGameplayType.Person)
        {
            MultiplayerManager.Instance.MultiplayerGameOver(MultiplayerScore, isOpponentGameOver);
        }
        else if (CurrentMultiplayerGameplay == MultiplayerGameplayType.AI)
        {
            if (IsOpponentGameOver)
            {
                EvaluateAndShowResult();
            }
            else
            {
                WaitForOpponentToFinish();
            }
        }
    }

    public void WaitForOpponentToFinish()
    {
        IsWaiting = true;
        waitTime = timerInt + MultiplayerMaxWaitTimeForResult;
        UIManager.Instance.ShowPopUp("Waiting for opponent to finish game", null, TypeOfPopUpButtons.NoButton, TypeOfPopUp.Evented, 0, null, null); 
    }

    public void EvaluateAndShowResult(bool opponentLeft = false)
    {
        IsWaiting = false;
        UIManager.Instance.HidePopUp();

        if (GameDataManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
            return;

        if (opponentLeft)
        {
            if (IsGameStarted)
            {
                IWin();
                return;
            }
        }

        Debug.Log("<color=red>EvaluateAndShowResult</color>");
        UIManager.Instance.HidePopUp();
        if (MultiplayerScore >= MultiplayerOpponentScore)
        {
            //I Win
            IWin();
        }
        else
        {
            //I Lose
            ILose();
        }
    }

    private void IWin()
    {
        Debug.Log("I Win");

        AddDeductCurrency(Currency.Banana, AddDeductAction.Add, (2 * CurrentBetAmount));
        Status = GameStatus.OutOfSession;
        ResetAfterMultiplayerDisconnect();
        GameDataManager.Instance.WinStreak_Multiplayer++;
        GameDataManager.Instance.LoseStreak_Multiplayer = 0;
        UIManager.Instance.MultiplayerBananasWon(2 * CurrentBetAmount);
        UIManager.Instance.MultiplayerWin();
    }

    private void ILose()
    {
        Debug.Log("I Lose");

        Status = GameStatus.OutOfSession;
        ResetAfterMultiplayerDisconnect();
        GameDataManager.Instance.WinStreak_Multiplayer = 0;
        GameDataManager.Instance.LoseStreak_Multiplayer++;
        UIManager.Instance.MultiplayerBananasLost(CurrentBetAmount);
        UIManager.Instance.MultiplayerLose();
    }

    private void MatchDrawn()
    {
        Debug.Log("Match Drawn");

        AddDeductCurrency(Currency.Banana, AddDeductAction.Add, (CurrentBetAmount));
        Status = GameStatus.OutOfSession;
        ResetAfterMultiplayerDisconnect();
        //GameDataManager.Instance.WinStreak_Multiplayer++;
        //GameDataManager.Instance.LoseStreak_Multiplayer = 0;
        UIManager.Instance.MultiplayerBananasWon(2 * CurrentBetAmount);
        UIManager.Instance.MultiplayerWin(); 
    }

    public void GetToMultiplayerPlayMenu()
    {
        CancelFindingmatch(false);
    }

    #endregion

    #endregion
}

[Serializable]
public class SetterHolder
{
    public String SetterPattern;
    public QPattern Pattern;
    public GameObject Setter;
}

[Serializable]
public struct TestQuestion
{
    public bool Test;
    public QPattern Question;
    public int QuestionNumber;
    public bool FirstTime;
}

public enum ApplicationResumeFrom
{
    None,
    ShareMultiplayer,
    ShareRegular,
    ReviewApp
}
