using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Popup : MonoBehaviour
{
    public Text PopupText;
    public Text ButtonText;
    public Button Button;
    public GameObject Data;

    public Button Home;

    public void SetPopup(string text, string buttonText, Action OnClick)
    {
        PopupText.text = text;
        ButtonText.text = buttonText;

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() =>
            {
                if (OnClick != null)
                {
                    OnClick.Invoke();
                }
            });
        //Button.onClick.AddListener(() => { ButtonScript.Instance.OnButtonClick((int)buttonId); });
    }
}
