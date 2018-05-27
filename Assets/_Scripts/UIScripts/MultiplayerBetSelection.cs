using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using IdiotTest.Scripts.GameScripts;

public class MultiplayerBetSelection : MonoBehaviour
{
    #region Fields
    public GameObject Holder;

    public List<BetListStatus> BetList;
    public List<GameObject> BetElements;

    public Button PrevButton;
    public Button NextButton;

    private BetAmount currentBetAmount;
    public BetAmount CurrentBetAmount
    {
        get { return currentBetAmount; }
        set
        {
            currentBetAmount = value;
            BetAmountChanged();
        }
    }
    #endregion

    #region Methods
    #region Mono Methods
    private void OnEnable()
    {
        CurrentBetAmount = GameDataManager.Instance.BetAmount;
    }
    #endregion

    #region Private Methods
    private void BetAmountChanged()
    {
        if (!PrevButton.IsInteractable() && !NextButton.IsInteractable())
            return;

        int bet = GetCurrentCurrentBet(CurrentBetAmount);
        if (bet != -1)
        {
            #region Deal with Prev Next Buttons
            if (bet == 0)
            {
                EnableDisablePrevNextButton(PrevNextButton.prev, EnableDisableAction.Disable);
            }
            else
            {
                EnableDisablePrevNextButton(PrevNextButton.prev, EnableDisableAction.Enable);
            }

            if (bet == BetList.Count - 1)
            {
                EnableDisablePrevNextButton(PrevNextButton.next, EnableDisableAction.Disable);
            }
            else
            {
                EnableDisablePrevNextButton(PrevNextButton.next, EnableDisableAction.Enable);
            }
            #endregion

            Holder.transform.localPosition = new Vector3(BetList[bet].HolderPos, Holder.transform.localPosition.y, Holder.transform.localPosition.z);

            ResetAllBetElements();
            BetList[bet].BetToEnlarge.transform.localScale = new Vector3(1.25f, 1.25f, 1);
            GameDataManager.Instance.BetAmount = BetList[bet].BetAmountChosen;

        }
    }

    private int GetCurrentCurrentBet(BetAmount betAmount)
    {
        int bet = -1;

        for (int i = 0; i < BetList.Count; i++)
        {
            if (BetList[i].BetAmountChosen == betAmount)
            {
                bet = i;
                break;
            }
        }

        return bet;
    }

    private void ResetAllBetElements()
    {
        for (int i = 0; i < BetElements.Count; i++)
        {
            BetElements[i].gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void IncreaseDecreaseBet(IncreaseDecreaseAction Action)
    {
        switch (CurrentBetAmount)
        {
            case BetAmount.One:
                switch (Action)
                {
                    case IncreaseDecreaseAction.Decrese:
                        break;

                    case IncreaseDecreaseAction.Increse:
                        CurrentBetAmount = BetAmount.Five;
                        break;
                }
                break;

            case BetAmount.Five:
                switch (Action)
                {
                    case IncreaseDecreaseAction.Decrese:
                        CurrentBetAmount = BetAmount.One;
                        break;

                    case IncreaseDecreaseAction.Increse:
                        CurrentBetAmount = BetAmount.Fifty;
                        break;
                }
                break;

            case BetAmount.Fifty:
                switch (Action)
                {
                    case IncreaseDecreaseAction.Decrese:
                        CurrentBetAmount = BetAmount.Five;
                        break;

                    case IncreaseDecreaseAction.Increse:
                        CurrentBetAmount = BetAmount.Hundred;
                        break;
                }
                break;

            case BetAmount.Hundred:
                switch (Action)
                {
                    case IncreaseDecreaseAction.Decrese:
                        CurrentBetAmount = BetAmount.Fifty;
                        break;

                    case IncreaseDecreaseAction.Increse:
                        CurrentBetAmount = BetAmount.FiveHundred;
                        break;
                }
                break;

            case BetAmount.FiveHundred:
                switch (Action)
                {
                    case IncreaseDecreaseAction.Decrese:
                        CurrentBetAmount = BetAmount.Hundred;
                        break;

                    case IncreaseDecreaseAction.Increse:
                        CurrentBetAmount = BetAmount.OneThousand;
                        break;
                }
                break;

            case BetAmount.OneThousand:
                switch (Action)
                {
                    case IncreaseDecreaseAction.Decrese:
                        CurrentBetAmount = BetAmount.FiveHundred;
                        break;

                    case IncreaseDecreaseAction.Increse:
                        break;
                }
                break;
        }
    }
    #endregion

    #region Public Methods
    public void PrevNextButtonPressed(int PrevNext)
    {
        PrevNextButton button = (PrevNextButton)PrevNext;
        switch (button)
        {
            case PrevNextButton.next:
                IncreaseDecreaseBet(IncreaseDecreaseAction.Increse);
                break;

            case PrevNextButton.prev:
                IncreaseDecreaseBet(IncreaseDecreaseAction.Decrese);
                break;
        }
    }

    public void EnableDisablePrevNextButton(PrevNextButton Button, EnableDisableAction Action)
    {
        switch (Button)
        {
            case PrevNextButton.prev:
                switch (Action)
                {
                    case EnableDisableAction.Enable:
                        PrevButton.interactable = true;
                        break;

                    case EnableDisableAction.Disable:
                        PrevButton.interactable = false;
                        break;
                }
                break;

            case PrevNextButton.next:
                switch (Action)
                {
                    case EnableDisableAction.Enable:
                        NextButton.interactable = true;
                        break;

                    case EnableDisableAction.Disable:
                        NextButton.interactable = false;
                        break;
                }
                break;

            case PrevNextButton.both:
                switch (Action)
                {
                    case EnableDisableAction.Enable:
                        PrevButton.interactable = true;
                        NextButton.interactable = true;
                        break;

                    case EnableDisableAction.Disable:
                        PrevButton.interactable = false;
                        NextButton.interactable = false;
                        break;
                }
                break;
        }

    }
    #endregion
    #endregion
}

[Serializable]
public struct BetListStatus
{
    public int CurrentStatus;
    public BetAmount BetAmountChosen;
    public float HolderPos;
    public GameObject BetToEnlarge;
}

public enum PrevNextButton
{
    prev = 0,
    next = 1,
    both = 2
}

public enum IncreaseDecreaseAction
{
    Increse,
    Decrese
}

public enum EnableDisableAction
{
    Enable,
    Disable
}
