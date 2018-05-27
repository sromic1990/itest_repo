using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utilities;
using Sourav.Utilities.Scripts.Algorithms.Shuffle;



public class AI_Manager : Singleton<AI_Manager>
{
    //TODO AI names
    public List<string> AInames;
    public List<AIUnit> ai_units;
    public AIState mAIstate;

    [SerializeField]
    private int MULTIPLAYER_QUESTION_NUMBER;
    // = 5;

    [SerializeField]
    private List<QuestionAnswerInfo> mInfo;

    [SerializeField]
    private int CurrentQuestionIndex = 0;

    private int maxAllowedTime;
    // = 60;

    private int ScorePerQuestion;
    private int ScoreForLastQuestion;

    private int QuestionMultiplier;
    private int LastQuestionMultiplier;

    void Awake()
    {
        GameManager.Instance.MultiplayerTimeTicker += MultiplayerTimer;
        mAIstate = AIState.Idle;
    }

    private void OnValidate()
    {
        for (int i = 0; i < ai_units.Count; i++)
        {
            ai_units[i].AIType = ai_units[i].m_BetAmount.ToString();
        }
    }

    public void StartAI(BetAmount betAmount)
    {
        ScorePerQuestion = GameManager.Instance.ScorePerQuestionMultiplayer;
        ScoreForLastQuestion = GameManager.Instance.ScoreForLastQuestionMultiplayer;

        QuestionMultiplier = GameManager.Instance.MultiplayerQuestionMultiplier;
        LastQuestionMultiplier = GameManager.Instance.MultiplayerLastQuestionMultiplier;

        maxAllowedTime = GameManager.Instance.MultiplayerTotalTime;
        MULTIPLAYER_QUESTION_NUMBER = GameManager.Instance.MultiplayerQuestionsPerRound;

        InitializeAI(betAmount);
        Debug.Log("<color=green>AI STARTED LISTENING</color>");
        mAIstate = AIState.Active;
    }

    private void InitializeAI(BetAmount betAmount)
    {
        AIUnit ai = ai_units.Find(p => (p.m_BetAmount == betAmount));

        mInfo = new List<QuestionAnswerInfo>();

        int correctNum = Random.Range(ai.m_QuestionToAnswer.MinQuestionsToBeAnswered, ai.m_QuestionToAnswer.MaxQuestionsToBeAnswered + 1);

        int TimeToAnswer = 0;

        for (int i = 0; i < MULTIPLAYER_QUESTION_NUMBER; i++)
        {
            QuestionAnswerInfo tInfo = new QuestionAnswerInfo();

            int answerInWithin = Random.Range(ai.MinTimeToAnswer, ai.MaxTimeToAnswer);

            tInfo.AnsweredInTime = answerInWithin;

            TimeToAnswer = (i == 0) ? maxAllowedTime - answerInWithin : TimeToAnswer - answerInWithin;

            tInfo.TimeToAnswerQuestion = TimeToAnswer;//UnityEngine.Random.Range(ai.MinTimeToAnswer, ai.MaxTimeToAnswer);

            mInfo.Add(tInfo);
        }

        FisherYatesShuffle obj = new FisherYatesShuffle(5);
        obj.ShuffleList();
        List<int> correctIndex = obj.ShuffledList;

        for (int i = 0; i < correctNum; i++)
            mInfo[correctIndex[i]].isCorrectAnswer = true;
        
    }

    private void MultiplayerTimer(int timeTick)
    {
        //Debug.Log("MultiplayerTimer tick = "+timeTick);
        if (mAIstate == AIState.Active)
        {
            //Debug.Log("mAIState = "+mAIstate.ToString());
            QuestionAnswerInfo QAinfo = mInfo[CurrentQuestionIndex];

            if (QAinfo.TimeToAnswerQuestion == timeTick)
            {
                //Debug.Log("QAinfo.TimeToAnswerQuestion == timeTick at "+timeTick);

                if (QAinfo.isCorrectAnswer)
                {

                    Debug.Log("QAinfo.isCorrectAnswer");

                    int score = ScorePerQuestion - (QuestionMultiplier * QAinfo.AnsweredInTime);

                    if (CurrentQuestionIndex == 4)
                    {
                        Debug.Log("ScoreForLastQuestion = " + ScoreForLastQuestion);
                        Debug.Log("TimeToAnswerQuestion LAST = " + QAinfo.TimeToAnswerQuestion);
                        Debug.Log("LastQuestionMultiplier = " + LastQuestionMultiplier);
                        score = ScoreForLastQuestion - (LastQuestionMultiplier * QAinfo.AnsweredInTime);
                        Debug.Log("score = " + score);
                    }

                    Debug.Log("Sending Scores");
                    Debug.Log("<color=red>" + score.ToString() + "</color>");
                    GameManager.Instance.MultiplayerOpponentScoreUpdate(score);    

                }
                CurrentQuestionIndex += 1;
            }

        }

        if (timeTick == 0 || CurrentQuestionIndex >= MULTIPLAYER_QUESTION_NUMBER)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (mAIstate == AIState.Active)
        {
            GameManager.Instance.AIGameOver();
        }
        mAIstate = AIState.Idle;
        CurrentQuestionIndex = 0;
    }

}

public enum AIState
{
    Idle,
    Active
}

[System.Serializable]
public class AIUnit
{
    public string AIType;
    public BetAmount m_BetAmount;
    public QuestionToAnswer m_QuestionToAnswer;
    public int MinTimeToAnswer;
    public int MaxTimeToAnswer;
}

[System.Serializable]
public struct QuestionToAnswer
{
    // public bool IsStatic;
    public int MinQuestionsToBeAnswered;
    public int MaxQuestionsToBeAnswered;
}

[System.Serializable]
public class QuestionAnswerInfo
{
    public int TimeToAnswerQuestion;
    public int AnsweredInTime;
    public bool isCorrectAnswer;
}
