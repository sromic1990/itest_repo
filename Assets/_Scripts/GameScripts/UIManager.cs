using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utilities;
using Sourav.Utilities.Extensions;
using System;
using UnityEngine.UI;
using Sourav.Utilities.Scripts.Attributes;
using IdiotTest.Scripts.GameScripts;

public class UIManager : Singleton<UIManager>
{
    #region Fields

    //Pattern UI holder
    public List<UIPanelHolder> QuestionUIPanels;

    private UI_Base UIPanel;

    private UI_Panel_Base CurrentUI;

    public AnswerButtonHolder LastClickedButton;

    public Text QuestionText;

    public UI_Timer UITimer;
    public UI_Timer IntroTimer;
    public UI_Popup QuestionFailedScreen;
    public UI_Popup_Updated Popup_Updated;

    public Canvas UICanvas;
    public GameObject HighScore_Regular;
    public GameObject HighScore_Multiplayer;
    public GameObject Store_Regular;
    public GameObject Store_Multiplayer;
    public GameObject Achievement_Multiplayer;
    public GameObject Achievement_Regular;
    public GameObject ResumeText_MainMenu;
    public List<GameObject> Music_Buttons_On;
    public List<GameObject> Music_Buttons_Off;
    public List<GameObject> SFX_Buttons_On;
    public List<GameObject> SFX_Buttons_Off;

    public IntroPanel QuestionIntro;

    public GameObject SkipQuestionButton;

    public UI_Base CurrentQuestionPanel;
    public QuestionPanel QuestionPanel;

    public GameObject LevelClearedAnimation;

    public List<GameObject> SinglePlayerElements;
    public List<GameObject> MultiPlayerElements;

    public MultiplayerUI MultiplayerUI;

    public UI_BasePanel BasePanel;

    public InputField MultiplayerJoinGameInput;

    #region Achievement related

    public AchievementPopup AchievementPopup;
    bool isCoroutineRunning;
    public float HideAchievementTime;
    Coroutine Coroutine;

    #endregion

    #region UI Changing objects on Status Change

    public List<Text> Lives;
    public List<Text> Bananas;
    public List<Text> OverallAttempts;
    public List<Text> LivesEarned;
    public List<Text> MultiplayerScoreTexts;
    public List<Text> MultiplayerOpponentScoreTexts;
    public List<Text> MultiplayerTimerTexts;
    public List<Text> WonBananasTexts;
    public List<Text> LostBananasTexts;
    public List<Text> PlayerIDTexts;

    public List<Button> Buttons_ToBe_Enabled_Disabled_On_Room_Occupancy_Change;
    #endregion

    #endregion

    #region Methods

    #region Mono Methods

    private void Start()
    {
        GameManager.Instance.TimeTicker += SetTimerText;
        GameManager.Instance.MultiplayerTimeTicker += SetMultiplayerTimerText;
        SetPlayerID();
    }

    private void OnValidate()
    {
        for (int i = 0; i < QuestionUIPanels.Count; i++)
        {
            QuestionUIPanels[i].PatternString = QuestionUIPanels[i].Pattern.ToString();
        }
    }

    #endregion

    public void SetUI(QPattern pattern, QuestionUIInfo info)
    {
        FindAndSetQuestionPanel(pattern, info);
    }

    #region GamePlay Specific

    #region Question Panel Specific

    private void FindAndSetQuestionPanel(QPattern pattern, QuestionUIInfo info)
    {
        HideAllQuestionPanels();

        UIPanel = null;

        for (int i = 0; i < QuestionUIPanels.Count; i++)
        {
            if (QuestionUIPanels[i].Pattern == pattern)
            {
                UIPanel = QuestionUIPanels[i].Panel;
                break;
            }
        }
        if (UIPanel != null)
        {
            CurrentQuestionPanel = UIPanel;
            UIPanel.gameObject.Show();
            UIPanel.SetUI(info);
        }
    }

    public void HideAllQuestionPanels()
    {
        for (int i = 0; i < QuestionUIPanels.Count; i++)
        {
            QuestionUIPanels[i].Panel.gameObject.Hide();
        }
    }

    #endregion

    #region GamePlay

