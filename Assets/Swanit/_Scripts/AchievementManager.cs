using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utilities;
using IdiotTest.Scripts.GameScripts;

public class AchievementManager : Singleton<AchievementManager>
{
    public List<AchievementData> mAchievements;

    #region Mono Methods
    void Start()
    {
        // AddDeductCurrency(Currency type, AddDeductAction action, int amount)
    }

    void OnValidate()
    {
        for (int i = 0; i < mAchievements.Count; i++)
        {
            mAchievements[i].AchievementTag = mAchievements[i].Type.ToString() + "_" + mAchievements[i].AchievementName.ToString();
        }
    }
    #endregion

    #region Local Achievements

    public void OnStreakChanged_Regular(int streak)
    {
        AchievementUnlocked(streak, AchievementType.Singleplayer_WinStreak);
    }

    public void OnAnsweredCorrectly(int totalCorrectlyAnswered)
    {
        AchievementUnlocked(totalCorrectlyAnswered, AchievementType.TotalCorrectAnswers);
    }

    public void OnQuestionPassed(int totalQuestionPassed)
    {
        AchievementUnlocked(totalQuestionPassed, AchievementType.TotalQuestionsPassed);
    }

    public void OnLivesChanged(int totalLives)
    {
        AchievementUnlocked(totalLives, AchievementType.TotalLivesOwned);
    }

    #endregion

    #region Multiplayer Achievements

    public void OnStreakChanged_Multiplayer(int streak)
    {
        AchievementUnlocked(streak, AchievementType.Multiplayer_WinStreak);
    }

    public void OnLoseStreakChanged_Multiplayer(int Lstreak)
    {
        AchievementUnlocked(Lstreak, AchievementType.Multiplayer_LoseStreak);
    }

    public void OnBananasChanged(int totalBananas)
    {
        AchievementUnlocked(totalBananas, AchievementType.TotalBananasOwned);
    }
    #endregion

    #region Utilities
    private void AchievementUnlocked(int value, AchievementType type)
    {
        AchievementData aData = new AchievementData();

        aData = mAchievements.Find(a => (a.Value == value && a.Type == type));

        if (aData == null)
            return;

        if (!GameDataManager.Instance.AchievementsContains(aData.AchievementName))
        {
            GameDataManager.Instance.AddAchievement(aData.AchievementName);
            Debug.Log("Achievement Unlocked  :::  " + aData.AchievementName.ToString());

            GameManager.Instance.AddDeductCurrency(aData.CurrencyType, AddDeductAction.Add, aData.Reward);
            string achievementName = GetCorrectAchievementName(aData.AchievementName);
            UIManager.Instance.ShowAchievement(aData.AchievemnetImg, achievementName);
        }

    }

    private string GetCorrectAchievementName(Achievement achievement)
    {
        switch(achievement)
        {
            case Achievement.GoodGoing:
                return "Good Going";

            case Achievement.IsThatAll:
                return "Is That All";

            case Achievement.JustGiveUp:
                return "Just Give Up";

            case Achievement.JustMissed:
                return "Just Missed";

            case Achievement.KeepItUp:
                return "Keep it Up";

            case Achievement.Opportunist:
                return "Opportunist";

            case Achievement.PeckyHead:
                return "Pecky Head";

            case Achievement.RiseAndShine:
                return "Rise And Shine";

            case Achievement.RookieStudent:
                return "Rookie Student";

            case Achievement.Struggler:
                return "Struggler";

            case Achievement.ThatsTheSpirit:
                return "Thats the Spirit";

            case Achievement.TinyBrains:
                return "Tiny Brains";

            case Achievement.ToughGetsGoing:
                return "Tough Gets Going";

            case Achievement.TrickPrick:
                return "Trick Prick";

            case Achievement.VictoryInName:
                return "Victory In Name";

            default: //YouRock
                return "You Rock";
        }
        
    }
    #endregion

}

[System.Serializable]
public class AchievementData
{
    public string AchievementTag;
    public Sprite AchievemnetImg;
    public Achievement AchievementName;
    public AchievementType Type;
    public int Value;
    public Currency CurrencyType;
    public int Reward;

};

public enum AchievementType
{
    Singleplayer_WinStreak,
    Multiplayer_WinStreak,
    Multiplayer_LoseStreak,
    TotalCorrectAnswers,
    TotalQuestionsPassed,
    TotalLivesOwned,
    TotalBananasOwned,
    TotalDoubleScoreLose,
    TotlDoubleScoreWin
}