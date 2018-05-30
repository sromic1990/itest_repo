using System.Collections;
using System.Collections.Generic;
using AdManager;
using IdiotTest.Scripts.GameScripts;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : Singleton<ButtonScript>
{
    public void OnButtonClick(int buttonId)
    {
        //Debug.Log("Clicked");
        ButtonID buttonClicked = GetButtonIDFromInt(buttonId);

        switch (buttonClicked)
        {
            case ButtonID.Restart:
                UIManager.Instance.HideQuestionFailed();
                GameManager.Instance.PlayGame();
                break;

            case ButtonID.NextQuestion:
                //Debug.Log("Calling next Question from ButtonScript NextQuestion");
                GameManager.Instance.NextQuestion();
                break;

            case ButtonID.Regular:
                //Debug.Log("Regular");
                GameManager.Instance.OpenGameplay();
                GameDataManager.Instance.CurrentGameMode = GameMode.SinglePlayer;
                GameManager.Instance.PlayGame();
                break;

            case ButtonID.Store_Life:
                UIManager.Instance.Store_SinglePlayer();
                if(!GameDataManager.Instance.IsUnlimitedLives)
                {
                    GameManager.Instance.OpenStore();
                }
                break;

            case ButtonID.Store_Banana:
                UIManager.Instance.Store_MultiPlayer();
                GameManager.Instance.OpenStore();
                break;

            case ButtonID.GetBack5Questions:
                GameManager.Instance.GetBack5Questions();
                break;

            case ButtonID.WatchVideo:
                AdManagerMain.Instance.ShowAds(AdsSDKs.AdMob, AdType.VideoRewardAd, GameManager.Instance.RewardAdFailed, GameManager.Instance.RewardAdWatched, GameManager.Instance.RewardAdSkipped);
                break;

            case ButtonID.HighScore:
                ScreenManager.Instance.SetANewScreen(ScreensEnum.Highscore);
                break;

            case ButtonID.Settings:
                ScreenManager.Instance.SetANewScreen(ScreensEnum.Settings);
                break;

            case ButtonID.Settings_Back:
                ScreenManager.Instance.BackFromScreen();
                //GameManager.Instance.Resume();
                break;

            case ButtonID.Pause:
                ScreenManager.Instance.SetANewScreen(ScreensEnum.Pause);
                GameManager.Instance.Status = GameStatus.InBetweenSession;
                break;

            case ButtonID.Store_Life_Gameplay:
                UIManager.Instance.Store_SinglePlayer();
                ScreenManager.Instance.SetANewScreen(ScreensEnum.Store);
                break;

            case ButtonID.Music_OnOff:
                switch (GameDataManager.Instance.MusicButton)
                {
                    case OnOffButton.On:
                        GameDataManager.Instance.MusicButton = OnOffButton.Off;
                        break;

                    case OnOffButton.Off:
                        GameDataManager.Instance.MusicButton = OnOffButton.On;
                        break;
                }
                break;

            case ButtonID.SFX_OnOff:
                Debug.Log("SFXOnOff");
                switch (GameDataManager.Instance.SFXButton)
                {
                    case OnOffButton.On:
                        GameDataManager.Instance.SFXButton = OnOffButton.Off;
                        break;

                    case OnOffButton.Off:
                        GameDataManager.Instance.SFXButton = OnOffButton.On;
                        break;
                }
                break;

            case ButtonID.Test_OnOff:
                Debug.Log("TestOnOff");
                switch(GameDataManager.Instance.TestButton)
                {
                    case OnOffButton.On:
                        GameDataManager.Instance.TestButton = OnOffButton.Off;
                        break;

                    case OnOffButton.Off:
                        GameDataManager.Instance.TestButton = OnOffButton.On;
                        break;
                }
                break;

            case ButtonID.ResetAttempts:
                GameDataManager.Instance.ResetData(ResetType.Progress);
                break;

            case ButtonID.ReviewApp:
                break;

            case ButtonID.Share:
                GameManager.Instance.ShareRegular();
                break;

            case ButtonID.Home:
                UIManager.Instance.HideQuestionFailed();
                ScreenManager.Instance.SetANewScreen(ScreensEnum.MainMenu);
                GameManager.Instance.Status = GameStatus.OutOfSession;
                if(GameDataManager.Instance.CurrentGameMode == GameMode.Multiplayer && MultiplayerManager.Instance.Status != ConnectionStatus.Disconnected)
                {
                    MultiplayerManager.Instance.Status = ConnectionStatus.Disconnected;
                }
                break;

            case ButtonID.Play:
                GameManager.Instance.OpenGameplay();
                break;

            case ButtonID.Resume:
                bool inBetweenSession = false;
                if(GameManager.Instance.Status == GameStatus.InBetweenSession)
                {
                    inBetweenSession = true;
                }
                GameManager.Instance.OpenGameplay();
                GameManager.Instance.Status = GameStatus.InSession;
                GameManager.Instance.PlayGame(!inBetweenSession);
                break;

            case ButtonID.Continue_From_Login:
                ScreenManager.Instance.SetANewScreen(ScreensEnum.MainMenu);
                break;

            case ButtonID.DontRidiculeMe:
                UIManager.Instance.HideQuestionFailed();
                GameManager.Instance.SkippedQuestion();
                break;

            case ButtonID.HighScore_Back:
                ScreenManager.Instance.BackFromScreen();
                UIManager.Instance.GameModeChanged();
                break;

            case ButtonID.HighScore_MultiplayerTab:
                UIManager.Instance.HighScore_MultiPlayer();
                break;

            case ButtonID.Regular_Achievement:
                UIManager.Instance.Achievements_Singleplayer();
                ScreenManager.Instance.SetANewScreen(ScreensEnum.Achievements);
                break;

            case ButtonID.HighScore_RegularTab:
                UIManager.Instance.HighScore_SinglePlayer();
                break;

            case ButtonID.Multiplayer_Achievement:
                UIManager.Instance.Achievements_Multiplayer();
                ScreenManager.Instance.SetANewScreen(ScreensEnum.Achievements);
                break;

            case ButtonID.Achievement_Singleplayer:
                UIManager.Instance.Achievements_Singleplayer();
                break;
            case ButtonID.Achievement_Multiplayer:
                UIManager.Instance.Achievements_Multiplayer();
                break;

            case ButtonID.Skip_Question:
                GameManager.Instance.SkippedQuestion();
                break;

            case ButtonID.Store_Back:
                ScreenManager.Instance.BackFromScreen();
                UIManager.Instance.GameModeChanged();
                break;

            case ButtonID.Restore:
                break;

            case ButtonID.Store_BananaTab:
                UIManager.Instance.Store_MultiPlayer();
                break;

            case ButtonID.Brain_Pill:
                GameManager.Instance.RemoveAds();
                GameManager.Instance.AddDeductCurrency(Currency.Life, AddDeductAction.Add, 50);
                break;

            case ButtonID.Brain_Operation:
                GameManager.Instance.RemoveAds();
                GameManager.Instance.AddDeductCurrency(Currency.Life, AddDeductAction.Add, 200);
                break;

            case ButtonID.New_Brain:
                GameManager.Instance.RemoveAds();
                GameManager.Instance.AddDeductCurrency(Currency.Life, AddDeductAction.Add, -1);
                break;

            case ButtonID.Store_LifeTab:
                UIManager.Instance.Store_SinglePlayer();
                break;

            case ButtonID.Button_Next:
                break;

            case ButtonID.Button_Prev:
                break;

            case ButtonID.Banana_Plate:
                GameManager.Instance.RemoveAds();
                GameManager.Instance.AddDeductCurrency(Currency.Banana, AddDeductAction.Add, 10);
                break;

            case ButtonID.Banana_Basket:
                GameManager.Instance.RemoveAds();
                GameManager.Instance.AddDeductCurrency(Currency.Banana, AddDeductAction.Add, 50);
                break;

            case ButtonID.Banana_Tree:
                GameManager.Instance.RemoveAds();
                GameManager.Instance.AddDeductCurrency(Currency.Banana, AddDeductAction.Add, 250);
                break;

            case ButtonID.Banana_Farm:
                GameManager.Instance.RemoveAds();
                GameManager.Instance.AddDeductCurrency(Currency.Banana, AddDeductAction.Add, 1000);
                break;

            case ButtonID.Banana_Market:
                GameManager.Instance.RemoveAds();
                GameManager.Instance.AddDeductCurrency(Currency.Banana, AddDeductAction.Add, 5000);
                break;

            case ButtonID.AchievementPopupButton:
                UIManager.Instance.HideAchievement();
                break;

            case ButtonID.Multiplayer:
                Debug.Log("Multiplayer");
                GameManager.Instance.PlayMultiplayer();
                //ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerMode);
                break;

            case ButtonID.BackFromMultiplayer:
                MultiplayerManager.Instance.LeaveRoom(false);
                MultiplayerManager.Instance.Disconnect();
                break;

            case ButtonID.RandomPlay:
                UIManager.Instance.SetMultiplayerUI(MultiplayerMode.FindPlayer);
                ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerGamePanel);
                break;
            case ButtonID.Challenge:
                UIManager.Instance.SetMultiplayerUI(MultiplayerMode.Challenge);
                ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerGamePanel);
                break;
            case ButtonID.AcceptChallenge:
                UIManager.Instance.SetMultiplayerUI(MultiplayerMode.AcceptChallenge);
                ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerGamePanel);
                break;
            case ButtonID.FindPlayer:
                GameManager.Instance.StartFindingMatch();
                break;
            case ButtonID.CancelFindingMatch:
                GameManager.Instance.CancelFindingmatch();
                break;
            case ButtonID.BackWithinMultiplayer:
                GameManager.Instance.GetToMultiplayerPlayMenu();
                break;
            case ButtonID.Rematch:
                MultiplayerManager.Instance.RematchRequest();
                break;
            case ButtonID.ShareFromMultiplayer:
                GameManager.Instance.MultiplayerCodeShared();
                break;
            case ButtonID.JoinGame:
                GameManager.Instance.JoinGame();
                break;
        }
    }

    private ButtonID GetButtonIDFromInt(int buttonId)
    {
        return (ButtonID)buttonId;
    }
}