    public void ShowGameplay(bool isReset = true)
    {
        if (isReset)
        {
            if (CurrentQuestionPanel != null)
            {
                CurrentQuestionPanel.Reset();
            }
        }

        ShowHideElementsOfGameplayAccordingToMode();

        BaseQuestion currentQuestion = GameManager.Instance.GetCurrentQuestion();

        QuestionText.text = currentQuestion.Question;

        if (currentQuestion.HasTimerValue)
        {
            UITimer.SetTimer(Mathf.RoundToInt(GameManager.Instance.GetCurrentQuestion(true).TimeForQuestion));
            ShowHideGameObject(UITimer.gameObject, ShowHideAction.Show);
        }
        else
        {
            ShowHideGameObject(UITimer.gameObject, ShowHideAction.Hide);
        }

    }

    private void ShowHideElementsOfGameplayAccordingToMode()
    {
        switch (GameDataManager.Instance.CurrentGameMode)
        {
            case GameMode.Multiplayer:
                ShowHideGameObjectList(MultiPlayerElements, ShowHideAction.Show);
                ShowHideGameObjectList(SinglePlayerElements, ShowHideAction.Hide);
                break;

            case GameMode.SinglePlayer:
                ShowHideGameObjectList(SinglePlayerElements, ShowHideAction.Show);
                ShowHideGameObjectList(MultiPlayerElements, ShowHideAction.Hide);
                break;
        }
    }

    public void ShowSecondaryQuestion()
    {
        BaseQuestion question = GameManager.Instance.GetCurrentQuestion();

        if (question.SecondaryQuestion.Count == 0)
            return;

        QuestionText.text = question.SecondaryQuestion[0];
    }

    public void OnGameOutOfSession()
    {
        QuestionPanel.ResetAllPanels();
        ScreenManager.Instance.HideQuestionPanel();
    }

    public void LevelCompleted()
    {
        Debug.Log("Level completed");
        HideQuestionPanel();
        LevelClearedAnimation.Show();
    }

    #endregion

    #region Intro Screen Specific

    public void ShowIntroScreen(string introText, int introTime, bool showOkButton, bool hasTimerValue)
    {
        //QuestionText.text = introText;

        ShowHideElementsOfGameplayAccordingToMode();

        QuestionIntro.SetUpIntro(introText, introTime, showOkButton, hasTimerValue);
        QuestionIntro.gameObject.Show();
    }

    public void HideIntro()
    {
        QuestionIntro.gameObject.Hide();
    }

    #endregion

    #region Timer Specific

    public void ResetTimer()
    {
        SetTimerText(0);
    }

    private void SetTimerText(int time)
    {
        UITimer.SetTime(time);
    }

    #endregion

    public void CheckForEffectsOnButtonClick()
    {
        if (LastClickedButton == null)
            return;

        if (LastClickedButton.mID == AnswerID.Balloon_Blue || LastClickedButton.mID == AnswerID.Balloon_Green || LastClickedButton.mID == AnswerID.Balloon_Yellow || LastClickedButton.mID == AnswerID.Balloon_Red)
        {
            BurstBalloon(LastClickedButton.mID, LastClickedButton.gameObject.transform.position);
        }
    }

    #region Multiplayer related
    public void SetMultiplayerUI(MultiplayerMode mode)
    {
        MultiplayerUI.Mode = mode;
    }

    private void SetMultiplayerTimerText(int timer)
    {
        //Debug.Log("Multiplayer Timer = "+timer);

        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        string time = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        for (int i = 0; i < MultiplayerTimerTexts.Count; i++)
        {
            MultiplayerTimerTexts[i].text = time;
        }
    }
    #endregion
    #endregion

    #region Answer Specific

    public void QuestionAnswered()
    {
        if (UIPanel != null)
        {
            UIPanel.QuestionAnswered();
        }
    }

    public void AnswerButtonClicked()
    {
        if (LastClickedButton != null)
        {
            //LastClickedButton.ResetClickCounter();
            LastClickedButton.ChangeButtonImageToClickedImage();
        }
    }

    public void ShowingAnswerResult()
    {
        ShowHideGameObject(UITimer.gameObject, ShowHideAction.Hide);
    }

    #endregion

    #region Popup Related

    public void ShowPopUp(string popupText, List<Text> ButtonTexts, TypeOfPopUpButtons PopUpButtonType, TypeOfPopUp PopUpType, float Time, Action OnYesPressed, Action OnNoPressed)
    {
        Popup_Updated.SetupPopUp(popupText, ButtonTexts, PopUpButtonType, PopUpType, Time, OnYesPressed, OnNoPressed);
        Popup_Updated.gameObject.Show();
    }

    public void HidePopUp()
    {
        Popup_Updated.gameObject.Hide();
    }

    #endregion

    #region Animation related

