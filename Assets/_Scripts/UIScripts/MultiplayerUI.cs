using System.Collections;
using System.Collections.Generic;
using Sourav.Utilities.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerUI : MonoBehaviour 
{
    public MultiplayerBetSelection BetSelection;
    public GameObject Random_FindPlayer;
    public GameObject Random_FindingPlayer;
    public GameObject Challenge;
    public GameObject Challenge_Accept;
    public Button FindMatch_Button;
    public Button CancelFindMatch_Button;
    public InputField InputField;
    public Button JoinGame_Button;
    public GameObject WaitingForOpponentToJoin_Challenge;
    public Button Back;

    private MultiplayerMode _mode;
    public MultiplayerMode Mode
    {
        get { return _mode; }
        set
        {
            _mode = value;
            OnMultiplayerModeChanged();
        }
    }

    public void OnMultiplayerModeChanged()
    {
        switch(Mode)
        {
            case MultiplayerMode.FindPlayer:
                BetSelection.EnableDisablePrevNextButton(PrevNextButton.both, EnableDisableAction.Enable);
                BetSelection.gameObject.Show();
                Random_FindPlayer.Show();
                Random_FindingPlayer.Hide();
                Challenge.Hide();
                Challenge_Accept.Hide();
                FindMatch_Button.interactable = true;
                FindMatch_Button.gameObject.Show();
                CancelFindMatch_Button.interactable = false;
                CancelFindMatch_Button.gameObject.Hide();
                InputField.gameObject.Hide();
                JoinGame_Button.interactable = false;
                JoinGame_Button.gameObject.Hide();
                WaitingForOpponentToJoin_Challenge.Hide();
                Back.interactable = true;
                break;

            case MultiplayerMode.FindingPlayer:
                BetSelection.gameObject.Show();
                BetSelection.EnableDisablePrevNextButton(PrevNextButton.both, EnableDisableAction.Disable);
                Random_FindPlayer.Hide();
                Random_FindingPlayer.Show();
                Challenge.Hide();
                Challenge_Accept.Hide();
                FindMatch_Button.interactable = false;
                FindMatch_Button.gameObject.Hide();
                CancelFindMatch_Button.interactable = true;
                CancelFindMatch_Button.gameObject.Show();
                InputField.gameObject.Hide();
                JoinGame_Button.interactable = false;
                JoinGame_Button.gameObject.Hide();
                WaitingForOpponentToJoin_Challenge.Hide();
                Back.interactable = false;
                break;

            case MultiplayerMode.Challenge:
                BetSelection.gameObject.Show();
                BetSelection.EnableDisablePrevNextButton(PrevNextButton.both, EnableDisableAction.Enable);
                Random_FindPlayer.Hide();
                Random_FindingPlayer.Hide();
                Challenge.Show();
                Challenge_Accept.Hide();
                FindMatch_Button.interactable = false;
                FindMatch_Button.gameObject.Hide();
                CancelFindMatch_Button.interactable = false;
                CancelFindMatch_Button.gameObject.Hide();
                InputField.gameObject.Hide();
                JoinGame_Button.interactable = false;
                JoinGame_Button.gameObject.Hide();
                WaitingForOpponentToJoin_Challenge.Hide();
                Back.interactable = true;
                break;

            case MultiplayerMode.ChallengeWaitingForPlayer:
                BetSelection.gameObject.Show();
                BetSelection.EnableDisablePrevNextButton(PrevNextButton.both, EnableDisableAction.Disable);
                Random_FindPlayer.Hide();
                Random_FindingPlayer.Hide();
                Challenge.Hide();
                Challenge_Accept.Hide();
                FindMatch_Button.interactable = false;
                FindMatch_Button.gameObject.Hide();
                CancelFindMatch_Button.interactable = true;
                CancelFindMatch_Button.gameObject.Hide();
                InputField.gameObject.Hide();
                JoinGame_Button.interactable = false;
                JoinGame_Button.gameObject.Hide();
                WaitingForOpponentToJoin_Challenge.Show();
                Back.interactable = true;
                break;

            case MultiplayerMode.AcceptChallenge:
                BetSelection.gameObject.Hide();
                BetSelection.EnableDisablePrevNextButton(PrevNextButton.both, EnableDisableAction.Disable);
                Random_FindPlayer.Hide();
                Random_FindingPlayer.Hide();
                Challenge.Hide();
                Challenge_Accept.Show();
                FindMatch_Button.interactable = false;
                FindMatch_Button.gameObject.Hide();
                CancelFindMatch_Button.interactable = false;
                CancelFindMatch_Button.gameObject.Hide();
                InputField.gameObject.Show();
                JoinGame_Button.interactable = true;
                JoinGame_Button.gameObject.Show();
                WaitingForOpponentToJoin_Challenge.Hide();
                Back.interactable = true;
                break;
        }
    }
}

public enum MultiplayerMode
{
    FindPlayer,
    FindingPlayer,
    Challenge,
    ChallengeWaitingForPlayer,
    AcceptChallenge
}
