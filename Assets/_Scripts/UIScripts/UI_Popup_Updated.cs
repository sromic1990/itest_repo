using System;
using System.Collections;
using System.Collections.Generic;
using Sourav.Utilities.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Updated : MonoBehaviour 
{
    public Text PopUpText;
    public TypeOfPopUpButtons PopUpType;
    public List<PopUpButtonInfo> Buttons;

    private Action OnYesPressed;
    private Action OnNoPressed;

    private Coroutine HideAfterTime;
    private bool IsCoroutineRunning;

    private void OnValidate()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].ButtonElement = Buttons[i].ButtonType.ToString();
        }
    }

    private void OnDisable()
    {
        if(IsCoroutineRunning)
        {
            StopCoroutine(HideAfterTime);
        }
        ResetPopUp();
    }

    private void ResetPopUp()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].ButtonText.text = Buttons[i].ButtonType.ToString();
        }
    }

    //TODO setup popup
    public void SetupPopUp(string popupText, List<string> ButtonTexts, TypeOfPopUpButtons PopUpButtonType, TypeOfPopUp PopUpType, float Time, Action OnYesPressed, Action OnNoPressed)
    {
        PopUpText.text = popupText;
        if(ButtonTexts != null)
        {
            if(ButtonTexts.Count > 0)
            {
                if(ButtonTexts.Count == 2)
                {
                    for (int i = 0; i < Buttons.Count; i++)
                    {
                        if(Buttons[i].ButtonType == PopUpButtons.Yes || Buttons[i].ButtonType == PopUpButtons.Ok)
                        {
                            Buttons[i].ButtonText.text = ButtonTexts[0];
                        }
                        else if(Buttons[i].ButtonType == PopUpButtons.No)
                        {
                            Buttons[i].ButtonText.text = ButtonTexts[1];
                        }
                    }
                }
                else if(ButtonTexts.Count == 1)
                {
                    for (int i = 0; i < Buttons.Count; i++)
                    {
                        if (Buttons[i].ButtonType == PopUpButtons.Yes || Buttons[i].ButtonType == PopUpButtons.Ok)
                        {
                            Buttons[i].ButtonText.text = ButtonTexts[0];
                        }
                    }
                }
            }
        }
        switch(PopUpButtonType)
        {
            case TypeOfPopUpButtons.YesNo:
                for (int i = 0; i < Buttons.Count; i++)
                {
                    if (Buttons[i].ButtonType == PopUpButtons.Yes || Buttons[i].ButtonType == PopUpButtons.No)
                    {
                        Buttons[i].Button.gameObject.Show();
                        Buttons[i].Button.onClick.RemoveAllListeners();
                        if (Buttons[i].ButtonType == PopUpButtons.Yes)
                        {
                            Buttons[i].Button.onClick.AddListener(YesPressed);
                        }
                        else
                        {
                            Buttons[i].Button.onClick.AddListener(NoPressed);
                        }
                    }
                    else
                    {
                        Buttons[i].Button.gameObject.Hide();
                    }
                }
                break;

            case TypeOfPopUpButtons.Ok:
                for (int i = 0; i < Buttons.Count; i++)
                {
                    if (Buttons[i].ButtonType == PopUpButtons.Ok)
                    {
                        Buttons[i].Button.gameObject.Show();
                        Buttons[i].Button.onClick.RemoveAllListeners();
                        Buttons[i].Button.onClick.AddListener(YesPressed);
                    }
                    else
                    {
                        Buttons[i].Button.gameObject.Hide();
                    }
                }
                break;

            default: //NO BUTTON
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].Button.gameObject.Hide();
                }
                break;
        }
        switch(PopUpType)
        {
            case TypeOfPopUp.Buttoned: // Already showing Buttons
                break;

            case TypeOfPopUp.Evented: // Nothing to do here
                break;

            case TypeOfPopUp.Timed:
                HideAfterTime = StartCoroutine(HidePopUpInTime(Time));
                break;

            case TypeOfPopUp.TimedAndButtoned:
                break;
        }

        this.OnYesPressed = OnYesPressed;
        this.OnNoPressed = OnNoPressed;
    }

    IEnumerator HidePopUpInTime(float time)
    {
        IsCoroutineRunning = true;
        yield return new WaitForSecondsRealtime(time);
        IsCoroutineRunning = false;
        if(this.gameObject.activeSelf)
        {
            HidePopUp();
        }
    }

    public void YesPressed()
    {
        HidePopUp();
        if(OnYesPressed != null)
        {
            OnYesPressed.Invoke();
        }
    }

    public void NoPressed()
    {
        HidePopUp();
        if (OnNoPressed != null)
        {
            OnNoPressed.Invoke();
        }
    }

    private void HidePopUp()
    {
        UIManager.Instance.HidePopUp();
    }
}

public enum TypeOfPopUpButtons
{
    YesNo,
    Ok,
    NoButton
}

public enum PopUpButtons
{
    Yes,
    No,
    Ok
}

[Flags]
public enum TypeOfPopUp
{
    Timed = 1,
    Buttoned = 1<<1,
    Evented = 1<<2,
    TimedAndButtoned = Timed | Buttoned,
    ButtonedAndEvented = Buttoned | Evented,
    TimedAndEvented = Timed | Evented,
    TimedAndButtonedAndEvented = Timed | Buttoned | Evented
}

[Serializable]
public class PopUpButtonInfo
{
    public string ButtonElement;
    public PopUpButtons ButtonType;
    public Button Button;
    public Text ButtonText;

}