    private void BurstBalloon(AnswerID balloonType, Vector3 ButtonPosition)
    {
    }

    public void CompletedMonkeyLevelAnimation()
    {
        LevelClearedAnimation.Hide();
        ScreenManager.Instance.SetANewScreen(ScreensEnum.LevelCleared);
    }

    #endregion

    #region Answered Question related

    public void ShowFailedCertificate()
    {
        HideQuestionPanel();
        ScreenManager.Instance.SetANewScreen(ScreensEnum.FailedCertificate);
    }

    public void ShowWarning()
    {
        HideQuestionPanel();
        ScreenManager.Instance.SetANewScreen(ScreensEnum.Warning);
    }

    public void AnsweredWrong()
    {
        HideQuestionPanel();
        CheckForEffectsOnButtonClick();
        ShowQuestionFailed("Wrong Answer", "Restart", ButtonID.Restart);
    }

    private void HideQuestionPanel()
    {
        CurrentQuestionPanel.gameObject.Hide();
    }

    public void AnsweredCorrect()
    {
        CheckForEffectsOnButtonClick();
        // Debug.Log("Correct alert");
        //ShowPopup("Correct Answer", "Next", ButtonID.NextQuestion);
    }

    public void HideQuestionFailed()
    {
        QuestionFailedScreen.gameObject.Hide();
    }

    public void ShowQuestionFailed(string text, string buttonText, ButtonID buttonId)
    {
        QuestionFailedScreen.SetPopup(text, buttonText, () =>
            {
                ButtonScript.Instance.OnButtonClick((int)buttonId);
            });
        QuestionFailedScreen.gameObject.Show();
    }

    #endregion

    #region DATA for UI Changed

    public void GameModeChanged()
    {
        BasePanel.GameModeChanged(GameDataManager.Instance.CurrentGameMode);
        switch (GameDataManager.Instance.CurrentGameMode)
        {
            case GameMode.SinglePlayer:
                HighScore_SinglePlayer();
                Store_SinglePlayer();
                break;

            case GameMode.Multiplayer:
                HighScore_MultiPlayer();
                Store_MultiPlayer();
                break;
        }
    }

    public void Store_MultiPlayer()
    {
        Store_Regular.Hide();
        Store_Multiplayer.Show();
    }

    public void Store_SinglePlayer()
    {
        Store_Regular.Show();
        Store_Multiplayer.Hide();
    }

    public void HighScore_MultiPlayer()
    {
        HighScore_Regular.Hide();
        HighScore_Multiplayer.Show();
    }

    public void HighScore_SinglePlayer()
    {
        if (HighScore_Regular != null && HighScore_Multiplayer != null)
        {
            HighScore_Regular.Show();
            HighScore_Multiplayer.Hide();

        }
    }

    public void Achievements_Multiplayer()
    {
        Achievement_Multiplayer.Show();
        Achievement_Regular.Hide();
    }

    public void Achievements_Singleplayer()
    {
        Achievement_Multiplayer.Hide();
        Achievement_Regular.Show();
    }

    public void MusicStateChanged()
    {
        switch (GameDataManager.Instance.MusicButton)
        {
            case OnOffButton.On:
                for (int i = 0; i < Music_Buttons_On.Count; i++)
                {
                    Music_Buttons_On[i].Show();
                }
                for (int i = 0; i < Music_Buttons_Off.Count; i++)
                {
                    Music_Buttons_Off[i].Hide();
                }
                break;

            case OnOffButton.Off:
                for (int i = 0; i < Music_Buttons_On.Count; i++)
                {
                    Music_Buttons_On[i].Hide();
                }
                for (int i = 0; i < Music_Buttons_Off.Count; i++)
                {
                    Music_Buttons_Off[i].Show();
                }
                break;
        }
    }

    public void SFXStateChanged()
    {
        switch (GameDataManager.Instance.SFXButton)
        {
            case OnOffButton.On:
                for (int i = 0; i < SFX_Buttons_On.Count; i++)
                {
                    SFX_Buttons_On[i].Show();
                }
                for (int i = 0; i < SFX_Buttons_Off.Count; i++)
                {
                    SFX_Buttons_Off[i].Hide();
                }
                break;

            case OnOffButton.Off:
                for (int i = 0; i < SFX_Buttons_On.Count; i++)
                {
                    SFX_Buttons_On[i].Hide();
                }
                for (int i = 0; i < SFX_Buttons_Off.Count; i++)
                {
                    SFX_Buttons_Off[i].Show();
                }
                break;
        }
    }

