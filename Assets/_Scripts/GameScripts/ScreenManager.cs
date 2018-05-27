using Scripts.Utilities;
using Sourav.Utilities.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sourav.Utilities.Scripts.DataStructures;

public class ScreenManager : Singleton<ScreenManager>
{
    public List<Screens> Screens;
    [SerializeField]
    private Stack_Sourav<ScreensEnum> ScreenStack;

    public GameObject QuestionPanel;
   

    public ScreensEnum CurrentScreen;
    public ScreensEnum PreviousScreen;

    private void Start()
    {
        ScreenStack = new Stack_Sourav<ScreensEnum>();
        CurrentScreen = ScreensEnum.MainMenu;
        SetANewScreen(ScreensEnum.MainMenu);
    }

    public void SetANewScreen(ScreensEnum Screen)
    {
        Screens screens = GetCorrectScreen(Screen);

        if (!screens.CanGoBackFromScreen)
        {
            PreviousScreen = Screen;
            ScreenStack.ClearStack();
        }
        else
        {
            ScreenStack.Push(CurrentScreen);
            PreviousScreen = CurrentScreen;
        }

        ShowScreen(Screen);
    }

    private void ShowScreen(ScreensEnum Screen, bool isBack = false)
    {
        Screens screens = GetCorrectScreen(Screen);
        if (isBack)
        {
            if (screens.CanGoBackFromScreen)
            {
                if (!ScreenStack.IsStackEmpty())
                {
                    Screen = ScreenStack.Pop();
                    screens = GetCorrectScreen(Screen);
                    Debug.Log("Screen = " + Screen.ToString());
                }
            }
            else
            {
                return;
            }
        }

        HideAllScreens();
        UIManager.Instance.HideIntro();

        CurrentScreen = Screen;

        if (screens.Screen == ScreensEnum.GamePlay)
        {
            //ShowQuestionPanel();
//            GameManager.Instance.Resume();
        }
        else
        {
            GameManager.Instance.Pause();
            //HideQuestionPanel();
        }

        screens.ScreenObj.Show();
    }

    public void BackFromScreen()
    {
        ShowScreen(CurrentScreen, true);
    }

    private Screens GetCorrectScreen(ScreensEnum Screen)
    {
        Screens screen = null;

        for (int i = 0; i < Screens.Count; i++)
        {
            if (Screens[i].Screen == Screen)
            {
                screen = Screens[i];
                break;
            }
        }

        return screen;
    }

    private void HideAllScreens()
    {
        //Debug.Log("Hiding all screens");
        for (int i = 0; i < Screens.Count; i++)
        {
            Screens[i].ScreenObj.Hide();
        }
    }

    public void HideQuestionPanel()
    {
        QuestionPanel.Hide();
    }

    public void ShowQuestionPanel()
    {
        QuestionPanel.Show();
    }

    private void OnValidate()
    {
        for (int i = 0; i < Screens.Count; i++)
        {
            Screens[i].ScreenName = Screens[i].Screen.ToString();
        }
    }
}

[Serializable]
public class Screens
{
    public string ScreenName;
    public ScreensEnum Screen;
    public GameObject ScreenObj;
    public bool IsOverlay;
    public bool CanGoBackFromScreen;
}

[System.Flags]
public enum ScreensEnum
{
    MainMenu = 1,
    Settings = 1 << 1,
    Pause = 1 << 2,
    LevelCleared = 1 << 3,
    Login = 1 << 4,
    GamePlay = 1 << 5,
    FailedCertificate = 1 << 6,
    PassedCertificate = 1 << 7,
    Warning = 1 << 8,
    Highscore = 1 << 9,
    QuestionFailed = 1 << 10,
    Store = 1 << 11,
    Intro = 1 << 12,
    MultiplayerMode = 1 << 13,
    MultiplayerGamePanel = 1 << 14,
    MultiplayerWin = 1 << 15,
    MultiplayerLose = 1 << 16,
    Achievements = 1 << 17
}