public enum ButtonID : int
{
    Restart = 0,
    NextQuestion = 1,
    //Main Menu
    Regular = 2,
    Store_Life = 3,
    Store_Banana = 4,
    HighScore = 5,
    Settings = 6,
        //Settings Menu
        Settings_Back = 7,
        Music_OnOff = 8,
        SFX_OnOff = 9,
        ResetAttempts = 10,
        ReviewApp = 11,
        Share = 12,
        //Pause Menu
        Home = 13,
        Play = 14,
        //Login
        Continue_From_Login = 15,
        //Warning
        DontRidiculeMe = 16,
        //HighScore
        HighScore_Back = 17,
        //RegularTab
        HighScore_MultiplayerTab = 18,
        Regular_Achievement = 19,
        //MultiplayerTab
        HighScore_RegularTab = 20,
        Multiplayer_Achievement = 21,
        //Question Failed
        Skip_Question = 22,
        //Store Menu
        Store_Back = 23,
        Restore = 24,
        //Life Store
        Store_BananaTab = 25,
        //Buy
        Brain_Pill = 26,
        Brain_Operation = 27,
        New_Brain = 28,
        //
        //Banana Store
        Store_LifeTab = 29,
        Button_Next = 30,
        Button_Prev = 31,
        //Buy
        Banana_Plate = 32,
        Banana_Basket = 33,
        Banana_Tree = 34,
        Banana_Farm = 35,
        Banana_Market = 36,
        //Gameplay
        Pause = 37,
        Store_Life_Gameplay = 38,
        //Achievement
        AchievementPopupButton = 39,
        //Pause 
        Resume = 40,
    Multiplayer = 41,
    BackFromMultiplayer = 42,
    RandomPlay = 43,
    Challenge = 44,
    AcceptChallenge = 45,
    FindPlayer = 46,
    Rematch = 47,
    CancelFindingMatch = 48,
    ShareFromMultiplayer = 49,
    JoinGame = 50,
    BackWithinMultiplayer = 51,
        Achievement_Singleplayer = 52,
        Achievement_Multiplayer = 53,
        Test_OnOff = 54,
        GetBack5Questions = 55,
        WatchVideo = 56
}