    public void LivesChanged()
    {
        for (int i = 0; i < Lives.Count; i++)
        {
            Lives[i].text = GameDataManager.Instance.TotalLives.ToString();
        }
    }

    public void BananasChanged()
    {
        for (int i = 0; i < Bananas.Count; i++)
        {
            Bananas[i].text = GameDataManager.Instance.TotalBananas.ToString();
        }
    }

    public void OverallAttemptsChanged()
    {
        for (int i = 0; i < OverallAttempts.Count; i++)
        {
            OverallAttempts[i].text = GameDataManager.Instance.OverallAttemptsTaken.ToString();
        }
    }

    public void LivesEarnedChanged()
    {
        for (int i = 0; i < LivesEarned.Count; i++)
        {
            LivesEarned[i].text = GameDataManager.Instance.LivesEarned.ToString();
        }
    }

    public void SingleplayerStatusChanged()
    {
        if (ResumeText_MainMenu == null)
            return;

        switch (GameDataManager.Instance.SinglePlayerStatus)
        {
            case SinglePlayerStatus.NewGame:
                ResumeText_MainMenu.Hide();
                break;

            case SinglePlayerStatus.ResumeGame:
                ResumeText_MainMenu.Show();
                break;
        }
    }

    public void SetPlayerID()
    {
        for (int i = 0; i < PlayerIDTexts.Count; i++)
        {
            PlayerIDTexts[i].text = GameDataManager.Instance.PlayerID;
        }
    }

    #endregion

    #region Achievements related

    public void ShowAchievement(Sprite achievementImage, string achievementTitle)
    {
        AchievementPopup.Setup_AchievementPopup(achievementImage, achievementTitle);
        AchievementPopup.gameObject.Show();

        Coroutine = StartCoroutine(HideAchievementAfter());
    }

    IEnumerator HideAchievementAfter()
    {
        isCoroutineRunning = true;
        yield return new WaitForSecondsRealtime(HideAchievementTime);
        isCoroutineRunning = false;
        HideAchievement();
    }

    public void HideAchievement()
    {
        AchievementPopup.gameObject.Hide();

        if (isCoroutineRunning)
        {
            StopCoroutine(Coroutine);
        }

    }

    #endregion

    #region Multiplayer UI related
    public void Multiplayer_ScoreChanged(int score)
    {
        for (int i = 0; i < MultiplayerScoreTexts.Count; i++)
        {
            MultiplayerScoreTexts[i].text = score.ToString();
        }
    }
    public void Multiplayer_OpponentScoreChanged(int score)
    {
        for (int i = 0; i < MultiplayerOpponentScoreTexts.Count; i++)
        {
            MultiplayerOpponentScoreTexts[i].text = score.ToString();
        }
    }
    public void MultiplayerWin()
    {
        HideQuestionPanel();
        ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerWin);
    }

    public void MultiplayerLose()
    {
        HideQuestionPanel();
        ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerLose);
    }

    public void MultiplayerBananasWon(int bananas)
    {
        for (int i = 0; i < WonBananasTexts.Count; i++)
        {
            WonBananasTexts[i].text = bananas.ToString();
        }
    }

    public void MultiplayerBananasLost(int bananas)
    {
        for (int i = 0; i < LostBananasTexts.Count; i++)
        {
            LostBananasTexts[i].text = bananas.ToString();
        }
    }

    public void RoomOccupancyChanged(bool isAlone)
    {
        for (int i = 0; i < Buttons_ToBe_Enabled_Disabled_On_Room_Occupancy_Change.Count; i++)
        {
            Buttons_ToBe_Enabled_Disabled_On_Room_Occupancy_Change[i].interactable = !isAlone;
        }
    }
    #endregion

    #region Utilities

    public void ShowHideGameObjectList(List<GameObject> list, ShowHideAction Action)
    {
        switch (Action)
        {
            case ShowHideAction.Show:
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Show();
                }
                break;

            case ShowHideAction.Hide:
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Hide();
                }
                break;
        }
    }

    public void ShowHideGameObject(GameObject gOBJ, ShowHideAction showHide)
    {
        switch (showHide)
        {
            case ShowHideAction.Show:
                gOBJ.Show();
                break;

            case ShowHideAction.Hide:
                gOBJ.Hide();
                break;
        }
    }

    #endregion

    #endregion
}

[Serializable]
public class UIPanelHolder
{
    [ReadOnly]
    public string PatternString;
    public QPattern Pattern;
    public UI_Base Panel;
}
